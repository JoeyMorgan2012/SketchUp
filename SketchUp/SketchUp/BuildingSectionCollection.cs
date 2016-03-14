using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SketchUp
{
	public class BuildingSectionCollection : List<BuildingSection>
	{
		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		private BuildingSectionCollection()
		{
		}

		public BuildingSectionCollection(int record, int card)
			: this()
		{
			Record = record;
			Card = card;
		}

		public decimal TotalDepreciationValue
		{
			get
			{
				var q = from t in this
						select t.DepreciationValue;

				return q.Sum();
			}
		}

		public decimal TotalFactorValue
		{
			get
			{
				var q = from t in this
						select t.FactorValue;

				return q.Sum();
			}
		}

		public static string GetNextSectionLetter(SWallTech.CAMRA_Connection conn, int record, int card)
		{
			string nextLetter = string.Empty;

			try
			{
				nextLetter = UtilityMethods.NextLetter(conn, record, card);
			}
			catch (System.Exception)
			{
				nextLetter = "A";
			}

			return nextLetter;
		}

		public BuildingSection GetBySectionLetter(string sectionLetter)
		{
			return this.FirstOrDefault(f => f.SectionLetter == sectionLetter);
		}
	}
}