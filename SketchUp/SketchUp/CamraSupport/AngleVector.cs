using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
    public class AngleVector
    {
        decimal xDistanceScaled;
        decimal yDistanceScaled;
        decimal xDistanceEntered;
        decimal yDistanceEntered;
        
        CamraDataEnums.CardinalDirection angledLineDirection;
       
     
        public CamraDataEnums.CardinalDirection AngledLineDirection
        {
            get
            {
                return angledLineDirection;
            }

            set
            {
                angledLineDirection = value;
            }
        }

       

        public decimal XDistanceEntered
        {
            get
            {
                return xDistanceEntered;
            }

            set
            {
                xDistanceEntered = value;
            }
        }

        public decimal YDistanceEntered
        {
            get
            {
                return yDistanceEntered;
            }

            set
            {
                yDistanceEntered = value;
            }
        }
    }
}
