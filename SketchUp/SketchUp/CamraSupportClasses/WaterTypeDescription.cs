using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
	public class WaterTypeDescription
	{
		public string PrintWaterDescription
		{
			get
			{
				return printWaterDescription;
			}

			set
			{
				printWaterDescription = value;
			}
		}

		public string WaterCode
		{
			get
			{
				return waterCode;
			}

			set
			{
				waterCode = value;
			}
		}

		public string WaterDescription
		{
			get
			{
				return waterDescription;
			}

			set
			{
				waterDescription = value;
			}
		}

		string printWaterDescription;

		string waterCode;

		string waterDescription;
	}

}
