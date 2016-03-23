using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SketchUp
{
	public class SectionDataCollection : List<SectionData>
	{
		public SWallTech.CAMRA_Connection DatabaseConnection
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

		private SectionDataCollection()
		{
		}

		public bool IsSectionCollectionChanged
		{
			get
			{
				foreach (var item in this)
				{
					if (item.SectionisChanged)
						return true;
				}

				return false;
			}
		}

		public SectionDataCollection(SWallTech.CAMRA_Connection fox, int record, int cardnum)
		{
			DatabaseConnection = fox;
			Record = record;
			Card = cardnum;

			getSection();
		}

		public void getSection()
		{
			StringBuilder subSection = new StringBuilder();
			subSection.Append("select jssect,jstype,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ");
			subSection.Append(String.Format("from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", MainForm.FClib, MainForm.FCprefix, Record, Card));
			subSection.Append("order by jssect ");

			IDataReader _Section = DatabaseConnection.DBConnection.getDataReader(subSection.ToString());

			//StringBuilder AddsubSection = new StringBuilder();
			//AddsubSection.Append("select jssect,jstype,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ");
			//AddsubSection.Append(String.Format("from addsection where jsrecord = {0} and jsdwell = {1} ", Record, Card));

			//DataSet _AddSection = DatabaseConnection.DBConnection.RunSelectStatement(AddsubSection.ToString());

			while (_Section.Read())
			{
				string subsect = _Section["jssect"].ToString().Trim();
				var _sect = SectionData.CreateNewSection(Record, Card, subsect, false);
				_sect.jssect = _Section["jssect"].ToString().Trim();
				_sect.jstype = _Section["jstype"].ToString().Trim();
				_sect.jsstory = Convert.ToDecimal(_Section["jsstory"].ToString());
				_sect.jsdesc = _Section["jsdesc"].ToString().Trim();
				_sect.jssketch = _Section["jssketch"].ToString().Trim();
				_sect.jssqft = Convert.ToDecimal(_Section["jssqft"].ToString());
				_sect.js0depr = _Section["js0depr"].ToString().Trim();
				_sect.jsclass = _Section["jsclass"].ToString().Trim();
				_sect.jsvalue = Convert.ToDecimal(_Section["jsvalue"].ToString());
				_sect.jsfactor = Convert.ToDecimal(_Section["jsfactor"].ToString());
				_sect.jsdeprc = Convert.ToDecimal(_Section["jsdeprc"].ToString());
				_sect.SetOriginalValues();

				this.Add(_sect);
			}

			//foreach (DataRow addrow in _AddSection.Tables[0].Rows)
			//{
			//	string subsect = addrow["jssect"].ToString().Trim();
			//	var _addsect = SectionData.CreateNewSection(Record, Card, subsect, false);
			//	_addsect.jssect = addrow["jssect"].ToString().Trim();
			//	_addsect.jstype = addrow["jstype"].ToString().Trim();
			//	_addsect.jsstory = Convert.ToDecimal(addrow["jsstory"].ToString());
			//	_addsect.jsdesc = addrow["jsdesc"].ToString().Trim();
			//	_addsect.jssketch = addrow["jssketch"].ToString().Trim();
			//	_addsect.jssqft = Convert.ToDecimal(addrow["jssqft"].ToString());
			//	_addsect.js0depr = addrow["js0depr"].ToString().Trim();
			//	_addsect.jsclass = addrow["jsclass"].ToString().Trim();
			//	_addsect.jsvalue = Convert.ToDecimal(addrow["jsvalue"].ToString());
			//	_addsect.jsfactor = Convert.ToDecimal(addrow["jsfactor"].ToString());
			//	_addsect.jsdeprc = Convert.ToDecimal(addrow["jsdeprc"].ToString());
			//	_addsect.SetOriginalValues();

			//	this.Add(_addsect);

			//}
		}

		public int updatetoSectionlData()
		{
			int updatedCount = 0;

			//int itemUpdateCount = 0;

			foreach (var item in this)
			{
				if (item.SectionisChanged)
				{
				}
			}

			return updatedCount;
		}
	}
}