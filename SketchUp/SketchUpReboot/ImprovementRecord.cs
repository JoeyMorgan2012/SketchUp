using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUpReboot
{
    public partial class ImprovementRecord
    {

        private string _IID;

        private decimal _IRECNO;

        private decimal _IDWELL;

        private decimal _ISEQNO;

        private string _IDESC;

        private decimal _ILEN;

        private decimal _IWID;

        private string _ICOND;

        private string _IFILL1;

        private decimal _ITOTV;

        private decimal _IDEPR;

        private decimal _IRATE;

        private string _IFILL2;

        private decimal _ICODE;

        private string _IFILL3;

        public ImprovementRecord()
        {
        }

        public string IID
        {
            get
            {
                return _IID;
            }
            set
            {
                if ((_IID != value))
                {
                    _IID = value;
                }
            }
        }

        public decimal IRECNO
        {
            get
            {
                return _IRECNO;
            }
            set
            {
                if ((_IRECNO != value))
                {
                    _IRECNO = value;
                }
            }
        }

        public decimal IDWELL
        {
            get
            {
                return _IDWELL;
            }
            set
            {
                if ((_IDWELL != value))
                {
                    _IDWELL = value;
                }
            }
        }

        public decimal ISEQNO
        {
            get
            {
                return _ISEQNO;
            }
            set
            {
                if ((_ISEQNO != value))
                {
                    _ISEQNO = value;
                }
            }
        }

        public string IDESC
        {
            get
            {
                return _IDESC;
            }
            set
            {
                if ((_IDESC != value))
                {
                    _IDESC = value;
                }
            }
        }

        public decimal ILEN
        {
            get
            {
                return _ILEN;
            }
            set
            {
                if ((_ILEN != value))
                {
                    _ILEN = value;
                }
            }
        }

        public decimal IWID
        {
            get
            {
                return _IWID;
            }
            set
            {
                if ((_IWID != value))
                {
                    _IWID = value;
                }
            }
        }

        public string ICOND
        {
            get
            {
                return _ICOND;
            }
            set
            {
                if ((_ICOND != value))
                {
                    _ICOND = value;
                }
            }
        }

        public string IFILL1
        {
            get
            {
                return _IFILL1;
            }
            set
            {
                if ((_IFILL1 != value))
                {
                    _IFILL1 = value;
                }
            }
        }

        public decimal ITOTV
        {
            get
            {
                return _ITOTV;
            }
            set
            {
                if ((_ITOTV != value))
                {
                    _ITOTV = value;
                }
            }
        }

        public decimal IDEPR
        {
            get
            {
                return _IDEPR;
            }
            set
            {
                if ((_IDEPR != value))
                {
                    _IDEPR = value;
                }
            }
        }

        public decimal IRATE
        {
            get
            {
                return _IRATE;
            }
            set
            {
                if ((_IRATE != value))
                {
                    _IRATE = value;
                }
            }
        }

        public string IFILL2
        {
            get
            {
                return _IFILL2;
            }
            set
            {
                if ((_IFILL2 != value))
                {
                    _IFILL2 = value;
                }
            }
        }

        public decimal ICODE
        {
            get
            {
                return _ICODE;
            }
            set
            {
                if ((_ICODE != value))
                {
                    _ICODE = value;
                }
            }
        }

        public string IFILL3
        {
            get
            {
                return _IFILL3;
            }
            set
            {
                if ((_IFILL3 != value))
                {
                    _IFILL3 = value;
                }
            }
        }
    }
}
