﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class MissingGarageData : Form
    {
        public MissingGarageData(SMParcelMast parcelMast, decimal newSectArea, string Type)
        {
            GarageCode = 0;
            GarNbr = 0;
            CPCode = 0;
            CpNbr = 0;

            InitializeComponent();
            originalParcelMast = SketchUpGlobals.SMParcelFromData.ParcelMast;
            Build_CarPort_Data();
            Build_Garage_Data();

            GarNbrCarTxt.Text = "0";
            CarPortNbrCarTxt.Text = "0";

            NewArea = newSectArea;

            SetControlVisibilityForSectionTypes(Type);

            SetDefaultCarsForGarage(parcelMast, Type);

            SetDefaultCarsForCarport(parcelMast, Type);

            PopulateComboBoxes();
        }

        public void ListCPortSelection()
        {
            string _CarPortTypeCodeDesc = String.Empty;
            int _cboxIndex = 0;

            _cportList = new Dictionary<int, StabType>();
            var index = -1;
            if (CarPortTypCbox.SelectedIndex <= 0)
            {
                CarPortTypCbox.Items.Add("< Car Port >");
                _cportList.Add(++index, null);

                foreach (var item in SketchUpLookups.CarPortTypeCollection)
                {
                    _cportList.Add(++index, item);
                    CarPortTypCbox.Items.Add(item._printedDescription);
                }
            }
            CarPortTypCbox.SelectedIndex = _cboxIndex;
        }

        public void ListGarageSelection()
        {
            string _garTypeCodeDesc = String.Empty;
            int _cboxIndex = 0;

            _garList = new Dictionary<int, StabType>();
            var index = -1;
            if (GarTypeCbox.SelectedIndex <= 0)
            {
                GarTypeCbox.Items.Add("< Garages >");
                _garList.Add(++index, null);

                foreach (var item in SketchUpLookups.GarageTypeCollection)
                {
                    _garList.Add(++index, item);
                    GarTypeCbox.Items.Add(item._printedDescription);
                }
            }
            GarTypeCbox.SelectedIndex = _cboxIndex;
        }

        private void Build_CarPort_Data()
        {
            if (CarPortTypCbox.Items.Count > 0)
            {
                CarPortTypCbox.SelectedIndex = 0;
                for (int i = 1; i < CarPortTypCbox.Items.Count; i++)
                {
                    string listCarPortType = CarPortTypCbox.Items[i].ToString().Substring(0, 2);
                    if (parcelMast.CarportTypeCode.ToString().Trim() == listCarPortType)
                    {
                        CarPortTypCbox.SelectedIndex = i;
                    }
                }
            }
        }

        private void Build_Garage_Data()
        {
            if (GarTypeCbox.Items.Count > 0)
            {
                GarTypeCbox.SelectedIndex = 0;
                for (int i = 1; i < GarTypeCbox.Items.Count; i++)
                {
                    string listGarageType = GarTypeCbox.Items[i].ToString().Substring(0, 2);
                    if (parcelMast.Garage1TypeCode.ToString().Trim() == listGarageType)
                    {
                        GarTypeCbox.SelectedIndex = i;
                    }
                }
            }
        }

        private void CarPortNbrCarTxt_Leave(object sender, EventArgs e)
        {
            CpNbr = Convert.ToInt32(CarPortNbrCarTxt.Text.ToString());
        }

        private void CarPortNbrCarTxt_TextChanged(object sender, EventArgs e)
        {
            CpNbr = Convert.ToInt32(CarPortNbrCarTxt.Text.ToString());
        }

        private void CarPortTypCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CarPortTypCbox.Items.Count > 0)
            {
                if (parcelMast != null && CarPortTypCbox.SelectedIndex > 0)
                {
                    int codeValue = 0;
                    string selectedValue = CarPortTypCbox.SelectedItem.ToString().Substring(0, 2);
                    Int32.TryParse(selectedValue, out codeValue);
                    CPCode = codeValue;

                    if (CPCode != originalParcelMast.CarportTypeCode)
                    {
                        CarPortTypCbox.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        CarPortTypCbox.BackColor = Color.White;
                    }
                    parcelMast.CarportTypeCode = CPCode;
                }
            }
        }

        private void GarNbrCarTxt_Leave(object sender, EventArgs e)
        {
            GarNbr = Convert.ToInt32(GarNbrCarTxt.Text.ToString());
        }

        private void GarNbrCarTxt_TextChanged(object sender, EventArgs e)
        {
            GarNbr = Convert.ToInt32(GarNbrCarTxt.Text.ToString());
        }

        private void GarTypeCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarTypeCbox.Items.Count > 0)
            {
                if (parcelMast != null && GarTypeCbox.SelectedIndex > 0)
                {
                    string selectedValue = GarTypeCbox.SelectedItem.ToString().Substring(0, 2);
                    int codeValue = 0;
                    Int32.TryParse(selectedValue, out codeValue);
                    GarageCode = codeValue;

                    if (GarageCode != originalParcelMast.Garage1TypeCode)
                    {
                        GarTypeCbox.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        GarTypeCbox.BackColor = Color.White;
                    }
                    parcelMast.Garage1TypeCode = GarageCode;
                }
            }
        }

        private void PopulateComboBoxes()
        {
            GarTypeCbox.Items.Clear();
            ListGarageSelection();
            CarPortTypCbox.Items.Clear();
            ListCPortSelection();
        }

        private void SetControlVisibilityForSectionTypes(string Type)
        {
            if (Type == "GAR")
            {
                CarPortTypCbox.Visible = false;
                CarPortNbrCarTxt.Visible = false;
                CarPortLbl.Visible = false;
                MissingCarporLbl.Visible = false;
                GarTypeCbox.Visible = true;
                GarNbrCarTxt.Visible = true;
                GarLbl.Visible = true;
                MissingGarLbl.Visible = true;
            }

            if (Type == "CP")
            {
                CarPortTypCbox.Visible = true;
                CarPortNbrCarTxt.Visible = true;
                CarPortLbl.Visible = true;
                MissingCarporLbl.Visible = true;
                GarTypeCbox.Visible = false;
                GarNbrCarTxt.Visible = false;
                GarLbl.Visible = false;
                MissingGarLbl.Visible = false;
            }
        }

        private void SetDefaultCarsForCarport(SMParcelMast parcelMast, string Type)
        {
            if (Type == "CP" && NewArea <= 275)
            {
                CarPortNbrCarTxt.Text = "1";
                parcelMast.CarportNumCars = 1;
                CpNbr = 1;
            }
            if (Type == "CP" && NewArea > 275)
            {
                CarPortNbrCarTxt.Text = "2";
                parcelMast.CarportNumCars = 2;
                CpNbr = 2;
            }
        }

        private void SetDefaultCarsForGarage(SMParcelMast parcelMast, string Type)
        {
            if (Type == "GAR" && NewArea <= 360)
            {
                GarNbrCarTxt.Text = "1";
                parcelMast.Garage1NumCars = 1;
                GarNbr = 1;
            }
            if (Type == "GAR" && NewArea > 360)
            {
                GarNbrCarTxt.Text = "2";
                parcelMast.Garage1NumCars = 2;
                GarNbr = 2;
            }
        }

        public static int CPCode
        {
            get; set;
        }

        public static int CpNbr
        {
            get; set;
        }

        public static int GarageCode
        {
            get; set;
        }

        public static int GarNbr
        {
            get; set;
        }

        //private DBAccessManager _fox = null;

        private Dictionary<int, StabType> _cportList = null;
        private Dictionary<int, StabType> _garList = null;
        private decimal NewArea = 0;
        private SMParcelMast originalParcelMast;
        private SMParcelMast parcelMast = SketchUpGlobals.ParcelWorkingCopy.ParcelMast;
    }
}