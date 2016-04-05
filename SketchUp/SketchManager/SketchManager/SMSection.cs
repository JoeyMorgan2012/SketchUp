using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWallTech
{
	public class SMSection
	{
		#region Constructors

		public SMSection(SMParcel parcel)
		{
			record = parcel.Record;
			dwelling = parcel.Card;
		}

		#endregion Constructors

		#region class methods

		public SMLine SelectLineByNumber(int lineNum)
		{
			SMLine line = null;
			if (Lines != null)
			{
				line = (from l in Lines where l.LineNumber == lineNum select l).FirstOrDefault<SMLine>();
			}

			return line;
		}

		#endregion class methods

		#region Class Properties

		public decimal AdjFactor
		{
			get
			{
				return adjFactor;
			}

			set
			{
				adjFactor = value;
			}
		}

		public string AttachedTo
		{
			get
			{
				return attachedTo;
			}

			set
			{
				attachedTo = value;
			}
		}

		public decimal Depreciation
		{
			get
			{
				return depreciation;
			}

			set
			{
				depreciation = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}

			set
			{
				description = value;
			}
		}

		public int Dwelling
		{
			get
			{
				return dwelling;
			}

			set
			{
				dwelling = value;
			}
		}

		public string HasSketch
		{
			get
			{
				return hasSketch;
			}

			set
			{
				hasSketch = value;
			}
		}

		public List<SMLine> Lines
		{
			get
			{
				return lines;
			}

			set
			{
				lines = value;
			}
		}

		public int Record
		{
			get
			{
				return record;
			}

			set
			{
				record = value;
			}
		}

		public string SectionClass
		{
			get
			{
				return sectionClass;
			}

			set
			{
				sectionClass = value;
			}
		}

		public string SectionLetter
		{
			get
			{
				return sectionLetter;
			}

			set
			{
				sectionLetter = value;
			}
		}

		public string SectionType
		{
			get
			{
				return sectionType;
			}

			set
			{
				sectionType = value;
			}
		}

		public decimal SectionValue
		{
			get
			{
				return sectionValue;
			}

			set
			{
				sectionValue = value;
			}
		}

		public decimal SqFt
		{
			get
			{
				return sqFt;
			}

			set
			{
				sqFt = value;
			}
		}

		public decimal Storeys
		{
			get
			{
				return storeys;
			}

			set
			{
				storeys = value;
			}
		}

		public string ZeroDepr
		{
			get
			{
				return zeroDepr;
			}

			set
			{
				zeroDepr = value;
			}
		}

		public bool RefreshSection
		{
			get
			{
				return refreshSection;
			}

			set
			{
				refreshSection = value;
			}
		}

		#endregion Class Properties

		#region Virtual/navigation properties

		public virtual SMParcel ParentParcel
		{
			get;
			set;
		}

		#endregion Virtual/navigation properties

		#region Fields

		private decimal adjFactor;
		private string attachedTo;
		private SMLine anchorLine;
		private decimal depreciation;
		private string description;
		private int dwelling;
		private string hasSketch;
		private List<SMLine> lines;
		private int record;
		private bool refreshSection = true;
		private string sectionClass;
		private string sectionLetter;
		private string sectionType;
		private decimal sectionValue;
		private decimal sqFt;
		private decimal storeys;
		private string zeroDepr;

		#endregion Fields
	}
}