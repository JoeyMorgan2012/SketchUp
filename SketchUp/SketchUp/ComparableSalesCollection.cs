using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public class ComparableSalesCollection : List<ParcelData>
    {
        private SWallTech.CAMRA_Connection _db = null;
        private ParcelData _currentParcel = null;

        public ParcelData Subject
        {
            get
            {
                return _currentParcel;
            }
        }

        private SortedDictionary<decimal, int> _dissimilarity = null;

        private DataTable _SCTable = new DataTable("ComparablesTable");

        public string LastQuerySql
        {
            get; set;
        }

        private int dyear = DateTime.Now.Year;

        private ComparableSalesCollection()
        {
        }

        //private decimal CalculateDissimilarity(ParcelData _comp)
        //{
        //    // add code for dissimilarity
        //    decimal tstStories = 0;
        //    if (_currentParcel.mstorN >= 1.0m && _currentParcel.mstorN < 1.3m)
        //    {
        //        tstStories = 1.0m;
        //    }
        //    if (_currentParcel.mstorN >= 1.3m && _currentParcel.mstorN < 1.8m)
        //    {
        //        tstStories = 1.5m;
        //    }
        //    if (_currentParcel.mstorN >= 1.8m)
        //    {
        //        tstStories = 2.0m;
        //    }

        //    StdDeviations subDevs = CamraSupport.StdDeviationCollection.StdDeviation(tstStories);

        //    DismWieghts subWghts = CamraSupport.DisimilarityWeightCollection.DisWght(Convert.ToDecimal(tstStories));

        //    string subMap = string.Empty;
        //    string compMap = string.Empty;

        //if (_currentParcel.mmap.Substring(0, 1) != " ")
        //{
        //    subMap = _currentParcel.mmap.Substring(0, 3).PadLeft(3, '0');
        //}
        //if (_currentParcel.mmap.Substring(0, 1) == " ")
        //{
        //    subMap = _currentParcel.mmap.Substring(0, 3);
        //}
        //if (_currentParcel.mmap.Substring(2, 1) == "-")
        //{
        //    subMap = _currentParcel.mmap.Substring(0, 2).PadLeft(3, '0');
        //}

        //if (_comp.mmap.Substring(0, 1) != " ")
        //{
        //    compMap = _comp.mmap.Substring(0, 3).PadLeft(3, '0');

        //    if (_comp.mmap.Substring(0, 1) == " ")
        //    {
        //        compMap = _comp.mmap.Substring(0, 3);
        //    }
        //    if (_comp.mmap.Substring(2, 1) == "-")
        //    {
        //        compMap = _comp.mmap.Substring(0, 2).PadLeft(3, '0');
        //    }

        //    MapCoordinates subcoords = CamraSupport.MapCoordinateCollection.MapCoordinate(subMap);
        //    MapCoordinates compcoords = CamraSupport.MapCoordinateCollection.MapCoordinate(compMap);

        //    //MapCoordinates subcoords = CamraSupport.MapCoordinateCollection.MapCoordinate(_currentParcel.mmap.ToString().Substring(0, 3));
        //    //MapCoordinates compcoords = CamraSupport.MapCoordinateCollection.MapCoordinate(_comp.mmap.ToString().Substring(0, 3));

        //    int _currentComputedDate = ((DateTime.Now.Year * 365) + (DateTime.Now.Month * 30) + (DateTime.Now.Day));

        //    decimal diss = 0;
        //if (CamraSupport.MapCoordinateCollection.Count > 0)
        //{
        //    diss = Convert.ToDecimal((Math.Sqrt(
        //            (Math.Pow(Convert.ToDouble(subcoords._xAxis - compcoords._xAxis), 2)) +
        //            (Math.Pow(Convert.ToDouble(subcoords._yAxis - compcoords._yAxis), 2))) * 100) * .125);
        //}

        //diss += Math.Abs((((_currentParcel.TotalPropertyValue - Convert.ToDecimal(_comp.msellp)) * .25m) * .01m));
        //diss += Math.Abs(((_currentComputedDate - ((_comp.myrsld * 365) + (_comp.mmosld * 30) + (_comp.mdasld))) * 1.0m) * .165m);
        //diss += Math.Abs((((dyear - _currentParcel.myrblt) - (dyear - _comp.myrblt)) * 50) * .125m);

        //diss += Math.Abs(((_currentParcel.depreciatonValue - _comp.depreciatonValue) * .15m) * .125m);

        //diss += DifferenceFormula(_currentParcel.mtota, _comp.mtota, subDevs._sdSize, subWghts._wtSize);
        //diss += DifferenceFormula(_currentParcel.BasementArea, _comp.BasementArea, subDevs._sdBsmt, subWghts._wtBsmt);
        //diss += DifferenceFormula(_currentParcel.FinBasementArea, _comp.FinBasementArea, subDevs._sdFinBsmt, subWghts._wtFinBsmt);
        //diss += DifferenceFormula(_currentParcel.auxA, _comp.auxA, subDevs._sdAuxArea, subWghts._wtAuxArea);
        //diss += DifferenceFormula(_currentParcel.porA, _comp.porA, subDevs._sdPor, subWghts._wtPor);
        //diss += DifferenceFormula(_currentParcel.sporA, _comp.sporA, subDevs._sdSpor, subWghts._wtSpor);
        //diss += DifferenceFormula(_currentParcel.eporA, _comp.eporA, subDevs._sdEpor, subWghts._wtEpor);
        //diss += DifferenceFormula(_currentParcel.deckA, _comp.deckA, subDevs._sdDeck, subWghts._wtDeck);
        //diss += DifferenceFormula(_currentParcel.patioA, _comp.patioA, subDevs._sdPor, subWghts._wtPor);
        //diss += DifferenceFormula(_currentParcel.carportA, _comp.carportA, subDevs._sdCarPort, subWghts._wtCarPort);
        //diss += DifferenceFormula(_currentParcel.garA, _comp.garA, subDevs._sdGarage, subWghts._wtGarage);

        //diss += Math.Abs((Convert.ToDecimal(_currentParcel.mtfp - _comp.mtfp) * .25m) * .025m);

        //        decimal subBathValue = (((_currentParcel.mNfbth * 3) + (_currentParcel.mNhbth * 2)) * CamraSupport.PlumbingRate);
        //        decimal compBathValue = (((_comp.mNfbth * 3) + (_comp.mNhbth * 2)) * CamraSupport.PlumbingRate);
        //        diss += Math.Abs(((subBathValue - compBathValue) * 1) * .025m);
        //        diss += Math.Abs(((_currentParcel.computedFactor - _comp.computedFactor) * 1000) * 1);

        //        // Land value here ???

        //        diss += Math.Abs(((_currentParcel.mtotoi - _comp.mtotoi) * .25m) * .125m);
        //        diss += Math.Abs((((_currentParcel.LocationFactor * 1000) - (_comp.LocationFactor * 1000)) * .50m));

        //        if (_currentParcel.msubdv == _comp.msubdv && diss > 300)
        //        {
        //            diss += -300;
        //        }

        //        return Decimal.Round(diss, 5);
        //    }
        //}

        private decimal DifferenceFormula(decimal subject, decimal comp, decimal stdv, decimal weight)
        {
            if (stdv == 0)
                return 0;

            return Convert.ToDecimal(Math.Pow((((subject - comp).AsDouble() / stdv.AsDouble()) * weight.AsDouble()), 2));
        }

        public void BuildComparables(List<SearchParameter> _parms)
        {
            _dissimilarity = new SortedDictionary<decimal, int>();

            ParcelDataCollection _comps = new ParcelDataCollection(_db, _currentParcel.mrecno, _currentParcel.mdwell);
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

            if (_currentParcel != null)
            {
                DataRow row = _SCTable.NewRow();
                row["Type"] = "Subject";
                row["Record"] = _currentParcel.Record;
                row["911Add"] = _currentParcel.SiteAddress;
                row["Map No"] = _currentParcel.mmap;
                row["SalePrice"] = _currentParcel.msellp;
                row["SaleDate"] = _currentParcel.SalesDate;
                row["Index"] = "0";
                row["Sub Division"] = CamraSupport.SubDivisionCodeCollection._subDivDescription(_currentParcel.msubdv.ToString().Trim());
                row["Location"] = _currentParcel.LocationQuality;
                row["Acres"] = Convert.ToDecimal(_currentParcel.macreN.ToString("N3"));
                row["YearBuilt"] = _currentParcel.myrblt;
                row["Stories"] = Convert.ToDecimal(_currentParcel.mstorN.ToString("N2"));
                row["ExteriorWall"] = CamraSupport.ExteriorWallTypeCollection.Description(_currentParcel.mexwll.ToString());
                row["Size"] = _currentParcel.mtota;
                row["Bsmt"] = _currentParcel.BasementArea;
                row["Fin.Bsmt"] = _currentParcel.FinBasementArea;
                row["Rooms"] = _currentParcel.mNroom;
                row["BedRooms"] = _currentParcel.mNbr;
                row["Full Baths"] = _currentParcel.mNfbth;
                row["Half Baths"] = _currentParcel.mNhbth;
                row["Heat"] = CamraSupport.HeatTypeCollection.Description(_currentParcel.mheat.ToString());
                row["A/C"] = _currentParcel.mac;
                row["Fireplace"] = _currentParcel.mfpN;
                row["StackedFP"] = _currentParcel.msfpN;
                row["InOPFP"] = _currentParcel.miofpN;
                if (_currentParcel.GasLogRecords.Count >= 1)
                {
                    row["GasLogFP"] = _currentParcel.GasLogRecords[0].NbrGasFP;
                }
                row["Flues"] = _currentParcel.mflN;
                row["StackedFlue"] = _currentParcel.msflN;
                row["MetalFlue"] = _currentParcel.mmflN;
                row["Aux.Liv.Area"] = _currentParcel.auxA;
                row["Porch"] = _currentParcel.porA;
                row["Scrn.Porch"] = _currentParcel.sporA;
                row["Encl.Porch"] = _currentParcel.eporA;
                row["Deck"] = _currentParcel.deckA;
                row["Patio"] = _currentParcel.patioA;
                row["CarPort"] = _currentParcel.carportA;
                row["NoCars_CP"] = _currentParcel.mcarNc;
                row["Garage"] = _currentParcel.garA;
                if (_currentParcel.mgart != 64)
                {
                    row["NoCars_GAR"] = _currentParcel.mgarNc;
                }
                else if (_currentParcel.mgart == 64)
                {
                    row["NoCars_GAR"] = 0;
                }
                row["NoCars_BI"] = _currentParcel.mbiNc;
                row["Class"] = _currentParcel.Class;
                row["Factor"] = Convert.ToDecimal(_currentParcel.Factor);
                row["QualityAdj"] = _currentParcel.computedFactor;
                row["Condition"] = _currentParcel.conditionType;
                row["Deprec"] = Convert.ToDecimal(_currentParcel.Deprc);
                row["PlumbValue"] = _currentParcel.mtplum;
                row["HeatValue"] = _currentParcel.mtheat;
                row["ACValue"] = _currentParcel.mtac;
                row["FPValue"] = _currentParcel.mtfp;
                row["FlueValue"] = _currentParcel.mtfl;
                row["BIGarValue"] = _currentParcel.mtbi;
                row["SWLValue"] = _currentParcel.mswl;
                row["TotalBldgVal"] = _currentParcel.mtotbv;
                row["OtherImp"] = _currentParcel.mtotoi;
                row["LandValue"] = _currentParcel.mtotld;
                row["TotalValue"] = _currentParcel.mtotpr;
                row["NbrParcelsSold"] = _currentParcel.mmcode;

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
                    r["Sub Division"] = CamraSupport.SubDivisionCodeCollection._subDivDescription(_comp.msubdv.ToString().Trim());
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
            _db = db;
            _currentParcel = data;

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

        public void SortByDissimilarity()
        {
        }
    }
}