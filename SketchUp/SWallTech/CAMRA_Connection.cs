using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWallTech
{
	public class CAMRA_Connection
	{
		private string _name = String.Empty;
		private string _ip = String.Empty;
		private string _user = String.Empty;
		private string _password = String.Empty;
		private string _localityPrefix = String.Empty;
		private string _library = String.Empty;
		private DBAccessManager _db = null;
		private InstalledLocalities _locs = null;

		public CAMRA_Connection()
		{
		}
		public CAMRA_Connection(string ipAddress, string userName, string password)
		{
			_name = userName;
			_ip = ipAddress;
			_password = password;
		}
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		public string DataSource
		{
			get
			{
				return _ip;
			}

			set
			{
				if (Regex.IsMatch(value, RegexPatterns.IPAddressRegexPattern) && !string.IsNullOrEmpty(value))
				{
					_ip = value;
				}
				else
				{
					Console.WriteLine(string.Format("{0}", value ?? string.Empty));
					_ip = string.Empty;
				}
			}
		}

		public string User
		{
			get
			{
				return _user;
			}

			set
			{
				_user = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}

			set
			{
				_password = value;
			}
		}

		public string LocalityPrefix
		{
			get
			{
				return _localityPrefix;
			}

			set
			{
				if (value.Length > 0 && value.Length != 3)
				{
					throw new FormatException("LocalityPrefix must be empty or 3 characters long.");
				}
				else
				{
					_localityPrefix = value;
				}
			}
		}

		public string Library
		{
			get
			{
				if (_locs == null || String.IsNullOrEmpty(LocalityPrefix))
				{
					ThrowAgumentException();
					return string.Empty;
					;
				}

				return _locs.GetLocalityLibrary(LocalityPrefix);
			}

			//set
			//{
			//    _library = value;
			//}
		}

		private void ThrowAgumentException()
		{
			throw new ArgumentException("Library could not be attained.");
		}

		public DBAccessManager DBConnection
		{
			get
			{
				if (_db == null)
				{
					OpenDbConnection();
				}

				return _db;
			}
		}

		public InstalledLocalities Localities
		{
			get
			{
				return _locs;
			}
		}

		public void OpenDbConnection()
		{
			try
			{
				if (_db == null)
				{
					_db = new DBAccessManager(DBAccessManager.DatabaseTypes.iSeriesDB2, _ip, null, _user, _password, null);
					if (_db.IsConnected)
					{
						_locs = new InstalledLocalities(_db);

						//Library = _locs.InstalledLibrary;
					}
				}
			}
			catch (Exception)
			{
				Console.WriteLine(string.Format("IP Address Provided was not correct{0}", _ip));
			}
		}

		public CAMRA_Connection Clone()
		{
			CAMRA_Connection conn = new CAMRA_Connection();
			conn._ip = _ip;
			conn._library = _library;
			conn._localityPrefix = _localityPrefix;
			conn._name = _name;
			conn._password = _password;
			conn._user = _user;
			return conn;
		}
	}
}