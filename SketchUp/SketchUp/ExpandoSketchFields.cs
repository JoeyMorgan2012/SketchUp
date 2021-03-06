﻿using SWallTech;
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

		private decimal adjNewSecX = 0;
		private decimal adjNewSecY = 0;
		private decimal adjOldSecX = 0;
		private decimal adjOldSecY = 0;
		private decimal AngD1 = 0;
		private decimal AngD2 = 0;
		private DataTable AreaTable = null;
		private DataTable AttachmentPointsDataTable = null;
		private DataTable AttachPoints = null;
		private int AttLineNo = 0;
		private string AttSectLtr = String.Empty;
		private string AttSpLineDir = String.Empty;
		private int AttSpLineNo = 0;
		private float BaseX = 0;
		private float BaseY = 0;
		private static bool checkDirection = false;

		// TODO: Remove if not needed:	
		private int click = 0;

		//Undo uses this but we are re-doing undo. JMM 3-15-2016
		private Color color = Color.Red;
		private List<int> cpCodes = null;
		private List<String> cpTypes = null;
		private int currentAttachmentLine = 0;
		private string CurrentAttDir = String.Empty;
		private string CurrentSecLtr = String.Empty;
		private DataTable dt = null;
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
		private string legalMoveDirection;
		private int lineCnt = 0;
		private int LineNumberToBreak = 0;
		private string Locality = String.Empty;
		private string midDirect = String.Empty;
		private int midLine = 0;
		private string midSection = String.Empty;
		private byte[] ms = null;
		private DataTable MulPts = null;
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
		SMParcel parcelWorkingCopy;
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
		private Dictionary<int, byte[]> savpic = null;
		private float ScaleBaseX = 0;
		private float ScaleBaseY = 0;
		private float SecBeginX = 0;
		private float SecBeginY = 0;
		private List<String> SecLetters = null;
		private BuildingSection section;
		private DataTable SectionLtrs = null;
		private DataTable SectionTable = null;
		private string SketchCard = String.Empty;
		private string SketchFolder = String.Empty;
		/* 
		Refactored by renaming and providing for null values. Going to ensure that the
		naming conventions are consistent for all properties. Any field that backs a property
		will be in camel case (e.g. camelCase) while fields that are not property-backing will be
		in Pascal case. (e.g. PascalCase).
		
		*/
		SMParcel sketchMgrParcel;
		private string SketchRecord = String.Empty;
		private DataTable sortDist = null;
		private decimal splitLineDist = 0;
		private int StandardDrawWidthAndHeight = 3;
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
		// TODO: Remove if not needed:	private SWallTech.CAMRA_Connection dbConnection = null;
		private int currentLineCount = 0;
		private ParcelData currentParcel = null;
		private float currentScale = 0;
		private SectionDataCollection _currentSection = null;
		private bool isAngle = false;
		private bool isClosing = false;
		private bool jumpModeActive = false;
		private bool isValidKey = false;
		private string lengthLabelString = String.Empty;
		private int newIndex = 0;
		private List<PointF> _newSectionPoints;
		private int nextLineCount = 0;
		private string nextSectionType = String.Empty;
		private decimal nextStoryHeight = 0;
		private bool openForm = false;
		private string priorDirection = "";
		private bool reopenTheSection = false;
		private int savedAttachmentLine;
		private string savedAttachmentSection = "";
		private float drawingScale = 1.0f;
		private Dictionary<int, float> startPointX = null;
		private Dictionary<int, float> startPointY = null;

		#endregion private fields

		#region Public Fields

		public float BeginSplitX = 0;
		public float BeginSplitY = 0;
		public decimal begSplitX = 0;
		public decimal begSplitY = 0;
		public string ConnectSec = String.Empty;
		public int CPcnt = 0;
		public decimal CPSize = 0;
		public int CurSecLineCnt = 0;
		public CAMRA_Connection dbConn = null;
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
		public float xAdjustment = 0;
		public float XadjP = 0;
		public float yAdjustment = 0;
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
		public string lastLineDirection = String.Empty;
		public decimal _nextSectArea = 0;
		public static string nextSectionLetter = String.Empty;
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

		public SMParcel ParcelWorkingCopy
		{
			get
			{
				return parcelWorkingCopy;
			}
			set
			{
				parcelWorkingCopy = value;
			}
		}

		public SMParcel SketchMgrParcel
		{
			get
			{
				return sketchMgrParcel;
			}
			set
			{
				sketchMgrParcel = value;
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

		public string SavedAttachmentSection
		{
			get
			{
				return savedAttachmentSection;
			}

			set
			{
				savedAttachmentSection = value;
			}
		}

		#endregion Properties
	}
}
