using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWallTech;

namespace SketchUp
{
	public static class Rates
	{
		public static SortedDictionary<string, decimal> ClassValues;
		public static decimal BasementRate;
		public static decimal FinBasementDefaultRate;
		public static decimal PlumbingRate;
	
		public static int ExtraKitRate;
		public static decimal PierRate;
		public static decimal SlabRate;
		public static decimal AirCondRate;
		public static decimal MaxACValue;
		public static Rat1Master Rate1Master;
		public static decimal OutBldRateLim;

		public static decimal NoHeatRate;
		public static decimal FlrHeatRate;

		public static decimal MinMHValue;
		public static decimal FirePlaceRate;
		public static decimal FirePlaceStackRate;
		public static decimal FlueRate;
		public static decimal FlueIncRate;
		public static decimal MetalFlueRate;
		public static decimal GasLogRate;
	}
}
