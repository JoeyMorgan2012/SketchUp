using System;
using System.Text;

namespace SketchUp
{
	public class ParcelChangedEventArgs : EventArgs
	{
		public ParcelChangedEventArgs()
			: base()
		{
		}

		public string PropertyName;
	}
}