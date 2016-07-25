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

        public static string NextSectLtr { get; set; } = string.Empty;

        public SMSection AttachmentSection { get; set; }

        public PointF DbJumpPoint { get; set; }

        public PointF DbStartPoint { get; set; }

        public double DbStartX
        {
            get { return dbStartX; }

            set { dbStartX = value; }
        }

        public double DbStartY
        {
            get { return dbStartY; }

            set { dbStartY = value; }
        }

        public bool IsNewSketch
        {
            get { return isNewSketch; }

            set { isNewSketch = value; }
        }

        public SMLine JumpPointLine
        {
            get { return jumpPointLine; }

            set { jumpPointLine = value; }
        }

        public string LastSectionLetter
        {
            get {
                lastSectionLetter = SketchUpGlobals.ParcelWorkingCopy.LastSectionLetter;
                return lastSectionLetter;
            }
        }

        public List<string> LegalMoveDirections
        {
            get {
                if (legalMoveDirections == null)
                {
                    legalMoveDirections = new List<string>();
                }
                return legalMoveDirections;
            }

            set { legalMoveDirections = value; }
        }

        public Image MainImage
        {
            get {
                if (_mainImage == null)
                {
                    _mainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                }
                return _mainImage;
            }

            set { _mainImage = value; }
        }

        public SMParcelMast ParcelMast
        {
            get { return parcelMast; }

            set { parcelMast = value; }
        }


        public Brush RedBrush => Brushes.DarkRed;

        public Pen RedPen => new Pen(Color.Red, 1);


        public PointF ScaledJumpPoint { get; set; }

        public PointF ScaledStartPoint { get; set; }

        public PointF StartOfCurrentLine { get; set; }

        public bool UndoJump { get; set; } = false;

        public bool UnsavedChangesExist
        {
            get { return unsavedChangesExist; }

            set {
                unsavedChangesExist = value;
                if (!value)
                {
                    DisplayStatus("Editing");
                }
                else
                {
                    DisplayStatus("Ready");
                }
            }
        }

        public SMParcel WorkingParcel { get; set; }

        public SMSection WorkingSection
        {
            get {
                if (EditState == DrawingState.Drawing || EditState == DrawingState.SectionAdded)
                {
                    workingSection = SketchUpGlobals.ParcelWorkingCopy.SelectSectionByLetter(LastSectionLetter);
                }
                return workingSection;
            }

            set { workingSection = value; }
        }

        public DrawingState EditState
        {
            get { return editState; }

            set {
                editState = value;
                SetButtonStates();
                ShowEditStatus();
                Application.DoEvents();
            }
        }

        private List<PointF> NewSectionPoints
        {
            get {
                if (_newSectionPoints == null)
                    _newSectionPoints = new List<PointF>();

                return _newSectionPoints;
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
        // TODO: Remove if not needed:  public static int finalClick;
        public static bool RefreshEditImageBtn = false;
        public bool _addSection = false;


        public double _calcNextSectArea = 0;

        public bool _closeSketch = false;
        public bool _hasMultiSection = false;
        public bool _hasNewSketch;
        public bool _hasSketch = false;

        public bool _undoLine = false;

        public bool _vacantParcelSketch = false;
        public float BeginSplitX = 0;
        public float BeginSplitY = 0;
        public double begSplitX = 0;
        public double begSplitY = 0;
        public int carportCount = 0;
        public string ConnectSec = string.Empty;
        public double CPSize = 0;
        public int CurSecLineCnt = 0;
        public SWallTech.CAMRA_Connection dbConn = null;
        public float delStartX = 0;
        public float delStartY = 0;
        public double distance = 0;
        public float distanceD = 0;
        public float distanceDXF = 0;
        public float distanceDYF = 0;
        public bool draw = false;
        public double EndSplitX = 0;
        public float EndSplitXF = 0;
        public double EndSplitY = 0;
        public float EndSplitYF = 0;
        public int garageCount = 0;

        public decimal GarSize = 0M;
        public float NextStartX = 0;
        public float NextStartY = 0;
        public string offsetDir = string.Empty;
        public double OrigLineLength = 0;
        public double OrigStartX = 0;
        public double OrigStartY = 0;
        public float PrevStartX = 0;
        public float PrevStartY = 0;
        public double RemainderLineLength = 0;
        public int SecItemCnt = 0;
        public int SecLineCnt = 0;
        public double startSplitX = 0;
        public double startSplitY = 0;
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

        private static DrawingState editState = DrawingState.SketchLoaded;


        private bool _isAngle = false;

        private float _currentScale = 0;
        private bool _isJumpMode = false;
        private bool _isKeyValid = false;
        private string _lenString = string.Empty;
        private Image _mainImage;
        private int _mouseX;
        private int _mouseY;



        private List<PointF> _newSectionPoints;

        private int _nextLineCount = 0;

        private string SketchRecord = string.Empty;

        private bool isNewSketch = false;
        // TODO: Remove if not needed:   private Brush redBrush;
        private Color colorRed = Color.Red;
        private DataTable MultiplePoints = null;
        private double dbStartX;
        private double dbStartY;
        private double _nextStoryHeight = 0;
        private float ScaleBaseX = 0;
        private float ScaleBaseY = 0;
        private int currentAttachmentLine = 0;
        private int lineCnt = 0;
        // TODO: Remove if not needed:  private List<string> FixSect = null;
        private List<string> legalMoveDirections;
        private List<string> SecLetters = null;
        // TODO: Remove if not needed:    private Pen redPen;
        private SMLine jumpPointLine;
        private SMParcelMast parcelMast;
        private string AttSectLtr = string.Empty;
        private string AttSpLineDir = string.Empty;
        private string CurrentAttDir = string.Empty;
        private string CurrentSecLtr = string.Empty;
        private string lastAngDir = string.Empty;
        private string lastDir = string.Empty;
        private string lastSectionLetter;
        private string Locality = string.Empty;
        private string midDirect = string.Empty;
        private string midSection = string.Empty;
        private string OffSetAttSpLineDir = string.Empty;
        private string SketchCard = string.Empty;
        private string SketchFolder = string.Empty;
        private string _nextSectType = string.Empty;



        // TODO: Remove if not needed:	  ------------------------------------
        //private int TempAttSplineNo = 0;
        //private float StartX = 0;
        //private float StartY = 0;
        //private DataTable StrtPts = null;
        //private double txtLoc = 0;
        //private float txtLocf = 0;
        //private float txtX = 0;
        //private float txtY = 0;
        //private double StrxD = 0;
        //private double StryD = 0;
        //private Point[] unadj_pts;

        private bool unsavedChangesExist;
        private SMSection workingSection;

        #endregion "Private Fields"

        #region "Enums"

        public enum DrawingState
        {
            DoneDrawing,
            Drawing,
            Flipping,
            JumpPointSelected,
            LoadingEditForm,
            NewSketch,
            SaveError,
            Saving,
            SectionAdded,
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

        // TODO: Remove if not needed:  private const int sketchBoxPaddingTotal = 20;
        private Bitmap AddImage = Properties.Resources.AddSection;
        private Bitmap BeginDrawingImage = Properties.Resources.MediumBlueEditPencil;
        private Bitmap CloseSectionImage = Properties.Resources.CloseSection;
        private Bitmap EditSectionsImage = Properties.Resources.EditImage;
        private Bitmap EditUndoLineImage = Properties.Resources.Undo_grey_32x;
        private Bitmap ExitSketchImage = Properties.Resources.Close_16x;
        private Bitmap FileAddSectionImage = Properties.Resources.AddSection;
        private Bitmap FileCloseNoSaveImage = Properties.Resources.DeleteListItem_32x;
        private Bitmap FileEditSectionImage = Properties.Resources.EditImage;
        private Bitmap FlipHorizontallyImage = Properties.Resources.FlipHorizontalImage;
        private Bitmap FlipVeritcallyImage = Properties.Resources.FlipVerticalImage;
        private Bitmap GreenCheckImage = Properties.Resources.GreenCheckCircle;
        private Bitmap AsteriskImage = Properties.Resources.Asterisk;
        private Bitmap JumpToCornerImage = Properties.Resources.JumpPointImage;
        private Bitmap SaveAndCloseImage = Properties.Resources.Save;
        private Bitmap SaveAndContinueImage = Properties.Resources.SaveAndContinue;
        private Bitmap SaveDrawingImage = Properties.Resources.Save;
        private Bitmap UnDoImage = Properties.Resources.Undo_grey_32x;

        #endregion constants and pseudo-constants
    }
}