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
    public partial class SystemTableRecord
    {
        public SystemTableRecord()
        {
        }

      
        public string TDESC
        {
            get
            {
                return _TDESC;
            }
            set
            {
                if ((_TDESC != value))
                {
                    _TDESC = value;
                }
            }
        }


        public string TDESCP
        {
            get
            {
                return _TDESCP;
            }
            set
            {
                if ((_TDESCP != value))
                {
                    _TDESCP = value;
                }
            }
        }

        public string TEXWAL
        {
            get
            {
                return _TEXWAL;
            }
            set
            {
                if ((_TEXWAL != value))
                {
                    _TEXWAL = value;
                }
            }
        }

    
        public string TFILL
        {
            get
            {
                return _TFILL;
            }
            set
            {
                if ((_TFILL != value))
                {
                    _TFILL = value;
                }
            }
        }

  
        public string TID
        {
            get
            {
                return _TID;
            }
            set
            {
                if ((_TID != value))
                {
                    _TID = value;
                }
            }
        }

  
        public string TLOC
        {
            get
            {
                return _TLOC;
            }
            set
            {
                if ((_TLOC != value))
                {
                    _TLOC = value;
                }
            }
        }

    
        public string TPSWD
        {
            get
            {
                return _TPSWD;
            }
            set
            {
                if ((_TPSWD != value))
                {
                    _TPSWD = value;
                }
            }
        }


        public string TTELEM
        {
            get
            {
                return _TTELEM;
            }
            set
            {
                if ((_TTELEM != value))
                {
                    _TTELEM = value;
                }
            }
        }

    
        public string TTID
        {
            get
            {
                return _TTID;
            }
            set
            {
                if ((_TTID != value))
                {
                    _TTID = value;
                }
            }
        }

        private string _TDESC;
        private string _TDESCP;
        private string _TEXWAL;
        private string _TFILL;
        private string _TID;
        private string _TLOC;
        private string _TPSWD;
        private string _TTELEM;
        private string _TTID;
    }
}
