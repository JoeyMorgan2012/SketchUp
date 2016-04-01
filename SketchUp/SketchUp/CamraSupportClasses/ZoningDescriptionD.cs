using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
	public class ZoningDescriptionD
	{
		string printZoneDescription;

		string zoneCode;

		string zoneDescription;

		public string PrintZoneDescription
		{
			get
			{
				return printZoneDescription;
			}

			set
			{
				printZoneDescription = value;
			}
		}

		public string ZoneCode
		{
			get
			{
				return zoneCode;
			}

			set
			{
				zoneCode = value;
			}
		}

		public string ZoneDescription
		{
			get
			{
				return zoneDescription;
			}

			set
			{
				zoneDescription = value;
			}
		}
	}
}
