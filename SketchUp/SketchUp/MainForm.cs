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
		#region fields

		private FormSplash splash;

		public static int Today
		{
			get; set;
		}

		public static int Month
		{
			get; set;
		}

		public static int Year
		{
			get; set;
		}

		public static string reopenSec
		{
			get; set;
		}

		public Image sketchImage
		{
			get; set;
		}

		public Image CurrentSketch
		{
			get; set;
		}

		public DateTime _today
		{
			get; set;
		}

		private DBAccessManager _db = null;

		private CAMRA_Connection _conn = null;

		private ParcelData _currentParcel = null;

		private SectionDataCollection _subSections = null;

		private string _selectedPath = String.Empty;

		private string _selectedPicPath = String.Empty;

		private string _selectedSktPath = String.Empty;

		private bool _hasNewSketch = false;

		private string SketchFolder
		{
			get; set;
		}

		public static string localLib = String.Empty;
		public static string localDescription = string.Empty;
		public static string localPreFix = String.Empty;

		public static int Record = 0;

		public static int Card = 0;

		public static string FClib = String.Empty;

		public static string FCprefix = String.Empty;

		public static int FCrecord = 0;

		public static int FCcard = 0;

		public static string FCipAddress = String.Empty;

		public static int InitalRec = 0;

		public static int InitalCard = 0;

		public static int chekr = 0;

		public static bool _hasSketch = false;

		public string IPAddress = String.Empty;

		public bool _mainClosed = false;

		public bool _isMinimized = false;

		#endregion fields

		#region Constructor

		public MainForm()
		{
			if (Program.commandLineArgs != null && Program.commandLineArgs.Library != null && !string.IsNullOrEmpty(Program.commandLineArgs.Library))

			{
				InitializeComponent();
				timer.Start();
				splash = ShowSpashScreen();
				splash.UpdateProgress(10);
				Application.DoEvents();
				_isMinimized = false;
				splash.UpdateProgress(20);
				Application.DoEvents();
				// TODO: Remove if not needed:	bool test = ExpandoSketch.RefreshEditImageBtn;

				DBAccessManager _connDB = null;
				splash.UpdateProgress(30);
				Application.DoEvents();
				ParseArgsToProperties(Program.commandLineArgs);
				splash.UpdateProgress(45);
				Application.DoEvents();

				EstablishCamraConnection();
				splash.UpdateProgress(65);
				Application.DoEvents();
				SetConnectionLibraryParameters(_connDB);
				splash.UpdateProgress(85);
				Application.DoEvents();
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
				InitializeComponent();
				timer.Start();
				splash = ShowSpashScreen();
				splash.UpdateProgress(10);
				Application.DoEvents();
				_isMinimized = false;
				splash.UpdateProgress(20);
				Application.DoEvents();
				bool test = ExpandoSketch.RefreshEditImageBtn;

				DBAccessManager _connDB = null;
				splash.UpdateProgress(30);
				Application.DoEvents();
				ParseArgsToProperties(Program.commandLineArgs);
				splash.UpdateProgress(45);
				Application.DoEvents();

				EstablishCamraConnection();
				splash.UpdateProgress(65);
				Application.DoEvents();
				SetConnectionLibraryParameters(_connDB);
				splash.UpdateProgress(85);
				Application.DoEvents();
			}
			else
			{
				ShowUpdateMessage();
			}
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

		#region Private Methods

		private void SetConnectionLibraryParameters(DBAccessManager _connDB)
		{
			Application.DoEvents();
			if (_conn == null || _conn.DBConnection == null)
			{
				UpdateStatus("CAMRA connection is null");
			}
			else if (_conn.DBConnection.IsConnected)
			{
				UpdateStatus("CAMRA connection successful");

				_db = _connDB;

				string dsIP = _conn.DBConnection.DataSource.ToString();

				localPreFix = FCprefix;

				_conn.DBConnection.DataSource = IPAddress;

				RecordTxt.Text = FCrecord.ToString();
				CardTxt.Text = FCcard.ToString();

				_conn.LocalityPrefix = localPreFix;
				localDescription = _conn.Localities.GetLocalityName(localPreFix);

				LocNameTxt.Text = localDescription;
				LibraryTxt.Text = localLib;
				PreFixTxt.Text = localPreFix;

				CheckGoodRecord(Record, Card);
				CamraSupport.Init(_conn);
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

		private void LoadInitialSketch()
		{
			_currentParcel = ParcelData.getParcel(_conn, Record, Card);
			_subSections = new SectionDataCollection(_conn, Record, Card);
			FixLength(Record, Card);
			int tr = _currentParcel.mrecno;
			ClearX();
			int seccnt = SectionCount();
			Application.DoEvents();
			SetTextAddOrEdit(seccnt);
			Application.DoEvents();
			if (CamraSupport.VacantOccupancies.Contains(_currentParcel.moccup))
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
				string clrvac = string.Format("update {0}.{1}mast set mgart = {2}, mgar#c = {3}, mgart2 = {4}, mgar#2 = {5}, mcarpt = {6}, mcar#c = {7}, mbi#c = {8}  where mrecno = {9} and mdwell = {10} ",
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
				_conn.DBConnection.ExecuteNonSelectStatement(clrvac);
				if (seccnt == 0)
				{
					DialogResult secresult;
					secresult = (MessageBox.Show("Must Enter Master Record Info Before Sketch", "Missing Master Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information));
					Application.DoEvents();
					if (secresult == DialogResult.OK)
					{
						this.WindowState = FormWindowState.Minimized;
					}
					if (secresult == DialogResult.Cancel)
					{
						//Ask Dave if there needs to be something else done here.
						splash.UpdateProgress();
						Application.DoEvents();
					}
				}
				SetTextAddOrEdit(seccnt);
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

		private int SectionCount()
		{
			string checkSect = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", MainForm.FClib, MainForm.FCprefix, _currentParcel.mrecno, _currentParcel.mdwell);

			int seccnt = Convert.ToInt32(_conn.DBConnection.ExecuteScalar(checkSect));
			return seccnt;
		}

		private void ClearX()
		{
			string clrx = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", MainForm.FClib, MainForm.FCprefix, _currentParcel.mrecno, _currentParcel.mdwell);

			_conn.DBConnection.ExecuteNonSelectStatement(clrx);
		}

		private void ParseArgsToProperties(CommandLineArguments args)
		{
			localLib = args.Library;
			localPreFix = args.Locality;
			Record = args.Record;
			Card = args.Card;
			IPAddress = args.IPAddress;
			SketchUp.Properties.Settings.Default.IPAddress = IPAddress;
		}

		private void ShowUpdateMessage()
		{
			Application.Run(new UpdateInfo());
		}

		private void ReturnUpdateMessage()
		{
			statusMessage.Text = "Update check completed.";
		}

		private void CheckGoodRecord(int _record, int Card)
		{
			StringBuilder checkMain = new StringBuilder();
			checkMain.Append(String.Format("select count(*) from {0}.{1}mast where mrecno = {2} and mdwell = {3} ",
						MainForm.localLib, MainForm.localPreFix, Record, Card));

			chekr = Convert.ToInt32(_conn.DBConnection.ExecuteScalar(checkMain.ToString()));

			if (chekr == 0)
			{
				MessageBox.Show("Invalid Master Record --- Please ReEnter");

				RecordTxt.Text = String.Empty;
				CardTxt.Text = String.Empty;
				sketchBox.Image = null;
				RecordTxt.Focus();
			}
		}

		private void FixLength(int Record, int Dwell)
		{
			StringBuilder fixNS = new StringBuilder();
			fixNS.Append(String.Format("update {0}.{1}line set jllinelen = jlylen where jlrecord = {2} and jldwell = {3} ",
								MainForm.FClib, MainForm.FCprefix, Record, Dwell));
			fixNS.Append("and jldirect in ( 'N','S' ) and jlylen <> jllinelen ");
			////UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixNS", fixNS);

			_conn.DBConnection.ExecuteNonSelectStatement(fixNS.ToString());
			////UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixNS", fixNS);

			StringBuilder fixEW = new StringBuilder();
			fixEW.Append(String.Format("update {0}.{1}line set jllinelen = jlxlen where jlrecord = {2} and jldwell = {3} ",
								MainForm.FClib, MainForm.FCprefix, Record, Dwell));
			fixEW.Append("and jldirect in ( 'E','W' ) and jlxlen <> jllinelen ");
			////UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixEW", fixEW);
			_conn.DBConnection.ExecuteNonSelectStatement(fixEW.ToString());
			////UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixEW", fixEW);
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
				_conn = new CAMRA_Connection()
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

		private void RecordTxt_Leave(object sender, EventArgs e)
		{
			int rec = 0;
			int.TryParse(RecordTxt.Text, out rec);

			Record = rec;
		}

		private void CardTxt_Leave(object sender, EventArgs e)
		{
			int card = 0;
			int.TryParse(CardTxt.Text, out card);

			Card = card;
		}

		private void AddSketchToParcel()
		{
			_currentParcel = ParcelData.getParcel(_conn, Record, Card);

			_subSections = new SectionDataCollection(_conn, Record, Card);

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

		private void CleanUpSketch()
		{
			MessageBox.Show("Clean Up unfinished Section");

			StringBuilder countSec = new StringBuilder();
			countSec.Append(String.Format("select count(*) from {0}.{1}line where jlsect = '{2}' ", MainForm.FClib, MainForm.FCprefix, ExpandoSketch._nextSectLtr));

			int sectcount = Convert.ToInt32(_conn.DBConnection.ExecuteScalar(countSec.ToString()));

			if (sectcount > 0)
			{
				Cursor = Cursors.WaitCursor;

				StringBuilder cleanup = new StringBuilder();
				cleanup.Append(String.Format("delete from {0}.{1}line where jlsect = '{2}' ", MainForm.FClib, MainForm.FCprefix, ExpandoSketch._nextSectLtr));
				cleanup.Append(String.Format(" and jlrecord = {0} and jldwell = {1} ",
							_currentParcel.mrecno,
							_currentParcel.mdwell));

				_conn.DBConnection.ExecuteNonSelectStatement(cleanup.ToString());

				Cursor = Cursors.Default;
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

		#region Form Control Methods

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

				_subSections = new SectionDataCollection(_conn, _currentParcel.mrecno, _currentParcel.mdwell);

				_currentParcel.BuildSketchData();
				getSketch(_currentParcel.Record, _currentParcel.Card);

				//Ask Dave why this happens twice
				CurrentSketch = _currentParcel.GetSketchImage(374);
				sketchBox.Image = CurrentSketch;

				if (EditImage.Text == "Add Sketch")
				{
					EditImage.Text = "Edit Sketch";
				}

				StringBuilder delXline = new StringBuilder();
				delXline.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ",
											MainForm.FClib, MainForm.FCprefix, _currentParcel.Record, _currentParcel.Card));

				_conn.DBConnection.ExecuteNonSelectStatement(delXline.ToString());

				StringBuilder cntSect = new StringBuilder();
				cntSect.Append(String.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
							FClib, FCprefix, _currentParcel.Record, _currentParcel.Card));

				int SectionCnt = Convert.ToInt32(_conn.DBConnection.ExecuteScalar(cntSect.ToString()));

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
						ExpandoSketch skexp = new ExpandoSketch(_currentParcel, SketchFolder, _currentParcel.mrecno.ToString(), _currentParcel.mdwell.ToString(),
						   MainForm.FCprefix, _conn, _subSections, _hasSketch, sketchImage, _hasNewSketch);

						skexp.ShowDialog(this);

						if (ExpandoSketch._isClosed == false && ExpandoSketch._deleteThisSketch == true)
						{
							CleanUpSketch();
						}

						int record = _currentParcel.mrecno;
						int card = _currentParcel.mdwell;

						_currentParcel = null;
						_subSections = null;

						_currentParcel = ParcelData.getParcel(_conn, record, card);
						_subSections = new SectionDataCollection(_conn, record, card);

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

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//Program.ShowCheckpointLog();
			Application.Exit();
		}

		#endregion Form Control Methods
	}
}