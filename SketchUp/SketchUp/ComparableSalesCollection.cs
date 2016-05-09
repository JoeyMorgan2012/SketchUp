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

    
    }
}