using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class ParcelData
	{
		#region public fields (properties)

		public decimal BaseChange;
		public int calcsubt;
		public int Card;

		// extra fields not in MAST Tabel
		public decimal computedFactor;

		public int CurrentValue;
		public int curVal1;
		public int curVal2;
		public decimal depreciatonValue;
		public decimal econDeprValue;
		public decimal factoredValue;
		public string FloorTyp2;
		public string FloorTyp3;
		public string FloorTyp4;
		public string FloorType;
		public decimal funcDeprValue;
		public int GasLogFP;
		public GasLogCollection GasLogRecords;
		public ImprDataCollection ImprRecords;

		//public SalesHistoryCollection SaleHistory;
		//public NoteDataCollection ParcelNotes;
		//public AttachedMapCollection AttachedMap;
		public InteriorImprovementCollection InteriorImpRecords;

		public string InteriorWallTyp2;
		public string InteriorWallTyp3;
		public string InteriorWallTyp4;
		public string InteriorWallType;
		public LandDataCollection LandRecords;
		public string m0depr;
		public decimal m1adj;
		public int m1area;
		public decimal m1dfac;
		public decimal m1dpth;
		public decimal m1frnt;
		public decimal m1rate;
		public string m1um;
		public decimal m2adj;
		public int m2area;
		public decimal m2dfac;
		public decimal m2dpth;
		public decimal m2frnt;
		public decimal m2rate;
		public string m2um;
		public string mac;
		public int macct;
		public decimal macpct;
		public string macre;
		public decimal macreN;
		public int macsf;
		public string madd1;
		public string madd2;
		public int mage;
		public string mascom;
		public int massb;
		public int massl;
		public int masslu;
		public int massm;
		public string mathom;
		public decimal mbasa;
		public int mbastp;
		public int mbiNc;
		public decimal mbrate;
		public int mcada;
		public string mcalc;
		public int mcamo;
		public int mcarNc;
		public int mcarpt;
		public int mcayr;
		public int mcdda;
		public int mcdmo;
		public string mcdr;
		public int mcdrdt;
		public int mcdyr;
		public int mchar;
		public string mcity;
		public string mclass;
		public string mcnst1;
		public string mcnst2;
		public string mcomm1;
		public string mcomm2;
		public string mcomm3;
		public string mcond;
		public int mcvda;
		public string mcvexp;
		public int mcvmo;
		public int mcvyr;
		public int mdasld;
		public int mdatlg;
		public int mdatpr;
		public string mdbook;
		public string mdcode;
		public string mdelay;
		public decimal mdeprc;
		public string mdesc1;
		public string mdesc2;
		public string mdesc3;
		public string mdesc4;
		public string mdirct;
		public int mdpage;
		public string mdsufx;
		public int mdwell;
		public decimal meacre;
		public int mease;
		public decimal mecond;
		public int meffag;
		public int mekit;
		public string melec;
		public int metxyr;
		public int mexwl2;
		public int mexwll;
		public decimal mfactr;
		public int mfairv;
		public string mfill4;
		public string mfill7;
		public string mfill9;
		public int mflN;
		public string mfloor;
		public string mflutp;
		public string mfnam;
		public int mfound;
		public string mfp1;
		public string mfp2;
		public int mfpN;
		public int mfuel;
		public decimal mfuncd;
		public int mgarN2;
		public int mgarNc;
		public int mgart;
		public int mgart2;
		public string mgas;
		public string mgispn;
		public string mgrntr;
		public int mheat;
		public string mhidnm;
		public string mhidpc;
		public int mhrdat;
		public string mhrnam;
		public int mhrphN;
		public string mhrses;
		public int mhrtim;
		public int mhseN;
		public string mhseN2;
		public int mimadj;
		public int mimprv;
		public string minit;
		public int minno2;
		public int minnoN;
		public int minspd;
		public string mintyp;
		public int mintyr;
		public string minwll;
		public int miofpN;
		public string mlgbkc;
		public string mlgbkN;
		public string mlgity;
		public int mlgiyr;
		public int mlgno2;
		public int mlgnoN;
		public int mlgpgN;
		public string mlnam;
		public string mltrcd;
		public string mluse;
		public string mmagcd;
		public string mmap;
		public int mmcode;
		public int mmflN;
		public int mmnnud;
		public int mmnud;
		public int mmortc;
		public int mmosld;
		public decimal mnbadj;
		public int mNbr;
		public int mnbrhd;
		public int mNdunt;
		public int mNfbth;
		public int mNflue;
		public int mNhbth;
		public int mNroom;
		public int moccup;
		public int moldoc;
		public string motdes;
		public decimal mpbfin;
		public string mpbook;
		public decimal mpbtot;
		public string mpcode;
		public decimal mpcomp;
		public string mperr;
		public string mpict;
		public int mppage;
		public string mprcit;
		public string mprout;
		public string mprsta;
		public int mprzp1;
		public string mprzp4;
		public decimal mpsf;
		public string mpsufx;
		public int mpuse;
		public string mqafil;
		public decimal mqapch;
		public string mrecid;
		public int mrecno;
		public string mrem1;
		public string mrem2;
		public int mroofg;
		public int mrooft;
		public int mrow;
		public int msbfin;
		public int msbtot;
		public string msdirs;
		public int msellp;
		public int msewer;
		public int msflN;
		public int msfpN;
		public int mss1;
		public int mss2;
		public string mstate;
		public decimal mstorN;
		public string mstory;
		public string mstrt;
		public string msttyp;
		public string msubdv;
		public int mswl;
		public int mtac;
		public int mtbas;
		public int mtbi;
		public int mtbimp;
		public int mtbv;
		public int mterrn;
		public int mtfbas;
		public int mtfl;
		public int mtfp;
		public int mtheat;
		public int mtime;
		public decimal mtota;
		public int mtotbv;
		public int mtotld;
		public int mtotoi;
		public int mtotpr;
		public int mtplum;
		public int mtsubt;
		public int mttadd;
		public int mtutil;
		public string muser1;
		public string muser2;
		public string muser3;
		public string muser4;
		public string musrid;
		public int mwater;
		public string mwbook;
		public string mwcode;
		public int mwpage;
		public string mwsufx;
		public int myrblt;
		public int myrsld;
		public string mzip4;
		public int mzip5;
		public int mzipbr;
		public string mzone;
		public decimal NbrHdAdjValue;
		public decimal orig_BasementArea;
		public decimal orig_BasementPercentage;
		public int orig_calcsubt;
		public decimal orig_computedFactor;
		public int orig_curVal1;
		public int orig_curVal2;
		public decimal orig_FinBasementArea;
		public decimal orig_FinBasementPercentage;
		public decimal orig_FinBasementRate;
		public int orig_GasLogFP;
		public string orig_m0depr;
		public decimal orig_m1adj;
		public int orig_m1area;
		public decimal orig_m1dfac;
		public decimal orig_m1dpth;
		public decimal orig_m1frnt;
		public decimal orig_m1rate;
		public string orig_m1um;
		public decimal orig_m2adj;
		public int orig_m2area;
		public decimal orig_m2dfac;
		public decimal orig_m2dpth;
		public decimal orig_m2frnt;
		public decimal orig_m2rate;
		public string orig_m2um;
		public string orig_mac;
		public int orig_macct;
		public decimal orig_macpct;
		public string orig_macre;
		public decimal orig_macreN;
		public int orig_macsf;
		public string orig_madd1;
		public string orig_madd2;
		public int orig_mage;
		public string orig_mascom;
		public int orig_massb;
		public int orig_massl;
		public int orig_masslu;
		public int orig_massm;
		public string orig_mathom;
		public decimal orig_mbasa;
		public int orig_mbastp;
		public int orig_mbiNc;
		public decimal orig_mbrate;
		public int orig_mcada;
		public string orig_mcalc;
		public int orig_mcamo;
		public int orig_mcarNc;
		public int orig_mcarpt;
		public int orig_mcayr;
		public int orig_mcdda;
		public int orig_mcdmo;
		public string orig_mcdr;
		public int orig_mcdrdt;
		public int orig_mcdyr;
		public int orig_mchar;
		public string orig_mcity;
		public string orig_mclass;
		public string orig_mcnst1;
		public string orig_mcnst2;
		public string orig_mcomm1;
		public string orig_mcomm2;
		public string orig_mcomm3;
		public string orig_mcond;
		public int orig_mcvda;
		public string orig_mcvexp;
		public int orig_mcvmo;
		public int orig_mcvyr;
		public int orig_mdasld;
		public int orig_mdatlg;
		public int orig_mdatpr;
		public string orig_mdbook;
		public string orig_mdcode;
		public string orig_mdelay;
		public decimal orig_mdeprc;
		public string orig_mdesc1;
		public string orig_mdesc2;
		public string orig_mdesc3;
		public string orig_mdesc4;
		public string orig_mdirct;
		public int orig_mdpage;
		public string orig_mdsufx;
		public int orig_mdwell;
		public decimal orig_meacre;
		public int orig_mease;
		public decimal orig_mecond;
		public int orig_meffag;
		public int orig_mekit;
		public string orig_melec;
		public int orig_metxyr;
		public int orig_mexwl2;
		public int orig_mexwll;
		public decimal orig_mfactr;
		public int orig_mfairv;
		public string orig_mfill4;
		public string orig_mfill7;
		public string orig_mfill9;
		public int orig_mflN;
		public string orig_mfloor;
		public string orig_mflutp;
		public string orig_mfnam;
		public int orig_mfound;
		public string orig_mfp1;
		public string orig_mfp2;
		public int orig_mfpN;
		public int orig_mfuel;
		public decimal orig_mfuncd;
		public int orig_mgarN2;
		public int orig_mgarNc;
		public int orig_mgart;
		public int orig_mgart2;
		public string orig_mgas;
		public string orig_mgispn;
		public string orig_mgrntr;
		public int orig_mheat;
		public string orig_mhidnm;
		public string orig_mhidpc;
		public int orig_mhrdat;
		public string orig_mhrnam;
		public int orig_mhrphN;
		public string orig_mhrses;
		public int orig_mhrtim;
		public int orig_mhseN;
		public string orig_mhseN2;
		public int orig_mimadj;
		public int orig_mimprv;
		public string orig_minit;
		public int orig_minno2;
		public int orig_minnoN;
		public int orig_minspd;
		public string orig_mintyp;
		public int orig_mintyr;
		public string orig_minwll;
		public int orig_miofpN;
		public string orig_mlgbkc;
		public string orig_mlgbkN;
		public string orig_mlgity;
		public int orig_mlgiyr;
		public int orig_mlgno2;
		public int orig_mlgnoN;
		public int orig_mlgpgN;
		public string orig_mlnam;
		public string orig_mltrcd;
		public string orig_mluse;
		public string orig_mmagcd;
		public string orig_mmap;
		public int orig_mmcode;
		public int orig_mmflN;
		public int orig_mmnnud;
		public int orig_mmnud;
		public int orig_mmortc;
		public int orig_mmosld;
		public decimal orig_mnbadj;
		public int orig_mNbr;
		public int orig_mnbrhd;
		public int orig_mNdunt;
		public int orig_mNfbth;
		public int orig_mNflue;
		public int orig_mNhbth;
		public int orig_mNroom;
		public int orig_moccup;
		public int orig_moldoc;
		public string orig_motdes;
		public decimal orig_mpbfin;
		public string orig_mpbook;
		public decimal orig_mpbtot;
		public string orig_mpcode;
		public decimal orig_mpcomp;
		public string orig_mperr;
		public string orig_mpict;
		public int orig_mppage;
		public string orig_mprcit;
		public string orig_mprout;
		public string orig_mprsta;
		public int orig_mprzp1;
		public string orig_mprzp4;
		public decimal orig_mpsf;
		public string orig_mpsufx;
		public int orig_mpuse;
		public string orig_mqafil;
		public decimal orig_mqapch;
		public string orig_mrecid;
		public int orig_mrecno;
		public string orig_mrem1;
		public string orig_mrem2;
		public int orig_mroofg;
		public int orig_mrooft;
		public int orig_mrow;
		public int orig_msbfin;
		public int orig_msbtot;
		public string orig_msdirs;
		public int orig_msellp;
		public int orig_msewer;
		public int orig_msflN;
		public int orig_msfpN;
		public int orig_mss1;
		public int orig_mss2;
		public string orig_mstate;
		public decimal orig_mstorN;
		public string orig_mstory;
		public string orig_mstrt;
		public string orig_msttyp;
		public string orig_msubdv;
		public int orig_mswl;
		public int orig_mtac;
		public int orig_mtbas;
		public int orig_mtbi;
		public int orig_mtbimp;
		public int orig_mtbv;
		public int orig_mterrn;
		public int orig_mtfbas;
		public int orig_mtfl;
		public int orig_mtfp;
		public int orig_mtheat;
		public int orig_mtime;
		public decimal orig_mtota;
		public int orig_mtotbv;
		public int orig_mtotld;
		public int orig_mtotoi;
		public int orig_mtotpr;
		public int orig_mtplum;
		public int orig_mtsubt;
		public int orig_mttadd;
		public int orig_mtutil;
		public string orig_muser1;
		public string orig_muser2;
		public string orig_muser3;
		public string orig_muser4;
		public string orig_musrid;
		public int orig_mwater;
		public string orig_mwbook;
		public string orig_mwcode;
		public int orig_mwpage;
		public string orig_mwsufx;
		public int orig_myrblt;
		public int orig_myrsld;
		public string orig_mzip4;
		public int orig_mzip5;
		public int orig_mzipbr;
		public string orig_mzone;
		public decimal orig_physDeprRate;
		public string orig_sub911Addr;

		public event EventHandler<ParcelChangedEventArgs> ParcelChangedEvent;

		public bool IsInCalculateMode = false;

		private Dictionary<string, string> locationQualityDictionary = new Dictionary<string, string>()
		{
			{"V", "Very Good"},
			{"G", "Good"},
			{"A", "Average"},
			{"F", "Fair"},
			{"P", "Poor"}
		};

		private decimal _basementArea;
		private decimal _basementPercent;
		private string _class = "";
		private decimal _deprc;
		private decimal _economic;
		private decimal _factor;
		public decimal _finishedBsmtArea;
		private decimal _finishedBsmtPercent;
		private decimal _finishedBsmtRate;
		private decimal _functional;
		private bool _isCalculating = false;
		private bool _isDirtyCheckingOn = false;
		private decimal _neighborhoodAdj;
		private decimal _percentComp;
		public decimal PercentChange;
		public decimal PercentChgBldg;
		public decimal PercentChgLand;
		public decimal percentCompValue;
		public decimal physDeprRate;
		public decimal PrevSalesRatio;
		public int Record;
		public string SalesDate;
		public string SalesDateL;
		public string SalesDateS;
		public decimal SalesRatio;

		//public AttachedMapCollection AttachedMapRecords;
		public BuildingLineCollection SectionLineRecords;

		public BuildingSectionCollection SectionRecords;
		public CPBuildingSectionCollection SectRecords;
		public SketchMaster Sketch;
		public decimal totalBldgValue;
		public decimal TotalImprovementValue;
		public decimal TotalPropertyValue;
		public string updatestatus;
		public bool ValidRecord;
		public bool ValidSale;
		public SWallTech.CAMRA_Connection _conn;

		internal CAMRA_Connection Connection
		{
			get
			{
				return _conn;
			}

			set
			{
				_conn = value;
			}
		}

		#endregion public fields (properties)

		//public MapCoordinates MapCoordinate
		//{
		//    get
		//    {
		//        return CamraSupport.MapCoordinateCollection.MapCoordinate(this.mmap.Substring(0, 3));
		//    }
		//}

		public ParcelData()
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
		}

		public int auxA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.AuxAreaTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public decimal BasementArea
		{
			get
			{
				return _basementArea;
			}

			set
			{
				_basementArea = value;
				if (_basementArea == 0)
				{
					CalculateParcel();
				}
				FireChangedEvent("BasementArea");
			}
		}

		public decimal BasementPercentage
		{
			get
			{
				return _basementPercent;
			}

			set
			{
				_basementPercent = value;
				if (_basementPercent == 0)
				{
					CalculateParcel();
				}
				FireChangedEvent("BasementPercentage");
			}
		}

		public int carportA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.CarPortTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public string Class
		{
			get
			{
				return _class;
			}

			set
			{
				if (!CamraSupport.GetClassCodeList().Contains(value)
					 && !"".Equals(value))
				{
					throw new ArgumentException("Class not a valid value");
				}
				_class = value;
				CalculateParcel();

				FireChangedEvent("Class");
			}
		}

		public string conditionType
		{
			get
			{
				return CamraSupport.ConditionTypeCollection.Description(this.mcond.ToString().Trim());
			}
		}

		public int deckA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.DeckTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public decimal Deprc
		{
			get
			{
				return _deprc;
			}

			set
			{
				_deprc = value;
				CalculateParcel();
				FireChangedEvent("Deprc");
			}
		}

		public decimal EconomicDeprc
		{
			get
			{
				return _economic;
			}

			set
			{
				_economic = value;
				CalculateParcel();
				FireChangedEvent("EconomicDeprc");
			}
		}

		public int eporA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.EnclPorchTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public decimal Factor
		{
			get
			{
				return _factor;
			}

			set
			{
				_factor = value;
				CalculateParcel();
				FireChangedEvent("Factor");
			}
		}

		public decimal FinBasementArea
		{
			get
			{
				return _finishedBsmtArea;
			}

			set
			{
				_finishedBsmtArea = value;
				if (_finishedBsmtArea == 0)
				{
					CalculateParcel();
				}
				FireChangedEvent("FinBasementArea");
			}
		}

		public decimal FinishedBasementPercentage
		{
			get
			{
				return _finishedBsmtPercent;
			}

			set
			{
				_finishedBsmtPercent = value;
				if (_finishedBsmtPercent == 0)
				{
					CalculateParcel();
				}
				FireChangedEvent("FinishedBasementPercentage");
			}
		}

		public decimal FinishedBasementRate
		{
			get
			{
				return _finishedBsmtRate;
			}

			set
			{
				_finishedBsmtRate = value;
				if (_finishedBsmtRate == 0)
				{
					CalculateParcel();
				}
				FireChangedEvent("FinishedBasementRate");
			}
		}

		public decimal FunctionalDeprc
		{
			get
			{
				return _functional;
			}

			set
			{
				_functional = value;
				CalculateParcel();
				_isDirtyCheckingOn = true;
				FireChangedEvent("FunctionalDeprc");
			}
		}

		public int garA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.GarageTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public decimal LocationFactor
		{
			get
			{
				var r = CamraSupport.Rate1Master.GetRat1Type(Rat1Master.Rat1Types.Subdivision,
					this.msubdv) as SubdivisionRat1Type;

				return r.QualityFactor;
			}
		}

		public string LocationQuality
		{
			get
			{
				var r = CamraSupport.Rate1Master.GetRat1Type(Rat1Master.Rat1Types.Subdivision,
								  this.msubdv) as SubdivisionRat1Type;

				//var r = CamraSupport.Rate1Master.GetRat1Type(

				return locationQualityDictionary[r.QualityCode];
			}
		}

		public decimal NeighborhoodAdj
		{
			get
			{
				return _neighborhoodAdj;
			}

			set
			{
				_neighborhoodAdj = value;
				CalculateParcel();
				FireChangedEvent("NeighborhoodAdj");
			}
		}

		public bool ParcelIsChanged
		{
			get
			{
				return AnyValueHasChanged();
			}
		}

		public int patioA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.PatioTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public decimal PercentComp
		{
			get
			{
				return _percentComp;
			}

			set
			{
				_percentComp = value;
				CalculateParcel();
				FireChangedEvent("PercentComp");
			}
		}

		public int porA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.PorchTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public string SiteAddress
		{
			get
			{
				string PropAddress = null;
				decimal strtnbr = Decimal.Round(this.mhseN, 0);

				if (strtnbr != 0 && this.mhseN2 == String.Empty)
				{
					PropAddress = String.Format("{0}  {1} {2} {3}",
						strtnbr.ToString().Trim(),
						this.mdirct.ToString().Trim(),
						this.mstrt.ToString().Trim(),
						this.msttyp.ToString().Trim()
						);
				}
				if (strtnbr != 0 && this.mhseN2 != String.Empty)
				{
					PropAddress = String.Format("{0} - {1}  {2} {3} {4}",
						strtnbr.ToString().Trim(),
						this.mhseN2.Trim(),
						this.mdirct.ToString().Trim(),
						this.mstrt.ToString().Trim(),
						this.msttyp.ToString().Trim()
						);
				}

				if (strtnbr == 0)
				{
					PropAddress = String.Format("{0} {1} {2} ",
						this.mdirct.ToString().Trim(),
						this.mstrt.ToString().Trim(),
						this.msttyp.ToString().Trim()
						);
				}

				return PropAddress;
			}
		}

		public int sporA
		{
			get
			{
				var d = (from s in Sketch.BuildingSections
						 where CamraSupport.ScrnPorchTypes.Contains(s.SectionType)
						 select s.SquareFootage).Sum();

				return Convert.ToInt32(d);
			}
		}

		public string sub911Addr
		{
			get
			{
				return String.Format("{0} {1} {2} {3}",
						mhseN,
						mdirct,
						mstrt,
						msttyp);
			}
		}

		public SubDivisionCodes SubDivisionCode
		{
			get; set;
		}

		private bool AnyValueHasChanged()
		{
			bool somethingChanged = (orig_mmap.Trim() != mmap.Trim()
					|| orig_mlnam.Trim() != mlnam.Trim()
					|| orig_mfnam.Trim() != mfnam.Trim()
					|| orig_madd1.Trim() != madd1.Trim()
					|| orig_madd2.Trim() != madd2.Trim()
					|| orig_mcity.Trim() != mcity.Trim()
					|| orig_mstate.Trim() != mstate.Trim()
					|| orig_mzip5 != mzip5
					|| orig_mzip4.Trim() != mzip4.Trim()
					|| orig_macre.Trim() != macre.Trim()
					|| orig_mzone.Trim() != mzone.Trim()
					|| orig_mluse.Trim() != mluse.Trim()
					|| orig_moccup != moccup
					|| orig_mstory.Trim() != mstory.Trim()
					|| orig_mage != mage
					|| orig_mcond.Trim() != mcond.Trim()
					|| orig_mclass.Trim() != mclass.Trim()
					|| orig_mfactr != mfactr
					|| orig_mdeprc != mdeprc
					|| orig_mfound != mfound
					|| orig_mexwll != mexwll
					|| orig_mrooft != mrooft
					|| orig_mroofg != mroofg
					|| orig_mNdunt != mNdunt
					|| orig_mNroom != mNroom
					|| orig_mNbr != mNbr
					|| orig_mNfbth != mNfbth
					|| orig_mNhbth != mNhbth
					|| orig_mfp2.Trim() != mfp2.Trim()
					|| orig_mltrcd.Trim() != mltrcd.Trim()
					|| orig_mheat != mheat
					|| orig_mfuel != mfuel
					|| orig_mac.Trim() != mac.Trim()
					|| orig_mfp1.Trim() != mfp1.Trim()
					|| orig_mcdr.Trim() != mcdr.Trim()
					|| orig_mekit != mekit
					|| orig_mbastp != mbastp
					|| orig_mpbtot != mpbtot
					|| orig_msbtot != msbtot
					|| orig_mpbfin != mpbfin
					|| orig_msbfin != msbfin
					|| orig_mbrate != mbrate
					|| orig_mNflue != mNflue
					|| orig_mflutp.Trim() != mflutp.Trim()
					|| orig_mgart != mgart
					|| orig_mgarNc != mgarNc
					|| orig_mcarpt != mcarpt
					|| orig_mcarNc != mcarNc
					|| orig_mbiNc != mbiNc
					|| orig_mrow != mrow
					|| orig_mease != mease
					|| orig_mwater != mwater
					|| orig_msewer != msewer
					|| orig_mgas.Trim() != mgas.Trim()
					|| orig_melec.Trim() != melec.Trim()
					|| orig_mterrn != mterrn
					|| orig_mchar != mchar
					|| orig_motdes.Trim() != motdes.Trim()
					|| orig_mgart2 != mgart2
					|| orig_mgarN2 != mgarN2
					|| orig_mdatlg != mdatlg
					|| orig_mdatpr != mdatpr
					|| orig_mintyp.Trim() != mintyp.Trim()
					|| orig_mintyr != mintyr
					|| orig_minnoN != minnoN
					|| orig_minno2 != minno2
					|| orig_mdsufx.Trim() != mdsufx.Trim()
					|| orig_mwsufx.Trim() != mwsufx.Trim()
					|| orig_mpsufx.Trim() != mpsufx.Trim()
					|| orig_mimprv != mimprv
					|| orig_mtotld != mtotld
					|| orig_mtotoi != mtotoi
					|| orig_mtotpr != mtotpr
					|| orig_massb != massb
					|| orig_macpct != macpct
					|| orig_m1frnt != m1frnt
					|| orig_m1dpth != m1dpth
					|| orig_m1area != m1area
					|| orig_mmcode != mmcode
					|| orig_m0depr != m0depr
					|| orig_m1um.Trim() != m1um.Trim()
					|| orig_m2frnt != m2frnt
					|| orig_m2dpth != m2dpth
					|| orig_m2area != m2area
					|| orig_mzipbr != mzipbr
					|| orig_mdelay.Trim() != mdelay.Trim()
					|| orig_m2um.Trim() != m2um.Trim()
					|| orig_mstrt.Trim() != mstrt.Trim()
					|| orig_mdirct.Trim() != mdirct.Trim()
					|| orig_mhseN != mhseN
					|| orig_mcdmo != mcdmo
					|| orig_mcdda != mcdda
					|| orig_mcdyr != mcdyr
					|| orig_m1dfac != m1dfac
					|| orig_mrem1.Trim() != mrem1.Trim()
					|| orig_mrem2.Trim() != mrem2.Trim()
					|| orig_mmagcd.Trim() != mmagcd.Trim()
					|| orig_mathom.Trim() != mathom.Trim()
					|| orig_mdesc1.Trim() != mdesc1.Trim()
					|| orig_mdesc2.Trim() != mdesc2.Trim()
					|| orig_mdesc3.Trim() != mdesc3.Trim()
					|| orig_mdesc4.Trim() != mdesc4.Trim()
					|| orig_mfairv != mfairv
					|| orig_mlgity.Trim() != mlgity.Trim()
					|| orig_mlgiyr != mlgiyr
					|| orig_mlgnoN != mlgnoN
					|| orig_mlgno2 != mlgno2
					|| orig_msubdv.Trim() != msubdv.Trim()
					|| orig_msellp != msellp
					|| orig_m2dfac != m2dfac
					|| orig_minit.Trim() != minit.Trim()
					|| orig_minspd != minspd
					|| orig_mswl != mswl
					|| orig_mtutil != mtutil
					|| orig_mnbadj != mnbadj
					|| orig_massl != massl
					|| orig_macsf != macsf
					|| orig_mcomm1.Trim() != mcomm1.Trim()
					|| orig_mcomm2.Trim() != mcomm2.Trim()
					|| orig_mcomm3.Trim() != mcomm3.Trim()
					|| orig_macct != macct
					|| orig_mexwl2 != mexwl2
					|| orig_mcalc.Trim() != mcalc.Trim()
					|| orig_mfill4.Trim() != mfill4.Trim()
					|| orig_mtbv != mtbv
					|| orig_mtbas != mtbas
					|| orig_mtfbas != mtfbas
					|| orig_mtplum != mtplum
					|| orig_mtheat != mtheat
					|| orig_mtac != mtac
					|| orig_mtfp != mtfp
					|| orig_mtfl != mtfl
					|| orig_mtbi != mtbi
					|| orig_mttadd != mttadd
					|| orig_mtsubt != mtsubt
					|| orig_mtotbv != mtotbv
					|| orig_musrid.Trim() != musrid.Trim()
					|| orig_mbasa != mbasa
					|| orig_mtota != mtota
					|| orig_mpsf != mpsf
					|| orig_minwll.Trim() != minwll.Trim()
					|| orig_mfloor != mfloor.Trim()
					|| orig_myrblt != myrblt
					|| orig_mcnst1.Trim() != mcnst1.Trim()
					|| orig_mcnst2.Trim() != mcnst2.Trim()
					|| orig_masslu != masslu
					|| orig_mmosld != mmosld
					|| orig_mdasld != mdasld
					|| orig_myrsld != myrsld
					|| orig_mtime != mtime
					|| orig_mhseN2.Trim() != mhseN2.Trim()
					|| orig_m1adj != m1adj
					|| orig_m2adj != m2adj
					|| orig_mlgbkc.Trim() != mlgbkc.Trim()
					|| orig_mlgbkN.Trim() != mlgbkN.Trim()
					|| orig_mlgpgN != mlgpgN
					|| orig_meffag != meffag
					|| orig_mpcomp != mpcomp
					|| orig_msttyp.Trim() != msttyp.Trim()
					|| orig_msdirs.Trim() != msdirs.Trim()
					|| orig_m1rate != m1rate
					|| orig_m2rate != m2rate
					|| orig_mfuncd != mfuncd
					|| orig_mecond != mecond
					|| orig_mnbrhd != mnbrhd
					|| orig_muser1.Trim() != muser1.Trim()
					|| orig_muser2.Trim() != muser2.Trim()
					|| orig_mdbook.Trim() != mdbook.Trim()
					|| orig_mdpage != mdpage
					|| orig_mwbook.Trim() != mwbook.Trim()
					|| orig_mwpage != mwpage
					|| orig_mdcode.Trim() != mdcode.Trim()
					|| orig_mwcode.Trim() != mwcode.Trim()
					|| orig_mmortc != mmortc
					|| orig_mfill7.Trim() != mfill7.Trim()
					|| orig_macreN != macreN
					|| orig_mgispn.Trim() != mgispn.Trim()
					|| orig_muser3.Trim() != muser3.Trim()
					|| orig_muser4.Trim() != muser4.Trim()
					|| orig_mimadj != mimadj
					|| orig_mcdrdt != mcdrdt
					|| orig_mmnud != mmnud
					|| orig_mmnnud != mmnnud
					|| orig_mss1 != mss1
					|| orig_mpcode.Trim() != mpcode.Trim()
					|| orig_mpbook.Trim() != mpbook.Trim()
					|| orig_mppage != mppage
					|| orig_mss2 != mss2
					|| orig_massm != massm
					|| orig_mfill9.Trim() != mfill9.Trim()
					|| orig_mgrntr.Trim() != mgrntr.Trim()
					|| orig_mcvmo != mcvmo
					|| orig_mcvda != mcvda
					|| orig_mcvyr != mcvyr
					|| orig_mprout.Trim() != mprout.Trim()
					|| orig_mperr.Trim() != mperr.Trim()
					|| orig_mtbimp != mtbimp
					|| orig_mpuse != mpuse
					|| orig_mcvexp.Trim() != mcvexp.Trim()
					|| orig_metxyr != metxyr
					|| orig_mqapch != mqapch
					|| orig_mqafil.Trim() != mqafil.Trim()
					|| orig_mpict.Trim() != mpict.Trim()
					|| orig_meacre != meacre
					|| orig_mprcit.Trim() != mprcit.Trim()
					|| orig_mprsta.Trim() != mprsta.Trim()
					|| orig_mprzp1 != mprzp1
					|| orig_mprzp4.Trim() != mprzp4.Trim()
					|| orig_mfpN != mfpN
					|| orig_msfpN != msfpN
					|| orig_mflN != mflN
					|| orig_msflN != msflN
					|| orig_mmflN != mmflN
					|| orig_miofpN != miofpN
					|| orig_mstorN != mstorN
					|| orig_mascom.Trim() != mascom.Trim()
					|| orig_mhrphN != mhrphN
					|| orig_mhrdat != mhrdat
					|| orig_mhrtim != mhrtim
					|| orig_mhrnam.Trim() != mhrnam.Trim()
					|| orig_mhrses.Trim() != mhrses.Trim()
					|| orig_mhidpc.Trim() != mhidpc.Trim()
					|| orig_mhidnm.Trim() != mhidnm.Trim()
					|| orig_mcamo != mcamo
					|| orig_mcada != mcada
					|| orig_mcayr != mcayr
					|| orig_moldoc != moldoc
					|| orig_FinBasementRate != FinishedBasementRate

					);
			return somethingChanged;
		}

		public void BuildSketchData()
		{
			this.SectionLineRecords = BuildingLineCollection.GetLines(_conn, this.mrecno, this.mdwell);

			//this.SectionLineRecords = FoxproSketches.BuildingLineCollection(this._fox, this.mrecno, this.mdwell);
			this.Sketch = new SketchMaster(_conn, this.mrecno, this.mdwell, this.moccup);
			var section = new CPBuildingSectionCollection(_conn, this);
			section.GetSection(_conn, this.mrecno, this.mdwell);
			this.SectRecords = section;
		}

		public void CalculateParcel()
		{
			if (CamraSupport.ResidentialOccupancies.Contains(this.moccup))
			{
				if (!_isCalculating)
				{
					if (IsInCalculateMode)
					{
						_isCalculating = true;
						TotalPropertyValue = 0;
						TotalImprovementValue = 0;
						totalBldgValue = 0;
						depreciatonValue = 0;
						funcDeprValue = 0;
						econDeprValue = 0;
						factoredValue = 0;
						computedFactor = 0;
						PercentChange = 0;
						CurrentValue = 0;
						SalesRatio = 0;
						PrevSalesRatio = 0;
						BaseChange = 0;
						NbrHdAdjValue = 0;
						percentCompValue = 0;
						FunctionalDeprc = this.mfuncd;

						if (macpct != 0 && macsf == 0)
						{
							macsf = Convert.ToInt32(macpct * mtota);
						}
						if (macpct == 0 && macsf > 0)
						{
							macpct = Convert.ToDecimal(macsf / mtota);
						}

						if (this.moccup == 16)
						{
							computedFactor = 0;
							factoredValue = 0;
							depreciatonValue = 0;
							funcDeprValue = 0;
							econDeprValue = 0;
							totalBldgValue = this.mfairv;
							percentCompValue = 0;
							NbrHdAdjValue = 0;
							TotalImprovementValue = Decimal.Round((totalBldgValue + this.mtotoi), 0);
							TotalPropertyValue = Decimal.Round((TotalImprovementValue + this.mtotld), 0);
						}

						if (this.moccup == 26)
						{
							computedFactor = 0;
							factoredValue = 0;
							depreciatonValue = 0;
							funcDeprValue = 0;
							econDeprValue = 0;
							totalBldgValue = this.mfairv;
							percentCompValue = 0;
							NbrHdAdjValue = 0;
							TotalImprovementValue = Decimal.Round((totalBldgValue + this.mtotoi), 0);
							TotalPropertyValue = Decimal.Round((TotalImprovementValue + this.mtotld), 0);
						}

						if (this.moccup != 16)
						{
							{
								computedFactor = Decimal.Round((GetClassValue(this.mclass) + this.mfactr), 2);
								orig_computedFactor = computedFactor;

								////   need the adjusted mtsubt here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

								calcsubt = Convert.ToInt32((mtota * mpsf) + mtbas + mtfbas + mtplum + mtheat + mtac + mtfp + mtfl + mtbimp + mtbi + mswl + mttadd + (mekit * CamraSupport.ExtraKitRate));

								orig_calcsubt = calcsubt;

								if (orig_mtsubt != calcsubt)
								{
									mtsubt = calcsubt;
								}

								factoredValue = Decimal.Round((this.mtsubt * computedFactor), 0);

								//physDeprRate = 0;
								decimal physDepr = 0;

								if (this.m0depr == "Y")
								{
									physDepr = 0;
									depreciatonValue = 0;
								}
								if (this.mdeprc > 0)  //   mdeprc was Deprc
								{
									physDepr = (this.factoredValue * this.mdeprc).ReverseAndRoundToZero();
									depreciatonValue = (this.mdeprc * factoredValue).ReverseAndRoundToZero();

									if (this.mcond == "G")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondG), 2);
									}
									if (this.mcond == "A")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondA), 2);
									}
									if (this.mcond == "F")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondF), 2);
									}
									if (this.mcond == "P")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondP), 2);
									}

									if (physDeprRate > 0.65m)
									{
										physDeprRate = 0.65m;
									}

									orig_physDeprRate = physDeprRate;
								}
								if (this.mdeprc == 0 && this.m0depr != "Y")
								{
									if (mage == 997)
									{
										mage = 1;
									}
									if (mage == 998)
									{
										mage = 45;
									}
									if (mage == 999 || mage == 900)
									{
										mage = 60;
									}

									if (this.mcond == "G")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondG), 2);
									}
									if (this.mcond == "A")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondA), 2);
									}
									if (this.mcond == "F")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondF), 2);
									}
									if (this.mcond == "P")
									{
										physDeprRate = Decimal.Round((Convert.ToDecimal(this.mage) * CamraSupport.DefDepCondP), 2);
									}

									if (physDeprRate > 0.65m)
									{
										physDeprRate = 0.65m;
									}

									physDepr = (this.factoredValue * physDeprRate).ReverseAndRoundToZero();

									depreciatonValue = (physDeprRate * factoredValue).ReverseAndRoundToZero();

									orig_physDeprRate = physDeprRate;
								}

								funcDeprValue = ((factoredValue + depreciatonValue) * this.mfuncd).ReverseAndRoundToZero();
								econDeprValue = ((factoredValue + depreciatonValue + funcDeprValue) * this.mecond).ReverseAndRoundToZero();

								totalBldgValue = Decimal.Round((factoredValue + depreciatonValue + funcDeprValue + econDeprValue), 0);

								decimal tbv3 = (Math.Round(Convert.ToDecimal(totalBldgValue) / 100, 0, MidpointRounding.AwayFromZero) * 100);

								totalBldgValue = tbv3;

								if (this.NeighborhoodAdj != 0)
								{
									NbrHdAdjValue = Decimal.Round((totalBldgValue * NeighborhoodAdj), 0);
								}
								else if (this.NbrHdAdjValue == 0)
								{
									NbrHdAdjValue = 0;
								}

								if (this.mpcomp != 0)
								{
									//percentCompValue = ((totalBldgValue + NbrHdAdjValue) * (1 - this.PercentComp)).ReverseAndRoundToZero();

									decimal pcval = ((totalBldgValue + NbrHdAdjValue) * (this.mpcomp)).ReverseAndRoundToZero();

									percentCompValue = ((totalBldgValue + NbrHdAdjValue) + pcval);
								}
								else
								{
									percentCompValue = 0;
								}

								//TotalImprovementValue = Decimal.Round((((totalBldgValue + NbrHdAdjValue )* (1 - this.PercentComp)) + mtotoi + mimadj), 0);
								//TotalImprovementValue = Decimal.Round((((totalBldgValue + NbrHdAdjValue) * (1-this.mpcomp)) + mtotoi + mimadj), 0);

								TotalImprovementValue = Decimal.Round((((totalBldgValue + NbrHdAdjValue) - percentCompValue) + mtotoi + mimadj), 0);

								TotalPropertyValue = Decimal.Round((TotalImprovementValue + mtotld), 0);
							}

							CurrentValue = Convert.ToInt32(massb + massl);
							if (CurrentValue > 0 && mdwell == 1)
							{
								decimal currentLandDec = Convert.ToDecimal(massl);
								decimal currentBldgDec = Convert.ToDecimal(massb);
								decimal newLandDec = Convert.ToDecimal(mtotld);
								decimal newBldgDec = Convert.ToDecimal(mimprv);
								decimal currentValueDec = Convert.ToDecimal(CurrentValue);
								if (currentLandDec == 0)
								{
									currentLandDec = 1;
								}
								if (currentBldgDec == 0)
								{
									currentBldgDec = 1;
								}

								PercentChange = Decimal.Round((TotalPropertyValue - currentValueDec) / currentValueDec, 2);
								PercentChgLand = Decimal.Round((newLandDec - currentLandDec) / currentLandDec, 2);
								PercentChgBldg = Decimal.Round((newBldgDec - currentBldgDec) / currentBldgDec, 2);
								BaseChange = Decimal.Round((mtotpr - currentValueDec) / currentValueDec, 2);
							}

							try
							{
								if (this.mtbas > 0)
								{
									if (_basementArea != orig_BasementArea)
									{
										_basementPercent = (BasementArea / mbasa);
										if (orig_BasementPercentage == 1)
										{
											_basementPercent = orig_BasementPercentage;
										}

										mtbas = Convert.ToInt32(_basementArea * (CamraSupport.BasementRate));
									}
									if (_basementArea == orig_BasementArea)
									{
										_basementArea = orig_BasementArea;
										_basementPercent = orig_BasementPercentage;
									}
								}

								if (mtfbas > 0 && mbrate > 0 && msbfin <= 0)
								{
									_finishedBsmtArea = Decimal.Round((Convert.ToDecimal(mtfbas / mbrate)), 0);
									_finishedBsmtPercent = (_finishedBsmtArea / BasementArea);
									orig_FinBasementRate = mbrate;
								}
								if (mtfbas > 0 && mbrate > 0 && msbfin > 0)
								{
									_finishedBsmtArea = msbfin;
									_finishedBsmtPercent = (_finishedBsmtArea / BasementArea);
									orig_FinBasementRate = mbrate;
								}
								else if (mbrate == 0)
								{
									_finishedBsmtArea = 0;
									_finishedBsmtPercent = 0;
								}
							}
							catch (DivideByZeroException divex)
							{
								Console.WriteLine(string.Format("{0}", divex.Message));
							}

							if (myrsld >= CamraSupport.SaleYearCutOff && msellp > 0)
							{
								decimal _saleRatio = (Convert.ToDecimal(mtotpr) / Convert.ToDecimal(msellp));
								decimal _prevRatio = (Convert.ToDecimal(massl + massb) / Convert.ToDecimal(msellp));

								SalesRatio = Decimal.Round(_saleRatio, 3);
								PrevSalesRatio = Decimal.Round(_prevRatio, 3);
								ValidSale = true;
							}
							else
							{
								SalesRatio = 0;
								PrevSalesRatio = 0;
								ValidSale = false;
							}

							_isCalculating = false;
						}
					}
				}
			}

			if (CamraSupport.CommercialOccupancies.Contains(this.moccup))
			{
				if (!_isCalculating)
				{
					if (IsInCalculateMode)
					{
						_isCalculating = true;
						TotalPropertyValue = 0;
						TotalImprovementValue = 0;
						totalBldgValue = 0;
						depreciatonValue = 0;
						funcDeprValue = 0;
						econDeprValue = 0;
						factoredValue = 0;
						computedFactor = 0;
						PercentChange = 0;
						CurrentValue = 0;
						PrevSalesRatio = 0;
						SalesRatio = 0;
						BaseChange = 0;
						NbrHdAdjValue = 0;
						percentCompValue = 0;

						if (this.mtsubt == 0)
						{
							factoredValue = 1;
							this.mtsubt = 1;
						}

						int tempRec = mrecno;
						int tempCard = mdwell;

						factoredValue = Convert.ToInt32(this.mtsubt + Sketch.BuildingSections.TotalFactorValue);
						computedFactor = Decimal.Round((factoredValue / this.mtsubt), 2);

						funcDeprValue = Convert.ToInt32(((factoredValue - Sketch.BuildingSections.TotalDepreciationValue) * this.mfuncd) * -1);
						econDeprValue = Convert.ToInt32(((factoredValue - Sketch.BuildingSections.TotalDepreciationValue + funcDeprValue) * this.mecond) * -1);

						totalBldgValue = Convert.ToInt32(factoredValue - Sketch.BuildingSections.TotalDepreciationValue + funcDeprValue + econDeprValue);

						decimal tbv1 = (Math.Round(Convert.ToDecimal(totalBldgValue) / 100, 0, MidpointRounding.AwayFromZero) * 100);

						totalBldgValue = tbv1;
					}
				}
			}

			if (CamraSupport.TaxExemptOccupancies.Contains(this.moccup))
			{
				if (!_isCalculating)
				{
					if (IsInCalculateMode)
					{
						_isCalculating = true;
						TotalPropertyValue = 0;
						TotalImprovementValue = 0;
						totalBldgValue = 0;
						depreciatonValue = 0;
						funcDeprValue = 0;
						econDeprValue = 0;
						factoredValue = 0;
						computedFactor = 0;
						PercentChange = 0;
						CurrentValue = 0;
						PrevSalesRatio = 0;
						SalesRatio = 0;
						BaseChange = 0;
						NbrHdAdjValue = 0;
						percentCompValue = 0;

						factoredValue = Convert.ToInt32(this.mtsubt + Sketch.BuildingSections.TotalFactorValue);
						computedFactor = Decimal.Round((factoredValue / this.mtsubt), 2);
						orig_computedFactor = computedFactor;
						funcDeprValue = Convert.ToInt32(((factoredValue - Sketch.BuildingSections.TotalDepreciationValue) * this.mfuncd) * -1);
						econDeprValue = Convert.ToInt32(((factoredValue - Sketch.BuildingSections.TotalDepreciationValue + funcDeprValue) * this.mecond) * -1);

						totalBldgValue = Convert.ToInt32(factoredValue - Sketch.BuildingSections.TotalDepreciationValue + funcDeprValue + econDeprValue);

						decimal tbv2 = (Math.Round(Convert.ToDecimal(totalBldgValue) / 100, 0, MidpointRounding.AwayFromZero) * 100);

						totalBldgValue = tbv2;
					}
				}
			}

			decimal neighborhoodAdjValue = 0;
			if (this.NeighborhoodAdj == 0)
			{
				NbrHdAdjValue = 0;
			}
			else if (this.NeighborhoodAdj != 0)
			{
				neighborhoodAdjValue = ((Convert.ToDecimal(totalBldgValue)) * (1 + this.mnbadj));
				NbrHdAdjValue = (totalBldgValue - neighborhoodAdjValue).ReverseAndRoundToZero();
			}

			if (this.mpcomp != 0)
			{
				percentCompValue = ((totalBldgValue + NbrHdAdjValue) * (1 - this.mpcomp)).ReverseAndRoundToZero();

				//percentCompValue = ((totalBldgValue + NbrHdAdjValue) * (1 - this.mpcomp)).ReverseAndRoundToZero();
			}
			else if (this.mpcomp == 0)
			{
				percentCompValue = 0;
			}

			if (this.mpcomp == 0 && this.mnbadj == 0)
			{
				TotalImprovementValue = Decimal.Round((this.totalBldgValue + this.mtotoi + this.mimadj), 0);
			}
			if (this.mpcomp != 0 && this.mnbadj == 0)
			{
				TotalImprovementValue = Decimal.Round(((totalBldgValue * this.mpcomp) + this.mtotoi + this.mimadj), 0);
			}
			if (this.mpcomp != 0 && this.mnbadj != 0)
			{
				TotalImprovementValue = Decimal.Round((((totalBldgValue + NbrHdAdjValue) * this.mpcomp) + this.mtotoi + this.mimadj), 0);
			}

			TotalPropertyValue = Decimal.Round((TotalImprovementValue + this.mtotld), 0);

			CurrentValue = Convert.ToInt32(massb + massl);
			if (CurrentValue > 0 && mdwell == 1)
			{
				decimal currentLandDec = Convert.ToDecimal(massl);
				decimal currentBldgDec = Convert.ToDecimal(massb);
				decimal newLandDec = Convert.ToDecimal(mtotld);
				decimal newBldgDec = Convert.ToDecimal(mimprv);
				decimal currentValueDec = Convert.ToDecimal(CurrentValue);
				if (currentLandDec == 0)
				{
					currentLandDec = 1;
				}
				if (currentBldgDec == 0)
				{
					currentBldgDec = 1;
				}

				PercentChange = Decimal.Round((TotalPropertyValue - currentValueDec) / currentValueDec, 3);
				PercentChgLand = Decimal.Round((newLandDec - currentLandDec) / currentLandDec, 3);
				PercentChgBldg = Decimal.Round((newBldgDec - currentBldgDec) / currentBldgDec, 3);
				BaseChange = Decimal.Round((mtotpr - currentValueDec) / currentValueDec, 3);
			}

			if (myrsld >= CamraSupport.SaleYearCutOff && msellp > 0)
			{
				decimal _saleRatio = (Convert.ToDecimal(mtotpr) / Convert.ToDecimal(msellp));
				decimal _prevRatio = (Convert.ToDecimal(massl + massb) / Convert.ToDecimal(msellp));

				SalesRatio = Decimal.Round(_saleRatio, 3);
				PrevSalesRatio = Decimal.Round(_prevRatio, 3);
				ValidSale = true;
			}
			else
			{
				SalesRatio = 0;
				PrevSalesRatio = 0;
				ValidSale = false;
			}
		}

		private void FireChangedEvent(string property)
		{
			if (_isDirtyCheckingOn)
			{
				if (ParcelChangedEvent != null)
				{
					ParcelChangedEvent(this,
						new ParcelChangedEventArgs()
						{
							PropertyName = property
						});
				}
			}
		}

		public decimal GetClassValue(string cls)
		{
			decimal retValue = 0;
			if (CamraSupport.GetClassCodeList().Contains(cls))
			{
				retValue = CamraSupport.GetClassValue(cls);
			}
			return retValue;
		}

		public static ParcelData getParcel(SWallTech.CAMRA_Connection _fox, int record, int card)
		{
			ParcelData _parcel = null;

			StringBuilder subParcel = new StringBuilder();
			subParcel.Append(" select mrecid,mrecno,mdwell,mmap,mlnam,mfnam,madd1,madd2,mcity,mstate,mzip5,mzip4,macre,mzone,mluse,moccup,mstory,mage,mcond,mclass,mfactr, ");
			subParcel.Append(" mdeprc,mfound,mexwll,mrooft,mroofg,m#dunt,m#room,m#br,m#fbth,m#hbth,mfp2,mltrcd,mheat,mfuel,mac,mfp1,mcdr,mekit,mbastp,mpbtot,msbtot, ");
			subParcel.Append(" mpbfin,msbfin,mbrate,m#flue,mflutp,mgart,mgar#c,mcarpt,mcar#c,mbi#c,mrow,mease,mwater,msewer,mgas,melec,mterrn,mchar,motdes,mgart2,mgar#2, ");
			subParcel.Append(" mdatlg,mdatpr,mintyp,mintyr,minno#,minno2,mdsufx,mwsufx,mpsufx,mimprv,mtotld,mtotoi,mtotpr,massb,macpct,m1frnt,m1dpth,m1area,mmcode,m0depr, ");
			subParcel.Append(" m1um,m2frnt,m2dpth,m2area,mzipbr,mdelay,m2um,mstrt,mdirct,mhse#,mcdmo,mcdda,mcdyr,m1dfac,mrem1,mrem2,mmagcd,mathom,mdesc1,mdesc2,mdesc3,mdesc4, ");
			subParcel.Append(" mfairv,mlgity,mlgiyr,mlgno#,mlgno2,msubdv,msellp,m2dfac,minit,minspd,mswl,mtutil,mnbadj,massl,macsf,mcomm1,mcomm2,mcomm3,macct,mexwl2,mcalc, ");
			subParcel.Append(" mfill4,mtbv,mtbas,mtfbas,mtplum,mtheat,mtac,mtfp,mtfl,mtbi,mttadd,mtsubt,mtotbv,musrid,mbasa,mtota,mpsf,minwll,mfloor,myrblt,mcnst1,mcnst2,masslu, ");
			subParcel.Append(" mmosld,mdasld,myrsld,mtime,mhse#2,m1adj,m2adj,mlgbkc,mlgbk#,mlgpg#,meffag,mpcomp,msttyp,msdirs,m1rate,m2rate,mfuncd,mecond,mnbrhd,muser1,muser2, ");
			subParcel.Append(" mdbook,mdpage,mwbook,mwpage,mdcode,mwcode,mmortc,mfill7,macre#,mgispn,muser3,muser4,mimadj,mcdrdt,mmnud,mmnnud,mss1,mpcode,mpbook,mppage,mss2,massm, ");
			subParcel.Append(" mfill9,mgrntr,mcvmo,mcvda,mcvyr,mprout,mperr,mtbimp,mpuse,mcvexp,metxyr,mqapch,mqafil,mpict,meacre,mprcit,mprsta,mprzp1,mprzp4,mfp#,msfp#,mfl#, ");
			subParcel.Append(" msfl#,mmfl#,miofp#,mstor#,mascom,mhrph#,mhrdat,mhrtim,mhrnam,mhrses,mhidpc,mhidnm,mcamo,mcada,mcayr,moldoc,substring(minwll,1,2) as walls, substring(minwll,3,2) as wall2, ");
			subParcel.Append(" substring(minwll,5,2) as wall3, substring(minwll,7,2) as wall4, substring(mfloor,1,2) as floors, substring(mfloor,3,2) as floor2, substring(mfloor,5,2) as floor3, substring(mfloor,7,2) as floor4 ");
			subParcel.Append(String.Format("from {0}.{1}mast ", MainForm.localLib, MainForm.localPreFix));
			subParcel.Append(String.Format("  where mrecno = {0} and mdwell = {1} and moccup < 30 ", record, card));

			DataSet Parcel = _fox.DBConnection.RunSelectStatement(subParcel.ToString());

			if (Parcel.Tables[0].Rows.Count > 0)
			{
				DataRow row = Parcel.Tables[0].Rows[0];

				_parcel = new ParcelData()
				{
					Record = record,
					Card = card,
					_conn = _fox,

					mrecid = row["mrecid"].ToString().Trim(),
					mrecno = record,
					mdwell = card,
					mmap = row["mmap"].ToString().TrimEnd(new char[] { ' ' }),
					mlnam = row["mlnam"].ToString().Trim(),
					mfnam = row["mfnam"].ToString().Trim(),
					madd1 = row["madd1"].ToString().Trim(),
					madd2 = row["madd2"].ToString().Trim(),
					mcity = row["mcity"].ToString().Trim(),
					mstate = row["mstate"].ToString().Trim(),
					mzip5 = Convert.ToInt32(row["mzip5"].ToString()),
					mzip4 = row["mzip4"].ToString().Trim(),
					macre = row["macre"].ToString().Trim(),
					mzone = row["mzone"].ToString().Trim(),
					mluse = row["mluse"].ToString().Trim(),
					moccup = Convert.ToInt32(row["moccup"].ToString()),
					mstory = row["mstory"].ToString().Trim(),
					mage = Convert.ToInt32(row["mage"].ToString()),
					mcond = row["mcond"].ToString().Trim(),
					mclass = row["mclass"].ToString().Trim(),
					mfactr = Convert.ToDecimal(row["mfactr"].ToString()),
					mdeprc = Convert.ToDecimal(row["mdeprc"].ToString()),
					mfound = Convert.ToInt32(row["mfound"].ToString()),
					mexwll = Convert.ToInt32(row["mexwll"].ToString()),
					mrooft = Convert.ToInt32(row["mrooft"].ToString()),
					mroofg = Convert.ToInt32(row["mroofg"].ToString()),
					mNdunt = Convert.ToInt32(row["m#dunt"].ToString()),
					mNroom = Convert.ToInt32(row["m#room"].ToString()),
					mNbr = Convert.ToInt32(row["m#br"].ToString()),
					mNfbth = Convert.ToInt32(row["m#fbth"].ToString()),
					mNhbth = Convert.ToInt32(row["m#hbth"].ToString()),
					mfp2 = row["mfp2"].ToString().Trim(),
					mltrcd = row["mltrcd"].ToString().Trim(),
					mheat = Convert.ToInt32(row["mheat"].ToString()),
					mfuel = Convert.ToInt32(row["mfuel"].ToString()),
					mac = row["mac"].ToString().Trim(),
					mfp1 = row["mfp1"].ToString().Trim(),
					mcdr = row["mcdr"].ToString().Trim(),
					mekit = Convert.ToInt32(row["mekit"].ToString()),
					mbastp = Convert.ToInt32(row["mbastp"].ToString()),
					mpbtot = Convert.ToDecimal(row["mpbtot"].ToString()),
					msbtot = Convert.ToInt32(row["msbtot"].ToString()),
					mpbfin = Convert.ToDecimal(row["mpbfin"].ToString()),
					msbfin = Convert.ToInt32(row["msbfin"].ToString()),
					mbrate = Convert.ToDecimal(row["mbrate"].ToString()),
					mNflue = Convert.ToInt32(row["m#flue"].ToString()),
					mflutp = row["mflutp"].ToString().Trim(),
					mgart = Convert.ToInt32(row["mgart"].ToString()),
					mgarNc = Convert.ToInt32(row["mgar#c"].ToString()),
					mcarpt = Convert.ToInt32(row["mcarpt"].ToString()),
					mcarNc = Convert.ToInt32(row["mcar#c"].ToString()),
					mbiNc = Convert.ToInt32(row["mbi#c"].ToString()),
					mrow = Convert.ToInt32(row["mrow"].ToString()),
					mease = Convert.ToInt32(row["mease"].ToString()),
					mwater = Convert.ToInt32(row["mwater"].ToString()),
					msewer = Convert.ToInt32(row["msewer"].ToString()),
					mgas = row["mgas"].ToString().Trim(),
					melec = row["melec"].ToString().Trim(),
					mterrn = Convert.ToInt32(row["mterrn"].ToString()),
					mchar = Convert.ToInt32(row["mchar"].ToString()),
					motdes = row["motdes"].ToString().Trim(),
					mgart2 = Convert.ToInt32(row["mgart2"].ToString()),
					mgarN2 = Convert.ToInt32(row["mgar#2"].ToString()),
					mdatlg = Convert.ToInt32(row["mdatlg"].ToString()),
					mdatpr = Convert.ToInt32(row["mdatpr"].ToString()),
					mintyp = row["mintyp"].ToString().Trim(),
					mintyr = Convert.ToInt32(row["mintyr"].ToString()),
					minnoN = Convert.ToInt32(row["minno#"].ToString()),
					minno2 = Convert.ToInt32(row["minno2"].ToString()),
					mdsufx = row["mdsufx"].ToString().Trim(),
					mwsufx = row["mwsufx"].ToString().Trim(),
					mpsufx = row["mpsufx"].ToString().Trim(),
					mimprv = Convert.ToInt32(row["mimprv"].ToString()),
					mtotld = Convert.ToInt32(row["mtotld"].ToString()),
					mtotoi = Convert.ToInt32(row["mtotoi"].ToString()),
					mtotpr = Convert.ToInt32(row["mtotpr"].ToString()),
					massb = Convert.ToInt32(row["massb"].ToString()),
					macpct = Convert.ToDecimal(row["macpct"].ToString()),
					m1frnt = Convert.ToDecimal(row["m1frnt"].ToString()),
					m1dpth = Convert.ToDecimal(row["m1dpth"].ToString()),
					m1area = Convert.ToInt32(row["m1area"].ToString()),
					mmcode = Convert.ToInt32(row["mmcode"].ToString()),
					m0depr = row["m0depr"].ToString().Trim(),
					m1um = row["m1um"].ToString().Trim(),
					m2frnt = Convert.ToDecimal(row["m2frnt"].ToString()),
					m2dpth = Convert.ToDecimal(row["m2dpth"].ToString()),
					m2area = Convert.ToInt32(row["m2area"].ToString()),
					mzipbr = Convert.ToInt32(row["mzipbr"].ToString()),
					mdelay = row["mdelay"].ToString().Trim(),
					m2um = row["m2um"].ToString().Trim(),
					mstrt = row["mstrt"].ToString().Trim(),
					mdirct = row["mdirct"].ToString().Trim(),
					mhseN = Convert.ToInt32(row["mhse#"].ToString()),
					mcdmo = Convert.ToInt32(row["mcdmo"].ToString()),
					mcdda = Convert.ToInt32(row["mcdda"].ToString()),
					mcdyr = Convert.ToInt32(row["mcdyr"].ToString()),
					m1dfac = Convert.ToDecimal(row["m1dfac"].ToString()),
					mrem1 = row["mrem1"].ToString().Trim(),
					mrem2 = row["mrem2"].ToString().Trim(),
					mmagcd = row["mmagcd"].ToString().Trim(),
					mathom = row["mathom"].ToString().Trim(),
					mdesc1 = row["mdesc1"].ToString().Trim(),
					mdesc2 = row["mdesc2"].ToString().Trim(),
					mdesc3 = row["mdesc3"].ToString().Trim(),
					mdesc4 = row["mdesc4"].ToString().Trim(),
					mfairv = Convert.ToInt32(row["mfairv"].ToString()),
					mlgity = row["mlgity"].ToString().Trim(),
					mlgiyr = Convert.ToInt32(row["mlgiyr"].ToString()),
					mlgnoN = Convert.ToInt32(row["mlgno#"].ToString()),
					mlgno2 = Convert.ToInt32(row["mlgno2"].ToString()),
					msubdv = row["msubdv"].ToString().Trim(),
					msellp = Convert.ToInt32(row["msellp"].ToString()),
					m2dfac = Convert.ToDecimal(row["m2dfac"].ToString()),
					minit = row["minit"].ToString().Trim(),
					minspd = Convert.ToInt32(row["minspd"].ToString()),
					mswl = Convert.ToInt32(row["mswl"].ToString()),
					mtutil = Convert.ToInt32(row["mtutil"].ToString()),
					mnbadj = Convert.ToDecimal(row["mnbadj"].ToString()),
					massl = Convert.ToInt32(row["massl"].ToString()),
					macsf = Convert.ToInt32(row["macsf"].ToString()),
					mcomm1 = row["mcomm1"].ToString().Trim(),
					mcomm2 = row["mcomm2"].ToString().Trim(),
					mcomm3 = row["mcomm3"].ToString().Trim(),
					macct = Convert.ToInt32(row["macct"].ToString()),
					mexwl2 = Convert.ToInt32(row["mexwl2"].ToString()),
					mcalc = row["mcalc"].ToString().Trim(),
					mfill4 = row["mfill4"].ToString().Trim(),
					mtbv = Convert.ToInt32(row["mtbv"].ToString()),
					mtbas = Convert.ToInt32(row["mtbas"].ToString()),
					mtfbas = Convert.ToInt32(row["mtfbas"].ToString()),
					mtplum = Convert.ToInt32(row["mtplum"].ToString()),
					mtheat = Convert.ToInt32(row["mtheat"].ToString()),
					mtac = Convert.ToInt32(row["mtac"].ToString()),
					mtfp = Convert.ToInt32(row["mtfp"].ToString()),
					mtfl = Convert.ToInt32(row["mtfl"].ToString()),
					mtbi = Convert.ToInt32(row["mtbi"].ToString()),
					mttadd = Convert.ToInt32(row["mttadd"].ToString()),
					mtsubt = Convert.ToInt32(row["mtsubt"].ToString()),
					mtotbv = Convert.ToInt32(row["mtotbv"].ToString()),
					musrid = row["musrid"].ToString().Trim(),
					mbasa = Convert.ToDecimal(row["mbasa"].ToString()),
					mtota = Convert.ToDecimal(row["mtota"].ToString()),
					mpsf = Convert.ToDecimal(row["mpsf"].ToString()),
					minwll = row["minwll"].ToString().Trim(),
					mfloor = row["mfloor"].ToString().Trim(),
					myrblt = Convert.ToInt32(row["myrblt"].ToString()),
					mcnst1 = row["mcnst1"].ToString().Trim(),
					mcnst2 = row["mcnst2"].ToString().Trim(),
					masslu = Convert.ToInt32(row["masslu"].ToString()),
					mmosld = Convert.ToInt32(row["mmosld"].ToString()),
					mdasld = Convert.ToInt32(row["mdasld"].ToString()),
					myrsld = Convert.ToInt32(row["myrsld"].ToString()),
					mtime = Convert.ToInt32(row["mtime"].ToString()),
					mhseN2 = row["mhse#2"].ToString().Trim(),
					m1adj = Convert.ToDecimal(row["m1adj"].ToString()),
					m2adj = Convert.ToDecimal(row["m2adj"].ToString()),
					mlgbkc = row["mlgbkc"].ToString().Trim(),
					mlgbkN = row["mlgbk#"].ToString().Trim(),
					mlgpgN = Convert.ToInt32(row["mlgpg#"].ToString()),
					meffag = Convert.ToInt32(row["meffag"].ToString()),
					mpcomp = Convert.ToDecimal(row["mpcomp"].ToString()),
					msttyp = row["msttyp"].ToString().Trim(),
					msdirs = row["msdirs"].ToString().Trim(),
					m1rate = Convert.ToDecimal(row["m1rate"].ToString()),
					m2rate = Convert.ToDecimal(row["m2rate"].ToString()),
					mfuncd = Convert.ToDecimal(row["mfuncd"].ToString()),
					mecond = Convert.ToDecimal(row["mecond"].ToString()),
					mnbrhd = Convert.ToInt32(row["mnbrhd"].ToString()),
					muser1 = row["muser1"].ToString().Trim(),
					muser2 = row["muser2"].ToString().Trim(),
					mdbook = row["mdbook"].ToString().Trim(),
					mdpage = Convert.ToInt32(row["mdpage"].ToString()),
					mwbook = row["mwbook"].ToString().Trim(),
					mwpage = Convert.ToInt32(row["mwpage"].ToString()),
					mdcode = row["mdcode"].ToString().Trim(),
					mwcode = row["mwcode"].ToString().Trim(),
					mmortc = Convert.ToInt32(row["mmortc"].ToString()),
					mfill7 = row["mfill7"].ToString().Trim(),
					macreN = Convert.ToDecimal(row["macre#"].ToString()),
					mgispn = row["mgispn"].ToString().Trim(),
					muser3 = row["muser3"].ToString().Trim(),
					muser4 = row["muser4"].ToString().Trim(),
					mimadj = Convert.ToInt32(row["mimadj"].ToString()),
					mcdrdt = Convert.ToInt32(row["mcdrdt"].ToString()),
					mmnud = Convert.ToInt32(row["mmnud"].ToString()),
					mmnnud = Convert.ToInt32(row["mmnnud"].ToString()),
					mss1 = Convert.ToInt32(row["mss1"].ToString()),
					mpcode = row["mpcode"].ToString().Trim(),
					mpbook = row["mpbook"].ToString().Trim(),
					mppage = Convert.ToInt32(row["mppage"].ToString()),
					mss2 = Convert.ToInt32(row["mss2"].ToString()),
					massm = Convert.ToInt32(row["massm"].ToString()),
					mfill9 = row["mfill9"].ToString().Trim(),
					mgrntr = row["mgrntr"].ToString().Trim(),
					mcvmo = Convert.ToInt32(row["mcvmo"].ToString()),
					mcvda = Convert.ToInt32(row["mcvda"].ToString()),
					mcvyr = Convert.ToInt32(row["mcvyr"].ToString()),
					mprout = row["mprout"].ToString().Trim(),
					mperr = row["mperr"].ToString().Trim(),
					mtbimp = Convert.ToInt32(row["mtbimp"].ToString()),
					mpuse = Convert.ToInt32(row["mpuse"].ToString()),
					mcvexp = row["mcvexp"].ToString().Trim(),
					metxyr = Convert.ToInt32(row["metxyr"].ToString()),
					mqapch = Convert.ToDecimal(row["mqapch"].ToString()),
					mqafil = row["mqafil"].ToString().Trim(),
					mpict = row["mpict"].ToString().Trim(),
					meacre = Convert.ToDecimal(row["meacre"].ToString()),
					mprcit = row["mprcit"].ToString().Trim(),
					mprsta = row["mprsta"].ToString().Trim(),
					mprzp1 = Convert.ToInt32(row["mprzp1"].ToString()),
					mprzp4 = row["mprzp4"].ToString().Trim(),
					mfpN = Convert.ToInt32(row["mfp#"].ToString()),
					msfpN = Convert.ToInt32(row["msfp#"].ToString()),
					mflN = Convert.ToInt32(row["mfl#"].ToString()),
					msflN = Convert.ToInt32(row["msfl#"].ToString()),
					mmflN = Convert.ToInt32(row["mmfl#"].ToString()),
					miofpN = Convert.ToInt32(row["miofp#"].ToString()),
					mstorN = Convert.ToDecimal(row["mstor#"].ToString()),
					mascom = row["mascom"].ToString().Trim(),
					mhrphN = Convert.ToInt32(row["mhrph#"].ToString()),
					mhrdat = Convert.ToInt32(row["mhrdat"].ToString()),
					mhrtim = Convert.ToInt32(row["mhrtim"].ToString()),
					mhrnam = row["mhrnam"].ToString().Trim(),
					mhrses = row["mhrses"].ToString().Trim(),
					mhidpc = row["mhidpc"].ToString().Trim(),
					mhidnm = row["mhidnm"].ToString().Trim(),
					mcamo = Convert.ToInt32(row["mcamo"].ToString()),
					mcada = Convert.ToInt32(row["mcada"].ToString()),
					mcayr = Convert.ToInt32(row["mcayr"].ToString()),
					moldoc = Convert.ToInt32(row["moldoc"].ToString()),
					InteriorWallType = row["walls"].ToString().Trim(),
					InteriorWallTyp2 = row["wall2"].ToString().Trim(),
					InteriorWallTyp3 = row["wall3"].ToString().Trim(),
					InteriorWallTyp4 = row["wall4"].ToString().Trim(),
					FloorType = row["floors"].ToString().Trim(),
					FloorTyp2 = row["floor2"].ToString().Trim(),
					FloorTyp3 = row["floor3"].ToString().Trim(),
					FloorTyp4 = row["floor4"].ToString().Trim(),

					SalesDate = String.Format("{0}/{1}/{2}",
						Convert.ToInt32(row["mmosld"].ToString().Trim()),
						Convert.ToInt32(row["mdasld"].ToString().Trim()),
						Convert.ToInt32(row["myrsld"].ToString().Trim())),

					orig_mrecid = row["mrecid"].ToString().Trim(),
					orig_mrecno = record,
					orig_mdwell = card,
					orig_mmap = row["mmap"].ToString().Trim(),
					orig_mlnam = row["mlnam"].ToString().Trim(),
					orig_mfnam = row["mfnam"].ToString().Trim(),
					orig_madd1 = row["madd1"].ToString().Trim(),
					orig_madd2 = row["madd2"].ToString().Trim(),
					orig_mcity = row["mcity"].ToString().Trim(),
					orig_mstate = row["mstate"].ToString().Trim(),
					orig_mzip5 = Convert.ToInt32(row["mzip5"].ToString()),
					orig_mzip4 = row["mzip4"].ToString().Trim(),
					orig_macre = row["macre"].ToString().Trim(),
					orig_mzone = row["mzone"].ToString().Trim(),
					orig_mluse = row["mluse"].ToString().Trim(),
					orig_moccup = Convert.ToInt32(row["moccup"].ToString()),
					orig_mstory = row["mstory"].ToString().Trim(),
					orig_mage = Convert.ToInt32(row["mage"].ToString()),
					orig_mcond = row["mcond"].ToString().Trim(),
					orig_mclass = row["mclass"].ToString().Trim(),
					orig_mfactr = Convert.ToDecimal(row["mfactr"].ToString()),
					orig_mdeprc = Convert.ToDecimal(row["mdeprc"].ToString()),
					orig_mfound = Convert.ToInt32(row["mfound"].ToString()),
					orig_mexwll = Convert.ToInt32(row["mexwll"].ToString()),
					orig_mrooft = Convert.ToInt32(row["mrooft"].ToString()),
					orig_mroofg = Convert.ToInt32(row["mroofg"].ToString()),
					orig_mNdunt = Convert.ToInt32(row["m#dunt"].ToString()),
					orig_mNroom = Convert.ToInt32(row["m#room"].ToString()),
					orig_mNbr = Convert.ToInt32(row["m#br"].ToString()),
					orig_mNfbth = Convert.ToInt32(row["m#fbth"].ToString()),
					orig_mNhbth = Convert.ToInt32(row["m#hbth"].ToString()),
					orig_mfp2 = row["mfp2"].ToString().Trim(),
					orig_mltrcd = row["mltrcd"].ToString().Trim(),
					orig_mheat = Convert.ToInt32(row["mheat"].ToString()),
					orig_mfuel = Convert.ToInt32(row["mfuel"].ToString()),
					orig_mac = row["mac"].ToString().Trim(),
					orig_mfp1 = row["mfp1"].ToString().Trim(),
					orig_mcdr = row["mcdr"].ToString().Trim(),
					orig_mekit = Convert.ToInt32(row["mekit"].ToString()),
					orig_mbastp = Convert.ToInt32(row["mbastp"].ToString()),
					orig_mpbtot = Convert.ToDecimal(row["mpbtot"].ToString()),
					orig_msbtot = Convert.ToInt32(row["msbtot"].ToString()),
					orig_mpbfin = Convert.ToDecimal(row["mpbfin"].ToString()),
					orig_msbfin = Convert.ToInt32(row["msbfin"].ToString()),
					orig_mbrate = Convert.ToDecimal(row["mbrate"].ToString()),
					orig_mNflue = Convert.ToInt32(row["m#flue"].ToString()),
					orig_mflutp = row["mflutp"].ToString().Trim(),
					orig_mgart = Convert.ToInt32(row["mgart"].ToString()),
					orig_mgarNc = Convert.ToInt32(row["mgar#c"].ToString()),
					orig_mcarpt = Convert.ToInt32(row["mcarpt"].ToString()),
					orig_mcarNc = Convert.ToInt32(row["mcar#c"].ToString()),
					orig_mbiNc = Convert.ToInt32(row["mbi#c"].ToString()),
					orig_mrow = Convert.ToInt32(row["mrow"].ToString()),
					orig_mease = Convert.ToInt32(row["mease"].ToString()),
					orig_mwater = Convert.ToInt32(row["mwater"].ToString()),
					orig_msewer = Convert.ToInt32(row["msewer"].ToString()),
					orig_mgas = row["mgas"].ToString().Trim(),
					orig_melec = row["melec"].ToString().Trim(),
					orig_mterrn = Convert.ToInt32(row["mterrn"].ToString()),
					orig_mchar = Convert.ToInt32(row["mchar"].ToString()),
					orig_motdes = row["motdes"].ToString().Trim(),
					orig_mgart2 = Convert.ToInt32(row["mgart2"].ToString()),
					orig_mgarN2 = Convert.ToInt32(row["mgar#2"].ToString()),
					orig_mdatlg = Convert.ToInt32(row["mdatlg"].ToString()),
					orig_mdatpr = Convert.ToInt32(row["mdatpr"].ToString()),
					orig_mintyp = row["mintyp"].ToString().Trim(),
					orig_mintyr = Convert.ToInt32(row["mintyr"].ToString()),
					orig_minnoN = Convert.ToInt32(row["minno#"].ToString()),
					orig_minno2 = Convert.ToInt32(row["minno2"].ToString()),
					orig_mdsufx = row["mdsufx"].ToString().Trim(),
					orig_mwsufx = row["mwsufx"].ToString().Trim(),
					orig_mpsufx = row["mpsufx"].ToString().Trim(),
					orig_mimprv = Convert.ToInt32(row["mimprv"].ToString()),
					orig_mtotld = Convert.ToInt32(row["mtotld"].ToString()),
					orig_mtotoi = Convert.ToInt32(row["mtotoi"].ToString()),
					orig_mtotpr = Convert.ToInt32(row["mtotpr"].ToString()),
					orig_massb = Convert.ToInt32(row["massb"].ToString()),
					orig_macpct = Convert.ToDecimal(row["macpct"].ToString()),
					orig_m1frnt = Convert.ToDecimal(row["m1frnt"].ToString()),
					orig_m1dpth = Convert.ToDecimal(row["m1dpth"].ToString()),
					orig_m1area = Convert.ToInt32(row["m1area"].ToString()),
					orig_mmcode = Convert.ToInt32(row["mmcode"].ToString()),
					orig_m0depr = row["m0depr"].ToString().Trim(),
					orig_m1um = row["m1um"].ToString().Trim(),
					orig_m2frnt = Convert.ToDecimal(row["m2frnt"].ToString()),
					orig_m2dpth = Convert.ToDecimal(row["m2dpth"].ToString()),
					orig_m2area = Convert.ToInt32(row["m2area"].ToString()),
					orig_mzipbr = Convert.ToInt32(row["mzipbr"].ToString()),
					orig_mdelay = row["mdelay"].ToString().Trim(),
					orig_m2um = row["m2um"].ToString().Trim(),
					orig_mstrt = row["mstrt"].ToString().Trim(),
					orig_mdirct = row["mdirct"].ToString().Trim(),
					orig_mhseN = Convert.ToInt32(row["mhse#"].ToString()),
					orig_mcdmo = Convert.ToInt32(row["mcdmo"].ToString()),
					orig_mcdda = Convert.ToInt32(row["mcdda"].ToString()),
					orig_mcdyr = Convert.ToInt32(row["mcdyr"].ToString()),
					orig_m1dfac = Convert.ToDecimal(row["m1dfac"].ToString()),
					orig_mrem1 = row["mrem1"].ToString().Trim(),
					orig_mrem2 = row["mrem2"].ToString().Trim(),
					orig_mmagcd = row["mmagcd"].ToString().Trim(),
					orig_mathom = row["mathom"].ToString().Trim(),
					orig_mdesc1 = row["mdesc1"].ToString().Trim(),
					orig_mdesc2 = row["mdesc2"].ToString().Trim(),
					orig_mdesc3 = row["mdesc3"].ToString().Trim(),
					orig_mdesc4 = row["mdesc4"].ToString().Trim(),
					orig_mfairv = Convert.ToInt32(row["mfairv"].ToString()),
					orig_mlgity = row["mlgity"].ToString().Trim(),
					orig_mlgiyr = Convert.ToInt32(row["mlgiyr"].ToString()),
					orig_mlgnoN = Convert.ToInt32(row["mlgno#"].ToString()),
					orig_mlgno2 = Convert.ToInt32(row["mlgno2"].ToString()),
					orig_msubdv = row["msubdv"].ToString().Trim(),
					orig_msellp = Convert.ToInt32(row["msellp"].ToString()),
					orig_m2dfac = Convert.ToDecimal(row["m2dfac"].ToString()),
					orig_minit = row["minit"].ToString().Trim(),
					orig_minspd = Convert.ToInt32(row["minspd"].ToString()),
					orig_mswl = Convert.ToInt32(row["mswl"].ToString()),
					orig_mtutil = Convert.ToInt32(row["mtutil"].ToString()),
					orig_mnbadj = Convert.ToDecimal(row["mnbadj"].ToString()),
					orig_massl = Convert.ToInt32(row["massl"].ToString()),
					orig_macsf = Convert.ToInt32(row["macsf"].ToString()),
					orig_mcomm1 = row["mcomm1"].ToString().Trim(),
					orig_mcomm2 = row["mcomm2"].ToString().Trim(),
					orig_mcomm3 = row["mcomm3"].ToString().Trim(),
					orig_macct = Convert.ToInt32(row["macct"].ToString()),
					orig_mexwl2 = Convert.ToInt32(row["mexwl2"].ToString()),
					orig_mcalc = row["mcalc"].ToString().Trim(),
					orig_mfill4 = row["mfill4"].ToString().Trim(),
					orig_mtbv = Convert.ToInt32(row["mtbv"].ToString()),
					orig_mtbas = Convert.ToInt32(row["mtbas"].ToString()),
					orig_mtfbas = Convert.ToInt32(row["mtfbas"].ToString()),
					orig_mtplum = Convert.ToInt32(row["mtplum"].ToString()),
					orig_mtheat = Convert.ToInt32(row["mtheat"].ToString()),
					orig_mtac = Convert.ToInt32(row["mtac"].ToString()),
					orig_mtfp = Convert.ToInt32(row["mtfp"].ToString()),
					orig_mtfl = Convert.ToInt32(row["mtfl"].ToString()),
					orig_mtbi = Convert.ToInt32(row["mtbi"].ToString()),
					orig_mttadd = Convert.ToInt32(row["mttadd"].ToString()),
					orig_mtsubt = Convert.ToInt32(row["mtsubt"].ToString()),
					orig_mtotbv = Convert.ToInt32(row["mtotbv"].ToString()),
					orig_musrid = row["musrid"].ToString().Trim(),
					orig_mbasa = Convert.ToDecimal(row["mbasa"].ToString()),
					orig_mtota = Convert.ToDecimal(row["mtota"].ToString()),
					orig_mpsf = Convert.ToDecimal(row["mpsf"].ToString()),
					orig_minwll = row["minwll"].ToString().Trim(),
					orig_mfloor = row["mfloor"].ToString().Trim(),
					orig_myrblt = Convert.ToInt32(row["myrblt"].ToString()),
					orig_mcnst1 = row["mcnst1"].ToString().Trim(),
					orig_mcnst2 = row["mcnst2"].ToString().Trim(),
					orig_masslu = Convert.ToInt32(row["masslu"].ToString()),
					orig_mmosld = Convert.ToInt32(row["mmosld"].ToString()),
					orig_mdasld = Convert.ToInt32(row["mdasld"].ToString()),
					orig_myrsld = Convert.ToInt32(row["myrsld"].ToString()),
					orig_mtime = Convert.ToInt32(row["mtime"].ToString()),
					orig_mhseN2 = row["mhse#2"].ToString().Trim(),
					orig_m1adj = Convert.ToDecimal(row["m1adj"].ToString()),
					orig_m2adj = Convert.ToDecimal(row["m2adj"].ToString()),
					orig_mlgbkc = row["mlgbkc"].ToString().Trim(),
					orig_mlgbkN = row["mlgbk#"].ToString().Trim(),
					orig_mlgpgN = Convert.ToInt32(row["mlgpg#"].ToString()),
					orig_meffag = Convert.ToInt32(row["meffag"].ToString()),
					orig_mpcomp = Convert.ToDecimal(row["mpcomp"].ToString()),
					orig_msttyp = row["msttyp"].ToString().Trim(),
					orig_msdirs = row["msdirs"].ToString().Trim(),
					orig_m1rate = Convert.ToDecimal(row["m1rate"].ToString()),
					orig_m2rate = Convert.ToDecimal(row["m2rate"].ToString()),
					orig_mfuncd = Convert.ToDecimal(row["mfuncd"].ToString()),
					orig_mecond = Convert.ToDecimal(row["mecond"].ToString()),
					orig_mnbrhd = Convert.ToInt32(row["mnbrhd"].ToString()),
					orig_muser1 = row["muser1"].ToString().Trim(),
					orig_muser2 = row["muser2"].ToString().Trim(),
					orig_mdbook = row["mdbook"].ToString().Trim(),
					orig_mdpage = Convert.ToInt32(row["mdpage"].ToString()),
					orig_mwbook = row["mwbook"].ToString().Trim(),
					orig_mwpage = Convert.ToInt32(row["mwpage"].ToString()),
					orig_mdcode = row["mdcode"].ToString().Trim(),
					orig_mwcode = row["mwcode"].ToString().Trim(),
					orig_mmortc = Convert.ToInt32(row["mmortc"].ToString()),
					orig_mfill7 = row["mfill7"].ToString().Trim(),
					orig_macreN = Convert.ToDecimal(row["macre#"].ToString()),
					orig_mgispn = row["mgispn"].ToString().Trim(),
					orig_muser3 = row["muser3"].ToString().Trim(),
					orig_muser4 = row["muser4"].ToString().Trim(),
					orig_mimadj = Convert.ToInt32(row["mimadj"].ToString()),
					orig_mcdrdt = Convert.ToInt32(row["mcdrdt"].ToString()),
					orig_mmnud = Convert.ToInt32(row["mmnud"].ToString()),
					orig_mmnnud = Convert.ToInt32(row["mmnnud"].ToString()),
					orig_mss1 = Convert.ToInt32(row["mss1"].ToString()),
					orig_mpcode = row["mpcode"].ToString().Trim(),
					orig_mpbook = row["mpbook"].ToString().Trim(),
					orig_mppage = Convert.ToInt32(row["mppage"].ToString()),
					orig_mss2 = Convert.ToInt32(row["mss2"].ToString()),
					orig_massm = Convert.ToInt32(row["massm"].ToString()),
					orig_mfill9 = row["mfill9"].ToString().Trim(),
					orig_mgrntr = row["mgrntr"].ToString().Trim(),
					orig_mcvmo = Convert.ToInt32(row["mcvmo"].ToString()),
					orig_mcvda = Convert.ToInt32(row["mcvda"].ToString()),
					orig_mcvyr = Convert.ToInt32(row["mcvyr"].ToString()),
					orig_mprout = row["mprout"].ToString().Trim(),
					orig_mperr = row["mperr"].ToString().Trim(),
					orig_mtbimp = Convert.ToInt32(row["mtbimp"].ToString()),
					orig_mpuse = Convert.ToInt32(row["mpuse"].ToString()),
					orig_mcvexp = row["mcvexp"].ToString().Trim(),
					orig_metxyr = Convert.ToInt32(row["metxyr"].ToString()),
					orig_mqapch = Convert.ToDecimal(row["mqapch"].ToString()),
					orig_mqafil = row["mqafil"].ToString().Trim(),
					orig_mpict = row["mpict"].ToString().Trim(),
					orig_meacre = Convert.ToDecimal(row["meacre"].ToString()),
					orig_mprcit = row["mprcit"].ToString().Trim(),
					orig_mprsta = row["mprsta"].ToString().Trim(),
					orig_mprzp1 = Convert.ToInt32(row["mprzp1"].ToString()),
					orig_mprzp4 = row["mprzp4"].ToString().Trim(),
					orig_mfpN = Convert.ToInt32(row["mfp#"].ToString()),
					orig_msfpN = Convert.ToInt32(row["msfp#"].ToString()),
					orig_mflN = Convert.ToInt32(row["mfl#"].ToString()),
					orig_msflN = Convert.ToInt32(row["msfl#"].ToString()),
					orig_mmflN = Convert.ToInt32(row["mmfl#"].ToString()),
					orig_miofpN = Convert.ToInt32(row["miofp#"].ToString()),
					orig_mstorN = Convert.ToDecimal(row["mstor#"].ToString()),
					orig_mascom = row["mascom"].ToString().Trim(),
					orig_mhrphN = Convert.ToInt32(row["mhrph#"].ToString()),
					orig_mhrdat = Convert.ToInt32(row["mhrdat"].ToString()),
					orig_mhrtim = Convert.ToInt32(row["mhrtim"].ToString()),
					orig_mhrnam = row["mhrnam"].ToString().Trim(),
					orig_mhrses = row["mhrses"].ToString().Trim(),
					orig_mhidpc = row["mhidpc"].ToString().Trim(),
					orig_mhidnm = row["mhidnm"].ToString().Trim(),
					orig_mcamo = Convert.ToInt32(row["mcamo"].ToString()),
					orig_mcada = Convert.ToInt32(row["mcada"].ToString()),
					orig_mcayr = Convert.ToInt32(row["mcayr"].ToString()),
					orig_moldoc = Convert.ToInt32(row["moldoc"].ToString()),

					SalesDateS = String.Format("{0} / {1}",
					   Convert.ToInt32(row["mmosld"].ToString().Trim()),
					   Convert.ToInt32(row["myrsld"].ToString().Trim())),

					SalesDateL = String.Format("{0}/{1}/{2}",
					   Convert.ToInt32(row["mmosld"].ToString()).ToString().PadLeft(2, '0'),
					   Convert.ToInt32(row["mdasld"].ToString()).ToString().PadLeft(2, '0'),
					   Convert.ToInt32(row["myrsld"].ToString()))
				};

				var gaslog = new GasLogCollection(_fox, _parcel.mrecno, _parcel.mdwell);
				gaslog.GetGasLog(_fox, _parcel.mrecno, _parcel.mdwell);
				_parcel.GasLogRecords = gaslog;

				if (_parcel.GasLogRecords.Count > 0)
				{
					_parcel.GasLogFP = Convert.ToInt32(_parcel.GasLogRecords[0].NbrGasFP.ToString());
					_parcel.orig_GasLogFP = Convert.ToInt32(_parcel.GasLogRecords[0].NbrGasFP.ToString());
				}
				else
				{
					_parcel.GasLogFP = 0;
					_parcel.orig_GasLogFP = 0;
				}

				string chkStatus = String.Empty;
				StringBuilder checkStatus = new StringBuilder();
				checkStatus.Append(String.Format("select icstatus from parrevlib.{0}irchgd where icrecno = {1} and iccard = {2} and icseqno = 1 ",
					MainForm.localPreFix, _parcel.mrecno, _parcel.mdwell));
				try
				{
					DataSet cks = _fox.DBConnection.RunSelectStatement(checkStatus.ToString());

					if (cks.Tables[0].Rows.Count > 0)
					{
						chkStatus = cks.Tables[0].Rows[0]["icstatus"].ToString();
					}
				}
				catch
				{
				}

				//parcel.AttachedMapRecords = AttachedMapCollection.Factory(_parcel, _fox);

				var interiorImp = new InteriorImprovementCollection(_fox);
				interiorImp.GetImprovement(_fox, _parcel.mrecno, _parcel.mdwell);
				_parcel.InteriorImpRecords = interiorImp;

				//var parcelNotes = new NoteDataCollection(_fox, _parcel.mrecno, _parcel.mdwell);
				//parcelNotes.getNotes(_fox, _parcel.mrecno, _parcel.mdwell);
				//_parcel.ParcelNotes = parcelNotes;

				var land = new LandDataCollection(_fox);
				land.GetLand(_fox, _parcel.mrecno, _parcel.mdwell);
				_parcel.LandRecords = land;

				var impr = new ImprDataCollection(_fox, _parcel.mrecno, _parcel.mdwell);
				impr.getImpr();
				_parcel.ImprRecords = impr;

				//var valuesTrackingRecords = new ValuesTrackingCollection(_fox, _parcel.mrecno, _parcel.mdwell);
				//valuesTrackingRecords.GetValuesTracking();
				//_parcel.ValuesTrackingRecords = valuesTrackingRecords;

				_parcel.BuildSketchData();

				_parcel.updatestatus = chkStatus.Trim();

				//var salesHistoryRecords = new SalesHistoryCollection(_fox, _parcel.mrecno, _parcel.mdwell);
				//salesHistoryRecords.GetSalesHistory(_parcel.mrecno, _parcel.mdwell);
				//_parcel.SaleHistory = salesHistoryRecords;

				_parcel.orig_sub911Addr = _parcel.sub911Addr;

				_parcel.SetOriginalValues();
				_parcel.IsInCalculateMode = true;
				_parcel.CalculateParcel();

				string tstWall = _parcel.minwll.Trim();

				_parcel.ValidRecord = true;
			}

			return _parcel;
		}

		public Bitmap GetSketchImage()
		{
			return GetSketchImage(400);
		}

		public Bitmap GetSketchImage(int bitmapWidth, int bitmapHeight, int sketchSizeXinPixels, int sketchSizeYinPixels, int sketchSizeInPixels, out float scale)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif

			BuildingSketcher sketcher = new BuildingSketcher(Sketch.BuildingSections);
			return sketcher.DrawSketch(bitmapWidth, bitmapHeight, sketchSizeXinPixels, sketchSizeYinPixels, sketchSizeInPixels, out scale);
		}

		public Bitmap GetSketchImage(int sketchSizeInPixels)
		{
			float scale = 1.0f;
			return GetSketchImage(sketchSizeInPixels, sketchSizeInPixels, sketchSizeInPixels, sketchSizeInPixels, sketchSizeInPixels, out scale);
		}

		private void SetOriginalValues()
		{
			if (this.m1um.Trim() == String.Empty)
			{
				this.orig_curVal1 = 0;
			}
			if (this.m1um == "F")
			{
				this.orig_curVal1 = Convert.ToInt32(Convert.ToInt32((((this.m1frnt * this.m1rate) * this.m1dfac) * (1 + this.m1adj))).RoundHundredsToString().Replace(",", ""));
			}
			if (this.m1um == "S")
			{
				int chkArea1 = 0;
				if (this.m1area > 0 && this.m1frnt == 0 && this.m1dpth == 0)
				{
					chkArea1 = this.m1area;
					this.curVal1 = Convert.ToInt32((chkArea1 * this.m1rate) * (1 + this.m1adj));
				}

				if (chkArea1 == 0)
				{
					chkArea1 = Convert.ToInt32(this.m1frnt * this.m1dpth);
					if (this.m1area != chkArea1 && this.m1area > 0)
					{
						this.m1area = chkArea1;
						this.curVal1 = Convert.ToInt32((chkArea1 * this.m1rate) * (1 + this.m1adj));
					}
				}

				if (chkArea1 > 0)
				{
					this.curVal1 = Convert.ToInt32((chkArea1 * this.m1rate) * (1 + this.m1adj));
				}
			}
			if (this.m2um.Trim() == String.Empty)
			{
				this.orig_curVal2 = 0;
			}
			if (this.m2um == "L")
			{
				this.orig_curVal2 = Convert.ToInt32(this.m2rate * (1 + this.m2adj));
			}

			if (this.m2um == "F")
			{
				this.orig_curVal2 = Convert.ToInt32(Convert.ToInt32((((this.m2frnt * this.m2rate) * this.m2dfac) * (1 + this.m2adj))).RoundHundredsToString().Replace(",", ""));
			}
			if (this.m2um == "S")
			{
				int chkArea2 = 0;
				if (this.m2area > 0 && this.m2frnt == 0 && this.m2dpth == 0)
				{
					chkArea2 = this.m2area;
					this.curVal2 = Convert.ToInt32((chkArea2 * this.m2rate) * (1 + this.m2adj));
				}

				if (chkArea2 == 0)
				{
					chkArea2 = Convert.ToInt32(this.m2frnt * this.m2dpth);
					if (this.m2area != chkArea2 && this.m2area > 0)
					{
						this.m2area = chkArea2;
						this.curVal2 = Convert.ToInt32((chkArea2 * this.m2rate) * (1 + this.m2adj));
					}
				}

				if (chkArea2 > 0)
				{
					this.curVal2 = Convert.ToInt32((chkArea2 * this.m2rate) * (1 + this.m2adj));
				}
			}
			if (this.m2um == "L")
			{
				this.orig_curVal2 = Convert.ToInt32(this.m2rate * (1 + this.m2adj));
			}

			if (CamraSupport.ResidentialOccupancies.Contains(this.moccup) && this.moccup != 16)
			{
				computedFactor = Decimal.Round((GetClassValue(this.Class) + this.Factor), 2);
				orig_computedFactor = Decimal.Round((GetClassValue(this.Class) + this.Factor), 2);
			}

			if (CamraSupport.CommercialOccupancies.Contains(this.moccup) && this.moccup != 26)
			{
				computedFactor = 0;
				orig_computedFactor = 0;
			}
			if (CamraSupport.TaxExemptOccupancies.Contains(this.moccup))
			{
				computedFactor = 0;
				orig_computedFactor = 0;
			}

			if (this.moccup == 16 || this.moccup == 26)
			{
				computedFactor = 0;
				orig_computedFactor = 0;
			}

			if (macpct != 0 && macsf == 0)
			{
				macsf = Convert.ToInt32(macpct * mtota);
			}

			if (macpct == 0 && macsf > 0)
			{
				macpct = Convert.ToDecimal(macsf / mtota);
			}

			if (CamraSupport.ResidentialOccupancies.Contains(this.moccup))
			{
				if (mtbas != 0)
				{
					//orig_BasementArea = (mtbas / CamraSupport.BasementRate);
					orig_BasementArea = mbasa;
					if (mpbtot == 0)
					{
						orig_BasementPercentage = Math.Round((msbtot / mbasa), 2);
					}

					//if (mbasa == 0)
					//{
					//    orig_BasementPercentage = orig_BasementArea / orig_BasementArea;
					//}
					BasementArea = orig_BasementArea;
					BasementPercentage = orig_BasementPercentage;
				}
				else
				{
					orig_BasementArea = 0;
					orig_BasementPercentage = 0;
					BasementArea = 0;
					BasementPercentage = 0;
				}
			}
			else
			{
				orig_BasementArea = 0;
				orig_BasementPercentage = 0;
				BasementArea = 0;
				BasementPercentage = 0;
			}

			if (mtfbas != 0 && orig_BasementArea != 0)
			{
				orig_FinBasementArea = (mtfbas / mbrate);
				orig_FinBasementPercentage = orig_FinBasementArea / orig_BasementArea;
			}

			_isDirtyCheckingOn = true;
		}
	}
}