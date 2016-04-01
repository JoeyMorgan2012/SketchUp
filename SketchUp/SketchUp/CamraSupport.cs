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

		public static DismWieghts DisWght(this List<DismWieghts> list, decimal code)
		{
			var q = from h in list
					where h._wtStory == code
					select h;

			return q.SingleOrDefault();
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
			string prefix = conn.LocalityPrefix;
			string localLibrary = conn.Library;

			ClearValues();

			#region Initialize empty List<T> variables

			// Initialize Static variables
			if (Rates.Rate1Master == null)
			{
				Rates.Rate1Master = new Rat1Master(conn);
			}
			if (Rates.ClassValues == null)
			{
				Rates.ClassValues = new SortedDictionary<string, decimal>();
			}
			if (DoorKnockerCollection == null)
			{
				DoorKnockerCollection = new List<StabType>();
			}
			if (DoorKnockerCollection_srt == null)
			{
				DoorKnockerCollection_srt = new List<StabTypeD>();
			}

			if (HeatTypeCollection == null)
			{
				HeatTypeCollection = new List<StabType>();
			}
			if (HeatTypeCollection_srt == null)
			{
				HeatTypeCollection_srt = new List<StabTypeD>();
			}
			if (AirCondCollection == null)
			{
				AirCondCollection = new List<StabType>();
			}
			if (AirCondCollection_srt == null)
			{
				AirCondCollection_srt = new List<StabTypeD>();
			}
			if (FuelTypeCollection == null)
			{
				FuelTypeCollection = new List<StabType>();
			}
			if (FuelTypeCollection_srt == null)
			{
				FuelTypeCollection_srt = new List<StabTypeD>();
			}
			if (ExteriorWallTypeCollection == null)
			{
				ExteriorWallTypeCollection = new List<StabType>();
			}
			if (ExteriorWallTypeCollection_srt == null)
			{
				ExteriorWallTypeCollection_srt = new List<StabTypeD>();
			}
			if (FoundationCollection == null)
			{
				FoundationCollection = new List<StabType>();
			}
			if (FoundationCollection_srt == null)
			{
				FoundationCollection_srt = new List<StabTypeD>();
			}
			if (RoofingCollection == null)
			{
				RoofingCollection = new List<StabType>();
			}
			if (RoofingCollection_srt == null)
			{
				RoofingCollection_srt = new List<StabTypeD>();
			}
			if (RoofTypeCollection == null)
			{
				RoofTypeCollection = new List<StabType>();
			}
			if (RoofTypeCollection_srt == null)
			{
				RoofTypeCollection_srt = new List<StabTypeD>();
			}
			if (InteriorWallTypeCollection == null)
			{
				InteriorWallTypeCollection = new List<StabType>();
			}
			if (InteriorWallTypeCollection_srt == null)
			{
				InteriorWallTypeCollection_srt = new List<StabTypeD>();
			}
			if (FloorCoverCollection == null)
			{
				FloorCoverCollection = new List<StabType>();
			}
			if (FloorCoverCollection_srt == null)
			{
				FloorCoverCollection_srt = new List<StabTypeD>();
			}
			if (OccupancyCollection == null)
			{
				OccupancyCollection = new List<StabType>();
			}
			if (OccupancyCollection_srt == null)
			{
				OccupancyCollection_srt = new List<StabTypeD>();
			}
			if (SewerTypeCollection == null)
			{
				SewerTypeCollection = new List<StabType>();
			}
			if (SewerTypeCollection_srt == null)
			{
				SewerTypeCollection_srt = new List<StabTypeD>();
			}
			if (WaterTypeCollection == null)
			{
				WaterTypeCollection = new List<StabType>();
			}
			if (WaterTypeCollection_srt == null)
			{
				WaterTypeCollection_srt = new List<StabTypeD>();
			}
			if (RightofWayTypeCollection == null)
			{
				RightofWayTypeCollection = new List<StabType>();
			}
			if (RightofWayTypeCollection_srt == null)
			{
				RightofWayTypeCollection_srt = new List<StabTypeD>();
			}
			if (EasementTypeCollection == null)
			{
				EasementTypeCollection = new List<StabType>();
			}
			if (EasementTypeCollection_srt == null)
			{
				EasementTypeCollection_srt = new List<StabTypeD>();
			}
			if (TerrainTypeCollection == null)
			{
				TerrainTypeCollection = new List<StabType>();
			}
			if (TerrainTypeCollection_srt == null)
			{
				TerrainTypeCollection_srt = new List<StabTypeD>();
			}
			if (CharacteristicTypeCollection == null)
			{
				CharacteristicTypeCollection = new List<StabType>();
			}
			if (CharacteristicTypeCollection_srt == null)
			{
				CharacteristicTypeCollection_srt = new List<StabTypeD>();
			}
			if (GarageTypeCollection == null)
			{
				GarageTypeCollection = new List<StabType>();
			}
			if (GarageTypeCollection_srt == null)
			{
				GarageTypeCollection_srt = new List<StabTypeD>();
			}
			if (CarPortTypeCollection == null)
			{
				CarPortTypeCollection = new List<StabType>();
			}
			if (CarPortTypeCollection_srt == null)
			{
				CarPortTypeCollection_srt = new List<StabTypeD>();
			}
			if (BasementTypeCollection == null)
			{
				BasementTypeCollection = new List<StabType>();
			}
			if (BasementTypeCollection_srt == null)
			{
				BasementTypeCollection_srt = new List<StabTypeD>();
			}
			if (NonBasementTypeCollection == null)
			{
				NonBasementTypeCollection = new List<StabType>();
			}
			if (NonBasementTypeCollection_srt == null)
			{
				NonBasementTypeCollection_srt = new List<StabTypeD>();
			}
			if (ConditionTypeCollection == null)
			{
				ConditionTypeCollection = new List<StabType>();
			}
			if (ConditionTypeCollection_srt == null)
			{
				ConditionTypeCollection_srt = new List<StabTypeD>();
			}
			if (ClassCollection == null)
			{
				ClassCollection = new List<StabType>();
			}
			if (ClassCollection_srt == null)
			{
				ClassCollection_srt = new List<StabTypeD>();
			}

			if (LandTypeCollection == null)
			{
				LandTypeCollection = new List<LandType>();
			}
			if (LandTypeCollection_srt == null)
			{
				LandTypeCollection_srt = new List<LandTypeD>();
			}
			if (LandUseTypeCollection == null)
			{
				LandUseTypeCollection = new List<LandUseType>();
			}
			if (HomeSiteTypeCollection == null)
			{
				HomeSiteTypeCollection = new List<HomeSiteType>();
			}
			if (HomeSiteTypeCollection_srt == null)
			{
				HomeSiteTypeCollection_srt = new List<HomeSiteTypeD>();
			}
			if (CommercialRateCollection == null)
			{
				CommercialRateCollection = new List<CommercialRate>();
			}
			if (CommercialRateCollection_srt == null)
			{
				CommercialRateCollection_srt = new List<CommercialRateD>();
			}
			if (WaterRateCollection == null)
			{
				WaterRateCollection = new List<WaterRates>();
			}

			if (SewerRateCollection == null)
			{
				SewerRateCollection = new List<SewerRates>();
			}

			if (ResidentialSectionTypeCollection == null)
			{
				ResidentialSectionTypeCollection = new List<ResidentialSections>();
			}
			if (AttachedSectionTypeCollection == null)
			{
				AttachedSectionTypeCollection = new List<AttachedSectionTypes>();
			}
			if (LivingAreaSectionTypeCollection == null)
			{
				LivingAreaSectionTypeCollection = new List<LivingAreaSectionTypes>();
			}
			if (ConditionTypeCollection == null)
			{
				ConditionTypeCollection = new List<StabType>();
			}
			if (UserCodeTypeCollection == null)
			{
				UserCodeTypeCollection = new List<UserCodeType>();
			}
			if (CommercialSectionTypeCollection == null)
			{
				CommercialSectionTypeCollection = new List<CommercialSections>();
			}

			if (CommercialIncomeSectionCollection == null)
			{
				CommercialIncomeSectionCollection = new List<CommercialIncomeSections>();
			}

			if (SubDivisionCodeCollection == null)
			{
				SubDivisionCodeCollection = new List<SubDivisionCodes>();
			}
			if (SubDivisionCodeCollection_srt == null)
			{
				SubDivisionCodeCollection_srt = new List<SubDivisionCodesD>();
			}

			if (MagDistrictCollection == null)
			{
				MagDistrictCollection = new List<MagDistrictCodes>();
			}

			if (AssessorCodeCollection == null)
			{
				AssessorCodeCollection = new List<AssessorCodes>();
			}

			if (DataEntryOperatorCollection == null)
			{
				DataEntryOperatorCollection = new List<DataEntryOperatorCode>();
			}

			if (OccupancyDescriptionCollection == null)
			{
				OccupancyDescriptionCollection = new List<OccupancyDescription>();
			}
			if (OccupancyDescriptionCollection_srt == null)
			{
				OccupancyDescriptionCollection_srt = new List<OccupancyDescriptionD>();
			}
			if (RightofWayDescriptionCollection == null)
			{
				RightofWayDescriptionCollection = new List<RightofWayTypeDescription>();
			}
			if (RightofWayDescriptionCollection_srt == null)
			{
				RightofWayDescriptionCollection_srt = new List<RightofWayTypeDescriptionD>();
			}
			if (PavementDescriptionCollection == null)
			{
				PavementDescriptionCollection = new List<PavementTypeDescription>();
			}
			if (PavementDescriptionCollection_srt == null)
			{
				PavementDescriptionCollection_srt = new List<PavementTypeDescriptionD>();
			}
			if (TerrainDescriptionCollection == null)
			{
				TerrainDescriptionCollection = new List<TerrainTypeDescription>();
			}
			if (TerrainDescriptionCollection_srt == null)
			{
				TerrainDescriptionCollection_srt = new List<TerrainTypeDescriptionD>();
			}
			if (CharacteristicDescriptionCollection == null)
			{
				CharacteristicDescriptionCollection = new List<CharateristicTypeDescription>();
			}
			if (CharacteristicDescriptionCollection_srt == null)
			{
				CharacteristicDescriptionCollection_srt = new List<CharateristicTypeDescriptionD>();
			}
			if (WaterDescriptionCollection == null)
			{
				WaterDescriptionCollection = new List<WaterTypeDescription>();
			}
			if (WaterDescriptionCollection_srt == null)
			{
				WaterDescriptionCollection_srt = new List<WaterTypeDescriptionD>();
			}
			if (SewerDescriptionCollection == null)
			{
				SewerDescriptionCollection = new List<SewerTypeDescription>();
			}
			if (SewerDescriptionCollection_srt == null)
			{
				SewerDescriptionCollection_srt = new List<SewerTypeDescriptionD>();
			}
			if (ZoningDescriptionCollection == null)
			{
				ZoningDescriptionCollection = new List<ZoningDescription>();
			}
			if (ZoningDescriptionCollection_srt == null)
			{
				ZoningDescriptionCollection_srt = new List<ZoningDescriptionD>();
			}

			if (MapCoordinateCollection == null)
			{
				MapCoordinateCollection = new List<MapCoordinates>();
			}
			if (AllStructureDescCollection == null)
			{
				AllStructureDescCollection = new List<AllStructureSections>();
			}

			if (StdDeviationCollection == null)
			{
				StdDeviationCollection = new List<StdDeviations>();
			}

			if (DisimilarityWeightCollection == null)
			{
				DisimilarityWeightCollection = new List<DismWieghts>();
			}

			if (RentalRateCollection == null)
			{
				RentalRateCollection = new List<RentRates>();
			}

			if (FloorAbreviationCollection == null)
			{
				FloorAbreviationCollection = new List<FloorAbreviation>();
			}

			if (WallAbreviationCollection == null)
			{
				WallAbreviationCollection = new List<InWallAbreviation>();
			}

			if (OtherImprovementCollection == null)
			{
				OtherImprovementCollection = new List<OtherImprovement>();
			}
			if (OtherImprovementCollection_srt == null)
			{
				OtherImprovementCollection_srt = new List<OtherImprovementD>();
			}

			#endregion Initialize empty List<T> variables

			// Get GasLogRate

			StringBuilder sqlGasLog = new StringBuilder();
			sqlGasLog.Append(String.Format(" select sysvalue from {0}.{1}sys where systype like 'GAS%' ", MainForm.localLib, MainForm.localPreFix));

			Rates.GasLogRate = ((Convert.ToDecimal(db.ExecuteScalar(sqlGasLog.ToString()))) / 100);

			// Get NoMaxAc

			StringBuilder sqlMaxAc = new StringBuilder();
			sqlMaxAc.Append(String.Format(" select sysvalue from {0}.{1}sys where systype like 'NOMAXAC%' ", MainForm.localLib, MainForm.localPreFix));

			NoMaxAc = db.ExecuteScalar(sqlMaxAc.ToString()).ToString().Trim();

			// Get Rate2 data

			StringBuilder sqlRat2 = new StringBuilder();
			sqlRat2.AppendLine("select rdca,rdcb,rdcc,rdcd,rdce,rdcm,rbrate,rdate,rekit,yadjy1,rpier,rslab ");
			sqlRat2.AppendLine(",racamt,rplfix,rfinbs,rcondg,rconda,rcondf,rcondp,roblim ");
			sqlRat2.AppendLine(String.Format(" from {0}.{1}rat2 where rsect2 = '0001'", MainForm.localLib, MainForm.localPreFix));

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

			// Capitalization Rates

			StringBuilder caprate = new StringBuilder();
			caprate.Append((String.Format("select cid,cmtgr,cmtgt,clvr,ceyld,chold,cetax,cchgr,cproj,cdisc,crevc,cincg,cexpci, " +
					" cexpcr,cvcla,ctcra from parrevlib.{0}rentr", MainForm.localPreFix)));

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

			// Get Map Coordinates

			DataSet ds_mapXY = db.RunSelectStatement(String.Format(
				"select qmapid,qxaxis,qyaxis from {0}.{1}map ", MainForm.localLib, MainForm.localPreFix));
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

			// Rental Rates

			#region Rental Rates

			StringBuilder getrents = new StringBuilder();
			getrents.Append(String.Format("select rid,rusecd,rtype,rclvr,rclgr,rclar,rclfr,rclpr,rmult,roiv,roig,roia,roif,roip,rvclv, " +
				" rvclg,rvcla,rvclf,rvclp,roerv,roerg,roera,roerf,roerp,rgimv,rgimg,rgima,rgimf, " +
				" rgimp from parrevlib.{0}rent order by rid,rusecd ", MainForm.localPreFix));

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

			#endregion Rental Rates

			// Get HomeSiteDescription
			GetHomesiteDescription();

			// Get LandDescription
			GetLandDescription();

			// Get LandUse Description
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

			//// SiteCodeValues

			var ds_WaterRate = from l in Rates.Rate1Master.GetRat1TypeCollection(Rat1Master.Rat1Types.SiteCode)
							   select new WaterRates()
							   {
								   WaterCode = int.Parse(l.Value.ElemCode.TrimEnd(new char[] { ' ' }).Substring(1)),
								   WaterDescription = l.Value.Description,
								   WaterRate = Convert.ToInt32(l.Value.Rate)
							   };

			//WaterRateCollection.AddRange(ds_WaterRate);
			// Deck Types
			StringBuilder getDeck = new StringBuilder();
			getDeck.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%DECK%' ",
							MainForm.localLib,
							MainForm.localPreFix));
			DataSet ds_deck = db.RunSelectStatement(getDeck.ToString());

			if (ds_deck.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds_deck.Tables[0].Rows)
				{
					string deckcode = row["rsecto"].ToString().Trim();

					DeckTypes.Add(deckcode);
				}
			}

			// Porch Types
			StringBuilder getPor = new StringBuilder();
			getPor.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%POR%' ",
							MainForm.localLib,
							MainForm.localPreFix));
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
							MainForm.localLib,
							MainForm.localPreFix));
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
							MainForm.localLib,
							MainForm.localPreFix));
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

			// Patio Types
			StringBuilder getPat = new StringBuilder();
			getPat.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%PAT%' ",
							MainForm.localLib,
							MainForm.localPreFix));
			DataSet ds_pat = db.RunSelectStatement(getPat.ToString());

			if (ds_pat.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds_pat.Tables[0].Rows)
				{
					string patcode = row["rsecto"].ToString().Trim();

					PatioTypes.Add(patcode);
				}
			}

			// Gargage Type List
			StringBuilder getgar = new StringBuilder();
			getgar.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%GAR%' ",
							MainForm.localLib,
							MainForm.localPreFix));
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

			// Carport Type List
			StringBuilder getcp = new StringBuilder();
			getcp.Append(String.Format("select rsecto,rdesc from {0}.{1}rat1 where rid = 'P' and rdesc like '%CAR%' and rdesc not like '%ENC%'  ",
							MainForm.localLib,
							MainForm.localPreFix));

			DataSet ds_cp = db.RunSelectStatement(getcp.ToString());

			if (ds_cp.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds_cp.Tables[0].Rows)
				{
					string garcode = row["rsecto"].ToString().Trim();

					CarPortTypes.Add(garcode);
				}
			}

			// Get USER Code Description
			DataSet ds_user = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc from {0}.{1}rat1 where rid = 'U' ", MainForm.localLib, MainForm.localPreFix));
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

			// Get Subdivision Code Description
			DataSet ds_subdivCode = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc,rincsf from {0}.{1}rat1 where rid = 'D' ", MainForm.localLib, MainForm.localPreFix));
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

			// Magisterial Districts
			DataSet ds_magDistCode = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc from {0}.{1}rat1 where rid = 'M' ", MainForm.localLib, MainForm.localPreFix));
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

			// WaterRates
			DataSet ds_WaterRates = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc,rtid,rtelem,rrpa from {0}.{1}rat1 where rid = 'S' and rtid = 'WAT' ", MainForm.localLib, MainForm.localPreFix));
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

			// SewerRates
			DataSet ds_SewerRates = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc,rtid,rtelem,rrpa from {0}.{1}rat1 where rid = 'S' and rtid = 'SEW' ", MainForm.localLib, MainForm.localPreFix));
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

			// Other Improvement Code
			DataSet ds_OICodes = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc from {0}.{1}rat1 where rid = 'I' ", MainForm.localLib, MainForm.localPreFix));
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

			// Get Assessor Inital Code Description
			DataSet ds_assInitCode = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc,rtid from {0}.{1}rat1 where rid = 'B' and substr(rsecto,1,2) = 'BA' ", MainForm.localLib, MainForm.localPreFix));
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

			// Get Data Entry Operator Description
			DataSet ds_dataEntryOpCode = db.RunSelectStatement(String.Format(
				" select rsecto,rdesc,rtid from {0}.{1}rat1 where rid = 'B' and substr(rsecto,1,2) = 'BD' ", MainForm.localLib, MainForm.localPreFix));
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

			// Get Occupancy List & Descriptions
			DataSet ds_occupancyDescriptions = db.RunSelectStatement(string.Format(
				"select ttid,ttelem,tdesc from {0}.{1}stab where ttid = 'OCC' ", MainForm.FClib, MainForm.FCprefix));
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

			// Get Building Section Types & Rates
			DataSet ds_residentialSection = db.RunSelectStatement(String.Format(

			//"select rsecto,rdesc,rrpsf from native.{0}rat1 where rid = 'P' and rrpsf > 0 ", prefix));
			"select rsecto,rdesc,rrpsf from {0}.{1}rat1 where rid = 'P' ", MainForm.localLib, MainForm.localPreFix));
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
				"select rsecto,rdesc,rclar,rclbr,rclcr,rcldr,rclmr from {0}.{1}rat1 where rid = 'C' and rrpsf = 0 ", MainForm.localLib, MainForm.localPreFix));
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
			  "select rsecto,rdesc,rclar,rclbr,rclcr,rcldr,rclmr from {0}.{1}rat1 where rid = 'C' and rrpsf = 0 and substr(rsecto,1,1) in ('A','B','C','I','H') ", MainForm.localLib, MainForm.localPreFix));
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
				"select ttid,ttelem,tdesc,tloc from {0}.{1}stab where ttid = 'FLR'", MainForm.FClib, MainForm.FCprefix));
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
				"select ttid,ttelem,tdesc,tloc from {0}.{1}stab where ttid = 'INW'", MainForm.FClib, MainForm.FCprefix));
			foreach (DataRow row in ds_wallAbrev.Tables[0].Rows)
			{
				var wallAbrvCode = new InWallAbreviation()
				{
					WallCode = Convert.ToString(row["ttelem"].ToString().Trim()),
					WallAbreviation = Convert.ToString(row["tloc"].ToString().Trim())
				};

				WallAbreviationCollection.Add(wallAbrvCode);
			}

			// Get STAB data
			//TODO: Modify for location-specific STAB and DESC files
			DataSet ds_stab = db.RunSelectStatement(String.Format(
				"select ttid,ttelem,tdescp,tdesc,tloc from {0}.{1}stab", MainForm.FClib, MainForm.FCprefix));

			foreach (DataRow row in ds_stab.Tables[0].Rows)
			{
				string TTelem = row["ttelem"].ToString().PadLeft(2, ' ');
				string Descp = row["tdescp"].ToString().Trim();

				string Pdescp = TTelem.Substring(0, 2);

				var stab = new StabType()
				{
					Code = row["ttelem"].ToString().Trim(),
					Description = row["tdesc"].ToString().Trim(),
					_printedDescription = String.Format("{0} - {1}", Pdescp.ToString().Trim(), Descp.ToString().Trim())
				};

				string ttid = row["ttid"].ToString().Trim();

				switch (ttid)
				{
					case "HT":
						HeatTypeCollection.Add(stab);
						break;

					case "FUL":
						FuelTypeCollection.Add(stab);
						break;

					case "FND":
						FoundationCollection.Add(stab);
						break;

					case "EXW":
						ExteriorWallTypeCollection.Add(stab);
						break;

					case "INW":
						InteriorWallTypeCollection.Add(stab);
						break;

					case "FLR":
						FloorCoverCollection.Add(stab);
						break;

					case "RFT":
						RoofTypeCollection.Add(stab);
						break;

					case "RFG":
						RoofingCollection.Add(stab);
						break;

					case "OCC":
						OccupancyCollection.Add(stab);
						break;

					case "SEW":
						SewerTypeCollection.Add(stab);
						break;

					case "WAT":
						WaterTypeCollection.Add(stab);
						break;

					case "ROW":
						RightofWayTypeCollection.Add(stab);
						break;

					case "EAS":
						EasementTypeCollection.Add(stab);
						break;

					case "TER":
						TerrainTypeCollection.Add(stab);
						break;

					case "CHR":
						CharacteristicTypeCollection.Add(stab);
						break;

					case "GAR":
						GarageTypeCollection.Add(stab);
						break;

					case "CAR":
						CarPortTypeCollection.Add(stab);
						break;

					case "BAS":
						NonBasementTypeCollection.Add(stab);
						break;

					case "CON":
						ConditionTypeCollection.Add(stab);
						break;

					default:
						break;
				}
			}
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

		private static void ClearValues()
		{
			Rates.Rate1Master = null;
			Rates.ClassValues = null;
			Rates.BasementRate = 0;
			Rates.FinBasementDefaultRate = 0;
			Rates.PlumbingRate = 0;
			SaleYearCutOff = 0;
			DefDepCondA = 0;
			DefDepCondF = 0;
			DefDepCondG = 0;
			DefDepCondP = 0;
			Rates.ExtraKitRate = 0;

			MortgageID = String.Empty;
			MortgageRate = 0;
			MortgageTerm = 0;
			LoanToValueRatio = 0;
			EquityYieldRate = 0;
			HoldingPeriod = 0;
			EffectiveTaxRate = 0;
			AppDepChgRate = 0;
			ProjectionPeriod = 0;
			DiscountRate = 0;
			ReversionCommission = 0;
			IncomeChgRate = 0;
			OperExpInitChg = 0;
			OperExpTermChg = 0;
			VacancyChgRate = 0;
			TermCapRateAdj = 0;

			DoorKnockerCollection = null;
			DoorKnockerCollection_srt = null;
			HeatTypeCollection = null;
			HeatTypeCollection_srt = null;
			AirCondCollection = null;
			AirCondCollection_srt = null;
			FuelTypeCollection = null;
			FuelTypeCollection_srt = null;
			ExteriorWallTypeCollection = null;
			EasementTypeCollection_srt = null;
			InteriorWallTypeCollection = null;
			InteriorWallTypeCollection_srt = null;
			WallAbreviationCollection = null;
			OtherImprovementCollection = null;
			OtherImprovementCollection_srt = null;
			RoofingCollection = null;
			RoofingCollection_srt = null;
			RoofTypeCollection = null;
			RoofTypeCollection_srt = null;
			FloorCoverCollection = null;
			FloorAbreviationCollection = null;
			OccupancyCollection = null;
			OccupancyCollection_srt = null;
			FoundationCollection = null;
			FoundationCollection_srt = null;
			WaterTypeCollection = null;
			WaterTypeCollection_srt = null;
			SewerTypeCollection = null;
			SewerTypeCollection_srt = null;
			RightofWayTypeCollection = null;
			RightofWayTypeCollection_srt = null;
			EasementTypeCollection = null;
			EasementTypeCollection_srt = null;
			TerrainTypeCollection = null;
			TerrainTypeCollection_srt = null;
			CharacteristicTypeCollection = null;
			CharacteristicTypeCollection_srt = null;
			GarageTypeCollection = null;
			GarageTypeCollection_srt = null;
			CarPortTypeCollection = null;
			CarPortTypeCollection_srt = null;
			NonBasementTypeCollection = null;
			NonBasementTypeCollection_srt = null;
			BasementTypeCollection = null;
			BasementTypeCollection_srt = null;
			ConditionTypeCollection = null;
			ConditionTypeCollection_srt = null;
			ClassCollection = null;
			ClassCollection_srt = null;
			CommercialRateCollection = null;
			CommercialRateCollection_srt = null;
			HomeSiteTypeCollection = null;
			HomeSiteTypeCollection_srt = null;
			CommercialRateCollection = null;
			CommercialRateCollection_srt = null;
			LandTypeCollection = null;
			LandTypeCollection_srt = null;
			WaterRateCollection = null;
			SewerRateCollection = null;
			LandUseTypeCollection = null;
			ResidentialSectionTypeCollection = null;
			AttachedSectionTypeCollection = null;
			UserCodeTypeCollection = null;
			CommercialSectionTypeCollection = null;
			CommercialIncomeSectionCollection = null;
			SubDivisionCodeCollection = null;
			SubDivisionCodeCollection_srt = null;
			ResidentialSectionTypeDescriptionCollection = null;
			CommercialSectionTypeDescriptionCollection = null;
			MapCoordinateCollection = null;
			StdDeviationCollection = null;
			DisimilarityWeightCollection = null;
			AssessorCodeCollection = null;
			DataEntryOperatorCollection = null;
			OccupancyDescriptionCollection = null;
			OccupancyDescriptionCollection_srt = null;
			OccupancyCollection = null;
			OccupancyCollection_srt = null;
			MagDistrictCollection = null;
			WaterDescriptionCollection = null;
			WaterDescriptionCollection_srt = null;
			SewerDescriptionCollection = null;
			SewerDescriptionCollection_srt = null;
			CharacteristicDescriptionCollection = null;
			CharacteristicDescriptionCollection_srt = null;
			PavementDescriptionCollection_srt = null;
			PavementDescriptionCollection = null;
			TerrainDescriptionCollection = null;
			TerrainDescriptionCollection_srt = null;
			RightofWayDescriptionCollection = null;
			RightofWayDescriptionCollection_srt = null;
			ZoningDescriptionCollection = null;
			ZoningDescriptionCollection_srt = null;
			UserCodeTypeCollection = null;
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
		public static List<CharateristicTypeDescription> CharacteristicDescriptionCollection;
		public static List<CharateristicTypeDescriptionD> CharacteristicDescriptionCollection_srt;
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
		public static List<DismWieghts> DisimilarityWeightCollection;
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

	public class AllStructureSections
	{
		public string _allSectionDescription
		{
			get; set;
		}

		public string _allSectionType
		{
			get; set;
		}

		public string _allSectPrintedDescription
		{
			get; set;
		}
	}

	public class AssessorCodes
	{
		public string _assInitCode
		{
			get; set;
		}

		public string _assInitDescription
		{
			get; set;
		}

		public string _assPrintDescription
		{
			get; set;
		}
	}

	public class AttachedSectionTypes
	{
		public string _attSectionDescription
		{
			get; set;
		}

		public decimal _attSectionRate
		{
			get; set;
		}

		public string _attSectionType
		{
			get; set;
		}
	}
	public class CharateristicTypeDescription
	{
		public string _charCode
		{
			get; set;
		}

		public string _charDescription
		{
			get; set;
		}

		public string _printCharDescription
		{
			get; set;
		}
	}

	public class CharateristicTypeDescriptionD
	{
		public string _charCode
		{
			get; set;
		}

		public string _charDescription
		{
			get; set;
		}

		public string _printCharDescription
		{
			get; set;
		}
	}

	public class CommercialIncomeSections
	{
		public string _commIncSectionDescription
		{
			get; set;
		}

		public string _commIncSectionType
		{
			get; set;
		}
	}

	public class CommercialRate
	{
		public int RclAr
		{
			get; set;
		}

		public int RclBr
		{
			get; set;
		}

		public int RclCr
		{
			get; set;
		}

		public int RclDr
		{
			get; set;
		}

		public int RclMr
		{
			get; set;
		}

		public string Rdesc
		{
			get; set;
		}

		public string RincSF
		{
			get; set;
		}

		public decimal RrpSF
		{
			get; set;
		}

		public string Rsecto
		{
			get; set;
		}
	}

	public class CommercialRateD
	{
		public int RclAr
		{
			get; set;
		}

		public int RclBr
		{
			get; set;
		}

		public int RclCr
		{
			get; set;
		}

		public int RclDr
		{
			get; set;
		}

		public int RclMr
		{
			get; set;
		}

		public string Rdesc
		{
			get; set;
		}

		public string RincSF
		{
			get; set;
		}

		public decimal RrpSF
		{
			get; set;
		}

		public string Rsecto
		{
			get; set;
		}
	}

	public class CommercialSections
	{
		public string _commSectionDescription
		{
			get; set;
		}

		public decimal _commSectionRateClassA
		{
			get; set;
		}

		public decimal _commSectionRateClassB
		{
			get; set;
		}

		public decimal _commSectionRateClassC
		{
			get; set;
		}

		public decimal _commSectionRateClassD
		{
			get; set;
		}

		public decimal _commSectionRateClassM
		{
			get; set;
		}

		public string _commSectionType
		{
			get; set;
		}
	}

	public class DataEntryOperatorCode
	{
		public string _dataEntryOpCode
		{
			get; set;
		}

		public string _dataEntryOpDescription
		{
			get; set;
		}

		public string _dataEntryPrintDescription
		{
			get; set;
		}
	}

	public class DismWieghts
	{
		public decimal _wtAuxArea
		{
			get; set;
		}

		public decimal _wtBsmt
		{
			get; set;
		}

		public decimal _wtCarPort
		{
			get; set;
		}

		public decimal _wtDeck
		{
			get; set;
		}

		public decimal _wtEpor
		{
			get; set;
		}

		public decimal _wtFinBsmt
		{
			get; set;
		}

		public decimal _wtGarage
		{
			get; set;
		}

		public decimal _wtPatio
		{
			get; set;
		}

		public decimal _wtPor
		{
			get; set;
		}

		public decimal _wtSize
		{
			get; set;
		}

		public decimal _wtSpor
		{
			get; set;
		}

		public decimal _wtStory
		{
			get; set;
		}
	}

	public class FloorAbreviation
	{
		public string FlrAbreviation
		{
			get; set;
		}

		public string FlrCode
		{
			get; set;
		}
	}

	public class HomeSiteType
	{
		public int HSCode
		{
			get; set;
		}

		public string HSDescription
		{
			get; set;
		}

		public int HSRate
		{
			get; set;
		}
	}

	public class HomeSiteTypeD
	{
		public int HSCode
		{
			get; set;
		}

		public string HSDescription
		{
			get; set;
		}

		public string HSPrintDesc
		{
			get; set;
		}

		public int HSRate
		{
			get; set;
		}
	}

	public class InWallAbreviation
	{
		public string WallAbreviation
		{
			get; set;
		}

		public string WallCode
		{
			get; set;
		}
	}

	public class LandType
	{
		public int LandCode
		{
			get; set;
		}

		public string LandDescription
		{
			get; set;
		}
	}

	public class LandTypeD
	{
		public int LandCode
		{
			get; set;
		}

		public string LandDescription
		{
			get; set;
		}

		public string LandPrintDesc
		{
			get; set;
		}
	}

	public class LandUseType
	{
		public string _landUseCode
		{
			get; set;
		}

		public string _landUseDescription
		{
			get; set;
		}

		public string _printUseDescription
		{
			get; set;
		}
	}

	public class LivingAreaSectionTypes
	{
		public string _LAattSectionDescription
		{
			get; set;
		}

		public decimal _LAattSectionRate
		{
			get; set;
		}

		public string _LAattSectionType
		{
			get; set;
		}
	}

	public class MagDistrictCodes
	{
		public string _magDistCode
		{
			get; set;
		}

		public string _magDistDescription
		{
			get; set;
		}

		public string _magPrintDescription
		{
			get; set;
		}
	}

	public class MapCoordinates
	{
		public string _subMap
		{
			get; set;
		}

		public int _xAxis
		{
			get; set;
		}

		public int _yAxis
		{
			get; set;
		}
	}

	public class OccupancyDescription
	{
		public string _occupancyCode
		{
			get; set;
		}

		public string _occupancyDescription
		{
			get; set;
		}

		public string _printOccupancyDescription
		{
			get; set;
		}
	}

	public class OccupancyDescriptionD
	{
		public string _occupancyCode
		{
			get; set;
		}

		public string _occupancyDescription
		{
			get; set;
		}

		public string _printOccupancyDescription
		{
			get; set;
		}
	}

	public class OtherImprovement
	{
		public int _OICode
		{
			get; set;
		}

		public string _OIDescription
		{
			get; set;
		}

		public string _printOIDescription
		{
			get; set;
		}
	}

	public class OtherImprovementD
	{
		public int _OICode
		{
			get; set;
		}

		public string _OIDescription
		{
			get; set;
		}

		public string _printOIDescription
		{
			get; set;
		}
	}

	public class PavementTypeDescription
	{
		public string _printPvmtDescription
		{
			get; set;
		}

		public string _pvmtCode
		{
			get; set;
		}

		public string _pvmtDescription
		{
			get; set;
		}
	}

	public class PavementTypeDescriptionD
	{
		public string _printPvmtDescription
		{
			get; set;
		}

		public string _pvmtCode
		{
			get; set;
		}

		public string _pvmtDescription
		{
			get; set;
		}
	}

	public class RentRates
	{
		public decimal _rAvgRate
		{
			get; set;
		}

		public decimal _rFairRate
		{
			get; set;
		}

		public decimal _rGIMAvg
		{
			get; set;
		}

		public decimal _rGIMFair
		{
			get; set;
		}

		public decimal _rGIMGood
		{
			get; set;
		}

		public decimal _rGIMPoor
		{
			get; set;
		}

		public decimal _rGIMVGood
		{
			get; set;
		}

		public decimal _rGoodRate
		{
			get; set;
		}

		public string _rid
		{
			get; set;
		}

		public decimal _rOERAvg
		{
			get; set;
		}

		public decimal _rOERFair
		{
			get; set;
		}

		public decimal _rOERGood
		{
			get; set;
		}

		public decimal _rOERPoor
		{
			get; set;
		}

		public decimal _rOERVGood
		{
			get; set;
		}

		public decimal _rOthIncAvg
		{
			get; set;
		}

		public decimal _rOthIncFair
		{
			get; set;
		}

		public decimal _rOthIncGood
		{
			get; set;
		}

		public decimal _rOthIncPoor
		{
			get; set;
		}

		public decimal _rOthIncVGood
		{
			get; set;
		}

		public decimal _rPoorRate
		{
			get; set;
		}

		public string _rPrintDescription
		{
			get; set;
		}

		public decimal _rSizeMultiplier
		{
			get; set;
		}

		public string _rTypeDescription
		{
			get; set;
		}

		public string _rUseCode
		{
			get; set;
		}

		public decimal _rVCLAvg
		{
			get; set;
		}

		public decimal _rVCLFair
		{
			get; set;
		}

		public decimal _rVCLGood
		{
			get; set;
		}

		public decimal _rVCLPoor
		{
			get; set;
		}

		public decimal _rVCLVGood
		{
			get; set;
		}

		public decimal _rVeryGoodRate
		{
			get; set;
		}
	}

	public class ResidentialSections
	{
		public string _resSectionDescription
		{
			get; set;
		}

		public decimal _resSectionRate
		{
			get; set;
		}

		public string _resSectionType
		{
			get; set;
		}
	}

	public class RightofWayTypeDescription
	{
		public string _printRowDescription
		{
			get; set;
		}

		public string _rowCode
		{
			get; set;
		}

		public string _rowDescription
		{
			get; set;
		}
	}

	public class RightofWayTypeDescriptionD
	{
		public string _printRowDescription
		{
			get; set;
		}

		public string _rowCode
		{
			get; set;
		}

		public string _rowDescription
		{
			get; set;
		}
	}

	public class SewerRates
	{
		public int SewerCode
		{
			get; set;
		}

		public string SewerDescription
		{
			get; set;
		}

		public int SewerRate
		{
			get; set;
		}
	}

	public class SewerTypeDescription
	{
		public string _printSewerDescription
		{
			get; set;
		}

		public string _sewerCode
		{
			get; set;
		}

		public string _sewerDescription
		{
			get; set;
		}
	}

	public class SewerTypeDescriptionD
	{
		public string _printSewerDescription
		{
			get; set;
		}

		public string _sewerCode
		{
			get; set;
		}

		public string _sewerDescription
		{
			get; set;
		}
	}

	public class StabType
	{
		public string _printedDescription
		{
			get; set;
		}

		public string Code
		{
			get; set;
		}

		public string Description
		{
			get; set;
		}

		public string shortDescription
		{
			get; set;
		}

		public string Type
		{
			get; set;
		}
	}

	public class StabTypeD
	{
		public string _printedDescription
		{
			get; set;
		}

		public string Code
		{
			get; set;
		}

		public string Description
		{
			get; set;
		}

		public string shortDescription
		{
			get; set;
		}

		public string Type
		{
			get; set;
		}
	}

	public class StdDeviations
	{
		public decimal _sdAuxArea
		{
			get; set;
		}

		public decimal _sdBsmt
		{
			get; set;
		}

		public decimal _sdCarPort
		{
			get; set;
		}

		public decimal _sdDeck
		{
			get; set;
		}

		public decimal _sdEpor
		{
			get; set;
		}

		public decimal _sdFinBsmt
		{
			get; set;
		}

		public decimal _sdGarage
		{
			get; set;
		}

		public decimal _sdPatio
		{
			get; set;
		}

		public decimal _sdPor
		{
			get; set;
		}

		public decimal _sdSize
		{
			get; set;
		}

		public decimal _sdSpor
		{
			get; set;
		}

		public decimal _sdStory
		{
			get; set;
		}
	}

	public class SubDivisionCodes
	{
		public string _printDescription
		{
			get; set;
		}

		public string _subDivCode
		{
			get; set;
		}

		public string _subDivDescription
		{
			get; set;
		}

		public string _sudDivQuality
		{
			get; set;
		}
	}

	public class SubDivisionCodesD
	{
		public string _printSubDivDescription
		{
			get; set;
		}

		public string _subDivCode
		{
			get; set;
		}

		public string _subDivDescription
		{
			get; set;
		}

		public string _sudDivQuality
		{
			get; set;
		}
	}

	public class TerrainTypeDescription
	{
		public string _printTerrDescription
		{
			get; set;
		}

		public string _terrCode
		{
			get; set;
		}

		public string _terrDescription
		{
			get; set;
		}
	}

	public class TerrainTypeDescriptionD
	{
		public string _printTerrDescription
		{
			get; set;
		}

		public string _terrCode
		{
			get; set;
		}

		public string _terrDescription
		{
			get; set;
		}
	}

	public class UserCodeType
	{
		public string _printUserCodeDescription
		{
			get; set;
		}

		public string _userCode
		{
			get; set;
		}

		public string _userCodeDescription
		{
			get; set;
		}
	}

	public class WaterRates
	{
		public int WaterCode
		{
			get; set;
		}

		public string WaterDescription
		{
			get; set;
		}

		public int WaterRate
		{
			get; set;
		}
	}

	public class WaterTypeDescription
	{
		public string _printWaterDescription
		{
			get; set;
		}

		public string _waterCode
		{
			get; set;
		}

		public string _waterDescription
		{
			get; set;
		}
	}

	public class WaterTypeDescriptionD
	{
		public string _printWaterDescription
		{
			get; set;
		}

		public string _waterCode
		{
			get; set;
		}

		public string _waterDescription
		{
			get; set;
		}
	}

	public class ZoningDescription
	{
		public string _printZoneDescription
		{
			get; set;
		}

		public string _zoneCode
		{
			get; set;
		}

		public string _zoneDescription
		{
			get; set;
		}
	}

	public class ZoningDescriptionD
	{
		public string _printZoneDescription
		{
			get; set;
		}

		public string _zoneCode
		{
			get; set;
		}

		public string _zoneDescription
		{
			get; set;
		}
	}
}
