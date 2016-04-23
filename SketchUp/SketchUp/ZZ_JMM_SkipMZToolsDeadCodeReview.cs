using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUp
{
    
    public class CodeRefactoringState : System.Attribute
    {
        public string ChangeDescription
        {
            get
            {
                return changeDescription;
            }
            set
            {
                changeDescription = value;
            }
        }

        public string CreatedBy
        {
            get
            {
                if (string.IsNullOrEmpty(createdBy))
                {
                    createdBy = "JMM";
                }
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        public string ExtractedFrom
        {
            get
            {
                return extractedFrom;
            }
            set
            {
                extractedFrom = value;
            }
        }

        public bool ExtractedMethod
        {
            get
            {
                return extractedMethod;
            }
            set
            {
                extractedMethod = value;
            }
        }

        public bool IsToDo
        {
            get
            {

                return (bool)(isToDo== null? true:isToDo);
            }
            set
            {
                isToDo = value;
            }
        }

        public DateTime ModifiedLast
        {
            get
            {
                if (modifiedLast==null)
                {
                    modifiedLast = DateTime.Now;
                }
                return (DateTime)modifiedLast;
            }
            set
            {
                modifiedLast = value;
            }
        }

        public bool ReplacesExistingCode
        {
            get
            {
                return replacesExistingCode;
            }
            set
            {
                replacesExistingCode = value;
            }
        }

        string changeDescription;
        string createdBy;
        string extractedFrom;
        bool extractedMethod;
        bool? isToDo;
        DateTime? modifiedLast;
        bool replacesExistingCode;
    }

    [System.AttributeUsage(System.AttributeTargets.All)]
    public class SkipMZToolsDeadCodeReview : System.Attribute
    {
    }
}
