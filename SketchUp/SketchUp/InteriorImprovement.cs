using System;
using System.Data;
using System.Text;

namespace SketchUp
{
    public class InteriorImprovement
    {
        public event EventHandler<InteriorImpChangedEventArgs> InteriorImpChangedEvent;

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

        public string Decription
        {
            get; set;
        }

        public int Quantity
        {
            get; set;
        }

        public int UnitPrice
        {
            get; set;
        }

        public int UnitTotal
        {
            get; set;
        }

        public InteriorImprovement()
        {
        }

        public static InteriorImprovement GetImprovement(SWallTech.CAMRA_Connection fox, int recno, int card, int seqno)
        {
            var db = fox;

            InteriorImprovement InteriorImp = null;

            StringBuilder iisql = new StringBuilder();
            iisql.Append("select udesc,uqty,uprice,utotal ");
            iisql.Append(String.Format(" from {0}.{1}bimp where urecno = {2} and udwell = {3} and useqno = {4} ",
                SketchUpGlobals.LocalLib,
                SketchUpGlobals.LocalityPreFix,
                recno,
                card,
                seqno));
            iisql.Append(" and utotal > 0 ");

            DataSet ds = db.DBConnection.RunSelectStatement(iisql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow IIreader = ds.Tables[0].Rows[0];
                InteriorImp = new InteriorImprovement()
                {
                    Record = recno,
                    Card = card,
                    SeqNo = seqno,
                    Decription = IIreader["udesc"].ToString().Trim(),
                    Quantity = Convert.ToInt32(IIreader["uqty"].ToString()),
                    UnitPrice = Convert.ToInt32(IIreader["uprice"].ToString()),
                    UnitTotal = Convert.ToInt32(IIreader["utotal"].ToString())
                };
            }

            return InteriorImp;
        }

        private void FireChangedEvent(string InteriorImpfile)
        {
            if (InteriorImpChangedEvent != null)
            {
                InteriorImpChangedEvent(this,
                    new InteriorImpChangedEventArgs()
                    {
                        InteriorImp = InteriorImpfile
                    });
            }
        }
    }

    public class InteriorImpChangedEventArgs : EventArgs
    {
        public InteriorImpChangedEventArgs()
            : base()
        {
        }

        public string InteriorImp
        {
            get; set;
        }
    }
}