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
    public partial class GasLogRecord
    {

        private decimal _GRECNO;

        private decimal _GDWELL;

        private decimal _GNOGAS;

        public GasLogRecord()
        {
        }

        public decimal GRECNO
        {
            get
            {
                return _GRECNO;
            }
            set
            {
                if ((_GRECNO != value))
                {
                    _GRECNO = value;
                }
            }
        }

        public decimal GDWELL
        {
            get
            {
                return _GDWELL;
            }
            set
            {
                if ((_GDWELL != value))
                {
                    _GDWELL = value;
                }
            }
        }

        public decimal GNOGAS
        {
            get
            {
                return _GNOGAS;
            }
            set
            {
                if ((_GNOGAS != value))
                {
                    _GNOGAS = value;
                }
            }
        }
    }
}
