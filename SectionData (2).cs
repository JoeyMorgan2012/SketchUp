using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SWallTech.DB;
using System.IO;
using DevelopingForDotNet.Extensions;
using SWallTech.App.RE;


namespace CamraParcel
{
    public class SectionData
    {
        public int jsrecord { get; set; }
        public int jsdwell { get; set; }
        public string jssect { get; set; }
        public string jstype { get; set; }
        public decimal jsstory { get; set; }
        public string jsdesc { get; set; }
        public string jssketch { get; set; }
        public decimal jssqft { get; set; }
        public string js0depr { get; set; }
        public string jsclass { get; set; }
        public decimal jsvalue { get; set; }
        public decimal jsfactor { get; set; }
        public decimal jsdeprc { get; set; }

        public string orig_jssect { get; set; }
        public string orig_jstype { get; set; }
        public decimal orig_jsstory { get; set; }
        public string orig_jsdesc { get; set; }
        public string orig_jssketch { get; set; }
        public decimal orig_jssqft { get; set; }
        public string orig_js0depr { get; set; }
        public string orig_jsclass { get; set; }
        public decimal orig_jsvalue { get; set; }
        public decimal orig_jsfactor { get; set; }
        public decimal orig_jsdeprc { get; set; }



        public bool SectionisChanged
        {
            get
            {
                return (orig_jssect != jssect
                    || orig_jstype != jstype
                    || orig_jsstory != jsstory
                    || orig_jsdesc != jsdesc
                    || orig_jssketch != jssketch
                    || orig_jssqft != jssqft
                    || orig_js0depr != js0depr
                    || orig_jsclass != jsclass
                    || orig_jsvalue != jsvalue
                    || orig_jsfactor != jsfactor
                    || orig_jsdeprc != jsdeprc);

            }
        }

        public bool IsNewRecord { get; private set; }
        private bool IsDataValid
        {
            get
            {
                return jssect.Trim() != String.Empty;
            }
        }

        private string _insertSQL = "insert into {13}.{14}section (jsrecord,jsdwell,jssect,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ) " +
                    " values ({0},{1},'{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}',{10},{11},{12} ) ";

        //private string _insertAddSQL = "insert into addsection (jsrecord,jsdwell,jssect,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ) " +
        //            " values ({0},{1},'{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}',{10},{11},{12} ) ";

        public bool InsertOrUpdate(DBAccessManager _fox)
        {
            if (IsDataValid)
            {
                if (IsNewRecord)
                {
                    _fox.ExecuteNonSelectStatement(String.Format(_insertSQL,
                        jsrecord, jsdwell, jssect, jstype, jsstory, jsdesc, jssketch, jssqft, js0depr, jsclass, jsvalue, jsfactor, jsdeprc, CamraParcel.locallib, CamraParcel.localPreFix));
                }
                else
                {
                    // update logic
                }
                return true;
            }

            return false;
        }

        private SectionData()
        {
        }

        public static SectionData CreateNewSection(int record, int card, string sect)
        {
            return CreateNewSection(record, card, sect, true);
        }



        public void SetOriginalValues()
        {
            orig_jssect = jssect;
            orig_jstype = jstype;
            orig_jsstory = jsstory;
            orig_jsdesc = jsdesc;
            orig_jssketch = jssketch;
            orig_jssqft = jssqft;
            orig_js0depr = js0depr;
            orig_jsclass = jsclass;
            orig_jsvalue = jsvalue;
            orig_jsfactor = jsfactor;
            orig_jsdeprc = jsdeprc;
        }

        public static SectionData CreateNewSection(int record, int card, string sect, bool isNewRecord)
        {
            var sec = new SectionData()
            {
                IsNewRecord = isNewRecord,
                jsrecord = record,
                jsdwell = card,
                jssect = sect,
                jstype = String.Empty,
                jsstory = 0,
                jsdesc = String.Empty,
                jssketch = String.Empty,
                jssqft = 0,
                js0depr = String.Empty,
                jsclass = String.Empty,
                jsvalue = 0,
                jsfactor = 0,
                jsdeprc = 0,

                orig_jssect = sect,
                orig_jstype = String.Empty,
                orig_jsstory = 0,
                orig_jsdesc = String.Empty,
                orig_jssketch = String.Empty,
                orig_jssqft = 0,
                orig_js0depr = String.Empty,
                orig_jsclass = String.Empty,
                orig_jsvalue = 0,
                orig_jsfactor = 0,
                orig_jsdeprc = 0

            };

            if (isNewRecord)
            {
                sec.SetOriginalValues();
            }

            return sec;

        }

