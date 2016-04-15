using System;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace SketchUp
{
    public class BuildingSection
    {
        public SWallTech.CAMRA_Connection DatabaseConnection
        {
            get; set;
        }

        public BuildingSection(SWallTech.CAMRA_Connection conn, int record, int card,
            string sectionLetter)
        {
            DatabaseConnection = conn;
            Record = record;
            Card = card;
            SectionLetter = sectionLetter;
        }

        private int _record;

        public int Record
        {
            get
            {
                return _record;
            }

            set
            {
                _record = value;
            }
        }

        private int _card;

        public int Card
        {
            get
            {
                return _card;
            }

            set
            {
                _card = value;
            }
        }

        private string _sectionLetter;

        public string SectionLetter
        {
            get
            {
                return _sectionLetter;
            }

            set
            {
                _sectionLetter = value;
            }
        }

        private BuildingLineCollection _sectionLines;

        public BuildingLineCollection SectionLines
        {
            get
            {
                return _sectionLines;
            }

            set
            {
                _sectionLines = value;
            }
        }

        private string _sectionType;

        public string SectionType
        {
            get
            {
                return _sectionType;
            }

            set
            {
                _sectionType = value;
            }
        }

        private string _class;

        public string Class
        {
            get
            {
                return _class;
            }

            set
            {
                _class = value;
            }
        }

        private decimal _squareFootage;

        public decimal SquareFootage
        {
            get
            {
                return _squareFootage;
            }

            set
            {
                _squareFootage = value;
            }
        }

        private decimal _factor;

        public decimal Factor
        {
            get
            {
                return _factor;
            }

            set
            {
                _factor = value;
            }
        }

        private decimal _depreciation;

        public decimal Depreciation
        {
            get
            {
                return _depreciation;
            }

            set
            {
                _depreciation = value;
            }
        }

        private bool _hasZeroDepreciation;

        public bool HasZeroDepreciation
        {
            get
            {
                return _hasZeroDepreciation;
            }

            set
            {
                _hasZeroDepreciation = value;
            }
        }

        private bool _HasSketch;

        public bool HasSketch
        {
            get
            {
                return _HasSketch;
            }

            set
            {
                _HasSketch = value;
            }
        }

        private decimal _storyHeight;

        public decimal StoryHeight
        {
            get
            {
                return _storyHeight;
            }

            set
            {
                _storyHeight = value;
            }
        }

        private string _description;

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public int DepreciationValue
        {
            get
            {
                if (HasZeroDepreciation)
                    return 0;

                return Convert.ToInt32(SquareFootage * Rate * (1 + Factor)) -
                        Convert.ToInt32(SquareFootage * Rate * (1 + Factor) * (1 - Depreciation));
            }
        }

        public int FactorValue
        {
            get
            {
                return Convert.ToInt32(SquareFootage * Rate * (1 + Factor)) -
                        Convert.ToInt32(SquareFootage * Rate);
            }
        }

        private decimal _rate;

        public decimal Rate
        {
            get
            {
                return _rate;
            }

            set
            {
                _rate = value;
            }
        }

        public int Value
        {
            get; set;
        }

        public Point[] SectionPoints
        {
            get
            {
                var callingMethod = new System.Diagnostics.StackTrace(1, false)
                         .GetFrame(0).GetMethod();

                Point[] points;
                if (this.SectionLines == null)
                {
                    points = new Point[0];
                }
                else
                {
                    points = PopulatePointsFromSections();
                }
                return points;
            }
        }

        private Point[] PopulatePointsFromSections()
        {
            Point[] points = new Point[this.SectionLines.Count];
            for (int i = 0; i < this.SectionLines.Count; i++)
            {
                var line = this.SectionLines[i + 1];
                points[i] = new Point(Convert.ToInt32(line.Point1X), Convert.ToInt32(line.Point1Y));
            }

            return points;
        }

        public decimal XAdjustment
        {
            get
            {
                return this.SectionLines[1].Point1X;
            }
        }

        public decimal YAdjustment
        {
            get
            {
                return this.SectionLines[1].Point1Y;
            }
        }

        // COPIED methods, may need attention
        public bool deleteForReal()
        {
            // DELETE from database

            return true;
        }

        public bool deleteForReal(bool deleteAttachments)
        {
            // DELETE from database

            return true;
        }

        public enum LimitTypes
        {
            MaxX,
            MaxY,
            MinX,
            MinY
        }

        public decimal GetLimit(LimitTypes type)
        {
            decimal limit = 0m;

            if (this.HasSketch)
            {
                string limitByType = string.Empty;
                string sql = string.Empty;

                switch (type)
                {
                    case LimitTypes.MaxX:
                        limitByType = "max(jlpt1x) ";
                        break;

                    case LimitTypes.MaxY:
                        limitByType = "max(jlpt1y) ";
                        break;

                    case LimitTypes.MinX:
                        limitByType = "min(jlpt1x) ";
                        break;

                    case LimitTypes.MinY:
                        limitByType = "min(jlpt1y) ";
                        break;

                    default:
                        limitByType = " 0 ";

                        break;
                }
                sql = string.Format("select {0}  from skline where jlrecord = {1} and jldwell = {2}  and jlsect = '{3}' and statusflag <> 'D'", Record.ToString(), Card.ToString(), SectionLetter);

                try
                {
                    object obj = DatabaseConnection.DBConnection.ExecuteScalar(sql);
                    string str = obj.ToString();
                    if (!"".Equals(str))
                    {
                        limit = decimal.Parse(str);

                        switch (type)
                        {
                            case LimitTypes.MaxX:
                            case LimitTypes.MinX:
                                limit -= this.XAdjustment;
                                break;

                            case LimitTypes.MaxY:
                            case LimitTypes.MinY:
                                limit -= this.YAdjustment;
                                break;

                            default:
                                break;
                        }
                    }
                }
                catch (Exception sqlex)
                {
                    throw sqlex;
                }
                finally
                {
                }
            }
            return limit;
        }

        public int IncrementAllLines(int startingLineNumber)
        {
            int lineCount = 0;
            for (int i = this.SectionLines.Count; i >= startingLineNumber; i--)
            {
                this.SectionLines[i].IncrementLineNumber();
                lineCount++;
            }

            return lineCount;
        }

        public static BuildingSection CreateSection(SWallTech.CAMRA_Connection conn,
            int record, int card)
        {
            return CreateSection(conn, record, card, String.Empty, 0M, String.Empty);
        }

        public static BuildingSection CreateSection(SWallTech.CAMRA_Connection conn,
            int record, int card, string sectionType, decimal storyHeight,
            string sectionLetter)
        {
            string sectLetter = String.Empty;
            if (sectionLetter.Equals(String.Empty))
            {
                sectLetter = BuildingSectionCollection.GetNextSectionLetter(conn, record, card);
            }
            else
            {
                sectLetter = sectionLetter;
            }

            var newSect = new BuildingSection(conn,
                    record,
                    card,
                    sectLetter);

            if ("A".Equals(newSect.SectionLetter.Trim()))
            {
                newSect.SectionType = "BASE";
            }
            else if (!String.Empty.Equals(sectionType))
            {
                newSect.SectionType = sectionType;
            }

            newSect.StoryHeight = storyHeight;

            return newSect;
        }
    }
}