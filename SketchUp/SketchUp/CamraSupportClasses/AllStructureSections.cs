using System;
using System.Text;

namespace SketchUp
{
	public class AllStructureSections
	{
		private string _allSectionDescription;
		private string _allSectionType;
		private string _allSectPrintedDescription;

		public string AllSectionDescription
		{
			get
			{
				return _allSectionDescription;
			}

			set
			{
				_allSectionDescription = value;
			}
		}

		public string AllSectionType
		{
			get
			{
				return _allSectionType;
			}

			set
			{
				_allSectionType = value;
			}
		}

		public string AllSectPrintedDescription
		{
			get
			{
				return _allSectPrintedDescription;
			}

			set
			{
				_allSectPrintedDescription = value;
			}
		}
	}
}