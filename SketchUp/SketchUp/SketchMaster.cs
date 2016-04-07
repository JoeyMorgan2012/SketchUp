using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SWallTech;

//using FoxproSketches;

namespace SketchUp
{
	public class SketchMaster
	{
		public SketchMaster(SWallTech.CAMRA_Connection fox, int record, int cardnum, int occupancy)
			: this()
		{
			CamraDbConnection = fox;

			//ParcelData _data =  ParcelData.GetParcel(_conn, record, cardnum);

			Record = record;
			Card = cardnum;
			Occupancy = occupancy;
			prefix = SketchUpGlobals.LocalityPreFix;

			GetSections();
		}

		private SketchMaster()
		{
			LivingSquareFootTypes = Globals.LivingSquareFootTypesList();
		}

		public Dictionary<string, List<BuildingLine>> GetAllBuildingSections()
		{
			Dictionary<string, List<BuildingLine>> allLines = new Dictionary<string, List<BuildingLine>>();
			foreach (string letter in GetSectionLetters())
			{
				allLines.Add(letter, GetLinesForSection(letter));
			}
			return allLines;
		}

		public int GetLimit(LimitTypes type)
		{
			decimal limit = 0m;

			if (HasSketch)
			{
				StringBuilder sql = new StringBuilder();
				sql.Append("select ");
				switch (type)
				{
					case LimitTypes.MaxX:
						sql.Append("max(jlpt1x) ");
						break;

					case LimitTypes.MaxY:
						sql.Append("max(jlpt1y) ");
						break;

					case LimitTypes.MinX:
						sql.Append("min(jlpt1x) ");
						break;

					case LimitTypes.MinY:
						sql.Append("min(jlpt1y) ");
						break;

					default:
						break;
				}
				sql.Append(" from skline where jlrecord = ");
				sql.Append(Record.ToString());
				sql.Append(" and jldwell = ");
				sql.Append(Card.ToString());

				try
				{
					var limitCmd = DatabaseConnection.CreateCommand(sql.ToString());
					object obj = limitCmd.ExecuteScalar();
					string str = obj.ToString();
					if (!"".Equals(str))
					{
						limit = decimal.Parse(str);
					}
				}
				catch (Exception sqlex)
				{
					throw sqlex;
				}
				finally
				{
				}
			}
			return Convert.ToInt32(limit);
		}

