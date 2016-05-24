using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public class SketchRepository
    {
        #region Constructor

        public SketchRepository(SMParcel selectedParcel)
        {
            workingCopyOfParcel = selectedParcel;
        }

        public SketchRepository(string dbDataSource, string dbUserName, string dbPassword, string localityPrefix)
        {
            SketchConnection = new SMConnection(dbDataSource, dbUserName, dbPassword, localityPrefix);
        }

        #endregion Constructor

        #region DAL Methods

        public void DeleteSketchData(SMParcel parcel)
        {
        }

        #region CRUD methods refactored out of ExpandoSketch Class

        #endregion CRUD methods refactored out of ExpandoSketch Class

        #region Selects for SketchManager Classes

        public SMParcelMast SelectParcelMasterWithParcel(int recordNumber, int dwellingNumber)
        {
            SMParcelMast parcelMast = new SMParcelMast();
            string selectSql = string.Format("SELECT MRECNO AS RECORD, MDWELL AS DWELLING, MOCCUP AS OCCUPANCYCODE,MCLASS  AS PROPERTYCLASS, MGART  AS GARAGE1TYPE, MGAR#C AS GARAGE1NUMCARS, MCARPT AS CARPORTTYPECODE, MCAR#C AS CARPORTNUMCARS, MGART2 AS GARAGE2TYPECODE, MGAR#2 AS GARAGE2NUMCARS,MBI#C AS NUMCARSBUILTINCODE,MSTOR#  AS MASTERPARCELSTOREYS,MTOTA as TOTALAREA  FROM {0} WHERE MRECNO={1} AND MDWELL={2}", SketchConnection.MastTable, recordNumber, dwellingNumber);
            DataSet parcelMastData = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);

            try
            {
                int carportTypeCode = 0;
                int carportNumCars = 0;
                int garage1NumCars = 0;
                int garage1TypeCode = 0;
                int garage2NumCars = 0;
                int garage2TypeCode = 0;
                int numCarsBuiltInCode = 0;
                int occupancyCode = 0;
                int propertyClass = 0;
                decimal totalLivingArea = 0.00M;
                decimal masterParcelStoreys;

                if (parcelMastData.Tables!=null&& parcelMastData.Tables.Count>0&&parcelMastData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in parcelMastData.Tables[0].Rows)
                    {
                        //Get numeric values from strings
                        int.TryParse(row["OCCUPANCYCODE"].ToString().Trim(), out occupancyCode);
                        int.TryParse(row["PROPERTYCLASS"].ToString().Trim(), out propertyClass);
                        int.TryParse(row["GARAGE1TYPE"].ToString().Trim(), out garage1TypeCode);
                        Int32.TryParse(row["GARAGE1NUMCARS"].ToString().Trim(), out garage1NumCars);
                        Int32.TryParse(row["CARPORTTYPECODE"].ToString().Trim(), out carportTypeCode);

                        Int32.TryParse(row["CARPORTNUMCARS"].ToString().Trim(), out carportNumCars);
                        Int32.TryParse(row["GARAGE2TYPECODE"].ToString().Trim(), out garage2TypeCode);
                        Int32.TryParse(row["GARAGE2NUMCARS"].ToString().Trim(), out garage2NumCars);
                        Int32.TryParse(row["NUMCARSBUILTINCODE"].ToString().Trim(), out numCarsBuiltInCode);
                        decimal.TryParse(row["MASTERPARCELSTOREYS"].ToString().Trim(), out masterParcelStoreys);
                        decimal.TryParse(row["TOTALAREA"].ToString(), out totalLivingArea);
                        parcelMast.Record = recordNumber;
                        parcelMast.Card = dwellingNumber;
                        parcelMast.OccupancyCode = occupancyCode;
                        parcelMast.CarportNumCars = carportNumCars;
                        parcelMast.CarportTypeCode = carportTypeCode;
                        parcelMast.Garage1NumCars = garage1NumCars;
                        parcelMast.Garage1TypeCode = garage1TypeCode;
                        parcelMast.Garage2NumCars = garage2NumCars;
                        parcelMast.Garage2TypeCode = garage2TypeCode;
                        parcelMast.PropertyClass = propertyClass;
                        parcelMast.MasterParcelStoreys = masterParcelStoreys;
                        parcelMast.TotalSquareFootage = totalLivingArea;
                        WorkingCopyOfParcel = SelectParcelWithSectionsAndLines(recordNumber, dwellingNumber);
                        WorkingCopyOfParcel.ParcelMast = parcelMast;
                        parcelMast.Parcel = WorkingCopyOfParcel;
                    }
                }
                return parcelMast;
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

        private List<SMSection> SelectParcelSections(SMParcel parcel)
        {
            List<SMSection> sections = new List<SMSection>();
            string selectSql = string.Format("SELECT JSRECORD, JSDWELL, JSSECT, JSTYPE, JSSTORY, JSDESC, JSSKETCH, JSSQFT, JS0DEPR, JSCLASS, JSVALUE, JSFACTOR, JSDEPRC FROM {0} WHERE JSRECORD={1} AND JSDWELL={2}", SketchConnection.SectionTable, parcel.Record, parcel.Card);
            try
            {
                decimal storeys = 0.00M;
                decimal value = 0.00M;
                decimal factor = 0.00M;
                decimal squareFeet = 0.00M;
                decimal depreciation = 0.00M;
                DataSet sectionsDs = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);
                if (sectionsDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in sectionsDs.Tables[0].Rows)
                    {
                        decimal.TryParse(row["JSSTORY"].ToString().Trim(), out storeys);
                        decimal.TryParse(row["JSSQFT"].ToString().Trim(), out squareFeet);
                        decimal.TryParse(row["JSVALUE"].ToString().Trim(), out value);
                        decimal.TryParse(row["JSFACTOR"].ToString().Trim(), out factor);
                        decimal.TryParse(row["JSDEPRC"].ToString().Trim(), out depreciation);
                        SMSection thisSection = new SMSection(parcel)
                        {
                            Record = parcel.Record,
                            Dwelling = parcel.Card,
                            SectionLetter = row["JSSECT"].ToString().Trim(),
                            SectionType = row["JSTYPE"].ToString().Trim(),
                            Storeys = storeys,
                            Description = row["JSDESC"].ToString().Trim(),
                            HasSketch = row["JSSKETCH"].ToString().Trim().ToUpper() == "Y" ? true : false,
                            SqFt = squareFeet,
                            ZeroDepr = row["JS0DEPR"].ToString().Trim(),
                            SectionClass = row["JSCLASS"].ToString().Trim(),
                            SectionValue = value,
                            AdjFactor = factor,
                            Depreciation = depreciation,
                            RefreshSection = false,
                            ParentParcel = parcel
                        };
                        thisSection.Lines = SelectSectionLines(thisSection);
                        sections.Add(thisSection);
                    }
                }

                return sections;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private SMParcel SelectParcelWithSectionsAndLines(int recordNumber, int dwellingNumber)
        {
            string selectSql = string.Format("SELECT JMRECORD, JMDWELL, JMSKETCH, JMSTORY, JMSTORYEX, JMSCALE, JMTOTSQFT, JMESKETCH FROM {0} WHERE JMRECORD={1} AND JMDWELL={2}", SketchConnection.MasterTable, recordNumber, dwellingNumber);
            decimal storeys = 0.00M;
            decimal scale = 1.00M;
            decimal totalSqareFeet = 0.00M;

            try
            {
                DataSet parcelData = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);
                if (parcelData.Tables[0].Rows.Count > 0)
                {
                    DataRow row = parcelData.Tables[0].Rows[0];
                    decimal.TryParse(row["JMSTORY"].ToString(), out storeys);
                    decimal.TryParse(row["JMSCALE"].ToString(), out scale);
                    decimal.TryParse(row["JMTOTSQFT"].ToString(), out totalSqareFeet);
                    SMParcel parcel = new SMParcel
                    {
                        Record = recordNumber,
                        Card = dwellingNumber,
                        HasSketch = row["JMSKETCH"].ToString().Trim(),
                        Storeys = storeys,
                        StoreyEx = row["JMSTORYEX"].ToString().Trim(),
                        Scale = scale,
                        TotalSqFt = totalSqareFeet,
                        ExSketch = row["JMESKETCH"].ToString().Trim()
                    };
                    parcel.Sections = SelectParcelSections(parcel);

                    foreach (SMSection sms in parcel.Sections)
                    {
                        sms.Lines = SelectSectionLines(sms);
                    }
                    return parcel;
                }
                else
                {
                    SMParcel parcel = new SMParcel
                    {
                        Record = recordNumber,
                        Card = dwellingNumber,
                        HasSketch = "N",
                        Storeys = 0,
                        StoreyEx = string.Empty,
                        Scale = 1,
                        TotalSqFt = 0,
                        ExSketch = string.Empty
                    };

                    return parcel;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private List<SMLine> SelectSectionLines(SMSection section)

        {
            List<SMLine> lines = new List<SMLine>();
            string selectSql = string.Format("SELECT JLRECORD , JLDWELL , JLSECT , JLLINE# , JLDIRECT , JLXLEN , JLYLEN , JLLINELEN , JLANGLE , JLPT1X , JLPT1Y , JLPT2X , JLPT2Y , JLATTACH FROM {0} WHERE JLRECORD={1} AND JLDWELL={2} AND JLSECT='{3}'", SketchConnection.LineTable, section.Record, section.Dwelling, section.SectionLetter);
            try
            {
                int lineNumber = 0;
                decimal xLen = 0.00M;
                decimal yLen = 0.00M;
                decimal lineLength = 0.00M;
                decimal angle = 0.00M;
                decimal startX = 0.0000M;
                decimal endX = 0.0000M;
                decimal startY = 0.0000M;
                decimal endY = 0.0000M;

                DataSet linesDs = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);
                if (linesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in linesDs.Tables[0].Rows)
                    {
                        int.TryParse(row["JLLINE#"].ToString().Trim(), out lineNumber);
                        decimal.TryParse(row["JLXLEN"].ToString().Trim(), out xLen);
                        decimal.TryParse(row["JLYLEN"].ToString().Trim(), out yLen);
                        decimal.TryParse(row["JLANGLE"].ToString().Trim(), out angle);
                        decimal.TryParse(row["JLPT1X"].ToString().Trim(), out startX);
                        decimal.TryParse(row["JLPT1Y"].ToString().Trim(), out startY);
                        decimal.TryParse(row["JLPT2X"].ToString().Trim(), out endX);
                        decimal.TryParse(row["JLPT2Y"].ToString().Trim(), out endY);
                        decimal.TryParse(row["JLLINELEN"].ToString().Trim(), out lineLength);
                        lines.Add(new SMLine(section)
                        {
                            LineNumber = lineNumber,
                            Direction = row["JLDIRECT"].ToString().Trim(),
                            XLength = xLen,
                            YLength = yLen,
                            LineLength = lineLength,
                            LineAngle = angle,
                            StartX = startX,
                            StartY = startY,
                            EndX = endX,
                            EndY = endY,
                            AttachedSection = row["JLATTACH"].ToString().Trim(),
                            ParentSection = section,
                            ParentParcel = section.ParentParcel
                        });
                    }
                }
                return lines;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        #endregion Selects for SketchManager Classes

        #endregion DAL Methods
        #region Sketch Management Methods
        private void DeleteSectionAndReorganize(SMParcel parcel,SMSection section)
        {
            try
            {
                List<SMSection> sectionsBeforeDelete = parcel.Sections.OrderBy(l => l.SectionLetter).ToList();

#if DEBUG
                DevUtilities du = new DevUtilities();
                Trace.WriteLine(du.ParcelInfo(parcel));
#endif

                AddSketchToSnapshots(parcel);
                DeleteSelectedSection(parcel,section);
#if DEBUG
                
                Trace.WriteLine(du.ParcelInfo(parcel));
#endif

                ReorganizeParcel(parcel, sectionsBeforeDelete);
#if DEBUG
                
                Trace.WriteLine(du.ParcelInfo(parcel));
                Trace.Flush();
                Trace.Close();
#endif


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

        private void ReorganizeParcel(SMParcel parcel, List<SMSection> sectionsBeforeDelete)
        {
            List<SMSection> parcelSections = parcel.Sections.OrderBy(l => l.SectionLetter).ToList();
            #if DEBUG
                DevUtilities du = new DevUtilities();
                Trace.WriteLine(du.ParcelInfo(parcel));
#endif

            foreach (SMSection sec in parcelSections)
            {

            }
        }

        public void DeleteSelectedSection(SMParcel parcel, SMSection section)
        {
            
            if (section.SectionLetter == "A")
            {
                string message = "You are removing the base section! You must have a section \"A\" in place. Removing this section will remove the entire sketch, and you will have to redraw it.\n\nProceed?";
                string title = "Delete Base Section A?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Warning;
                MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
                DialogResult response = MessageBox.Show(message, title, buttons, icon, defButton);
                switch (response)
                {

                    case DialogResult.Yes:
                        DeleteSketch();
                        break;
                    case DialogResult.No:
                    default:
                        //Do not alter the sketch
                        break;
                }
            }
            else
            {

                parcel.Sections.Remove(section);

            }
        }

        private void DeleteSketch()
        {
                       string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }
        #endregion
        #region Fields

        private string dataSource;
        private string lineRecordTable;
        private string locality;
        private string masterRecordTable;
        private string mastRecordTable;
        private string password;
        private string sectionRecordTable;
        private SMConnection sketchConnection;
        private string userName;
        private SMParcel workingCopyOfParcel;

        #endregion Fields

        #region Properties

        public static string NextLetter(string lastLetter = "A")
        {
            string nextLetter = string.Empty;
            switch (lastLetter)
            {
                case "A":
                    nextLetter = "B";
                    break;

                case "B":
                    nextLetter = "C";
                    break;

                case "C":
                    nextLetter = "D";
                    break;

                case "D":
                    nextLetter = "F";
                    break;

                case "F":
                    nextLetter = "G";
                    break;

                case "G":
                    nextLetter = "H";
                    break;

                case "H":
                    nextLetter = "I";
                    break;

                case "I":
                    nextLetter = "J";
                    break;

                case "J":
                    nextLetter = "K";
                    break;

                case "K":
                    nextLetter = "L";
                    break;

                case "L":
                    nextLetter = "M";
                    break;

                default:
                    nextLetter = string.Empty;
                    break;
            }
            return nextLetter;
        }

        public static string NextLetter(SWallTech.CAMRA_Connection conn, int record, int card)
        {
            var lastLetter = string.Empty;
            string nextLetter = string.Empty;
            string sql = string.Format("SELECT JSSECT FROM \"NATIVE\".\"{0}SECTION\" WHERE JSRECORD = {1} AND JSDWELL = {2} order by jssect desc fetch first row only", conn.LocalityPrefix, record, card);

            DataSet ds = conn.DBConnection.RunSelectStatement(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                lastLetter = dr["JSSECT"].ToString().Trim().ToUpper();
            }
            if (lastLetter != null)
            {
                switch (lastLetter)
                {
                    case "A":
                        nextLetter = "B";
                        break;

                    case "B":
                        nextLetter = "C";
                        break;

                    case "C":
                        nextLetter = "D";
                        break;

                    case "D":
                        nextLetter = "F";
                        break;

                    case "F":
                        nextLetter = "G";
                        break;

                    case "G":
                        nextLetter = "H";
                        break;

                    case "H":
                        nextLetter = "I";
                        break;

                    case "I":
                        nextLetter = "J";
                        break;

                    case "J":
                        nextLetter = "K";
                        break;

                    case "K":
                        nextLetter = "L";
                        break;

                    case "L":
                        nextLetter = "M";
                        break;

                    default:
                        nextLetter = string.Empty;
                        break;
                }
            }
            else
            {
                nextLetter = "A";
            }
            return nextLetter;
        }

        public string DataSource
        {
            get
            {
                return dataSource;
            }

            set
            {
                dataSource = value;
            }
        }

        public string LineRecordTable
        {
            get
            {
                lineRecordTable = SketchConnection.LineTable;
                return lineRecordTable;
            }

            set
            {
                lineRecordTable = value;
            }
        }

        public string Locality
        {
            get
            {
                return locality;
            }

            set
            {
                locality = value;
            }
        }

        public string MasterRecordTable
        {
            get
            {
                masterRecordTable = SketchConnection.MasterTable;
                return masterRecordTable;
            }

            set
            {
                masterRecordTable = value;
            }
        }

        public string MastRecordTable
        {
            get
            {
                mastRecordTable = SketchConnection.MastTable;
                return mastRecordTable;
            }

            set
            {
                mastRecordTable = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string SectionRecordTable
        {
            get
            {
                sectionRecordTable = SketchConnection.SectionTable;
                return sectionRecordTable;
            }

            set
            {
                sectionRecordTable = value;
            }
        }

        public SMConnection SketchConnection
        {
            get
            {
                return sketchConnection;
            }

            set
            {
                sketchConnection = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public SMParcel WorkingCopyOfParcel
        {
            get
            {
                return workingCopyOfParcel;
            }

            set
            {
                workingCopyOfParcel = value;
            }
        }

        public SMParcel AddSketchToSnapshots(SMParcel parcel)
        {
            parcel.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(parcel);
            return SketchUpGlobals.ParcelWorkingCopy;
        }

        #endregion Properties
    }
}