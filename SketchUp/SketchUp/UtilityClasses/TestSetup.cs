using System;
using System.Text;
using SWallTech;
using SketchUp;
namespace SketchUp
{
    public class TestSetup
    {
        public SMParcel TestParcel(string dataSourceIP, string user, string pw, string loc, int record, int dwelling)
        {
            SketchRepository sr = new SketchRepository(dataSourceIP, user, password, locality);
            SMParcel parcel = sr.SelectParcelMasterWithParcel(record, dwelling).Parcel;
        
            return parcel;
        }

        public SMParcel TestParcel()
        {
#if DEBUG

            InitializeWithTestValues();
#endif
            SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
            SMParcel parcel = sr.SelectParcelMasterWithParcel(record, dwelling).Parcel;
            return parcel;
        }

        public  void InitializeWithTestValues()
        {
            //The application sets these, but for the PoC project we need to initialize the connection information manually. JMM 6-6-16
            SketchUpGlobals.IpAddress = "192.168.176.241";
            SketchUpGlobals.UserName = "CAMRA2";
            SketchUpGlobals.Password = "CAMRA2";
            SketchUpGlobals.LocalityPrefix = "AUG";
            SketchUpGlobals.Record = 11787;
            SketchUpGlobals.Card = 1;

            SMConnection conn = new SMConnection(SketchUpGlobals.IpAddress, SketchUpGlobals.UserName, SketchUpGlobals.Password, SketchUpGlobals.LocalityPrefix);
            SketchUpLookups.Init(conn.DbConn);
        }

        public SMParcel TestParcel(string dataSourceIP, string user, string pw, string loc, int record, int dwelling, float containerSizeX, float containerSizeY)
        {
            SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
            SMParcel parcel = sr.SelectParcelMasterWithParcel(record, dwelling).Parcel;
         
          

            return parcel;
        }
        public SMParcelMast TestParcelMast()
        {
            SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
            SMParcelMast mast= sr.SelectParcelMasterWithParcel(record, dwelling);
            return mast;

            
        }
        public SMParcel TestParcelWithContainer(float containerSizeX, float containerSizeY)
        {
            SketchRepository sr = new SketchRepository(DataSource, UserName, Password, Locality);
            SMParcel parcel = sr.SelectParcelMasterWithParcel(record, dwelling).Parcel;
      
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