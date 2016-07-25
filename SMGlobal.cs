using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public static class SMGlobal
    {
        #region Methods

        public static CamraDataEnums.CardinalDirection CalculateLineDirection(PointF startPoint, PointF endPoint)
        {
            try
            {
                CamraDataEnums.CardinalDirection lineDirection = CamraDataEnums.CardinalDirection.None;

                // Check for NEWS line first
                float startX = startPoint.X;
                float startY = startPoint.Y;
                float endX = endPoint.X;
                float endY = endPoint.Y;
                if (startX == endX && endY < startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.N;
                }
                else if (startX == endX && endY > startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.S;
                }
                else if (startY == endY && endX < startX)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.W;
                }
                else if (endX > startX && endY == startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.E;
                }
                else if (endX > startX && endY > startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.SE;
                }
                else if (endX > startX && endY < startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.NE;
                }
                else if (endX < startX && endY > startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.SW;
                }
                else if (endX < startX && endY < startY)
                {
                    lineDirection = CamraDataEnums.CardinalDirection.NW;
                }

                return lineDirection;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        public static string CodeToDescription(List<StabType> stabTypeList, int code)
        {
            string description = string.Empty;
            var descriptionLookup = (from t in stabTypeList where t.Code == code.ToString() select t.Description).FirstOrDefault();
            if (descriptionLookup != null)
            {
                description = descriptionLookup.ToString();
            }
            return description;
        }

        public static PointF DbPointToScaledPoint(double dataX, double dataY, double scale, PointF sketchOrigin)
        {
            var screenX = (float)((dataX * scale) + (double)sketchOrigin.X);
            var screenY = (float)((dataY * scale) + (double)sketchOrigin.Y);
            return new PointF((float)Math.Round(screenX, 2), (float)Math.Round(screenY, 2));
        }

        public static int DescriptionToCode(List<StabType> stabTypeList, string description)
        {
            int code = 0;
            var codeLookup = (from c in stabTypeList where c.Description.ToUpper().Trim() == description.ToUpper().Trim() select c.Code).FirstOrDefault();
            if (codeLookup != null)
            {
                int.TryParse(codeLookup.ToString(), out code);
            }
            return code;
        }

        public static CamraDataEnums.CardinalDirection DirectionFromString(string direction)
        {
            CamraDataEnums.CardinalDirection dir = CamraDataEnums.CardinalDirection.None;
            switch (direction)

            {
                case "E":
                    {
                        dir = CamraDataEnums.CardinalDirection.E;
                        break;
                    }
                case "NE":
                    {
                        dir = CamraDataEnums.CardinalDirection.NE;
                        break;
                    }
                case "SE":
                    {
                        dir = CamraDataEnums.CardinalDirection.SE;
                        break;
                    }
                case "W":
                    {
                        dir = CamraDataEnums.CardinalDirection.W;
                        break;
                    }
                case "NW":
                    {
                        dir = CamraDataEnums.CardinalDirection.NW;
                        break;
                    }
                case "SW":
                    {
                        dir = CamraDataEnums.CardinalDirection.SW;
                        break;
                    }

                case "S":
                    {
                        dir = CamraDataEnums.CardinalDirection.S;
                        break;
                    }
                case "N":
                    {
                        dir = CamraDataEnums.CardinalDirection.N;
                        break;
                    }
                default:
                    break;
            }
            return dir;
        }

        public static MoveDirection GetDirectionOfKeyEntered(KeyEventArgs e)
        {
            SMGlobal.MoveDirection moveDir;
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.E:
                case Keys.R:
                    moveDir = MoveDirection.E;

                    break;

                case Keys.Left:
                case Keys.L:
                case Keys.W:
                    moveDir = MoveDirection.W;

                    break;

                case Keys.Up:
                case Keys.N:
                case Keys.U:
                    moveDir = MoveDirection.N;

                    break;

                case Keys.Down:
                case Keys.D:
                case Keys.S:
                    moveDir = MoveDirection.S;

                    break;

                default:
                    moveDir = MoveDirection.None;

                    break;
            }
            return moveDir;
        }

        public static decimal LargerDecimal(decimal firstNumber, decimal secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return secondNumber;
            }
            else
            {
                return firstNumber;
            }
        }

        public static double LargerDouble(double firstNumber, double secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return secondNumber;
            }
            else
            {
                return firstNumber;
            }
        }

        public static float LargerFloat(float firstNumber, float secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return secondNumber;
            }
            else
            {
                return firstNumber;
            }
        }

        public static int LargerInteger(int firstNumber, int secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return secondNumber;
            }
            else
            {
                return firstNumber;
            }
        }

        public static double LineLength(PointF startPoint, PointF endPoint)
        {
            double xLength = endPoint.X - startPoint.X;
            double yLength = endPoint.Y - startPoint.Y;
            double xLenSquared = Math.Pow(xLength, 2);
            double yLenSquared = Math.Pow(yLength, 2);
            double lengthSquaredTotals = xLenSquared + yLenSquared;
            return Math.Round(Math.Sqrt(lengthSquaredTotals), 1);
        }

        public static bool PointIsOnLine(PointF lineStart, PointF lineEnd, PointF checkedPoint)
        {
            bool lineContainsPoint = false;
            bool checkedPointIsBetweenEndpoints = CheckedPointIsBetweenEndpoints(lineStart, lineEnd, checkedPoint);

            double mainLineSlope = LineSlope(lineStart, lineEnd);
            double checkLineSlope = LineSlope(lineStart, checkedPoint);
            lineContainsPoint = (checkedPointIsBetweenEndpoints && (Math.Abs(mainLineSlope) == Math.Abs(checkLineSlope)));
            return lineContainsPoint;
        }

        public static CamraDataEnums.CardinalDirection ReverseDirection(CamraDataEnums.CardinalDirection direction)
        {
            CamraDataEnums.CardinalDirection reverseDirection = direction;
            switch (direction)
            {
                case CamraDataEnums.CardinalDirection.E:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.W;
                        break;
                    }
                case CamraDataEnums.CardinalDirection.NE:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.NW;
                        break;
                    }
                case CamraDataEnums.CardinalDirection.SE:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.SW;
                        break;
                    }
                case CamraDataEnums.CardinalDirection.W:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.E;
                        break;
                    }
                case CamraDataEnums.CardinalDirection.NW:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.NE;
                        break;
                    }
                case CamraDataEnums.CardinalDirection.SW:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.SE;
                        break;
                    }

                case CamraDataEnums.CardinalDirection.S:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.N;
                        break;
                    }
                case CamraDataEnums.CardinalDirection.N:
                    {
                        reverseDirection = CamraDataEnums.CardinalDirection.S;
                        break;
                    }

                default:
                    Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}.\n{2} is not a valid direction value.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, direction));
                    break;
            }

            return reverseDirection;
        }

        public static string ReverseDirection(string direction)
        {
            string reverseDirection = direction;
            switch (direction)
            {
                case "E":
                    {
                        reverseDirection = "W";
                        break;
                    }
                case "NE":
                    {
                        reverseDirection = "NW";
                        break;
                    }
                case "SE":
                    {
                        reverseDirection = "SW";
                        break;
                    }
                case "W":
                    {
                        reverseDirection = "E";
                        break;
                    }
                case "NW":
                    {
                        reverseDirection = "NE";
                        break;
                    }
                case "SW":
                    {
                        reverseDirection = "SE";
                        break;
                    }

                case "S":
                    {
                        reverseDirection = "N";
                        break;
                    }
                case "N":
                    {
                        reverseDirection = "S";
                        break;
                    }

                default:
                    Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}.\n{2} is not a valid direction value.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, direction));
                    break;
            }

            return reverseDirection;
        }

        public static PointF ScaledPointToDbPoint(double screenX, double screenY, double scale, PointF sketchOrigin)
        {
            var dataX = (float)((screenX / scale) - (double)sketchOrigin.X);
            var dataY = (float)((screenY / scale) - (double)sketchOrigin.Y);
            var dbPoint = new PointF(dataX, dataY);
            return dbPoint;
        }

        public static decimal SmallerDecimal(decimal firstNumber, decimal secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return firstNumber;
            }
            else
            {
                return secondNumber;
            }
        }

        public static double SmallerDouble(double firstNumber, double secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return firstNumber;
            }
            else
            {
                return secondNumber;
            }
        }

        public static float SmallerFloat(float firstNumber, float secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return firstNumber;
            }
            else
            {
                return secondNumber;
            }
        }

        public static int SmallerInteger(int firstNumber, int secondNumber)
        {
            if (secondNumber >= firstNumber)
            {
                return firstNumber;
            }
            else
            {
                return secondNumber;
            }
        }

        public static double StoryValueFromText(string storiesText)
        {
            double stories = 1.00;
            if (storiesText == "S/L" || storiesText == "S/F")
            {
                stories = 1.00;
            }
            else
            {
                double.TryParse(storiesText, out stories);
            }
            return stories;
        }

        #endregion Methods

        #region Fields

        public const string EastArrow = "\u2192";

        public const string NorthArrow = "\u2191";

        public const string NorthEastArrow = "\u2197";

        public const string NorthWestArrow = "\u2196";

        public const string SouthArrow = "\u2193";

        public const string SouthEastArrow = "\u2198";

        public const string SouthWestArrow = "\u2199";

        public const string WestArrow = "\u2190";

        #endregion Fields

        #region Enums

        public enum MoveDirection
        {
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW,
            None
        }

        public enum SnapshotState
        {
            OriginalMainImage,
            InitialEditBase,
            Intermediate,
            CurrentlyEditing,
            PreSaveReview,
            ApprovedForSave
        }

        #endregion Enums

        private static bool CheckedPointIsBetweenEndpoints(PointF lineStart, PointF lineEnd, PointF checkedPoint)
        {
            bool xInRange = false;
            bool yInRange = false;

            // handles the situation where the endpoint is less than the start point based on line
            // direction or the order in which the paramters are passed in
            if (lineEnd.Y <= lineStart.Y)
            {
                yInRange = checkedPoint.Y >= lineEnd.Y && checkedPoint.Y <= lineStart.Y;
            }
            else
            {
                yInRange = checkedPoint.Y <= lineEnd.Y && checkedPoint.Y >= lineStart.Y;
            }
            if (lineEnd.X <= lineStart.X)
            {
                xInRange = checkedPoint.X >= lineEnd.X && checkedPoint.X <= lineStart.X;
            }
            else
            {
                xInRange = checkedPoint.X <= lineEnd.X && checkedPoint.X >= lineStart.X;
            }
            return xInRange && yInRange;
        }

        private static double LineSlope(PointF lineStart, PointF lineEnd)
        {
            double rise = (double)lineEnd.Y - (double)lineStart.Y;
            double run = (double)lineEnd.X - (double)lineStart.X;
            double slope = 0.00;
            if (run == 0)
            {
                slope = 0.00;
            }
            else
            {
                slope = rise / run;
            }
            return slope;
        }
    }
}