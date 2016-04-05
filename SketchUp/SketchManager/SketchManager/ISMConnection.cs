namespace SWallTech
{
	public interface ISMConnection
	{
		CAMRA_Connection DbConn
		{
			get;
			set;
		}

		string IpAddress
		{
			get;
			set;
		}

		string Library
		{
			get;
		}

		string LineTable
		{
			get;
			set;
		}

		string Locality
		{
			get;
			set;
		}

		string MasterTable
		{
			get;
			set;
		}

		string Password
		{
			get;
			set;
		}

		string SectionTable
		{
			get;
			set;
		}

		string UserName
		{
			get;
			set;
		}
	}
}