        public static SectionData getSection(DBAccessManager _fox, int record, int card)
        {
            SectionData _section = null;
            //SectionData _Addsection = null;

            StringBuilder subSection = new StringBuilder();
            subSection.Append("select jssect,jstype,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ");
            subSection.Append(String.Format("from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", CamraParcel.locallib, CamraParcel.localPreFix, record, card));

            DataSet Section = _fox.RunSelectStatement(subSection.ToString());

            //StringBuilder AddsubSection = new StringBuilder();
            //AddsubSection.Append("select jssect,jstype,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ");
            //AddsubSection.Append(String.Format("from addsection where jsrecord = {0} and jsdwell = {1} ", record, card));

            //DataSet AddSection = _fox.RunSelectStatement(AddsubSection.ToString());

            if (Section.Tables[0].Rows.Count > 0)
            {
                DataRow row = Section.Tables[0].Rows[0];



                _section = new SectionData()
                {
                    jsrecord = record,
                    jsdwell = card,

                    jssect = row["jssect"].ToString().Trim(),
                    jstype = row["jstype"].ToString().Trim(),
                    jsstory = Convert.ToDecimal(row["jsstory"].ToString()),
                    jsdesc = row["jsdesc"].ToString().Trim(),
                    jssketch = row["jssketch"].ToString().Trim(),
                    jssqft = Convert.ToDecimal(row["jssqft"].ToString()),
                    js0depr = row["js0depr"].ToString().Trim(),
                    jsclass = row["jsclass"].ToString().Trim(),
                    jsvalue = Convert.ToDecimal(row["jsvalue"].ToString()),
                    jsfactor = Convert.ToDecimal(row["jsfactor"].ToString()),
                    jsdeprc = Convert.ToDecimal(row["jsdeprc"].ToString()),

                    orig_jssect = row["jssect"].ToString().Trim(),
                    orig_jstype = row["jstype"].ToString().Trim(),
                    orig_jsstory = Convert.ToDecimal(row["jsstory"].ToString()),
                    orig_jsdesc = row["jsdesc"].ToString().Trim(),
                    orig_jssketch = row["jssketch"].ToString().Trim(),
                    orig_jssqft = Convert.ToDecimal(row["jssqft"].ToString()),
                    orig_js0depr = row["js0depr"].ToString().Trim(),
                    orig_jsclass = row["jsclass"].ToString().Trim(),
                    orig_jsvalue = Convert.ToDecimal(row["jsvalue"].ToString()),
                    orig_jsfactor = Convert.ToDecimal(row["jsfactor"].ToString()),
                    orig_jsdeprc = Convert.ToDecimal(row["jsdeprc"].ToString())

                };



            }

            //if (AddSection.Tables[0].Rows.Count > 0)
            //{
            //    DataRow addrow = AddSection.Tables[0].Rows[0];

            //    _section = new SectionData()
            //    {
            //        jsrecord = record,
            //        jsdwell = card,

            //        jssect = addrow["jssect"].ToString().Trim(),
            //        jstype = addrow["jstype"].ToString().Trim(),
            //        jsstory = Convert.ToDecimal(addrow["jsstory"].ToString()),
            //        jsdesc = addrow["jsdesc"].ToString().Trim(),
            //        jssketch = addrow["jssketch"].ToString().Trim(),
            //        jssqft = Convert.ToDecimal(addrow["jssqft"].ToString()),
            //        js0depr = addrow["js0depr"].ToString().Trim(),
            //        jsclass = addrow["jsclass"].ToString().Trim(),
            //        jsvalue = Convert.ToDecimal(addrow["jsvalue"].ToString()),
            //        jsfactor = Convert.ToDecimal(addrow["jsfactor"].ToString()),
            //        jsdeprc = Convert.ToDecimal(addrow["jsdeprc"].ToString()),

            //        orig_jssect = addrow["jssect"].ToString().Trim(),
            //        orig_jstype = addrow["jstype"].ToString().Trim(),
            //        orig_jsstory = Convert.ToDecimal(addrow["jsstory"].ToString()),
            //        orig_jsdesc = addrow["jsdesc"].ToString().Trim(),
            //        orig_jssketch = addrow["jssketch"].ToString().Trim(),
            //        orig_jssqft = Convert.ToDecimal(addrow["jssqft"].ToString()),
            //        orig_js0depr = addrow["js0depr"].ToString().Trim(),
            //        orig_jsclass = addrow["jsclass"].ToString().Trim(),
            //        orig_jsvalue = Convert.ToDecimal(addrow["jsvalue"].ToString()),
            //        orig_jsfactor = Convert.ToDecimal(addrow["jsfactor"].ToString()),
            //        orig_jsdeprc = Convert.ToDecimal(addrow["jsdeprc"].ToString())

            //    };


            //}

            return _section;

        }


    }
}
