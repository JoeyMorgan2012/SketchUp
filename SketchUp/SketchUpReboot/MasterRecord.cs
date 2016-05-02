    using SketchUp;
    using SWallTech;	
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Linq;
	using System.Linq;
	using System.Reflection;

namespace SketchUpReboot
{
    public partial class MasterRecord
    {
        public MasterRecord()
        {
        }

        public string M0DEPR
        {
            get
            {
                return _M0DEPR;
            }
            set
            {
                if ((_M0DEPR != value))
                {
                    _M0DEPR = value;
                }
            }
        }

        public decimal M1ADJ
        {
            get
            {
                return _M1ADJ;
            }
            set
            {
                if ((_M1ADJ != value))
                {
                    _M1ADJ = value;
                }
            }
        }

        public decimal M1AREA
        {
            get
            {
                return _M1AREA;
            }
            set
            {
                if ((_M1AREA != value))
                {
                    _M1AREA = value;
                }
            }
        }

        public decimal M1DFAC
        {
            get
            {
                return _M1DFAC;
            }
            set
            {
                if ((_M1DFAC != value))
                {
                    _M1DFAC = value;
                }
            }
        }

        public decimal M1DPTH
        {
            get
            {
                return _M1DPTH;
            }
            set
            {
                if ((_M1DPTH != value))
                {
                    _M1DPTH = value;
                }
            }
        }

        public decimal M1FRNT
        {
            get
            {
                return _M1FRNT;
            }
            set
            {
                if ((_M1FRNT != value))
                {
                    _M1FRNT = value;
                }
            }
        }

        public decimal M1RATE
        {
            get
            {
                return _M1RATE;
            }
            set
            {
                if ((_M1RATE != value))
                {
                    _M1RATE = value;
                }
            }
        }

        public string M1UM
        {
            get
            {
                return _M1UM;
            }
            set
            {
                if ((_M1UM != value))
                {
                    _M1UM = value;
                }
            }
        }

        public decimal M2ADJ
        {
            get
            {
                return _M2ADJ;
            }
            set
            {
                if ((_M2ADJ != value))
                {
                    _M2ADJ = value;
                }
            }
        }

        public decimal M2AREA
        {
            get
            {
                return _M2AREA;
            }
            set
            {
                if ((_M2AREA != value))
                {
                    _M2AREA = value;
                }
            }
        }

        public decimal M2DFAC
        {
            get
            {
                return _M2DFAC;
            }
            set
            {
                if ((_M2DFAC != value))
                {
                    _M2DFAC = value;
                }
            }
        }

        public decimal M2DPTH
        {
            get
            {
                return _M2DPTH;
            }
            set
            {
                if ((_M2DPTH != value))
                {
                    _M2DPTH = value;
                }
            }
        }

        public decimal M2FRNT
        {
            get
            {
                return _M2FRNT;
            }
            set
            {
                if ((_M2FRNT != value))
                {
                    _M2FRNT = value;
                }
            }
        }

        public decimal M2RATE
        {
            get
            {
                return _M2RATE;
            }
            set
            {
                if ((_M2RATE != value))
                {
                    _M2RATE = value;
                }
            }
        }

        public string M2UM
        {
            get
            {
                return _M2UM;
            }
            set
            {
                if ((_M2UM != value))
                {
                    _M2UM = value;
                }
            }
        }

        public string MAC
        {
            get
            {
                return _MAC;
            }
            set
            {
                if ((_MAC != value))
                {
                    _MAC = value;
                }
            }
        }

        public decimal MACCT
        {
            get
            {
                return _MACCT;
            }
            set
            {
                if ((_MACCT != value))
                {
                    _MACCT = value;
                }
            }
        }

        public decimal MACPCT
        {
            get
            {
                return _MACPCT;
            }
            set
            {
                if ((_MACPCT != value))
                {
                    _MACPCT = value;
                }
            }
        }

        public string MACRE
        {
            get
            {
                return _MACRE;
            }
            set
            {
                if ((_MACRE != value))
                {
                    _MACRE = value;
                }
            }
        }

        public decimal MACRE_no
        {
            get
            {
                return _MACRE_no;
            }
            set
            {
                if ((_MACRE_no != value))
                {
                    _MACRE_no = value;
                }
            }
        }

        public decimal MACSF
        {
            get
            {
                return _MACSF;
            }
            set
            {
                if ((_MACSF != value))
                {
                    _MACSF = value;
                }
            }
        }

        public string MADD1
        {
            get
            {
                return _MADD1;
            }
            set
            {
                if ((_MADD1 != value))
                {
                    _MADD1 = value;
                }
            }
        }

        public string MADD2
        {
            get
            {
                return _MADD2;
            }
            set
            {
                if ((_MADD2 != value))
                {
                    _MADD2 = value;
                }
            }
        }

        public decimal MAGE
        {
            get
            {
                return _MAGE;
            }
            set
            {
                if ((_MAGE != value))
                {
                    _MAGE = value;
                }
            }
        }

        public string MASCOM
        {
            get
            {
                return _MASCOM;
            }
            set
            {
                if ((_MASCOM != value))
                {
                    _MASCOM = value;
                }
            }
        }

        public decimal MASSB
        {
            get
            {
                return _MASSB;
            }
            set
            {
                if ((_MASSB != value))
                {
                    _MASSB = value;
                }
            }
        }

        public decimal MASSL
        {
            get
            {
                return _MASSL;
            }
            set
            {
                if ((_MASSL != value))
                {
                    _MASSL = value;
                }
            }
        }

        public decimal MASSLU
        {
            get
            {
                return _MASSLU;
            }
            set
            {
                if ((_MASSLU != value))
                {
                    _MASSLU = value;
                }
            }
        }

        public decimal MASSM
        {
            get
            {
                return _MASSM;
            }
            set
            {
                if ((_MASSM != value))
                {
                    _MASSM = value;
                }
            }
        }

        public string MATHOM
        {
            get
            {
                return _MATHOM;
            }
            set
            {
                if ((_MATHOM != value))
                {
                    _MATHOM = value;
                }
            }
        }

        public decimal MBASA
        {
            get
            {
                return _MBASA;
            }
            set
            {
                if ((_MBASA != value))
                {
                    _MBASA = value;
                }
            }
        }

        public decimal MBASTP
        {
            get
            {
                return _MBASTP;
            }
            set
            {
                if ((_MBASTP != value))
                {
                    _MBASTP = value;
                }
            }
        }

