/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public static class Rates
    {
        #region Public Methods

        public static decimal RateForSection(SMSection section)
        {
            decimal rate = 0.00M;

            switch (section.ParentParcel.ParcelMast.OccupancyType)
            {
                case CamraDataEnums.OccupancyType.CodeNotFound:
                    break;

                case CamraDataEnums.OccupancyType.Commercial:
                    rate = CommercialSectionRate(section);
                    break;

                case CamraDataEnums.OccupancyType.Residential:
                    rate = ResidentialSectionRate(section);
                    break;

                case CamraDataEnums.OccupancyType.TaxExempt:
                    if (SketchUpLookups.ResidentialSectionTypeCollection.FirstOrDefault(r => r.ResSectionType.Trim().ToUpper() == section.SectionType) != null)
                    {
                        rate = ResidentialSectionRate(section);
                    }
                    else if (SketchUpLookups.CommercialSectionTypeCollection.FirstOrDefault(c => c._commSectionType == section.SectionType) != null)
                    {
                        rate = CommercialSectionRate(section);
                    }
                    else
                    {
                        rate = 0.00M;
                        Trace.WriteLine($"Tax Exempt's Section Type {section.SectionType} not found in rate table.");
                    }
                    break;

                case CamraDataEnums.OccupancyType.Vacant:
                    rate = 0.00M;
                    break;

                default:
                    rate = 0.00M;
                    Trace.WriteLine($"Occupancy type is \"{section.ParentParcel.ParcelMast.OccupancyType}\" or not recognized.; Cannot determine rate from rate table.");
                    break;
            }

            return rate;
        }
        public static decimal RateForSection(CamraDataEnums.OccupancyType occType, string sectionType, string sectionClass)
        {
            decimal rate = 0.00M;

            switch (occType)
            {
                case CamraDataEnums.OccupancyType.CodeNotFound:
                    break;

                case CamraDataEnums.OccupancyType.Commercial:
                    rate = CommercialSectionRate(sectionType, sectionClass);
                    break;

                case CamraDataEnums.OccupancyType.Residential:
                    rate = ResidentialSectionRate(sectionType, sectionClass);
                    break;

                case CamraDataEnums.OccupancyType.TaxExempt:
                    if (SketchUpLookups.ResidentialSectionTypeCollection.FirstOrDefault(r => r.ResSectionType.Trim().ToUpper() == sectionType) != null)
                    {
                        rate = ResidentialSectionRate(sectionType, sectionClass);
                    }
                    else if (SketchUpLookups.CommercialSectionTypeCollection.FirstOrDefault(c => c._commSectionType == sectionType) != null)
                    {
                        rate = CommercialSectionRate(sectionType, sectionClass);
                    }
                    else
                    {
                        rate = 0.00M;
                        Trace.WriteLine($"Tax Exempt's Section Type {sectionType} not found in rate table.");
                    }
                    break;

                case CamraDataEnums.OccupancyType.Vacant:
                    rate = 0.00M;
                    break;

                default:
                    rate = 0.00M;
                    Trace.WriteLine($"Occupancy type is \"{occType}\" or not recognized.; Cannot determine rate from rate table.");
                    break;
            }

            return rate;
        }
        private static decimal CommercialSectionRate(string sectionType, string sectionClass)
        {
            decimal rate;
            CommercialSections commType = (from c in SketchUpLookups.CommercialSectionTypeCollection where c._commSectionType.ToUpper().Trim() == sectionType select c).FirstOrDefault();
            if (commType == null)
            {
                rate = 0.00M;
                Trace.WriteLine($"Commercial type {sectionType} was not found in the rate table.");
            }
            else
            {
                switch (sectionClass)
                {
                    case "A":
                        rate = commType._commSectionRateClassA;
                        break;

                    case "B":
                        rate = commType._commSectionRateClassB;
                        break;

                    case "C":
                        rate = commType._commSectionRateClassC;
                        break;

                    case "D":
                        rate = commType._commSectionRateClassD;
                        break;

                    case "M":
                        rate = commType._commSectionRateClassM;
                        break;

                    default:
                        rate = 0.00M;
                        Trace.WriteLine($"Section's Class is not valid.\nChoices are A,B,C,D and M.");
                        Trace.WriteLine($"Class {sectionClass} was not found in the rate table.");
                        break;
                }
            }
            return rate;
        }

        private static decimal CommercialSectionRate(SMSection section)
        {
            decimal rate;
            CommercialSections commType = (from c in SketchUpLookups.CommercialSectionTypeCollection where c._commSectionType.ToUpper().Trim() == section.SectionType select c).FirstOrDefault();
            if (commType == null)
            {
                rate = 0.00M;
                Trace.WriteLine($"Commercial type {section.SectionType} was not found in the rate table.");
            }
            else
            {
                switch (section.SectionClass)
                {
                    case "A":
                        rate = commType._commSectionRateClassA;
                        break;

                    case "B":
                        rate = commType._commSectionRateClassB;
                        break;

                    case "C":
                        rate = commType._commSectionRateClassC;
                        break;

                    case "D":
                        rate = commType._commSectionRateClassD;
                        break;

                    case "M":
                        rate = commType._commSectionRateClassM;
                        break;

                    default:
                        rate = 0.00M;
                        Trace.WriteLine($"Section's Class is not valid.\nChoices are A,B,C,D and M.");
                        Trace.WriteLine($"Class {section.SectionClass} was not found in the rate table.");
                        break;
                }
            }
            return rate;
        }

        private static decimal ResidentialSectionRate(string sectionType, string sectionClass)
        {
            decimal rate;
            ResidentalSections resType = (from r in SketchUpLookups.ResidentialSectionTypeCollection where r.ResSectionType.ToUpper().Trim() == sectionType select r).FirstOrDefault();
            rate = resType == null ? 0.00M : resType.ResSectionRate;
            if (resType == null)
            {
                rate = 0.00M;
                Trace.WriteLine($"Residential type {sectionType} was not found in the rate table.");
            }
            else
            {
                rate = resType.ResSectionRate;
            }
            return rate;
        }
        private static decimal ResidentialSectionRate(SMSection section)
        {
            decimal rate;
            ResidentalSections resType = (from r in SketchUpLookups.ResidentialSectionTypeCollection where r.ResSectionType.ToUpper().Trim() == section.SectionType select r).FirstOrDefault();
            rate = resType == null ? 0.00M : resType.ResSectionRate;
            if (resType == null)
            {
                rate = 0.00M;
                Trace.WriteLine($"Residential type {section.SectionType} was not found in the rate table.");
            }
            else
            {
                rate = resType.ResSectionRate;
            }
            return rate;
        }

        #endregion Public Methods

        #region Public Fields

        public static decimal AirCondRate;
        public static decimal BasementRate;
        public static SortedDictionary<string, decimal> ClassValues;
        public static int ExtraKitRate;
        public static decimal FinBasementDefaultRate;
        public static decimal FirePlaceRate;
        public static decimal FirePlaceStackRate;
        public static decimal FlrHeatRate;
        public static decimal FlueIncRate;
        public static decimal FlueRate;
        public static decimal GasLogRate;
        public static decimal MaxACValue;
        public static decimal MetalFlueRate;
        public static decimal MinMHValue;
        public static decimal NoHeatRate;
        public static decimal OutBldRateLim;
        public static decimal PierRate;
        public static decimal PlumbingRate;
        public static Rat1Master Rate1Master;
        public static decimal SlabRate;

        #endregion Public Fields
    }
}