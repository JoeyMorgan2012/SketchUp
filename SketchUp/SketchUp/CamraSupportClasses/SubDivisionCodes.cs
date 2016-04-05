using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
	public class SubDivisionCodes
	{
		string printDescription;

		string subDivCode;
		string subDivDescription;

		string subDivQuality;

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

		public string SubDivCode
		{
			get
			{
				return subDivCode;
			}

			set
			{
				subDivCode = value;
			}
		}

		public string SubDivDescription
		{
			get
			{
				return subDivDescription;
			}

			set
			{
				subDivDescription = value;
			}
		}

		public string SubDivQuality
		{
			get
			{
				return subDivQuality;
			}

			set
			{
				subDivQuality = value;
			}
		}
	}

}
