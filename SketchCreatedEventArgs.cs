using System;
using System.Drawing;

namespace SketchUp
{
	public class SketchCreatedEventArgs : EventArgs
	{
		private Bitmap _sketch;
		private int _record;
		private int _card;

		public SketchCreatedEventArgs(Bitmap sketch, int record, int card)
		{
			_sketch = sketch;
			_record = record;
			_card = card;
		}

		public Bitmap Sketch
		{
			get
			{
				return _sketch;
			}
		}

		public int Record
		{
			get
			{
				return _record;
			}
		}

		public int Card
		{
			get
			{
				return _card;
			}
		}
	}
}