        public decimal MBI_no_C
        {
            get
            {
                return _MBI_no_C;
            }
            set
            {
                if ((_MBI_no_C != value))
                {
                    _MBI_no_C = value;
                }
            }
        }

        public decimal MBRATE
        {
            get
            {
                return _MBRATE;
            }
            set
            {
                if ((_MBRATE != value))
                {
                    _MBRATE = value;
                }
            }
        }

        public decimal MCADA
        {
            get
            {
                return _MCADA;
            }
            set
            {
                if ((_MCADA != value))
                {
                    _MCADA = value;
                }
            }
        }

        public string MCALC
        {
            get
            {
                return _MCALC;
            }
            set
            {
                if ((_MCALC != value))
                {
                    _MCALC = value;
                }
            }
        }

        public decimal MCAMO
        {
            get
            {
                return _MCAMO;
            }
            set
            {
                if ((_MCAMO != value))
                {
                    _MCAMO = value;
                }
            }
        }

        public decimal MCARPT
        {
            get
            {
                return _MCARPT;
            }
            set
            {
                if ((_MCARPT != value))
                {
                    _MCARPT = value;
                }
            }
        }

        public decimal MCAR_no_C
        {
            get
            {
                return _MCAR_no_C;
            }
            set
            {
                if ((_MCAR_no_C != value))
                {
                    _MCAR_no_C = value;
                }
            }
        }

        public decimal MCAYR
        {
            get
            {
                return _MCAYR;
            }
            set
            {
                if ((_MCAYR != value))
                {
                    _MCAYR = value;
                }
            }
        }

        public decimal MCDDA
        {
            get
            {
                return _MCDDA;
            }
            set
            {
                if ((_MCDDA != value))
                {
                    _MCDDA = value;
                }
            }
        }

        public decimal MCDMO
        {
            get
            {
                return _MCDMO;
            }
            set
            {
                if ((_MCDMO != value))
                {
                    _MCDMO = value;
                }
            }
        }

        public string MCDR
        {
            get
            {
                return _MCDR;
            }
            set
            {
                if ((_MCDR != value))
                {
                    _MCDR = value;
                }
            }
        }

        public decimal MCDRDT
        {
            get
            {
                return _MCDRDT;
            }
            set
            {
                if ((_MCDRDT != value))
                {
                    _MCDRDT = value;
                }
            }
        }

        public decimal MCDYR
        {
            get
            {
                return _MCDYR;
            }
            set
            {
                if ((_MCDYR != value))
                {
                    _MCDYR = value;
                }
            }
        }

        public decimal MCHAR
        {
            get
            {
                return _MCHAR;
            }
            set
            {
                if ((_MCHAR != value))
                {
                    _MCHAR = value;
                }
            }
        }

        public string MCITY
        {
            get
            {
                return _MCITY;
            }
            set
            {
                if ((_MCITY != value))
                {
                    _MCITY = value;
                }
            }
        }

        public string MCLASS
        {
            get
            {
                return _MCLASS;
            }
            set
            {
                if ((_MCLASS != value))
                {
                    _MCLASS = value;
                }
            }
        }

        public string MCNST1
        {
            get
            {
                return _MCNST1;
            }
            set
            {
                if ((_MCNST1 != value))
                {
                    _MCNST1 = value;
                }
            }
        }

        public string MCNST2
        {
            get
            {
                return _MCNST2;
            }
            set
            {
                if ((_MCNST2 != value))
                {
                    _MCNST2 = value;
                }
            }
        }

        public string MCOMM1
        {
            get
            {
                return _MCOMM1;
            }
            set
            {
                if ((_MCOMM1 != value))
                {
                    _MCOMM1 = value;
                }
            }
        }

        public string MCOMM2
        {
            get
            {
                return _MCOMM2;
            }
            set
            {
                if ((_MCOMM2 != value))
                {
                    _MCOMM2 = value;
                }
            }
        }

        public string MCOMM3
        {
            get
            {
                return _MCOMM3;
            }
            set
            {
                if ((_MCOMM3 != value))
                {
                    _MCOMM3 = value;
                }
            }
        }

        public string MCOND
        {
            get
            {
                return _MCOND;
            }
            set
            {
                if ((_MCOND != value))
                {
                    _MCOND = value;
                }
            }
        }

        public decimal MCVDA
        {
            get
            {
                return _MCVDA;
            }
            set
            {
                if ((_MCVDA != value))
                {
                    _MCVDA = value;
                }
            }
        }

        public string MCVEXP
        {
            get
            {
                return _MCVEXP;
            }
            set
            {
                if ((_MCVEXP != value))
                {
                    _MCVEXP = value;
                }
            }
        }

        public decimal MCVMO
        {
            get
            {
                return _MCVMO;
            }
            set
            {
                if ((_MCVMO != value))
                {
                    _MCVMO = value;
                }
            }
        }

        public decimal MCVYR
        {
            get
            {
                return _MCVYR;
            }
            set
            {
                if ((_MCVYR != value))
                {
                    _MCVYR = value;
                }
            }
        }

        public decimal MDASLD
        {
            get
            {
                return _MDASLD;
            }
            set
            {
                if ((_MDASLD != value))
                {
                    _MDASLD = value;
                }
            }
        }

        public decimal MDATLG
        {
            get
            {
                return _MDATLG;
            }
            set
            {
                if ((_MDATLG != value))
                {
                    _MDATLG = value;
                }
            }
        }

        public decimal MDATPR
        {
            get
            {
                return _MDATPR;
            }
            set
            {
                if ((_MDATPR != value))
                {
                    _MDATPR = value;
                }
            }
        }

        public string MDBOOK
        {
            get
            {
                return _MDBOOK;
            }
            set
            {
                if ((_MDBOOK != value))
                {
                    _MDBOOK = value;
                }
            }
        }

        public string MDCODE
        {
            get
            {
                return _MDCODE;
            }
            set
            {
                if ((_MDCODE != value))
                {
                    _MDCODE = value;
                }
            }
        }

        public string MDELAY
        {
            get
            {
                return _MDELAY;
            }
            set
            {
                if ((_MDELAY != value))
                {
                    _MDELAY = value;
                }
            }
        }

        public decimal MDEPRC
        {
            get
            {
                return _MDEPRC;
            }
            set
            {
                if ((_MDEPRC != value))
                {
                    _MDEPRC = value;
                }
            }
        }

        public string MDESC1
        {
            get
            {
                return _MDESC1;
            }
            set
            {
                if ((_MDESC1 != value))
                {
                    _MDESC1 = value;
                }
            }
        }

