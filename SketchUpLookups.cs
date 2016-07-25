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
using SWallTech;

namespace SketchUp
{
    public static class SketchUpLookups
    {
        #region "Public Methods"
        public static List<string> ClassCodeList()
        {
            if (Rates.ClassValues == null)
            {
                return null;
            }
            return Rates.ClassValues.Keys.OrderBy(f => f).ToList();
        }

        public static decimal ClassValue(string cls)
        {
            decimal retValue = 0;
            if (Rates.ClassValues.ContainsKey(cls))
            {
                retValue = Rates.ClassValues[cls];
            }
            return retValue;
        }

        public static string CommercialSectionType(this List<CommercialSections> list, string code)
        {
            var q = from h in list
                    where h._commSectionType == code
                    select h._commSectionDescription;

            return q.SingleOrDefault();
        }

        public static string CommercialSectionTypeDescription(this List<CommercialSections> list, string code)
        {
            var q = from h in list
                    where h._commSectionType == code
                    select h._commSectionDescription;

            return q.SingleOrDefault();
        }

        public static string Description(this List<StabType> list, string code)
        {
            var q = from h in list
                    where h.Code == code
                    select h.Description;

            return q.SingleOrDefault();
        }

        public static void Init(CAMRA_Connection conn)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Running {MethodBase.GetCurrentMethod().Name}");
#endif
            DBAccessManager db = conn.DBConnection;

            ClearValues();
            InitializeRatTableCollectionLists(conn);
            GetClassValuesData(db);
            GetResidentialSections(db);
            GetLivingAreaSections(db);
            GetCommercialSections(db);
            GetDeckTypes(db);
            GetAllPorchTypes(db);
            GetGarageTypes(db);
            GetCarportTypes(db);
            GetStabFileData(db);
            GetSectionDescriptions(db);
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        public static void InitializeWithTestSettings()
        {
            //The application sets these, but for the PoC project we need to initialize the connection information manually. JMM 6-6-16
            SketchUpGlobals.IpAddress = "192.168.176.241";
            SketchUpGlobals.UserName = "CAMRA2";
            SketchUpGlobals.Password = "CAMRA2";
            SketchUpGlobals.LocalityPrefix = "AUG";
            SketchUpGlobals.Record = 11787;
            SketchUpGlobals.Card = 1;

            var conn = new SMConnection(SketchUpGlobals.IpAddress, SketchUpGlobals.UserName, SketchUpGlobals.Password, SketchUpGlobals.LocalityPrefix);

            // Init(conn.DbConn);
        }

        public static string SectionDescriptionFromType(string sectionType)
        {
            string description = string.Empty;
            ListOrComboBoxItem typeLookup = SectionDescriptionLookups.FirstOrDefault(c => c.Code == sectionType);
            if (typeLookup != null)
            {
                description = typeLookup.Description.Trim();
            }
            return description;
        }

        public static List<string> SectionLetters()
        {
            var letters = new List<string> { "A", "B", "C", "D", "F", "G", "H", "I", "J", "K", "L", "M" };
            return letters;
        }

