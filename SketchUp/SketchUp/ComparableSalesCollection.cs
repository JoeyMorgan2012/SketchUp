using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SketchUp
{
	public class ComparableSalesCollection : List<ParcelData>
	{
		private SWallTech.CAMRA_Connection dbConn = null;
		private ParcelData _currentParcel = null;

		public ParcelData Subject
		{
			get
			{
				return CurrentParcel;
			}
		}

		private SortedDictionary<decimal, int> _dissimilarity = null;

		private DataTable _SCTable = new DataTable("ComparablesTable");
		private string lastQuerySql;
		
		public ParcelData CurrentParcel
		{
			get
			{
				return _currentParcel;
			}

			set
			{
				_currentParcel = value;
			}
		}

		public string LastQuerySql
		{
			get
			{
				return lastQuerySql;
			}

			set
			{
				lastQuerySql = value;
			}
		}

		private int dyear = DateTime.Now.Year;

		private ComparableSalesCollection()
		{
		}

	

		private decimal DifferenceFormula(decimal subject, decimal comp, decimal stdv, decimal weight)
		{
			if (stdv == 0)
				return 0;

			return Convert.ToDecimal(Math.Pow((((subject - comp).AsDouble() / stdv.AsDouble()) * weight.AsDouble()), 2));
		}

		public void BuildComparables(List<SearchParameter> _parms)
		{
			_dissimilarity = new SortedDictionary<decimal, int>();

			ParcelDataCollection _comps = new ParcelDataCollection(dbConn, CurrentParcel.mrecno, CurrentParcel.mdwell);
			try
			{
				_comps.GetData(_parms, true);

				foreach (ParcelData _comp in _comps)
				{
					//decimal diss = CalculateDissimilarity(_comp);
					//_dissimilarity.Add(diss, _comp.Record);
				}

				foreach (int recno in _dissimilarity.Values)
				{
					this.Add(_comps.Where(f => f.Record == recno).SingleOrDefault());
				}
			}
			catch (RecordMaximumExceededException rex)
			{
				Console.WriteLine(string.Format("{0}", rex.Message));
			}
		}

		public DataTable ComparablesTable()
		{
			_SCTable.Rows.Clear();

			if (CurrentParcel != null)
			{
				DataRow row = _SCTable.NewRow();
				row["Type"] = "Subject";
				row["Record"] = CurrentParcel.Record;
				row["911Add"] = CurrentParcel.SiteAddress;
				row["Map No"] = CurrentParcel.mmap;
				row["SalePrice"] = CurrentParcel.msellp;
				row["SaleDate"] = CurrentParcel.SalesDate;
				row["Index"] = "0";
				row["Sub Division"] = CamraSupport.SubDivisionCodeCollection.SubDivDescription(CurrentParcel.msubdv.ToString().Trim());
				row["Location"] = CurrentParcel.LocationQuality;
				row["Acres"] = Convert.ToDecimal(CurrentParcel.macreN.ToString("N3"));
				row["YearBuilt"] = CurrentParcel.myrblt;
				row["Stories"] = Convert.ToDecimal(CurrentParcel.mstorN.ToString("N2"));
				row["ExteriorWall"] = CamraSupport.ExteriorWallTypeCollection.Description(CurrentParcel.mexwll.ToString());
				row["Size"] = CurrentParcel.mtota;
				row["Bsmt"] = CurrentParcel.BasementArea;
				row["Fin.Bsmt"] = CurrentParcel.FinBasementArea;
				row["Rooms"] = CurrentParcel.mNroom;
				row["BedRooms"] = CurrentParcel.mNbr;
				row["Full Baths"] = CurrentParcel.mNfbth;
				row["Half Baths"] = CurrentParcel.mNhbth;
				row["Heat"] = CamraSupport.HeatTypeCollection.Description(CurrentParcel.mheat.ToString());
				row["A/C"] = CurrentParcel.mac;
				row["Fireplace"] = CurrentParcel.mfpN;
				row["StackedFP"] = CurrentParcel.msfpN;
				row["InOPFP"] = CurrentParcel.miofpN;
				if (CurrentParcel.GasLogRecords.Count >= 1)
				{
					row["GasLogFP"] = CurrentParcel.GasLogRecords[0].NbrGasFP;
				}
				row["Flues"] = CurrentParcel.mflN;
				row["StackedFlue"] = CurrentParcel.msflN;
				row["MetalFlue"] = CurrentParcel.mmflN;
				row["Aux.Liv.Area"] = CurrentParcel.auxA;
				row["Porch"] = CurrentParcel.porA;
				row["Scrn.Porch"] = CurrentParcel.sporA;
				row["Encl.Porch"] = CurrentParcel.eporA;
				row["Deck"] = CurrentParcel.deckA;
				row["Patio"] = CurrentParcel.patioA;
				row["CarPort"] = CurrentParcel.carportA;
				row["NoCars_CP"] = CurrentParcel.mcarNc;
				row["Garage"] = CurrentParcel.garA;
				if (CurrentParcel.mgart != 64)
				{
					row["NoCars_GAR"] = CurrentParcel.mgarNc;
				}
				else if (CurrentParcel.mgart == 64)
				{
					row["NoCars_GAR"] = 0;
				}
				row["NoCars_BI"] = CurrentParcel.mbiNc;
				row["Class"] = CurrentParcel.Class;
				row["Factor"] = Convert.ToDecimal(CurrentParcel.Factor);
				row["QualityAdj"] = CurrentParcel.computedFactor;
				row["Condition"] = CurrentParcel.conditionType;
				row["Deprec"] = Convert.ToDecimal(CurrentParcel.Deprc);
				row["PlumbValue"] = CurrentParcel.mtplum;
				row["HeatValue"] = CurrentParcel.mtheat;
				row["ACValue"] = CurrentParcel.mtac;
				row["FPValue"] = CurrentParcel.mtfp;
				row["FlueValue"] = CurrentParcel.mtfl;
				row["BIGarValue"] = CurrentParcel.mtbi;
				row["SWLValue"] = CurrentParcel.mswl;
				row["TotalBldgVal"] = CurrentParcel.mtotbv;
				row["OtherImp"] = CurrentParcel.mtotoi;
				row["LandValue"] = CurrentParcel.mtotld;
				row["TotalValue"] = CurrentParcel.mtotpr;
				row["NbrParcelsSold"] = CurrentParcel.mmcode;

				_SCTable.Rows.Add(row);

				foreach (ParcelData _comp in this)
				{
					DataRow r = _SCTable.NewRow();

					//    build row
					r["Type"] = "Comp";
					r["Record"] = _comp.Record;
					r["911Add"] = _comp.SiteAddress;
					r["Map No"] = _comp.mmap;
					r["SalePrice"] = _comp.msellp;
					r["SaleDate"] = _comp.SalesDate;
					r["Index"] = _dissimilarity.Where(f => f.Value == _comp.Record).SingleOrDefault().Key;
					r["Sub Division"] = CamraSupport.SubDivisionCodeCollection.SubDivDescription(_comp.msubdv.ToString().Trim());
					r["Location"] = _comp.LocationQuality;
					r["Acres"] = Convert.ToDecimal(_comp.macreN.ToString("N3"));
					r["YearBuilt"] = _comp.myrblt;
					r["Stories"] = Convert.ToDecimal(_comp.mstorN.ToString("N2"));
					r["ExteriorWall"] = CamraSupport.ExteriorWallTypeCollection.Description(_comp.mexwll.ToString());
					r["Size"] = _comp.mtota;
					r["Bsmt"] = _comp.BasementArea;
					r["Fin.Bsmt"] = _comp.FinBasementArea;
					r["Rooms"] = _comp.mNroom;
					r["BedRooms"] = _comp.mNbr;
					r["Full Baths"] = _comp.mNfbth;
					r["Half Baths"] = _comp.mNhbth;
					r["Heat"] = CamraSupport.HeatTypeCollection.Description(_comp.mheat.ToString());
					r["A/C"] = _comp.mac;
					r["Fireplace"] = _comp.mfpN;
					r["StackedFP"] = _comp.msfpN;
					r["InOPFP"] = _comp.miofpN;
					if (_comp.GasLogRecords.Count >= 1)
					{
						r["GasLogFP"] = _comp.GasLogRecords[0].NbrGasFP;
					}
					r["Flues"] = _comp.mflN;
					r["StackedFlue"] = _comp.msflN;
					r["MetalFlue"] = _comp.mmflN;
					r["Aux.Liv.Area"] = Convert.ToInt32(_comp.auxA);
					r["Porch"] = Convert.ToInt32(_comp.porA);
					r["Scrn.Porch"] = Convert.ToInt32(_comp.sporA);
					r["Encl.Porch"] = Convert.ToInt32(_comp.eporA);
					r["Deck"] = Convert.ToInt32(_comp.deckA);
					r["Patio"] = Convert.ToInt32(_comp.patioA);
					r["CarPort"] = Convert.ToInt32(_comp.carportA);
					r["NoCars_CP"] = _comp.mcarNc;
					r["Garage"] = Convert.ToInt32(_comp.garA);
					if (_comp.mgart != 64)
					{
						r["NoCars_GAR"] = _comp.mgarNc;
					}
					else if (_comp.mgart == 64)
					{
						r["NoCars_GAR"] = 0;
					}

					r["NoCars_BI"] = _comp.mbiNc;
					r["Class"] = _comp.Class;
					r["Factor"] = Convert.ToDecimal(_comp.Factor);
					r["QualityAdj"] = _comp.computedFactor;
					r["Condition"] = _comp.conditionType;
					r["Deprec"] = Convert.ToDecimal(_comp.Deprc);
					r["PlumbValue"] = _comp.mtplum;
					r["HeatValue"] = _comp.mtheat;
					r["ACValue"] = _comp.mtac;
					r["FPValue"] = _comp.mtfp;
					r["FlueValue"] = _comp.mtfl;
					r["BIGarValue"] = _comp.mtbi;
					r["SWLValue"] = _comp.mswl;
					r["TotalBldgVal"] = _comp.mtotbv;
					r["OtherImp"] = _comp.mtotoi;
					r["LandValue"] = _comp.mtotld;
					r["TotalValue"] = _comp.mtotpr;
					r["NbrParcelsSold"] = _comp.mmcode;

					_SCTable.Rows.Add(r);
				}
			}

			return _SCTable;
		}

		public ComparableSalesCollection(SWallTech.CAMRA_Connection db, ParcelData data)
			: this()
		{
			dbConn = db;
			CurrentParcel = data;

			_SCTable = new DataTable("ComparablesTable");
			_SCTable.Columns.Add(new DataColumn("Type", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Record", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("911Add", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Map No", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("SalePrice", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("SaleDate", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Index", typeof(decimal)));
			_SCTable.Columns.Add(new DataColumn("Sub Division", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Location", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Acres", typeof(decimal)));
			_SCTable.Columns.Add(new DataColumn("YearBuilt", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Stories", typeof(decimal)));
			_SCTable.Columns.Add(new DataColumn("ExteriorWall", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Size", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Bsmt", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Fin.Bsmt", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Rooms", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("BedRooms", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Full Baths", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Half Baths", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Heat", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("A/C", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Fireplace", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("StackedFP", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("InOPFP", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("GasLogFP", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Flues", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("StackedFlue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("MetalFlue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Aux.Liv.Area", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Porch", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Scrn.Porch", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Encl.Porch", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Deck", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Patio", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("CarPort", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("NoCars_CP", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Garage", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("NoCars_GAR", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("NoCars_BI", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("Class", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Factor", typeof(decimal)));
			_SCTable.Columns.Add(new DataColumn("QualityAdj", typeof(decimal)));
			_SCTable.Columns.Add(new DataColumn("Condition", typeof(string)));
			_SCTable.Columns.Add(new DataColumn("Deprec", typeof(decimal)));
			_SCTable.Columns.Add(new DataColumn("PlumbValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("HeatValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("ACValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("FPValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("FlueValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("BIGarValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("SWLValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("TotalBldgVal", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("OtherImp", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("LandValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("TotalValue", typeof(int)));
			_SCTable.Columns.Add(new DataColumn("NbrParcelsSold", typeof(int)));
		}

	
	}
}