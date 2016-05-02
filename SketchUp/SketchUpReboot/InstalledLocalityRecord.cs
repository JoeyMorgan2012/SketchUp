using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUpReboot
{
   public class InstalledLocalityRecord
    {
        string prefix;
        string description;
        string library;

        public string Prefix
        {
            get
            {
                return prefix;
            }

            set
            {
                prefix = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public string Library
        {
            get
            {
                return library;
            }

            set
            {
                library = value;
            }
        }
    }
}
