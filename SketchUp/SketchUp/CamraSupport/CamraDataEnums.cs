using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public static class CamraDataEnums
    {
        #region Enums to replace hard-coded arrays and "magic numbers"
        //public static List<int> CommercialOccupancies = new List<int>() { 11, 13, 14, 26 };
        public enum CommercialOccupancyCodes
        {
            Apartment = 11,
            Commercial = 13,
            Industrical = 14,
            FairValueCommercial = 26
        }

        // public static List<string> GarageTypes = new List<string>() 
        // { "GAR", "BGAR", "FGAR", "UGAR", "GARL","GARB","GARF","GABK","GACB","GCEB","GAFV","GACB","GALF","GAUB","GAUF","GCEF" };
        public enum GarageTypes
        {
            BGAR,
            FGAR,
            GABK,
            GACB,
            GAFV,
            GALF,
            GAR,
            GARB,
            GARF,
            GARL,
            GAUB,
            GAUF,
            GCEB,
            GCEF,
            UGAR
        }

        // public static List<int> IncomeOccupancies = new List<int>() { 11, 13, 14 };
        public enum IncomeOccupancies
        {
            Apartment = 11,
            Commercial = 13,
            Industrical = 14
        }

        /*
         public static List<string> AuxAreaTypes = new List<string>() { "BEGR", "EGAR", "FEGR", "RMAD", "SUNR", "RMAF", "RMAP", "RMTS" };
        */
        public enum AuxAreaTypes
        {
            BEGR,
            EGAR,
            FEGR,
            RMAD,
            SUNR,
            RMAF,
            RMAP,
            RMTS
        }
        //  public static List<string> ResidentialLandUseTypes = new List<string>() { "1", "2", "3", "5", "6" };

        //public static List<int> ResidentialOccupancies = new List<int>() { 10, 12, 16, 20, 21, 22, 24 };
        public enum ResidentialOccupancyCodes
        {
            Dwelling = 10,
            Farm = 12,
            FairValueRes = 16,
            TownhouseOrCondo = 20,
            SingleWideMobileHome = 21,
            DoubleWideMobileHome = 22,
            TripleWideMobileHome = 24

        }
        #endregion
        #region Helper Methods to replace static arrays
        public static List<string> GetEnumStrings(Type enumType)
        {
            var enumStrings = Enum.GetNames(enumType);
            List<string> stringValues = new List<string>();
            foreach (string n in enumStrings)
            {
                stringValues.Add(n);
            }
            stringValues.Sort();
            return stringValues;
        }
        public static List<int> GetEnumValues(Type enumType)
        {
            var enumValues = Enum.GetValues(enumType);
            List<int> values = new List<int>();
            foreach (int n in enumValues)
            {
                values.Add(n);
            }
            values.Sort();
            return values;
        } 
        #endregion
    }
}