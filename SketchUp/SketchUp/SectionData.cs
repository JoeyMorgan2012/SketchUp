using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public class SectionData
    {
        #region Properties

        private SectionData()
        {
        }

        public bool IsNewRecord
        {
            get; private set;
        }

        public string js0depr
        {
            get; set;
        }

        public string jsclass
        {
            get; set;
        }

        public decimal jsdeprc
        {
            get; set;
        }

        public string jsdesc
        {
            get; set;
        }

        public int jsdwell
        {
            get; set;
        }

        public decimal jsfactor
        {
            get; set;
        }

        public int jsrecord
        {
            get; set;
        }

        public string jssect
        {
            get; set;
        }

        public string jssketch
        {
            get; set;
        }

        public decimal jssqft
        {
            get; set;
        }

        public decimal jsstory
        {
            get; set;
        }

        public string jstype
        {
            get; set;
        }

        public decimal jsvalue
        {
            get; set;
        }

        public string orig_js0depr
        {
            get; set;
        }

        public string orig_jsclass
        {
            get; set;
        }

        public decimal orig_jsdeprc
        {
            get; set;
        }

        public string orig_jsdesc
        {
            get; set;
        }

        public decimal orig_jsfactor
        {
            get; set;
        }

        public string orig_jssect
        {
            get; set;
        }

        public string orig_jssketch
        {
            get; set;
        }

        public decimal orig_jssqft
        {
            get; set;
        }

        public decimal orig_jsstory
        {
            get; set;
        }

        public string orig_jstype
        {
            get; set;
        }

        public decimal orig_jsvalue
        {
            get; set;
        }

        #endregion Properties

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

        private bool IsDataValid
        {
            get
            {
                return jssect.Trim() != String.Empty;
            }
        }

        //private string _insertAddSQL = "insert into addsection (jsrecord,jsdwell,jssect,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ) " +
        //            " values ({0},{1},'{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}',{10},{11},{12} ) ";

        public static SectionData CreateNewSection(int record, int card, string sect)
        {
            return CreateNewSection(record, card, sect, true);
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
            //TODO: Find out how the heck it knows which section to get.
            SectionData _section = null;

            //SectionData _Addsection = null;

            string subSection = string.Format("select jssect,jstype,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc  from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, record, card);

            IDataReader dr = _fox.getDataReader(subSection);

            while (dr.Read())
            {
                _section = new SectionData()
                {
                    jsrecord = record,
                    jsdwell = card,

                    jssect = dr["jssect"].ToString().Trim(),
                    jstype = dr["jstype"].ToString().Trim(),
                    jsstory = Convert.ToDecimal(dr["jsstory"].ToString()),
                    jsdesc = dr["jsdesc"].ToString().Trim(),
                    jssketch = dr["jssketch"].ToString().Trim(),
                    jssqft = Convert.ToDecimal(dr["jssqft"].ToString()),
                    js0depr = dr["js0depr"].ToString().Trim(),
                    jsclass = dr["jsclass"].ToString().Trim(),
                    jsvalue = Convert.ToDecimal(dr["jsvalue"].ToString()),
                    jsfactor = Convert.ToDecimal(dr["jsfactor"].ToString()),
                    jsdeprc = Convert.ToDecimal(dr["jsdeprc"].ToString()),

                    orig_jssect = dr["jssect"].ToString().Trim(),
                    orig_jstype = dr["jstype"].ToString().Trim(),
                    orig_jsstory = Convert.ToDecimal(dr["jsstory"].ToString()),
                    orig_jsdesc = dr["jsdesc"].ToString().Trim(),
                    orig_jssketch = dr["jssketch"].ToString().Trim(),
                    orig_jssqft = Convert.ToDecimal(dr["jssqft"].ToString()),
                    orig_js0depr = dr["js0depr"].ToString().Trim(),
                    orig_jsclass = dr["jsclass"].ToString().Trim(),
                    orig_jsvalue = Convert.ToDecimal(dr["jsvalue"].ToString()),
                    orig_jsfactor = Convert.ToDecimal(dr["jsfactor"].ToString()),
                    orig_jsdeprc = Convert.ToDecimal(dr["jsdeprc"].ToString())
                };
            }
            return _section;
        }

        [CodeRefactoringState(IsToDo =true,ChangeDescription ="Section CRUD is not implemented.",ReplacesExistingCode =false)]
        public bool InsertOrUpdate(DBAccessManager _fox)
        {
            if (IsDataValid)
            {
                if (IsNewRecord)
                {
                    string _insertSQL = string.Format("insert into {13}.{14}section (jsrecord,jsdwell,jssect,jsstory,jsdesc,jssketch,jssqft,js0depr,jsclass,jsvalue,jsfactor,jsdeprc ) values ({0},{1},'{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}',{10},{11},{12} ) ", jsrecord, jsdwell, jssect, jstype, jsstory, jsdesc, jssketch, jssqft, js0depr, jsclass, jsvalue, jsfactor, jsdeprc, SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix);
                    
                    //TODO: Add the CRUD operations
                
                }
                else
                {
                    // update logic
                }
                return true;
            }

            return false;
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
    }
}