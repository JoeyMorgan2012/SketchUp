	using System.Data.Linq;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System;
    using SketchUp;
    using SWallTech;	
namespace SketchUpReboot
{
    public partial class InteriorImprovement
    {

        private string _UID;

        private decimal _URECNO;

        private decimal _UDWELL;

        private decimal _USEQNO;

        private string _UDESC;

        private decimal _UQTY;

        private decimal _UPRICE;

        private decimal _UTOTAL;

        public InteriorImprovement()
        {
        }


        public string UID
        {
            get
            {
                return _UID;
            }
            set
            {
                if ((_UID != value))
                {
                    _UID = value;
                }
            }
        }

        public decimal URECNO
        {
            get
            {
                return _URECNO;
            }
            set
            {
                if ((_URECNO != value))
                {
                    _URECNO = value;
                }
            }
        }

        public decimal UDWELL
        {
            get
            {
                return _UDWELL;
            }
            set
            {
                if ((_UDWELL != value))
                {
                    _UDWELL = value;
                }
            }
        }

        public decimal USEQNO
        {
            get
            {
                return _USEQNO;
            }
            set
            {
                if ((_USEQNO != value))
                {
                    _USEQNO = value;
                }
            }
        }

        public string UDESC
        {
            get
            {
                return _UDESC;
            }
            set
            {
                if ((_UDESC != value))
                {
                    _UDESC = value;
                }
            }
        }

        public decimal UQTY
        {
            get
            {
                return _UQTY;
            }
            set
            {
                if ((_UQTY != value))
                {
                    _UQTY = value;
                }
            }
        }

        public decimal UPRICE
        {
            get
            {
                return _UPRICE;
            }
            set
            {
                if ((_UPRICE != value))
                {
                    _UPRICE = value;
                }
            }
        }


        public decimal UTOTAL
        {
            get
            {
                return _UTOTAL;
            }
            set
            {
                if ((_UTOTAL != value))
                {
                    _UTOTAL = value;
                }
            }
        }
    }
}
