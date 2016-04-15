using System;
using System.IO;
using System.Text;

namespace SketchUp
{
    public class ImprData
    {
        public string iid
        {
            get; set;
        }

        public int irecno
        {
            get; set;
        }

        public int idwell
        {
            get; set;
        }

        public int iseqno
        {
            get; set;
        }

        public string idesc
        {
            get; set;
        }

        public decimal ilen
        {
            get; set;
        }

        public decimal iwid
        {
            get; set;
        }

        public string icond
        {
            get; set;
        }

        public string ifill1
        {
            get; set;
        }

        public int itotv
        {
            get; set;
        }

        public decimal idepr
        {
            get; set;
        }

        public decimal irate
        {
            get; set;
        }

        public string ifill2
        {
            get; set;
        }

        public int icode
        {
            get; set;
        }

        public string ifill3
        {
            get; set;
        }

        public string orig_iid
        {
            get; set;
        }

        public string orig_idesc
        {
            get; set;
        }

        public decimal orig_ilen
        {
            get; set;
        }

        public decimal orig_iwid
        {
            get; set;
        }

        public string orig_icond
        {
            get; set;
        }

        public string orig_ifill1
        {
            get; set;
        }

        public int orig_itotv
        {
            get; set;
        }

        public decimal orig_idepr
        {
            get; set;
        }

        public decimal orig_irate
        {
            get; set;
        }

        public string orig_ifill2
        {
            get; set;
        }

        public int orig_icode
        {
            get; set;
        }

        public string orig_ifill3
        {
            get; set;
        }

        public bool ImprisChanged
        {
            get
            {
                return (orig_iid.Trim() != iid.Trim()
                    || orig_idesc.Trim() != idesc.Trim()
                    || orig_ilen != ilen
                    || orig_iwid != iwid
                    || orig_icond.Trim() != icond.Trim()
                    || orig_itotv != itotv
                    || orig_idepr != idepr
                    || orig_irate != irate
                    || orig_icode != icode);
            }
        }
    }
}