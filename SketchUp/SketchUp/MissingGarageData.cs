using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public partial class MissingGarageData : Form
    {
        //private DBAccessManager _fox = null;
        private CAMRA_Connection _conn = null;

        private SketchUpParcelData _currentParcel = null;

        private decimal NewArea = 0;

        public static int GarCode
        {
            get; set;
        }

        public static int GarNbr
        {
            get; set;
        }

        public static int CPCode
        {
            get; set;
        }

        public static int CpNbr
        {
            get; set;
        }

        public MissingGarageData(SWallTech.CAMRA_Connection conn, SketchUpParcelData data, decimal newSectArea, string Type)
        {
            _conn = conn;
            _currentParcel = data;
            GarCode = 0;
            GarNbr = 0;
            CPCode = 0;
            CpNbr = 0;

            InitializeComponent();

            Build_CarPort_Data();
            Build_Garage_Data();

            GarNbrCarTxt.Text = "0";
            CarPortNbrCarTxt.Text = "0";

            NewArea = newSectArea;

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

            if (Type == "GAR" && NewArea <= 360)
            {
                GarNbrCarTxt.Text = "1";
                _currentParcel.mgarNc = 1;
                GarNbr = 1;
            }
            if (Type == "GAR" && NewArea > 360)
            {
                GarNbrCarTxt.Text = "2";
                _currentParcel.mgarNc = 2;
                GarNbr = 2;
            }

            if (Type == "CP" && NewArea <= 275)
            {
                CarPortNbrCarTxt.Text = "1";
                _currentParcel.mcarNc = 1;
                CpNbr = 1;
            }
            if (Type == "CP" && NewArea > 275)
            {
                CarPortNbrCarTxt.Text = "2";
                _currentParcel.mcarNc = 2;
                CpNbr = 2;
            }

            GarTypeCbox.Items.Clear();
            ListGarageSelection();
            CarPortTypCbox.Items.Clear();
            ListCPortSelection();
        }

        private Dictionary<int, StabType> _garList = null;
        private Dictionary<int, StabType> _cportList = null;

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

                foreach (var item in CamraSupport.GarageTypeCollection)
                {
                    _garList.Add(++index, item);
                    GarTypeCbox.Items.Add(item._printedDescription);
                }
            }
            GarTypeCbox.SelectedIndex = _cboxIndex;
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

                foreach (var item in CamraSupport.CarPortTypeCollection)
                {
                    _cportList.Add(++index, item);
                    CarPortTypCbox.Items.Add(item._printedDescription);
                }
            }
            CarPortTypCbox.SelectedIndex = _cboxIndex;
        }

        private void Build_Garage_Data()
        {
            if (GarTypeCbox.Items.Count > 0)
            {
                GarTypeCbox.SelectedIndex = 0;
                for (int i = 1; i < GarTypeCbox.Items.Count; i++)
                {
                    string listGarageType = GarTypeCbox.Items[i].ToString().Substring(0, 2);
                    if (_currentParcel.mgart.ToString().Trim() == listGarageType)
                    {
                        GarTypeCbox.SelectedIndex = i;
                    }
                }
            }
        }

        private void GarTypeCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GarTypeCbox.Items.Count > 0)
            {
                if (_currentParcel != null && GarTypeCbox.SelectedIndex > 0)
                {
                    int Garageck = Convert.ToInt32(GarTypeCbox.SelectedItem.ToString().Substring(0, 2));
                    GarCode = Garageck;

                    if (Garageck != _currentParcel.orig_mgart)
                    {
                        GarTypeCbox.BackColor = Color.PaleGreen;
                        _currentParcel.mgart = Garageck;
                    }
                    if (Garageck == _currentParcel.orig_mgart)
                    {
                        GarTypeCbox.BackColor = Color.White;
                        _currentParcel.mgart = Garageck;
                    }
                }
            }
        }

        private void Build_CarPort_Data()
        {
            if (CarPortTypCbox.Items.Count > 0)
            {
                CarPortTypCbox.SelectedIndex = 0;
                for (int i = 1; i < CarPortTypCbox.Items.Count; i++)
                {
                    string listCarPortType = CarPortTypCbox.Items[i].ToString().Substring(0, 2);
                    if (_currentParcel.mcarpt.ToString().Trim() == listCarPortType)
                    {
                        CarPortTypCbox.SelectedIndex = i;
                    }
                }
            }
        }

        private void CarPortTypCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CarPortTypCbox.Items.Count > 0)
            {
                if (_currentParcel != null && CarPortTypCbox.SelectedIndex > 0)
                {
                    int CarPortck = Convert.ToInt32(CarPortTypCbox.SelectedItem.ToString().Substring(0, 2));
                    CPCode = CarPortck;

                    if (CarPortck != _currentParcel.orig_mcarpt)
                    {
                        CarPortTypCbox.BackColor = Color.PaleGreen;
                        _currentParcel.mcarpt = CarPortck;
                    }
                    if (CarPortck == _currentParcel.orig_mcarpt)
                    {
                        CarPortTypCbox.BackColor = Color.White;
                        _currentParcel.mcarpt = CarPortck;
                    }
                }
            }
        }

        private void CarPortNbrCarTxt_TextChanged(object sender, EventArgs e)
        {
            CpNbr = Convert.ToInt32(CarPortNbrCarTxt.Text.ToString());
        }

        private void CarPortNbrCarTxt_Leave(object sender, EventArgs e)
        {
            CpNbr = Convert.ToInt32(CarPortNbrCarTxt.Text.ToString());
        }

        private void GarNbrCarTxt_TextChanged(object sender, EventArgs e)
        {
            GarNbr = Convert.ToInt32(GarNbrCarTxt.Text.ToString());
        }

        private void GarNbrCarTxt_Leave(object sender, EventArgs e)
        {
            GarNbr = Convert.ToInt32(GarNbrCarTxt.Text.ToString());
        }
    }
}