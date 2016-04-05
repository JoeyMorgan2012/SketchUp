using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;
namespace SketchUp
{
	public partial class ExpandoSketch
	{
		#region fields
		float BaseX = 0;
		float BaseY = 0;

		float JumpX = 0;
		float JumpY = 0;

		float pt2X = 0;
		float pt2Y = 0;
		public float Xadj = 0;
		public float Yadj = 0;
		public float XadjP = 0;
		public float YadjP = 0;

		decimal XadjR = 0;
		decimal YadjR = 0;
		int mylineNo = 0;

		decimal Xadj1 = 0;
		decimal Yadj1 = 0;

		decimal begNewSecX = 0;
		decimal begNewSecY = 0;
		decimal adjNewSecX = 0;
		decimal adjNewSecY = 0;
		decimal adjOldSecX = 0;
		decimal adjOldSecY = 0;

		float endOldSecX = 0;
		float endOldSecY = 0;
		decimal splitLineDist = 0;
		decimal NewSplitLIneDist = 0;
		int breakLineNbr = 0;
		int lineCnt = 0;
		int AttLineNo = 0;
		int AttSpLineNo = 0;
		int TempAttSplineNo = 0;
		string AttSpLineDir = String.Empty;
		string OffSetAttSpLineDir = String.Empty;
		int CurrentAttLine = 0;
		string CurrentAttDir = String.Empty;
		int _newIndex = 0;
		int _curLineCnt = 0;
		float PrevX = 0;
		float PrevY = 0;
		float EndX = 0;
		float EndY = 0;
		float txtLocf = 0;
		decimal txtLoc = 0;
		float txtX = 0;
		float txtY = 0;
		string _lenString = String.Empty;
		float SecBeginX = 0;
		float SecBeginY = 0;
		string AttSectLtr = String.Empty;
		float ScaleBaseX = 0;
		float ScaleBaseY = 0;
		Image _baseImage;
		SWallTech.CAMRA_Connection _conn = null;
		//ParcelData _currentParcel = null;
		//SectionDataCollection _currentSection = null;
		Image _mainimage;
		int _mouseX;
		int _mouseY;
		float _scale = 1.0f;
		string Locality = String.Empty;
	//	BuildingSection section;
		string SketchCard = String.Empty;
		string SketchFolder = String.Empty;
		string SketchRecord = String.Empty;
		float StartX = 0;
		float StartY = 0;
		string _nextSectType = String.Empty;
		decimal _nextStoryHeight = 0;
		int _nextLineCount = 0;
		float _currentScale = 0;
		bool _isAngle = false;
		bool _isclosing = false;
		bool _isJumpMode = false;
		bool _isKeyValid = false;
		List<PointF> _newSectionPoints;
		bool _openForm = false;
		string _priorDirection = "";
		bool _reOpenSec = false;
		int _savedAttLine;
		string _savedAttSection = "";
		Dictionary<int, float> _StartX = null;
		Dictionary<int, float> _StartY = null;
		decimal AngD1 = 0;
		decimal AngD2 = 0;
		DataTable AreaTable = null;
		DataTable AttachPoints = null;
		DataTable AttPts = null;
		bool checkRedraw = false;
		int click = 0;
		Color color = Color.Red;
		List<int> CPCodes = null;
		List<String> CPTypes = null;
		string CurrentSecLtr = String.Empty;
		DataTable dt = null;
		DataTable DupAttPoints = null;
		decimal EndxD = 0;
		decimal EndyD = 0;
		List<string> FixSect = null;
		List<int> GarCodes = null;
		List<String> GarTypes = null;
		bool isInAddNewPointMode = false;
		bool isLastLine;
		DataTable JumpTable = null;
		string midDirect = String.Empty;
		int midLine = 0;
		string midSection = String.Empty;
		byte[] ms = null;
		DataTable MulPts = null;
		int NewPointIndex;
		decimal prevPt2X = 0;
		decimal prevPt2Y = 0;
		decimal prevTst1 = 0;
		decimal prevTst2 = 0;
		Point[] pts;
		DataTable REJumpTable = null;
		DataTable RESpJumpTable = null;
		int s = 3;
		Dictionary<int, byte[]> savpic = null;
		List<String> SecLetters = null;
		DataTable SectionLtrs = null;
		DataTable SectionTable = null;
		DataTable sortDist = null;
		DataTable StrtPts = null;
		decimal StrxD = 0;
		decimal StryD = 0;
		Point[] unadj_pts;
		static List<int> savcnt;

		#endregion fields

		#region Enums

		enum MoveDirections
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

		#region refactored methods

		string ReverseDirection(string direction)
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
				default:
					Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}.\n{2} is not a valid direction value.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, direction));
					break;
			}

			return reverseDirection;
		}

		#endregion refactored methods
	}
}
