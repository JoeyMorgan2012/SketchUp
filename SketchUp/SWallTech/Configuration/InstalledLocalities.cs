using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SWallTech
{
	/// <summary>
	/// Summary description for InstalledLocalities.
	/// </summary>
	public class InstalledLocalities
	{
		/// <summary>
		/// A simple class for listing the installed localities for RE.
		/// </summary>
		/// <param name="db">DBAccessManager object representing the database connection.</param>
		public InstalledLocalities(DBAccessManager db)
		{
			this.db = db;
			this.listByCode = new SortedList();
			this.listByName = new SortedList();
			this.codes = new List<string>();
			this.names = new List<string>();
			this.listMapConfig = new SortedList();
			this.listBatchFiles = new SortedList<string, List<string>>();
			this.extendedText = new SortedList<string, string>();
			prefixLibraries = new Dictionary<string, string>();
			this.buildList();
		}

		/// <summary>
		/// Gets the list of installed localities prefixes
		/// </summary>
		public List<string> Codes
		{
			get
			{
				return this.codes;
			}
		}

		/// <summary>
		/// Gets the list of loc codes and extended text (code - name)
		/// </summary>
		public SortedList<string, string> ExtendedDescriptionList
		{
			get
			{
				return this.extendedText;
			}
		}

		/// <summary>
		/// Gets the list of installed localities sorted by Locality Code
		/// </summary>
		public SortedList GetListByCode
		{
			get
			{
				return this.listByCode;
			}
		}

		/// <summary>
		/// Gets the list of installed localities sorted by Name
		/// </summary>
		public SortedList GetListByName
		{
			get
			{
				return this.listByName;
			}
		}

		/// <summary>
		/// Gets the list of installed locality names
		/// </summary>
		public List<string> Names
		{
			get
			{
				return this.names;
			}
		}

		/// <summary>
		/// Gets the list of Batch Files based on the Locality Prefix
		/// </summary>
		public List<string> GetBatchList(string localityPrefix)
		{
			return this.listBatchFiles[localityPrefix];
		}

		public string GetCodeFromExtendedDescription(string desc)
		{
			string loc = "";
			if (extendedText.ContainsValue(desc))
			{
				loc = extendedText.Keys[extendedText.IndexOfValue(desc)];
			}
			else
			{
				throw new ArgumentException(desc + " not found.");
			}
			return loc;
		}

		/// <summary>
		/// Gets the Locality Prefix based on the Locality Name
		/// </summary>
		public string GetLocalityLibrary(string localityPrefix)
		{
			return prefixLibraries[localityPrefix];
		}

		/// <summary>
		/// Gets the Locality Name based on the Locality Prefix
		/// </summary>
		public string GetLocalityName(string localityPrefix)
		{
			try
			{
				string sql = string.Format("select DISTINCT DESC30 from camfilib.cafpsysctl WHERE PREFIX='{0}'", localityPrefix);
				Console.WriteLine(string.Format("{0}", sql));
				var localName = db.ExecuteScalar(sql);
				return localName.ToString().Trim();
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Error in {0} \n{1}", MethodBase.GetCurrentMethod().Name, ex.Message));

				throw;
			}
		}

		/// <summary>
		/// Gets the Library (database) name of the installed RE data.
		/// </summary>
		//public string InstalledLibrary
		//{
		//    get
		//    {
		//        return this.library ;
		//    }
		//}
		/// <summary>
		/// Gets the Locality Prefix based on the Locality Name
		/// </summary>
		public string GetLocalityPrefix(string localityName)
		{
			return this.listByName[localityName].ToString();
		}

		/// <summary>
		/// Gets the Locality MapConfig object based on the Locality Prefix
		/// </summary>
		public MapConfig GetMapConfig(string localityPrefix)
		{
			return (MapConfig)this.listMapConfig[localityPrefix];
		}

		public List<string> RefreshBatchList(string localityCode)
		{
			List<string> b = this.BuildBatchList(localityCode);
			if (this.listBatchFiles.ContainsKey(localityCode))
			{
				this.listBatchFiles[localityCode] = b;
			}
			else
			{
				this.listBatchFiles.Add(localityCode, b);
			}

			return b;
		}

		#region fields

		private List<string> codes;
		private DBAccessManager db;
		private SortedList<string, string> extendedText;
		private SortedList<string, List<string>> listBatchFiles;
		private SortedList listByCode;
		private SortedList listByName;
		private SortedList listMapConfig;
		private List<string> names;

		//private string library;
		private Dictionary<string, string> prefixLibraries;

		#endregion fields

		private List<string> BuildBatchList(string localityCode)
		{
			// Get Batches
			List<string> batches = new List<string>();
			string sql = "select NAME from sysibm.sqltables where table00001 = '" +
				prefixLibraries[localityCode] + "' and name like '" + localityCode +
				"BT%' order by NAME";
			IDataReader batchReader = this.db.getDataReader(sql);
			int NAME_ord = batchReader.GetOrdinal("NAME");
			while (batchReader.Read())
			{
				string file = batchReader.GetString(NAME_ord).Trim();
				if (file.Length == 7)
				{
					batches.Add(file.Substring(5, 2));
				}
			}
			batchReader.Close();

			return batches;
		}

		private void buildList()
		{
			this.listByCode.Clear();
			this.listByName.Clear();
			this.listMapConfig.Clear();
			this.listBatchFiles.Clear();
			this.extendedText.Clear();
			prefixLibraries.Clear();

			if (this.db.IsConnected)
			{
				//

				IDataReader reader = this.db.getDataReader("select PREFIX,DESC30,LIBRARY from " +
					" camfilib.cafpsysctl  order by DESC30 ");
				int TLOC_ord = reader.GetOrdinal("PREFIX");
				int TDESC_ord = reader.GetOrdinal("DESC30");
				int TLibrary_ord = reader.GetOrdinal("LIBRARY");

				while (reader.Read())
				{
					string code = reader.GetString(TLOC_ord).Trim();
					string name = reader.GetString(TDESC_ord).Trim();
					string library = reader.GetString(TLibrary_ord).Trim();
					this.listByCode.Add(code, name);
					this.listByName.Add(name, code);
					this.extendedText.Add(code, code + " - " + name);
					this.codes.Add(code);
					this.names.Add(name);

					prefixLibraries.Add(code, library);

					//this.listMapConfig.Add(code, new MapConfig(this.db, library, code));
				}

				if (reader != null)
				{
					if (!reader.IsClosed)
					{
						reader.Close();
						try
						{
							reader.Dispose();
						}
						catch (Exception)
						{
							/* Just to make sure the reader is destroyed and
							releases its memory.
							*/
						}
					}
				}

				foreach (string s in this.listByCode.Keys)
				{
					this.listBatchFiles.Add(s, this.BuildBatchList(s));
				}

				this.codes.Sort();
				this.names.Sort();
			}
		}
	}
}