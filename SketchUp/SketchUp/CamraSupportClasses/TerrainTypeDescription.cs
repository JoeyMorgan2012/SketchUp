using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
	public class TerrainTypeDescription
	{
		string _printTerrDescription;
		string _terrCode;
		string _terrDescription;

		public string PrintTerrDescription
		{
			get
			{
				return _printTerrDescription;
			}

			set
			{
				_printTerrDescription = value;
			}
		}

		public string TerrCode
		{
			get
			{
				return _terrCode;
			}

			set
			{
				_terrCode = value;
			}
		}

		public string TerrDescription
		{
			get
			{
				return _terrDescription;
			}

			set
			{
				_terrDescription = value;
			}
		}
	}

}
