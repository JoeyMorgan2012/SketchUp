    using SketchUp;
    using SWallTech;	
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Linq;
	using System.Linq;
	using System.Reflection;

namespace SketchUpReboot
{
    public partial class LandRecord
    {
        public LandRecord()
        {
        }

        public decimal LACCOD
        {
            get
            {
                return _LACCOD;
            }
            set
            {
                if ((_LACCOD != value))
                {
                    _LACCOD = value;
                }
            }
        }

     
        public string LACRE
        {
            get
            {
                return _LACRE;
            }
            set
            {
                if ((_LACRE != value))
                {
                    _LACRE = value;
                }
            }
        }

       
        public decimal LACREno
        {
            get
            {
                return _LACREno;
            }
            set
            {
                if ((_LACREno != value))
                {
                    _LACREno = value;
                }
            }
        }

      
        public decimal LADJ
        {
            get
            {
                return _LADJ;
            }
            set
            {
                if ((_LADJ != value))
                {
                    _LADJ = value;
                }
            }
        }

       
        public string LDESCR
        {
            get
            {
                return _LDESCR;
            }
            set
            {
                if ((_LDESCR != value))
                {
                    _LDESCR = value;
                }
            }
        }

       
        public decimal LDWELL
        {
            get
            {
                return _LDWELL;
            }
            set
            {
                if ((_LDWELL != value))
                {
                    _LDWELL = value;
                }
            }
        }

      
        public string LHS
        {
            get
            {
                return _LHS;
            }
            set
            {
                if ((_LHS != value))
                {
                    _LHS = value;
                }
            }
        }

        public string LID
        {
            get
            {
                return _LID;
            }
            set
            {
                if ((_LID != value))
                {
                    _LID = value;
                }
            }
        }

      
        public string LLP
        {
            get
            {
                return _LLP;
            }
            set
            {
                if ((_LLP != value))
                {
                    _LLP = value;
                }
            }
        }

       
        public decimal LRECNO
        {
            get
            {
                return _LRECNO;
            }
            set
            {
                if ((_LRECNO != value))
                {
                    _LRECNO = value;
                }
            }
        }

       
        public decimal LSEQNO
        {
            get
            {
                return _LSEQNO;
            }
            set
            {
                if ((_LSEQNO != value))
                {
                    _LSEQNO = value;
                }
            }
        }

      
        public decimal LSEWER
        {
            get
            {
                return _LSEWER;
            }
            set
            {
                if ((_LSEWER != value))
                {
                    _LSEWER = value;
                }
            }
        }

     
        public decimal LTOTAL
        {
            get
            {
                return _LTOTAL;
            }
            set
            {
                if ((_LTOTAL != value))
                {
                    _LTOTAL = value;
                }
            }
        }

     
        public decimal LUTIL
        {
            get
            {
                return _LUTIL;
            }
            set
            {
                if ((_LUTIL != value))
                {
                    _LUTIL = value;
                }
            }
        }

       
        public decimal LVALUE
        {
            get
            {
                return _LVALUE;
            }
            set
            {
                if ((_LVALUE != value))
                {
                    _LVALUE = value;
                }
            }
        }

      
        public decimal LWATER
        {
            get
            {
                return _LWATER;
            }
            set
            {
                if ((_LWATER != value))
                {
                    _LWATER = value;
                }
            }
        }

        private decimal _LACCOD;
        private string _LACRE;
        private decimal _LACREno;
        private decimal _LADJ;
        private string _LDESCR;
        private decimal _LDWELL;
        private string _LHS;
        private string _LID;
        private string _LLP;
        private decimal _LRECNO;
        private decimal _LSEQNO;
        private decimal _LSEWER;
        private decimal _LTOTAL;
        private decimal _LUTIL;
        private decimal _LVALUE;
        private decimal _LWATER;
    }
}
