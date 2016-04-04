using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	//Refactored stringbuilders to strings and extracted long code runs into separate methods. JMM Feb 2016
	public partial class MainForm : Form
	{
		#region Constructor

		public MainForm()
		{
			if (Program.commandLineArgs != null && Program.commandLineArgs.Library != null && !string.IsNullOrEmpty(Program.commandLineArgs.Library))

			{
				InitializeFormAndData();
			}
			else
			{
				ShowUpdateMessage();
			}
		}

		public MainForm(string[] newArgs)
		{
			if (Program.commandLineArgs != null && Program.commandLineArgs.Library != null && !string.IsNullOrEmpty(Program.commandLineArgs.Library))

			{
				InitializeFormAndData();
			}
			else
			{
				ShowUpdateMessage();
			}
		}

		private void InitializeFormAndData()
		{
			InitializeComponent();
			timer.Start();
			splash = ShowSpashScreen();
			splash.UpdateProgress(10);
			Application.DoEvents();
			_isMinimized = false;
			splash.UpdateProgress(20);
			Application.DoEvents();

			DBAccessManager connMgr = null;
			splash.UpdateProgress(30);
			Application.DoEvents();
			ParseArgsToProperties(Program.commandLineArgs);
			splash.UpdateProgress(45);
			Application.DoEvents();

			EstablishCamraConnection();
			splash.UpdateProgress(65);
			Application.DoEvents();
			SetConnectionLibraryParameters(connMgr);
			splash.UpdateProgress(85);
			Application.DoEvents();
			CurrentSMParcel = LoadSMParcelObjects();
			WorkingSMParcel = CurrentSMParcel;
			splash.UpdateProgress(100);
			Application.DoEvents();
		}

		private SMParcel LoadSMParcelObjects()
		{
			SketchRepository sketchRepo = new SketchRepository(dbConnection.DataSource, dbConnection.User, dbConnection.Password, dbConnection.LocalityPrefix);

			SMParcel parcel = sketchRepo.SelectParcelData(Record, Card);
			parcel.Sections = sketchRepo.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sketchRepo.SelectSectionLines(sms);
			}
			parcel.IdentifyAttachedToSections();
			return parcel;
		}

		private void UpdateProgressBar(object sender, ElapsedEventArgs e)
		{
			var reportProgress = new Action(() =>
			{
				// inside this anonymous delegate, we can do all the UI updates
				splash.UpdateProgress();
			});
			Invoke(reportProgress);
		}

		#endregion Constructor

		#region fields

		public static int Card = 0;
		public static int chekr = 0;
		private SMParcel currentSMParcel;
		private CAMRA_Connection dbConnection = null;
		public static int FCcard = 0;
		public static string FCipAddress = String.Empty;
		public static string FClib = String.Empty;
		public static string FCprefix = String.Empty;
		public static int FCrecord = 0;
		public static int InitalCard = 0;
		public static int InitalRec = 0;
		public string IPAddress = String.Empty;
		public static string localDescription = string.Empty;
		public static string localLib = String.Empty;
		public static string localPrefix = String.Empty;
		public static int Record = 0;
		private FormSplash splash;
		private SMParcel workingSMParcel;
		private ParcelData _currentParcel = null;
		private DBAccessManager _db = null;
		private bool _hasNewSketch = false;
		public static bool _hasSketch = false;
		public bool _isMinimized = false;
		public bool _mainClosed = false;
		private string _selectedPath = String.Empty;
		private string _selectedPicPath = String.Empty;
		private string _selectedSktPath = String.Empty;
		private SectionDataCollection _subSections = null;

		public Image CurrentSketch
		{
			get; set;
		}


		public static int Month
		{
			get; set;
		}

		public static string reopenSec
		{
			get; set;
		}

		private string SketchFolder
		{
			get; set;
		}

		public Image sketchImage
		{
			get; set;
		}

		public static int Today
		{
			get; set;
		}

		public static int Year
		{
			get; set;
		}

		public DateTime _today
		{
			get; set;
		}

		public SMParcel CurrentSMParcel
		{
			get
			{
				return currentSMParcel;
			}

			set
			{
				currentSMParcel = value;
			}
		}

		public SMParcel WorkingSMParcel
		{
			get
			{
				return workingSMParcel;
			}

			set
			{
				workingSMParcel = value;
			}
		}

		#endregion fields

		#region Form Control Methods

		private void EditImage_Click(object sender, EventArgs e)
		{
			//_currentParcel = ParcelData.getParcel(_conn, record, card);
			//_subSections = new SectionDataCollection(_conn, record, card);
			try
			{
				GetSelectedImages();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, MethodBase.GetCurrentMethod().Module.Name);
				Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
				throw;
			}
		}

		private void GetSelectedImages()
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			try
			{
				_subSections = new SectionDataCollection(dbConnection, _currentParcel.mrecno, _currentParcel.mdwell);

				_currentParcel.BuildSketchData();
				getSketch(_currentParcel.Record, _currentParcel.Card);

				//Ask Dave why this happens twice
				CurrentSketch = _currentParcel.GetSketchImage(374);
				sketchBox.Image = CurrentSketch;

				if (EditImage.Text == "Add Sketch")
				{
					EditImage.Text = "Edit Sketch";
				}

				UtilityMethods.DeleteXLines(dbConnection, _currentParcel.Record, _currentParcel.Card, FClib, FCprefix);

				int SectionCnt = UtilityMethods.SectionCount(dbConnection, _currentParcel.Record, _currentParcel.Card, FClib, FCprefix);

				bool omitSketch = false;

				if (SectionCnt == 0)
				{
					DialogResult result;
					result = (MessageBox.Show("Add Sketch ?", "Sketch Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question));

					if (result == DialogResult.Yes)
					{
						//BuildNewSketch();

						EditImage.Text = "Edit Sketch";

						_hasNewSketch = true;
					}
					if (result == DialogResult.No)
					{
						omitSketch = true;
					}
				}
				SetTextAddOrEdit(SectionCnt);

				try
				{
					if (omitSketch == false && ExpandoSketch._cantSketch == false)
					{
						ExpandoSketch expandoSketchForm = new ExpandoSketch(_currentParcel, SketchFolder, _currentParcel.mrecno.ToString(), _currentParcel.mdwell.ToString(),
						   MainForm.FCprefix, dbConnection, _subSections, _hasSketch, sketchImage, _hasNewSketch);

						expandoSketchForm.ShowDialog(this);

						if (ExpandoSketch._isClosed == false && ExpandoSketch._deleteThisSketch == true)
						{
							CleanUpSketch();
						}

						int record = _currentParcel.mrecno;
						int card = _currentParcel.mdwell;

						_currentParcel = null;
						_subSections = null;

						_currentParcel = ParcelData.getParcel(dbConnection, record, card);
						_subSections = new SectionDataCollection(dbConnection, record, card);

						_currentParcel.BuildSketchData();

						getSketch(_currentParcel.Record, _currentParcel.Card);
						CurrentSketch = _currentParcel.GetSketchImage(374);
						sketchBox.Image = CurrentSketch;
					}
				}
				catch (Exception ex)
				{
					string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
					Trace.WriteLine(errMessage);
					Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

					MessageBox.Show(errMessage);
#endif
					throw;
				}

				if (ExpandoSketch._deleteMaster == true)
				{
					EditImage.Text = "Add Sketch";
				}
			}
			catch (Exception ex)
			{
				string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
				Trace.WriteLine(errMessage);
				Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

				MessageBox.Show(errMessage);
#endif
				throw;
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//Program.ShowCheckpointLog();
			Application.Exit();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Application.DoEvents();
		}

		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				notifyIcon1.Icon = SystemIcons.Application;
				notifyIcon1.BalloonTipText = "Main Form Minimized";
				notifyIcon1.ShowBalloonTip(10000);
			}
			else if (this.WindowState == FormWindowState.Normal)
			{
				notifyIcon1.BalloonTipText = "Main Form is back to Normal";
				notifyIcon1.ShowBalloonTip(10000);
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
		}

		//TODO: Start here Thursday Feb 11
		private void SelectRecordBtn_Click(object sender, EventArgs e)
		{
			if (Record == 0)
			{
				MessageBox.Show("Must Enter Record No.");
				RecordTxt.Focus();
			}
			if (Record != 0 && Card == 0)
			{
				Card = 1;
			}

			if (Record != InitalRec)
			{
				MessageBox.Show(String.Format("Now Working on New Record - {0}  Old Record was - {1}", Record, InitalRec));

				InitalRec = Record;
				InitalCard = Card;

				CheckGoodRecord(Record, Card);

				int tstchkr = chekr;

				if (chekr > 0)
				{
					AddSketchToParcel();
				}
			}
		}

		#endregion Form Control Methods

		#region Private Methods

		private void AddSketchToParcel()
		{
			_currentParcel = ParcelData.getParcel(dbConnection, Record, Card);

			_subSections = new SectionDataCollection(dbConnection, Record, Card);

			if (!CamraSupport.VacantOccupancies.Contains(_currentParcel.moccup))
			{
				SetTextAddOrEdit(_subSections.Count);
			}
			if (CamraSupport.VacantOccupancies.Contains(_currentParcel.moccup))
			{
				MessageBox.Show("Can't Add Sketch to Vacant Parcel...Add Master Record Data!");
				this.WindowState = FormWindowState.Minimized;
			}

			try
			{
				getSketch(_currentParcel.mrecno, _currentParcel.mdwell);
			}
			catch
			{
			}

			CurrentSketch = _currentParcel.GetSketchImage(374);
			sketchBox.Image = CurrentSketch;
		}

		private void CardTxt_Leave(object sender, EventArgs e)
		{
			int card = 0;
			int.TryParse(CardTxt.Text, out card);

			Card = card;
		}

		private void CheckGoodRecord(int _record, int Card)
		{
			StringBuilder checkMain = new StringBuilder();
			checkMain.Append(String.Format("select count(*) from {0}.{1}mast where mrecno = {2} and mdwell = {3} ",
						MainForm.localLib, MainForm.localPrefix, Record, Card));

			chekr = Convert.ToInt32(dbConnection.DBConnection.ExecuteScalar(checkMain.ToString()));

			if (chekr == 0)
			{
				MessageBox.Show("Invalid Master Record --- Please ReEnter");

				RecordTxt.Text = String.Empty;
				CardTxt.Text = String.Empty;
				sketchBox.Image = null;
				RecordTxt.Focus();
			}
		}

		private void CleanUpSketch()
		{
			MessageBox.Show("Clean Up unfinished Section");

			StringBuilder countSec = new StringBuilder();
			countSec.Append(String.Format("select count(*) from {0}.{1}line where jlsect = '{2}' ", MainForm.FClib, MainForm.FCprefix, ExpandoSketch.nextSectionLetter));

			int sectcount = Convert.ToInt32(dbConnection.DBConnection.ExecuteScalar(countSec.ToString()));

			if (sectcount > 0)
			{
				Cursor = Cursors.WaitCursor;

				StringBuilder cleanup = new StringBuilder();
				cleanup.Append(String.Format("delete from {0}.{1}line where jlsect = '{2}' ", MainForm.FClib, MainForm.FCprefix, ExpandoSketch.nextSectionLetter));
				cleanup.Append(String.Format(" and jlrecord = {0} and jldwell = {1} ",
							_currentParcel.mrecno,
							_currentParcel.mdwell));

				dbConnection.DBConnection.ExecuteNonSelectStatement(cleanup.ToString());

				Cursor = Cursors.Default;
			}
		}

		private void ClearVacantParcel(int gar1cde, int gar1cnt, int gar2cde, int gar2cnt, int cpcde, int cpcnt, int bicnt)
		{
			string clearVacantParcelSql = string.Format("update {0}.{1}mast set mgart = {2}, mgar#c = {3}, mgart2 = {4}, mgar#2 = {5}, mcarpt = {6}, mcar#c = {7}, mbi#c = {8}  where mrecno = {9} and mdwell = {10} ",
												MainForm.FClib,
												MainForm.FCprefix,
												gar1cde,
												gar1cnt,
												gar2cde,
												gar2cnt,
												cpcde,
												cpcnt,
												bicnt,
										_currentParcel.mrecno,
										_currentParcel.mdwell);
			dbConnection.DBConnection.ExecuteNonSelectStatement(clearVacantParcelSql);
		}

		private void ClearX()
		{
			string clrx = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", MainForm.FClib, MainForm.FCprefix, _currentParcel.mrecno, _currentParcel.mdwell);

			dbConnection.DBConnection.ExecuteNonSelectStatement(clrx);
		}

		private void ConnectToCamra()
		{
			if (string.IsNullOrEmpty(IPAddress))
			{
				IPAddress = Program.commandLineArgs.IPAddress;
			}
			if (string.IsNullOrEmpty(SketchUp.Properties.Settings.Default.IPAddress))
			{
				SketchUp.Properties.Settings.Default.IPAddress = IPAddress;
			}

			bool goodIP = System.Text.RegularExpressions.Regex.IsMatch(IPAddress, SWallTech.RegexPatterns.IPAddressRegexPattern) && !string.IsNullOrEmpty(IPAddress);

			if (goodIP)
			{
				dbConnection = new CAMRA_Connection()
				{
					DataSource = SketchUp.Properties.Settings.Default.IPAddress,

					User = "camra2",
					Password = "camra2"
				};
			}
			else
			{
				UpdateStatus(string.Format("IP Address missing or invalid. (IPAddress: {0})", IPAddress));
				MessageBox.Show("No connection information supplied. The program will update and close.");
				if (Application.OpenForms != null && Application.OpenForms.Count > 0)
				{
					for (int i = Application.OpenForms.Count - 1; i > 0; i--)
					{
						Application.OpenForms[i].Close();
					}
				}

				Application.DoEvents();
				Application.Exit();
			}
		}

		private void EstablishCamraConnection()
		{
			string defaultIPAdress = string.IsNullOrEmpty(SketchUp.Properties.Settings.Default.IPAddress) ? Program.commandLineArgs.IPAddress : SketchUp.Properties.Settings.Default.IPAddress.Trim();
			if (string.IsNullOrEmpty(SketchUp.Properties.Settings.Default.IPAddress))
			{
				SketchUp.Properties.Settings.Default.IPAddress = Program.commandLineArgs.IPAddress;
			}
			SketchUp.Properties.Settings.Default.IPAddress = defaultIPAdress;

			if (string.IsNullOrEmpty(Program.commandLineArgs.IPAddress))
			{
				ReturnUpdateMessage();
				return;
			}
			else
			{
				SetFCValues(defaultIPAdress);

				ConnectToCamra();
				SketchFolder = String.Format(@"{0}:\{1}\{2}\new_Sketch",
								"C",
								FClib,
								FCprefix);
			}
		}

		private void FixLength(int Record, int Dwell)
		{
			StringBuilder fixNS = new StringBuilder();
			fixNS.Append(String.Format("update {0}.{1}line set jllinelen = jlylen where jlrecord = {2} and jldwell = {3} ",
								MainForm.FClib, MainForm.FCprefix, Record, Dwell));
			fixNS.Append("and jldirect in ( 'N','S' ) and jlylen <> jllinelen ");
			////UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixNS", fixNS);

			dbConnection.DBConnection.ExecuteNonSelectStatement(fixNS.ToString());
			////UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixNS", fixNS);

			StringBuilder fixEW = new StringBuilder();
			fixEW.Append(String.Format("update {0}.{1}line set jllinelen = jlxlen where jlrecord = {2} and jldwell = {3} ",
								MainForm.FClib, MainForm.FCprefix, Record, Dwell));
			fixEW.Append("and jldirect in ( 'E','W' ) and jlxlen <> jllinelen ");
			////UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixEW", fixEW);
			dbConnection.DBConnection.ExecuteNonSelectStatement(fixEW.ToString());
			////UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixEW", fixEW);
		}

		private void getSketch(int newRecord, int newCard)
		{
			string newRecordString = newRecord.ToString().PadLeft(7, '0');
			string newCardString = newCard.ToString().PadLeft(2, '0');
			string skFolderPath = String.Format(@"{0}:\{1}\{2}\{3}",
				   "C",
				   FClib,
				   FCprefix,
				   "new_Sketch");

			if (Directory.Exists(skFolderPath))
			{
				_selectedSktPath = skFolderPath;

				sketchBox.Image = null;

				DirectoryInfo dir = new DirectoryInfo(_selectedSktPath);
				string pattern = String.Format("{0}_{1}.JPG",
					newRecordString,
					newCardString);

				string sketchpath = string.Format("{0}/{1}",
			   _selectedSktPath,
			   pattern);

				if (File.Exists(sketchpath))
				{
					DialogResult result;
					result = (MessageBox.Show("Saved Sketch Exists, Do you want to Display", "Saved Sketch Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question));

					if (result == DialogResult.Yes)
					{
						_hasSketch = true;
					}
					if (result == DialogResult.No)
					{
						_hasSketch = false;
					}
				}

				SetSketchBoxValues(ref sketchpath);

#if DEBUG

				//Debugging Code -- remove for production release
				//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
				//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			}
		}

		private void LoadInitialSketch()
		{
			_currentParcel = ParcelData.getParcel(dbConnection, Record, Card);

			_subSections = new SectionDataCollection(dbConnection, Record, Card);
			FixLength(Record, Card);
			int tr = _currentParcel.mrecno;
			UtilityMethods.DeleteXLines(dbConnection, Record, Card, FClib, FCprefix);
			int sectionCount = UtilityMethods.SectionCount(dbConnection, Record, Card, localLib, localPrefix);
			Application.DoEvents();
			SetTextAddOrEdit(sectionCount);
			Application.DoEvents();
			bool vacantParcel = CamraSupport.VacantOccupancies.Contains(_currentParcel.moccup);
			if (vacantParcel)
			{
				int gar1cde = 0;
				int gar1cnt = 0;
				int gar2cde = _currentParcel.mgart2;
				int gar2cnt = _currentParcel.mgarN2;
				int cpcde = 0;
				int cpcnt = 0;
				int bicnt = 0;

				if (_currentParcel.mgart2 != 64)
				{
					gar2cde = 0;
					gar2cnt = 0;
				}
				Application.DoEvents();
				ClearVacantParcel(gar1cde, gar1cnt, gar2cde, gar2cnt, cpcde, cpcnt, bicnt);
				if (sectionCount == 0)
				{
					DialogResult sectionWarningDialogResult;
					sectionWarningDialogResult = (MessageBox.Show("Must Enter Master Record Info Before Sketch", "Missing Master Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information));
					Application.DoEvents();
					if (sectionWarningDialogResult == DialogResult.OK)
					{
						this.WindowState = FormWindowState.Minimized;
					}
					if (sectionWarningDialogResult == DialogResult.Cancel)
					{
						//Ask Dave if there needs to be something else done here.
						splash.UpdateProgress();
						Application.DoEvents();
					}
				}
				SetTextAddOrEdit(sectionCount);
			}

			try
			{
				getSketch(_currentParcel.mrecno, _currentParcel.mdwell);
			}
			catch (Exception ex)
			{
				Logger.TraceMessage(string.Format("{0} failed. Error: {1}", "", ex.Message));
				Logger.Error(ex, MethodBase.GetCurrentMethod().Module.Name);
				MessageBox.Show(ex.Message);
				throw;
			}

			CurrentSketch = _currentParcel.GetSketchImage(374);
			sketchBox.Image = CurrentSketch;
		}

		private void ParseArgsToProperties(CommandLineArguments args)
		{
			localLib = args.Library;
			localPrefix = args.Locality;
			Record = args.Record;
			Card = args.Card;
			IPAddress = args.IPAddress;
			SketchUp.Properties.Settings.Default.IPAddress = IPAddress;
		}

		private void RecordTxt_Leave(object sender, EventArgs e)
		{
			int rec = 0;
			int.TryParse(RecordTxt.Text, out rec);

			Record = rec;
		}

		private void ReturnUpdateMessage()
		{
			statusMessage.Text = "Update check completed.";
		}

		private void SetConnectionLibraryParameters(DBAccessManager _connDB)
		{
			Application.DoEvents();
			if (dbConnection == null || dbConnection.DBConnection == null)
			{
				UpdateStatus("CAMRA connection is null");
			}
			else if (dbConnection.DBConnection.IsConnected)
			{
				UpdateStatus("CAMRA connection successful");

				_db = _connDB;

				string dsIP = dbConnection.DBConnection.DataSource.ToString();

				localPrefix = FCprefix;

				dbConnection.DBConnection.DataSource = IPAddress;

				RecordTxt.Text = FCrecord.ToString();
				CardTxt.Text = FCcard.ToString();

				dbConnection.LocalityPrefix = localPrefix;
				localDescription = dbConnection.Localities.GetLocalityName(localPrefix);

				LocNameTxt.Text = localDescription;
				LibraryTxt.Text = localLib;
				PreFixTxt.Text = localPrefix;

				CheckGoodRecord(Record, Card);
				CamraSupport.Init(dbConnection);
				Application.DoEvents();
				if (chekr > 0)
				{
					Application.DoEvents();
					LoadInitialSketch();

					if (localLib == String.Empty)
					{
						MessageBox.Show("Invalid Library & File Information");
					}
				}

				_today = DateTime.Today;

				Year = DateTime.Today.Year;
				Month = DateTime.Today.Month;
				Today = DateTime.Today.Day;

				Cursor = Cursors.Arrow;
			}

			Application.DoEvents();
			splash.Close();
		}

		private static void SetFCValues(string IP)
		{
			FClib = Program.commandLineArgs.Library;
			FCprefix = Program.commandLineArgs.Locality;
			FCrecord = Program.commandLineArgs.Record;
			InitalRec = Program.commandLineArgs.Record;
			FCcard = Program.commandLineArgs.Card;
			FCipAddress = Program.commandLineArgs.IPAddress;
			InitalRec = Program.commandLineArgs.Record;
			InitalCard = Program.commandLineArgs.Card;
		}

		private void SetSketchBoxValues(ref string sketchpath)
		{
			CurrentSketch = _currentParcel.GetSketchImage(374);

#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif

			if (File.Exists(sketchpath) && _hasSketch == true)
			{
				sketchImage = sketchpath.GetImage();

				sketchBox.Image = sketchImage;
			}
			else
			{
				sketchBox.Image = CurrentSketch;
			}
		}

		private void SetTextAddOrEdit(int sectionCount)
		{
			if (!CamraSupport.VacantOccupancies.Contains(_currentParcel.moccup))
			{
				if (sectionCount > 0)
				{
					EditImage.Text = "Edit Sketch";
				}
				if (sectionCount == 0)
				{
					EditImage.Text = "Add Sketch";
				}
			}
		}

		private FormSplash ShowSpashScreen()
		{
			FormSplash splash = new FormSplash();
			splash.Show();
			splash.Update();
			Cursor = Cursors.WaitCursor;
			return splash;
		}

		private void ShowUpdateMessage()
		{
			Application.Run(new UpdateInfo());
		}

		public void UpdateStatus(string status)
		{
			try
			{
				statusMessage.Text = status;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, MethodBase.GetCurrentMethod().Module.Name);
				Console.WriteLine(string.Format("Status bar not available. Status message: {0}", ex.Message));
#if DEBUG
				MessageBox.Show(status);
#endif
			}
		}

		#endregion Private Methods

		private void MainForm_Shown(object sender, EventArgs e)
		{
			//splash.loadingProgBar.Value = 100;
			//splash.Close();
			//splash.Dispose();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			splash.UpdateProgress();
		}
	}
}