        public static List<ListOrComboBoxItem> SectionsByOccupancy(CamraDataEnums.OccupancyType occupancyCode)
        {
            var sectList = new List<ListOrComboBoxItem>();
            switch (occupancyCode)
            {
                case CamraDataEnums.OccupancyType.CodeNotFound:
                    sectList.Add(new ListOrComboBoxItem { Code = "CNF", Description = "Invalid Occupancy Code" });
                    break;

                case CamraDataEnums.OccupancyType.Commercial:
                case CamraDataEnums.OccupancyType.TaxExempt:

                    foreach (CommercialSections cs in CommercialSectionTypeCollection.OrderBy(s => s._commSectionDescription))
                    {
                        sectList.Add(new ListOrComboBoxItem { Code = cs._commSectionType, Description = $"{cs._commSectionType} - {cs._commSectionDescription}".ToString(), PrintDescription = cs._commSectionDescription });
                    }
                    List<ResidentalSections> commOk = (from rs in ResidentialSectionTypeCollection where !CamraDataEnums.GetEnumStrings(typeof(CamraDataEnums.InvalidCommercialSection)).Contains(rs.ResSectionType) select rs).ToList();
                    foreach (ResidentalSections rs in commOk.OrderBy(c => c.ResSectionDescription))
                    {
                        sectList.Add(new ListOrComboBoxItem { Code = rs.ResSectionType, Description = $"{rs.ResSectionType} - {rs.ResSectionDescription}".ToString(), PrintDescription = rs.ResSectionDescription });
                    }

                    break;

                case CamraDataEnums.OccupancyType.Residential:
                    foreach (ResidentalSections rs in ResidentialSectionTypeCollection.OrderBy(s => s.ResSectionDescription))
                    {
                        sectList.Add(new ListOrComboBoxItem { Code = rs.ResSectionType, Description = $"{rs.ResSectionType} - {rs.ResSectionDescription}".ToString(), PrintDescription = rs.ResSectionDescription });
                    }
                    break;

                case CamraDataEnums.OccupancyType.Vacant:
                case CamraDataEnums.OccupancyType.Other:
                    foreach (ResidentalSections rs in ResidentialSectionTypeCollection.OrderBy(s => s.ResSectionDescription))
                    {
                        sectList.Add(new ListOrComboBoxItem { Code = rs.ResSectionType, Description = $"{rs.ResSectionType} - {rs.ResSectionDescription}".ToString(), PrintDescription = rs.ResSectionDescription });
                    }
                    foreach (CommercialSections cs in CommercialSectionTypeCollection.OrderBy(s => s._commSectionDescription))
                    {
                        sectList.Add(new ListOrComboBoxItem { Code = cs._commSectionType, Description = $"{cs._commSectionType} - {cs._commSectionDescription}".ToString(), PrintDescription = cs._commSectionDescription });
                    }
                    break;

                default:
                    break;
            }
            foreach (ResidentalSections rs in ResidentialSectionTypeCollection.OrderBy(s => s.ResSectionDescription))
            {
                sectList.Add(new ListOrComboBoxItem { Code = rs.ResSectionType, Description = $"{rs.ResSectionType} - {rs.ResSectionDescription}".ToString(), PrintDescription = rs.ResSectionDescription });
            }
            sectList.OrderBy(d => d.Description);
            return sectList;
        }

        public static List<string> PropertyClassLetters => new List<string> { "A", "B", "C", "D", "M" };
        private static void GetSectionDescriptions(DBAccessManager db)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Running {MethodBase.GetCurrentMethod().Name}");
#endif
            try
            {
                sectionDescriptions = new List<ListOrComboBoxItem>();
                string library = SketchUpGlobals.LocalLib;
                string locality = SketchUpGlobals.LocalityPrefix;
                FormattableString sql = $"SELECT DISTINCT R.RSECTO as Code,R.RDESC as Description FROM {library}.{locality}RAT1 R WHERE R. RID = 'P' OR(RID = 'C' AND RRPSF = 0) ORDER BY RSECTO,RDESC";
                DataSet descriptions = db.RunSelectStatement(sql.ToString());
                string sectType = string.Empty;
                string sectDescr = string.Empty;
                string printDescr = string.Empty;
                ListOrComboBoxItem lci;
                if (descriptions != null && descriptions.Tables.Count > 0)
                {
                    DataTable dt = descriptions.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            sectType = row["Code"].ToString();
                            sectDescr = row["description"].ToString();
                            printDescr = $"{sectType} - {sectDescr}".ToString();
                            lci = new ListOrComboBoxItem { Code = sectType.ToUpper().Trim(), Description = sectDescr, PrintDescription = printDescr };
                            sectionDescriptions.Add(lci);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private static List<ListOrComboBoxItem> sectionDescriptions;

        #endregion "Public Methods"

        #region "Private methods"

        private static void ClearValues()
        {
            CarPortTypeCollection = null;

            ClassCollection = null;

            GarageTypeCollection = null;

            OccupancyCollection = null;

            //OccupancyDescriptionCollection = null;
            LivingAreaSectionTypeCollection = null;
            Rates.ClassValues = null;
            Rates.Rate1Master = null;
        }

        private static void GetAllPorchTypes(DBAccessManager db)
        {
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            var getPor = new StringBuilder();
            getPor.Append(string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPrefix));
            getPor.Append(" and rdesc not like '%CAR%' and rdesc not like '%ENCL%' and rdesc not like '%SCR%' ");
            DataSet ds_por = db.RunSelectStatement(getPor.ToString());

            if (ds_por.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_por.Tables[0].Rows)
                {
                    string porcode = row["rsecto"].ToString().Trim();

                    PorchTypes.Add(porcode);
                }
            }

            // Screen Porch Types
            var getSPor = new StringBuilder();
            getSPor.Append(string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPrefix));
            getSPor.Append(" and rdesc not like '%CAR%' and rdesc not like '%ENCL%' and rdesc like '%SCR%' ");
            DataSet ds_spor = db.RunSelectStatement(getSPor.ToString());

