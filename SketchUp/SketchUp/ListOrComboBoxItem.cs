using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
   public class ListOrComboBoxItem
    {
        string code;
        string description;
        string printDescription;
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
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

        public string PrintDescription
        {
            get
            {
                return printDescription;
            }

            set
            {
                printDescription = value;
            }
        }
    }
}
