/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/

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
        #region "Constructor"

        public SketchRepository(SMParcel selectedParcel)
        {
            SketchConnection = new SMConnection(SketchUpGlobals.IpAddress, "CAMRA2", "CAMRA2", SketchUpGlobals.LocalityPrefix);
            WorkingParcel = selectedParcel;
        }

        public SketchRepository(string dbDataSource, string dbUserName, string dbPassword, string localityPrefix)
        {
            SketchConnection = new SMConnection(dbDataSource, dbUserName, dbPassword, localityPrefix);
        }

        #endregion "Constructor"

        #region "Public Methods"

        public static string NextLetterFromDb(SWallTech.CAMRA_Connection conn, int record, int card)
        {
            var lastLetter = string.Empty;
            string nextLetter = string.Empty;
            FormattableString sql = $"SELECT JSSECT FROM {SketchUpGlobals.LocalLib}.{SketchUpGlobals.LocalityPrefix}SECTION WHERE JSRECORD = {record} AND JSDWELL = {card} order by jssect desc fetch first row only";

            DataSet ds = conn.DBConnection.RunSelectStatement(sql.ToString());
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

        public SMParcel AddSketchToSnapshots(SMParcel parcel)
        {
            parcel.SnapshotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(parcel);
            return SketchUpGlobals.ParcelWorkingCopy;
        }

        public int DeleteSketch(SMParcel parcel, bool makeVacant = false)
        {
            try
            {
                int linesDeleted = 0;
                int sectionsDeleted = 0;
                int masterDeleted = 0;
                int mastDeleted = 0;
                FormattableString deleteLinesSql = $"DELETE FROM {LineRecordTable} WHERE JLRECORD = {parcel.Record} AND JLDWELL = {parcel.Card}";

                linesDeleted = SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(deleteLinesSql.ToString());
#if DEBUG || TEST
                Trace.WriteLine($"Deleted {linesDeleted} Lines.");
#endif
                FormattableString deleteSectionsSql = $"DELETE FROM {SectionRecordTable} WHERE JSRECORD = {parcel.Record} AND JSDWELL = {parcel.Card}";

                sectionsDeleted = SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(deleteSectionsSql.ToString());
#if DEBUG || TEST
                Trace.WriteLine($"Deleted {sectionsDeleted} Sections.");
#endif
                FormattableString deleteMasterSql = $"DELETE FROM {MasterRecordTable} WHERE JMRECORD = {parcel.Record} AND JMDWELL = {parcel.Card}";

                masterDeleted = SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(deleteMasterSql.ToString());
#if DEBUG || TEST
                Trace.WriteLine($"Deleted {masterDeleted} Master record(s).");
#endif
                if (makeVacant)
                {
                    mastDeleted = MakeParcelVacant(parcel);
#if DEBUG || TEST
                    string wasMadeVacant = mastDeleted > 0 ? "was" : "was not";
                    Trace.WriteLine($"MakeParcelVacant {wasMadeVacant} run.");
#endif
                }
                return linesDeleted;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        public SMParcelMast RevertToSavedParcel(SMParcel parcel)
        {
            SMParcelMast parcelMast = SelectParcelMasterWithParcel(parcel.Record, parcel.Card);
            SketchUpGlobals.SMParcelFromData = parcelMast.Parcel;
            SketchUpGlobals.SketchSnapshots.Clear();
            SketchUpGlobals.SMParcelFromData.SnapshotIndex = 0;
            AddSketchToSnapshots(SketchUpGlobals.SMParcelFromData);
            SketchUpGlobals.ParcelMast = parcelMast;
            WorkingParcel = parcelMast.Parcel;
            SketchUpGlobals.SketchSnapshots.Clear();
            WorkingParcel.SnapshotIndex++;
            AddSketchToSnapshots(WorkingParcel);
            return parcelMast;
        }

        public SMParcelMast SaveCurrentParcel(SMParcel parcel)
        {
            try
            {
                FormattableString resultsMessage = $"";
                WorkingParcel = parcel;

                foreach (SMSection s in parcel.Sections.Where(d => d.DeleteSection))
                {
                    parcel.RemoveSectionFromParcel(s);
                }
                parcel.ReorganizeSections();
                Success = UpdateDatabase(parcel);

                UpdateLivingArea(parcel);
                UpdateTotalArea(parcel);
                if (Success)
                {
                    parcel = SelectParcelMasterWithParcel(SketchUpGlobals.Record, SketchUpGlobals.Card).Parcel;
                }
                return parcel.ParcelMast;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                return parcel.ParcelMast;
            }
        }

        public SMParcelMast SelectParcelMasterWithParcel(int recordNumber, int dwellingNumber)
        {
            var parcelMast = new SMParcelMast();

            FormattableString selectSql = $"SELECT MRECNO AS RECORD , MDWELL AS DWELLING , MOCCUP AS OCCUPANCYCODE , MCLASS  AS PROPERTYCLASS , MGART  AS GARAGE1TYPE , MGAR#C AS GARAGE1NUMCARS , MCARPT AS CARPORTTYPECODE , MCAR#C AS CARPORTNUMCARS , MGART2 AS GARAGE2TYPECODE , MGAR#2 AS GARAGE2NUMCARS ,MBI#C   AS NUMCARSBUILTINCODE ,MSTOR#  AS STORIESVALUE , CASE (TRIM(MSTORY)) WHEN '' THEN CAST(MSTOR# AS CHAR(6)) ELSE MSTORY END        AS STORIESTEXT ,MTOTA   AS TOTALAREA FROM {SketchConnection.MastTable} WHERE MRECNO={recordNumber} AND MDWELL={dwellingNumber}";

            DataSet parcelMastData = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql.ToString());

            int carportTypeCode = 0;
            int carportNumCars = 0;
            int garage1NumCars = 0;
            int garage1TypeCode = 0;
            int garage2NumCars = 0;
            int garage2TypeCode = 0;
            int numCarsBuiltInCode = 0;
            int occupancyCode = 0;
            string propertyClass = string.Empty;
            decimal totalLivingArea = 0.00M;
            double storiesValue;
            string storiesText;
            if (parcelMastData.Tables != null && parcelMastData.Tables.Count > 0 && parcelMastData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in parcelMastData.Tables[0].Rows)
                {
                    //Get numeric values from strings
                    int.TryParse(row["OCCUPANCYCODE"].ToString().Trim(), out occupancyCode);
                    int.TryParse(row["GARAGE1TYPE"].ToString().Trim(), out garage1TypeCode);
                    int.TryParse(row["GARAGE1NUMCARS"].ToString().Trim(), out garage1NumCars);
                    int.TryParse(row["CARPORTTYPECODE"].ToString().Trim(), out carportTypeCode);
                    int.TryParse(row["CARPORTNUMCARS"].ToString().Trim(), out carportNumCars);
                    int.TryParse(row["GARAGE2TYPECODE"].ToString().Trim(), out garage2TypeCode);
                    int.TryParse(row["GARAGE2NUMCARS"].ToString().Trim(), out garage2NumCars);
                    int.TryParse(row["NUMCARSBUILTINCODE"].ToString().Trim(), out numCarsBuiltInCode);
                    double.TryParse(row["STORIESVALUE"].ToString().Trim(), out storiesValue);
                    decimal.TryParse(row["TOTALAREA"].ToString(), out totalLivingArea);
                    storiesText = row["STORIESTEXT"].ToString();
                    propertyClass = row["PROPERTYCLASS"].ToString().Trim();
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
                    parcelMast.StoriesValue = storiesValue;
                    parcelMast.StoriesText = storiesText;
                    parcelMast.TotalArea = totalLivingArea;
                    parcelMast.NumCarsBuiltInCode = numCarsBuiltInCode;
                    WorkingParcel = SelectParcelWithSectionsAndLines(recordNumber, dwellingNumber);
                    WorkingParcel.ParcelMast = parcelMast;
                    parcelMast.Parcel = WorkingParcel;
                }
            }

            return parcelMast;
        }

        #endregion "Public Methods"

        #region "Private Methods"

        private int InsertLines(SMParcel parcel)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            int recordsAffected = 0;
            FormattableString insertLineSql;

            try
            {
                foreach (SMSection s in parcel.Sections.OrderBy(s => s.SectionLetter).Where(s => !s.DeleteSection))
                {
                    foreach (SMLine l in s.Lines.Distinct().OrderBy(l => l.LineNumber))
                    {
                        insertLineSql = $"INSERT INTO {LineRecordTable}(JLRECORD, JLDWELL, JLSECT, JLLINE#, JLDIRECT, JLXLEN, JLYLEN, JLLINELEN, JLANGLE, JLPT1X, JLPT1Y, JLPT2X, JLPT2Y, JLATTACH) VALUES({l.Record}, {l.Card}, '{l.SectionLetter}', {l.LineNumber}, '{l.Direction.ToString()}', {Math.Abs(l.XLength)}, {Math.Abs(l.YLength)}, {Math.Abs(l.LineLength)}, {l.LineAngle}, {l.StartX}, {l.StartY}, {l.EndX}, {l.EndY}, '{l.AttachedSection}')";
#if DEBUG || TEST
                        Trace.WriteLine($"{DateTime.Now}: Insert Line SQL:{insertLineSql}");
#endif
                        recordsAffected += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(insertLineSql.ToString());
                    }
#if DEBUG || TEST
                    Trace.WriteLine($"{DateTime.Now}: Wrote {recordsAffected} lines to section {s.SectionLetter}");
#endif
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
            return recordsAffected;
        }

        private int InsertMaster(SMParcel parcel)
        {
            FormattableString insertMasterSql = $"INSERT INTO {MasterRecordTable}(JMRECORD, JMDWELL, JMSKETCH, JMSTORY, JMSTORYEX, JMSCALE, JMTOTSQFT, JMESKETCH) VALUES ( {parcel.Record}, {parcel.Card}, 'Y', {parcel.Stories},'{parcel.StoryEx}', {parcel.Scale}, {parcel.TotalSqFt},'{parcel.ExSketch}' )";
            return SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(insertMasterSql.ToString());
        }

        private int InsertSections(SMParcel parcel)
        {
            FormattableString insertSectionSql;

            int recordsAffected = 0;
            foreach (SMSection s in parcel.Sections.Where(s => !s.DeleteSection))
            {
                insertSectionSql =
                    $"INSERT INTO {SectionRecordTable}(JSRECORD, JSDWELL, JSSECT, JSTYPE, JSSTORY, JSDESC, JSSKETCH, JSSQFT, JS0DEPR, JSCLASS, JSVALUE, JSFACTOR, JSDEPRC) VALUES({s.Record},{s.Card}, '{s.SectionLetter}', '{s.SectionType}', {s.StoriesValue}, '{s.Description}', '{s.HasSketch}', {s.SqFt}, '{s.ZeroDepr}', '{s.SectionClass}', {s.SectionValue}, {s.AdjFactor}, {s.Depreciation})";
                recordsAffected += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(insertSectionSql.ToString());
            }

            return recordsAffected;
        }

        private int MakeParcelVacant(SMParcel parcel)
        {
            int originalOccCode = SketchUpGlobals.SMParcelFromData.ParcelMast.OccupancyCode;
            int resultsCount = 0;
            int vacantCode = 15;

            DeleteSketch(parcel);
            try
            {
                switch (parcel.ParcelMast.OccupancyType)
                {
                    case CamraDataEnums.OccupancyType.Commercial:
                        vacantCode = (int)CamraDataEnums.VacantOccupancies.VacantCommercial;
                        break;

                    case CamraDataEnums.OccupancyType.TaxExempt:
                        vacantCode = (int)CamraDataEnums.VacantOccupancies.VacantExempt;
                        break;

                    default:
                        vacantCode = (int)CamraDataEnums.VacantOccupancies.VacantLand;

                        break;
                }

                string mastUpdateSql = $"UPDATE {MastRecordTable} SET MOCCUP = {vacantCode} , MSTORY = '' , MAGE = 0 , MCOND = '' , MCLASS = '' , MFACTR = 0 , MDEPRC = 0 , MFOUND = 0 , MEXWLL = 0 , MROOFT = 0 , MROOFG = 0 , M#DUNT = 0 , M#ROOM = 0 , M#BR = 0 , M#FBTH = 0 , M#HBTH = 0 , MSWL = 0 , MFP2 = '' , MHEAT = 0 , MFUEL = 0 , MAC = '' , MFP1 = '' , MEKIT = 0 , MBASTP = 0 , MPBTOT = 0 , MSBTOT = 0 , MPBFIN = 0 , MSBFIN = 0 , MBRATE = 0 , M#FLUE = 0 , MFLUTP = '' , MGART = 0 , MGAR#C = 0 , MCARPT = 0 , MCAR#C = 0 , MBI#C = 0 , MGART2 = 0 , MGAR#2 = 0 , MACPCT = 0 , M0DEPR = '' , MEFFAG = 0 , MFAIRV = 0 , MEXWL2 = 0 , MTBV = 0 , MTBAS = 0 , MTFBAS = 0 , MTPLUM = 0 , MTHEAT = 0 , MTAC = 0 , MTFP = 0 , MTFL = 0 , MTBI = 0 , MTTADD = 0 , MNBADJ = 0 , MTSUBT = 0 , MTOTBV = 0 , MBASA = 0 , MTOTA = 0 , MPSF = 0 , MINWLL = '' , MFLOOR = '' , MYRBLT = 0 , MPCOMP = 0 , MFUNCD = 0 , MECOND = 0 , MIMADJ = 0 , MTBIMP = 0 , MCVEXP = 'Improvement Deleted' , MQAPCH = 0 , MQAFIL = '' , MFP# = 0 , MSFP# = 0 , MFL#= 0 , MSFL# = 0 , MMFL# = 0 , MIOFP# = 0 , MSTOR# = 0 , MOLDOC = @OriginalOccCode , MCVMO = {DateTime.Today.Month.ToString()} , MCVDA = {DateTime.Today.Day.ToString()} , MCVYR = {DateTime.Today.Year.ToString()} WHERE MRECNO = {parcel.Record} AND MDWELL = {parcel.Card}";
                string gasLogUpdateSql = $"UPDATE {SketchUpGlobals.LocalLib}.{SketchUpGlobals.LocalityPrefix}GASLG SET GNOGAS = 0 WHERE GRECNO = {parcel.Record}	AND GDWELL = {parcel.Card}";
                resultsCount += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(gasLogUpdateSql.ToString());
                resultsCount += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(mastUpdateSql.ToString());
                return resultsCount;
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

        private List<SMSection> SelectParcelSections(SMParcel parcel)
        {
            var sections = new List<SMSection>();
            string selectSql = $"SELECT JSRECORD, JSDWELL, JSSECT, JSTYPE, JSSTORY, JSDESC, JSSKETCH, JSSQFT, JS0DEPR, JSCLASS, JSVALUE, JSFACTOR, JSDEPRC FROM {SketchConnection.SectionTable} WHERE JSRECORD={parcel.Record} AND JSDWELL={parcel.Card}";
            try
            {
                double stories = 0.00;
                double value = 0.00;
                double factor = 0.00;
                decimal squareFeet = 0.00M;
                double depreciation = 0.00;
                DataSet sectionsDs = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);
                if (sectionsDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in sectionsDs.Tables[0].Rows)
                    {
                        double.TryParse(row["JSSTORY"].ToString().Trim(), out stories);
                        decimal.TryParse(row["JSSQFT"].ToString().Trim(), out squareFeet);
                        double.TryParse(row["JSVALUE"].ToString().Trim(), out value);
                        double.TryParse(row["JSFACTOR"].ToString().Trim(), out factor);
                        double.TryParse(row["JSDEPRC"].ToString().Trim(), out depreciation);
                        var thisSection = new SMSection(parcel)
                        {
                            Record = parcel.Record,
                            Card = parcel.Card,
                            SectionLetter = row["JSSECT"].ToString().Trim(),
                            SectionType = row["JSTYPE"].ToString().Trim(),
                            StoriesValue = stories,
                            Description = row["JSDESC"].ToString().Trim(),
                            HasSketch = row["JSSKETCH"].ToString().Trim().ToUpper(),
                            SqFt = squareFeet,
                            ZeroDepr = row["JS0DEPR"].ToString().Trim(),
                            SectionClass = row["JSCLASS"].ToString().Trim(),
                            AdjFactor = factor,
                            Depreciation = depreciation,
                            RefreshSection = false,
                            ParentParcel = parcel
                        };
                        if (string.IsNullOrEmpty(thisSection.Description))
                        {
                            thisSection.Description = SketchUpLookups.SectionDescriptionFromType(thisSection.SectionType);
                        }
                        thisSection.Lines = SelectSectionLines(thisSection);
                        sections.Add(thisSection);
                    }
                }

                return sections;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private SMParcel SelectParcelWithSectionsAndLines(int recordNumber, int dwellingNumber)
        {
            string selectSql = $"SELECT JMRECORD, JMDWELL, JMSKETCH, JMSTORY, JMSTORYEX, JMSCALE, JMTOTSQFT, JMESKETCH FROM {SketchConnection.MasterTable} WHERE JMRECORD={recordNumber} AND JMDWELL={dwellingNumber}";
            double stories = 0.00;
            double scale = 1.00;
            decimal totalSqareFeet = 0.00M;

            try
            {
                DataSet parcelData = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);
                if (parcelData.Tables[0].Rows.Count > 0)
                {
                    DataRow row = parcelData.Tables[0].Rows[0];
                    double.TryParse(row["JMSTORY"].ToString(), out stories);
                    double.TryParse(row["JMSCALE"].ToString(), out scale);
                    decimal.TryParse(row["JMTOTSQFT"].ToString(), out totalSqareFeet);
                    var parcel = new SMParcel
                    {
                        Record = recordNumber,
                        Card = dwellingNumber,
                        HasSketch = row["JMSKETCH"].ToString().Trim(),
                        Stories = stories,
                        StoryEx = row["JMSTORYEX"].ToString().Trim(),
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
                    var parcel = new SMParcel
                    {
                        Record = recordNumber,
                        Card = dwellingNumber,
                        HasSketch = "N",
                        Stories = 0,
                        StoryEx = string.Empty,
                        Scale = 1,
                        TotalSqFt = 0,
                        ExSketch = string.Empty
                    };
                    parcel.IdentifyAttachedToSections();
                    return parcel;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private List<SMLine> SelectSectionLines(SMSection section)

        {
            var lines = new List<SMLine>();
            string selectSql = string.Format("SELECT JLRECORD , JLDWELL , JLSECT , JLLINE# , JLDIRECT , JLXLEN , JLYLEN , JLLINELEN , JLANGLE , JLPT1X , JLPT1Y , JLPT2X , JLPT2Y , JLATTACH FROM {0} WHERE JLRECORD={1} AND JLDWELL={2} AND JLSECT='{3}'", SketchConnection.LineTable, section.Record, section.Card, section.SectionLetter);
            try
            {
                int lineNumber = 0;
                double xLen = 0.00;
                double yLen = 0.00;
                double lineLength = 0.00;
                double angle = 0.00;
                double startX = 0.00;
                double endX = 0.00;
                double startY = 0.00;
                double endY = 0.00;

                DataSet linesDs = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql);
                if (linesDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in linesDs.Tables[0].Rows)
                    {
                        int.TryParse(row["JLLINE#"].ToString().Trim(), out lineNumber);
                        double.TryParse(row["JLXLEN"].ToString().Trim(), out xLen);
                        double.TryParse(row["JLYLEN"].ToString().Trim(), out yLen);
                        double.TryParse(row["JLANGLE"].ToString().Trim(), out angle);
                        double.TryParse(row["JLPT1X"].ToString().Trim(), out startX);
                        double.TryParse(row["JLPT1Y"].ToString().Trim(), out startY);
                        double.TryParse(row["JLPT2X"].ToString().Trim(), out endX);
                        double.TryParse(row["JLPT2Y"].ToString().Trim(), out endY);
                        double.TryParse(row["JLLINELEN"].ToString().Trim(), out lineLength);
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
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private void UpdateCarports(SMParcel parcel)
        {
            MessageBox.Show(string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().Name), "Refactoring in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool UpdateDatabase(SMParcel parcel)
        {
            int resultsCount = 0;
            bool updateCompleted = false;
            try
            {
                resultsCount += DeleteSketch(parcel);
                resultsCount += UpdateMastData(parcel);
                resultsCount += InsertMaster(parcel);
                resultsCount += InsertSections(parcel);
                resultsCount += InsertLines(parcel);
                updateCompleted = resultsCount > 0;
                return updateCompleted;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Debug.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
                return updateCompleted;
            }
        }

        private void UpdateLivingArea(SMParcel parcel)
        {
            decimal areaSum = 0.00M;
            decimal dbArea = 0.00M;
            var laTypes = new List<string>();
            laTypes.AddRange((from la in SketchUpLookups.LivingAreaSectionTypeCollection select la._LAattSectionType).ToList());
            decimal.TryParse(parcel.ParcelMast.TotalArea.ToString(), out dbArea);
            foreach (SMSection s in parcel.Sections)
            {
                if (laTypes.Contains(s.SectionType))
                {
                    areaSum += s.SqFt;
                }
            }
            SketchUpGlobals.ParcelWorkingCopy.ParcelMast.TotalArea = areaSum;
        }

        private int UpdateMastData(SMParcel parcel)
        {
            try
            {
                FormattableString updateMastSql = $"UPDATE {MastRecordTable} SET MSTOR#={parcel.ParcelMast.StoriesValue}, MGART={parcel.ParcelMast.Garage1TypeCode}, MGAR#C={parcel.ParcelMast.Garage1NumCars}, MCARPT={parcel.ParcelMast.CarportTypeCode}, MCAR#C={parcel.ParcelMast.CarportNumCars}, MBI#C={parcel.ParcelMast.NumCarsBuiltInCode}, MGART2={parcel.ParcelMast.Garage2TypeCode}, MGAR#2={parcel.ParcelMast.Garage2NumCars}, MTOTA={parcel.ParcelMast.TotalArea},MCLASS='{parcel.ParcelMast.PropertyClass}'  WHERE MRECNO={parcel.ParcelMast.Record} AND MDWELL={parcel.ParcelMast.Card}";
                return SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(updateMastSql.ToString());
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Debug.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private void UpdateTotalArea(SMParcel parcel)
        {
            try
            {
                parcel.TotalSqFt = (from s in parcel.Sections select s.SqFt).Sum();
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Debug.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        #endregion "Private Methods"

        #region "Manual properties"

        public string DataSource { get; set; }

        public string LineRecordTable
        {
            get {
                lineRecordTable = SketchConnection.LineTable;
                return lineRecordTable;
            }

            set { lineRecordTable = value; }
        }

        public string Locality { get; set; }

        public string MasterRecordTable
        {
            get {
                masterRecordTable = SketchConnection.MasterTable;
                return masterRecordTable;
            }

            set { masterRecordTable = value; }
        }

        public string MastRecordTable
        {
            get {
                mastRecordTable = SketchConnection.MastTable;
                return mastRecordTable;
            }

            set { mastRecordTable = value; }
        }

        public string Password { get; set; }

        public string SectionRecordTable
        {
            get {
                sectionRecordTable = SketchConnection.SectionTable;
                return sectionRecordTable;
            }

            set { sectionRecordTable = value; }
        }

        public SMConnection SketchConnection { get; set; }

        public bool Success { get; set; }

        public string UserName { get; set; }

        public SMParcel WorkingParcel { get; set; }

        #endregion "Manual properties"

        #region "Private Fields"

        private string lineRecordTable;
        private string masterRecordTable;
        private string mastRecordTable;
        private string sectionRecordTable;

        #endregion "Private Fields"
    }
}