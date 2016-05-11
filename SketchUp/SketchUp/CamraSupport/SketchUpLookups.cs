using SWallTech;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Rates.ClassValues.Keys.OrderBy(f => f).ToList<string>();
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

        public static void Init(SWallTech.CAMRA_Connection conn)
        {
            DBAccessManager db = conn.DBConnection;
            ClearValues();
            InitializeRatTableCollectionLists(conn);
            GetClassValuesData(db);
            GetDeckTypes(db);
            GetAllPorchTypes(db);
            GetGarageTypes(db);
            GetCarportTypes(db);
            GetStabFileData(db);
        }

#endregion

#region "Private methods"

        private static void ClearValues()
        {

            CarPortTypeCollection = null;
         
            ClassCollection = null;
         
            GarageTypeCollection = null;
         
            OccupancyCollection = null;
         
            OccupancyDescriptionCollection = null;
         
            Rates.ClassValues = null;
            Rates.Rate1Master = null;

        }

        private static void GetAllPorchTypes(DBAccessManager db)
        {
            StringBuilder getPor = new StringBuilder();
            getPor.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));
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
            StringBuilder getSPor = new StringBuilder();
            getSPor.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));
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

            //   Encl Porch Types
            StringBuilder getEPor = new StringBuilder();
            getEPor.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));
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
        }

        private static void GetBuildingSectionTypesAndRates(DBAccessManager db)
        {
            DataSet ds_residentialSection = db.RunSelectStatement(String.Format(

            "select rsecto,rdesc,rrpsf from {0}.{1}rat1 where rid = 'P' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_residentialSection.Tables[0].Rows)
            {
                string residentialSectionType = Convert.ToString(row["rsecto"].ToString());
                var residentialBuildingSection = new ResidentialSections()
                {
                    _resSectionType = row["rsecto"].ToString().Trim(),
                    _resSectionDescription = row["rdesc"].ToString().Trim(),
                    _resSectionRate = Convert.ToDecimal(row["rrpsf"].ToString()),
                };
                ResidentialSectionTypeCollection.Add(residentialBuildingSection);
            }

            DataSet ds_commercialSection = db.RunSelectStatement(String.Format(
                "select rsecto,rdesc,rclar,rclbr,rclcr,rcldr,rclmr from {0}.{1}rat1 where rid = 'C' and rrpsf = 0 ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
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

        private static void GetCarportTypes(DBAccessManager db)
        {
            StringBuilder getcp = new StringBuilder();
            getcp.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%CAR%' and rdesc not like '%ENC%'  ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));

            DataSet ds_cp = db.RunSelectStatement(getcp.ToString());

            if (ds_cp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_cp.Tables[0].Rows)
                {
                    string garcode = row["rsecto"].ToString().Trim();

                    CarPortTypes.Add(garcode);
                }
            }
        }

        private static void GetClassValuesData(DBAccessManager db)
        {
            string sqlRat2 = string.Format("select rdca,rdcb,rdcc,rdcd,rdce,rdcm from {0}.{1}rat2 where rsect2 = '0001'", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix);
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
        }

        private static void GetDeckTypes(DBAccessManager db)
        {
            string getDeckTypesSql = string.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%DECK%' ", SketchUpGlobals.LocalLib,
                             SketchUpGlobals.LocalityPreFix);
            DataSet ds_deck = db.RunSelectStatement(getDeckTypesSql);

            if (ds_deck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_deck.Tables[0].Rows)
                {
                    string deckcode = row["rsecto"].ToString().Trim();

                    DeckTypes.Add(deckcode);
                }
            }
        }

        private static void GetGarageTypes(DBAccessManager db)
        {
            StringBuilder getgar = new StringBuilder();
            getgar.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%GAR%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));
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
        }

        private static void GetStabFileData(DBAccessManager db)
        {
            // Modified for location-specific STAB and DESC files -- JMM 04-13-2016
            DataSet ds_stab = db.RunSelectStatement(String.Format(
                "SELECT TTID,TTELEM,TDESCP,TDESC,TLOC FROM {0}.{1}STAB WHERE TTID IN  ('BAS','CAR','CLS','GAR','OCC') Order by TTID, TTELEM ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));
            List<StabType> allStabTypes = new List<StabType>();
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
                    _printedDescription = String.Format("{0} - {1}", Pdescp.ToString().Trim(), Descp.ToString().Trim())
                };
                allStabTypes.Add(stab);
            }
            ParseRatTableDataToLists(allStabTypes);
        }

        private static void InitializeRatTableCollectionLists(CAMRA_Connection conn)
        {
            // Initialize Static variables
            AuxAreaTypes = new List<string>();
            BasementTypeCollection = new List<StabType>();
            CarPortTypeCollection = new List<StabType>();
            CarPortTypes = new List<string>();
            ClassCollection = new List<StabType>();
            CommercialOccupancies = new List<int>();
            CommercialSectionTypeCollection = new List<CommercialSections>();
            DeckTypes = new List<string>();
            EnclPorchTypes = new List<string>();
            GarageTypeCollection = new List<StabType>();
            GarageTypes = new List<string>();
            OccupancyCollection = new List<StabType>();
            OccupancyDescriptionCollection = new List<OccupancyDescription>();
            PorchTypes = new List<string>();
            Rat1AllTypes = new List<StabType>();
            Rates.Rate1Master = new Rat1Master(conn);
            ResidentialOccupancies = new List<int>();
            ResidentialSectionTypeCollection = new List<ResidentialSections>();
            ScrnPorchTypes = new List<string>();

            Rates.ClassValues = new SortedDictionary<string, decimal>();

        }

        private static void ParseRatTableDataToLists(List<StabType> allStabTypes)
        {
            var BAS = (from t in allStabTypes where t.Type == "BAS" select t).ToList<StabType>();
            var CAR = (from t in allStabTypes where t.Type == "CAR" select t).ToList<StabType>();
            var CLS = (from t in allStabTypes where t.Type == "CLS" select t).ToList<StabType>();
            var GAR = (from t in allStabTypes where t.Type == "GAR" select t).ToList<StabType>();
            var OCC = (from t in allStabTypes where t.Type == "OCC" select t).ToList<StabType>();
            CarPortTypeCollection.AddRange(CAR);
            ClassCollection.AddRange(CLS);
            GarageTypeCollection.AddRange(GAR);
            BasementTypeCollection.AddRange(BAS);
            OccupancyCollection.AddRange(OCC);

        }

