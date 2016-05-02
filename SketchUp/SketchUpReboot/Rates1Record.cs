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
    public partial class Rates1Record
    {
        public Rates1Record()
        {
        }

        public decimal RCLAR
        {
            get
            {
                return _RCLAR;
            }
            set
            {
                if ((_RCLAR != value))
                {
                    _RCLAR = value;
                }
            }
        }

   
        public decimal RCLBR
        {
            get
            {
                return _RCLBR;
            }
            set
            {
                if ((_RCLBR != value))
                {
                    _RCLBR = value;
                }
            }
        }

   
        public decimal RCLCR
        {
            get
            {
                return _RCLCR;
            }
            set
            {
                if ((_RCLCR != value))
                {
                    _RCLCR = value;
                }
            }
        }


        public decimal RCLDR
        {
            get
            {
                return _RCLDR;
            }
            set
            {
                if ((_RCLDR != value))
                {
                    _RCLDR = value;
                }
            }
        }

        public decimal RCLMR
        {
            get
            {
                return _RCLMR;
            }
            set
            {
                if ((_RCLMR != value))
                {
                    _RCLMR = value;
                }
            }
        }

  
        public string RDESC
        {
            get
            {
                return _RDESC;
            }
            set
            {
                if ((_RDESC != value))
                {
                    _RDESC = value;
                }
            }
        }

     
        public string RID
        {
            get
            {
                return _RID;
            }
            set
            {
                if ((_RID != value))
                {
                    _RID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RINCSF", DbType = "NVarChar(1)")]
        public string RINCSF
        {
            get
            {
                return _RINCSF;
            }
            set
            {
                if ((_RINCSF != value))
                {
                    _RINCSF = value;
                }
            }
        }

       
        public decimal RRPA
        {
            get
            {
                return _RRPA;
            }
            set
            {
                if ((_RRPA != value))
                {
                    _RRPA = value;
                }
            }
        }

      
        public decimal RRPAN
        {
            get
            {
                return _RRPAN;
            }
            set
            {
                if ((_RRPAN != value))
                {
                    _RRPAN = value;
                }
            }
        }

        public decimal RRPSF
        {
            get
            {
                return _RRPSF;
            }
            set
            {
                if ((_RRPSF != value))
                {
                    _RRPSF = value;
                }
            }
        }

      
        public string RSECTO
        {
            get
            {
                return _RSECTO;
            }
            set
            {
                if ((_RSECTO != value))
                {
                    _RSECTO = value;
                }
            }
        }

      
        public string RTELEM
        {
            get
            {
                return _RTELEM;
            }
            set
            {
                if ((_RTELEM != value))
                {
                    _RTELEM = value;
                }
            }
        }

 
        public string RTID
        {
            get
            {
                return _RTID;
            }
            set
            {
                if ((_RTID != value))
                {
                    _RTID = value;
                }
            }
        }

        private decimal _RCLAR;
        private decimal _RCLBR;
        private decimal _RCLCR;
        private decimal _RCLDR;
        private decimal _RCLMR;
        private string _RDESC;
        private string _RID;
        private string _RINCSF;
        private decimal _RRPA;
        private decimal _RRPAN;
        private decimal _RRPSF;
        private string _RSECTO;
        private string _RTELEM;
        private string _RTID;
    }
}
