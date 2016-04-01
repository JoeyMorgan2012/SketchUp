using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
	public class SubDivisionCodesD
	{
		string _printSubDivDescription;

		public string PrintSubDivDescription
		{
			get
			{
				return _printSubDivDescription;
			}

			set
			{
				_printSubDivDescription = value;
			}
		}



		public string SubDivCode
		{
			get
			{
				return _subDivCode;
			}

			set
			{
				_subDivCode = value;
			}
		}

		string _subDivCode;
		string _subDivDescription;

		string _sudDivQuality;

	}
}