		public BuildingLineCollection GetLines(string sectionLetter)
		{
			BuildingLineCollection sectionLines = new BuildingLineCollection(Record, Card, sectionLetter);

			StringBuilder buildingLineSql = new StringBuilder();
			buildingLineSql.Append(" select jlsect, jlline# , jldirect, jlxlen, jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
			buildingLineSql.Append(String.Format(" from {0}.{1}line", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
			buildingLineSql.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}'", Record, Card, sectionLetter));
			buildingLineSql.Append(" order by jlline# ");

			DataSet buildingLineData = CamraDbConnection.DBConnection.RunSelectStatement(buildingLineSql.ToString());

			if (buildingLineData.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow blreader in buildingLineData.Tables[0].Rows)
				{
					var line = new BuildingLine()
					{
						Record = Record,
						Card = Card,
						SectionLetter = blreader["jlsect"].ToString().Trim(),
						LineNumber = Convert.ToInt32(blreader["jlline#"].ToString()),
						Directional = blreader["jldirect"].ToString().Trim(),
						XLength = Convert.ToDecimal(blreader["jlxlen"].ToString()),
						YLength = Convert.ToDecimal(blreader["jlylen"].ToString()),

						// = Convert.ToDecimal(blreader["jllinelen"].ToString()),
						//JLAngle = Convert.ToDecimal(blreader["jlangle"].ToString()),
						Point1X = Convert.ToDecimal(blreader["jlpt1x"].ToString()),
						Point1Y = Convert.ToDecimal(blreader["jlpt1y"].ToString()),
						Point2X = Convert.ToDecimal(blreader["jlpt2x"].ToString()),
						Point2Y = Convert.ToDecimal(blreader["jlpt2y"].ToString()),
						Attachment = blreader["jlattach"].ToString().Trim()
					};

					sectionLines.Add(line);
				}
			}

			return sectionLines;
		}

		public List<string> GetSectionLetters()
		{
			var q = from l in BuildingSections
					orderby l.SectionLetter
					group l.SectionLetter by l.SectionLetter
						into s
					select new
					{
						SectionLetter = s.Key
					};

			List<string> lets = new List<string>();
			foreach (var i in q)
			{
				lets.Add(i.SectionLetter);
			}
			return lets;
		}

		public void GetSections()
		{
			ClearXLines(CamraDbConnection);

			BuildingSections = new BuildingSectionCollection(Record, Card);

			DataSet dsSections = SelectBuildingSections(Record, Card);

			if (dsSections.Tables[0].Rows.Count > 0)
			{
				BuildingSections = ReadDataSetIntoBuildingSectionsCollection(dsSections, Record, Card);
			}
		}

		private static void SetSectionRateForResOrComm(List<string> residentialTypes, List<string> commercialTypes, DataRow row, BuildingSection section)
		{
			SectionResCommType sectionResCommType = SectionResCommType.NotFound;

			if (residentialTypes.Contains(section.SectionType))
			{
				sectionResCommType = SectionResCommType.Residential;
			}
			else if (commercialTypes.Contains(section.SectionType))
			{
				sectionResCommType = SectionResCommType.Commercial;
			}

			switch (sectionResCommType)
			{
				case SectionResCommType.Residential:
					section.Rate = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionRate(section.SectionType);
					section.Description = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionTypeDescription(section.SectionType);
					break;

				case SectionResCommType.Commercial:
					string stx = section.SectionType;

					var comm = (from c in CamraSupport.CommercialSectionTypeCollection
								where c._commSectionType == stx
								select c).SingleOrDefault();

					section.Class = Convert.ToString(row["jsclass"].ToString().Trim());
					section.Description = CamraSupport.CommercialSectionTypeCollection.CommercialSectionTypeDescription(section.SectionType);
					switch (section.Class)
					{
						case "A":
							section.Rate = comm._commSectionRateClassA;
							break;

						case "B":
							section.Rate = comm._commSectionRateClassB;
							break;

						case "C":
							section.Rate = comm._commSectionRateClassC;
							break;

						case "D":
							section.Rate = comm._commSectionRateClassD;
							break;

						case "M":
							section.Rate = comm._commSectionRateClassM;
							break;

						default:
							break;
					}
					break;

				case SectionResCommType.NotFound:
				default:
					section.Rate = 0.00M;
					throw new Exception("Section Type not found in Rat1");
			}
		}

		private void ClearXLines(CAMRA_Connection dbConnection)
		{
			try
			{
				string clrlinex = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, Record, Card);

				dbConnection.DBConnection.ExecuteNonSelectStatement(clrlinex.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		private List<BuildingLine> GetLinesForSection(string letter)
		{
			var q = (from sect in BuildingSections
					 where sect.SectionLetter == letter
					 select sect).FirstOrDefault();
			return q.SectionLines;
		}

		private BuildingSection LoadSectionInformationIntoBuildingSections(List<string> residentialTypes, List<string> commercialTypes, DataRow row)
		{
			string sectionLetter = Convert.ToString(row["jssect"].ToString().Trim());
			BuildingSection section = new BuildingSection(CamraDbConnection, Record, Card, sectionLetter)
			{
				SectionType = Convert.ToString(row["jstype"].ToString().Trim()),
				SquareFootage = Convert.ToDecimal(row["jssqft"]),
				Factor = Convert.ToDecimal(row["jsfactor"]),
				Depreciation = Convert.ToDecimal(row["jsdeprc"]),
				HasSketch = row["jssketch"].ToString().Trim() == "Y"
			};
			SetSectionRateForResOrComm(residentialTypes, commercialTypes, row, section);
			SetValueByOccupancyType(row, section);

			section.SectionLines = GetLines(section.SectionLetter);
			return section;
		}

		private BuildingSectionCollection ReadDataSetIntoBuildingSectionsCollection(DataSet dsSections, int record, int card)
		{
			BuildingSectionCollection buildingSections = new BuildingSectionCollection(record, card);
			List<string> residentialTypes = (from t in CamraSupport.ResidentialSectionTypeCollection
											 select t._resSectionType).ToList();

			List<string> commercialTypes = (from t in CamraSupport.CommercialSectionTypeCollection
											select t._commSectionType).ToList();

			foreach (DataRow row in dsSections.Tables[0].Rows)
			{
				BuildingSection section = LoadSectionInformationIntoBuildingSections(residentialTypes, commercialTypes, row);

				buildingSections.Add(section);
			}
			return buildingSections;
		}
		private DataSet SelectBuildingSections(int record, int card)
		{
			string buildingSectionSelectSql = string.Format(" select jsrecord, jsdwell, jssect, jstype,jsstory,jsdesc,jssketch,jssqft,js0depr, jsclass,jsvalue, jsfactor, jsdeprc from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, record, card);

			DataSet buildingSectionData = CamraDbConnection.DBConnection.RunSelectStatement(buildingSectionSelectSql);
			return buildingSectionData;
		}

		private void SetValueByOccupancyType(DataRow row, BuildingSection section)
		{
			SectionOccResCommType sectionOccupancyType = SectionOccResCommType.NotFound;
			if (CamraSupport.ResidentialOccupancies.Contains(Occupancy))
			{
				sectionOccupancyType = SectionOccResCommType.Residential;
			}
			else if (CamraSupport.TaxExemptOccupancies.Contains(Occupancy))
			{
				sectionOccupancyType = SectionOccResCommType.TaxExempt;
			}
			else if (CamraSupport.CommercialOccupancies.Contains(Occupancy))
			{
				sectionOccupancyType = SectionOccResCommType.Commercial;
			}

			switch (sectionOccupancyType)
			{
				case SectionOccResCommType.Residential:
					section.Value = Convert.ToInt32(section.Rate * section.SquareFootage);
					break;

				case SectionOccResCommType.Commercial:

					if (row["js0depr"].ToString() == "Y")
					{
						section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor));
					}
					else
					{
						section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor) * (1 - section.Depreciation));
					}
					break;

				case SectionOccResCommType.TaxExempt:
					if (row["js0depr"].ToString() == "Y")
					{
						section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor));
					}
					else
					{
						section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor) * (1 - section.Depreciation));
					}
					break;

				case SectionOccResCommType.NotFound:

				default:
					throw new Exception("Occupancy Type not found in Rat1");
			}
		}
		public enum LimitTypes
		{
			MaxX,
			MaxY,
			MinX,
			MinY
		}

		public enum SectionOccResCommType
		{
			Residential = 1,
			Commercial = 2,
			TaxExempt = 3,
			NotFound = 4
		}

		public enum SectionResCommType
		{
			Residential = 1, Commercial = 2, NotFound = 3
		}

		//public BuildingSectionCollection SectionRecords { get; set; }
		public BuildingSectionCollection BuildingSections
		{
			get; set;
		}

		public SWallTech.CAMRA_Connection CamraDbConnection
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		public DBAccessManager DatabaseConnection
		{
			get; set;
		}

		public bool HasSketch
		{
			get; set;
		}

		public decimal LivingAreaSquareFootage
		{
			get; set;
		}

		public List<string> LivingSquareFootTypes
		{
			get
			{
				if (livingSquareFootTypes == null || livingSquareFootTypes.Count == 0)
				{
					livingSquareFootTypes = Globals.LivingSquareFootTypesList();
				}

				return livingSquareFootTypes;
			}

			set
			{
				livingSquareFootTypes = value;
			}
		}

		public int Occupancy
		{
			get; set;
		}

		public string prefix
		{
			get; set;
		}

		public int Record
		{
			get; set;
		}

		public decimal StoryHeight
		{
			get; set;
		}

		private List<string> livingSquareFootTypes = new List<string>();
	}
}