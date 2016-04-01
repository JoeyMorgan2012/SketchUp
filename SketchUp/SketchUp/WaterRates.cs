
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
	public class WaterRates
	{

		public int WaterCode
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

		public int WaterRate
		{
			get
			{
				return waterRate;
			}

			set
			{
				waterRate = value;
			}
		}

		int waterCode;


		string waterDescription;


		int waterRate;
	}
}
