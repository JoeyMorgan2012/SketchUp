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
    public partial class SystemInfoRecord
    {
        public SystemInfoRecord()
        {
        }

     
        public string SYSTYPE
        {
            get
            {
                return _SYSTYPE;
            }
            set
            {
                if ((_SYSTYPE != value))
                {
                    _SYSTYPE = value;
                }
            }
        }

      
        public string SYSVALUE
        {
            get
            {
                return _SYSVALUE;
            }
            set
            {
                if ((_SYSVALUE != value))
                {
                    _SYSVALUE = value;
                }
            }
        }

        private string _SYSTYPE;
        private string _SYSVALUE;
    }
}
