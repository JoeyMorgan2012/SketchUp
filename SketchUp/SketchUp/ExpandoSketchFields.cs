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
        #region "Properties"

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

        public PointF DbEndPoint
        {
            get
            {
                return dbEndPoint;
            }

            set
            {
                dbEndPoint = value;
            }
        }

        public decimal DbEndX
        {
            get
            {
                return dbEndX;
            }

            set
            {
                dbEndX = value;
            }
        }

        public decimal DbEndY
        {
            get
            {
                return dbEndY;
            }

            set
            {
                dbEndY = value;
            }
        }

        public PointF DbJumpPoint
        {
            get
            {
                return dbJumpPoint;
            }

            set
            {
                dbJumpPoint = value;
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

        public PointF DbStartPoint
        {
            get
            {
                return dbStartPoint;
            }

            set
            {
                dbStartPoint = value;
            }
        }

        public decimal DbStartX
        {
            get
            {
                return dbStartX;
            }

            set
            {
                dbStartX = value;
            }
        }

        public decimal DbStartY
        {
            get
            {
                return dbStartY;
            }

            set
            {
                dbStartY = value;
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

        public List<string> LegalMoveDirections
        {
            get
            {
                if (legalMoveDirections == null)
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
                    _mainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
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

        public PointF ScaledEndPoint
        {
            get
            {
                return scaledEndPoint;
            }

            set
            {
                scaledEndPoint = value;
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

        public PointF ScaledStartPoint
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

        public bool UnsavedChangesExist
        {
            get
            {
                return unsavedChangesExist;
            }

            set
            {
                unsavedChangesExist = value;
            }
        }

        public SMParcel WorkingParcel
        {
            get
            {
                return workingParcel;
            }

            set
            {
                workingParcel = value;
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

        private DrawingState EditState
        {
            get
            {
                return editState;
            }

            set
            {
                SetButtonStates(value);
                ShowState(value);
                editState = value;
            }
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

        #endregion "Properties"

        #region "Public Fields"

        public static bool _cantSketch = false;
        public static bool _deleteMaster = false;
        public static bool _deleteThisSketch = false;
        public static bool _insertLine = false;
        public static bool _isClosed = false;
        public static bool _undoMode = false;
        public static int finalClick;
        public static bool RefreshEditImageBtn = false;
        public bool _addSection = false;

        // TODO: Remove if not needed:	 private Image _baseImage;
        public decimal _calcNextSectArea = 0;

        public bool _closeSketch = false;
        public bool _hasMultiSection = false;
        public bool _hasNewSketch;
        public bool _hasSketch = false;

        // TODO: Remove if not needed:	    private string _priorDirection = "";
        // TODO: Remove if not needed:	   private bool _reOpenSec = false;
        // TODO: Remove if not needed:	    private int _savedAttLine;
        // TODO: Remove if not needed:	    private string _savedAttSection = "";
        // TODO: Remove if not needed:	     private Dictionary<int, float> _StartX = null;
        // TODO: Remove if not needed:	   private Dictionary<int, float> _StartY = null;
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
        public int garageCount = 0;

        // TODO: Remove if not needed:	  private List<int> GarCodes = null;
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

        #endregion "Public Fields"

        #region "Private Fields"

        // TODO: Remove if not needed:	 private List<String> carportTypes = null;
        private static bool checkDirection = false;

        // TODO: Remove if not needed:	 private DataTable DupAttPoints = null;
        private static DrawingState editState = DrawingState.SketchLoaded;

        private static string nextSectLtr = String.Empty;

        // TODO: Remove if not needed:	     private SWallTech.CAMRA_Connection _conn = null;
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

        // TODO: Remove if not needed:	  private int _newIndex = 0;
        private List<PointF> _newSectionPoints;

        private int _nextLineCount = 0;
        private string _nextSectType = String.Empty;
        private decimal _nextStoryHeight = 0;
        private bool _openForm = false;
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

        // TODO: Remove if not needed:	   private int AttLineNo = 0;
        private string AttSectLtr = String.Empty;

        private string AttSpLineDir = String.Empty;
        private int AttSpLineNo = 0;
        private float BaseX = 0;
        private float BaseY = 0;
        private Brush blackBrush;
        private Brush blueBrush;
        private Pen bluePen;
        private List<int> carportCodes = null;

        //Undo uses this but we are re-doing undo. JMM 3-15-2016
        private Color colorRed = Color.Red;

        private int currentAttachmentLine = 0;
        private string CurrentAttDir = String.Empty;
        private string CurrentSecLtr = String.Empty;
        private PointF dbEndPoint;
        private decimal dbEndX;
        private decimal dbEndY;

        // TODO: Remove if not needed:	    private DataTable SectionTable = null;

        //  private static List<int> savcnt;
        private PointF dbJumpPoint;

        private decimal dbLineLengthX;
        private decimal dbLineLengthY;
        private PointF dbStartPoint;
        private decimal dbStartX;
        private decimal dbStartY;
        private int directionModifyerX = 1;
        private int directionModifyerY = 1;
        private MoveDirections directionOfMovement;
        private DataTable displayDataTable = null;
        private decimal distanceEntered;
        private float drawingScale = 1.0f;
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

        // TODO: Remove if not needed:	      private List<String> GarTypes = null;
        private Brush greenBrush;

        private bool isInAddNewPointMode = false;

        // TODO: Remove if not needed:	   private bool isLastLine;
        private bool isNewSketch = false;

        private SMLine jumpPointLine;
        private List<SMLine> jumpPointLines;
        private DataTable JumpTable = null;
        private float JumpX = 0;
        private float JumpY = 0;
        private string lastAngDir = string.Empty;
        private string lastDir = string.Empty;
        private string lastSectionLetter;
        private List<string> legalMoveDirections;
        private int lineCnt = 0;
        private string Locality = String.Empty;
        private string midDirect = String.Empty;
        private int midLine = 0;
        private string midSection = String.Empty;
        private decimal movementDistanceScaled;
        private DataTable MultiplePoints = null;
        private int mylineNo = 0;
        private decimal nextSectArea = 0;
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
        private Brush redBrush;
        private Pen redPen;
        private DataTable RESpJumpTable = null;

        //   private Dictionary<int, byte[]> savpic = null;
        private float ScaleBaseX = 0;

        private float ScaleBaseY = 0;
        private PointF scaledBeginPoint;
        private PointF scaledEndPoint;
        private PointF scaledJumpPoint;
        private float SecBeginX = 0;
        private float SecBeginY = 0;
        private List<String> SecLetters = null;
        private PointF sectionAttachPoint;
        private string SketchCard = String.Empty;
        private string SketchFolder = String.Empty;
        private Bitmap sketchImage;
        private Bitmap sketchImageBMP;
        private PointF sketchOrigin;
        private string SketchRecord = String.Empty;
        private DataTable sortDist = null;

        // TODO: Remove if not needed:	  private decimal splitLineDist = 0;
        // TODO: Remove if not needed:	     private int StandardDrawWidthAndHeight = 3;
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

        // TODO: Remove if not needed:	  private int TempAttSplineNo = 0;
        //  TODO: Remove if not needed:
        private decimal txtLoc = 0;

        private float txtLocf = 0;
        private float txtX = 0;
        private float txtY = 0;

        // TODO: Remove if not needed:	    private Point[] unadj_pts;
        private bool undoJump = false;

        private bool unsavedChangesExist;
        private SMParcel workingParcel;
        private SMSection workingSection;

        #endregion "Private Fields"

        #region "Enums"

        public enum DrawingState
        {
            DoneDrawing,
            Drawing,
            JumpPointSelected,
            SectionAdded,
            SectionDeleted,
            SectionEditCompleted,
            SectionEditStarted,
            SketchLoaded,
            SketchSaved
        }

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

        #endregion "Enums"

        #region constants and pseudo-constants

        private const int sketchBoxPaddingTotal = 20;
        private Bitmap AddImage = Properties.Resources.AddSection;
        private Bitmap BeginDrawingImage = Properties.Resources.Edit_32xMD;
        private Bitmap CloseSectionImage = Properties.Resources.CloseSection;
        private Bitmap DoneSketchingImage = Properties.Resources.GreenCheck;
        private Bitmap EditSectionsImage = Properties.Resources.EditImage;
        private Bitmap EditUndoLineImage = Properties.Resources.Undo_grey_32x;
        private Bitmap ExitSketchImage = Properties.Resources.Close_16x;
        private Bitmap FileAddSectionImage = Properties.Resources.AddSection;
        private Bitmap FileCloseNoSaveImage = Properties.Resources.DeleteListItem_32x;
        private Bitmap FileEditSectionImage = Properties.Resources.EditImage;
        private Bitmap FlipHorizontallyImage = Properties.Resources.FlipHorizontalImage;
        private Bitmap FlipVeritcallyImage = Properties.Resources.FlipVerticalImage;
        private Bitmap JumpToCornerImage = Properties.Resources.JumpPointImage;
        private Bitmap SaveAndCloseImage = Properties.Resources.Save;
        private Bitmap SaveAndContinueImage = Properties.Resources.SaveAndContinue;
        private Bitmap SaveDrawingImage = Properties.Resources.Save;
        private Bitmap UnDoImage = Properties.Resources.Undo_grey_32x;

        #endregion constants
    }
}