        public string MDESC2
        {
            get
            {
                return _MDESC2;
            }
            set
            {
                if ((_MDESC2 != value))
                {
                    _MDESC2 = value;
                }
            }
        }

        public string MDESC3
        {
            get
            {
                return _MDESC3;
            }
            set
            {
                if ((_MDESC3 != value))
                {
                    _MDESC3 = value;
                }
            }
        }

        public string MDESC4
        {
            get
            {
                return _MDESC4;
            }
            set
            {
                if ((_MDESC4 != value))
                {
                    _MDESC4 = value;
                }
            }
        }

        public string MDIRCT
        {
            get
            {
                return _MDIRCT;
            }
            set
            {
                if ((_MDIRCT != value))
                {
                    _MDIRCT = value;
                }
            }
        }

        public decimal MDPAGE
        {
            get
            {
                return _MDPAGE;
            }
            set
            {
                if ((_MDPAGE != value))
                {
                    _MDPAGE = value;
                }
            }
        }

        public string MDSUFX
        {
            get
            {
                return _MDSUFX;
            }
            set
            {
                if ((_MDSUFX != value))
                {
                    _MDSUFX = value;
                }
            }
        }

        public decimal MDWELL
        {
            get
            {
                return _MDWELL;
            }
            set
            {
                if ((_MDWELL != value))
                {
                    _MDWELL = value;
                }
            }
        }

        public decimal MEACRE
        {
            get
            {
                return _MEACRE;
            }
            set
            {
                if ((_MEACRE != value))
                {
                    _MEACRE = value;
                }
            }
        }

        public decimal MEASE
        {
            get
            {
                return _MEASE;
            }
            set
            {
                if ((_MEASE != value))
                {
                    _MEASE = value;
                }
            }
        }

        public decimal MECOND
        {
            get
            {
                return _MECOND;
            }
            set
            {
                if ((_MECOND != value))
                {
                    _MECOND = value;
                }
            }
        }

        public decimal MEFFAG
        {
            get
            {
                return _MEFFAG;
            }
            set
            {
                if ((_MEFFAG != value))
                {
                    _MEFFAG = value;
                }
            }
        }

        public decimal MEKIT
        {
            get
            {
                return _MEKIT;
            }
            set
            {
                if ((_MEKIT != value))
                {
                    _MEKIT = value;
                }
            }
        }

        public string MELEC
        {
            get
            {
                return _MELEC;
            }
            set
            {
                if ((_MELEC != value))
                {
                    _MELEC = value;
                }
            }
        }

        public decimal METXYR
        {
            get
            {
                return _METXYR;
            }
            set
            {
                if ((_METXYR != value))
                {
                    _METXYR = value;
                }
            }
        }

        public decimal MEXWL2
        {
            get
            {
                return _MEXWL2;
            }
            set
            {
                if ((_MEXWL2 != value))
                {
                    _MEXWL2 = value;
                }
            }
        }

        public decimal MEXWLL
        {
            get
            {
                return _MEXWLL;
            }
            set
            {
                if ((_MEXWLL != value))
                {
                    _MEXWLL = value;
                }
            }
        }

        public decimal MFACTR
        {
            get
            {
                return _MFACTR;
            }
            set
            {
                if ((_MFACTR != value))
                {
                    _MFACTR = value;
                }
            }
        }

        public decimal MFAIRV
        {
            get
            {
                return _MFAIRV;
            }
            set
            {
                if ((_MFAIRV != value))
                {
                    _MFAIRV = value;
                }
            }
        }

        public string MFILL4
        {
            get
            {
                return _MFILL4;
            }
            set
            {
                if ((_MFILL4 != value))
                {
                    _MFILL4 = value;
                }
            }
        }

        public string MFILL7
        {
            get
            {
                return _MFILL7;
            }
            set
            {
                if ((_MFILL7 != value))
                {
                    _MFILL7 = value;
                }
            }
        }

        public string MFILL9
        {
            get
            {
                return _MFILL9;
            }
            set
            {
                if ((_MFILL9 != value))
                {
                    _MFILL9 = value;
                }
            }
        }

        public string MFLOOR
        {
            get
            {
                return _MFLOOR;
            }
            set
            {
                if ((_MFLOOR != value))
                {
                    _MFLOOR = value;
                }
            }
        }

        public string MFLUTP
        {
            get
            {
                return _MFLUTP;
            }
            set
            {
                if ((_MFLUTP != value))
                {
                    _MFLUTP = value;
                }
            }
        }

        public decimal MFL_no
        {
            get
            {
                return _MFL_no;
            }
            set
            {
                if ((_MFL_no != value))
                {
                    _MFL_no = value;
                }
            }
        }

        public string MFNAM
        {
            get
            {
                return _MFNAM;
            }
            set
            {
                if ((_MFNAM != value))
                {
                    _MFNAM = value;
                }
            }
        }

        public decimal MFOUND
        {
            get
            {
                return _MFOUND;
            }
            set
            {
                if ((_MFOUND != value))
                {
                    _MFOUND = value;
                }
            }
        }

        public string MFP1
        {
            get
            {
                return _MFP1;
            }
            set
            {
                if ((_MFP1 != value))
                {
                    _MFP1 = value;
                }
            }
        }

        public string MFP2
        {
            get
            {
                return _MFP2;
            }
            set
            {
                if ((_MFP2 != value))
                {
                    _MFP2 = value;
                }
            }
        }

        public decimal MFP_no
        {
            get
            {
                return _MFP_no;
            }
            set
            {
                if ((_MFP_no != value))
                {
                    _MFP_no = value;
                }
            }
        }

        public decimal MFUEL
        {
            get
            {
                return _MFUEL;
            }
            set
            {
                if ((_MFUEL != value))
                {
                    _MFUEL = value;
                }
            }
        }

        public decimal MFUNCD
        {
            get
            {
                return _MFUNCD;
            }
            set
            {
                if ((_MFUNCD != value))
                {
                    _MFUNCD = value;
                }
            }
        }

        public decimal MGART
        {
            get
            {
                return _MGART;
            }
            set
            {
                if ((_MGART != value))
                {
                    _MGART = value;
                }
            }
        }

        public decimal MGART2
        {
            get
            {
                return _MGART2;
            }
            set
            {
                if ((_MGART2 != value))
                {
                    _MGART2 = value;
                }
            }
        }

        public decimal MGAR_no_2
        {
            get
            {
                return _MGAR_no_2;
            }
            set
            {
                if ((_MGAR_no_2 != value))
                {
                    _MGAR_no_2 = value;
                }
            }
        }

