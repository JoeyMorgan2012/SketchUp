using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SWallTech.DB;
using SWallTech.App.RE;

namespace CamraParcel
{
    public class SectionDataCollection : List<SectionData>
    {
        public CAMRA_Connection DatabaseConnection { get; set; }
        public int Record { get; set; }
        public int Card { get; set; }

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

        public SectionDataCollection(CAMRA_Connection fox, int record, int cardnum)
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
            subSection.Append(String.Format("from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", CamraParcel.locallib, CamraParcel.localPreFix, Record, Card));
            subSection.Append("order by jssect ");

            DataSet _Section = DatabaseConnection.DBConnection.RunSelectStatement(subSection.ToString());

            //StringBuilder AddsubSection = new StringBuilder();
            //AddsubSection.Append("select jssect,jstype,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ");
            //AddsubSection.Append(String.Format("from addsection where jsrecord = {0} and jsdwell = {1} ", Record, Card));

            //DataSet _AddSection = DatabaseConnection.DBConnection.RunSelectStatement(AddsubSection.ToString());

            foreach (DataRow row in _Section.Tables[0].Rows)
            {
                string subsect = row["jssect"].ToString().Trim();
                var _sect = SectionData.CreateNewSection(Record, Card, subsect, false);
                _sect.jssect = row["jssect"].ToString().Trim();
                _sect.jstype = row["jstype"].ToString().Trim();
                _sect.jsstory = Convert.ToDecimal(row["jsstory"].ToString());
                _sect.jsdesc = row["jsdesc"].ToString().Trim();
                _sect.jssketch = row["jssketch"].ToString().Trim();
                _sect.jssqft = Convert.ToDecimal(row["jssqft"].ToString());
                _sect.js0depr = row["js0depr"].ToString().Trim();
                _sect.jsclass = row["jsclass"].ToString().Trim();
                _sect.jsvalue = Convert.ToDecimal(row["jsvalue"].ToString());
                _sect.jsfactor = Convert.ToDecimal(row["jsfactor"].ToString());
                _sect.jsdeprc = Convert.ToDecimal(row["jsdeprc"].ToString());
                _sect.SetOriginalValues();

                this.Add(_sect);

            }
            //foreach (DataRow addrow in _AddSection.Tables[0].Rows)
            //{
            //    string subsect = addrow["jssect"].ToString().Trim();
            //    var _addsect = SectionData.CreateNewSection(Record, Card, subsect, false);
            //    _addsect.jssect = addrow["jssect"].ToString().Trim();
            //    _addsect.jstype = addrow["jstype"].ToString().Trim();
            //    _addsect.jsstory = Convert.ToDecimal(addrow["jsstory"].ToString());
            //    _addsect.jsdesc = addrow["jsdesc"].ToString().Trim();
            //    _addsect.jssketch = addrow["jssketch"].ToString().Trim();
            //    _addsect.jssqft = Convert.ToDecimal(addrow["jssqft"].ToString());
            //    _addsect.js0depr = addrow["js0depr"].ToString().Trim();
            //    _addsect.jsclass = addrow["jsclass"].ToString().Trim();
            //    _addsect.jsvalue = Convert.ToDecimal(addrow["jsvalue"].ToString());
            //    _addsect.jsfactor = Convert.ToDecimal(addrow["jsfactor"].ToString());
            //    _addsect.jsdeprc = Convert.ToDecimal(addrow["jsdeprc"].ToString());
            //    _addsect.SetOriginalValues();

            //    this.Add(_addsect);

            //}
        }
        public int updatetoSectionlData()
        {
            int updatedCount = 0;
            int itemUpdateCount = 0;

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
