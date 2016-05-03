using SWallTech;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SketchUp
{
    public static class CamraSupport
    {
        private static void ClearValues()
        {
            AirCondCollection = null;
            AirCondCollection_srt = null;
            AppDepChgRate = 0;
            AssessorCodeCollection = null;
            AttachedSectionTypeCollection = null;
            BasementTypeCollection = null;
            BasementTypeCollection_srt = null;
            CarPortTypeCollection = null;
            CarPortTypeCollection_srt = null;
            CharacteristicDescriptionCollection = null;
            CharacteristicDescriptionCollection_srt = null;
            CharacteristicTypeCollection = null;
            CharacteristicTypeCollection_srt = null;
            ClassCollection = null;
            ClassCollection_srt = null;
            CommercialIncomeSectionCollection = null;
            CommercialRateCollection = null;
            CommercialRateCollection_srt = null;
            CommercialSectionTypeCollection = null;
            CommercialSectionTypeDescriptionCollection = null;
            ConditionTypeCollection = null;
            ConditionTypeCollection_srt = null;
            DataEntryOperatorCollection = null;
            DefDepCondA = 0;
            DefDepCondF = 0;
            DefDepCondG = 0;
            DefDepCondP = 0;
            DiscountRate = 0;
            DisimilarityWeightCollection = null;
            DoorKnockerCollection = null;
            DoorKnockerCollection_srt = null;
            EasementTypeCollection = null;
            EasementTypeCollection_srt = null;
            EasementTypeCollection_srt = null;
            EffectiveTaxRate = 0;
            EquityYieldRate = 0;
            ExteriorWallTypeCollection = null;
            FloorAbreviationCollection = null;
            FloorCoverCollection = null;
            FoundationCollection = null;
            FoundationCollection_srt = null;
            FuelTypeCollection = null;
            FuelTypeCollection_srt = null;
            GarageTypeCollection = null;
            GarageTypeCollection_srt = null;
            HeatTypeCollection = null;
            HeatTypeCollection_srt = null;
            HoldingPeriod = 0;
            HomeSiteTypeCollection = null;
            HomeSiteTypeCollection_srt = null;
            IncomeChgRate = 0;
            InteriorWallTypeCollection = null;
            InteriorWallTypeCollection_srt = null;
            LandTypeCollection = null;
            LandTypeCollection_srt = null;
            LandUseTypeCollection = null;
            LoanToValueRatio = 0;
            MagDistrictCollection = null;
            MapCoordinateCollection = null;
            MortgageID = String.Empty;
            MortgageRate = 0;
            MortgageTerm = 0;
            NonBasementTypeCollection = null;
            NonBasementTypeCollection_srt = null;
            OccupancyCollection = null;
            OccupancyCollection = null;
            OccupancyCollection_srt = null;
            OccupancyCollection_srt = null;
            OccupancyDescriptionCollection = null;
            OccupancyDescriptionCollection_srt = null;
            OperExpInitChg = 0;
            OperExpTermChg = 0;
            OtherImprovementCollection = null;
            OtherImprovementCollection_srt = null;
            PavementDescriptionCollection = null;
            PavementDescriptionCollection_srt = null;
            ProjectionPeriod = 0;
            Rates.BasementRate = 0;
            Rates.ClassValues = null;
            Rates.ExtraKitRate = 0;
            Rates.FinBasementDefaultRate = 0;
            Rates.PlumbingRate = 0;
            Rates.Rate1Master = null;
            ResidentialSectionTypeCollection = null;
            ResidentialSectionTypeDescriptionCollection = null;
            ReversionCommission = 0;
            RightofWayDescriptionCollection = null;
            RightofWayDescriptionCollection_srt = null;
            RightofWayTypeCollection = null;
            RightofWayTypeCollection_srt = null;
            RoofingCollection = null;
            RoofingCollection_srt = null;
            RoofTypeCollection = null;
            RoofTypeCollection_srt = null;
            SaleYearCutOff = 0;
            SewerDescriptionCollection = null;
            SewerDescriptionCollection_srt = null;
            SewerRateCollection = null;
            SewerTypeCollection = null;
            SewerTypeCollection_srt = null;
            StdDeviationCollection = null;
            SubDivisionCodeCollection = null;
            SubDivisionCodeCollection_srt = null;
            TermCapRateAdj = 0;
            TerrainDescriptionCollection = null;
            TerrainDescriptionCollection_srt = null;
            TerrainTypeCollection = null;
            TerrainTypeCollection_srt = null;
            UserCodeTypeCollection = null;
            UserCodeTypeCollection = null;
            VacancyChgRate = 0;
            WallAbreviationCollection = null;
            WaterDescriptionCollection = null;
            WaterDescriptionCollection_srt = null;
            WaterRateCollection = null;
            WaterTypeCollection = null;
            WaterTypeCollection_srt = null;
            ZoningDescriptionCollection = null;
            ZoningDescriptionCollection_srt = null;
        }

        public static string CommercialIncomeSections(this List<CommercialIncomeSections> list, string code)
        {
            var q = from h in list
                    where h._commIncSectionType == code
                    select h._commIncSectionDescription;

            return q.SingleOrDefault();
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

        public static DismWeights DisWght(this List<DismWeights> list, decimal code)
        {
            var q = from h in list
                    where h._wtStory == code
                    select h;

            return q.SingleOrDefault();
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

        private static void GetAssessorInitalCodeDescriptions(DBAccessManager db)
        {
            DataSet ds_assInitCode = db.RunSelectStatement(String.Format(
                " select rsecto,rdesc,rtid from {0}.{1}rat1 where rid = 'B' and substr(rsecto,1,2) = 'BA' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_assInitCode.Tables[0].Rows)
            {
                string assInitial = String.Format("{0} - {1}",
                    row["rtid"].ToString().Trim(),
                    row["rdesc"].ToString().Trim());
                var assInitCode = new AssessorCodes()
                {
                    _assInitCode = row["rtid"].ToString().Trim(),
                    _assInitDescription = row["rdesc"].ToString().Trim(),
                    _assPrintDescription = assInitial.ToString().Trim(),
                };
                AssessorCodeCollection.Add(assInitCode);
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

            DataSet ds_commercialIncomeSection = db.RunSelectStatement(String.Format(
              "select rsecto,rdesc,rclar,rclbr,rclcr,rcldr,rclmr from {0}.{1}rat1 where rid = 'C' and rrpsf = 0 and substr(rsecto,1,1) in ('A','B','C','I','H') ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_commercialIncomeSection.Tables[0].Rows)
            {
                string commercialIncomeSectionType = Convert.ToString(row["rsecto"].ToString().Trim());
                var commIncSectionType = new CommercialIncomeSections()
                {
                    _commIncSectionType = Convert.ToString(row["rsecto"].ToString().Trim()),
                    _commIncSectionDescription = Convert.ToString(row["rdesc"].ToString().Trim()),
                };
                CommercialIncomeSectionCollection.Add(commIncSectionType);
            }

            DataSet ds_flrAbrev = db.RunSelectStatement(String.Format(
                "select ttid,ttelem,tdesc,tloc from {0}.{1}stab where ttid = 'FLR'", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));
            foreach (DataRow row in ds_flrAbrev.Tables[0].Rows)
            {
                var flrAbrvCode = new FloorAbreviation()
                {
                    FlrCode = Convert.ToString(row["ttelem"].ToString().Trim()),
                    FlrAbreviation = Convert.ToString(row["tloc"].ToString().Trim())
                };
                FloorAbreviationCollection.Add(flrAbrvCode);
            }

            DataSet ds_wallAbrev = db.RunSelectStatement(String.Format(
                "select ttid,ttelem,tdesc,tloc from {0}.{1}stab where ttid = 'INW'", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));
            foreach (DataRow row in ds_wallAbrev.Tables[0].Rows)
            {
                var wallAbrvCode = new InWallAbreviation()
                {
                    WallCode = Convert.ToString(row["ttelem"].ToString().Trim()),
                    WallAbreviation = Convert.ToString(row["tloc"].ToString().Trim())
                };

                WallAbreviationCollection.Add(wallAbrvCode);
            }
        }

        private static void GetCapitalizationRates(DBAccessManager db)
        {
            StringBuilder caprate = new StringBuilder();
            caprate.Append((String.Format("select cid,cmtgr,cmtgt,clvr,ceyld,chold,cetax,cchgr,cproj,cdisc,crevc,cincg,cexpci, " +
                    " cexpcr,cvcla,ctcra from parrevlib.{0}rentr", SketchUpGlobals.LocalityPreFix)));

            try
            {
                DataSet ds_capRates = db.RunSelectStatement(caprate.ToString());

                if (ds_capRates.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds_capRates.Tables[0].Rows[0];

                    MortgageID = dr["cid"].ToString();
                    MortgageRate = Convert.ToDecimal(dr["cmtgr"].ToString());
                    MortgageTerm = Convert.ToInt32(dr["cmtgt"].ToString());
                    LoanToValueRatio = Convert.ToDecimal(dr["clvr"].ToString());
                    EquityYieldRate = Convert.ToDecimal(dr["ceyld"].ToString());
                    HoldingPeriod = Convert.ToInt32(dr["chold"].ToString());
                    EffectiveTaxRate = Convert.ToDecimal(dr["cetax"].ToString());
                    AppDepChgRate = Convert.ToDecimal(dr["cchgr"].ToString());
                    ProjectionPeriod = Convert.ToInt32(dr["cproj"].ToString());
                    DiscountRate = Convert.ToDecimal(dr["cdisc"].ToString());
                    ReversionCommission = Convert.ToDecimal(dr["crevc"].ToString());
                    IncomeChgRate = Convert.ToDecimal(dr["cincg"].ToString());
                    OperExpInitChg = Convert.ToDecimal(dr["cexpci"].ToString());
                    OperExpTermChg = Convert.ToDecimal(dr["cexpcr"].ToString());
                    VacancyChgRate = Convert.ToDecimal(dr["cvcla"].ToString());
                    TermCapRateAdj = Convert.ToDecimal(dr["ctcra"].ToString());
                }
            }
            catch
            {
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

        public static List<string> GetClassCodeList()
        {
            if (Rates.ClassValues == null)
            {
                return null;
            }
            return Rates.ClassValues.Keys.OrderBy(f => f).ToList<string>();
        }

        public static decimal GetClassValue(string cls)
        {
            decimal retValue = 0;
            if (Rates.ClassValues.ContainsKey(cls))
            {
                retValue = Rates.ClassValues[cls];
            }
            return retValue;
        }

        private static void GetDataEntryOperatorDescriptions(DBAccessManager db)
        {
            DataSet ds_dataEntryOpCode = db.RunSelectStatement(String.Format(
                " select rsecto,rdesc,rtid from {0}.{1}rat1 where rid = 'B' and substr(rsecto,1,2) = 'BD' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_dataEntryOpCode.Tables[0].Rows)
            {
                string dataOpInitial = String.Format("{0} - {1}",
                     row["rtid"].ToString().Trim(),
                    row["rdesc"].ToString().Trim());
                var dataOpInitCode = new DataEntryOperatorCode()
                {
                    _dataEntryOpCode = row["rtid"].ToString().Trim(),
                    _dataEntryOpDescription = row["rdesc"].ToString().Trim(),
                    _dataEntryPrintDescription = dataOpInitial.ToString().Trim(),
                };
                DataEntryOperatorCollection.Add(dataOpInitCode);
            }
        }

        private static void GetDeckTypes(DBAccessManager db)
        {
            StringBuilder getDeck = new StringBuilder();
            getDeck.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%DECK%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));
            DataSet ds_deck = db.RunSelectStatement(getDeck.ToString());

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

        private static void GetGasLogRate(DBAccessManager db)
        {
            StringBuilder sqlGasLog = new StringBuilder();
            sqlGasLog.Append(String.Format(" select sysvalue from {0}.{1}sys where systype like 'GAS%' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));

            Rates.GasLogRate = ((Convert.ToDecimal(db.ExecuteScalar(sqlGasLog.ToString()))) / 100);
        }

        private static void GetHomesiteDescription()
        {
            var ds_HSite = from l in Rates.Rate1Master.GetRat1TypeCollection(Rat1Master.Rat1Types.Homesite)
                           select new HomeSiteType()
                           {
                               HSCode = int.Parse(l.Value.ElemCode.TrimEnd(new char[] { ' ' }).Substring(1)),
                               HSDescription = l.Value.Description,
                               HSRate = Convert.ToInt32(l.Value.Rate)
                           };
            HomeSiteTypeCollection.AddRange(ds_HSite);
        }

        private static void GetLandDescription()
        {
            var ds_land = from l in Rates.Rate1Master.GetRat1TypeCollection(Rat1Master.Rat1Types.Land)
                          select new LandType()
                          {
                              LandCode = int.Parse(l.Value.ElemCode.TrimEnd(new char[] { ' ' }).Substring(1)),
                              LandDescription = l.Value.Description
                          };
            LandTypeCollection.AddRange(ds_land);
        }

        private static void GetLandUseDescription()
        {
            var ds_landUse = from l in Rates.Rate1Master.GetRat1TypeCollection(Rat1Master.Rat1Types.LandUseCode)
                             select new LandUseType()
                             {
                                 _landUseCode = l.Value.ElemCode.Substring(1, 2),
                                 _landUseDescription = l.Value.Description,
                                 _printUseDescription = String.Format("{0} - {1}",
                                        l.Value.ElemCode.Substring(1, 2),
                                        l.Value.Description)
                             };
            LandUseTypeCollection.AddRange(ds_landUse);
        }

        private static void GetMagisterialDistricts(DBAccessManager db)
        {
            DataSet ds_magDistCode = db.RunSelectStatement(String.Format(
                            " select rsecto,rdesc from {0}.{1}rat1 where rid = 'M' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_magDistCode.Tables[0].Rows)
            {
                string magDistCode = String.Format("{0} - {1}",

                    //row["rsecto"].ToString().TrimEnd(),
                    row["rsecto"].ToString().Substring(0, 2),
                    row["rdesc"].ToString().Trim());
                var magCode = new MagDistrictCodes()
                {
                    //_magDistCode = row["rsecto"].ToString().Substring(0, 2),
                    _magDistCode = row["rsecto"].ToString().TrimEnd(),
                    _magDistDescription = row["rdesc"].ToString().Trim(),
                    _magPrintDescription = magDistCode.Trim(),
                };
                MagDistrictCollection.Add(magCode);
            }
        }

        private static void GetMapCoordinates(DBAccessManager db)
        {
            DataSet ds_mapXY = db.RunSelectStatement(String.Format(
                            "select qmapid,qxaxis,qyaxis from {0}.{1}map ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_mapXY.Tables[0].Rows)
            {
                var mapXYCode = new MapCoordinates()
                {
                    _subMap = row["qmapid"].ToString(),
                    _xAxis = Convert.ToInt32(row["qxaxis"].ToString()),
                    _yAxis = Convert.ToInt32(row["qyaxis"].ToString())
                };
                MapCoordinateCollection.Add(mapXYCode);
            }
        }

        private static void GetNoMaxAc(DBAccessManager db)
        {
            StringBuilder sqlMaxAc = new StringBuilder();
            sqlMaxAc.Append(String.Format(" select sysvalue from {0}.{1}sys where systype like 'NOMAXAC%' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));

            NoMaxAc = db.ExecuteScalar(sqlMaxAc.ToString()).ToString().Trim();
        }

        private static void GetOccupancyListAndDescriptions(DBAccessManager db)
        {
            DataSet ds_occupancyDescriptions = db.RunSelectStatement(string.Format(
                            "select ttid,ttelem,tdesc from {0}.{1}stab where ttid = 'OCC' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));
            foreach (DataRow row in ds_occupancyDescriptions.Tables[0].Rows)
            {
                string occdescription = String.Format("{0} - {1}",
                    row["ttelem"].ToString().Trim(),
                    row["tdesc"].ToString().Trim());

                var occupancyDescription = new OccupancyDescription()
                {
                    _occupancyCode = row["ttelem"].ToString().Trim(),
                    _occupancyDescription = row["tdesc"].ToString().Trim(),
                    _printOccupancyDescription = occdescription.ToString().Trim(),
                };
                OccupancyDescriptionCollection.Add(occupancyDescription);
            }
        }

        private static void GetOtherImprovementCodes(DBAccessManager db)
        {
            DataSet ds_OICodes = db.RunSelectStatement(String.Format(
                " select rsecto,rdesc from {0}.{1}rat1 where rid = 'I' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_OICodes.Tables[0].Rows)
            {
                string oiDesc = String.Format("{0} - {1}",
                    row["rsecto"].ToString().Substring(1, 3),
                    row["rdesc"].ToString());

                var _oiCode = new OtherImprovement()
                {
                    _OICode = Convert.ToInt32(row["rsecto"].ToString().Substring(1, 3)),
                    _OIDescription = row["rdesc"].ToString(),
                    _printOIDescription = oiDesc.Trim(),
                };

                OtherImprovementCollection.Add(_oiCode);
            }
        }

        private static void GetPatioTypes(DBAccessManager db)
        {
            StringBuilder getPat = new StringBuilder();
            getPat.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%PAT%' ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix));
            DataSet ds_pat = db.RunSelectStatement(getPat.ToString());

            if (ds_pat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_pat.Tables[0].Rows)
                {
                    string patcode = row["rsecto"].ToString().Trim();

                    PatioTypes.Add(patcode);
                }
            }
        }

        private static void GetRat2Data(DBAccessManager db)
        {
            StringBuilder sqlRat2 = new StringBuilder();
            sqlRat2.AppendLine("select rdca,rdcb,rdcc,rdcd,rdce,rdcm,rbrate,rdate,rekit,yadjy1,rpier,rslab ");
            sqlRat2.AppendLine(",racamt,rplfix,rfinbs,rcondg,rconda,rcondf,rcondp,roblim ");
            sqlRat2.AppendLine(String.Format(" from {0}.{1}rat2 where rsect2 = '0001'", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));

            //DataSet ds = db.RunSelectStatement(String.Format(
            //    "select rdca,rdcb,rdcc,rdcd,rdce,rdcm,rbrate,rdate,rekit,yadjy1,rpier,rslab,racamt,rplfix,rfinbs from native.{0}rat2 where rsect2 = '0001'", prefix));

            DataSet ds = db.RunSelectStatement(sqlRat2.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string yearstr = String.Empty;
                string monthstr = String.Empty;
                string daystr = String.Empty;
                int RAyear = 0;
                int RAmonth = 0;
                string reassmentyearstr = Convert.ToString(dr["rdate"].ToString().Trim());

                if (reassmentyearstr.Length == 7)
                {
                    monthstr = reassmentyearstr.Substring(0, 1);
                    daystr = reassmentyearstr.Substring(1, 2);
                    yearstr = reassmentyearstr.Substring(3, 4);
                    RAyear = Convert.ToInt32(yearstr);
                    RAmonth = Convert.ToInt32(monthstr);
                }
                else if (reassmentyearstr.Length == 8)
                {
                    monthstr = reassmentyearstr.Substring(0, 2);
                    daystr = reassmentyearstr.Substring(2, 2);
                    yearstr = reassmentyearstr.Substring(4, 4);
                    RAyear = Convert.ToInt32(yearstr);
                    RAmonth = Convert.ToInt32(monthstr);
                }

                ReassessmentDate = String.Format("{0}/{1}/{2}", monthstr, daystr, yearstr);

                Rates.ClassValues.Add("A", Convert.ToDecimal(dr["rdca"].ToString()));
                Rates.ClassValues.Add("B", Convert.ToDecimal(dr["rdcb"].ToString()));
                Rates.ClassValues.Add("C", Convert.ToDecimal(dr["rdcc"].ToString()));
                Rates.ClassValues.Add("D", Convert.ToDecimal(dr["rdcd"].ToString()));
                Rates.ClassValues.Add("E", Convert.ToDecimal(dr["rdce"].ToString()));
                Rates.ClassValues.Add("M", Convert.ToDecimal(dr["rdcm"].ToString()));

                Rates.BasementRate = Convert.ToDecimal(dr["rbrate"].ToString());
                Rates.FinBasementDefaultRate = Convert.ToDecimal(dr["rfinbs"].ToString());
                Rates.PlumbingRate = Convert.ToDecimal(dr["rplfix"].ToString());
                ReassessmentYear = RAyear;
                ReassessmentMonth = RAmonth;

                SaleYearCutOff = Convert.ToInt32((RAyear - 5).ToString());
                Rates.ExtraKitRate = Convert.ToInt32(dr["rekit"].ToString());
                Rates.PierRate = Convert.ToDecimal(dr["rpier"].ToString());
                Rates.SlabRate = Convert.ToDecimal(dr["rslab"].ToString());
                Rates.AirCondRate = Convert.ToDecimal(dr["racamt"].ToString());
                Rates.OutBldRateLim = Convert.ToDecimal(dr["roblim"].ToString());

                DefDepCondG = Convert.ToDecimal(dr["rcondg"].ToString());
                DefDepCondA = Convert.ToDecimal(dr["rconda"].ToString());
                DefDepCondF = Convert.ToDecimal(dr["rcondf"].ToString());
                DefDepCondP = Convert.ToDecimal(dr["rcondp"].ToString());
            }
        }

        private static void GetRentalRates(DBAccessManager db)
        {
            StringBuilder getrents = new StringBuilder();
            getrents.Append(String.Format("select rid,rusecd,rtype,rclvr,rclgr,rclar,rclfr,rclpr,rmult,roiv,roig,roia,roif,roip,rvclv, " +
                " rvclg,rvcla,rvclf,rvclp,roerv,roerg,roera,roerf,roerp,rgimv,rgimg,rgima,rgimf, " +
                " rgimp from parrevlib.{0}rent order by rid,rusecd ", SketchUpGlobals.LocalityPreFix));

            try
            {
                DataSet ds_rentRates = db.RunSelectStatement(getrents.ToString());

                foreach (DataRow row in ds_rentRates.Tables[0].Rows)
                {
                    string rateDescription = String.Format("{0} - {1} ",
                        row["rusecd"].ToString(),
                        row["rtype"].ToString());

                    var rentRateValues = new RentRates()
                    {
                        _rid = row["rid"].ToString(),
                        _rUseCode = row["rusecd"].ToString(),
                        _rTypeDescription = row["rtype"].ToString(),
                        _rVeryGoodRate = Convert.ToDecimal(row["rclvr"].ToString()),
                        _rGoodRate = Convert.ToDecimal(row["rclgr"].ToString()),
                        _rAvgRate = Convert.ToDecimal(row["rclar"].ToString()),
                        _rFairRate = Convert.ToDecimal(row["rclfr"].ToString()),
                        _rPoorRate = Convert.ToDecimal(row["rclpr"].ToString()),
                        _rSizeMultiplier = Convert.ToDecimal(row["rmult"].ToString()),
                        _rOthIncVGood = Convert.ToDecimal(row["roiv"].ToString()),
                        _rOthIncGood = Convert.ToDecimal(row["roig"].ToString()),
                        _rOthIncAvg = Convert.ToDecimal(row["roia"].ToString()),
                        _rOthIncFair = Convert.ToDecimal(row["roif"].ToString()),
                        _rOthIncPoor = Convert.ToDecimal(row["roip"].ToString()),
                        _rVCLVGood = Convert.ToDecimal(row["rvclv"].ToString()),
                        _rVCLGood = Convert.ToDecimal(row["rvclg"].ToString()),
                        _rVCLAvg = Convert.ToDecimal(row["rvcla"].ToString()),
                        _rVCLFair = Convert.ToDecimal(row["rvclf"].ToString()),
                        _rVCLPoor = Convert.ToDecimal(row["rvclp"].ToString()),
                        _rOERVGood = Convert.ToDecimal(row["roerv"].ToString()),
                        _rOERGood = Convert.ToDecimal(row["roerg"].ToString()),
                        _rOERAvg = Convert.ToDecimal(row["roera"].ToString()),
                        _rOERFair = Convert.ToDecimal(row["roerf"].ToString()),
                        _rOERPoor = Convert.ToDecimal(row["roerp"].ToString()),
                        _rGIMVGood = Convert.ToDecimal(row["rgimv"].ToString()),
                        _rGIMGood = Convert.ToDecimal(row["rgimg"].ToString()),
                        _rGIMAvg = Convert.ToDecimal(row["rgima"].ToString()),
                        _rGIMFair = Convert.ToDecimal(row["rgimf"].ToString()),
                        _rGIMPoor = Convert.ToDecimal(row["rgimp"].ToString()),
                        _rPrintDescription = rateDescription.ToString().TrimEnd()
                    };
                    RentalRateCollection.Add(rentRateValues);
                }
            }
            catch
            {
            }
        }

        private static void GetSewerRates(DBAccessManager db)
        {
            DataSet ds_SewerRates = db.RunSelectStatement(String.Format(
                            " select rsecto,rdesc,rtid,rtelem,rrpa from {0}.{1}rat1 where rid = 'S' and rtid = 'SEW' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_SewerRates.Tables[0].Rows)
            {
                var sewRate = new SewerRates()
                {
                    SewerCode = Convert.ToInt32(row["rtelem"].ToString()),
                    SewerDescription = row["rdesc"].ToString().Trim(),
                    SewerRate = Convert.ToInt32(row["rrpa"].ToString()),
                };
                SewerRateCollection.Add(sewRate);
            }
        }

        private static void GetStabFileData(DBAccessManager db)
        {
            // Modified for location-specific STAB and DESC files -- JMM 04-13-2016
            DataSet ds_stab = db.RunSelectStatement(String.Format(
                "SELECT TTID,TTELEM,TDESCP,TDESC,TLOC FROM {0}.{1}STAB", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));
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

        private static void GetSubdivisionCodeDescriptions(DBAccessManager db)
        {
            DataSet ds_subdivCode = db.RunSelectStatement(String.Format(
                " select rsecto,rdesc,rincsf from {0}.{1}rat1 where rid = 'D' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_subdivCode.Tables[0].Rows)
            {
                string subDivCode = String.Format("{0} - {1}",
                    row["rsecto"].ToString().Substring(1, 3),
                    row["rdesc"].ToString().Trim());
                var subdivCode = new SubDivisionCodes()
                {
                    _subDivCode = row["rsecto"].ToString().Substring(1, 3),
                    _subDivDescription = row["rdesc"].ToString().Trim(),
                    _sudDivQuality = row["rincsf"].ToString().Trim(),
                    _printDescription = subDivCode.ToString().Trim(),
                };
                SubDivisionCodeCollection.Add(subdivCode);
            }
        }

        private static void GetUserCodeDescriptions(DBAccessManager db)
        {
            DataSet ds_user = db.RunSelectStatement(String.Format(
                " select rsecto,rdesc from {0}.{1}rat1 where rid = 'U' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_user.Tables[0].Rows)
            {
                string userCode = String.Format("{0} - {1}",
                    row["rsecto"].ToString().Substring(3, 1),
                    row["rdesc"].ToString().Trim());
                var usercode = new UserCodeType()
                {
                    _userCode = row["rsecto"].ToString().Substring(3, 1),
                    _userCodeDescription = row["rdesc"].ToString().Trim(),
                    _printUserCodeDescription = userCode.ToString().Trim()
                };
                UserCodeTypeCollection.Add(usercode);
            }
        }

        private static void GetWaterRates()
        {
            var ds_WaterRate = from l in Rates.Rate1Master.GetRat1TypeCollection(Rat1Master.Rat1Types.SiteCode)
                               select new WaterRates()
                               {
                                   WaterCode = int.Parse(l.Value.ElemCode.TrimEnd(new char[] { ' ' }).Substring(1)),
                                   WaterDescription = l.Value.Description,
                                   WaterRate = Convert.ToInt32(l.Value.Rate)
                               };

            //TODO: Ask Dave: Why was the AddRange in GetWaterRates commented out??
            WaterRateCollection.AddRange(ds_WaterRate);
        }

        private static void GetWaterRates(DBAccessManager db)
        {
            DataSet ds_WaterRates = db.RunSelectStatement(String.Format(
                            " select rsecto,rdesc,rtid,rtelem,rrpa from {0}.{1}rat1 where rid = 'S' and rtid = 'WAT' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            foreach (DataRow row in ds_WaterRates.Tables[0].Rows)
            {
                var watRate = new WaterRates()
                {
                    WaterCode = Convert.ToInt32(row["rtelem"].ToString()),
                    WaterDescription = row["rdesc"].ToString().Trim(),
                    WaterRate = Convert.ToInt32(row["rrpa"].ToString()),
                };
                WaterRateCollection.Add(watRate);
            }
        }

        public static string HSTypeDescription(this List<HomeSiteType> list, Int32 code)
        {
            var q = from h in list
                    where h.HSCode == code
                    select h.HSDescription;
            return q.SingleOrDefault();
        }

        public static void Init(SWallTech.CAMRA_Connection conn)
        {
            DBAccessManager db = conn.DBConnection;
            ClearValues();
            InitializeRatTableCollectionLists(conn);
            GetGasLogRate(db);
            GetNoMaxAc(db);
            GetRat2Data(db);
            GetCapitalizationRates(db);
            GetMapCoordinates(db);
            GetRentalRates(db);
            GetHomesiteDescription();
            GetLandDescription();
            GetLandUseDescription();
            GetWaterRates();
            GetDeckTypes(db);
            GetAllPorchTypes(db);
            GetPatioTypes(db);
            GetGarageTypes(db);
            GetCarportTypes(db);
            GetUserCodeDescriptions(db);
            GetSubdivisionCodeDescriptions(db);
            GetMagisterialDistricts(db);
            GetWaterRates(db);
            GetSewerRates(db);
            GetOtherImprovementCodes(db);
            GetAssessorInitalCodeDescriptions(db);
            GetDataEntryOperatorDescriptions(db);
            GetOccupancyListAndDescriptions(db);
            GetBuildingSectionTypesAndRates(db);
            GetStabFileData(db);
        }

        private static void InitializeRatTableCollectionLists(CAMRA_Connection conn)
        {
            // Initialize Static variables

            Rates.Rate1Master = new Rat1Master(conn);

            AirCondCollection = new List<StabType>();
            AirCondCollection_srt = new List<StabTypeD>();
            AllStructureDescCollection = new List<AllStructureSections>();
            AssessorCodeCollection = new List<AssessorCodes>();
            AttachedSectionTypeCollection = new List<AttachedSectionTypes>();
            BasementTypeCollection = new List<StabType>();
            BasementTypeCollection_srt = new List<StabTypeD>();
            CarPortTypeCollection = new List<StabType>();
            CarPortTypeCollection_srt = new List<StabTypeD>();
            CharacteristicDescriptionCollection = new List<CharacteristicTypeDescription>();
            CharacteristicDescriptionCollection_srt = new List<CharacteristicTypeDescriptionD>();
            CharacteristicTypeCollection = new List<StabType>();
            CharacteristicTypeCollection_srt = new List<StabTypeD>();
            ClassCollection = new List<StabType>();
            ClassCollection_srt = new List<StabTypeD>();
            CommercialIncomeSectionCollection = new List<CommercialIncomeSections>();
            CommercialRateCollection = new List<CommercialRate>();
            CommercialRateCollection_srt = new List<CommercialRateD>();
            CommercialSectionTypeCollection = new List<CommercialSections>();
            ConditionTypeCollection = new List<StabType>();
            ConditionTypeCollection = new List<StabType>();
            ConditionTypeCollection_srt = new List<StabTypeD>();
            DataEntryOperatorCollection = new List<DataEntryOperatorCode>();
            DisimilarityWeightCollection = new List<DismWeights>();
            DoorKnockerCollection = new List<StabType>();
            DoorKnockerCollection_srt = new List<StabTypeD>();
            EasementTypeCollection = new List<StabType>();
            EasementTypeCollection_srt = new List<StabTypeD>();
            ExteriorWallTypeCollection = new List<StabType>();
            ExteriorWallTypeCollection_srt = new List<StabTypeD>();
            FloorAbreviationCollection = new List<FloorAbreviation>();
            FloorCoverCollection = new List<StabType>();
            FloorCoverCollection_srt = new List<StabTypeD>();
            FoundationCollection = new List<StabType>();
            FoundationCollection_srt = new List<StabTypeD>();
            FuelTypeCollection = new List<StabType>();
            FuelTypeCollection_srt = new List<StabTypeD>();
            GarageTypeCollection = new List<StabType>();
            GarageTypeCollection_srt = new List<StabTypeD>();
            HeatTypeCollection = new List<StabType>();
            HeatTypeCollection_srt = new List<StabTypeD>();
            HomeSiteTypeCollection = new List<HomeSiteType>();
            HomeSiteTypeCollection_srt = new List<HomeSiteTypeD>();
            InteriorWallTypeCollection = new List<StabType>();
            InteriorWallTypeCollection_srt = new List<StabTypeD>();
            LandTypeCollection = new List<LandType>();
            LandTypeCollection_srt = new List<LandTypeD>();
            LandUseTypeCollection = new List<LandUseType>();
            LivingAreaSectionTypeCollection = new List<LivingAreaSectionTypes>();
            MagDistrictCollection = new List<MagDistrictCodes>();
            MapCoordinateCollection = new List<MapCoordinates>();
            NonBasementTypeCollection = new List<StabType>();
            NonBasementTypeCollection_srt = new List<StabTypeD>();
            OccupancyCollection = new List<StabType>();
            OccupancyCollection_srt = new List<StabTypeD>();
            OccupancyDescriptionCollection = new List<OccupancyDescription>();
            OccupancyDescriptionCollection_srt = new List<OccupancyDescriptionD>();
            OtherImprovementCollection = new List<OtherImprovement>();
            OtherImprovementCollection_srt = new List<OtherImprovementD>();
            PavementDescriptionCollection = new List<PavementTypeDescription>();
            PavementDescriptionCollection_srt = new List<PavementTypeDescriptionD>();
            Rates.ClassValues = new SortedDictionary<string, decimal>();
            RentalRateCollection = new List<RentRates>();
            ResidentialSectionTypeCollection = new List<ResidentialSections>();
            RightofWayDescriptionCollection = new List<RightofWayTypeDescription>();
            RightofWayDescriptionCollection_srt = new List<RightofWayTypeDescriptionD>();
            RightofWayTypeCollection = new List<StabType>();
            RightofWayTypeCollection_srt = new List<StabTypeD>();
            RoofingCollection = new List<StabType>();
            RoofingCollection_srt = new List<StabTypeD>();
            RoofTypeCollection = new List<StabType>();
            RoofTypeCollection_srt = new List<StabTypeD>();
            SewerDescriptionCollection = new List<SewerTypeDescription>();
            SewerDescriptionCollection_srt = new List<SewerTypeDescriptionD>();
            SewerRateCollection = new List<SewerRates>();
            SewerTypeCollection = new List<StabType>();
            SewerTypeCollection_srt = new List<StabTypeD>();
            StdDeviationCollection = new List<StdDeviations>();
            SubDivisionCodeCollection = new List<SubDivisionCodes>();
            SubDivisionCodeCollection_srt = new List<SubDivisionCodesD>();
            TerrainDescriptionCollection = new List<TerrainTypeDescription>();
            TerrainDescriptionCollection_srt = new List<TerrainTypeDescriptionD>();
            TerrainTypeCollection = new List<StabType>();
            TerrainTypeCollection_srt = new List<StabTypeD>();
            UserCodeTypeCollection = new List<UserCodeType>();
            WallAbreviationCollection = new List<InWallAbreviation>();
            WaterDescriptionCollection = new List<WaterTypeDescription>();
            WaterDescriptionCollection_srt = new List<WaterTypeDescriptionD>();
            WaterRateCollection = new List<WaterRates>();
            WaterTypeCollection = new List<StabType>();
            WaterTypeCollection_srt = new List<StabTypeD>();
            ZoningDescriptionCollection = new List<ZoningDescription>();
            ZoningDescriptionCollection_srt = new List<ZoningDescriptionD>();
        }

        public static string LandDescription(this List<LandType> list, Int32 code)
        {
            var q = from h in list
                    where h.LandCode == code
                    select h.LandDescription;
            return q.SingleOrDefault();
        }

        public static MapCoordinates MapCoordinate(this List<MapCoordinates> list, string code)
        {
            var q = from h in list
                    where h._subMap == code
                    select h;

            return q.SingleOrDefault();
        }

        private static void ParseRatTableDataToLists(List<StabType> allStabTypes)
        {
            var BAS = (from t in allStabTypes where t.Type == "BAS" select t).ToList<StabType>();
            var CAR = (from t in allStabTypes where t.Type == "CAR" select t).ToList<StabType>();
            var CHR = (from t in allStabTypes where t.Type == "CHR" select t).ToList<StabType>();
            var CON = (from t in allStabTypes where t.Type == "CON" select t).ToList<StabType>();
            var EAS = (from t in allStabTypes where t.Type == "EAS" select t).ToList<StabType>();
            var EXW = (from t in allStabTypes where t.Type == "EXW" select t).ToList<StabType>();
            var FLR = (from t in allStabTypes where t.Type == "FLR" select t).ToList<StabType>();
            var FND = (from t in allStabTypes where t.Type == "FND" select t).ToList<StabType>();
            var FUL = (from t in allStabTypes where t.Type == "FUL" select t).ToList<StabType>();
            var GAR = (from t in allStabTypes where t.Type == "GAR" select t).ToList<StabType>();
            var HT = (from t in allStabTypes where t.Type == "HT" select t).ToList<StabType>();
            var INW = (from t in allStabTypes where t.Type == "INW" select t).ToList<StabType>();
            var OCC = (from t in allStabTypes where t.Type == "OCC" select t).ToList<StabType>();
            var RFG = (from t in allStabTypes where t.Type == "RFG" select t).ToList<StabType>();
            var RFT = (from t in allStabTypes where t.Type == "RFT" select t).ToList<StabType>();
            var ROW = (from t in allStabTypes where t.Type == "ROW" select t).ToList<StabType>();
            var SEW = (from t in allStabTypes where t.Type == "SEW" select t).ToList<StabType>();
            var TER = (from t in allStabTypes where t.Type == "TER" select t).ToList<StabType>();
            var WAT = (from t in allStabTypes where t.Type == "WAT" select t).ToList<StabType>();
            CarPortTypeCollection.AddRange(CAR);
            CharacteristicTypeCollection.AddRange(CHR);
            ConditionTypeCollection.AddRange(CON);
            EasementTypeCollection.AddRange(EAS);
            ExteriorWallTypeCollection.AddRange(EXW);
            FloorCoverCollection.AddRange(FLR);
            FoundationCollection.AddRange(FND);
            FuelTypeCollection.AddRange(FUL);
            GarageTypeCollection.AddRange(GAR);
            HeatTypeCollection.AddRange(HT);
            InteriorWallTypeCollection.AddRange(INW);
            NonBasementTypeCollection.AddRange(BAS);
            OccupancyCollection.AddRange(OCC);
            RightofWayTypeCollection.AddRange(ROW);
            RoofingCollection.AddRange(RFG);
            RoofTypeCollection.AddRange(RFT);
            SewerTypeCollection.AddRange(SEW);
            TerrainTypeCollection.AddRange(TER);
            WaterTypeCollection.AddRange(WAT);
        }

        public static decimal ResidentialSectionRate(this List<ResidentialSections> list, string code)
        {
            var q = from h in list
                    where h._resSectionType == code
                    select h._resSectionRate;
            return q.SingleOrDefault();
        }

        public static string ResidentialSectionTypeDescription(this List<ResidentialSections> list, string code)
        {
            var q = from h in list
                    where h._resSectionType == code
                    select h._resSectionDescription;
            return q.SingleOrDefault();
        }

        public static string SewerTypeDescription(this List<SewerRates> list, Int32 code)
        {
            var q = from h in list
                    where h.SewerCode == code
                    select h.SewerDescription;
            return q.SingleOrDefault();
        }

        public static StdDeviations StdDeviation(this List<StdDeviations> list, decimal code)
        {
            var q = from h in list
                    where h._sdStory == code
                    select h;

            return q.SingleOrDefault();
        }

        public static WaterRates WaterTypes(this List<WaterRates> list, Int32 code)
        {
            var q = from h in list
                    where h.WaterCode == code
                    select h;

            return q.SingleOrDefault();
        }

        public static string _assInitDescription(this List<AssessorCodes> list, string code)
        {
            var q = from h in list
                    where h._assInitCode == code
                    select h._assInitDescription;

            return q.SingleOrDefault();
        }

        public static string _dataEntryOpInitDescription(this List<DataEntryOperatorCode> list, string code)
        {
            var q = from h in list
                    where h._dataEntryOpCode == code
                    select h._dataEntryOpDescription;

            return q.SingleOrDefault();
        }

        public static string _floorAbreviation(this List<FloorAbreviation> list, string code)
        {
            var q = from h in list
                    where h.FlrCode == code
                    select h.FlrAbreviation;

            return q.SingleOrDefault();
        }

        public static string _InWallAbreviation(this List<InWallAbreviation> list, string code)
        {
            var q = from h in list
                    where h.WallCode == code
                    select h.WallAbreviation;

            return q.SingleOrDefault();
        }

        public static string _landUseDescription(this List<LandUseType> list, string code)
        {
            var q = from h in list
                    where h._landUseCode == code
                    select h._landUseDescription;
            return q.SingleOrDefault();
        }

        public static string _magDistDescription(this List<MagDistrictCodes> list, string code)
        {
            var q = from h in list
                    where h._magDistCode == code
                    select h._magDistDescription;
            return q.SingleOrDefault();
        }

        public static string _subDivDescription(this List<SubDivisionCodes> list, string code)
        {
            var q = from h in list
                    where h._subDivCode == code
                    select h._subDivDescription;

            return q.SingleOrDefault();
        }

        public static string _userCodeDescription(this List<UserCodeType> list, string code)
        {
            var q = from h in list
                    where h._userCode == code
                    select h._userCodeDescription;
            return q.SingleOrDefault();
        }

        public static string ListCommercialOccupancies
        {
            get
            {
                string s = String.Empty;
                for (int i = 0; i < CommercialOccupancies.Count; i++)
                {
                    if (i == 0)
                    {
                        s += String.Format("{0}", CommercialOccupancies[i]);
                    }
                    else
                    {
                        s += String.Format(",{0}", CommercialOccupancies[i]);
                    }
                }
                return s;
            }
        }

        public static string ListIncomeOccupancies
        {
            get
            {
                string s = String.Empty;
                for (int i = 0; i < IncomeOccupancies.Count; i++)
                {
                    if (i == 0)
                    {
                        s += String.Format("{0}", IncomeOccupancies[i]);
                    }
                    else
                    {
                        s += String.Format(",{0}", IncomeOccupancies[i]);
                    }
                }
                return s;
            }
        }

        public static string ListResidentialOccupancies
        {
            get
            {
                string s = String.Empty;
                for (int i = 0; i < ResidentialOccupancies.Count; i++)
                {
                    if (i == 0)
                    {
                        s += String.Format("{0}", ResidentialOccupancies[i]);
                    }
                    else
                    {
                        s += String.Format(",{0}", ResidentialOccupancies[i]);
                    }
                }
                return s;
            }
        }

        public static string ListVacantOccupancies
        {
            get
            {
                string s = String.Empty;
                for (int i = 0; i < VacantOccupancies.Count; i++)
                {
                    if (i == 0)
                    {
                        s += String.Format("{0}", VacantOccupancies[i]);
                    }
                    else
                    {
                        s += String.Format(",{0}", VacantOccupancies[i]);
                    }
                }
                return s;
            }
        }

        public static List<StabType> AirCondCollection;
        public static List<StabTypeD> AirCondCollection_srt;
        public static List<AllStructureSections> AllStructureDescCollection;
        public static decimal AppDepChgRate;
        public static List<AssessorCodes> AssessorCodeCollection;
        public static List<AttachedSectionTypes> AttachedSectionTypeCollection;
        public static List<string> AuxAreaTypes = new List<string>() { "BEGR", "EGAR", "FEGR", "RMAD", "SUNR", "RMAF", "RMAP", "RMTS" };
        public static List<StabType> BasementTypeCollection;
        public static List<StabTypeD> BasementTypeCollection_srt;
        public static decimal BltInRate;
        public static decimal BltInRate2;
        public static List<StabType> CarPortTypeCollection;
        public static List<StabTypeD> CarPortTypeCollection_srt;

        //public static List<string> CarPortTypes = new List<string>() { "CP", "BCP", "WCP", "BWCP", "UCP", "CPB", "CPU", "CPW", "CPWB" };
        public static List<string> CarPortTypes = new List<string>();
        public static List<CharacteristicTypeDescription> CharacteristicDescriptionCollection;
        public static List<CharacteristicTypeDescriptionD> CharacteristicDescriptionCollection_srt;
        public static List<StabType> CharacteristicTypeCollection;
        public static List<StabTypeD> CharacteristicTypeCollection_srt;
        public static List<StabType> ClassCollection;
        public static List<StabTypeD> ClassCollection_srt;
        public static List<CommercialIncomeSections> CommercialIncomeSectionCollection;
        public static List<int> CommercialOccupancies = new List<int>() { 11, 13, 14, 26 };
        public static List<CommercialRate> CommercialRateCollection;
        public static List<CommercialRateD> CommercialRateCollection_srt;
        public static List<CommercialSections> CommercialSectionTypeCollection;
        public static List<CommercialSections> CommercialSectionTypeDescriptionCollection;
        public static List<string> CommIndLandUseTypes = new List<string>() { "4", "5" };
        public static List<StabType> ConditionTypeCollection;
        public static List<StabTypeD> ConditionTypeCollection_srt;
        public static List<DataEntryOperatorCode> DataEntryOperatorCollection;

        //public static List<string> DeckTypes = new List<string>() { "DECK", "DEK", "DK", "DEKA", "DEKG" };
        public static List<string> DeckTypes = new List<string>();
        public static decimal DefDepCondA;
        public static decimal DefDepCondF;
        public static decimal DefDepCondG;
        public static decimal DefDepCondP;
        public static decimal DiscountRate;
        public static List<DismWeights> DisimilarityWeightCollection;
        public static List<StabType> DoorKnockerCollection;
        public static List<StabTypeD> DoorKnockerCollection_srt;
        public static List<StabType> EasementTypeCollection;
        public static List<StabTypeD> EasementTypeCollection_srt;
        public static decimal EffectiveTaxRate;

        //public static List<string> EnclPorchTypes = new List<string>() { "EPOR", "EPR", "JPOR", "POEB", "POEF", "PORJ" };
        public static List<string> EnclPorchTypes = new List<string>();
        public static decimal EquityYieldRate;
        public static List<StabType> ExteriorWallTypeCollection;
        public static List<StabTypeD> ExteriorWallTypeCollection_srt;
        public static List<FloorAbreviation> FloorAbreviationCollection;
        public static List<StabType> FloorCoverCollection;
        public static List<StabTypeD> FloorCoverCollection_srt;
        public static List<StabType> FoundationCollection;
        public static List<StabTypeD> FoundationCollection_srt;
        public static List<StabType> FuelTypeCollection;
        public static List<StabTypeD> FuelTypeCollection_srt;
        public static int FullBathCnt;
        public static List<StabType> GarageTypeCollection;
        public static List<StabTypeD> GarageTypeCollection_srt;

        //public static List<string> GarageTypes = new List<string>() { "GAR", "BGAR", "FGAR", "UGAR", "GARL","GARB","GARF","GABK","GACB","GCEB",
        //                                                              "GAFV","GACB","GALF","GAUB","GAUF","GCEF" };
        public static List<string> GarageTypes = new List<string>();
        public static int HalfBathCnt;
        public static List<StabType> HeatTypeCollection;
        public static List<StabTypeD> HeatTypeCollection_srt;
        public static int HoldingPeriod;
        public static List<HomeSiteType> HomeSiteTypeCollection;
        public static List<HomeSiteTypeD> HomeSiteTypeCollection_srt;
        public static decimal IncomeChgRate;
        public static List<int> IncomeOccupancies = new List<int>() { 11, 13, 14 };
        public static List<StabType> InteriorWallTypeCollection;
        public static List<StabTypeD> InteriorWallTypeCollection_srt;
        public static List<string> InvalidCommercialSection = new List<string>() { "BASE", "ADD", "NBAD", "LAG", "OH" };
        public static List<LandType> LandTypeCollection;
        public static List<LandTypeD> LandTypeCollection_srt;
        public static List<LandUseType> LandUseTypeCollection;
        public static List<LivingAreaSectionTypes> LivingAreaSectionTypeCollection;
        public static decimal LoanToValueRatio;
        public static List<MagDistrictCodes> MagDistrictCollection;
        public static List<MapCoordinates> MapCoordinateCollection;
        public static int max1stysf;
        public static int max2stysf;
        public static int maxhstysf;
        public static int min1stysf;
        public static int min2stysf;
        public static int minhstysf;
        public static string MortgageID;
        public static decimal MortgageRate;
        public static int MortgageTerm;
        public static string NoMaxAc;
        public static List<StabType> NonBasementTypeCollection;
        public static List<StabTypeD> NonBasementTypeCollection_srt;
        public static List<StabType> OccupancyCollection;
        public static List<StabTypeD> OccupancyCollection_srt;
        public static List<OccupancyDescription> OccupancyDescriptionCollection;
        public static List<OccupancyDescriptionD> OccupancyDescriptionCollection_srt;
        public static decimal OperExpInitChg;
        public static decimal OperExpTermChg;
        public static List<OtherImprovement> OtherImprovementCollection;
        public static List<OtherImprovementD> OtherImprovementCollection_srt;

        //public static List<string> PatioTypes = new List<string>() { "PAT", "BPAT", "CPAT", "WPAT", "PATO", "PABK", "PACN", "PACV", "PATW" };
        public static List<string> PatioTypes = new List<string>();
        public static List<PavementTypeDescription> PavementDescriptionCollection;
        public static List<PavementTypeDescriptionD> PavementDescriptionCollection_srt;

        //public static List<string> PorchTypes = new List<string>() { "POR" };
        public static List<string> PorchTypes = new List<string>();
        public static List<int> PrintStructureOccupancies = new List<int>() { 10, 12, 16, 20, 21, 22, 24, 11, 13, 14, 17, 26 };
        public static int ProjectionPeriod;
        public static string ReassessmentDate;
        public static int ReassessmentDay;
        public static int ReassessmentMonth;
        public static int ReassessmentYear;
        public static List<RentRates> RentalRateCollection;
        public static List<string> ResidentialLandUseTypes = new List<string>() { "1", "2", "3", "5", "6" };
        public static List<int> ResidentialOccupancies = new List<int>() { 10, 12, 16, 20, 21, 22, 24 };
        public static List<ResidentialSections> ResidentialSectionTypeCollection;
        public static List<ResidentialSections> ResidentialSectionTypeDescriptionCollection;
        public static decimal ReversionCommission;
        public static List<RightofWayTypeDescription> RightofWayDescriptionCollection;
        public static List<RightofWayTypeDescriptionD> RightofWayDescriptionCollection_srt;
        public static List<StabType> RightofWayTypeCollection;
        public static List<StabTypeD> RightofWayTypeCollection_srt;
        public static List<StabType> RoofingCollection;
        public static List<StabTypeD> RoofingCollection_srt;
        public static List<StabType> RoofTypeCollection;
        public static List<StabTypeD> RoofTypeCollection_srt;
        public static int SaleYearCutOff;

        //public static List<string> ScrnPorchTypes = new List<string>() { "SPOR", "PORS" };
        public static List<string> ScrnPorchTypes = new List<string>();
        public static List<SewerTypeDescription> SewerDescriptionCollection;
        public static List<SewerTypeDescriptionD> SewerDescriptionCollection_srt;
        public static List<SewerRates> SewerRateCollection;
        public static List<StabType> SewerTypeCollection;
        public static List<StabTypeD> SewerTypeCollection_srt;
        public static int StandardComDepth;
        public static int StandardResDepth;
        public static List<StdDeviations> StdDeviationCollection;
        public static decimal sty1thwtype1base;
        public static decimal sty1thwtype1factr;
        public static decimal sty1thwtype2base;
        public static decimal sty1thwtype2factr;
        public static decimal sty1thwtype3base;
        public static decimal sty1thwtype3factr;
        public static decimal sty1wtype1base;
        public static decimal sty1wtype1factr;
        public static decimal sty1wtype2base;
        public static decimal sty1wtype2factr;
        public static decimal sty1wtype3base;
        public static decimal sty1wtype3factr;
        public static decimal sty2thwtype1base;
        public static decimal sty2thwtype1factr;
        public static decimal sty2thwtype2base;
        public static decimal sty2thwtype2factr;
        public static decimal sty2thwtype3base;
        public static decimal sty2thwtype3factr;
        public static decimal sty2wtype1base;
        public static decimal sty2wtype1factr;
        public static decimal sty2wtype2base;
        public static decimal sty2wtype2factr;
        public static decimal sty2wtype3base;
        public static decimal sty2wtype3factr;
        public static decimal styhthwtype1base;
        public static decimal styhthwtype1factr;
        public static decimal styhthwtype2base;
        public static decimal styhthwtype2factr;
        public static decimal styhthwtype3base;
        public static decimal styhthwtype3factr;
        public static decimal styhwtype1base;
        public static decimal styhwtype1factr;
        public static decimal styhwtype2base;
        public static decimal styhwtype2factr;
        public static decimal styhwtype3base;
        public static decimal styhwtype3factr;
        public static List<SubDivisionCodes> SubDivisionCodeCollection;
        public static List<SubDivisionCodesD> SubDivisionCodeCollection_srt;
        public static List<int> TaxExemptOccupancies = new List<int>() { 17 };
        public static decimal TermCapRateAdj;
        public static List<TerrainTypeDescription> TerrainDescriptionCollection;
        public static List<TerrainTypeDescriptionD> TerrainDescriptionCollection_srt;
        public static List<StabType> TerrainTypeCollection;
        public static List<StabTypeD> TerrainTypeCollection_srt;
        public static List<UserCodeType> UserCodeTypeCollection;
        public static decimal VacancyChgRate;
        public static List<int> VacantOccupancies = new List<int>() { 5, 15, 23, 25, 27 };
        public static List<InWallAbreviation> WallAbreviationCollection;
        public static List<WaterTypeDescription> WaterDescriptionCollection;
        public static List<WaterTypeDescriptionD> WaterDescriptionCollection_srt;
        public static List<WaterRates> WaterRateCollection;
        public static List<StabType> WaterTypeCollection;
        public static List<StabTypeD> WaterTypeCollection_srt;
        public static List<ZoningDescription> ZoningDescriptionCollection;
        public static List<ZoningDescriptionD> ZoningDescriptionCollection_srt;
    }
}