        public decimal MGAR_no_C
        {
            get
            {
                return _MGAR_no_C;
            }
            set
            {
                if ((_MGAR_no_C != value))
                {
                    _MGAR_no_C = value;
                }
            }
        }

        public string MGAS
        {
            get
            {
                return _MGAS;
            }
            set
            {
                if ((_MGAS != value))
                {
                    _MGAS = value;
                }
            }
        }

        public string MGISPN
        {
            get
            {
                return _MGISPN;
            }
            set
            {
                if ((_MGISPN != value))
                {
                    _MGISPN = value;
                }
            }
        }

        public string MGRNTR
        {
            get
            {
                return _MGRNTR;
            }
            set
            {
                if ((_MGRNTR != value))
                {
                    _MGRNTR = value;
                }
            }
        }

        public decimal MHEAT
        {
            get
            {
                return _MHEAT;
            }
            set
            {
                if ((_MHEAT != value))
                {
                    _MHEAT = value;
                }
            }
        }

        public string MHIDNM
        {
            get
            {
                return _MHIDNM;
            }
            set
            {
                if ((_MHIDNM != value))
                {
                    _MHIDNM = value;
                }
            }
        }

        public string MHIDPC
        {
            get
            {
                return _MHIDPC;
            }
            set
            {
                if ((_MHIDPC != value))
                {
                    _MHIDPC = value;
                }
            }
        }

        public decimal MHRDAT
        {
            get
            {
                return _MHRDAT;
            }
            set
            {
                if ((_MHRDAT != value))
                {
                    _MHRDAT = value;
                }
            }
        }

        public string MHRNAM
        {
            get
            {
                return _MHRNAM;
            }
            set
            {
                if ((_MHRNAM != value))
                {
                    _MHRNAM = value;
                }
            }
        }

        public decimal MHRPH_no
        {
            get
            {
                return _MHRPH_no;
            }
            set
            {
                if ((_MHRPH_no != value))
                {
                    _MHRPH_no = value;
                }
            }
        }

        public string MHRSES
        {
            get
            {
                return _MHRSES;
            }
            set
            {
                if ((_MHRSES != value))
                {
                    _MHRSES = value;
                }
            }
        }

        public decimal MHRTIM
        {
            get
            {
                return _MHRTIM;
            }
            set
            {
                if ((_MHRTIM != value))
                {
                    _MHRTIM = value;
                }
            }
        }

        public decimal MHSE_no
        {
            get
            {
                return _MHSE_no;
            }
            set
            {
                if ((_MHSE_no != value))
                {
                    _MHSE_no = value;
                }
            }
        }

        public string MHSE_no_2
        {
            get
            {
                return _MHSE_no_2;
            }
            set
            {
                if ((_MHSE_no_2 != value))
                {
                    _MHSE_no_2 = value;
                }
            }
        }

        public decimal MIMADJ
        {
            get
            {
                return _MIMADJ;
            }
            set
            {
                if ((_MIMADJ != value))
                {
                    _MIMADJ = value;
                }
            }
        }

        public decimal MIMPRV
        {
            get
            {
                return _MIMPRV;
            }
            set
            {
                if ((_MIMPRV != value))
                {
                    _MIMPRV = value;
                }
            }
        }

        public string MINIT
        {
            get
            {
                return _MINIT;
            }
            set
            {
                if ((_MINIT != value))
                {
                    _MINIT = value;
                }
            }
        }

        public decimal MINNO2
        {
            get
            {
                return _MINNO2;
            }
            set
            {
                if ((_MINNO2 != value))
                {
                    _MINNO2 = value;
                }
            }
        }

        public decimal MINNO_no
        {
            get
            {
                return _MINNO_no;
            }
            set
            {
                if ((_MINNO_no != value))
                {
                    _MINNO_no = value;
                }
            }
        }

        public decimal MINSPD
        {
            get
            {
                return _MINSPD;
            }
            set
            {
                if ((_MINSPD != value))
                {
                    _MINSPD = value;
                }
            }
        }

        public string MINTYP
        {
            get
            {
                return _MINTYP;
            }
            set
            {
                if ((_MINTYP != value))
                {
                    _MINTYP = value;
                }
            }
        }

        public decimal MINTYR
        {
            get
            {
                return _MINTYR;
            }
            set
            {
                if ((_MINTYR != value))
                {
                    _MINTYR = value;
                }
            }
        }

        public string MINWLL
        {
            get
            {
                return _MINWLL;
            }
            set
            {
                if ((_MINWLL != value))
                {
                    _MINWLL = value;
                }
            }
        }

        public decimal MIOFP_no
        {
            get
            {
                return _MIOFP_no;
            }
            set
            {
                if ((_MIOFP_no != value))
                {
                    _MIOFP_no = value;
                }
            }
        }

        public string MLGBKC
        {
            get
            {
                return _MLGBKC;
            }
            set
            {
                if ((_MLGBKC != value))
                {
                    _MLGBKC = value;
                }
            }
        }

        public string MLGBK_no
        {
            get
            {
                return _MLGBK_no;
            }
            set
            {
                if ((_MLGBK_no != value))
                {
                    _MLGBK_no = value;
                }
            }
        }

        public string MLGITY
        {
            get
            {
                return _MLGITY;
            }
            set
            {
                if ((_MLGITY != value))
                {
                    _MLGITY = value;
                }
            }
        }

        public decimal MLGIYR
        {
            get
            {
                return _MLGIYR;
            }
            set
            {
                if ((_MLGIYR != value))
                {
                    _MLGIYR = value;
                }
            }
        }

        public decimal MLGNO2
        {
            get
            {
                return _MLGNO2;
            }
            set
            {
                if ((_MLGNO2 != value))
                {
                    _MLGNO2 = value;
                }
            }
        }

        public decimal MLGNO_no
        {
            get
            {
                return _MLGNO_no;
            }
            set
            {
                if ((_MLGNO_no != value))
                {
                    _MLGNO_no = value;
                }
            }
        }

        public decimal MLGPG_no
        {
            get
            {
                return _MLGPG_no;
            }
            set
            {
                if ((_MLGPG_no != value))
                {
                    _MLGPG_no = value;
                }
            }
        }

        public string MLNAM
        {
            get
            {
                return _MLNAM;
            }
            set
            {
                if ((_MLNAM != value))
                {
                    _MLNAM = value;
                }
            }
        }

        public string MLTRCD
        {
            get
            {
                return _MLTRCD;
            }
            set
            {
                if ((_MLTRCD != value))
                {
                    _MLTRCD = value;
                }
            }
        }

