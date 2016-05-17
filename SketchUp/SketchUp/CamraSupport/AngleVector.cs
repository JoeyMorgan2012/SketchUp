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
        decimal scale;
        CamraDataEnums.CardinalDirection angledLineDirection;

        public decimal XDistanceScaled
        {
            get
            {
                xDistanceScaled = XDistanceEntered * Scale;
                return xDistanceScaled;
            }

            set
            {
                xDistanceScaled = value;
            }
        }

        public decimal YDistanceScaled
        {
            get
            {
                yDistanceScaled = yDistanceEntered * Scale;
                return yDistanceScaled;
            }

            set
            {
                yDistanceScaled = value;
            }
        }

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

        public decimal Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
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
