using System;
using System.Text;
using SWallTech;

namespace SketchManager.Tests
{
	public class TestSetup
	{
		public SMParcel TestParcel(string dataSourceIP, string user, string pw, string loc, int record, int dwelling)
		{
			SketchRepository sr = new SketchRepository(dataSourceIP, user, password, locality);
			SMParcel parcel = sr.SelectParcelData(record, dwelling);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.IdentifyAttachedToSections();

			return parcel;
		}

		public SMParcel TestParcel()
		{
			SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
			SMParcel parcel = sr.SelectParcelData(Record, Dwelling);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.IdentifyAttachedToSections();

			return parcel;
		}

		public SMParcel TestParcel(string dataSourceIP, string user, string pw, string loc, int record, int dwelling, float containerSizeX, float containerSizeY)
		{
			SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
			SMParcel parcel = sr.SelectParcelData(record, dwelling);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.IdentifyAttachedToSections();

			return parcel;
		}

		public SMParcel TestParcelWithContainer(float containerSizeX, float containerSizeY)
		{
			SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
			SMParcel parcel = sr.SelectParcelData(Record, Dwelling);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.IdentifyAttachedToSections();

			return parcel;
		}

		public string DataSource
		{
			get
			{
				return dataSource;
			}

			set
			{
				dataSource = value;
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

		public string Locality
		{
			get
			{
				return locality;
			}

			set
			{
				locality = value;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}

			set
			{
				password = value;
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

		public string UserName
		{
			get
			{
				return userName;
			}

			set
			{
				userName = value;
			}
		}

		private string dataSource = "192.168.176.241";
		private int dwelling = 1;
		private string locality = "AUG";
		private string password = "CAMRA2";
		private int record = 11787;
		private string userName = "CAMRA2";
	}
}