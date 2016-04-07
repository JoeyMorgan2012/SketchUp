using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SketchUp
{
	public partial class ExpandoSketch : Form
	{
		#region Enums

		private enum MoveDirections
		{
			N,
			NE,
			E,
			SE,
			S,
			SW,
			W,
			NW
		}

		#endregion Enums

		#region Fields

		#region private fields
		/* 
		Refactored by renaming and providing for null values. Going to ensure that the
		naming conventions are consistent for all properties. Any field that backs a property
		will be in camel case (e.g. camelCase) while fields that are not property-backing will be
		in Pascal case. (e.g. PascalCase).

		*/
		private DataTable AreaTable = null;
		private DataTable AttachPoints = null;
		private DataTable AttachmentPointsDataTable = null;
		private float BaseX = 0;
		private float BaseY = 0;
		private decimal NewSectionBeginPointX = 0;
		private decimal NewSectionBeginPointY = 0;
		private int LineNumberToBreak = 0;

		//private decimal Xadj1 = 0;
		//private decimal Yadj1 = 0;
		//public static bool _undoModeA = false;
		private bool NeedToRedraw = false;

		// TODO: Remove if not needed:	
		private int click = 0;
		//Undo uses this but we are re-doing undo. JMM 3-15-2016
		private Color color = Color.Red;
		private List<int> cpCodes = null;
		private List<String> cpTypes = null;
		private int currentAttachmentLine = 0;
		private DataTable displayDataTable = null;
		private DataTable DupAttPoints = null;
		private float endOldSecX = 0;
		private float endOldSecY = 0;
		private float EndX = 0;
		private decimal EndxD = 0;
		private float EndY = 0;
		private decimal EndyD = 0;
		private List<string> FixSect = null;
		private List<int> GarCodes = null;
		private List<String> GarTypes = null;
		private bool isInAddNewPointMode = false;
		private bool isLastLine;
		private DataTable JumpTable = null;
		private float JumpX = 0;
		private float JumpY = 0;
		private int lineCnt = 0;
		private int midLine = 0;
		private byte[] ms = null;
		private DataTable MulPts = null;
		private int mylineNo = 0;
		private int NewPointIndex;
		private decimal NewSplitLIneDist = 0;
		private decimal prevPt2X = 0;
		private decimal prevPt2Y = 0;
		private decimal prevTst1 = 0;
		private decimal prevTst2 = 0;
		private float PrevX = 0;
		private float PrevY = 0;
		private float pt2X = 0;
		private float pt2Y = 0;
		private Point[] pts;
		private DataTable REJumpTable = null;
		private DataTable RESpJumpTable = null;
		private int StandardDrawWidthAndHeight = 3;
		private Dictionary<int, byte[]> savpic = null;
		private float ScaleBaseX = 0;
		private float ScaleBaseY = 0;
		private float SecBeginX = 0;
		private float SecBeginY = 0;
		private List<String> SecLetters = null;
		private BuildingSection section;
		private DataTable SectionLtrs = null;
		private DataTable SectionTable = null;
		private DataTable sortDist = null;
		private decimal splitLineDist = 0;
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
		private int _curLineCnt = 0;
		private ParcelData _currentParcel = null;
		private float _currentScale = 0;
		private bool _isAngle = false;
		private bool _isclosing = false;
		private bool _isJumpMode = false;
		private bool _isKeyValid = false;
		private int _newIndex = 0;
		private List<PointF> _newSectionPoints;
		private int _nextLineCount = 0;
		private decimal _nextStoryHeight = 0;
		private bool _openForm = false;
		private bool _reOpenSec = false;
		private int _savedAttLine;
		private float _scale = 1.0f;
		private Dictionary<int, float> _StartX = null;
		private Dictionary<int, float> _StartY = null;

		private int AttLineNo = 0;
		private int AttSpLineNo = 0;

		private string AttSpLineDir = String.Empty;
		private string OffSetAttSpLineDir = String.Empty;

		private string AttSectLtr = String.Empty;
		private static bool checkDirection = false;
		private string CurrentAttDir = String.Empty;
		private string CurrentSecLtr = String.Empty;
		private string legalMoveDirection;
		private string Locality = String.Empty;
		private string midDirect = String.Empty;
		private string midSection = String.Empty;

		private string SketchCard = String.Empty;
		private string SketchFolder = String.Empty;
		private string SketchRecord = String.Empty;
		private SWallTech.CAMRA_Connection _conn = null;
		private SectionDataCollection _currentSection = null;
		private string _lenString = String.Empty;
		private string _nextSectType = String.Empty;
		private string _priorDirection = "";
		private string _savedAttSection = "";
		private decimal adjNewSecX = 0;
		private decimal adjNewSecY = 0;
		private decimal AngD1 = 0;
		private decimal AngD2 = 0;
		private decimal adjOldSecX = 0;
		private decimal adjOldSecY = 0;

		#endregion private fields

		#region Public Fields

		public string ConnectSec = String.Empty;
		public float BeginSplitX = 0;
		public float BeginSplitY = 0;
		public decimal begSplitX = 0;
		public decimal begSplitY = 0;

		public int CPcnt = 0;
		public decimal CPSize = 0;
		public int CurSecLineCnt = 0;
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
		public static int finalClick;
		public SWallTech.CAMRA_Connection dbConn = null;
		public int Garcnt = 0;
		public decimal GarSize = 0;
		public static List<string> Letters = new List<string>() { "A", "B", "C", "D", "F", "G", "H", "I", "J", "K", "L", "M" };
		public float NextStartX = 0;
		public float NextStartY = 0;
		public string offsetDir = String.Empty;
		public decimal OrigLineLength = 0;
		public decimal OrigStartX = 0;
		public decimal OrigStartY = 0;
		public float PrevStartX = 0;
		public float PrevStartY = 0;
		public static bool RefreshEditImageBtn = false;
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
		public bool _addSection = false;
		public decimal _calcNextSectArea = 0;
		public static bool _cantSketch = false;
		public bool _closeSketch = false;
		public static bool _deleteMaster = false;
		public static bool _deleteThisSketch = false;
		public bool _hasMultiSection = false;
		public bool _hasNewSketch;
		public bool _hasSketch = false;
		public static bool _insertLine = false;
		public static bool _isClosed = false;
		public bool _isNewSketch = false;
		public string _lastAngDir = String.Empty;
		public string _lastDir = String.Empty;
		public decimal _nextSectArea = 0;
		public static string _nextSectLtr = String.Empty;
		public bool _undoJump = false;
		public bool _undoLine = false;
		public static bool _undoMode = false;
		public bool _vacantParcelSketch = false;

		#endregion Public Fields

		#endregion Fields

		#region Properties

		private static List<int> savcnt;
		private Image _baseImage;
		private Image _mainimage;
		private int _mouseX;
		private int _mouseY;

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

		public BuildingSection SketchSection
		{
			get
			{
				return this.section;
			}

			set
			{
				this.section = value;
				this.unadj_pts = this.section.SectionPoints;
				this.LoadSection();
			}
		}

		public List<int> CpCodes
		{
			get
			{
				if (cpCodes==null)
				{
					cpCodes = new List<int>();
				}
				return cpCodes;
			}

			set
			{
				cpCodes = value;
			}
		}

		#endregion Properties
	}
}