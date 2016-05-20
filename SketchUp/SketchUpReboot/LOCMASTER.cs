using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUpReboot
{
    public partial class LOCMASTER
    {
        public LOCMASTER()
        {
        }

        public decimal JMDWELL
        {
            get
            {
                return _JMDWELL;
            }
            set
            {
                if ((_JMDWELL != value))
                {
                    _JMDWELL = value;
                }
            }
        }

        public string JMESKETCH
        {
            get
            {
                return _JMESKETCH;
            }
            set
            {
                if ((_JMESKETCH != value))
                {
                    _JMESKETCH = value;
                }
            }
        }

        public decimal JMRECORD
        {
            get
            {
                return _JMRECORD;
            }
            set
            {
                if ((_JMRECORD != value))
                {
                    _JMRECORD = value;
                }
            }
        }

        public decimal JMSCALE
        {
            get
            {
                return _JMSCALE;
            }
            set
            {
                if ((_JMSCALE != value))
                {
                    _JMSCALE = value;
                }
            }
        }

        public string JMSKETCH
        {
            get
            {
                return _JMSKETCH;
            }
            set
            {
                if ((_JMSKETCH != value))
                {
                    _JMSKETCH = value;
                }
            }
        }

        public decimal JMSTORY
        {
            get
            {
                return _JMSTORY;
            }
            set
            {
                if ((_JMSTORY != value))
                {
                    _JMSTORY = value;
                }
            }
        }

        public string JMSTORYEX
        {
            get
            {
                return _JMSTORYEX;
            }
            set
            {
                if ((_JMSTORYEX != value))
                {
                    _JMSTORYEX = value;
                }
            }
        }

        public decimal JMTOTSQFT
        {
            get
            {
                return _JMTOTSQFT;
            }
            set
            {
                if ((_JMTOTSQFT != value))
                {
                    _JMTOTSQFT = value;
                }
            }
        }

        private decimal _JMDWELL;
        private string _JMESKETCH;
        private decimal _JMRECORD;
        private decimal _JMSCALE;
        private string _JMSKETCH;
        private decimal _JMSTORY;
        private string _JMSTORYEX;
        private decimal _JMTOTSQFT;
    }
}