        public string MLUSE
        {
            get
            {
                return _MLUSE;
            }
            set
            {
                if ((_MLUSE != value))
                {
                    _MLUSE = value;
                }
            }
        }

        public string MMAGCD
        {
            get
            {
                return _MMAGCD;
            }
            set
            {
                if ((_MMAGCD != value))
                {
                    _MMAGCD = value;
                }
            }
        }

        public string MMAP
        {
            get
            {
                return _MMAP;
            }
            set
            {
                if ((_MMAP != value))
                {
                    _MMAP = value;
                }
            }
        }

        public decimal MMCODE
        {
            get
            {
                return _MMCODE;
            }
            set
            {
                if ((_MMCODE != value))
                {
                    _MMCODE = value;
                }
            }
        }

        public decimal MMFL_no
        {
            get
            {
                return _MMFL_no;
            }
            set
            {
                if ((_MMFL_no != value))
                {
                    _MMFL_no = value;
                }
            }
        }

        public decimal MMNNUD
        {
            get
            {
                return _MMNNUD;
            }
            set
            {
                if ((_MMNNUD != value))
                {
                    _MMNNUD = value;
                }
            }
        }

        public decimal MMNUD
        {
            get
            {
                return _MMNUD;
            }
            set
            {
                if ((_MMNUD != value))
                {
                    _MMNUD = value;
                }
            }
        }

        public decimal MMORTC
        {
            get
            {
                return _MMORTC;
            }
            set
            {
                if ((_MMORTC != value))
                {
                    _MMORTC = value;
                }
            }
        }

        public decimal MMOSLD
        {
            get
            {
                return _MMOSLD;
            }
            set
            {
                if ((_MMOSLD != value))
                {
                    _MMOSLD = value;
                }
            }
        }

        public decimal MNBADJ
        {
            get
            {
                return _MNBADJ;
            }
            set
            {
                if ((_MNBADJ != value))
                {
                    _MNBADJ = value;
                }
            }
        }

        public decimal MNBRHD
        {
            get
            {
                return _MNBRHD;
            }
            set
            {
                if ((_MNBRHD != value))
                {
                    _MNBRHD = value;
                }
            }
        }

        public decimal MOCCUP
        {
            get
            {
                return _MOCCUP;
            }
            set
            {
                if ((_MOCCUP != value))
                {
                    _MOCCUP = value;
                }
            }
        }

        public decimal MOLDOC
        {
            get
            {
                return _MOLDOC;
            }
            set
            {
                if ((_MOLDOC != value))
                {
                    _MOLDOC = value;
                }
            }
        }

        public string MOTDES
        {
            get
            {
                return _MOTDES;
            }
            set
            {
                if ((_MOTDES != value))
                {
                    _MOTDES = value;
                }
            }
        }

        public decimal MPBFIN
        {
            get
            {
                return _MPBFIN;
            }
            set
            {
                if ((_MPBFIN != value))
                {
                    _MPBFIN = value;
                }
            }
        }

        public string MPBOOK
        {
            get
            {
                return _MPBOOK;
            }
            set
            {
                if ((_MPBOOK != value))
                {
                    _MPBOOK = value;
                }
            }
        }

        public decimal MPBTOT
        {
            get
            {
                return _MPBTOT;
            }
            set
            {
                if ((_MPBTOT != value))
                {
                    _MPBTOT = value;
                }
            }
        }

        public string MPCODE
        {
            get
            {
                return _MPCODE;
            }
            set
            {
                if ((_MPCODE != value))
                {
                    _MPCODE = value;
                }
            }
        }

        public decimal MPCOMP
        {
            get
            {
                return _MPCOMP;
            }
            set
            {
                if ((_MPCOMP != value))
                {
                    _MPCOMP = value;
                }
            }
        }

        public string MPERR
        {
            get
            {
                return _MPERR;
            }
            set
            {
                if ((_MPERR != value))
                {
                    _MPERR = value;
                }
            }
        }

        public string MPICT
        {
            get
            {
                return _MPICT;
            }
            set
            {
                if ((_MPICT != value))
                {
                    _MPICT = value;
                }
            }
        }

        public decimal MPPAGE
        {
            get
            {
                return _MPPAGE;
            }
            set
            {
                if ((_MPPAGE != value))
                {
                    _MPPAGE = value;
                }
            }
        }

        public string MPRCIT
        {
            get
            {
                return _MPRCIT;
            }
            set
            {
                if ((_MPRCIT != value))
                {
                    _MPRCIT = value;
                }
            }
        }

        public string MPROUT
        {
            get
            {
                return _MPROUT;
            }
            set
            {
                if ((_MPROUT != value))
                {
                    _MPROUT = value;
                }
            }
        }

        public string MPRSTA
        {
            get
            {
                return _MPRSTA;
            }
            set
            {
                if ((_MPRSTA != value))
                {
                    _MPRSTA = value;
                }
            }
        }

        public decimal MPRZP1
        {
            get
            {
                return _MPRZP1;
            }
            set
            {
                if ((_MPRZP1 != value))
                {
                    _MPRZP1 = value;
                }
            }
        }

        public string MPRZP4
        {
            get
            {
                return _MPRZP4;
            }
            set
            {
                if ((_MPRZP4 != value))
                {
                    _MPRZP4 = value;
                }
            }
        }

        public decimal MPSF
        {
            get
            {
                return _MPSF;
            }
            set
            {
                if ((_MPSF != value))
                {
                    _MPSF = value;
                }
            }
        }

        public string MPSUFX
        {
            get
            {
                return _MPSUFX;
            }
            set
            {
                if ((_MPSUFX != value))
                {
                    _MPSUFX = value;
                }
            }
        }

        public decimal MPUSE
        {
            get
            {
                return _MPUSE;
            }
            set
            {
                if ((_MPUSE != value))
                {
                    _MPUSE = value;
                }
            }
        }

        public string MQAFIL
        {
            get
            {
                return _MQAFIL;
            }
            set
            {
                if ((_MQAFIL != value))
                {
                    _MQAFIL = value;
                }
            }
        }

        public decimal MQAPCH
        {
            get
            {
                return _MQAPCH;
            }
            set
            {
                if ((_MQAPCH != value))
                {
                    _MQAPCH = value;
                }
            }
        }

        public string MRECID
        {
            get
            {
                return _MRECID;
            }
            set
            {
                if ((_MRECID != value))
                {
                    _MRECID = value;
                }
            }
        }

