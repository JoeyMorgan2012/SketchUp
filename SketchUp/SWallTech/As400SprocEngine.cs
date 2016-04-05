using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using IBM.Data.DB2.iSeries;

namespace SWallTech
{
	public class As400SprocEngine : PropertyChangedBase, IDisposable
	{
		private const string ConnectionStringFormat = "DataSource={0};UserID={1};Password={2};";

		private iDB2Command _cmd = null;
		private iDB2Connection _conn = null;
		private string _ip;
		private string _pass;

		private Dictionary<As400DbType, iDB2DbType> _typeConverter =
			new Dictionary<As400DbType, iDB2DbType>()
				{
					{As400DbType.Numeric, iDB2DbType.iDB2Numeric},
					{As400DbType.Character, iDB2DbType.iDB2Char}
				};

		private string _user;

		#region Constructors

		public As400SprocEngine()
		{
		}

		public As400SprocEngine(string ip, string user, string password)
			: this()
		{
			IpAddress = ip;
			User = user;
			Password = password;

			ConnectToDatabase();
		}

		#endregion Constructors

		#region Enums

		public enum As400DbType
		{
			Numeric,
			Character
		}

		#endregion Enums

		#region Class Properties

		public string IpAddress
		{
			get
			{
				return _ip.Trim();
			}

			set
			{
				_ip = value;
				FirePropertyChangedEvent("IpAddress");
			}
		}

		public bool IsConnected
		{
			get
			{
				return _conn != null && _conn.State == ConnectionState.Open;
			}
		}

		public List<string> ParmTypes
		{
			get
			{
				var list = new List<string>();
				list.AddRange(Enum.GetNames(typeof(iDB2DbType)));
				return list;
			}
		}

		public string Password
		{
			get
			{
				return _pass.ToUpper().Trim();
			}

			set
			{
				_pass = value;
				FirePropertyChangedEvent("Password");
			}
		}

		public string QualifiedProcedureName
		{
			get
			{
				return _cmd.CommandText;
			}

			set
			{
				_cmd.CommandText = value;
				FirePropertyChangedEvent("QualifiedProcedureName");
			}
		}

		public string User
		{
			get
			{
				return _user.ToUpper().Trim();
			}

			set
			{
				_user = value;
				FirePropertyChangedEvent("User");
			}
		}

		#endregion Class Properties

		#region Class Methods

		public void AddParameter(string name, As400DbType type, object value)
		{
			AddParameterIDB2(name, _typeConverter[type], null, value, ParameterDirection.Input);
		}

		public void AddParameter(string name, As400DbType type, int? size, object value, ParameterDirection direction)
		{
			AddParameterIDB2(name, _typeConverter[type], size, value, direction);
		}

		public void AddParameterIDB2(string name, iDB2DbType type, object value)
		{
			AddParameterIDB2(name, type, null, value, ParameterDirection.Input);
		}

		public void AddParameterIDB2(string name, iDB2DbType type, int? size, object value, ParameterDirection direction)
		{
			if (size.HasValue)
			{
				_cmd.Parameters.Add(name, type, size.Value);
			}
			else
			{
				_cmd.Parameters.Add(name, type);
			}
			_cmd.Parameters[name].Value = value;
			_cmd.Parameters[name].Direction = direction;
		}

		public void ClearParameters()
		{
			_cmd.Parameters.Clear();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
		public void ConnectToDatabase()
		{
			if (IpAddress.Equals("") || User.Equals("") || Password.Equals(""))
				throw new ArgumentException("Required field is blank.");

			try
			{
				var connectionstring = String.Format(ConnectionStringFormat,
					  IpAddress, User, Password);
				_conn = new iDB2Connection(connectionstring);
				_conn.Open();

				FirePropertyChangedEvent("IsConnected");

				if (IsConnected)
				{
					_cmd = _conn.CreateCommand();
					_cmd.CommandTimeout = 0;
					_cmd.CommandType = CommandType.StoredProcedure;
				}
			}
			catch (IBM.Data.DB2.iSeries.iDB2ConnectionTimeoutException ex)
			{
				string methodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;

				Console.WriteLine(string.Format("{0} fired error {1}: {2}", methodName, ex.InnerException.GetType().ToString(), ex.Message));
			}
			catch (IBM.Data.DB2.iSeries.iDB2ConnectionFailedException cfe)
			{
				string methodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;

				string errorMessage = string.Format("{0} fired error {1}: {2}", methodName, cfe.InnerException.GetType().ToString(), cfe.MessageDetails);
				throw new Exception(errorMessage);
			}
			catch (Exception ex)
			{
				string methodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
				string errorMessage = string.Format("{0} with connection string {3} fired {1} error:  {2}", methodName, ex.InnerException.GetType().Name, ex.Message, _conn.ConnectionString);
				throw new Exception(errorMessage);
			}
		}

		public Dictionary<string, object> ExecuteSproc()
		{
			try
			{
				_cmd.Prepare();
				var q = _cmd.ExecuteNonQuery();

				//if (_cmd.Parameters.Contains(_returnParameterName))
				//{
				//    return _cmd.Parameters[_returnParameterName].Value;
				//}
				//return null;

				var dict = new Dictionary<string, object>();
				foreach (iDB2Parameter parm in _cmd.Parameters)
				{
					if (parm.Direction == ParameterDirection.Input)
						continue;

					dict.Add(parm.ParameterName, parm.Value);
				}

				if (dict.Count <= 0)
					dict.Add("AffectedRowCount", q);

				return dict;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public DataSet ExecuteSprocCollection()
		{
			try
			{
				_cmd.Prepare();

				var adapter = new iDB2DataAdapter(_cmd);
				var ds = new DataSet();
				ds.Locale = CultureInfo.CurrentUICulture;
				adapter.Fill(ds);

				return ds;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void SetParameterValue(string name, object value)
		{
			if (!_cmd.Parameters.Contains(name))
				throw new ArgumentOutOfRangeException(String.Format("Parameter {0} does not exist.", name));

			_cmd.Parameters[name].Value = value;
		}

		#endregion Class Methods

		#region IDisposable Members

		public void Dispose()
		{
			if (_cmd != null)
			{
				_cmd.Dispose();
			}
			if (_conn != null)
			{
				_conn.Close();
				_conn.Dispose();
			}
		}

		#endregion IDisposable Members
	}
}