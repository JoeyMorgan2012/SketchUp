using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SWallTech
{
    public class SMLineManager
    {
        #region Breaking a line

        public SMParcel BreakLine(SMParcel parcelCopy, string sectionLetter,
            int lineNumber, PointF breakPoint, PointF sketchOrigin)
        {
            List<SMLine> linesOut = new List<SMLine>();

            SMLine originalLine = (from l in
                parcelCopy.Sections.Where(s => s.SectionLetter == sectionLetter).FirstOrDefault<SMSection>().Lines.Where(n => n.LineNumber
                    == lineNumber)
                                   select l).FirstOrDefault<SMLine>();

            bool validBreakPoint =
                SMGlobal.PointIsOnLine(originalLine.StartPoint,
                originalLine.EndPoint, breakPoint);
            if (validBreakPoint)
            {
                SMLine Line1 = FirstHalfOfLine(breakPoint, originalLine);

                SMLine Line2 = SecondHalfOfLine(breakPoint, originalLine);
                List<SMLine> linesBefore = (from l in parcelCopy.Sections.Where(s =>
                    s.SectionLetter ==
                    sectionLetter).FirstOrDefault<SMSection>().Lines.Where(n =>
                    n.LineNumber < originalLine.LineNumber)
                                            select
         l).ToList();
                List<SMLine> linesAfter = (from l in parcelCopy.Sections.Where(s =>
                   s.SectionLetter ==
                   sectionLetter).FirstOrDefault<SMSection>().Lines.Where(n =>
                   n.LineNumber > originalLine.LineNumber)
                                           select l).ToList();
                foreach (SMLine line in linesAfter)
                {
                    line.LineNumber += 1;
                }
                linesOut.AddRange(linesBefore);
                linesOut.Add(Line1);
                linesOut.Add(Line2);
                linesOut.AddRange(linesAfter);
                parcelCopy.Sections.Where(s => s.SectionLetter ==
                    sectionLetter).FirstOrDefault<SMSection>().Lines =
                    linesOut;
            }
            return parcelCopy;
        }

        public SMParcel BreakLine(SMParcel parcelCopy, string sectionLetter,
      int lineNumber, PointF breakPoint, PointF sketchOrigin, string newSectionLetter)
        {
            List<SMLine> linesOut = new List<SMLine>();

            SMLine originalLine = (from l in
                parcelCopy.Sections.Where(s => s.SectionLetter == sectionLetter).FirstOrDefault<SMSection>().Lines.Where(n => n.LineNumber
                    == lineNumber)
                                   select l).FirstOrDefault<SMLine>();

            bool validBreakPoint =
                SMGlobal.PointIsOnLine(originalLine.StartPoint,
                originalLine.EndPoint, breakPoint);
            if (validBreakPoint)
            {
                SMLine Line1 = FirstHalfOfLine(breakPoint, originalLine);

                SMLine Line2 = SecondHalfOfLine(breakPoint, originalLine, "");
                List<SMLine> linesBefore = (from l in parcelCopy.Sections.Where(s =>
                    s.SectionLetter ==
                    sectionLetter).FirstOrDefault<SMSection>().Lines.Where(n =>
                    n.LineNumber < originalLine.LineNumber)
                                            select
         l).ToList();
                List<SMLine> linesAfter = (from l in parcelCopy.Sections.Where(s =>
                   s.SectionLetter ==
                   sectionLetter).FirstOrDefault<SMSection>().Lines.Where(n =>
                   n.LineNumber > originalLine.LineNumber)
                                           select l).ToList();
                foreach (SMLine line in linesAfter)
                {
                    line.LineNumber += 1;
                }
                linesOut.AddRange(linesBefore);
                linesOut.Add(Line1);
                linesOut.Add(Line2);
                linesOut.AddRange(linesAfter);
                parcelCopy.Sections.Where(s => s.SectionLetter ==
                    sectionLetter).FirstOrDefault<SMSection>().Lines =
                    linesOut;
            }
            return parcelCopy;
        }

        private static SMLine SecondHalfOfLine(PointF breakPoint, SMLine originalLine)
        {
            return new SMLine
            {
                AttachedSection = originalLine.AttachedSection,
                ComparisonPoint = originalLine.ComparisonPoint,
                Direction = originalLine.Direction,
                Dwelling = originalLine.Dwelling,
                LineNumber = originalLine.LineNumber + 1,
                SectionLetter = originalLine.SectionLetter,
                ParentParcel = originalLine.ParentParcel,
                ParentSection = originalLine.ParentSection,
                Record = originalLine.Record,
                StartX = (decimal)breakPoint.X,
                StartY = (decimal)breakPoint.Y,
                EndX = originalLine.EndX,
                EndY = originalLine.EndY
            };
        }

        private static SMLine SecondHalfOfLine(PointF breakPoint, SMLine originalLine, string newSectionLetter)
        {
            return new SMLine
            {
                AttachedSection = newSectionLetter,
                ComparisonPoint = originalLine.ComparisonPoint,
                Direction = originalLine.Direction,
                Dwelling = originalLine.Dwelling,
                LineNumber = originalLine.LineNumber + 1,
                SectionLetter = originalLine.SectionLetter,
                ParentParcel = originalLine.ParentParcel,
                ParentSection = originalLine.ParentSection,
                Record = originalLine.Record,
                StartX = (decimal)breakPoint.X,
                StartY = (decimal)breakPoint.Y,
                EndX = originalLine.EndX,
                EndY = originalLine.EndY
            };
        }

        private static SMLine FirstHalfOfLine(PointF breakPoint, SMLine originalLine)
        {
            return new SMLine
            {
                AttachedSection = originalLine.AttachedSection,
                ComparisonPoint = originalLine.ComparisonPoint,
                Direction = originalLine.Direction,
                Dwelling = originalLine.Dwelling,
                LineNumber = originalLine.LineNumber,
                SectionLetter = originalLine.SectionLetter,
                ParentParcel = originalLine.ParentParcel,
                ParentSection = originalLine.ParentSection,
                Record = originalLine.Record,
                StartX = originalLine.StartX,
                StartY = originalLine.StartY,
                EndX = (decimal)breakPoint.X,
                EndY = (decimal)breakPoint.Y
            };
        }

        private SMLine CopyWithNewStartPoint(SMLine originalLine, PointF breakPoint, PointF sketchOrigin)
        {
            decimal sketchScale = originalLine.ParentParcel.Scale;
            SMLine newLine = originalLine;
            newLine.StartX = (decimal)breakPoint.X;
            newLine.StartY = (decimal)breakPoint.Y;
            newLine.XLength = Math.Abs(newLine.EndX - newLine.StartX);
            newLine.YLength = Math.Abs(newLine.EndY - newLine.StartY);
            var lineStartX = (float)((newLine.StartX * sketchScale) + (decimal)sketchOrigin.Y);
            var lineStartY = (float)((newLine.StartY * sketchScale) + (decimal)sketchOrigin.Y);
            newLine.ScaledStartPoint = new PointF(lineStartX, lineStartY);
            newLine.LineNumber = originalLine.LineNumber + 1;
            return newLine;
        }

        private SMLine CopyLineWithNewEndPoint(SMLine originalLine, PointF breakPoint, PointF sketchOrigin)
        {
            decimal sketchScale = originalLine.ParentParcel.Scale;
            SMLine newLine = originalLine;
            newLine.EndX = (decimal)breakPoint.X;
            newLine.EndY = (decimal)breakPoint.Y;
            newLine.XLength = Math.Abs(newLine.EndX - newLine.StartX);
            newLine.YLength = Math.Abs(newLine.EndY - newLine.StartY);
            var lineStartX = (float)((newLine.StartX * sketchScale) + (decimal)sketchOrigin.Y);
            var lineStartY = (float)((newLine.StartY * sketchScale) + (decimal)sketchOrigin.Y);
            newLine.ScaledStartPoint = new PointF(lineStartX, lineStartY);
            return newLine;
        }

        #endregion Breaking a line

        #region Combining lines

        public bool LinesCanBeCombined(SMLine firstLine, SMLine secondLine)
        {
            bool secondLineNotAnAttachmentPoint = false;
            bool sameDirection = false;

            bool line1EndPointOnCombinedLine = false;
            bool line2StartPointOnCombinedLine = false;

            secondLineNotAnAttachmentPoint = (secondLine.AttachedSection.Trim() == string.Empty);
            sameDirection = (firstLine.Direction.Trim() == secondLine.Direction.Trim());
            line1EndPointOnCombinedLine = SMGlobal.PointIsOnLine(firstLine.ScaledStartPoint, secondLine.ScaledEndPoint, firstLine.ScaledEndPoint);
            line2StartPointOnCombinedLine = SMGlobal.PointIsOnLine(firstLine.ScaledStartPoint, secondLine.ScaledEndPoint, secondLine.ScaledStartPoint);
            bool canBeCombined = (sameDirection && line1EndPointOnCombinedLine && line2StartPointOnCombinedLine && secondLineNotAnAttachmentPoint);
            return canBeCombined;
        }

        public SMLine CombinedLines(SMLine firstLine, SMLine secondLine)
        {
            SMLine newLine = new SMLine
            {
                AttachedSection = firstLine.AttachedSection,
                ComparisonPoint = firstLine.ComparisonPoint,
                Direction = firstLine.Direction,
                Dwelling = firstLine.Dwelling,
                LineNumber = firstLine.LineNumber,
                SectionLetter = firstLine.SectionLetter,
                ParentParcel = firstLine.ParentParcel,
                ParentSection = firstLine.ParentSection,
                Record = firstLine.Record,
                StartX = firstLine.StartX,
                StartY = firstLine.StartY,
                EndX = secondLine.EndX,
                EndY = secondLine.EndY
            };
            return newLine;
        }

        #endregion Combining lines
    }
}