            if (ds_spor.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_spor.Tables[0].Rows)
                {
                    string sporcode = row["rsecto"].ToString().Trim();

                    ScrnPorchTypes.Add(sporcode);
                }
            }

            // Encl Porch Types
            var getEPor = new StringBuilder();
            getEPor.Append(string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPrefix));
            getEPor.Append(" and rdesc not like '%CAR%' and rdesc like '%ENCL%' and rdesc not like '%SCR%' ");
            DataSet ds_epor = db.RunSelectStatement(getSPor.ToString());

            if (ds_epor.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_epor.Tables[0].Rows)
                {
                    string eporcode = row["rsecto"].ToString().Trim();

                    EnclPorchTypes.Add(eporcode);
                }
            }
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private static void GetCarportTypes(DBAccessManager db)
        {
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            var getcp = new StringBuilder();
            getcp.Append(string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%CAR%' and rdesc not like '%ENC%'  ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPrefix));

            DataSet ds_cp = db.RunSelectStatement(getcp.ToString());

            if (ds_cp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_cp.Tables[0].Rows)
                {
                    string garcode = row["rsecto"].ToString().Trim();

                    CarPortTypes.Add(garcode);
                }
            }
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private static void GetClassValuesData(DBAccessManager db)
        {
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            string sqlRat2 = string.Format("select rdca,rdcb,rdcc,rdcd,rdce,rdcm from {0}.{1}rat2 where rsect2 = '0001'", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix);
            DataSet ds = db.RunSelectStatement(sqlRat2.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Rates.ClassValues.Add("A", Convert.ToDecimal(dr["rdca"].ToString()));
                Rates.ClassValues.Add("B", Convert.ToDecimal(dr["rdcb"].ToString()));
                Rates.ClassValues.Add("C", Convert.ToDecimal(dr["rdcc"].ToString()));
                Rates.ClassValues.Add("D", Convert.ToDecimal(dr["rdcd"].ToString()));
                Rates.ClassValues.Add("E", Convert.ToDecimal(dr["rdce"].ToString()));
                Rates.ClassValues.Add("M", Convert.ToDecimal(dr["rdcm"].ToString()));
            }
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private static void GetCommercialSections(DBAccessManager db)
        {
            DataSet ds_commercialSection = db.RunSelectStatement(string.Format(
                "select rsecto,rdesc,rclar,rclbr,rclcr,rcldr,rclmr from {0}.{1}rat1 where rid = 'C' and rrpsf = 0 ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
            foreach (DataRow row in ds_commercialSection.Tables[0].Rows)
            {
                string commercialSectionType = Convert.ToString(row["rsecto"].ToString().Trim());
                var commSectionType = new CommercialSections()
                {
                    _commSectionType = Convert.ToString(row["rsecto"].ToString().Trim()),
                    _commSectionDescription = Convert.ToString(row["rdesc"].ToString().Trim()),
                    _commSectionRateClassA = Convert.ToDecimal(row["rclar"].ToString()),
                    _commSectionRateClassB = Convert.ToDecimal(row["rclbr"].ToString()),
                    _commSectionRateClassC = Convert.ToDecimal(row["rclcr"].ToString()),
                    _commSectionRateClassD = Convert.ToDecimal(row["rcldr"].ToString()),
                    _commSectionRateClassM = Convert.ToDecimal(row["rclmr"].ToString())
                };
                CommercialSectionTypeCollection.Add(commSectionType);
            }
        }

        private static void GetDeckTypes(DBAccessManager db)
        {
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            string getDeckTypesSql = string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%DECK%' ", SketchUpGlobals.LocalLib,
                             SketchUpGlobals.LocalityPrefix);
            DataSet ds_deck = db.RunSelectStatement(getDeckTypesSql);

            if (ds_deck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_deck.Tables[0].Rows)
                {
                    string deckcode = row["rsecto"].ToString().Trim();

                    DeckTypes.Add(deckcode);
                }
            }
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private static void GetGarageTypes(DBAccessManager db)
        {
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            var getgar = new StringBuilder();
            getgar.Append(string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%GAR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPrefix));
            getgar.Append("and rdesc not like '%ENC%' and rdesc not like '%COM%' and rdesc not like '%APT%' and rdesc not like '%LIV%' ");

            DataSet ds_gar = db.RunSelectStatement(getgar.ToString());

            if (ds_gar.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_gar.Tables[0].Rows)
                {
                    string garcode = row["rsecto"].ToString().Trim();

                    GarageTypes.Add(garcode);
                }
            }
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private static void GetLivingAreaSections(DBAccessManager db)
        {
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            string sql =
                string.Format("SELECT RSECTO,RDESC,RRPSF FROM {0}.{1}RAT1 WHERE RID IN ('P','C') AND RRPSF = 0 AND RSECTO NOT IN ( 'Z01' ,'TRPK')",
                SketchUpGlobals.LocalLib,
                SketchUpGlobals.LocalityPrefix);
            DataSet ds_LASection = db.RunSelectStatement(sql);
            foreach (DataRow row in ds_LASection.Tables[0].Rows)
            {
                var la = new LivingAreaSectionTypes()
                {
                    _LAattSectionType = row["RSECTO"].ToString().Trim(),
                    _LAattSectionDescription = row["RDESC"].ToString().Trim(),
                    _LAattSectionRate = Convert.ToDecimal(row["RRPSF"].ToString()),
                };
                LivingAreaSectionTypeCollection.Add(la);
            }
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private static void GetResidentialSections(DBAccessManager db)
        {
#if DEBUG || TEST

            //Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            string sql =
                string.Format("SELECT RSECTO,RDESC,RRPSF FROM {0}.{1}RAT1 WHERE RID = 'P' ",
                SketchUpGlobals.LocalLib,
                SketchUpGlobals.LocalityPrefix);
            DataSet ds_residentialSection = db.RunSelectStatement(sql);
            foreach (DataRow row in ds_residentialSection.Tables[0].Rows)
            {
                var residentialBuildingSection = new ResidentalSections()
                {
                    ResSectionType = row["rsecto"].ToString().Trim(),
                    ResSectionDescription = row["rdesc"].ToString().Trim(),
                    ResSectionRate = Convert.ToDecimal(row["rrpsf"].ToString()),
                };
                ResidentialSectionTypeCollection.Add(residentialBuildingSection);
            }
        }

        private static void GetStabFileData(DBAccessManager db)
        {
            // Modified for location-specific STAB and DESC files -- JMM 04-13-2016

            try
            {
                DataSet ds_stab = db.RunSelectStatement(string.Format(
             "SELECT TTID,TTELEM,TDESCP,TDESC,TLOC FROM {0}.{1}STAB WHERE TTID IN  ('BAS','CAR','CLS','GAR','OCC') Order by TTID, TTELEM ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
                var allStabTypes = new List<StabType>();
                foreach (DataRow row in ds_stab.Tables[0].Rows)
                {
                    string TTelem = row["ttelem"].ToString().PadLeft(2, ' ');
                    string Descp = row["tdescp"].ToString().Trim();

                    string Pdescp = TTelem.Substring(0, 2);

                    var stab = new StabType()
                    {
                        Type = row["ttid"].ToString().Trim(),
                        Code = row["ttelem"].ToString().Trim(),
                        Description = row["tdesc"].ToString().Trim(),
                        ShortDescription = string.Format("{0} - {1}", Pdescp.ToString().Trim(), Descp.ToString().Trim())
                    };
                    allStabTypes.Add(stab);
                }
                ParseRatTableDataToLists(allStabTypes);
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

        private static void InitializeRatTableCollectionLists(CAMRA_Connection conn)
        {
            // Initialize Static variables

            try
            {
                AuxAreaTypes = new List<string>();
                BasementTypeCollection = new List<StabType>();
                CarPortTypeCollection = new List<StabType>();
                CarPortTypes = new List<string>();
                ClassCollection = new List<StabType>();
                CommercialOccupancies = new List<int>();
                CommercialSectionTypeCollection = new List<CommercialSections>();
                LivingAreaSectionTypeCollection = new List<LivingAreaSectionTypes>();
                DeckTypes = new List<string>();
                EnclPorchTypes = new List<string>();
                GarageTypeCollection = new List<StabType>();
                GarageTypes = new List<string>();
                OccupancyCollection = new List<StabType>();

                // OccupancyDescriptionCollection = new List<OccupancyDescription>();
                PorchTypes = new List<string>();
                Rat1AllTypes = new List<StabType>();
                Rates.Rate1Master = new Rat1Master(conn);
                ResidentialOccupancies = new List<int>();
                ResidentialSectionTypeCollection = new List<ResidentalSections>();
                ScrnPorchTypes = new List<string>();

                Rates.ClassValues = new SortedDictionary<string, decimal>();
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

        private static void ParseRatTableDataToLists(List<StabType> allStabTypes)
        {
            try
            {

                List<StabType> BAS = (from t in allStabTypes where t.Type == "BAS" select t).ToList();
                List<StabType> CAR = (from t in allStabTypes where t.Type == "CAR" select t).ToList();
                List<StabType> CLS = (from t in allStabTypes where t.Type == "CLS" select t).ToList();
                List<StabType> GAR = (from t in allStabTypes where t.Type == "GAR" select t).ToList();
                List<StabType> OCC = (from t in allStabTypes where t.Type == "OCC" select t).ToList();
                CarPortTypeCollection.AddRange(CAR);
                ClassCollection.AddRange(CLS);
                GarageTypeCollection.AddRange(GAR);
                BasementTypeCollection.AddRange(BAS);
                OccupancyCollection.AddRange(OCC);
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

        #endregion "Private methods"

        #region "Properties"

        public static List<string> AuxAreaTypes
        {
            get {
                auxAreaTypes = CamraDataEnums.GetEnumStrings(typeof(CamraDataEnums.AuxAreaTypes));

                return auxAreaTypes;
            }

            set { auxAreaTypes = value; }
        }

        public static List<StabType> BasementTypeCollection { get; set; }

        public static List<StabType> CarPortTypeCollection { get; set; }

        public static List<string> CarPortTypes { get; set; } = new List<string>();

        public static List<StabType> CharacteristicTypeCollection { get; set; }

        public static List<StabType> ClassCollection { get; set; }

        public static List<int> CommercialOccupancies
        {
            get {
                commercialOccupanyCodes = CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.CommercialOccupancyCodes));
                return commercialOccupanyCodes;
            }

            set { commercialOccupanyCodes = value; }
        }

        public static List<CommercialSections> CommercialSectionTypeCollection { get; set; }

        public static List<string> DeckTypes { get; set; } = new List<string>();

        public static List<string> EnclPorchTypes { get; set; }

        public static List<StabType> GarageTypeCollection { get; set; }

        public static List<string> GarageTypes { get; set; }

        public static List<string> Letters
        {
            get {
                if (letters == null || letters.Count == 0)
                {
                    letters = CamraDataEnums.GetEnumStrings(typeof(CamraDataEnums.Letters)).ToList();
                }
                return letters;
            }

            set { letters = value; }
        }

        public static List<LivingAreaSectionTypes> LivingAreaSectionTypeCollection { get; set; }

        public static List<StabType> OccupancyCollection { get; set; }


        public static List<string> PorchTypes { get; set; }

        public static List<StabType> Rat1AllTypes { get; set; }

        public static List<int> ResidentialOccupancies
        {
            get {
                residentialOccupancyCodes = CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.ResidentialOccupancyCodes));
                return residentialOccupancyCodes;
            }

            set { residentialOccupancyCodes = value; }
        }

        public static List<ResidentalSections> ResidentialSectionTypeCollection { get; set; }

        public static List<string> ScrnPorchTypes { get; set; }

        public static List<ListOrComboBoxItem> SectionDescriptionLookups
        {
            get {
                if (sectionDescriptions == null)
                {
                    sectionDescriptions = new List<ListOrComboBoxItem>();
                }
                return sectionDescriptions;
            }

            set { sectionDescriptions = value; }
        }

        #endregion "Properties"

        #region "Private Fields"

        private static List<string> auxAreaTypes;
        private static List<int> commercialOccupanyCodes;
        private static List<string> letters;
        private static List<int> residentialOccupancyCodes;

        #endregion "Private Fields"
    }
}