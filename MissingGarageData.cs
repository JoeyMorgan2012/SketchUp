using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class MissingGarageData : Form
    {
        #region "Constructor"

        public MissingGarageData(SMParcelMast parcelMast)
        {
            InitializeComponent();
            InitializeForm(parcelMast);
        }

        public MissingGarageData(SMParcelMast parcelMast, decimal sectionArea, string sectionType)
        {
            InitializeComponent();
            InitializeForm(parcelMast);
        }

        #endregion



        #region "Private methods"



        private void CarPortNbrCarTxt_Leave(object sender, EventArgs e)
        {
            CarportNumCars = Convert.ToInt32(CarPortNbrCarTxt.Text.ToString());
        }



        private void CarPortTypCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CarPortTypCbox.Items.Count > 0)
            {
                if (ParcelMast != null && CarPortTypCbox.SelectedIndex > 0)
                {
                    int codeValue = 0;
                    string selectedValue = CarPortTypCbox.SelectedValue.ToString();
                    Int32.TryParse(selectedValue, out codeValue);
                    CarportCode = codeValue;

                    if (CarportCode != OriginalParcelMast.CarportTypeCode)
                    {
                        CarPortTypCbox.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        CarPortTypCbox.BackColor = Color.White;
                    }
                    ParcelMast.CarportTypeCode = CarportCode;
                }
            }
        }

        private void Gar1NbrCarTxt_Leave(object sender, EventArgs e)
        {
            Gar1NumCars = Convert.ToInt32(Gar1NbrCarTxt.Text.ToString());
        }
        private void Gar2NbrCarTxt_Leave(object sender, EventArgs e)
        {
            Gar2NumCars = Convert.ToInt32(Gar2NbrCarTxt.Text.ToString());
        }

        private void Gar2NbrCarTxt_TextChanged(object sender, EventArgs e)
        {
            Gar1NumCars = Convert.ToInt32(Gar1NbrCarTxt.Text.ToString());
        }

        private void Gar1TypeCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Gar1TypeCbox.Items.Count > 0)
            {
                if (ParcelMast != null && Gar1TypeCbox.SelectedIndex > 0)
                {
                    string selectedValue = Gar1TypeCbox.SelectedValue.ToString().Substring(0, 2);
                    int codeValue = 0;
                    Int32.TryParse(selectedValue, out codeValue);
                    Garage1Code = codeValue;

                    if (Garage1Code != OriginalParcelMast.Garage1TypeCode)
                    {
                        Gar1TypeCbox.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        Gar1TypeCbox.BackColor = Color.White;
                    }
                    ParcelMast.Garage1TypeCode = Garage1Code;
                }
            }
        }
        private void Gar2TypeCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Gar2TypeCbox.Items.Count > 0)
            {
                if (ParcelMast != null && Gar2TypeCbox.SelectedIndex > 0)
                {
                    string selectedValue = Gar2TypeCbox.SelectedValue.ToString().Substring(0, 2);
                    int codeValue = 0;
                    Int32.TryParse(selectedValue, out codeValue);
                    Garage1Code = codeValue;

                    if (Garage2Code != OriginalParcelMast.Garage2TypeCode)
                    {
                        Gar2TypeCbox.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        Gar2TypeCbox.BackColor = Color.White;
                    }
                    ParcelMast.Garage2TypeCode = Garage2Code;
                }
            }
        }

        private void InitializeForm(SMParcelMast parcelMast)
        {
            ParcelMast = parcelMast;
            OriginalParcelMast = SketchUpGlobals.SMParcelFromData.ParcelMast;
            Garage1Code = ParcelMast.Garage1TypeCode;
            Garage2Code = ParcelMast.Garage2TypeCode;
            Gar1NumCars = ParcelMast.Garage1NumCars;
            Gar2NumCars = ParcelMast.Garage2NumCars;
            CarportCode = ParcelMast.CarportTypeCode;
            CarportNumCars = ParcelMast.CarportNumCars;


            PopulateComboBoxes();
            ShowCurrentValues(ParcelMast);


            //SetControlVisibilityForSectionTypes(Type);

            //SetDefaultCarsForGarage(parcelMast, Type);

            //SetDefaultCarsForCarport(parcelMast, Type);

        }

        private void PopulateComboBoxes()
        {
            var garageData = SketchUpLookups.GarageTypeCollection.Select(garage => new ListOrComboBoxItem
            {
                Code = garage.Code,
                Description = garage.Description
            }).ToList();
            garageData.Add(new ListOrComboBoxItem
            {
                Code = "(None)",
                Description = "<Garage Type>"
            });
            Gar1TypeCbox.ValueMember = "Code";
            Gar1TypeCbox.DisplayMember = "Description";
            Gar1TypeCbox.DataSource = garageData.OrderBy(g => g.Description).ToList();
            Gar2TypeCbox.ValueMember = "Code";
            Gar2TypeCbox.DisplayMember = "Description";
            Gar2TypeCbox.DataSource = garageData.OrderBy(g => g.Description).ToList();
            var carports = SketchUpLookups.CarPortTypeCollection.Select(carport => new ListOrComboBoxItem
            {
                Code = carport.Code,
                Description = carport.Description
            }).ToList();
            carports.Add(new ListOrComboBoxItem { Code = "(None)", Description = "<Carport Type>" });

            CarPortTypCbox.ValueMember = "Code";
            CarPortTypCbox.DisplayMember = "Description";
            CarPortTypCbox.DataSource = carports.OrderBy(c => c.Description).ToList();
        }

        private void SetControlVisibilityForSectionTypes(string Type)
        {
            if (Type == "GAR")
            {
                CarPortTypCbox.Visible = false;
                CarPortNbrCarTxt.Visible = false;
                CarPortLbl.Visible = false;
                MissingCarporLbl.Visible = false;
                Gar1TypeCbox.Visible = true;
                Gar1NbrCarTxt.Visible = true;
                Garage1Label.Visible = true;
                MissingGarLbl.Visible = true;
            }

            if (Type == "CP")
            {
                CarPortTypCbox.Visible = true;
                CarPortNbrCarTxt.Visible = true;
                CarPortLbl.Visible = true;
                MissingCarporLbl.Visible = true;
                Gar1TypeCbox.Visible = false;
                Gar1NbrCarTxt.Visible = false;
                Garage1Label.Visible = false;
                MissingGarLbl.Visible = false;
            }
        }

        private void SetDefaultCarsForCarport(SMParcelMast parcelMast, string Type)
        {
            if (Type == "CP" && NewArea <= 275)
            {
                CarPortNbrCarTxt.Text = "1";
                parcelMast.CarportNumCars = 1;
                CarportNumCars = 1;
            }
            if (Type == "CP" && NewArea > 275)
            {
                CarPortNbrCarTxt.Text = "2";
                parcelMast.CarportNumCars = 2;
                CarportNumCars = 2;
            }
        }

        private void SetDefaultCarsForGarage(SMParcelMast parcelMast, string Type)
        {
            if (Type == "GAR" && NewArea <= 360)
            {
                Gar1NbrCarTxt.Text = "1";
                parcelMast.Garage1NumCars = 1;
                Gar1NumCars = 1;
            }
            if (Type == "GAR" && NewArea > 360)
            {
                Gar1NbrCarTxt.Text = "2";
                parcelMast.Garage1NumCars = 2;
                Gar1NumCars = 2;
            }
        }

        private void ShowCurrentValues(SMParcelMast parcelMast)
        {
            int gar1Index = 0;
            int gar2Index = 0;
            int carportIndex = 0;


            Garage1Code = parcelMast.Garage1TypeCode;
            Garage2Code = parcelMast.Garage1TypeCode;
            CarportCode = parcelMast.CarportTypeCode;
            gar1Index = Gar1TypeCbox.Items.IndexOf(Garage1Code.ToString());
            foreach (ListOrComboBoxItem item in Gar1TypeCbox.Items)
            {
                if (item.Code == Garage1Code.ToString())
                {
                    gar1Index = Gar1TypeCbox.Items.IndexOf(item);
                }
            }
            foreach (ListOrComboBoxItem item in Gar2TypeCbox.Items)
            {
                if (item.Code == Garage2Code.ToString())
                {
                    gar2Index = Gar2TypeCbox.Items.IndexOf(item);
                }
            }
            foreach (ListOrComboBoxItem item in CarPortTypCbox.Items)
            {
                if (item.Code == CarportCode.ToString())
                {
                    carportIndex = CarPortTypCbox.Items.IndexOf(item);
                }
            }

            Gar1TypeCbox.SelectedIndex = gar1Index > 0 ? gar1Index : 0;
            Gar2TypeCbox.SelectedIndex = gar2Index > 0 ? gar2Index : 0;
            CarPortTypCbox.SelectedIndex = carportIndex > 0 ? carportIndex : 0;
            Gar1NbrCarTxt.Text = ParcelMast.Garage1NumCars.ToString();
            Gar2NbrCarTxt.Text = ParcelMast.Garage2NumCars.ToString();
            CarPortNbrCarTxt.Text = ParcelMast.CarportNumCars.ToString();
        }

        #endregion

        #region "Properties"

        public int CarportCode { get; set; }

        public bool CarportDataOk
        {
            get { return carportDataOk; }
            set { carportDataOk = value; }
        }

        public int CarportNumCars { get; set; }

        public int Gar1NumCars { get; set; }

        public int Gar2NumCars { get; set; }

        public int Garage1Code { get; set; }

        public int Garage2Code { get; set; }

        public bool GarageDataOk
        {
            get { return garageDataOk; }
            set { garageDataOk = value; }
        }

        public SMParcelMast OriginalParcelMast
        {
            get { return originalParcelMast; }
            set { originalParcelMast = value; }
        }

        public SMParcelMast ParcelMast
        {
            get { return parcelMast; }
            set { parcelMast = value; }
        }

        #endregion

        #region "Private Fields"

        private bool carportDataOk;

        private bool garageDataOk;
        private decimal NewArea = 0;
        private SMParcelMast originalParcelMast;
        private SMParcelMast parcelMast;
        // TODO: Remove if not needed:	  private bool updatesComplete = false;


        #endregion

        private void btnDone_Click(object sender, EventArgs e)
        {
            UpdateParcelValuesFromForm();
        }

        private void UpdateParcelValuesFromForm()
        {
            ParcelMast.Garage1NumCars = Gar1NumCars;
            ParcelMast.Garage2NumCars = Gar2NumCars;
            ParcelMast.CarportNumCars = CarportNumCars;
            ParcelMast.Garage1TypeCode = Garage1Code;
            ParcelMast.Garage2TypeCode = Garage2Code;
            ParcelMast.CarportTypeCode = CarportCode;
        }
    }
}
