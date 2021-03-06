﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public class SMLineManager
    {
        #region Breaking a line

        public SMParcel BreakLine(SMParcel parcelCopy, string sectionLetter,
       int lineNumber, PointF breakPoint)
        {
            var linesOut = new List<SMLine>();

            SMLine originalLine = (from l in
                parcelCopy.Sections.Where(s => s.SectionLetter == sectionLetter).FirstOrDefault().Lines.Where(n => n.LineNumber
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
                    sectionLetter).FirstOrDefault().Lines.Where(n =>
                    n.LineNumber < originalLine.LineNumber)
                                            select
         l).ToList();
                List<SMLine> linesAfter = (from l in parcelCopy.Sections.Where(s =>
                   s.SectionLetter ==
                   sectionLetter).FirstOrDefault().Lines.Where(n =>
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
                    sectionLetter).FirstOrDefault().Lines =
                    linesOut;
            }
            return parcelCopy;
        }

        public List<SMLine> LinesConnectedToJumpPoint(SMParcel parcel, PointF scaledJumpPoint)
        {
            try
            {
                var linesConnected = new List<SMLine>();
                if (parcel.AllSectionLines != null && parcel.AllSectionLines.Count > 0)
                {
                    linesConnected = (from l in parcel.AllSectionLines select l).Where(p => p.ScaledEndPoint == scaledJumpPoint || p.ScaledStartPoint == scaledJumpPoint).ToList();
                }
                return linesConnected;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        public SMSection SectionWithLineBreak(SMSection section, int lineNumber, PointF breakPoint)
        {
            var linesOut = new List<SMLine>();

            SMLine originalLine =
                (from l in section.Lines
                 .Where(n => n.LineNumber == lineNumber)
                 select l).FirstOrDefault();
            bool validBreakPoint =
                SMGlobal.PointIsOnLine(originalLine.StartPoint, originalLine.EndPoint, breakPoint);
            if (validBreakPoint)
            {
                SMLine Line1 = FirstHalfOfLine(breakPoint, originalLine);
                SMLine Line2 = SecondHalfOfLine(breakPoint, originalLine, "");
                List<SMLine> linesBefore = (from l in section.Lines.Where(n => n.LineNumber < originalLine.LineNumber) select l).ToList();
                List<SMLine> linesAfter =
                    (from l in section.Lines
                     .Where(n => n.LineNumber > originalLine.LineNumber)
                     select l).ToList();
                foreach (SMLine line in linesAfter)
                {
                    line.LineNumber += 1;
                }
                linesOut.AddRange(linesBefore);
                linesOut.Add(Line1);
                linesOut.Add(Line2);
                linesOut.AddRange(linesAfter);
                section.Lines = linesOut;
            }
            return section;
        }

        private static SMLine SecondHalfOfLine(PointF breakPoint, SMLine originalLine)
        {
            return new SMLine
            {
                AttachedSection = originalLine.AttachedSection,
                ComparisonPoint = originalLine.ComparisonPoint,
                Direction = originalLine.Direction,
                Card = originalLine.Card,
                LineNumber = originalLine.LineNumber + 1,
                SectionLetter = originalLine.SectionLetter,
                ParentParcel = originalLine.ParentParcel,
                ParentSection = originalLine.ParentSection,
                Record = originalLine.Record,
                StartX = breakPoint.X,
                StartY = breakPoint.Y,
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
                Card = originalLine.Card,
                LineNumber = originalLine.LineNumber + 1,
                SectionLetter = originalLine.SectionLetter,
                ParentParcel = originalLine.ParentParcel,
                ParentSection = originalLine.ParentSection,
                Record = originalLine.Record,
                StartX = breakPoint.X,
                StartY = breakPoint.Y,
                EndX = originalLine.EndX,
                EndY = originalLine.EndY
            };
        }

        private SMLine CopyLineWithNewEndPoint(SMLine originalLine, PointF breakPoint, PointF sketchOrigin)
        {
            try
            {
                double sketchScale = originalLine.ParentParcel.Scale;
                SMLine newLine = originalLine;
                newLine.EndX = breakPoint.X;
                newLine.EndY = breakPoint.Y;
                newLine.XLength = Math.Abs(newLine.EndX - newLine.StartX);
                newLine.YLength = Math.Abs(newLine.EndY - newLine.StartY);
                var lineStartX = (float)((newLine.StartX * sketchScale) + sketchOrigin.Y);
                var lineStartY = (float)((newLine.StartY * sketchScale) + sketchOrigin.Y);
                newLine.ScaledStartPoint = new PointF(lineStartX, lineStartY);
                return newLine;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private SMLine CopyWithNewStartPoint(SMLine originalLine, PointF breakPoint, PointF sketchOrigin)
        {
            double sketchScale = originalLine.ParentParcel.Scale;
            SMLine newLine = originalLine;
            newLine.StartX = breakPoint.X;
            newLine.StartY = breakPoint.Y;
            newLine.XLength = Math.Abs(newLine.EndX - newLine.StartX);
            newLine.YLength = Math.Abs(newLine.EndY - newLine.StartY);
            var lineStartX = (float)((newLine.StartX * sketchScale) + sketchOrigin.Y);
            var lineStartY = (float)((newLine.StartY * sketchScale) + sketchOrigin.Y);
            newLine.ScaledStartPoint = new PointF(lineStartX, lineStartY);
            newLine.LineNumber = originalLine.LineNumber + 1;
            return newLine;
        }

        private SMLine FirstHalfOfLine(PointF breakPoint, SMLine originalLine)
        {
            return new SMLine
            {
                AttachedSection = originalLine.AttachedSection,
                ComparisonPoint = originalLine.ComparisonPoint,
                Direction = originalLine.Direction,
                Card = originalLine.Card,
                LineNumber = originalLine.LineNumber,
                SectionLetter = originalLine.SectionLetter,
                ParentParcel = originalLine.ParentParcel,
                ParentSection = originalLine.ParentSection,
                Record = originalLine.Record,
                StartX = originalLine.StartX,
                StartY = originalLine.StartY,
                EndX = breakPoint.X,
                EndY = breakPoint.Y
            };
        }

        #endregion Breaking a line

        #region Combining lines

        // Not implemented until second release! JMM 04-26-16
        public SMLine CombinedLines(SMLine firstLine, SMLine secondLine)
        {
            var newLine = new SMLine
            {
                AttachedSection = firstLine.AttachedSection,
                ComparisonPoint = firstLine.ComparisonPoint,
                Direction = firstLine.Direction,
                Card = firstLine.Card,
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

        public bool LinesCanBeCombined(SMLine firstLine, SMLine secondLine)
        {
            bool firstLineNotAnAttachmentPoint = false;
            bool sameDirection = false;

            bool line1EndPointOnCombinedLine = false;
            bool line2StartPointOnCombinedLine = false;

            firstLineNotAnAttachmentPoint = (firstLine.AttachedSection.Trim() == string.Empty);
            sameDirection = (firstLine.Direction.Trim() == secondLine.Direction.Trim());
            line1EndPointOnCombinedLine = SMGlobal.PointIsOnLine(firstLine.ScaledStartPoint, secondLine.ScaledEndPoint, firstLine.ScaledEndPoint);
            line2StartPointOnCombinedLine = SMGlobal.PointIsOnLine(firstLine.ScaledStartPoint, secondLine.ScaledEndPoint, secondLine.ScaledStartPoint);
            bool canBeCombined = (sameDirection && line1EndPointOnCombinedLine && line2StartPointOnCombinedLine && firstLineNotAnAttachmentPoint);
            return canBeCombined;
        }

        #endregion Combining lines
    }
}