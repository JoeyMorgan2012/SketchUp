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
        decimal xLength;
        decimal yLength;
        
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

       

        public decimal XLength
        {
            get
            {
                return xLength;
            }

            set
            {
                xLength = value;
            }
        }

        public decimal YLength
        {
            get
            {
                return yLength;
            }

            set
            {
                yLength = value;
            }
        }
    }
}
