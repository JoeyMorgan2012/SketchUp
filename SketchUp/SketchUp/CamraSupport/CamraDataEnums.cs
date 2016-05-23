using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public static class CamraDataEnums
    {
#region "Public Methods"

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

#region "Enums"

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
        //public static List<string> Letters = new List<string>() { "A", "B", "C", "D", "F", "G", "H", "I", "J", "K", "L", "M" };
        public enum Letters
        {
            A,
            B,
            C,
            D,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M
        }
        /*
         public static List<string> AuxAreaTypes = new List<string>() { "BEGR", "EGAR", "FEGR", "RMAD", "SUNR", "RMAF", "RMAP", "RMTS" };
        */
        public enum CardinalDirection
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

        //public static List<string> CarPortTypes = new List<string>() { "CP", "BCP", "WCP", "BWCP", "UCP", "CPB", "CPU", "CPW", "CPWB" };
        public enum CarPortTypes
        {
            BCP,
            BWCP,
            CP,
            CPB,
            CPU,
            CPW,
            CPWB,
            UCP,
            WCP
        }

        //public static List<int> CommercialOccupancies = new List<int>() { 11, 13, 14, 26 };
        public enum CommercialOccupancyCodes
        {
            Apartment = 11,
            Commercial = 13,
            Industrical = 14,
            FairValueCommercial = 26
        }

        //public static List<string> EnclPorchTypes = new List<string>() { "EPOR", "EPR", "JPOR", "POEB", "POEF", "PORJ" };
        public enum EnclPorchTypes
        {
            BPAT,
            CPAT,
            PABK,
            PACN,
            PACV,
            PAT,
            PATO,
            PATW,
            WPAT
        }

        // public static List<string> GarageTypes = new List<string>()
        // { "GAR", "BGAR", "FGAR", "UGAR", "GARL","GARB","GARF","GABK","GCEB","GAFV","GACB","GALF","GAUB","GAUF","GCEF" };
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

        //public static List<string> InvalidCommercialSection = new List<string>() { "BASE", "ADD", "NBAD", "LAG", "OH" };
        public enum InvalidCommercialSection
        {
            BASE,
            ADD,
            NBAD,
            LAG,
            OH
        }

        public enum OccupancyType
        {
            CodeNotFound,
            Commercial,
            Residential,
            TaxExempt,
            Vacant,
            Other
        }

        //public static List<string> PatioTypes = new List<string>() { "PAT", "BPAT", "CPAT", "WPAT", "PATO", "PABK", "PACN", "PACV", "PATW" };
        public enum PatioTypes
        {
            BPAT,
            CPAT,
            PABK,
            PACN,
            PACV,
            PAT,
            PATO,
            PATW,
            WPAT
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

        //public static List<int> TaxExemptOccupancies = new List<int>() { 17 };
        public enum TaxExemptOccupancies
        {
            Exempt =17
        }

        //   public static List<int> VacantOccupancies = new List<int>() { 5, 15, 23, 25, 27 };
        public enum VacantOccupancies
        {
            VacantExempt = 5,
            VacantLand = 15,
            VacantWithMH = 23,
            VacantCommercial = 25,
            MobileHomePark = 27
        }

#endregion
    }
}
