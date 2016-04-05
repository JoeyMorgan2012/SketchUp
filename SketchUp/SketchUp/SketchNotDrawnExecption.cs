using System;

namespace SketchUp
{
	public class SketchNotDrawnException : ApplicationException
	{
		public SketchNotDrawnException()
			: base("A CAMRA Sketch failed to draw successfully.")
		{
		}

		public SketchNotDrawnException(int record, int card, string sectionLetter, Exception innerException)
			: base(String.Format("Sketch for Record {0}, Card {1} failed to draw in section {2}.",
									record, card, sectionLetter),
					innerException)
		{
			Record = record;
			Card = card;
			FailedSection = sectionLetter;
		}

		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		public string FailedSection
		{
			get; set;
		}
	}
}