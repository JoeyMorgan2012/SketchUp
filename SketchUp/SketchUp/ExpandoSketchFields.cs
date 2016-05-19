using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    /*
        The ExpandoSketch Form contains all of the sketch-rendering code.
        The original file was over 8,000 lines long, so the class is broken into two
        physical files defining the logical class. The breakdown is:

            ExpandoSketch.cs - All methods not refactored into SketchRepository

            ExpandoSketchFields.cs -  This file contains fields, properties and enums for the ExpandoSketch Form class.
    */

    public partial class ExpandoSketch : Form
    {
        #region Enums

        public enum MoveDirections
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

        public enum MovementMode
        {
            Draw,
            Erase,
            Jump,
            MoveDrawRed,
            MoveNoLine,
            NoMovement
        }

        public enum DrawingState
        {
         
            Drawing,
            JumpPointSelected,
            SectionAdded,
            SketchLoaded,
            
        }

        #endregion Enums

        #region Fields

        #region private fields

        private static bool checkDirection = false;
        private static DrawingState editState=DrawingState.SketchLoaded;
        private Image _baseImage;
        private SWallTech.CAMRA_Connection _conn = null;
        private int _curLineCnt = 0;
        private float _currentScale = 0;
        private bool _isAngle = false;
        private bool _isclosing = false;
        private bool _isJumpMode = false;
        private bool _isKeyValid = false;
        private string _lenString = String.Empty;
        private Image _mainImage;
        private int _mouseX;
        private int _mouseY;
        private int _newIndex = 0;
        private List<PointF> _newSectionPoints;
        private int _nextLineCount = 0;
        private string _nextSectType = String.Empty;
        private decimal _nextStoryHeight = 0;
        private bool _openForm = false;
        private string _priorDirection = "";
        private bool _reOpenSec = false;
        private int _savedAttLine;
        private string _savedAttSection = "";
        private Dictionary<int, float> _StartX = null;
        private Dictionary<int, float> _StartY = null;
        private decimal adjNewSecX = 0;
        private decimal adjNewSecY = 0;
        private decimal adjOldSecX = 0;
        private decimal adjOldSecY = 0;
        private decimal AngD1 = 0;
        private decimal AngD2 = 0;
        private DataTable AreaTable = null;
        private DataTable AttachmentPointsDataTable = null;
        private SMSection attachmentSection;
        private DataTable attachPoints;
        private int AttLineNo = 0;
        private string AttSectLtr = String.Empty;
        private string AttSpLineDir = String.Empty;
        private int AttSpLineNo = 0;
        private float BaseX = 0;
        private float BaseY = 0;
        private Brush blackBrush;
        private Brush blueBrush;
        private Pen bluePen;
        private List<int> carportCodes = null;
        private List<String> carportTypes = null;

        //Undo uses this but we are re-doing undo. JMM 3-15-2016
        private Color colorRed = Color.Red;

        private int currentAttachmentLine = 0;
        private string CurrentAttDir = String.Empty;
        private string CurrentSecLtr = String.Empty;
        private decimal dbEndOfMovementX;
        private decimal dbEndOfMovementY;
        private decimal dbLineLengthX;
        private decimal dbLineLengthY;
        private PointF dbMovementEndPoint;
        private PointF dbMovementStartPoint;
        private decimal dbStartOfMovementX;
        private decimal dbStartOfMovementY;
        private int directionModifyerX = 1;
        private int directionModifyerY = 1;
        private MoveDirections directionOfMovement;
        private DataTable displayDataTable = null;
        private decimal distanceEntered;
        private float drawingScale = 1.0f;
        private DataTable DupAttPoints = null;
        private PointF endOfCurrentLine;
        private PointF endOfJumpMovePoint;
        private float endOldSecX = 0;
        private float endOldSecY = 0;
        private float EndX = 0;
        private decimal EndxD = 0;
        private float EndY = 0;
        private decimal EndyD = 0;
        private bool firstTimeLoaded;
        private List<string> FixSect = null;
        private Graphics g;
        private List<int> GarCodes = null;
        private List<String> GarTypes = null;
        private Brush greenBrush;
        private bool isInAddNewPointMode = false;
        private bool isLastLine;
        private SMLine jumpPointLine;
        private List<SMLine> jumpPointLines;
        private DataTable JumpTable = null;
        private float JumpX = 0;
        private float JumpY = 0;
        private List<string> legalMoveDirections;
        private int lineCnt = 0;
        private int LineNumberToBreak = 0;
        private string Locality = String.Empty;
     
        private string midDirect = String.Empty;
        private int midLine = 0;
        private string midSection = String.Empty;
        private decimal movementDistanceScaled;
        private byte[] ms = null;
        private DataTable MultiplePoints = null;
        private int mylineNo = 0;

        //private decimal Xadj1 = 0;
        //private decimal Yadj1 = 0;
        //public static bool _undoModeA = false;
        private bool NeedToRedraw = false;

        private int NewPointIndex;
        private decimal NewSectionBeginPointX = 0;
        private decimal NewSectionBeginPointY = 0;
        private decimal NewSplitLIneDist = 0;
        private string OffSetAttSpLineDir = String.Empty;
        private Pen orangePen;
        private SMParcelMast parcelMast;
        private decimal prevPt2X = 0;
        private decimal prevPt2Y = 0;
        private decimal prevTst1 = 0;
        private decimal prevTst2 = 0;
        private float PrevX = 0;
        private float PrevY = 0;
        private float pt2X = 0;
        private float pt2Y = 0;
        private Point[] pts;
        private Brush redBrush;
        private Pen redPen;
        private DataTable REJumpTable = null;
        private DataTable RESpJumpTable = null;

        //   private Dictionary<int, byte[]> savpic = null;
        private float ScaleBaseX = 0;

        private float ScaleBaseY = 0;
        private PointF scaledBeginPoint;
        private PointF scaledEndOfMovement;
        private PointF scaledJumpPoint;
        private PointF scaledStartOfMovement;
        private float SecBeginX = 0;
        private float SecBeginY = 0;
        private List<String> SecLetters = null;
        private PointF sectionAttachPoint;
        private DataTable SectionLtrs = null;
        private DataTable SectionTable = null;

        //  private static List<int> savcnt;
       
        private string SketchCard = String.Empty;
        private string SketchFolder = String.Empty;
        private Bitmap sketchImage;
        private Bitmap sketchImageBMP;
        private PointF sketchOrigin;
        private string SketchRecord = String.Empty;
        private DataTable sortDist = null;
        private decimal splitLineDist = 0;
        private int StandardDrawWidthAndHeight = 3;
        private PointF startOfCurrentLine;
        /*
		Refactored by renaming and providing for null values. Going to ensure that the
		naming conventions are consistent for all properties. Any field that backs a property
		will be in camel case (e.g. camelCase) while fields that are not property-backing will be
		in Pascal case. (e.g. PascalCase).

		*/
        private float StartX = 0;
        private float StartY = 0;
        private DataTable StrtPts = null;
        private decimal StrxD = 0;
        private decimal StryD = 0;
        private int TempAttSplineNo = 0;
        private decimal txtLoc = 0;
        private float txtLocf = 0;
        private float txtX = 0;
        private float txtY = 0;
        private Point[] unadj_pts;
        private decimal XadjR = 0;
        private decimal YadjR = 0;
        private SMSection workingSection;
        private string lastSectionLetter;
        #endregion private fields

        #region Public Fields
        
        
        public static bool _cantSketch = false;
        public static bool _deleteMaster = false;
        public static bool _deleteThisSketch = false;
        public static bool _insertLine = false;
        public static bool _isClosed = false;
        public static bool _undoMode = false;
        public static int finalClick;
        public static List<string> Letters = new List<string>() { "A", "B", "C", "D", "F", "G", "H", "I", "J", "K", "L", "M" };
        public static bool RefreshEditImageBtn = false;
        public bool _addSection = false;
        public decimal _calcNextSectArea = 0;
        public bool _closeSketch = false;
        public bool _hasMultiSection = false;
        public bool _hasNewSketch;
        public bool _hasSketch = false;
        public bool _undoLine = false;
        public bool _vacantParcelSketch = false;
        public float BeginSplitX = 0;
        public float BeginSplitY = 0;
        public decimal begSplitX = 0;
        public decimal begSplitY = 0;
        public int carportCount = 0;
        public string ConnectSec = String.Empty;
        public decimal CPSize = 0;
        public int CurSecLineCnt = 0;
        public SWallTech.CAMRA_Connection dbConn = null;
        public float delStartX = 0;
        public float delStartY = 0;
        public decimal distance = 0;
        public float distanceD = 0;
        public float distanceDXF = 0;
        public float distanceDYF = 0;
        public bool draw = false;
        public decimal EndSplitX = 0;
        public float EndSplitXF = 0;
        public decimal EndSplitY = 0;
        public float EndSplitYF = 0;
        public int Garcnt = 0;
        public decimal GarSize = 0;
        public float NextStartX = 0;
        public float NextStartY = 0;
        public string offsetDir = String.Empty;
        public decimal OrigLineLength = 0;
        public decimal OrigStartX = 0;
        public decimal OrigStartY = 0;
        public float PrevStartX = 0;
        public float PrevStartY = 0;
        public decimal RemainderLineLength = 0;
        public int SecItemCnt = 0;
        public int SecLineCnt = 0;
        public decimal startSplitX = 0;
        public decimal startSplitY = 0;
        public DataTable undoPoints = null;
        public float UNextStartX = 0;
        public float UNextStartY = 0;
        public float UPrevStartX = 0;
        public float UPrevStartY = 0;
        public float Xadj = 0;
        public float XadjP = 0;
        public float Yadj = 0;
        public float YadjP = 0;
        private static string nextSectLtr = String.Empty;
        private bool isNewSketch = false;
        private string lastAngDir = string.Empty;
        private string lastDir = string.Empty;
        private decimal nextSectArea = 0;
        private bool undoJump = false;

        #endregion Public Fields

        #endregion Fields

        #region Properties

        public static string NextSectLtr
        {
            get
            {
                return nextSectLtr;
            }

            set
            {
                nextSectLtr = value;
            }
        }

        public SMSection AttachmentSection
        {
            get
            {
                return attachmentSection;
            }

            set
            {
                attachmentSection = value;
            }
        }

        public DataTable AttachPoints
        {
            get
            {
                return attachPoints;
            }

            set
            {
                attachPoints = value;
            }
        }

        public Brush BlackBrush
        {
            get
            {
                blackBrush = Brushes.Black;
                return blackBrush;
            }

            set
            {
                blackBrush = value;
            }
        }

        public Brush BlueBrush
        {
            get
            {
                blueBrush = Brushes.DarkBlue;
                return blueBrush;
            }

            set
            {
                blueBrush = value;
            }
        }

        public Pen BluePen
        {
            get
            {
                if (bluePen == null)
                {
                    bluePen = new Pen(Color.DarkBlue, 1);
                }

                return bluePen;
            }

            set
            {
                bluePen = value;
            }
        }

        public List<int> CarportCodes
        {
            get
            {
                if (carportCodes == null)
                {
                    carportCodes = new List<int>();
                }
                return carportCodes;
            }

            set
            {
                carportCodes = value;
            }
        }

        public decimal DbEndOfMovementX
        {
            get
            {
                return dbEndOfMovementX;
            }

            set
            {
                dbEndOfMovementX = value;
            }
        }

        public decimal DbEndOfMovementY
        {
            get
            {
                return dbEndOfMovementY;
            }

            set
            {
                dbEndOfMovementY = value;
            }
        }

        public decimal DbLineLengthX
        {
            get
            {
                return dbLineLengthX;
            }

            set
            {
                dbLineLengthX = value;
            }
        }

        public decimal DbLineLengthY
        {
            get
            {
                return dbLineLengthY;
            }

            set
            {
                dbLineLengthY = value;
            }
        }

        public PointF DbMovementEndPoint
        {
            get
            {
                return dbMovementEndPoint;
            }

            set
            {
                dbMovementEndPoint = value;
            }
        }

        public PointF DbMovementStartPoint
        {
            get
            {
                return dbMovementStartPoint;
            }

            set
            {
                dbMovementStartPoint = value;
            }
        }

        public decimal DbStartOfMovementX
        {
            get
            {
                return dbStartOfMovementX;
            }

            set
            {
                dbStartOfMovementX = value;
            }
        }

        public decimal DbStartOfMovementY
        {
            get
            {
                return dbStartOfMovementY;
            }

            set
            {
                dbStartOfMovementY = value;
            }
        }

        public int DirectionModifyerX
        {
            get
            {
                return directionModifyerX;
            }

            set
            {
                directionModifyerX = value;
            }
        }

        public int DirectionModifyerY
        {
            get
            {
                return directionModifyerY;
            }

            set
            {
                directionModifyerY = value;
            }
        }

        public MoveDirections DirectionOfMovement
        {
            get
            {
                return directionOfMovement;
            }

            set
            {
                directionOfMovement = value;
            }
        }

        public decimal DistanceEntered
        {
            get
            {
                return distanceEntered;
            }

            set
            {
                distanceEntered = value;
            }
        }

        public float DrawingScale
        {
            get
            {
                return drawingScale;
            }

            set
            {
                drawingScale = value;
            }
        }

        public PointF EndOfCurrentLine
        {
            get
            {
                return endOfCurrentLine;
            }

            set
            {
                endOfCurrentLine = value;
            }
        }

        public PointF EndOfJumpMovePoint
        {
            get
            {
                return endOfJumpMovePoint;
            }

            set
            {
                endOfJumpMovePoint = value;
            }
        }

        public bool FirstTimeLoaded
        {
            get
            {
                return firstTimeLoaded;
            }

            set
            {
                firstTimeLoaded = value;
            }
        }

        public Brush GreenBrush
        {
            get
            {
                greenBrush = Brushes.DarkGreen;
                return greenBrush;
            }

            set
            {
                greenBrush = value;
            }
        }

        public bool IsNewSketch
        {
            get
            {
                return isNewSketch;
            }

            set
            {
                isNewSketch = value;
            }
        }

        public SMLine JumpPointLine
        {
            get
            {
                return jumpPointLine;
            }

            set
            {
                jumpPointLine = value;
            }
        }

        public List<SMLine> JumpPointLines
        {
            get
            {
                return jumpPointLines;
            }

            set
            {
                jumpPointLines = value;
            }
        }

        public string LastAngDir
        {
            get
            {
                return lastAngDir;
            }

            set
            {
                lastAngDir = value;
            }
        }

        public string LastDir
        {
            get
            {
                return lastDir;
            }

            set
            {
                lastDir = value;
            }
        }

        public List<string> LegalMoveDirections
        {
            get
            {
                if (legalMoveDirections==null)
                {
                    legalMoveDirections = new List<string>();
                }
                return legalMoveDirections;
            }

            set
            {
                legalMoveDirections = value;
            }
        }

     

        public Image MainImage
        {
            get
            {
                if (_mainImage == null)
                {
                    _mainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                }
                return _mainImage;
            }

            set
            {
                _mainImage = value;
            }
        }

        public decimal MovementDistanceScaled
        {
            get
            {
                return movementDistanceScaled;
            }

            set
            {
                movementDistanceScaled = value;
            }
        }

        public decimal NextSectArea
        {
            get
            {
                return nextSectArea;
            }

            set
            {
                nextSectArea = value;
            }
        }

        public Pen OrangePen
        {
            get
            {
                if (orangePen == null)
                {
                    orangePen = new Pen(Color.DarkOrange, 1);
                }
                return orangePen;
            }

            set
            {
                orangePen = value;
            }
        }

        public SMParcelMast ParcelMast
        {
            get
            {
                return parcelMast;
            }

            set
            {
                parcelMast = value;
            }
        }

        public Brush RedBrush
        {
            get
            {
                redBrush = Brushes.DarkRed;
                return redBrush;
            }

            set
            {
                redBrush = value;
            }
        }

        public Pen RedPen
        {
            get
            {
                if (redPen == null)
                {
                    redPen = new Pen(Color.Red, 1);
                }
                return redPen;
            }

            set
            {
                redPen = value;
            }
        }

        public PointF ScaledBeginPoint
        {
            get
            {
                return scaledBeginPoint;
            }

            set
            {
                scaledBeginPoint = value;
            }
        }

        public PointF ScaledEndOfMovement
        {
            get
            {
                return scaledEndOfMovement;
            }

            set
            {
                scaledEndOfMovement = value;
            }
        }

        public PointF ScaledJumpPoint
        {
            get
            {
                return scaledJumpPoint;
            }

            set
            {
                scaledJumpPoint = value;
            }
        }

        public PointF ScaledStartOfMovement
        {
            get
            {
                return scaledStartOfMovement;
            }

            set
            {
                scaledStartOfMovement = value;
            }
        }

        public PointF SectionAttachPoint
        {
            get
            {
                return sectionAttachPoint;
            }

            set
            {
                sectionAttachPoint = value;
            }
        }

       

        public Bitmap SketchImage
        {
            get
            {
                return sketchImage;
            }

            set
            {
                sketchImage = value;
            }
        }

        public Bitmap SketchImageBMP
        {
            get
            {
                return sketchImageBMP;
            }

            set
            {
                sketchImageBMP = value;
            }
        }

        public PointF SketchOrigin
        {
            get
            {
                return sketchOrigin;
            }

            set
            {
                sketchOrigin = value;
            }
        }

        public PointF StartOfCurrentLine
        {
            get
            {
                return startOfCurrentLine;
            }

            set
            {
                startOfCurrentLine = value;
            }
        }

        public bool UndoJump
        {
            get
            {
                return undoJump;
            }

            set
            {
                undoJump = value;
            }
        }

        private  DrawingState EditState
        {
            get
            {
    return editState;
            }

            set
            {
                  ShowState(value);
                editState = value;
              
                
            }
        }

        private void ShowState(DrawingState value)
        {
            string message = string.Empty;
            switch (value)
            {
                case DrawingState.Drawing:
                    message = string.Format("Drawing Section {0}", WorkingSection.SectionLabel);
                    break;
                case DrawingState.JumpPointSelected:
                    StringBuilder directions = new StringBuilder();
                    if (LegalMoveDirections.Count>0)
                    {

                    }
                    int counter = 1;
                    foreach (string s in LegalMoveDirections.Distinct())
                    {
                        if (counter < LegalMoveDirections.Count)
                        {
                            directions.AppendFormat("{0}, ", s);
                        }
                        else
                        {
                            directions.AppendFormat("{0}", s);
                        }
                        counter++;
                    }

                 
                    message = string.Format("Move ({0}) to set start, or click \"Begin\" to start drawing the section here.",directions.ToString());
                    break;
                case DrawingState.SectionAdded:
                    message = string.Format("Added Section {0}. Right-click to select the nearest corner as a reference point.", WorkingSection.SectionLabel);
                    break;
                case DrawingState.SketchLoaded:
                    message = string.Format("Ready to edit the sketch of record {0}", SketchUpGlobals.ParcelWorkingCopy.Record);
                    break;
                default:
                    break;
            }
            DisplayStatus(message);
        }

        private List<PointF> NewSectionPoints
        {
            get
            {
                if (_newSectionPoints == null)
                    _newSectionPoints = new List<PointF>();

                return _newSectionPoints;
            }

            set
            {
                _newSectionPoints = value;
            }
        }

        public SMSection WorkingSection
        {
            get
            {
                workingSection = SketchUpGlobals.ParcelWorkingCopy.SelectSectionByLetter(LastSectionLetter);
                return workingSection;
            }

            set
            {
                workingSection = value;
            }
        }

        public string LastSectionLetter
        {
            get
            {
                lastSectionLetter = SketchUpGlobals.ParcelWorkingCopy.LastSectionLetter;
                return lastSectionLetter;
            }

            set
            {
                lastSectionLetter = value;
            }
        }

        #endregion Properties
    }
}