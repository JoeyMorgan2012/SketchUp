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
            workingParcel = selectedParcel;
        }

        public SketchRepository(string dbDataSource, string dbUserName, string dbPassword, string localityPrefix)
        {
            SketchConnection = new SMConnection(dbDataSource, dbUserName, dbPassword, localityPrefix);
        }

#endregion

#region "Public Methods"

        public SMParcel AddSketchToSnapshots(SMParcel parcel)
        {
            parcel.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(parcel);
            return SketchUpGlobals.ParcelWorkingCopy;
        }

        public int DeleteSketch(SMParcel parcel)
        {
            try
            {
                int resultsCount = 0;
                FormattableString deleteLinesSql = $"DELETE FROM {LineRecordTable} WHERE JLRECORD = {parcel.Record} AND JLDWELL = {parcel.Card}";

                resultsCount += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(deleteLinesSql.ToString());

                FormattableString deleteSectionsSql = $"DELETE FROM {SectionRecordTable} WHERE JSRECORD = {parcel.Record} AND JSDWELL = {parcel.Card}";

                resultsCount += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(deleteSectionsSql.ToString());

                FormattableString deleteMasterSql = $"DELETE FROM {MasterRecordTable} WHERE JMRECORD = {parcel.Record} AND JMDWELL = {parcel.Card}";

                resultsCount += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(deleteMasterSql.ToString());
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
                throw;
            }
        }

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

        public SMParcelMast SaveCurrentParcel(SMParcel parcel)
        {

            try
            {
                bool success = false;
                FormattableString resultsMessage = $"";
                workingParcel = parcel;
                UpdateLivingArea(parcel);
                UpdateTotalArea(parcel);

                success = UpdateDatabase(parcel);

                if (success)
                {
                    resultsMessage = $"Parcel {parcel.Record} Dwelling #{parcel.Card} was successfully updated.";
                    MessageBox.Show(resultsMessage.ToString(), "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    resultsMessage = $"An error occurred while saving Parcel {parcel.Record} Dwelling #{parcel.Card}.";
                    MessageBox.Show(resultsMessage.ToString(), "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                SMParcelMast parcelMast = RefreshWorkingCopyFromDb(parcel);
                return parcelMast;
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
            SMParcelMast parcelMast = new SMParcelMast();
            FormattableString selectSql = $"SELECT MRECNO AS RECORD , MDWELL AS DWELLING , MOCCUP AS OCCUPANCYCODE , MCLASS  AS PROPERTYCLASS , MGART  AS GARAGE1TYPE , MGAR#C AS GARAGE1NUMCARS , MCARPT AS CARPORTTYPECODE , MCAR#C AS CARPORTNUMCARS , MGART2 AS GARAGE2TYPECODE , MGAR#2 AS GARAGE2NUMCARS ,MBI#C   AS NUMCARSBUILTINCODE ,MSTOR#  AS STOREYSVALUE , CASE (TRIM(MSTORY)) WHEN '' THEN CAST(MSTOR# AS CHAR(6)) ELSE MSTORY END        AS STOREYSTEXT ,MTOTA   AS TOTALAREA FROM {SketchConnection.MastTable} WHERE MRECNO={recordNumber} AND MDWELL={dwellingNumber}";
            DataSet parcelMastData = SketchConnection.DbConn.DBConnection.RunSelectStatement(selectSql.ToString());

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
            decimal storeysValue;
            string storeysText;
            if (parcelMastData.Tables != null && parcelMastData.Tables.Count > 0 && parcelMastData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in parcelMastData.Tables[0].Rows)
                {
                    //Get numeric values from strings
                    int.TryParse(row["OCCUPANCYCODE"].ToString().Trim(), out occupancyCode);
                    int.TryParse(row["PROPERTYCLASS"].ToString().Trim(), out propertyClass);
                    int.TryParse(row["GARAGE1TYPE"].ToString().Trim(), out garage1TypeCode);
                    int.TryParse(row["GARAGE1NUMCARS"].ToString().Trim(), out garage1NumCars);
                    int.TryParse(row["CARPORTTYPECODE"].ToString().Trim(), out carportTypeCode);
                    int.TryParse(row["CARPORTNUMCARS"].ToString().Trim(), out carportNumCars);
                    int.TryParse(row["GARAGE2TYPECODE"].ToString().Trim(), out garage2TypeCode);
                    int.TryParse(row["GARAGE2NUMCARS"].ToString().Trim(), out garage2NumCars);
                    int.TryParse(row["NUMCARSBUILTINCODE"].ToString().Trim(), out numCarsBuiltInCode);
                    decimal.TryParse(row["STOREYSVALUE"].ToString().Trim(), out storeysValue);
                    decimal.TryParse(row["TOTALAREA"].ToString(), out totalLivingArea);
                    storeysText = row["STOREYSTEXT"].ToString();
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
                    parcelMast.StoreysValue = storeysValue;
                    parcelMast.StoreysText = storeysText;
                    parcelMast.TotalArea = totalLivingArea;
                    parcelMast.NumCarsBuiltInCode = numCarsBuiltInCode;
                    WorkingParcel = SelectParcelWithSectionsAndLines(recordNumber, dwellingNumber);
                    WorkingParcel.ParcelMast = parcelMast;
                    parcelMast.Parcel = WorkingParcel;
                }
            }

            return parcelMast;
        }

#endregion

#region "Private methods"

        private int InsertLines(SMParcel parcel)
        {
            int recordsAffected = 0;
            FormattableString insertLineSql;
            foreach (SMSection s in parcel.Sections.OrderBy(s => s.SectionLetter))
            {
                foreach (SMLine l in s.Lines.OrderBy(l => l.LineNumber))
                {
                    insertLineSql = $"INSERT INTO NATIVE.AUGLINE(JLRECORD, JLDWELL, JLSECT, JLLINE#, JLDIRECT, JLXLEN, JLYLEN, JLLINELEN, JLANGLE, JLPT1X, JLPT1Y, JLPT2X, JLPT2Y, JLATTACH) VALUES({l.Record}, {l.Card}, '{l.SectionLetter}', {l.LineNumber}, '{l.Direction.ToString()}', {l.XLength}, {l.YLength}, {l.LineLength}, {l.LineAngle}, {l.StartX}, {l.StartY}, {l.EndX}, {l.EndY}, '{l.AttachedSection}')";
                    recordsAffected += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(insertLineSql.ToString());
                }
            }
            return recordsAffected;
        }

        private int InsertMaster(SMParcel parcel)
        {
            FormattableString insertMasterSql = $"INSERT INTO {MasterRecordTable}(JMRECORD, JMDWELL, JMSKETCH, JMSTORY, JMSTORYEX, JMSCALE, JMTOTSQFT, JMESKETCH) VALUES ( {parcel.Record}, {parcel.Card}, '{parcel.HasSketch.ToUpper().Trim()}', {parcel.Storeys},'{parcel.StoreyEx}', {parcel.Scale}, {parcel.TotalSqFt},'{parcel.ExSketch}' )";
            return SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(insertMasterSql.ToString());
        }

        private int InsertSections(SMParcel parcel)
        {
            FormattableString insertSectionSql;

            int recordsAffected = 0;
            foreach (SMSection s in parcel.Sections)
            {
                insertSectionSql =
                    $"INSERT INTO {SectionRecordTable}(JSRECORD, JSDWELL, JSSECT, JSTYPE, JSSTORY, JSDESC, JSSKETCH, JSSQFT, JS0DEPR, JSCLASS, JSVALUE, JSFACTOR, JSDEPRC) VALUES({s.Record},{s.Card}, '{s.SectionLetter}', '{s.SectionType}', {s.StoreysValue}, '{s.Description}', '{s.HasSketch}', {s.SqFt}, '{s.ZeroDepr}', '{s.SectionClass}', {s.SectionValue}, {s.AdjFactor}, {s.Depreciation})";
                recordsAffected += SketchConnection.DbConn.DBConnection.ExecuteNonSelectStatement(insertSectionSql.ToString());
            }

            return recordsAffected;
        }

        private SMParcelMast RefreshWorkingCopyFromDb(SMParcel parcel)
        {
            SMParcelMast parcelMast = SelectParcelMasterWithParcel(parcel.Record, parcel.Card);
            SketchUpGlobals.SMParcelFromData = workingParcel;
            SketchUpGlobals.ParcelMast = parcelMast;
            workingParcel = parcelMast.Parcel;
            SketchUpGlobals.SketchSnapshots.Clear();
            workingParcel.SnapShotIndex = 0;
            AddSketchToSnapshots(workingParcel);
            return parcelMast;
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
                            Card = parcel.Card,
                            SectionLetter = row["JSSECT"].ToString().Trim(),
                            SectionType = row["JSTYPE"].ToString().Trim(),
                            StoreysValue = storeys,
                            Description = row["JSDESC"].ToString().Trim(),
                            HasSketch = row["JSSKETCH"].ToString().Trim().ToUpper(),
                            SqFt = squareFeet,
                            ZeroDepr = row["JS0DEPR"].ToString().Trim(),
                            SectionClass = row["JSCLASS"].ToString().Trim(),
                            SectionValue = value,
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
            List<SMLine> lines = new List<SMLine>();
            string selectSql = string.Format("SELECT JLRECORD , JLDWELL , JLSECT , JLLINE# , JLDIRECT , JLXLEN , JLYLEN , JLLINELEN , JLANGLE , JLPT1X , JLPT1Y , JLPT2X , JLPT2Y , JLATTACH FROM {0} WHERE JLRECORD={1} AND JLDWELL={2} AND JLSECT='{3}'", SketchConnection.LineTable, section.Record, section.Card, section.SectionLetter);
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
                Trace.WriteLine($"{resultsCount} records affected for save.");
                updateCompleted = true;
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
            List<string> laTypes = new List<string>();
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
                FormattableString updateMastSql = $"UPDATE {MastRecordTable} SET MSTOR#={parcel.ParcelMast.StoreysValue} , MGART={parcel.ParcelMast.Garage1TypeCode} , MGAR#C={parcel.ParcelMast.Garage1NumCars} , MCARPT={parcel.ParcelMast.CarportTypeCode} , MCAR#C={parcel.ParcelMast.CarportNumCars} , MBI#C={parcel.ParcelMast.NumCarsBuiltInCode} , MGART2={parcel.ParcelMast.Garage2TypeCode} , MGAR#2={parcel.ParcelMast.Garage2NumCars} , MTOTA={parcel.ParcelMast.TotalArea}  WHERE MRECNO={parcel.ParcelMast.Record} AND MDWELL={parcel.ParcelMast.Card}";
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

#endregion

#region "Properties"

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

        public SMParcel WorkingParcel
        {
            get
            {
                return workingParcel;
            }
            set
            {
                workingParcel = value;
            }
        }

#endregion

#region "Private Fields"

        private string dataSource;
        private string lineRecordTable;
        private string locality;
        private string masterRecordTable;
        private string mastRecordTable;
        private string password;
        private string sectionRecordTable;
        private SMConnection sketchConnection;
        private string userName;
        private SMParcel workingParcel;

#endregion
    }
}
