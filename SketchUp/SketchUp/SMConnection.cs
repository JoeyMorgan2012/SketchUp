using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SWallTech;
namespace SketchUp
{
    public class SMConnection 
    {
        #region constructors

        public SMConnection(string DataSourceIPAddress, string UserId, string UserPassword, string LocalityPrefix)
        {
            dataSource = DataSourceIPAddress;
            userName = UserId;
            password = UserPassword;
            locality = LocalityPrefix;
            library = DbConn.Library;
        }

        #endregion constructors

        private CAMRA_Connection ExistingDbConnection(CAMRA_Connection camraDbConnection)
        {
            if (!camraDbConnection.DBConnection.IsConnected)
            {
                camraDbConnection.DBConnection.connect();
            }

            return camraDbConnection;
        }

        private CAMRA_Connection NewConnectionFromProperties()
        {
            CAMRA_Connection dbConnection = null;
            bool ConnectionInformationComplete = !(string.IsNullOrEmpty(dataSource) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(locality));
            if (ConnectionInformationComplete)
            {
                dbConnection = new CAMRA_Connection
                {
                    DataSource = dataSource,

                    User = userName,
                    Password = password
                };
                if (!dbConnection.DBConnection.IsConnected)
                {
                    dbConnection.DBConnection.connect();
                }
            }
            return dbConnection;
        }

        public CAMRA_Connection DbConn
        {
            get
            {
                if (dbConn == null)
                {
                    dbConn = NewConnectionFromProperties();
                }
                else
                {
                    dbConn = ExistingDbConnection(dbConn);
                }
                dbConn.LocalityPrefix = Locality;
                return dbConn;
            }

            set
            {
                dbConn = value;
            }
        }

        [Required]
        public string IpAddress
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

        [Required]
        public string Library
        {
            get
            {
                if (DbConn != null && DbConn.DBConnection.IsConnected)
                {
                    DbConn.LocalityPrefix = Locality;
                    library = DbConn.Library.ToUpper();
                }
                return library;
            }
        }

        public string LineTable
        {
            get
            {
                lineTable = string.Format("{0}.{1}LINE", Library, Locality);
                return lineTable;
            }

            set
            {
                lineTable = value;
            }
        }

        [Required]
        public string Locality
        {
            get
            {
                return locality.ToUpper();
            }

            set
            {
                locality = value;
            }
        }

        public string MasterTable
        {
            get
            {
                masterTable = string.Format("{0}.{1}MASTER", Library, Locality);
                return masterTable;
            }

            set
            {
                masterTable = value;
            }
        }

        [Required]
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

        public string SectionTable
        {
            get
            {
                sectionTable = string.Format("{0}.{1}SECTION", Library, Locality);
                return sectionTable;
            }

            set
            {
                sectionTable = value;
            }
        }

        [Required]
        public string UserName
        {
            get
            {
                return userName.ToUpper();
            }

            set
            {
                userName = value;
            }
        }

        public string MastTable
        {
            get
            {
                mastTable =string.Format("{0}.{1}MAST", Library, Locality);
                return mastTable;
            }

            set
            {
                mastTable = value;
            }
        }

        private string dataSource = string.Empty;
        private CAMRA_Connection dbConn;

        private string library = string.Empty;
        private string lineTable = string.Empty;
        private string locality = string.Empty;
        private string masterTable = string.Empty;
        private string mastTable = string.Empty;
        private string password = string.Empty;
        private string sectionTable = string.Empty;
        private string userName = string.Empty;
    }
}