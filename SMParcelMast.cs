using System;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public class SMParcelMast
    {
        #region "Private methods"

        private CamraDataEnums.OccupancyType SelectOccupancyType(int occupancyCode)
        {
            CamraDataEnums.OccupancyType occType = CamraDataEnums.OccupancyType.CodeNotFound;
            if (string.IsNullOrEmpty(OccupancyCode.ToString()))
            {
                occType = CamraDataEnums.OccupancyType.CodeNotFound;
            }
            else if (SketchUpLookups.CommercialOccupancies.Contains(OccupancyCode))
            {
                occType = CamraDataEnums.OccupancyType.Commercial;
            }
            else if (SketchUpLookups.ResidentialOccupancies.Contains(OccupancyCode))
            {
                occType = CamraDataEnums.OccupancyType.Residential;
            }
            else if (CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.TaxExemptOccupancies)).Contains(OccupancyCode))
            {
                occType = CamraDataEnums.OccupancyType.TaxExempt;
            }
            else
            {
                occType = CamraDataEnums.OccupancyType.Other;
            }
            return occType;
        }

        #endregion "Private methods"

        #region "Properties"

        public int Card { get; set; }

        public int CarportNumCars { get; set; }

        public int CarportTypeCode { get; set; }

        public int Garage1NumCars { get; set; }

        public int Garage1TypeCode { get; set; }

        public int Garage2NumCars { get; set; }

        public int Garage2TypeCode { get; set; }

        public int NumCarsBuiltInCode { get; set; }

        public int OccupancyCode { get; set; }

        public CamraDataEnums.OccupancyType OccupancyType
        {
            get {
                occupancyType = SelectOccupancyType(OccupancyCode);
                return occupancyType;
            }

            set { occupancyType = value; }
        }

        public virtual SMParcel Parcel { get; set; }

        public string PropertyClass { get; set; }

        public int Record { get; set; }

        public string StoriesText { get; set; }

        public double StoriesValue { get; set; }

        public decimal TotalArea { get; set; }

        #endregion "Properties"

        #region "Private Fields"

        private CamraDataEnums.OccupancyType occupancyType;

        #endregion "Private Fields"
    }
}