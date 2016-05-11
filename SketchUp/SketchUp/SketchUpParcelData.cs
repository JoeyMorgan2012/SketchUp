using SWallTech;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public class SketchUpParcelData
    {
      

        public SketchUpParcelData()
        {
        }

#region "Public Methods"

public decimal GetClassValue(string cls)
        {
            decimal retValue = 0;
            if (SketchUpCamraSupport.ClassCodeList().Contains(cls))
            {
                retValue = SketchUpCamraSupport.ClassValue(cls);
            }
            return retValue;
        }

        public static SketchUpParcelData getParcel(CAMRA_Connection camConn, int record, int card)
        {
         
            // Omitted any fields not needed for SketchUp. JMM 5-9-2016
           string getParcelSql = string.Format("select mrecid,mrecno,mdwell,moccup,mclass,  mgart,mgar#c,mcarpt,mcar#c,mgart2,mgar#2 from {0}.{1}mast  where mrecno = {0} and mdwell = {1} and moccup < 30 ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, record, card);

            DataSet mastParcelData = camConn.DBConnection.RunSelectStatement(getParcelSql.ToString());

            if (mastParcelData.Tables[0].Rows.Count > 0)
            {
                DataRow row = mastParcelData.Tables[0].Rows[0];

                string chkStatus = String.Empty;
                StringBuilder checkStatus = new StringBuilder();
                checkStatus.Append(String.Format("select icstatus from parrevlib.{0}irchgd where icrecno = {1} and iccard = {2} and icseqno = 1 ",
                    SketchUpGlobals.LocalityPreFix, _parcel.mrecno, _parcel.Mdwell));
                try
                {
                    DataSet cks = camConn.DBConnection.RunSelectStatement(checkStatus.ToString());

                    if (cks.Tables[0].Rows.Count > 0)
                    {
                        chkStatus = cks.Tables[0].Rows[0]["icstatus"].ToString();
                    }
                }
                catch
                {
                }

         
                _parcel.updatestatus = chkStatus.Trim();

                _parcel.ValidRecord = true;
            }

            return _parcel;
        }

        public Bitmap GetSketchImage()
        {
            SMParcel parcel = SketchUpGlobals.ParcelWorkingCopy;
            Bitmap placeholderBmp = new Bitmap(400, 400);
            PictureBox placeHolderPict = new PictureBox { Width = placeholderBmp.Width, Height = placeholderBmp.Height };
            SMSketcher sms = new SMSketcher(parcel, placeHolderPict,placeholderBmp);
            sms.RenderSketch();
            parcel.SetScaleAndOriginForParcel(placeHolderPict);
            placeholderBmp=(Bitmap)sms.SketchImage;
            return placeholderBmp;
        }

#endregion

#region "Private methods"

        private bool AnyValueHasChanged()
        {
            bool somethingChanged = orig_macre.Trim() != macre.Trim()
                    || orig_mluse.Trim() != mluse.Trim()
                    || orig_moccup != Moccup
                    || orig_mstory.Trim() != mstory.Trim()
                    || orig_mclass.Trim() != mclass.Trim()
                    || orig_mfp2.Trim() != mfp2.Trim()
                    || orig_mltrcd.Trim() != mltrcd.Trim()
                    || orig_mpbtot != mpbtot
                    || orig_msbtot != msbtot
                    || orig_mgart != mgart
                    || orig_mgarNc != mgarNc
                    || orig_mcarpt != mcarpt
                    || orig_mcarNc != mcarNc
                    || orig_mgart2 != mgart2
                    || orig_mgarN2 != mgarN2;
            return somethingChanged;
        }

        private void CalculateParcel()
        { //TODO: Refactor into SketchManager
            //TODO: Replace the valuation calculations with the minimal calcs needed for sketches.
                        string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void FireChangedEvent(string property)
        {
            if (_isDirtyCheckingOn)
            {
                if (ParcelChangedEvent != null)
                {
                    ParcelChangedEvent(this,
                        new ParcelChangedEventArgs()
                        {
                            PropertyName = property
                        });
                }
            }
        }

        private void SetOriginalValues()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif

            //if (this.m1um.Trim() == String.Empty)
            //{
            //	this.orig_curVal1 = 0;
            //}
            //if (this.m1um == "F")
            //{
            //	this.orig_curVal1 = Convert.ToInt32(Convert.ToInt32((((this.m1frnt * this.m1rate) * this.m1dfac) * (1 + this.m1adj))).RoundHundredsToString().Replace(",", ""));
            //}
            //if (this.m1um == "S")
            //{
            //	int chkArea1 = 0;
            //	if (this.m1area > 0 && this.m1frnt == 0 && this.m1dpth == 0)
            //	{
            //		chkArea1 = this.m1area;
            //		this.curVal1 = Convert.ToInt32((chkArea1 * this.m1rate) * (1 + this.m1adj));
            //	}

            //	if (chkArea1 == 0)
            //	{
            //		chkArea1 = Convert.ToInt32(this.m1frnt * this.m1dpth);
            //		if (this.m1area != chkArea1 && this.m1area > 0)
            //		{
            //			this.m1area = chkArea1;
            //			this.curVal1 = Convert.ToInt32((chkArea1 * this.m1rate) * (1 + this.m1adj));
            //		}
            //	}

            //	if (chkArea1 > 0)
            //	{
            //		this.curVal1 = Convert.ToInt32((chkArea1 * this.m1rate) * (1 + this.m1adj));
            //	}
            //}
            //if (this.m2um.Trim() == String.Empty)
            //{
            //	this.orig_curVal2 = 0;
            //}
            //if (this.m2um == "L")
            //{
            //	this.orig_curVal2 = Convert.ToInt32(this.m2rate * (1 + this.m2adj));
            //}

            //if (this.m2um == "F")
            //{
            //	this.orig_curVal2 = Convert.ToInt32(Convert.ToInt32((((this.m2frnt * this.m2rate) * this.m2dfac) * (1 + this.m2adj))).RoundHundredsToString().Replace(",", ""));
            //}
            //if (this.m2um == "S")
            //{
            //	int chkArea2 = 0;
            //	if (this.m2area > 0 && this.m2frnt == 0 && this.m2dpth == 0)
            //	{
            //		chkArea2 = this.m2area;
            //		this.curVal2 = Convert.ToInt32((chkArea2 * this.m2rate) * (1 + this.m2adj));
            //	}

            //	if (chkArea2 == 0)
            //	{
            //		chkArea2 = Convert.ToInt32(this.m2frnt * this.m2dpth);
            //		if (this.m2area != chkArea2 && this.m2area > 0)
            //		{
            //			this.m2area = chkArea2;
            //			this.curVal2 = Convert.ToInt32((chkArea2 * this.m2rate) * (1 + this.m2adj));
            //		}
            //	}

            //	if (chkArea2 > 0)
            //	{
            //		this.curVal2 = Convert.ToInt32((chkArea2 * this.m2rate) * (1 + this.m2adj));
            //	}
            //}
            //if (this.m2um == "L")
            //{
            //	this.orig_curVal2 = Convert.ToInt32(this.m2rate * (1 + this.m2adj));
            //}

            //if (CamraSupport.ResidentialOccupancyCodes.Contains(this.moccup) && this.moccup != 16)
            //{
            //	computedFactor = Decimal.Round((GetClassValue(this.Class) + this.Factor), 2);
            //	orig_computedFactor = Decimal.Round((GetClassValue(this.Class) + this.Factor), 2);
            //}

            //if (CamraSupport.CommercialOccupancyCodes.Contains(this.moccup) && this.moccup != 26)
            //{
            //	computedFactor = 0;
            //	orig_computedFactor = 0;
            //}
            //if (CamraSupport.TaxExemptOccupancies.Contains(this.moccup))
            //{
            //	computedFactor = 0;
            //	orig_computedFactor = 0;
            //}

            //if (this.moccup == 16 || this.moccup == 26)
            //{
            //	computedFactor = 0;
            //	orig_computedFactor = 0;
            //}

            //if (macpct != 0 && macsf == 0)
            //{
            //	macsf = Convert.ToInt32(macpct * mtota);
            //}

            //if (macpct == 0 && macsf > 0)
            //{
            //	macpct = Convert.ToDecimal(macsf / mtota);
            //}

            //if (CamraSupport.ResidentialOccupancyCodes.Contains(this.moccup))
            //{
            //	if (mtbas != 0)
            //	{
            //		//orig_BasementArea = (mtbas / CamraSupport.BasementRate);
            //		orig_BasementArea = mbasa;
            //		if (mpbtot == 0)
            //		{
            //			orig_BasementPercentage = Math.Round((msbtot / mbasa), 2);
            //		}

            //		//if (mbasa == 0)
            //		//{
            //		//    orig_BasementPercentage = orig_BasementArea / orig_BasementArea;

            //	}
            //	else
            //	{
            //		orig_BasementArea = 0;
            //		orig_BasementPercentage = 0;
            //		BasementArea = 0;
            //		BasementPercentage = 0;
            //	}
            //}
            //else
            //{
            //	orig_BasementArea = 0;
            //	orig_BasementPercentage = 0;
            //	BasementArea = 0;
            //	BasementPercentage = 0;
            //}

            //if (mtfbas != 0 && orig_BasementArea != 0)
            //{
            //	orig_FinBasementArea = (mtfbas / mbrate);
            //	orig_FinBasementPercentage = orig_FinBasementArea / orig_BasementArea;
            //}

            //_isDirtyCheckingOn = true;
        }

#endregion

        public int auxA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.AuxAreaTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }
        int mdwell;
        public int carportA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.CarPortTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }

        public string Class
        {
            get
            {
                return _class;
            }
            set
            {
                if (!SketchUpCamraSupport.ClassCodeList().Contains(value)
                     && !"".Equals(value))
                {
                    throw new ArgumentException("Class not a valid value");
                }
                _class = value;
                CalculateParcel();

                FireChangedEvent("Class");
            }
        }

    

        internal CAMRA_Connection Connection
        {
            get
            {
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }

        public int deckA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.DeckTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }

     
        public int garA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.GarageTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }

    

        public int Moccup
        {
            get
            {
                return moccup;
            }
            set
            {
                moccup = value;
            }
        }

        public bool ParcelIsChanged
        {
            get
            {
                return AnyValueHasChanged();
            }
        }

        public int patioA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.PatioTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }

        public int porA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.PorchTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }

        public int sporA
        {
            get
            {
                var d = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections
                         where SketchUpCamraSupport.ScrnPorchTypes.Contains(s.SectionType)
                         select s.SqFt).Sum();

                return Convert.ToInt32(d);
            }
        }

        public int Mdwell
        {
            get
            {
                return mdwell;
            }

            set
            {
                mdwell = value;
            }
        }

        public decimal BaseChange;
        public int calcsubt;
        public int Card;

        // extra fields not in MAST Table
        public int mcarNc;
        public int mcarpt;
        public int mgarN2;
        public int mgarNc;
        public int mgart;
        public int mgart2;
        private int moccup;
        public int mrecno;
        public string mstory;
        public int orig_mcarNc;
        public int orig_mcarpt;
        public int orig_mdwell;
        public int orig_mgarN2;
        public int orig_mgarNc;
        public int orig_mgart;
        public int orig_mgart2;
        public int Record;
        public string updatestatus;
        public bool ValidRecord;
        private string _class = "";
        public CAMRA_Connection _conn;
        public decimal _finishedBsmtArea;
        
        private bool _isDirtyCheckingOn = false;
        private int orig_mrecno;
        private int orig_moccup;
        private string orig_mclass;
        private int orig_mbiNc;

        public event EventHandler<ParcelChangedEventArgs> ParcelChangedEvent;
    }
}
