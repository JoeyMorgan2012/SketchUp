using System;
using System.Data;
using System.Text;

namespace SketchUp
{
    public class ValuesTracking
    {
        public event EventHandler<ValuesTrackingEventsArgs> ValuesTrackingChangedEvent;

        public int Record
        {
            get; set;
        }

        public int Card
        {
            get; set;
        }

        public int SeqNo
        {
            get; set;
        }

        public int VLand
        {
            get; set;
        }

        public int VImpr
        {
            get; set;
        }

        public int VTotal
        {
            get; set;
        }

        public string VReason
        {
            get; set;
        }

        public int VEftxy
        {
            get; set;
        }

        public string VUser
        {
            get; set;
        }

        public string VChgDate
        {
            get; set;
        }

        public ValuesTracking()
        {
        }

        public static ValuesTracking GetValuesTracking(SWallTech.CAMRA_Connection conn, string prefix, int recno, int card, int seqno)
        {
            var db = conn.DBConnection;

            ValuesTracking ValuesTrackingList = null;

            StringBuilder vtSql = new StringBuilder();
            vtSql.Append("select vland,vimpr,vtotal,vreasn,vusrid,vchgmo,vchgda,vchgyr,veftxy ");
            vtSql.Append(String.Format("from {0}.{1}valu ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            vtSql.Append(String.Format("where vrecno = {0} and vdwell = {1} and vseqno = {2} ", recno, card, seqno));
            vtSql.Append("order by vseqno desc ");

            DataSet ds = db.RunSelectStatement(vtSql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow VTreader = ds.Tables[0].Rows[0];

                ValuesTrackingList = new ValuesTracking()
                {
                    Record = recno,
                    Card = card,
                    SeqNo = seqno,
                    VLand = Convert.ToInt32(VTreader["vland"].ToString()),
                    VImpr = Convert.ToInt32(VTreader["vimpr"].ToString()),
                    VTotal = Convert.ToInt32(VTreader["vtotal"].ToString()),
                    VReason = VTreader["vreasn"].ToString().Trim(),
                    VUser = VTreader["vusrid"].ToString().Trim(),
                    VChgDate = String.Format("{0}/{1}/{2}",
                            VTreader["vchgmo"].ToString().Trim(),
                            VTreader["vchgda"].ToString().Trim(),
                            VTreader["vchgyr"].ToString().Trim()),

                    VEftxy = Convert.ToInt32(VTreader["veftxy"].ToString())
                };
            }

            return ValuesTrackingList;
        }

        private void FireChangedEvent(string ValuesTrackingfile)
        {
            if (ValuesTrackingChangedEvent != null)
            {
                ValuesTrackingChangedEvent(this,
                    new ValuesTrackingEventsArgs()
                    {
                        ValuesTrackingList = ValuesTrackingfile
                    });
            }
        }
    }

    public class ValuesTrackingEventsArgs : EventArgs
    {
        public ValuesTrackingEventsArgs()
            : base()
        {
        }

        public string ValuesTrackingList
        {
            get; set;
        }
    }
}