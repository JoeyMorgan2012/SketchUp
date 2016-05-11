using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
    public class SMParcelMast
    {
        public int CarportNumCars
        {
            get
            {
                return carportNumCars;
            }
            set
            {
                carportNumCars = value;
            }
        }

        public int CarportTypeCode
        {
            get
            {
                return carportTypeCode;
            }
            set
            {
                carportTypeCode = value;
            }
        }

        public int Card
        {
            get
            {
                return card;
            }
            set
            {
                card = value;
            }
        }

        public int Garage1NumCars
        {
            get
            {
                return garage1NumCars;
            }
            set
            {
                garage1NumCars = value;
            }
        }

        public int Garage1TypeCode
        {
            get
            {
                return garage1TypeCode;
            }
            set
            {
                garage1TypeCode = value;
            }
        }

        public int Garage2NumCars
        {
            get
            {
                return garage2NumCars;
            }
            set
            {
                garage2NumCars = value;
            }
        }

        public int Garage2TypeCode
        {
            get
            {
                return garage2TypeCode;
            }
            set
            {
                garage2TypeCode = value;
            }
        }

        public int NumCarsBuiltInCode
        {
            get
            {
                return numCarsBuiltInCode;
            }
            set
            {
                numCarsBuiltInCode = value;
            }
        }

        public int OccupancyCode
        {
            get
            {
                return occupancyCode;
            }
            set
            {
                occupancyCode = value;
            }
        }

        public virtual SMParcel Parcel
        {
            get
            {
                return parcel;
            }
            set
            {
                parcel = value;
            }
        }

        public int PropertyClass
        {
            get
            {
                return propertyClass;
            }
            set
            {
                propertyClass = value;
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

        public decimal MasterParcelStoreys
        {
            get
            {
                return masterParcelStoreys;
            }

            set
            {
                masterParcelStoreys = value;
            }
        }

        int carportNumCars;
        int carportTypeCode;
        int card;
        int garage1NumCars;
        int garage1TypeCode;
        int garage2NumCars;
        int garage2TypeCode;
        decimal masterParcelStoreys;
        int numCarsBuiltInCode;
        int occupancyCode;
        int propertyClass;
        int record;
        SMParcel parcel;
    }
}