        public decimal MRECNO
        {
            get
            {
                return _MRECNO;
            }
            set
            {
                if ((_MRECNO != value))
                {
                    _MRECNO = value;
                }
            }
        }

       
        public string MREM1
        {
            get
            {
                return _MREM1;
            }
            set
            {
                if ((_MREM1 != value))
                {
                    _MREM1 = value;
                }
            }
        }

        public string MREM2
        {
            get
            {
                return _MREM2;
            }
            set
            {
                if ((_MREM2 != value))
                {
                    _MREM2 = value;
                }
            }
        }

        public decimal MROOFG
        {
            get
            {
                return _MROOFG;
            }
            set
            {
                if ((_MROOFG != value))
                {
                    _MROOFG = value;
                }
            }
        }

        public decimal MROOFT
        {
            get
            {
                return _MROOFT;
            }
            set
            {
                if ((_MROOFT != value))
                {
                    _MROOFT = value;
                }
            }
        }

        public decimal MROW
        {
            get
            {
                return _MROW;
            }
            set
            {
                if ((_MROW != value))
                {
                    _MROW = value;
                }
            }
        }

        public decimal MSBFIN
        {
            get
            {
                return _MSBFIN;
            }
            set
            {
                if ((_MSBFIN != value))
                {
                    _MSBFIN = value;
                }
            }
        }

        public decimal MSBTOT
        {
            get
            {
                return _MSBTOT;
            }
            set
            {
                if ((_MSBTOT != value))
                {
                    _MSBTOT = value;
                }
            }
        }

        public string MSDIRS
        {
            get
            {
                return _MSDIRS;
            }
            set
            {
                if ((_MSDIRS != value))
                {
                    _MSDIRS = value;
                }
            }
        }

        public decimal MSELLP
        {
            get
            {
                return _MSELLP;
            }
            set
            {
                if ((_MSELLP != value))
                {
                    _MSELLP = value;
                }
            }
        }

        public decimal MSEWER
        {
            get
            {
                return _MSEWER;
            }
            set
            {
                if ((_MSEWER != value))
                {
                    _MSEWER = value;
                }
            }
        }

        public decimal MSFL_no
        {
            get
            {
                return _MSFL_no;
            }
            set
            {
                if ((_MSFL_no != value))
                {
                    _MSFL_no = value;
                }
            }
        }

        public decimal MSFP_no
        {
            get
            {
                return _MSFP_no;
            }
            set
            {
                if ((_MSFP_no != value))
                {
                    _MSFP_no = value;
                }
            }
        }

        public decimal MSS1
        {
            get
            {
                return _MSS1;
            }
            set
            {
                if ((_MSS1 != value))
                {
                    _MSS1 = value;
                }
            }
        }

        public decimal MSS2
        {
            get
            {
                return _MSS2;
            }
            set
            {
                if ((_MSS2 != value))
                {
                    _MSS2 = value;
                }
            }
        }

        public string MSTATE
        {
            get
            {
                return _MSTATE;
            }
            set
            {
                if ((_MSTATE != value))
                {
                    _MSTATE = value;
                }
            }
        }

        public string MSTORY
        {
            get
            {
                return _MSTORY;
            }
            set
            {
                if ((_MSTORY != value))
                {
                    _MSTORY = value;
                }
            }
        }

        public decimal MSTOR_no
        {
            get
            {
                return _MSTOR_no;
            }
            set
            {
                if ((_MSTOR_no != value))
                {
                    _MSTOR_no = value;
                }
            }
        }

        public string MSTRT
        {
            get
            {
                return _MSTRT;
            }
            set
            {
                if ((_MSTRT != value))
                {
                    _MSTRT = value;
                }
            }
        }

        public string MSTTYP
        {
            get
            {
                return _MSTTYP;
            }
            set
            {
                if ((_MSTTYP != value))
                {
                    _MSTTYP = value;
                }
            }
        }

        public string MSUBDV
        {
            get
            {
                return _MSUBDV;
            }
            set
            {
                if ((_MSUBDV != value))
                {
                    _MSUBDV = value;
                }
            }
        }

        public decimal MSWL
        {
            get
            {
                return _MSWL;
            }
            set
            {
                if ((_MSWL != value))
                {
                    _MSWL = value;
                }
            }
        }

        public decimal MTAC
        {
            get
            {
                return _MTAC;
            }
            set
            {
                if ((_MTAC != value))
                {
                    _MTAC = value;
                }
            }
        }

        public decimal MTBAS
        {
            get
            {
                return _MTBAS;
            }
            set
            {
                if ((_MTBAS != value))
                {
                    _MTBAS = value;
                }
            }
        }

        public decimal MTBI
        {
            get
            {
                return _MTBI;
            }
            set
            {
                if ((_MTBI != value))
                {
                    _MTBI = value;
                }
            }
        }

        public decimal MTBIMP
        {
            get
            {
                return _MTBIMP;
            }
            set
            {
                if ((_MTBIMP != value))
                {
                    _MTBIMP = value;
                }
            }
        }

        public decimal MTBV
        {
            get
            {
                return _MTBV;
            }
            set
            {
                if ((_MTBV != value))
                {
                    _MTBV = value;
                }
            }
        }

        public decimal MTERRN
        {
            get
            {
                return _MTERRN;
            }
            set
            {
                if ((_MTERRN != value))
                {
                    _MTERRN = value;
                }
            }
        }

        public decimal MTFBAS
        {
            get
            {
                return _MTFBAS;
            }
            set
            {
                if ((_MTFBAS != value))
                {
                    _MTFBAS = value;
                }
            }
        }

        public decimal MTFL
        {
            get
            {
                return _MTFL;
            }
            set
            {
                if ((_MTFL != value))
                {
                    _MTFL = value;
                }
            }
        }

        public decimal MTFP
        {
            get
            {
                return _MTFP;
            }
            set
            {
                if ((_MTFP != value))
                {
                    _MTFP = value;
                }
            }
        }

        public decimal MTHEAT
        {
            get
            {
                return _MTHEAT;
            }
            set
            {
                if ((_MTHEAT != value))
                {
                    _MTHEAT = value;
                }
            }
        }

        public decimal MTIME
        {
            get
            {
                return _MTIME;
            }
            set
            {
                if ((_MTIME != value))
                {
                    _MTIME = value;
                }
            }
        }

        public decimal MTOTA
        {
            get
            {
                return _MTOTA;
            }
            set
            {
                if ((_MTOTA != value))
                {
                    _MTOTA = value;
                }
            }
        }

        public decimal MTOTBV
        {
            get
            {
                return _MTOTBV;
            }
            set
            {
                if ((_MTOTBV != value))
                {
                    _MTOTBV = value;
                }
            }
        }

