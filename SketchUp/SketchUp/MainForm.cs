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
    //Refactored stringbuilders to strings and extracted long code runs into separate methods. JMM Feb 2016
    public partial class MainForm : Form
    {
        #region Constructor

        public MainForm()
        {
            if (Program.commandLineArgs != null && Program.commandLineArgs.Library != null && !string.IsNullOrEmpty(Program.commandLineArgs.Library))

            {
                LoadDataFromCamraDb();
                PrepareSketchManager();
                ShowDbVersionOfSketch();
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
                LoadDataFromCamraDb();
                PrepareSketchManager();
                ShowDbVersionOfSketch();
            }
            else
            {
                ShowUpdateMessage();
            }
        }

        public void LoadDataFromCamraDb()
        {
            InitializeComponent();
            timer.Start();
            splash = ShowSplashScreen();
            splash.UpdateProgress(10);
            Application.DoEvents();
            SketchUpGlobals.MainFormIsMinimized = false;
            splash.UpdateProgress(20);
            Application.DoEvents();

            try
            {
                SketchUpGlobals.DbAccessMgr = null;
                splash.UpdateProgress(30);
                Application.DoEvents();
                ParseArgsToProperties(Program.commandLineArgs);
                splash.UpdateProgress(45);
                Application.DoEvents();
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
                SketchUpGlobals.SketchSnapshots.Clear();
                sketchRepo = GetSketchRepository();
                SketchUpGlobals.SMParcelFromData = StoredSMParcel(SketchUpGlobals.Record,
                SketchUpGlobals.Card, sketchRepo);
                SketchUpGlobals.SMParcelFromData.SnapShotIndex = 0;
                SketchUpGlobals.SketchSnapshots.Add(SketchUpGlobals.SMParcelFromData);
                mainFormParcel = SketchUpGlobals.SMParcelFromData;
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

        private void PrepareSketchManager()
        {
            InitializeParcelSnapshots();
            splash.UpdateProgress(100);
            Application.DoEvents();
        }

        private void ShowDbVersionOfSketch()
        {
            SMSketcher sketcher = new SMSketcher(SketchUpGlobals.SMParcelFromData,  sketchBox);
            sketcher.SketchFont = new Font("Segue UI", 6);
            sketcher.PenColor = Color.Blue;
            sketcher.SketchPen = new Pen(sketcher.PenColor, 2);
            sketcher.RenderSketch(true);
            sketchBox.Image = sketcher.SketchImage;
        }

        #endregion Constructor

        #region fields

        private FormSplash splash;

        #endregion fields

        #region Form Control Methods

        //        private static int CountSectionsInDb()
        //        {
        //            try
        //            {
        //                StringBuilder cntSect = new StringBuilder();
        //                cntSect.Append(String.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
        //                            SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, SketchUpGlobals.CurrentParcel.Record, SketchUpGlobals.CurrentParcel.Card));

        //                int SectionCnt = Convert.ToInt32(SketchUpGlobals.CamraDbConn.DBConnection.ExecuteScalar(cntSect.ToString()));
        //                return SectionCnt;
        //            }
        //            catch (Exception ex)
        //            {
        //                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
        //                Trace.WriteLine(errMessage);
        //                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
        //#if DEBUG

        //                MessageBox.Show(errMessage);
        //#endif
        //                throw;
        //            }
        //        }

        private void AddWorkingCopyOfSketchToSnapshots()
        {
            try
            {
                SMParcel parcel = MainFormParcel;
                parcel.SnapShotIndex++;
                SketchUpGlobals.SketchSnapshots.Add(parcel);
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

        private void EditImage_Click(object sender, EventArgs e)
        {
            try
            {
                AddWorkingCopyOfSketchToSnapshots();

                GetSelectedImages();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        private void GetSelectedImages()
        {
            //This is the legacy code getting all the master data, rat tables,etc.
            /* Trying to eliminate anything that is extraneous
            SketchUpGlobals.SubSections = new SectionDataCollection(SketchUpGlobals.CamraDbConn, SketchUpGlobals.CurrentParcel.mrecno, SketchUpGlobals.CurrentParcel.mdwell);

            SketchUpGlobals.CurrentParcel.BuildSketchData();
            getSketch(SketchUpGlobals.CurrentParcel.Record, SketchUpGlobals.CurrentParcel.Card);

            //Ask Dave why this happens twice

            SketchUpGlobals.CurrentSketchImage = SketchUpGlobals.CurrentParcel.GetSketchImage(374);
            SketchImage= SketchUpGlobals.CurrentSketchImage;
            sketchBox.Image = SketchImage;
            */
            SMSketcher sketcher = new SMSketcher(MainFormParcel, sketchBox);
            sketcher.PenColor = Color.Blue;
            sketcher.SketchPen = new Pen(sketcher.PenColor, 5);
            sketcher.RenderSketch();
            SketchUpGlobals.SketchImage = sketcher.SketchImage;
            if (EditImage.Text == "Add Sketch")
            {
                EditImage.Text = "Edit Sketch";
            }

            ClearX();

            int SectionCnt = MainFormParcel.Sections.Count;

            bool omitSketch = false;

            if (SectionCnt == 0)
            {
                SketchUpGlobals.HasNewSketch = false;
                SketchUpGlobals.HasSketch = false;
                DialogResult result;
                result = (MessageBox.Show("Add Sketch?", "Sketch Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question));

                if (result == DialogResult.Yes)
                {
                    //BuildNewSketch();

                    EditImage.Text = "Edit Sketch";

                    SketchUpGlobals.HasNewSketch = true;
                }
                if (result == DialogResult.No)
                {
                    omitSketch = true;
                }
            }
            SetTextAddOrEdit(SectionCnt);
            bool doSketch = !(omitSketch || ExpandoSketch._cantSketch);
            if (doSketch)
            {
                ExpandoSketch skexp = new ExpandoSketch(SketchUpGlobals.CurrentParcel, SketchUpGlobals.SketchFolder, SketchUpGlobals.CurrentParcel.mrecno.ToString(), SketchUpGlobals.CurrentParcel.mdwell.ToString(),
                   SketchUpGlobals.FcLocalityPrefix, SketchUpGlobals.CamraDbConn, SketchUpGlobals.SubSections, SketchUpGlobals.HasSketch, SketchUpGlobals.SketchImage, SketchUpGlobals.HasNewSketch);

                skexp.ShowDialog(this);

                if (ExpandoSketch._isClosed == false && ExpandoSketch._deleteThisSketch == true)
                {
                    CleanUpSketch();
                }

                RetrieveAndShowCurrentSketchImage();
            }

            if (ExpandoSketch._deleteMaster == true)
            {
                EditImage.Text = "Add Sketch";
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            RetrieveAndShowCurrentSketchImage();
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

        private void RetrieveAndShowCurrentSketchImage()
        {
            MainFormParcel = SketchUpGlobals.ParcelWorkingCopy;
            SMSketcher sms = new SMSketcher(MainFormParcel, sketchBox);
            sms.RenderSketch();
            SketchUpGlobals.SketchImage = sms.SketchImage;
            sketchBox.Image = sms.SketchImage;
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
                MessageBox.Show(String.Format("Now Working on New Record - {0}  Old Record was - {1}", SketchUpGlobals.Record, SketchUpGlobals.InitalRecord));

                SketchUpGlobals.InitalRecord = SketchUpGlobals.Record;
                SketchUpGlobals.InitalCard = SketchUpGlobals.Card;
                GetNewSMParcel(SketchUpGlobals.Record, SketchUpGlobals.Card);
                CheckGoodRecord(SketchUpGlobals.Record, SketchUpGlobals.Card);

                if (SketchUpGlobals.Checker > 0)
                {
                    AddSketchToParcel();
                }
            }
        }

        private void GetNewSMParcel(int record, int card)
        {
            MainFormParcel=SketchRepo.SelectParcelWithSectionsAndLines(record, card);
            SketchUpGlobals.SketchSnapshots.Clear();
            MainFormParcel.SnapShotIndex = 0;
            SketchUpGlobals.SketchSnapshots.Add(MainFormParcel);
            SketchUpGlobals.SMParcelFromData = MainFormParcel;
        }

        private void SelectRecordBtn_Click(object sender, EventArgs e)
        {
            SelectRecord();
        }

        #endregion Form Control Methods

        #region Private Methods

        public void LoadInitialSketch()
        {
            SketchUpGlobals.CurrentParcel = ParcelData.getParcel(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);

            SketchUpGlobals.SubSections = new SectionDataCollection(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);
            FixLength(SketchUpGlobals.Record, SketchUpGlobals.Card);
            int tr = SketchUpGlobals.CurrentParcel.mrecno;
            ClearX();
            int seccnt = SectionCount();
            Application.DoEvents();
            SetTextAddOrEdit(seccnt);
            Application.DoEvents();
            if (CamraSupport.VacantOccupancies.Contains(SketchUpGlobals.CurrentParcel.moccup))
            {
                int gar1cde = 0;
                int gar1cnt = 0;
                int gar2cde = SketchUpGlobals.CurrentParcel.mgart2;
                int gar2cnt = SketchUpGlobals.CurrentParcel.mgarN2;
                int cpcde = 0;
                int cpcnt = 0;
                int bicnt = 0;

                if (SketchUpGlobals.CurrentParcel.mgart2 != 64)
                {
                    gar2cde = 0;
                    gar2cnt = 0;
                }
                Application.DoEvents();
                UpdateGarageAndCarportNumbersInDb(gar1cde, gar1cnt, gar2cde, gar2cnt, cpcde, cpcnt, bicnt);
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
                getSketch(SketchUpGlobals.CurrentParcel.mrecno, SketchUpGlobals.CurrentParcel.mdwell);
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

            SketchUpGlobals.CurrentSketchImage = SketchUpGlobals.CurrentParcel.GetSketchImage(374);
            sketchBox.Image = SketchUpGlobals.CurrentSketchImage;
        }

        public void ParseArgsToProperties(CommandLineArguments args)
        {
            SketchUpGlobals.LocalLib = args.Library;
            SketchUpGlobals.LocalityPreFix = args.Locality;
            SketchUpGlobals.Record = args.Record;
            SketchUpGlobals.Card = args.Card;
            SketchUpGlobals.IpAddress = args.IPAddress;
            SketchUp.Properties.Settings.Default.IPAddress = SketchUpGlobals.IpAddress;
        }

        //TODO: Make private again for release
        public FormSplash ShowSplashScreen()
        {
            FormSplash splash = new FormSplash();
            splash.Show();
            splash.Update();
            Cursor = Cursors.WaitCursor;
            return splash;
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
            }
        }

        private static void DeletePartialSectionInDb()
        {
            StringBuilder cleanup = new StringBuilder();
            cleanup.Append(String.Format("delete from {0}.{1}line where jlsect = '{2}' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, ExpandoSketch.NextSectLtr));
            cleanup.Append(String.Format(" and jlrecord = {0} and jldwell = {1} ",
                        SketchUpGlobals.CurrentParcel.mrecno,
                        SketchUpGlobals.CurrentParcel.mdwell));

            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(cleanup.ToString());
        }

        private static int MasterRecordCount()
        {
            StringBuilder checkMain = new StringBuilder();
            checkMain.Append(String.Format("select count(*) from {0}.{1}mast where mrecno = {2} and mdwell = {3} ",
                        SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, SketchUpGlobals.Record, SketchUpGlobals.Card));

            int recordCount = Convert.ToInt32(SketchUpGlobals.CamraDbConn.DBConnection.ExecuteScalar(checkMain.ToString()));
            return recordCount;
        }

        private static void SetFCValues(string IP)
        {
            SketchUpGlobals.FcLib = Program.commandLineArgs.Library;
            SketchUpGlobals.FcLocalityPrefix = Program.commandLineArgs.Locality;

            SketchUpGlobals.FcCard = Program.commandLineArgs.Card;
            SketchUpGlobals.FcIpAddress = Program.commandLineArgs.IPAddress;
            SketchUpGlobals.FcRecord = Program.commandLineArgs.Record;
            SketchUpGlobals.InitalRecord = Program.commandLineArgs.Record;
            SketchUpGlobals.InitalCard = Program.commandLineArgs.Card;
            SketchUpGlobals.LocalityPreFix = Program.commandLineArgs.Locality;
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
                        SketchUpGlobals.CurrentParcel.mrecno,
                        SketchUpGlobals.CurrentParcel.mdwell);
            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(clrvac);
        }

        private void AddSketchToParcel()
        {
            SketchUpGlobals.CurrentParcel = ParcelData.getParcel(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);

            SketchUpGlobals.SubSections = new SectionDataCollection(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);

           
            if (CamraSupport.VacantOccupancies.Contains(SketchUpGlobals.CurrentParcel.moccup))
            {
                MessageBox.Show("Can't Add Sketch to Vacant Parcel...Add Master Record Data!");
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                SetTextAddOrEdit(SketchUpGlobals.SubSections.Count);
            }
            try
            {
                getSketch(SketchUpGlobals.CurrentParcel.mrecno, SketchUpGlobals.CurrentParcel.mdwell);

            }
            catch
            {
            }
            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch(true);
            SketchUpGlobals.SketchImage = sms.SketchImage;
            SketchUpGlobals.CurrentSketchImage = SketchUpGlobals.CurrentParcel.GetSketchImage(374);
            sketchBox.Image = SketchUpGlobals.SketchImage;
        }

        private void CardTxt_Leave(object sender, EventArgs e)
        {
            int card = 0;
            int.TryParse(CardTxt.Text, out card);

            SketchUpGlobals.Card = card;
        }

        private void CheckGoodRecord(int _record, int Card)
        {
            SketchUpGlobals.Checker = MasterRecordCount();

            if (SketchUpGlobals.Checker == 0)
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
            countSec.Append(String.Format("select count(*) from {0}.{1}line where jlsect = '{2}' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, ExpandoSketch.NextSectLtr));

            int sectcount = Convert.ToInt32(SketchUpGlobals.CamraDbConn.DBConnection.ExecuteScalar(countSec.ToString()));

            if (sectcount > 0)
            {
                Cursor = Cursors.WaitCursor;

                DeletePartialSectionInDb();
                DeletePartialSectionInModels();
                Cursor = Cursors.Default;
            }
        }

        private void ClearX()
        {
            string clrx = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, SketchUpGlobals.CurrentParcel.mrecno, SketchUpGlobals.CurrentParcel.mdwell);

            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(clrx);
        }

        private void ConnectToCamra()
        {
            if (string.IsNullOrEmpty(SketchUpGlobals.IpAddress))
            {
                SketchUpGlobals.IpAddress = Program.commandLineArgs.IPAddress;
            }
            if (string.IsNullOrEmpty(SketchUp.Properties.Settings.Default.IPAddress))
            {
                SketchUp.Properties.Settings.Default.IPAddress = SketchUpGlobals.IpAddress;
            }

            bool goodIP = System.Text.RegularExpressions.Regex.IsMatch(SketchUpGlobals.IpAddress, SWallTech.RegexPatterns.IPAddressRegexPattern) && !string.IsNullOrEmpty(SketchUpGlobals.IpAddress);

            if (goodIP)
            {
                SketchUpGlobals.CamraDbConn = new CAMRA_Connection
                {
                    DataSource = SketchUp.Properties.Settings.Default.IPAddress,

                    User = SketchUp.Properties.Settings.Default.UserName,
                    Password = SketchUp.Properties.Settings.Default.Password
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

        private void DeletePartialSectionInModels()
        {
            try
            {
                SMParcel parcel = SketchUpGlobals.ParcelWorkingCopy;
                SMParcel original = SketchUpGlobals.SMParcelFromData;
                if (parcel.Sections.Count > original.Sections.Count)
                {
                    string partialSection = parcel.LastSectionLetter;
                    List<SMSection> sectionsAdded = (from s in parcel.Sections where !original.Sections.Contains(s) select s).ToList();
                    foreach (SMSection s in sectionsAdded)
                    {
                        s.Lines.RemoveAll(l => l.SectionLetter == s.SectionLetter);
                        parcel.Sections.RemoveAll(a => sectionsAdded.Contains(a));
                    }

                    parcel.SnapShotIndex++;
                    SketchUpGlobals.SketchSnapshots.Add(parcel);
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
                SketchUpGlobals.SketchFolder = String.Format(@"{0}:\{1}\{2}\new_Sketch",
                                "C",
                                SketchUpGlobals.FcLib,
                                SketchUpGlobals.FcLocalityPrefix);
            }
        }

        private void FixLength(int Record, int Dwell)
        {
            StringBuilder fixNS = new StringBuilder();
            fixNS.Append(String.Format("update {0}.{1}line set jllinelen = jlylen where jlrecord = {2} and jldwell = {3} ",
                                SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, Record, Dwell));
            fixNS.Append("and jldirect in ( 'N','S' ) and jlylen <> jllinelen ");
            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(fixNS.ToString());

            StringBuilder fixEW = new StringBuilder();
            fixEW.Append(String.Format("update {0}.{1}line set jllinelen = jlxlen where jlrecord = {2} and jldwell = {3} ",
                                SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, Record, Dwell));
            fixEW.Append("and jldirect in ( 'E','W' ) and jlxlen <> jllinelen ");
            SketchUpGlobals.CamraDbConn.DBConnection.ExecuteNonSelectStatement(fixEW.ToString());
        }

        private void getSketch(int newRecord, int newCard)
        {
            string newRecordString = newRecord.ToString().PadLeft(7, '0');
            string newCardString = newCard.ToString().PadLeft(2, '0');
            string skFolderPath = String.Format(@"{0}:\{1}\{2}\{3}",
                   "C",
                 SketchUpGlobals.FcLib,
                                SketchUpGlobals.FcLocalityPrefix,
            "new_Sketch");

            if (Directory.Exists(skFolderPath))
            {
                SketchUpGlobals.SketchFolder = skFolderPath;

                sketchBox.Image = null;

                DirectoryInfo dir = new DirectoryInfo(SketchUpGlobals.SketchFolder);
                string pattern = String.Format("{0}_{1}.JPG",
                    newRecordString,
                    newCardString);

                string sketchpath = string.Format("{0}/{1}",
               SketchUpGlobals.SketchFolder,
               pattern);

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

                SetSketchBoxValues(ref sketchpath);
            }
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

        private int SectionCount()
        {
            string checkSect = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, SketchUpGlobals.CurrentParcel.mrecno, SketchUpGlobals.CurrentParcel.mdwell);

            int seccnt = Convert.ToInt32(SketchUpGlobals.CamraDbConn.DBConnection.ExecuteScalar(checkSect));
            return seccnt;
        }

        private void SetConnectionLibraryParameters(DBAccessManager _db)
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

                SketchUpGlobals.LocalityPreFix = SketchUpGlobals.FcLocalityPrefix;

                SketchUpGlobals.CamraDbConn.DBConnection.DataSource = SketchUpGlobals.IpAddress;

                RecordTxt.Text = SketchUpGlobals.FcRecord.ToString();
                CardTxt.Text = SketchUpGlobals.FcCard.ToString();

                SketchUpGlobals.CamraDbConn.LocalityPrefix = SketchUpGlobals.LocalityPreFix;
                SketchUpGlobals.LocalityDescription = SketchUpGlobals.CamraDbConn.Localities.GetLocalityName(SketchUpGlobals.LocalityPreFix);

                LocNameTxt.Text = SketchUpGlobals.LocalityDescription;
                LibraryTxt.Text = SketchUpGlobals.LocalLib;
                PreFixTxt.Text = SketchUpGlobals.LocalityPreFix;

                CheckGoodRecord(SketchUpGlobals.Record, SketchUpGlobals.Card);
                CamraSupport.Init(SketchUpGlobals.CamraDbConn);
                Application.DoEvents();
                if (SketchUpGlobals.Checker > 0)
                {
                    Application.DoEvents();
                    LoadInitialSketch();

                    if (SketchUpGlobals.LocalLib == String.Empty)
                    {
                        MessageBox.Show("Invalid Library & File Information");
                    }
                }

                SketchUpGlobals.Today = DateTime.Today;

                SketchUpGlobals.Year = DateTime.Today.Year;
                SketchUpGlobals.Month = DateTime.Today.Month;
                int todayDayNumber = DateTime.Today.Day;

                Cursor = Cursors.Arrow;
            }

            Application.DoEvents();
            splash.Close();
        }

        private void SetSketchBoxValues(ref string sketchpath)
        {
            SketchUpGlobals.CurrentSketchImage = SketchUpGlobals.CurrentParcel.GetSketchImage(374);

            if (File.Exists(sketchpath) && SketchUpGlobals.HasSketch == true)
            {
                SketchImage = sketchpath.GetImage();

                sketchBox.Image = SketchImage;
            }
            else
            {
                sketchBox.Image = SketchUpGlobals.CurrentSketchImage;
            }
        }

        private void SetTextAddOrEdit(int sectionCount)
        {
            if (!CamraSupport.VacantOccupancies.Contains(SketchUpGlobals.CurrentParcel.moccup))
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

        private void ShowUpdateMessage()
        {
            Application.Run(new UpdateInfo());
        }

        #endregion Private Methods

        #region Properties

        public SMParcel MainFormParcel
        {
            get
            {
                if (mainFormParcel == null)
                {
                    if (SketchUpGlobals.ParcelWorkingCopy == null)
                    {
                        InitializeParcelSnapshots();
                    }
                    else
                    {
                        mainFormParcel = SketchUpGlobals.ParcelWorkingCopy;
                    }
                }
                return mainFormParcel;
            }

            set
            {
                mainFormParcel = value;
            }
        }

        public Image SketchImage
        {
            get
            {
                return sketchImage;
            }

            set
            {
                sketchImage = value;
            }
        }

        public SketchRepository SketchRepo
        {
            get
            {
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

            set
            {
                sketchRepo = value;
            }
        }

        private SMParcel mainFormParcel;

        private Image sketchImage;
        private SketchRepository sketchRepo;

        #endregion Properties

        #region SMParcel Initializations

       

        private SketchRepository GetSketchRepository()
        {
            try
            {
                SketchRepository sr = new SketchRepository(SketchUpGlobals.CamraDbConn.DataSource, SketchUpGlobals.CamraDbConn.User, SketchUpGlobals.CamraDbConn.Password, SketchUpGlobals.LocalityPreFix);
                return sr;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Console.WriteLine(message);
#if DEBUG
                MessageBox.Show(message);
#endif
                throw;
            }
        }

        private SMParcel StoredSMParcel(int record, int dwelling, SketchRepository sr)
        {
            SMParcel parcel = sr.SelectParcelWithSectionsAndLines(record, dwelling);
            parcel.IdentifyAttachedToSections();
            return parcel;
        }

        #endregion SMParcel Initializations

        private void sketchBox_MouseMove(object sender, MouseEventArgs e)
        {
            statusMessage.Text = string.Format("{0},{1}", e.X, e.Y);
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
    }
}