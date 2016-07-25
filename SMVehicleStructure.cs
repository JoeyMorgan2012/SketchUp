using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public class SMVehicleStructure
    {
        #region "Constructor"

        public SMVehicleStructure(SMParcel workingParcel)
        {
            Parcel = workingParcel;
            OriginalParcel = SketchUpGlobals.SMParcelFromData;
            SetPropertiesFromParcelValues();
        }

        #endregion

        #region "Private methods"

        private List<ListOrComboBoxItem> CarportListItems()
        {
            ListOrComboBoxItem cbi;
            var carportTypes = new List<ListOrComboBoxItem>();
            foreach (StabType cp in SketchUpLookups.CarPortTypeCollection)
            {
                cbi = new ListOrComboBoxItem { Code = cp.Code, Description = cp.Description, PrintDescription = cp.ShortDescription };
                carportTypes.Add(cbi);
            }
            return carportTypes.OrderBy(c => c.Description).ToList();
        }

        private decimal carsByGarageArea(decimal garageArea)
        {
            decimal cars = 1.00M;

            return cars;
        }

        private List<ListOrComboBoxItem> GarageTypeListItems()
        {
            ListOrComboBoxItem cbi;
            var garageTypes = new List<ListOrComboBoxItem>();
            foreach (StabType g in SketchUpLookups.GarageTypeCollection)
            {
                cbi = new ListOrComboBoxItem { Code = g.Code, Description = g.Description, PrintDescription = g.ShortDescription };
                garageTypes.Add(cbi);
            }
            return garageTypes.OrderBy(c => c.Description).ToList();
        }

        private void SetPropertiesFromParcelValues()
        {
            GarageTypesComboSource = GarageTypeListItems();
            CarportTypesComboSource = CarportListItems();
            Gar1CodeDb = OriginalParcel.ParcelMast.Garage1TypeCode;
            Gar1carsDb = OriginalParcel.ParcelMast.Garage1NumCars;
            Garage1AreaDb = GarageArea(OriginalParcel, Gar1CodeDb);
            Gar2CodeDb = OriginalParcel.ParcelMast.Garage1TypeCode;
            Gar2carsDb = OriginalParcel.ParcelMast.Garage2NumCars;
            CarportCarsDb = OriginalParcel.ParcelMast.CarportNumCars;
            Gar1Code = Parcel.ParcelMast.Garage1TypeCode;
            Gar1cars = Parcel.ParcelMast.Garage1NumCars;
            Garage1Area = GarageArea(Parcel, Gar1Code);
            Gar2Code = Parcel.ParcelMast.Garage1TypeCode;
            Gar2cars = Parcel.ParcelMast.Garage2NumCars;
            CarportCars = Parcel.ParcelMast.CarportNumCars;
        }

        private decimal CarpArea(SMParcel parcel)
        {
            decimal cpArea = 0.00M;
            decimal.TryParse((from s in parcel.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s.SqFt).Sum().ToString(), out cpArea);
            return cpArea;
        }

        private decimal GarageArea(SMParcel parcel, int garCode)
        {
            decimal garArea = 0.00M;
            string codeToType = (from g in SketchUpLookups.GarageTypeCollection where g.Code == garCode.ToString() select g.Type).FirstOrDefault();
            List<SMSection> allGarages = (from s in parcel.Sections where SketchUpLookups.GarageTypes.Contains(s.SectionType) select s).ToList();
            if (allGarages != null)
            {
                if (allGarages.Count > 1)
                {


                }
                else if (allGarages.Count == 1)
                {
                    //Only one garage

                }
            }
            return garArea;
        }

        #endregion

        #region "Properties"

        public int BiGarCode
        {
            get { return biGarCode; }
            set { biGarCode = value; }
        }

        public decimal CarportArea
        {
            get { return carportArea; }
            set { carportArea = value; }
        }

        public decimal CarportAreaDb
        {
            get { return carportAreaDb; }
            set { carportAreaDb = value; }
        }

        public int CarportCars
        {
            get { return carportCars; }
            set { carportCars = value; }
        }

        public int CarportCarsDb
        {
            get { return carportCarsDb; }
            set { carportCarsDb = value; }
        }

        public int CarportTypeCode
        {
            get { return carportTypeCode; }
            set { carportTypeCode = value; }
        }

        public int CarportTypeCodeDb
        {
            get { return carportTypeCodeDb; }
            set { carportTypeCodeDb = value; }
        }

        public List<ListOrComboBoxItem> CarportTypesComboSource
        {
            get {
                if (carportTypes == null)
                {
                    carportTypes = CarportListItems();
                }
                return carportTypes;
            }
            set { carportTypes = value; }
        }

        public int Gar1cars
        {
            get { return gar1cars; }
            set { gar1cars = value; }
        }

        public int Gar1carsDb
        {
            get { return gar1carsDb; }
            set { gar1carsDb = value; }
        }

        public int Gar1Code
        {
            get { return gar1Code; }
            set { gar1Code = value; }
        }

        public int Gar1CodeDb
        {
            get { return gar1CodeDb; }
            set { gar1CodeDb = value; }
        }

        public int Gar2cars
        {
            get { return gar2cars; }
            set { gar2cars = value; }
        }

        public int Gar2carsDb
        {
            get { return gar2carsDb; }
            set { gar2carsDb = value; }
        }

        public int Gar2Code
        {
            get { return gar2Code; }
            set { gar2Code = value; }
        }

        public int Gar2CodeDb
        {
            get { return gar2CodeDb; }
            set { gar2CodeDb = value; }
        }

        public decimal Garage1Area
        {
            get { return garage1Area; }
            set { garage1Area = value; }
        }

        public decimal Garage1AreaDb
        {
            get { return garage1AreaDb; }
            set { garage1AreaDb = value; }
        }

        public decimal Garage2Area
        {
            get { return garage2Area; }
            set { garage2Area = value; }
        }

        public decimal Garage2AreaDb
        {
            get { return garage2AreaDb; }
            set { garage2AreaDb = value; }
        }

        public List<ListOrComboBoxItem> GarageTypesComboSource
        {
            get {
                if (garageTypes == null)
                {
                    garageTypes = GarageTypeListItems();
                }
                return garageTypes;
            }
            set { garageTypes = value; }
        }

        public SMParcel OriginalParcel
        {
            get { return originalParcel; }
            set { originalParcel = value; }
        }

        public SMParcel Parcel
        {
            get { return parcel; }
            set { parcel = value; }
        }

        #endregion

        #region "Private Fields"

        private int biGarCode;
        private decimal carportArea;
        private decimal carportAreaDb;
        private int carportCars;
        private int carportCarsDb;
        int carportTypeCode;
        int carportTypeCodeDb;
        private List<ListOrComboBoxItem> carportTypes;
        private int gar1cars;
        private int gar1carsDb;
        private int gar1Code;
        private int gar1CodeDb;
        private int gar2cars;
        private int gar2carsDb;
        private int gar2Code;
        private int gar2CodeDb;
        private decimal garage1Area;
        private decimal garage1AreaDb;
        private decimal garage2Area;
        private decimal garage2AreaDb;
        private List<ListOrComboBoxItem> garageTypes;
        private SMParcel originalParcel;
        private SMParcel parcel;

        #endregion
    }
}
