using System;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public class CPBuildingSection
    {
        public event EventHandler<CPBuildingSectionChangedEventsArgs> CPBuildingSectionChangedEvent;

        public int Record
        {
            get; set;
        }

        public int Card
        {
            get; set;
        }

        public string SectionLetter
        {
            get; set;
        }

        public string SectionType
        {
            get; set;
        }

        public string SectionDescription
        {
            get; set;
        }

        public decimal SectionStory
        {
            get; set;
        }

        public decimal SectionSize
        {
            get; set;
        }

        public string SectionClass
        {
            get; set;
        }

        public decimal SectionFactor
        {
            get; set;
        }

        public decimal SectionDepreciation
        {
            get; set;
        }

        public string SectionNoDepreciation
        {
            get; set;
        }

        public int SectionDepreciationValue
        {
            get; set;
        }

        public int SectionFactorValue
        {
            get; set;
        }

        public decimal SectionRate
        {
            get; set;
        }

        public decimal SectionRateClassA
        {
            get; set;
        }

        public decimal SectionRateClassB
        {
            get; set;
        }

        public decimal SectionRateClassC
        {
            get; set;
        }

        public decimal SectionRateClassD
        {
            get; set;
        }

        public decimal SectionRateClassM
        {
            get; set;
        }

        public int SectionValue
        {
            get; set;
        }

        public int Occupancy
        {
            get; set;
        }

        public CPBuildingSection()
        {
        }

        public static CPBuildingSection GetSection(DBAccessManager fox, int recno, int card,
            string sectionLetter, int occupancy)
        {
            CPBuildingSection section = null;

            StringBuilder bssql = new StringBuilder();
            bssql.Append(" select jstype, jsstory, jssqft, jsclass, jsfactor, jsdeprc, js0depr ");
            bssql.Append(String.Format(" from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, recno, card, sectionLetter));

            DataSet ds = fox.RunSelectStatement(bssql.ToString());

            int sectionValue = 0;
            decimal sectionRate = 0;
            string sectionClass = "";
            string sectionType = "";
            string sectionDescription = "";
            int sectionDepreciationValue = 0;
            int sectionFactorValue = 0;

            var residentialTypes = (from t in CamraSupport.ResidentialSectionTypeCollection
                                    select t._resSectionType).ToList();
            var commercialTypes = (from t in CamraSupport.CommercialSectionTypeCollection
                                   select t._commSectionType).ToList();

            foreach (DataRow bsreader in ds.Tables[0].Rows)
            {
                sectionType = Convert.ToString(bsreader["jstype"].ToString().Trim());
                if (residentialTypes.Contains(sectionType))
                {
                    sectionRate = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionRate(sectionType);
                    sectionDescription = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionTypeDescription(sectionType);
                }
                else if (commercialTypes.Contains(sectionType))
                {
                    var comm = (from c in CamraSupport.CommercialSectionTypeCollection
                                where c._commSectionType == sectionType
                                select c).SingleOrDefault();

                    sectionClass = Convert.ToString(bsreader["jsclass"].ToString().Trim());
                    sectionDescription = CamraSupport.CommercialSectionTypeCollection.CommercialSectionTypeDescription(sectionType);
                    switch (sectionClass)
                    {
                        case "A":
                            sectionRate = comm._commSectionRateClassA;
                            break;

                        case "B":
                            sectionRate = comm._commSectionRateClassB;
                            break;

                        case "C":
                            sectionRate = comm._commSectionRateClassC;
                            break;

                        case "D":
                            sectionRate = comm._commSectionRateClassD;
                            break;

                        case "M":
                            sectionRate = comm._commSectionRateClassM;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    //throw new Exception("Section Type not found in Rat1");
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow bsreader = ds.Tables[0].Rows[0];

                decimal sectionSqft = Convert.ToDecimal(bsreader["jssqft"]);
                decimal sectionFactor = Convert.ToDecimal(bsreader["jsfactor"]);
                decimal sectionDepreciation = Convert.ToDecimal(bsreader["jsdeprc"]);

                if (CamraSupport.ResidentialOccupancies.Contains(occupancy))
                {
                    sectionValue = Convert.ToInt32(sectionRate * sectionSqft);
                }
                if (CamraSupport.CommercialOccupancies.Contains(occupancy))
                {
                    if (bsreader["js0depr"].ToString() != "Y")
                    {
                        sectionFactorValue = (((Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor))) -
                            (Convert.ToInt32(sectionSqft * sectionRate))));
                        sectionDepreciationValue = (((Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor))) -
                            (Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor) * (1 - sectionDepreciation)))));
                        sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor) * (1 - sectionDepreciation));
                    }
                    else
                    {
                        sectionFactorValue = ((Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor))) -
                            (Convert.ToInt32(sectionSqft * sectionRate)));
                        sectionDepreciationValue = 0;
                        sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor));
                    }
                }
                if (CamraSupport.TaxExemptOccupancies.Contains(occupancy))
                {
                    if (bsreader["js0depr"].ToString() != "Y")
                    {
                        sectionFactorValue = (((Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor))) -
                            (Convert.ToInt32(sectionSqft * sectionRate))));
                        sectionDepreciationValue = (((Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor))) -
                            (Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor) * (1 - sectionDepreciation)))));
                        sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor) * (1 - sectionDepreciation));
                    }
                    else
                    {
                        sectionFactorValue = ((Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor))) -
                            (Convert.ToInt32(sectionSqft * sectionRate)));
                        sectionDepreciationValue = 0;
                        sectionValue = Convert.ToInt32(sectionSqft * sectionRate * (1 + sectionFactor));
                    }
                }

                section = new CPBuildingSection()
                {
                    Record = recno,
                    Card = card,
                    SectionLetter = sectionLetter,
                    SectionType = bsreader["jstype"].ToString().TrimEnd(),
                    SectionDescription = sectionDescription.ToString().Trim(),
                    SectionStory = Convert.ToDecimal(bsreader["jsstory"].ToString()),
                    SectionSize = Convert.ToDecimal(bsreader["jssqft"].ToString()),
                    SectionClass = bsreader["jsclass"].ToString(),
                    SectionFactor = Convert.ToDecimal(bsreader["jsfactor"].ToString()),
                    SectionDepreciation = Convert.ToDecimal(bsreader["jsdeprc"].ToString()),
                    SectionNoDepreciation = bsreader["js0depr"].ToString(),
                    SectionRate = CamraSupport.ResidentialSectionTypeCollection.ResidentialSectionRate(bsreader["jstype"].ToString().Trim()),
                    SectionValue = sectionValue,
                    SectionDepreciationValue = sectionDepreciationValue,
                    SectionFactorValue = sectionFactorValue,
                    Occupancy = occupancy
                };
            }
            return section;
        }

        private void FireChangedEvent(string sectionfile)
        {
            if (CPBuildingSectionChangedEvent != null)
            {
                CPBuildingSectionChangedEvent(this,
                    new CPBuildingSectionChangedEventsArgs()
                    {
                        CPBuildingSectionName = sectionfile
                    });
            }
        }
    }

    public class CPBuildingSectionChangedEventsArgs : EventArgs
    {
        public CPBuildingSectionChangedEventsArgs()
            : base()
        {
        }

        public string CPBuildingSectionName
        {
            get; set;
        }
    }
}