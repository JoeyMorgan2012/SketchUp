using System;
using System.Collections.Generic;
using System.Text;

namespace SWallTech
{
	public class SketchSnapshotCollection
	{
		private List<SMParcel> sketchSnapshots;

		public List<SMParcel> SketchSnapshots
		{
			get
			{
				if (sketchSnapshots == null)
				{
					sketchSnapshots = new List<SMParcel>();
				}
				return sketchSnapshots;
			}

			set
			{
				sketchSnapshots = value;
			}
		}
	}
}