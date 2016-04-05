using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using SWallTech;

namespace SWallTech
{
	public class SketchRepository
	{
		#region DAL Methods

		public SMParcel SelectParcelData(int recordNumber, int dwellingNumber)
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
					return parcel;
				}
				else
				{
					return new SMParcel();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		public List<SMSection> SelectParcelSections(SMParcel parcel)
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

						sections.Add(new SMSection(parcel)
						{
							Record = parcel.Record,
							Dwelling = parcel.Card,
							SectionLetter = row["JSSECT"].ToString().Trim(),
							SectionType = row["JSTYPE"].ToString().Trim(),
							Storeys = storeys,
							Description = row["JSDESC"].ToString().Trim(),
							HasSketch = row["JSSKETCH"].ToString().Trim(),
							SqFt = squareFeet,
							ZeroDepr = row["JS0DEPR"].ToString().Trim(),
							SectionClass = row["JSCLASS"].ToString().Trim(),
							SectionValue = value,
							AdjFactor = factor,
							Depreciation = depreciation,
							RefreshSection = false,
							ParentParcel = parcel
						});
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

		public List<SMLine> SelectSectionLines(SMSection section)

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
						Int32.TryParse(row["JLLINE#"].ToString().Trim(), out lineNumber);
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

		#endregion DAL Methods

		#region Constructor

		public SketchRepository(string dbDataSource, string dbUserName, string dbPassword, string localityPrefix)
		{
			SketchConnection = new SMConnection(dbDataSource, dbUserName, dbPassword, localityPrefix);
		}

		#endregion Constructor

		#region Properties

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
				return masterRecordTable;
			}

			set
			{
				masterRecordTable = value;
			}
		}

		public string Password
		{
			get
			{
				return passworM;
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
				return sectionRecordTable;
			}

			set
			{
				sectionRecordTable = value;
			}
		}

		public ISMConnection SketchConnection
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

		#endregion Properties

		public List<SMParcel> SketchSnapshots()
		{
			List<SMParcel> sketchSnapshots = new List<SMParcel>();

			return sketchSnapshots;
		}

	

		
		#region Fields

		private string dataSource;
		private string lineRecordTable;
		private string locality;
		private string masterRecordTable;
		private string passworM;
		private string sectionRecordTable;
		private ISMConnection sketchConnection;
		private string userName;
		private string password;

		#endregion Fields
	}
}