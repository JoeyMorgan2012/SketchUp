using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class AdjustCarsData : Form
    {
 // TODO: Remove if not needed:	       private bool carportsOk;
 // TODO: Remove if not needed:	       private bool garagesOk;
        private bool garagesAndCarportsOk;
 // TODO: Remove if not needed:	       private string feedbackMessage;
        SMVehicleStructure vehicleStructures;
        public bool GaragesAndCarportsOk
        {
            get
            {
                return garagesAndCarportsOk;
            }

            set
            {
                garagesAndCarportsOk = value;
            }
        }

        public SMVehicleStructure VehicleStructures
        {
            get
            {
                return vehicleStructures;
            }

            set
            {
                vehicleStructures = value;
            }
        }

        public AdjustCarsData(SMParcel parcel)
        {
            InitializeComponent();
            vehicleStructures = new SMVehicleStructure(parcel);
        }
    }
}
