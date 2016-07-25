using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public partial class MainForm : Form
    {
        #region Constructor

        public MainForm()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Running {MethodBase.GetCurrentMethod().Name}");
#endif
            try
            {
                if (Program.commandLineArgs != null && Program.commandLineArgs.Library != null && !string.IsNullOrEmpty(Program.commandLineArgs.Library))

                {
                    LoadSplashScreenWhileGettingData();
                    InitializeParcelSnapshots();
                    splash.UpdateProgress(100);
                    Application.DoEvents();
                    ShowSketch(MainFormParcel);
                    BringToFront();
                }
                else
                {
                    ShowUpdateMessage();
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        public MainForm(string[] newArgs)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Running {MethodBase.GetCurrentMethod().Name}");
#endif
            try
            {
                if (Program.commandLineArgs != null && Program.commandLineArgs.Library != null && !string.IsNullOrEmpty(Program.commandLineArgs.Library))

                {
                    MainStatus = MainFormStatus.Connecting;
                    LoadSplashScreenWhileGettingData();
                    InitializeParcelSnapshots();
                    splash.UpdateProgress(100);
                    Application.DoEvents();
                    ShowSketch(SketchUpGlobals.ParcelWorkingCopy);
                    Activate();
                }
                else
                {
                    ShowUpdateMessage();
                }
#if DEBUG || TEST
                Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        public void LoadSplashScreenWhileGettingData()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            InitializeComponent();
            timer.Start();
            splash = ShowSplashScreen();
            splash.UpdateProgress(25);
            Application.DoEvents();
            SketchUpGlobals.MainFormIsMinimized = false;
            splash.UpdateProgress(50);

            try
            {
                SketchUpGlobals.DbAccessMgr = null;
                splash.UpdateProgress(30);
                Application.DoEvents();
                ParseArgsToProperties(Program.commandLineArgs);
                splash.UpdateProgress(45);
            }
            catch (Exception ex)
            {
#if DEBUG

                MessageBox.Show(ex.Message);
#endif
                Logger.Error(ex, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().Name));
                throw;
            }

            try
            {
                EstablishCamraConnection();
                splash.UpdateProgress(65);
                Application.DoEvents();
                SetConnectionLibraryParameters(SketchUpGlobals.DbAccessMgr);
            }
            catch (Exception ex)
            {
#if DEBUG

                MessageBox.Show(ex.Message);
#endif
                Logger.Error(ex, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().Name));
                throw;
            }
            splash.UpdateProgress(85);
            Application.DoEvents();
        }

        private void InitializeParcelSnapshots()
        {
            try
            {
#if DEBUG || TEST
                Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
                ;
#endif
                MainFormParcelMast = SketchRepo.SelectParcelMasterWithParcel(SketchUpGlobals.Record, SketchUpGlobals.Card);
                mainFormParcel = MainFormParcelMast.Parcel;
                mainFormParcel.SnapshotIndex = 0;
                SketchUpGlobals.SketchSnapshots.Clear();
                SketchUpGlobals.SketchSnapshots.Add(MainFormParcel);
                SectionsCount = SectionCount();

#if DEBUG || TEST
                Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
            }
            catch (Exception ex)
            {
#if DEBUG

                MessageBox.Show(ex.Message);
#endif
                Logger.Error(ex, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().Name));
                throw;
            }
        }

        private void ShowSketch(SMParcel parcel)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");