        public decimal MTOTLD
        {
            get
            {
                return _MTOTLD;
            }
            set
            {
                if ((_MTOTLD != value))
                {
                    _MTOTLD = value;
                }
            }
        }

        public decimal MTOTOI
        {
            get
            {
                return _MTOTOI;
            }
            set
            {
                if ((_MTOTOI != value))
                {
                    _MTOTOI = value;
                }
            }
        }

        public decimal MTOTPR
        {
            get
            {
                return _MTOTPR;
            }
            set
            {
                if ((_MTOTPR != value))
                {
                    _MTOTPR = value;
                }
            }
        }

        public decimal MTPLUM
        {
            get
            {
                return _MTPLUM;
            }
            set
            {
                if ((_MTPLUM != value))
                {
                    _MTPLUM = value;
                }
            }
        }

        public decimal MTSUBT
        {
            get
            {
                return _MTSUBT;
            }
            set
            {
                if ((_MTSUBT != value))
                {
                    _MTSUBT = value;
                }
            }
        }

        public decimal MTTADD
        {
            get
            {
                return _MTTADD;
            }
            set
            {
                if ((_MTTADD != value))
                {
                    _MTTADD = value;
                }
            }
        }

        public decimal MTUTIL
        {
            get
            {
                return _MTUTIL;
            }
            set
            {
                if ((_MTUTIL != value))
                {
                    _MTUTIL = value;
                }
            }
        }

        public string MUSER1
        {
            get
            {
                return _MUSER1;
            }
            set
            {
                if ((_MUSER1 != value))
                {
                    _MUSER1 = value;
                }
            }
        }

        public string MUSER2
        {
            get
            {
                return _MUSER2;
            }
            set
            {
                if ((_MUSER2 != value))
                {
                    _MUSER2 = value;
                }
            }
        }

        public string MUSER3
        {
            get
            {
                return _MUSER3;
            }
            set
            {
                if ((_MUSER3 != value))
                {
                    _MUSER3 = value;
                }
            }
        }

        public string MUSER4
        {
            get
            {
                return _MUSER4;
            }
            set
            {
                if ((_MUSER4 != value))
                {
                    _MUSER4 = value;
                }
            }
        }

        public string MUSRID
        {
            get
            {
                return _MUSRID;
            }
            set
            {
                if ((_MUSRID != value))
                {
                    _MUSRID = value;
                }
            }
        }

        public decimal MWATER
        {
            get
            {
                return _MWATER;
            }
            set
            {
                if ((_MWATER != value))
                {
                    _MWATER = value;
                }
            }
        }

        public string MWBOOK
        {
            get
            {
                return _MWBOOK;
            }
            set
            {
                if ((_MWBOOK != value))
                {
                    _MWBOOK = value;
                }
            }
        }

        public string MWCODE
        {
            get
            {
                return _MWCODE;
            }
            set
            {
                if ((_MWCODE != value))
                {
                    _MWCODE = value;
                }
            }
        }

        public decimal MWPAGE
        {
            get
            {
                return _MWPAGE;
            }
            set
            {
                if ((_MWPAGE != value))
                {
                    _MWPAGE = value;
                }
            }
        }

        public string MWSUFX
        {
            get
            {
                return _MWSUFX;
            }
            set
            {
                if ((_MWSUFX != value))
                {
                    _MWSUFX = value;
                }
            }
        }

        public decimal MYRBLT
        {
            get
            {
                return _MYRBLT;
            }
            set
            {
                if ((_MYRBLT != value))
                {
                    _MYRBLT = value;
                }
            }
        }

        public decimal MYRSLD
        {
            get
            {
                return _MYRSLD;
            }
            set
            {
                if ((_MYRSLD != value))
                {
                    _MYRSLD = value;
                }
            }
        }

        public string MZIP4
        {
            get
            {
                return _MZIP4;
            }
            set
            {
                if ((_MZIP4 != value))
                {
                    _MZIP4 = value;
                }
            }
        }

        public decimal MZIP5
        {
            get
            {
                return _MZIP5;
            }
            set
            {
                if ((_MZIP5 != value))
                {
                    _MZIP5 = value;
                }
            }
        }

        public decimal MZIPBR
        {
            get
            {
                return _MZIPBR;
            }
            set
            {
                if ((_MZIPBR != value))
                {
                    _MZIPBR = value;
                }
            }
        }

        public string MZONE
        {
            get
            {
                return _MZONE;
            }
            set
            {
                if ((_MZONE != value))
                {
                    _MZONE = value;
                }
            }
        }

        public decimal M_FLUE
        {
            get
            {
                return _M_FLUE;
            }
            set
            {
                if ((_M_FLUE != value))
                {
                    _M_FLUE = value;
                }
            }
        }

        public decimal M_no_BR
        {
            get
            {
                return _M_no_BR;
            }
            set
            {
                if ((_M_no_BR != value))
                {
                    _M_no_BR = value;
                }
            }
        }

        public decimal M_no_DUNT
        {
            get
            {
                return _M_no_DUNT;
            }
            set
            {
                if ((_M_no_DUNT != value))
                {
                    _M_no_DUNT = value;
                }
            }
        }

        public decimal M_no_FBTH
        {
            get
            {
                return _M_no_FBTH;
            }
            set
            {
                if ((_M_no_FBTH != value))
                {
                    _M_no_FBTH = value;
                }
            }
        }

        public decimal M_no_HBTH
        {
            get
            {
                return _M_no_HBTH;
            }
            set
            {
                if ((_M_no_HBTH != value))
                {
                    _M_no_HBTH = value;
                }
            }
        }

        public decimal M_no_ROOM
        {
            get
            {
                return _M_no_ROOM;
            }
            set
            {
                if ((_M_no_ROOM != value))
                {
                    _M_no_ROOM = value;
                }
            }
        }

