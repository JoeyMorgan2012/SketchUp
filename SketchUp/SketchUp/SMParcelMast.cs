using System;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public class SMParcelMast
    {
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

        public CamraDataEnums.OccupancyType OccupancyType
        {
            get
            {
                occupancyType = SelectOccupancyType(OccupancyCode);
                return occupancyType;
            }

            set
            {
                occupancyType = value;
            }
        }

        private CamraDataEnums.OccupancyType SelectOccupancyType(int occupancyCode)
        {
            CamraDataEnums.OccupancyType occType=CamraDataEnums.OccupancyType.CodeNotFound;
            if (string.IsNullOrEmpty(OccupancyCode.ToString()))
            {
                occType = CamraDataEnums.OccupancyType.CodeNotFound;
            }
            else if (SketchUpCamraSupport.CommercialOccupancies.Contains(OccupancyCode))
            {
                occType = CamraDataEnums.OccupancyType.Commercial;
            }
            else if (SketchUpCamraSupport.ResidentialOccupancies.Contains(OccupancyCode))
            {
                occType = CamraDataEnums.OccupancyType.Residential;
            }
            else if (SketchUpCamraSupport.TaxExemptOccupancies.Contains(OccupancyCode))
            {
                occType = CamraDataEnums.OccupancyType.TaxExempt;
            }
            else
            {
                occType = CamraDataEnums.OccupancyType.Other;
            }
            return occType;
        }

        CamraDataEnums.OccupancyType occupancyType;
        private int card;
        private int carportNumCars;
        private int carportTypeCode;
        private int garage1NumCars;
        private int garage1TypeCode;
        private int garage2NumCars;
        private int garage2TypeCode;
        private decimal masterParcelStoreys;
        private int numCarsBuiltInCode;
        private int occupancyCode;
        private SMParcel parcel;
        private int propertyClass;
        private int record;
        
    }
}