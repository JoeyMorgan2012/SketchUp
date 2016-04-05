using System;
using System.Text;

namespace SketchUp
{
	public class BuildingLine
	{
		private int _record;

		public int Record
		{
			get
			{
				return _record;
			}

			set
			{
				_record = value;
			}
		}

		private int _card;

		public int Card
		{
			get
			{
				return _card;
			}

			set
			{
				_card = value;
			}
		}

		private string _sectionLetter;

		public string SectionLetter
		{
			get
			{
				return _sectionLetter;
			}

			set
			{
				_sectionLetter = value;
			}
		}

		//JLLINE, JLPT1X, JLPT1Y, JLPT2X, JLPT2Y, JLDIRECT, JLXLEN, JLYLEN, JLATTACH
		private int _lineNumber;

		private decimal _point1X;
		private decimal _point1Y;
		private decimal _point2X;
		private decimal _point2Y;
		private decimal _xLength;
		private decimal _yLength;
		private string _directional;
		private string _attachment;

		public int LineNumber
		{
			get
			{
				return _lineNumber;
			}

			set
			{
				_lineNumber = value;
			}
		}

		public decimal Point1X
		{
			get
			{
				return _point1X;
			}

			set
			{
				_point1X = value;
			}
		}

		public decimal Point1Y
		{
			get
			{
				return _point1Y;
			}

			set
			{
				_point1Y = value;
			}
		}

		public decimal Point2X
		{
			get
			{
				return _point2X;
			}

			set
			{
				_point2X = value;
			}
		}

		public decimal Point2Y
		{
			get
			{
				return _point2Y;
			}

			set
			{
				_point2Y = value;
			}
		}

		public decimal XLength
		{
			get
			{
				return _xLength;
			}

			set
			{
				_xLength = value;
			}
		}

		public decimal YLength
		{
			get
			{
				return _yLength;
			}

			set
			{
				_yLength = value;
			}
		}

		public string Directional
		{
			get
			{
				return _directional;
			}

			set
			{
				_directional = value;
			}
		}

		public string Attachment
		{
			get
			{
				return _attachment;
			}

			set
			{
				_attachment = value;
			}
		}

		public string StatusFlag
		{
			get; set;
		}

		public string LineLengthString
		{
			get
			{
				switch (Directional)
				{
					case "N":
					case "S":
						return YLength.ToString();

					case "E":
					case "W":
						return XLength.ToString();

					case "NE":
					case "NW":
					case "SE":
					case "SW":
						return String.Format("{0},{1}", YLength, XLength);
				}

				return String.Empty;
			}
		}

		public Exception lastException
		{
			get; set;
		}

		public int IncrementLineNumber()
		{
			this.lastException = null;
			int rowCount = -1;
			try
			{
				if ("".Equals(this.StatusFlag.Trim()))
				{
					this.StatusFlag = "U";
				}
			}
			catch (System.Exception ex)
			{
				this.lastException = ex;
			}
			return rowCount;
		}

		/* These are just placeholders, but removing them breaks the class. JMM 02/12/2016 */

		public bool Update()
		{
			return true;
		}

		public bool Insert()
		{
			return true;
		}

		public bool Delete()
		{
			return true;
		}
	}
}