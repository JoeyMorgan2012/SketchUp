using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class CPBuildingSectionCollection : List<CPBuildingSection>
	{
		public CPBuildingSectionCollection(ParcelData data, SWallTech.CAMRA_Connection fox)
		{
			_currentParcel = data;
			_fox = fox;
		}

		public CPBuildingSectionCollection(SWallTech.CAMRA_Connection fox, ParcelData _parent)
		{
			_parentParcel = _parent;

			//_bsdb = conn.DBConnection;

			_BSTable = new DataTable("BuildingSectionTable");
			_BSTable.Columns.Add(new DataColumn("Record", typeof(int)));
			_BSTable.Columns.Add(new DataColumn("Card", typeof(int)));
			_BSTable.Columns.Add(new DataColumn("SectionLetter", typeof(string)));
			_BSTable.Columns.Add(new DataColumn("SectionType", typeof(string)));
			_BSTable.Columns.Add(new DataColumn("SectionDescription", typeof(string)));
			_BSTable.Columns.Add(new DataColumn("SectionStory", typeof(decimal)));
			_BSTable.Columns.Add(new DataColumn("SectionSize", typeof(decimal)));
			_BSTable.Columns.Add(new DataColumn("SectionClass", typeof(string)));
			_BSTable.Columns.Add(new DataColumn("SectionFactor", typeof(decimal)));
			_BSTable.Columns.Add(new DataColumn("SectionDepreciation", typeof(decimal)));
			_BSTable.Columns.Add(new DataColumn("SectionNoDepreciation", typeof(string)));
			_BSTable.Columns.Add(new DataColumn("SectionRate", typeof(decimal)));
			_BSTable.Columns.Add(new DataColumn("SectionValue", typeof(int)));
		}

		public int Occupancy
		{
			get; set;
		}

		public DataTable BuildingSectionTable
		{
			get
			{
				return _BSTable;
			}
		}

		public DataTable BuildingSectionShortCom
		{
			get
			{
				DataTable tb = _BSTable.Clone();
				tb.Columns.Remove("Record");
				tb.Columns.Remove("Card");
				tb.Columns.Remove("SectionLetter");
				tb.Columns.Remove("SectionType");
				tb.Columns.Remove("SectionClass");

				//tb.Columns.Remove("SectionFactor");
				//tb.Columns.Remove("SectionDepreciation");
				tb.Columns.Remove("SectionNoDepreciation");

				foreach (DataRow drow in _BSTable.Rows)
				{
					DataRow row = tb.NewRow();

					//row["SectionLetter"] = drow["SectionLetter"];

					row["SectionStory"] = drow["SectionStory"];
					row["SectionDescription"] = drow["SectionDescription"];
					row["SectionSize"] = drow["SectionSize"];
					row["SectionRate"] = drow["SectionRate"];
					row["SectionFactor"] = drow["SectionFactor"];
					row["SectionDepreciation"] = drow["SectionDepreciation"];
					row["SectionValue"] = drow["SectionValue"];
					tb.Rows.Add(row);
				}

				return tb;
			}
		}

		public DataTable BuildingSectionShort
		{
			get
			{
				DataTable tb = _BSTable.Clone();
				tb.Columns.Remove("Record");
				tb.Columns.Remove("Card");
				tb.Columns.Remove("SectionLetter");

				//tb.Columns.Remove("SectionType");
				tb.Columns.Remove("SectionClass");
				tb.Columns.Remove("SectionFactor");
				tb.Columns.Remove("SectionDepreciation");
				tb.Columns.Remove("SectionNoDepreciation");

				foreach (DataRow drow in _BSTable.Rows)
				{
					DataRow row = tb.NewRow();

					row["SectionType"] = drow["SectionType"];
					row["SectionStory"] = drow["SectionStory"];
					row["SectionDescription"] = drow["SectionDescription"];
					row["SectionSize"] = drow["SectionSize"];
					row["SectionRate"] = drow["SectionRate"];
					row["SectionValue"] = drow["SectionValue"];
					tb.Rows.Add(row);
				}

				return tb;
			}
		}

		public DataTable BuildingSectionExpanded
		{
			get
			{
				DataTable tb = _BSTable.Clone();
				tb.Columns.Remove("Record");
				tb.Columns.Remove("Card");
				tb.Columns.Remove("SectionLetter");

				//tb.Columns.Remove("SectionType");

				foreach (DataRow drow in _BSTable.Rows)
				{
					DataRow row = tb.NewRow();

					//row["SectionLetter"] = drow["SectionLetter"];
					row["SectionType"] = drow["SectionType"];
					row["SectionDescription"] = drow["SectionDescription"];
					row["SectionStory"] = drow["SectionStory"];
					row["SectionClass"] = drow["SectionClass"];
					row["SectionSize"] = drow["SectionSize"];
					row["SectionRate"] = drow["SectionRate"];
					row["SectionFactor"] = drow["SectionFactor"];
					row["SectionNoDepreciation"] = drow["SectionNoDepreciation"];
					row["SectionDepreciation"] = drow["SectionDepreciation"];
					row["SectionValue"] = drow["SectionValue"];
					tb.Rows.Add(row);
				}

				return tb;
			}
		}

		public int TotalDepreciationValue
		{
			get
			{
				var q = from t in this
						select t.SectionDepreciationValue;

				return q.Sum();
			}
		}

		public int TotalFactorValue
		{
			get
			{
				var q = from t in this
						select t.SectionFactorValue;

				return q.Sum();
			}
		}

		public DataTable GetSection(SWallTech.CAMRA_Connection _conn, int record, int card)
		{
			_BSTable.Rows.Clear();

			StringBuilder bssql = new StringBuilder();
			bssql.Append(" select jsrecord, jsdwell, jssect, jstype, jsclass, jssqft, jsfactor, jsdeprc, js0depr ");
			bssql.Append(String.Format(" from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", MainForm.localLib, MainForm.localPreFix, record, card));

			DataSet ds = _conn.DBConnection.RunSelectStatement(bssql.ToString());
			string sectionType = String.Empty;
			string sectionClass = String.Empty;
			string sectionTypeDescription = String.Empty;
			string sectionLetterType = String.Empty;
			decimal sectionRate = 0;
			int sectionValue = 0;

			//TODO: Uncomment if these turn out to be needed.
			//decimal sectionDeprecitionValue = 0;
			//decimal sectionFactorValue = 0;

			var residentialTypes = (from t in CamraSupport.ResidentialSectionTypeCollection
									select t._resSectionType).ToList();
			var commercialTypes = (from t in CamraSupport.CommercialSectionTypeCollection
								   select t._commSectionType).ToList();

			foreach (DataRow bsreader in ds.Tables[0].Rows)
			{
				sectionType = Convert.ToString(bsreader["jstype"].ToString().Trim());

				sectionLetterType = String.Format("{0}- {1}",
					Convert.ToString(bsreader["jssect"].ToString()),
					Convert.ToString(bsreader["jstype"].ToString()).Trim());

				decimal sectionSqft = Convert.ToDecimal(bsreader["jssqft"]);
				decimal sectionFactor = Convert.ToDecimal(bsreader["jsfactor"]);
				decimal sectionDepreciation = Convert.ToDecimal(bsreader["jsdeprc"]);

				if (residentialTypes.Contains(sectionType))
				{
					sectionRate = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionRate(sectionType);
					sectionTypeDescription =
						CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionTypeDescription(sectionType);
				}
				else if (commercialTypes.Contains(sectionType))
				{
					var comm = (from c in CamraSupport.CommercialSectionTypeCollection
								where c._commSectionType == sectionType
								select c).SingleOrDefault();

					sectionClass = Convert.ToString(bsreader["jsclass"].ToString().Trim());
					sectionTypeDescription =
						CamraSupport.CommercialSectionTypeCollection.CommercialSectionTypeDescription(sectionType);
					switch (sectionClass)
					{
						case "A":
							sectionRate = comm._commSectionRateClassA;
							break;

						case "B":
							sectionRate = comm._commSectionRateClassB;
							break;

						case "C":
							sectionRate = comm._commSectionRateClassC;
							break;

						case "D":
							sectionRate = comm._commSectionRateClassD;
							break;

						case "M":
							sectionRate = comm._commSectionRateClassM;
							break;

						default:
							break;
					}
				}
				else
				{
					//throw new Exception("Section Type not found in Rat1");
				}

				CPBuildingSection sectionData = CPBuildingSection.GetSection(_conn.DBConnection, _parentParcel.mrecno, _parentParcel.mdwell,
					bsreader["jssect"].ToString().Trim(),
					_parentParcel.moccup);

				int occupancy = _parentParcel.moccup;

				if (CamraSupport.ResidentialOccupancies.Contains(occupancy))
				{
					sectionValue = Convert.ToInt32(sectionRate * sectionSqft);
				}
				if (CamraSupport.CommercialOccupancies.Contains(occupancy))
				{
					if (bsreader["js0depr"].ToString() == "Y")
					{
						sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor));
					}
					else
					{
						sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor) * (1 - sectionDepreciation));
					}
				}
				if (CamraSupport.TaxExemptOccupancies.Contains(occupancy))
				{
					if (bsreader["js0depr"].ToString() == "Y")
					{
						sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor));
					}
					else
					{
						sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor) * (1 - sectionDepreciation));
					}
				}

				if (sectionData != null)
				{
					DataRow row = _BSTable.NewRow();
					row["Record"] = record;
					row["Card"] = card;
					row["SectionLetter"] = sectionData.SectionLetter.Trim();
					row["SectionType"] = sectionLetterType.Trim();
					row["SectionDescription"] = sectionTypeDescription.Trim();
					row["SectionStory"] = sectionData.SectionStory;
					row["SectionSize"] = sectionData.SectionSize;
					row["SectionClass"] = sectionData.SectionClass;
					row["SectionFactor"] = sectionData.SectionFactor;
					row["SectionDepreciation"] = sectionData.SectionDepreciation;
					row["SectionNoDepreciation"] = sectionData.SectionNoDepreciation;
					row["SectionRate"] = sectionRate;
					row["SectionValue"] = sectionValue;

					_BSTable.Rows.Add(row);

					this.Add(sectionData);
				}
			}

			return _BSTable;
		}

		public CPBuildingSection GetSingleSection(string sectionLetter)
		{
			var q = (from p in this
					 where p.SectionLetter == sectionLetter
					 select p).SingleOrDefault();

			return q;
		}

		private ParcelData _currentParcel = null;
		private SWallTech.CAMRA_Connection _fox = null;
		private DataTable _BSTable = new DataTable("BuildingSectionTable");

		private ParcelData _parentParcel = null;

		private CPBuildingSectionCollection()
		{
		}
	}
}