#endregion

        public static List<string> AuxAreaTypes
        {
            get
            {
                auxAreaTypes = CamraDataEnums.GetEnumStrings(typeof(CamraDataEnums.AuxAreaTypes));

                return auxAreaTypes;
            }
            set
            {
                auxAreaTypes = value;
            }
        }

        public static List<StabType> BasementTypeCollection
        {
            get
            {
                return basementTypeCollection;
            }
            set
            {
                basementTypeCollection = value;
            }
        }

        public static List<StabType> CarPortTypeCollection
        {
            get
            {
                return carPortTypeCollection;
            }
            set
            {
                carPortTypeCollection = value;
            }
        }

        public static List<string> CarPortTypes
        {
            get
            {
                return carPortTypes;
            }
            set
            {
                carPortTypes = value;
            }
        }

        public static List<StabType> CharacteristicTypeCollection
        {
            get
            {
                return characteristicTypeCollection;
            }
            set
            {
                characteristicTypeCollection = value;
            }
        }

        public static List<StabType> ClassCollection
        {
            get
            {
                return classCollection;
            }
            set
            {
                classCollection = value;
            }
        }

        public static List<int> CommercialOccupancies
        {
            get
            {
                commercialOccupanyCodes = CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.CommercialOccupancyCodes));
                return commercialOccupanyCodes;
            }
            set
            {
                commercialOccupanyCodes = value;
            }
        }

        public static List<CommercialSections> CommercialSectionTypeCollection
        {
            get
            {
                return commercialSectionTypeCollection;
            }
            set
            {
                commercialSectionTypeCollection = value;
            }
        }

        public static List<string> DeckTypes
        {
            get
            {
                return deckTypes;
            }
            set
            {
                deckTypes = value;
            }
        }

        public static List<string> EnclPorchTypes
        {
            get
            {
                return enclPorchTypes;
            }
            set
            {
                enclPorchTypes = value;
            }
        }

        public static List<StabType> GarageTypeCollection
        {
            get
            {
                return garageTypeCollection;
            }
            set
            {
                garageTypeCollection = value;
            }
        }

        public static List<string> GarageTypes
        {
            get
            {
                return garageTypes;
            }
            set
            {
                garageTypes = value;
            }
        }

        public static List<StabType> OccupancyCollection
        {
            get
            {
                return occupancyCollection;
            }
            set
            {
                occupancyCollection = value;
            }
        }

        public static List<OccupancyDescription> OccupancyDescriptionCollection
        {
            get
            {
                return occupancyDescriptionCollection;
            }
            set
            {
                occupancyDescriptionCollection = value;
            }
        }

        public static List<string> PorchTypes
        {
            get
            {
                return porchTypes;
            }
            set
            {
                porchTypes = value;
            }
        }

        public static List<int> ResidentialOccupancies
        {
            get
            {
                residentialOccupancyCodes = CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.ResidentialOccupancyCodes));
                return residentialOccupancyCodes;
            }
            set
            {
                residentialOccupancyCodes = value;
            }
        }

        public static List<ResidentialSections> ResidentialSectionTypeCollection
        {
            get
            {
                return residentialSectionTypeCollection;
            }
            set
            {
                residentialSectionTypeCollection = value;
            }
        }

        public static List<string> ScrnPorchTypes
        {
            get
            {
                return scrnPorchTypes;
            }

            set
            {
                scrnPorchTypes = value;
            }
        }

        public static List<StabType> Rat1AllTypes
        {
            get
            {
                return rat1AllTypes;
            }

            set
            {
                rat1AllTypes = value;
            }
        }

        private static List<string> auxAreaTypes;
        private static List<StabType> basementTypeCollection;
        private static List<StabType> carPortTypeCollection;
        private static List<string> carPortTypes = new List<string>();
        private static List<StabType> characteristicTypeCollection;
        private static List<StabType> classCollection;
        private static List<int> commercialOccupanyCodes;
        private static List<CommercialSections> commercialSectionTypeCollection;
        private static List<string> deckTypes = new List<string>();
        private static List<string> enclPorchTypes;
        private static List<StabType> garageTypeCollection;
        private static List<string> garageTypes;
        private static List<StabType> occupancyCollection;
        private static List<OccupancyDescription> occupancyDescriptionCollection;
        private static List<string> porchTypes; 
        private static List<int> residentialOccupancyCodes;
        private static List<ResidentialSections> residentialSectionTypeCollection;
        private static List<string> scrnPorchTypes;
        private static List<StabType> rat1AllTypes;
    }
}
