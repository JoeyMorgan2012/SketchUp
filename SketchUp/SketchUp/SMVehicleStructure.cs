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
            GarageTypesComboSource = GarageTypeListItems();
            CarportTypesComboSource = CarportListItems();
        }

        private List<ListOrComboBoxItem> CarportListItems()
        {
            throw new NotImplementedException();
        }

        private List<ListOrComboBoxItem> GarageTypeListItems()
        {
            throw new NotImplementedException();
        }

        #endregion "Constructor"

        #region "Properties"

        public int BiGarCode
        {
            get
            {
                return biGarCode;
            }

            set
            {
                biGarCode = value;
            }
        }

        public decimal CarportArea
        {
            get
            {
                return carportArea;
            }

            set
            {
                carportArea = value;
            }
        }

        public List<ListOrComboBoxItem> CarportTypesComboSource
        {
            get
            {
                if (carportTypes==null)
                {
                    carportTypes = CarportListItems();
                }
                return carportTypes;
            }

            set
            {
                carportTypes = value;
            }
        }

        public int Gar1cars
        {
            get
            {
                return gar1cars;
            }

            set
            {
                gar1cars = value;
            }
        }

        public int Gar1Code
        {
            get
            {
                return gar1Code;
            }

            set
            {
                gar1Code = value;
            }
        }

        public int Gar2cars
        {
            get
            {
                return gar2cars;
            }

            set
            {
                gar2cars = value;
            }
        }

        public int Gar2Code
        {
            get
            {
                return gar2Code;
            }

            set
            {
                gar2Code = value;
            }
        }

        public decimal Garage1Area
        {
            get
            {
                return garage1Area;
            }

            set
            {
                garage1Area = value;
            }
        }

        public decimal Garage2Area
        {
            get
            {
                return garage2Area;
            }

            set
            {
                garage2Area = value;
            }
        }

        public List<ListOrComboBoxItem> GarageTypesComboSource
        {
            get
            {
                if (garageTypes==null)
                {
                    garageTypes = GarageTypeListItems();
                }
                return garageTypes;
            }

            set
            {
                garageTypes = value;
            }
        }

        public SMParcel OriginalParcel
        {
            get
            {
                return originalParcel;
            }

            set
            {
                originalParcel = value;
            }
        }

        public SMParcel Parcel
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

        #endregion "Properties"

        #region "Private Fields"

        private int biGarCode;
        private decimal carportArea;
        private List<ListOrComboBoxItem> carportTypes;
        private int gar1cars;
        private int gar1Code;
        private int gar2cars;
        private int gar2Code;
        private decimal garage1Area;
        private decimal garage2Area;
        private List<ListOrComboBoxItem> garageTypes;
        private SMParcel originalParcel;
        private SMParcel parcel;

        #endregion "Private Fields"
    }
}