using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

//using FoxproSketches;

namespace SketchUp
{
	public class SketchMaster
	{
		public enum LimitTypes
		{
			MaxX,
			MaxY,
			MinX,
			MinY
		}

		public DBAccessManager DatabaseConnection
		{
			get; set;
		}

		public SWallTech.CAMRA_Connection _fox
		{
			get; set;
		}

		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		public int Occupancy
		{
			get; set;
		}

		public decimal StoryHeight
		{
			get; set;
		}

		public decimal LivingAreaSquareFootage
		{
			get; set;
		}

		public bool HasSketch
		{
			get; set;
		}

		public string prefix
		{
			get; set;
		}

		private List<string> livingSquareFootTypes = new List<string>();

		private SketchMaster()
		{
			livingSquareFootTypes.Add("BASE");
			livingSquareFootTypes.Add("ADD");
			livingSquareFootTypes.Add("OH");
			livingSquareFootTypes.Add("LAG");
			livingSquareFootTypes.Add("NBAD");
		}

		public SketchMaster(SWallTech.CAMRA_Connection fox, int record, int cardnum, int occupancy)
			: this()
		{
			_fox = fox;

			//ParcelData _data =  ParcelData.GetParcel(_conn, record, cardnum);

			Record = record;
			Card = cardnum;
			Occupancy = occupancy;
			prefix = MainForm.localPreFix;

			GetSections();
		}

		//public BuildingSectionCollection SectionRecords { get; set; }
		public BuildingSectionCollection SectionRecords
		{
			get; set;
		}

		public void GetSections()
		{
			string clrlinex = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", MainForm.FClib, MainForm.FCprefix, Record, Card);

			_fox.DBConnection.ExecuteNonSelectStatement(clrlinex.ToString());

			SectionRecords = new BuildingSectionCollection(Record, Card);

			string bssql = string.Format(" select jsrecord, jsdwell, jssect, jstype,jsstory,jsdesc,jssketch,jssqft,js0depr, jsclass,jsvalue, jsfactor, jsdeprc from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", MainForm.localLib, MainForm.localPreFix, Record, Card);

			DataSet ds = _fox.DBConnection.RunSelectStatement(bssql);

			if (ds.Tables[0].Rows.Count > 0)
			{
				var residentialTypes = (from t in CamraSupport.ResidentialSectionTypeCollection
										select t._resSectionType).ToList();

				var commercialTypes = (from t in CamraSupport.CommercialSectionTypeCollection
									   select t._commSectionType).ToList();

				foreach (DataRow bsreader in ds.Tables[0].Rows)
				{
					var sectionLetter = Convert.ToString(bsreader["jssect"].ToString().Trim());
					var section = new BuildingSection(_fox, Record, Card, sectionLetter)
					{
						SectionType = Convert.ToString(bsreader["jstype"].ToString().Trim()),
						SquareFootage = Convert.ToDecimal(bsreader["jssqft"]),
						Factor = Convert.ToDecimal(bsreader["jsfactor"]),
						Depreciation = Convert.ToDecimal(bsreader["jsdeprc"]),
						HasSketch = bsreader["jssketch"].ToString().Trim() == "Y"
					};

					if (residentialTypes.Contains(section.SectionType))
					{
						section.Rate = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionRate(section.SectionType);
						section.Description = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionTypeDescription(section.SectionType);
					}
					else if (commercialTypes.Contains(section.SectionType))
					{
						string stx = section.SectionType;

						var comm = (from c in CamraSupport.CommercialSectionTypeCollection
									where c._commSectionType == stx
									select c).SingleOrDefault();

						section.Class = Convert.ToString(bsreader["jsclass"].ToString().Trim());
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
					}
					else
					{
						//throw new Exception("Section Type not found in Rat1");
					}

					if (CamraSupport.ResidentialOccupancies.Contains(Occupancy))
					{
						section.Value = Convert.ToInt32(section.Rate * section.SquareFootage);
					}
					if (CamraSupport.CommercialOccupancies.Contains(Occupancy))
					{
						if (bsreader["js0depr"].ToString() == "Y")
						{
							section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor));
						}
						else
						{
							section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor) * (1 - section.Depreciation));
						}
					}
					if (CamraSupport.TaxExemptOccupancies.Contains(Occupancy))
					{
						if (bsreader["js0depr"].ToString() == "Y")
						{
							section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor));
						}
						else
						{
							section.Value = Convert.ToInt32(section.SquareFootage * section.Rate * (1 + section.Factor) * (1 - section.Depreciation));
						}
					}

					section.SectionLines = GetLines(section.SectionLetter);

					SectionRecords.Add(section);
				}
			}
		}

		public BuildingLineCollection GetLines(string sectionLetter)
		{
			var sectionLines = new BuildingLineCollection(Record, Card, sectionLetter);

			StringBuilder blsql = new StringBuilder();
			blsql.Append(" select jlsect, jlline# , jldirect, jlxlen, jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
			blsql.Append(String.Format(" from {0}.{1}line", MainForm.localLib, MainForm.localPreFix));
			blsql.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}'", Record, Card, sectionLetter));
			blsql.Append(" order by jlline# ");

			DataSet ds = _fox.DBConnection.RunSelectStatement(blsql.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow blreader in ds.Tables[0].Rows)
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
			var q = from l in SectionRecords
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

		public Dictionary<string, List<BuildingLine>> GetAllBuildingSections()
		{
			Dictionary<string, List<BuildingLine>> allLines = new Dictionary<string, List<BuildingLine>>();
			foreach (string letter in GetSectionLetters())
			{
				allLines.Add(letter, GetLinesForSection(letter));
			}
			return allLines;
		}

		private List<BuildingLine> GetLinesForSection(string letter)
		{
			var q = (from sect in SectionRecords
					 where sect.SectionLetter == letter
					 select sect).FirstOrDefault();
			return q.SectionLines;
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
	}
}