        private string _M0DEPR;
        private decimal _M1ADJ;
        private decimal _M1AREA;
        private decimal _M1DFAC;
        private decimal _M1DPTH;
        private decimal _M1FRNT;
        private decimal _M1RATE;
        private string _M1UM;
        private decimal _M2ADJ;
        private decimal _M2AREA;
        private decimal _M2DFAC;
        private decimal _M2DPTH;
        private decimal _M2FRNT;
        private decimal _M2RATE;
        private string _M2UM;
        private string _MAC;
        private decimal _MACCT;
        private decimal _MACPCT;
        private string _MACRE;
        private decimal _MACRE_no;
        private decimal _MACSF;
        private string _MADD1;
        private string _MADD2;
        private decimal _MAGE;
        private string _MASCOM;
        private decimal _MASSB;
        private decimal _MASSL;
        private decimal _MASSLU;
        private decimal _MASSM;
        private string _MATHOM;
        private decimal _MBASA;
        private decimal _MBASTP;
        private decimal _MBI_no_C;
        private decimal _MBRATE;
        private decimal _MCADA;
        private string _MCALC;
        private decimal _MCAMO;
        private decimal _MCARPT;
        private decimal _MCAR_no_C;
        private decimal _MCAYR;
        private decimal _MCDDA;
        private decimal _MCDMO;
        private string _MCDR;
        private decimal _MCDRDT;
        private decimal _MCDYR;
        private decimal _MCHAR;
        private string _MCITY;
        private string _MCLASS;
        private string _MCNST1;
        private string _MCNST2;
        private string _MCOMM1;
        private string _MCOMM2;
        private string _MCOMM3;
        private string _MCOND;
        private decimal _MCVDA;
        private string _MCVEXP;
        private decimal _MCVMO;
        private decimal _MCVYR;
        private decimal _MDASLD;
        private decimal _MDATLG;
        private decimal _MDATPR;
        private string _MDBOOK;
        private string _MDCODE;
        private string _MDELAY;
        private decimal _MDEPRC;
        private string _MDESC1;
        private string _MDESC2;
        private string _MDESC3;
        private string _MDESC4;
        private string _MDIRCT;
        private decimal _MDPAGE;
        private string _MDSUFX;
        private decimal _MDWELL;
        private decimal _MEACRE;
        private decimal _MEASE;
        private decimal _MECOND;
        private decimal _MEFFAG;
        private decimal _MEKIT;
        private string _MELEC;
        private decimal _METXYR;
        private decimal _MEXWL2;
        private decimal _MEXWLL;
        private decimal _MFACTR;
        private decimal _MFAIRV;
        private string _MFILL4;
        private string _MFILL7;
        private string _MFILL9;
        private string _MFLOOR;
        private string _MFLUTP;
        private decimal _MFL_no;
        private string _MFNAM;
        private decimal _MFOUND;
        private string _MFP1;
        private string _MFP2;
        private decimal _MFP_no;
        private decimal _MFUEL;
        private decimal _MFUNCD;
        private decimal _MGART;
        private decimal _MGART2;
        private decimal _MGAR_no_2;
        private decimal _MGAR_no_C;
        private string _MGAS;
        private string _MGISPN;
        private string _MGRNTR;
        private decimal _MHEAT;
        private string _MHIDNM;
        private string _MHIDPC;
        private decimal _MHRDAT;
        private string _MHRNAM;
        private decimal _MHRPH_no;
        private string _MHRSES;
        private decimal _MHRTIM;
        private decimal _MHSE_no;
        private string _MHSE_no_2;
        private decimal _MIMADJ;
        private decimal _MIMPRV;
        private string _MINIT;
        private decimal _MINNO2;
        private decimal _MINNO_no;
        private decimal _MINSPD;
        private string _MINTYP;
        private decimal _MINTYR;
        private string _MINWLL;
        private decimal _MIOFP_no;
        private string _MLGBKC;
        private string _MLGBK_no;
        private string _MLGITY;
        private decimal _MLGIYR;
        private decimal _MLGNO2;
        private decimal _MLGNO_no;
        private decimal _MLGPG_no;
        private string _MLNAM;
        private string _MLTRCD;
        private string _MLUSE;
        private string _MMAGCD;
        private string _MMAP;
        private decimal _MMCODE;
        private decimal _MMFL_no;
        private decimal _MMNNUD;
        private decimal _MMNUD;
        private decimal _MMORTC;
        private decimal _MMOSLD;
        private decimal _MNBADJ;
        private decimal _MNBRHD;
        private decimal _MOCCUP;
        private decimal _MOLDOC;
        private string _MOTDES;
        private decimal _MPBFIN;
        private string _MPBOOK;
        private decimal _MPBTOT;
        private string _MPCODE;
        private decimal _MPCOMP;
        private string _MPERR;
        private string _MPICT;
        private decimal _MPPAGE;
        private string _MPRCIT;
        private string _MPROUT;
        private string _MPRSTA;
        private decimal _MPRZP1;
        private string _MPRZP4;
        private decimal _MPSF;
        private string _MPSUFX;
        private decimal _MPUSE;
        private string _MQAFIL;
        private decimal _MQAPCH;
        private string _MRECID;
        private decimal _MRECNO;
        private string _MREM1;
        private string _MREM2;
        private decimal _MROOFG;
        private decimal _MROOFT;
        private decimal _MROW;
        private decimal _MSBFIN;
        private decimal _MSBTOT;
        private string _MSDIRS;
        private decimal _MSELLP;
        private decimal _MSEWER;
        private decimal _MSFL_no;
        private decimal _MSFP_no;
        private decimal _MSS1;
        private decimal _MSS2;
        private string _MSTATE;
        private string _MSTORY;
        private decimal _MSTOR_no;
        private string _MSTRT;
        private string _MSTTYP;
        private string _MSUBDV;
        private decimal _MSWL;
        private decimal _MTAC;
        private decimal _MTBAS;
        private decimal _MTBI;
        private decimal _MTBIMP;
        private decimal _MTBV;
        private decimal _MTERRN;
        private decimal _MTFBAS;
        private decimal _MTFL;
        private decimal _MTFP;
        private decimal _MTHEAT;
        private decimal _MTIME;
        private decimal _MTOTA;
        private decimal _MTOTBV;
        private decimal _MTOTLD;
        private decimal _MTOTOI;
        private decimal _MTOTPR;
        private decimal _MTPLUM;
        private decimal _MTSUBT;
        private decimal _MTTADD;
        private decimal _MTUTIL;
        private string _MUSER1;
        private string _MUSER2;
        private string _MUSER3;
        private string _MUSER4;
        private string _MUSRID;
        private decimal _MWATER;
        private string _MWBOOK;
        private string _MWCODE;
        private decimal _MWPAGE;
        private string _MWSUFX;
        private decimal _MYRBLT;
        private decimal _MYRSLD;
        private string _MZIP4;
        private decimal _MZIP5;
        private decimal _MZIPBR;
        private string _MZONE;
        private decimal _M_FLUE;
        private decimal _M_no_BR;
        private decimal _M_no_DUNT;
        private decimal _M_no_FBTH;
        private decimal _M_no_HBTH;
        private decimal _M_no_ROOM;
    }
}