#endif
            var sketcher = new SMSketcher(parcel, sketchBox);
            sketcher.SketchFont = new Font("Segue UI", 6);
            sketcher.PenColor = Color.Blue;
            sketcher.SketchPen = new Pen(sketcher.PenColor, 2);
            sketcher.RenderSketch();
            sketchBox.Image = sketcher.SketchImage;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        #endregion Constructor

        #region fields

        private SplashForm splash;

        #endregion fields

        #region Form Control Methods

        private void BuildNewSketch()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            SMParcelMast newMast = SketchRepo.SelectParcelMasterWithParcel(SketchUpGlobals.Record, SketchUpGlobals.Card);
            var parcel = new SMParcel();
            parcel.Record = newMast.Record;
            parcel.TotalSqFt = 0.00M;
            parcel.ExSketch = string.Empty;
            parcel.Scale = 1.0;
            parcel.ParcelMast = newMast;
            parcel.SnapshotIndex = 0;
            SketchRepo.AddSketchToSnapshots(parcel);
            parcel.SnapshotIndex++;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private bool CheckForAddOnNoSketch(bool omitSketch)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            if (MainFormParcel.Sections == null || MainFormParcel.Sections.Count == 0)
            {
                SketchUpGlobals.HasNewSketch = false;
                SketchUpGlobals.HasSketch = false;
                DialogResult result;
                result = (MessageBox.Show("Add Sketch?", "Sketch Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question));

                if (result == DialogResult.Yes)
                {
                    BuildNewSketch();

                    EditImage.Text = "Edit Sketch";

                    SketchUpGlobals.HasNewSketch = true;
                }
                if (result == DialogResult.No)
                {
                    omitSketch = true;
                }
            }
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
            return omitSketch;
        }

        private void CreateSketchForEditing()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            var sketcher = new SMSketcher(MainFormParcel, sketchBox);
            sketcher.PenColor = Color.Blue;
            sketcher.SketchPen = new Pen(sketcher.PenColor, 5);
            sketcher.RenderSketch(string.Empty);
            SketchUpGlobals.SketchImage = sketcher.SketchImage;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void EditImage_Click(object sender, EventArgs e)
        {
            try
            {
                EditSketch();
                UpdateStatus("Ready");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        private void EditSketch()
        {
            UseWaitCursor = true;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            UpdateStatus("Loading Sketch Editor...");
            CreateSketchForEditing();
            if (EditImage.Text == "Add Sketch")
            {
                EditImage.Text = "Edit Sketch";
            }

            // ClearXLinesFromDb();

            int SectionCnt = MainFormParcel.Sections.Count;

            bool omitSketch = false;

            omitSketch = CheckForAddOnNoSketch(omitSketch);
            SetTextAddOrEdit(SectionCnt);
            bool doSketch = !(omitSketch || ExpandoSketch._cantSketch);
            if (doSketch)
            {
                LoadExpandoSketch();
            }
        }

        private void GetNewSMParcel(int record, int card)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            MainFormParcel = SketchRepo.SelectParcelMasterWithParcel(record, card).Parcel;
            SketchUpGlobals.SketchSnapshots.Clear();
            MainFormParcel.SnapshotIndex = 0;
            SketchUpGlobals.SketchSnapshots.Add(MainFormParcel);
            SketchUpGlobals.SMParcelFromData = MainFormParcel;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void LoadExpandoSketch()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            var skexp = new ExpandoSketch(SketchUpGlobals.SketchFolder, SketchUpGlobals.Record, SketchUpGlobals.Card,
              SketchUpGlobals.HasSketch, SketchUpGlobals.HasNewSketch);

            skexp.ShowDialog(this);
            skexp.BringToFront();
            if (skexp == null || skexp.IsDisposed)
            {
                UseWaitCursor = false;
                statusMessage.Text = "Ready";
            }

            if (!ExpandoSketch._isClosed && ExpandoSketch._deleteThisSketch)
            {
                ShowSketch(SketchUpGlobals.ParcelWorkingCopy);
            }
            BringToFront();
            UseWaitCursor = false;
            if (ExpandoSketch._deleteMaster)
            {
                EditImage.Text = "Add Sketch";
            }
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void LoadMainForm()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            MainStatus = MainFormStatus.Loading;
            Application.DoEvents();
            RetrieveAndShowCurrentSketchImage();
            MainStatus = MainFormStatus.Ready;
            SetTextAddOrEdit();
            EditImage.Focus();
            UseWaitCursor = false;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadMainForm();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.BalloonTipText = "Main Form Minimized";
                notifyIcon1.ShowBalloonTip(10000);
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon1.BalloonTipText = "Main Form is back to Normal";
                notifyIcon1.ShowBalloonTip(10000);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void RetrieveAndShowCurrentSketchImage()
        {
            try
            {
#if DEBUG || TEST
                Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
                ;
#endif
                SketchUpGlobals.SMParcelFromData = SketchRepo.SelectParcelMasterWithParcel(SketchUpGlobals.Record, SketchUpGlobals.Card).Parcel;
                MainFormParcel = SketchUpGlobals.SMParcelFromData;
                var sms = new SMSketcher(MainFormParcel, sketchBox);
                sms.RenderSketch(string.Empty);
                SketchUpGlobals.SketchImage = sms.SketchImage;
                sketchBox.Image = sms.SketchImage;
#if DEBUG || TEST
                Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if TEST||DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void SelectRecord()
        {
            if (SketchUpGlobals.Record == 0)
            {
                MessageBox.Show("Must Enter Record No.");
                RecordTxt.Focus();
            }
            if (SketchUpGlobals.Record != 0 && SketchUpGlobals.Card == 0)
            {
                SketchUpGlobals.Card = 1;
            }

            if (SketchUpGlobals.Record != SketchUpGlobals.InitalRecord)
            {
                MessageBox.Show(string.Format("Now Working on New Record - {0}  Old Record was - {1}", SketchUpGlobals.Record, SketchUpGlobals.InitalRecord));

                SketchUpGlobals.InitalRecord = SketchUpGlobals.Record;
                SketchUpGlobals.InitalCard = SketchUpGlobals.Card;

                CheckGoodRecord(SketchUpGlobals.Record, SketchUpGlobals.Card);

                if (SketchUpGlobals.Checker > 0)
                {
                    GetNewSMParcel(SketchUpGlobals.Record, SketchUpGlobals.Card);
                    AddSketchToParcel();
                }
            }
        }

        private void SelectRecordBtn_Click(object sender, EventArgs e)
        {
            SelectRecord();
        }

        #endregion Form Control Methods

        #region Methods

        public void LoadInitialSketch()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            FixLength(SketchUpGlobals.Record, SketchUpGlobals.Card);
            VacantOccupancyCode = CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.VacantOccupancies)).Contains(SketchUpGlobals.ParcelMast.OccupancyCode);
            ClearXLinesFromDb();
            int seccnt = SectionCount();
            Application.DoEvents();
            SetTextAddOrEdit(seccnt);
            Application.DoEvents();

            if (VacantOccupancyCode)
            {
                int gar2cde = SketchUpGlobals.ParcelMast.Garage2TypeCode;
                int gar2cnt = SketchUpGlobals.ParcelMast.Garage2NumCars;
                int detachedGarageCode = 0;
                string garCode = (from c in SketchUpLookups.GarageTypeCollection where c.ShortDescription.ToUpper() == "DET GAR" select c.Code).FirstOrDefault();
                int.TryParse(garCode, out detachedGarageCode);
                if (SketchUpGlobals.ParcelMast.Garage2TypeCode != detachedGarageCode)
                {
                    gar2cde = 0;
                    gar2cnt = 0;
                }

                Application.DoEvents();
                if (SectionsCount == 0)
                {
                    DialogResult secresult;
                    secresult = (MessageBox.Show("Must Enter Master Record Info Before Sketch", "Missing Master Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information));
                    Application.DoEvents();
                    if (secresult == DialogResult.OK)
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                    if (secresult == DialogResult.Cancel)
                    {
                        //Ask Dave if there needs to be something else done here.
                        splash.UpdateProgress();
                        Application.DoEvents();
                    }
                }
                else
                {
                    SetTextAddOrEdit(SectionsCount);
                }
            }

            try
            {
                getSketch(SketchUpGlobals.ParcelMast.Record, SketchUpGlobals.ParcelMast.Card);
            }
            catch (Exception ex)
            {
                Logger.TraceMessage(string.Format("{0} failed. Error: {1}", "", ex.Message));
                Logger.Error(ex, MethodBase.GetCurrentMethod().Module.Name);
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                throw;
            }

            sketchBox.Image = SketchUpGlobals.CurrentSketchImage;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        public void ParseArgsToProperties(CommandLineArguments args)
        {
            SketchUpGlobals.LocalLib = args.Library;
            SketchUpGlobals.LocalityPrefix = args.Locality;
            SketchUpGlobals.Record = args.Record;
            SketchUpGlobals.Card = args.Card;
            SketchUpGlobals.IpAddress = args.IPAddress;
            Properties.Settings.Default.IPAddress = SketchUpGlobals.IpAddress;
        }

        public void UpdateStatus(string status = "OK")
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            try
            {
                statusMessage.Text = status;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, MethodBase.GetCurrentMethod().Module.Name);
                Console.WriteLine(string.Format("Status bar not available. Status message: {0}", ex.Message));
            }
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private SplashForm ShowSplashScreen()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            var splash = new SplashForm();
            splash.Show();
            splash.Update();
            UseWaitCursor = true;
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
            return splash;
        }

        #endregion Methods

        #region Properties

        public MainFormStatus MainStatus
        {
            get { return mainStatus; }

            set {
                mainStatus = value;
                UpdateStatusMessage(value);
            }
        }

        private void UpdateStatusMessage(MainFormStatus currentStatus)
        {
            string statusText = "Ready";
            Bitmap statusImage = GoodConnectionImage;
#if DEBUG || TEST
            Trace.WriteLine($"Main form current status is {currentStatus}.");

            var tr = new StackTrace();
            Trace.IndentLevel++;
            foreach (StackFrame frame in tr.GetFrames().Where(f => f.GetMethod().Module.Name != "System.Windows.Forms.dll").Skip(1).Take(4))
            {
                Trace.WriteLine($"{frame.GetMethod().Name}");
            }

            Trace.IndentLevel--;
            Trace.Flush();

#endif
            switch (currentStatus)
            {
                case MainFormStatus.Connected:
                    statusText = $"Connected to CAMRA Database at {SketchUpGlobals.IpAddress}";
                    statusImage = GoodConnectionImage;

                    break;

                case MainFormStatus.Connecting:
                    statusText = $"Attempting to connect to CAMRA Database at {SketchUpGlobals.IpAddress}";

                    statusImage = ConnectingImage;
                    break;

                case MainFormStatus.Disconnected:
                    statusText = $"Unable to connect to CAMRA Database at {SketchUpGlobals.IpAddress}";

                    statusImage = BadConnectionImage;
                    break;

                case MainFormStatus.LoadingEditForm:
                    statusText = "Loading Sketch Editor";
                    statusImage = Properties.Resources.SmallBlueLoadingCircle;
                    break;

                case MainFormStatus.Ready:

                case MainFormStatus.Refreshed:
                    statusText = "Ready";
                    statusImage = GoodConnectionImage;
                    break;

                case MainFormStatus.Refreshing:
                    statusText = $"Refreshing Sketch for Record {SketchUpGlobals.Record}, Card # {SketchUpGlobals.Card}...";
                    statusImage = Properties.Resources.SmallBlueLoadingCircle;
                    break;

                case MainFormStatus.Loading:
                    statusText = $"Loading information for Record {SketchUpGlobals.Record}, Card # {SketchUpGlobals.Card}...";
                    statusImage = Properties.Resources.SmallBlueLoadingCircle;

                    break;
            }
            statusMessage.Text = statusText;
            statusMessage.Image = statusImage;
        }

        #endregion Properties

        #region Enums

        public enum MainFormStatus
        {
            Connected,
            Connecting,
            Disconnected,
            LoadingEditForm,
            Ready,
            Refreshed,
            Refreshing,
            Loading
        }

        #endregion Enums

        private static int MasterRecordCount()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Running {MethodBase.GetCurrentMethod().Name}");

#endif

            try
            {
                var checkMain = $"select count(*) from {SketchRepo.MastRecordTable} where mrecno = {SketchUpGlobals.Record} and mdwell = {SketchUpGlobals.Card}";

                return Convert.ToInt32(SketchUpGlobals.CamraDbConn.DBConnection.ExecuteScalar(checkMain.ToString()));
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                return 0;
            }
        }

        private static void SetFCValues(string IP)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: running {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            SketchUpGlobals.FcLib = Program.commandLineArgs.Library;
            SketchUpGlobals.FcLocalityPrefix = Program.commandLineArgs.Locality;

            SketchUpGlobals.FcCard = Program.commandLineArgs.Card;
            SketchUpGlobals.FcIpAddress = Program.commandLineArgs.IPAddress;
            SketchUpGlobals.FcRecord = Program.commandLineArgs.Record;
            SketchUpGlobals.InitalRecord = Program.commandLineArgs.Record;
            SketchUpGlobals.InitalCard = Program.commandLineArgs.Card;
            SketchUpGlobals.LocalityPrefix = Program.commandLineArgs.Locality;
        }

        private static void UpdateGarageAndCarportNumbersInDb(int gar1cde, int gar1cnt, int gar2cde, int gar2cnt, int cpcde, int cpcnt, int bicnt)
        {
            string clrvac = string.Format("update {0}.{1}mast set mgart = {2}, mgar#c = {3}, mgart2 = {4}, mgar#2 = {5}, mcarpt = {6}, mcar#c = {7}, mbi#c = {8}  where mrecno = {9} and mdwell = {10} ",
                                SketchUpGlobals.FcLib,
                                SketchUpGlobals.FcLocalityPrefix,
                                gar1cde,
                                gar1cnt,
                                gar2cde,
                                gar2cnt,
                                cpcde,
                                cpcnt,
                                bicnt,
                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card);
            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(clrvac);
        }

        private void AddSketchToParcel()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;
#endif
            if (CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.VacantOccupancies)).Contains(SketchUpGlobals.ParcelWorkingCopy.ParcelMast.OccupancyCode))
            {
                MessageBox.Show("Can't Add Sketch to Vacant Parcel...Add Master Record Data!");
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                SetTextAddOrEdit(SketchUpGlobals.ParcelWorkingCopy.Sections.Count);
            }
            try
            {
                getSketch(SketchUpGlobals.Record, SketchUpGlobals.ParcelMast.Card);
            }
            catch
            {
            }
            LoadSketchInPictureBox();
        }

        private void CardTxt_Leave(object sender, EventArgs e)
        {
            int card = 0;
            int.TryParse(CardTxt.Text, out card);

            SketchUpGlobals.Card = card;
        }

        private void CheckGoodRecord(int _record, int Card)
        {
            try
            {
                SketchUpGlobals.Checker = MasterRecordCount();

                if (SketchUpGlobals.Checker == 0)
                {
                    MessageBox.Show("Invalid Master Record --- Please Re-enter", "Invalid Record Number");

                    RecordTxt.Text = string.Empty;
                    CardTxt.Text = string.Empty;
                    sketchBox.Image = null;
                    RecordTxt.Focus();
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void ClearXLinesFromDb()
        {
            string clrx = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, SketchUpGlobals.Record, SketchUpGlobals.Card);

            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(clrx);
        }

        private void ConnectToCamra()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Starting {MethodBase.GetCurrentMethod().Name}");

#endif
            if (string.IsNullOrEmpty(SketchUpGlobals.IpAddress))
            {
                SketchUpGlobals.IpAddress = Program.commandLineArgs.IPAddress;
            }
            if (string.IsNullOrEmpty(Properties.Settings.Default.IPAddress))
            {
                Properties.Settings.Default.IPAddress = SketchUpGlobals.IpAddress;
            }

            bool goodIP = System.Text.RegularExpressions.Regex.IsMatch(SketchUpGlobals.IpAddress, SWallTech.RegexPatterns.IPAddressRegexPattern) && !string.IsNullOrEmpty(SketchUpGlobals.IpAddress);

            if (goodIP)
            {
                SketchUpGlobals.CamraDbConn = new CAMRA_Connection
                {
                    DataSource = Properties.Settings.Default.IPAddress,

                    User = Properties.Settings.Default.UserName,
                    Password = Properties.Settings.Default.Password
                };
            }
            else
            {
                UpdateStatus(string.Format("IP Address missing or invalid. (IPAddress: {0})", SketchUpGlobals.IpAddress));
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
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            string defaultIPAdress = string.IsNullOrEmpty(Properties.Settings.Default.IPAddress) ? Program.commandLineArgs.IPAddress : Properties.Settings.Default.IPAddress.Trim();
            if (string.IsNullOrEmpty(Properties.Settings.Default.IPAddress))
            {
                Properties.Settings.Default.IPAddress = Program.commandLineArgs.IPAddress;
            }
            Properties.Settings.Default.IPAddress = defaultIPAdress;

            if (string.IsNullOrEmpty(Program.commandLineArgs.IPAddress))
            {
                ReturnUpdateMessage();
                return;
            }
            else
            {
                SetFCValues(defaultIPAdress);

                ConnectToCamra();
                SketchUpGlobals.SketchFolder = $@"C:\{SketchUpGlobals.FcLib}\{SketchUpGlobals.FcLocalityPrefix}\new_Sketch";
            }
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void FixLength(int Record, int Dwell)
        {
            var fixNS = new StringBuilder();
            fixNS.Append(string.Format("update {0}.{1}line set jllinelen = jlylen where jlrecord = {2} and jldwell = {3} ",
                                SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, Record, Dwell));
            fixNS.Append("and jldirect in ( 'N','S' ) and jlylen <> jllinelen ");
            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(fixNS.ToString());

            var fixEW = new StringBuilder();
            fixEW.Append(string.Format("update {0}.{1}line set jllinelen = jlxlen where jlrecord = {2} and jldwell = {3} ",
                                SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, Record, Dwell));
            fixEW.Append("and jldirect in ( 'E','W' ) and jlxlen <> jllinelen ");
            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(fixEW.ToString());
        }

        private void getSketch(int newRecord, int newCard)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            string newRecordString = newRecord.ToString().PadLeft(7, '0');
            string newCardString = newCard.ToString().PadLeft(2, '0');
            string skFolderPath = string.Format(@"{0}:\{1}\{2}\{3}",
                   "C",
                 SketchUpGlobals.FcLib,
                                SketchUpGlobals.FcLocalityPrefix,
            "new_Sketch");

            if (Directory.Exists(skFolderPath))
            {
                SketchUpGlobals.SketchFolder = skFolderPath;

                sketchBox.Image = null;

                var dir = new DirectoryInfo(SketchUpGlobals.SketchFolder);
                string pattern = $"{newRecordString}_{newCardString}.JPG";

                string sketchpath = $@"{SketchUpGlobals.SketchFolder}/{pattern}";

                if (File.Exists(sketchpath))
                {
                    DialogResult result;
                    result = (MessageBox.Show("Saved Sketch Exists, Do you want to Display", "Saved Sketch Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question));

                    if (result == DialogResult.Yes)
                    {
                        SketchUpGlobals.HasSketch = true;
                    }
                    if (result == DialogResult.No)
                    {
                        SketchUpGlobals.HasSketch = false;
                    }
                }

                var sms = new SMSketcher(MainFormParcelMast.Parcel, sketchBox);
                sms.RenderSketch(string.Empty);
                MainFormParcelMast.Parcel = sms.LocalParcelCopy;
                SketchUpGlobals.SketchImage = sms.SketchImage;
                if (File.Exists(sketchpath) && SketchUpGlobals.HasSketch == true)
                {
                    SketchImage = sketchpath.GetImage();
                }
                else
                {
                    SketchImage = SketchUpGlobals.CurrentSketchImage;
                }
                sketchBox.Image = SketchImage;
            }
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void LoadSketchInPictureBox()
        {
            var sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch(string.Empty);
            SketchUpGlobals.SketchImage = sms.SketchImage;
            sketchBox.Image = SketchUpGlobals.SketchImage;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            LoadSketchInPictureBox();
            Application.DoEvents();
            SetTextAddOrEdit();

            statusMessage.Text = "Ready";

            BringToFront();
            UseWaitCursor = false;
        }

        private void RecordTxt_Leave(object sender, EventArgs e)
        {
            int rec = 0;
            int.TryParse(RecordTxt.Text, out rec);

            SketchUpGlobals.Record = rec;
        }

        private void ReturnUpdateMessage()
        {
            statusMessage.Text = "Update check completed.";
        }

        private int SectionCount() => (SketchUpGlobals.ParcelWorkingCopy == null || SketchUpGlobals.ParcelWorkingCopy.Sections == null) ? 0 : SketchUpGlobals.ParcelWorkingCopy.Sections.Count;

        private void SetConnectionLibraryParameters(DBAccessManager _db)
        {
            try
            {
                Application.DoEvents();
                if (SketchUpGlobals.CamraDbConn == null || SketchUpGlobals.CamraDbConn.DBConnection == null)
                {
                    UpdateStatus("CAMRA connection is null");
                }
                else if (SketchUpGlobals.CamraDbConn.DBConnection.IsConnected)
                {
                    UpdateStatus("CAMRA connection successful");

                    _db = SketchUpGlobals.DbAccessMgr;

                    string dsIP = SketchUpGlobals.CamraDbConn.DBConnection.DataSource.ToString();

                    SketchUpGlobals.LocalityPrefix = SketchUpGlobals.FcLocalityPrefix;

                    SketchUpGlobals.CamraDbConn.DBConnection.DataSource = SketchUpGlobals.IpAddress;

                    RecordTxt.Text = SketchUpGlobals.FcRecord.ToString();
                    CardTxt.Text = SketchUpGlobals.FcCard.ToString();

                    SketchUpGlobals.CamraDbConn.LocalityPrefix = SketchUpGlobals.LocalityPrefix;
                    SketchUpGlobals.LocalityDescription = SketchUpGlobals.CamraDbConn.Localities.GetLocalityName(SketchUpGlobals.LocalityPrefix);

                    LocNameTxt.Text = SketchUpGlobals.LocalityDescription;
                    LibraryTxt.Text = SketchUpGlobals.LocalLib;
                    PreFixTxt.Text = SketchUpGlobals.LocalityPrefix;
                    RecordTxt.Text = SketchUpGlobals.Record.ToString().Trim();
                    CardTxt.Text = SketchUpGlobals.Card.ToString().Trim();
                    CheckGoodRecord(SketchUpGlobals.Record, SketchUpGlobals.Card);
                    if (SketchUpGlobals.LocalLib == string.Empty)
                    {
                        MessageBox.Show("Invalid Library & File Information");
                    }
                    SketchUpLookups.Init(SketchUpGlobals.CamraDbConn);
                    Application.DoEvents();
                    if (SketchUpGlobals.Checker > 0)
                    {
                        Application.DoEvents();
                        //LoadInitialSketch();
                    }

                    Cursor = Cursors.Arrow;
                }
                StartPosition = FormStartPosition.CenterScreen;
                Application.DoEvents();
                splash.Close();
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    WindowState = FormWindowState.Normal;
                    Show();
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void SetTextAddOrEdit(int sectionCount)
        {
            if (!VacantOccupancyCode)
            {
                if (SectionsCount > 0)
                {
                    EditImage.Text = "Edit Sketch";
                }
                if (SectionsCount == 0)
                {
                    EditImage.Text = "Add Sketch";
                }
            }
        }

        private void SetTextAddOrEdit()
        {
            SectionsCount = (SketchUpGlobals.ParcelWorkingCopy.Sections == null ? 0 : SketchUpGlobals.ParcelWorkingCopy.Sections.Count);
            if (!CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.VacantOccupancies)).Contains(MainFormParcel.ParcelMast.OccupancyCode))
            {
                if (SectionsCount > 0)
                {
                    EditImage.Text = "Edit Sketch";
                }
                if (SectionsCount == 0)
                {
                    EditImage.Text = "Add Sketch";
                }
            }
        }

        private void ShowUpdateMessage()
        {
            Application.Run(new UpdateInfo());
        }

        #region Properties

        public static SketchRepository SketchRepo
        {
            get {
                try
                {
                    if (sketchRepo == null)
                    {
                        sketchRepo = GetSketchRepository();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                }
                return sketchRepo;
            }

            set { SketchRepo = value; }
        }

        public SMParcel MainFormParcel
        {
            get {
                if (mainFormParcel == null)
                {
                    if (SketchUpGlobals.ParcelWorkingCopy == null)
                    {
                        InitializeParcelSnapshots();
                    }
                }
                return mainFormParcel;
            }

            set { mainFormParcel = value; }
        }

        public SMParcelMast MainFormParcelMast { get; set; }

        public int SectionsCount { get; set; }

        public Image SketchImage { get; set; }

        private bool VacantOccupancyCode { get; set; }

        private static SketchRepository sketchRepo;
        private SMParcel mainFormParcel;

        #endregion Properties

        #region SMParcel Initializations

        private static SketchRepository GetSketchRepository()
        {
            try
            {
                return new SketchRepository(SketchUpGlobals.CamraDbConn.DataSource, SketchUpGlobals.CamraDbConn.User, SketchUpGlobals.CamraDbConn.Password, SketchUpGlobals.LocalityPrefix);
            }
            catch (Exception ex)
            {
                string message = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Console.WriteLine(message);
#if DEBUG
                MessageBox.Show(message);
#endif
                throw;
            }
        }

        private SMParcel StoredSMParcel(int record, int dwelling, SketchRepository sr)
        {
            SMParcelMast storedParcelMaster = sr.SelectParcelMasterWithParcel(record, dwelling);
            return storedParcelMaster.Parcel;
        }

        #endregion SMParcel Initializations

        #region Methods

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            UseWaitCursor = false;
            RetrieveAndShowCurrentSketchImage();
            EditImage.Focus();
            BringToFront();
            Visible = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            splash.UpdateProgress();
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

        #endregion Methods

        private Bitmap BadConnectionImage = Properties.Resources.ConnectionOffline_64x;
        private Bitmap ConnectingImage = Properties.Resources.BluePlug;
        private Bitmap GoodConnectionImage = Properties.Resources.CloudOK_32x;
        private MainFormStatus mainStatus;
    }
}