using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	//Refactored several stringbuilders to strings and extracted many long code runs into separate methods. JMM Feb 2016
	public partial class ExpandoSketch : Form
	{
		#region DataTable Construction Refactored

		private DataTable ConstructAreaTable()
		{
			DataTable at = new DataTable();
			at.Columns.Add("XPt1", typeof(decimal));
			at.Columns.Add("YPt1", typeof(decimal));
			at.Columns.Add("XPt2", typeof(decimal));
			at.Columns.Add("YPt2", typeof(decimal));
			return at;
		}

		private DataTable ConstructAttachmentPointsDataTable()
		{
			DataTable atpt = new DataTable();
			atpt.Columns.Add("Sect", typeof(string));
			atpt.Columns.Add("X1", typeof(decimal));
			atpt.Columns.Add("Y1", typeof(decimal));
			atpt.Columns.Add("X2", typeof(decimal));
			atpt.Columns.Add("Y2", typeof(decimal));
			return atpt;
		}

		private DataTable ConstructAttachPointsDataTable()
		{
			DataTable atpt = new DataTable();
			atpt.Columns.Add("RecNo", typeof(int));
			atpt.Columns.Add("CardNo", typeof(int));
			atpt.Columns.Add("Sect", typeof(string));
			atpt.Columns.Add("Direct", typeof(string));
			atpt.Columns.Add("Xpt1", typeof(decimal));
			atpt.Columns.Add("Ypt1", typeof(decimal));
			atpt.Columns.Add("Xpt2", typeof(decimal));
			atpt.Columns.Add("Ypt2", typeof(decimal));
			atpt.Columns.Add("Attch", typeof(string));
			return atpt;
		}

		private DataTable ConstructDupAttPointsTable()
		{
			DataTable dupAtPt = new DataTable();
			dupAtPt.Columns.Add("RecNo", typeof(int));
			dupAtPt.Columns.Add("CardNo", typeof(int));
			dupAtPt.Columns.Add("Sect", typeof(string));
			dupAtPt.Columns.Add("LineNo", typeof(int));
			dupAtPt.Columns.Add("Direct", typeof(string));
			dupAtPt.Columns.Add("Xpt1", typeof(decimal));
			dupAtPt.Columns.Add("Ypt1", typeof(decimal));
			dupAtPt.Columns.Add("Xpt2", typeof(decimal));
			dupAtPt.Columns.Add("Ypt2", typeof(decimal));
			dupAtPt.Columns.Add("Attch", typeof(string));
			dupAtPt.Columns.Add("Index", typeof(int));
			return dupAtPt;
		}

		private DataTable ConstructJumpTable()
		{
			DataTable jt = new DataTable();
			jt.Columns.Add("Record", typeof(int));
			jt.Columns.Add("Card", typeof(int));
			jt.Columns.Add("Sect", typeof(string));
			jt.Columns.Add("LineNo", typeof(int));
			jt.Columns.Add("Direct", typeof(string));
			jt.Columns.Add("XLen", typeof(decimal));
			jt.Columns.Add("YLen", typeof(decimal));
			jt.Columns.Add("Length", typeof(decimal));
			jt.Columns.Add("Angle", typeof(decimal));
			jt.Columns.Add("XPt1", typeof(decimal));
			jt.Columns.Add("YPt1", typeof(decimal));
			jt.Columns.Add("XPt2", typeof(decimal));
			jt.Columns.Add("YPt2", typeof(decimal));
			jt.Columns.Add("Attach", typeof(string));
			jt.Columns.Add("Dist", typeof(decimal));
			return jt;
		}

		private DataTable ConstructMulPtsTable()
		{
			DataTable mp = new DataTable();
			mp.Columns.Add("Sect", typeof(string));
			mp.Columns.Add("Line", typeof(int));
			mp.Columns.Add("X1", typeof(decimal));
			mp.Columns.Add("Y1", typeof(decimal));
			mp.Columns.Add("X2", typeof(decimal));
			mp.Columns.Add("Y2", typeof(decimal));
			return mp;
		}

		private DataTable ConstructREJumpTable()
		{
			DataTable ret = new DataTable();
			try
			{
				ret.Columns.Add("Record", typeof(int));
				ret.Columns.Add("Card", typeof(int));
				ret.Columns.Add("Sect", typeof(string));
				ret.Columns.Add("LineNo", typeof(int));
				ret.Columns.Add("Direct", typeof(string));
				ret.Columns.Add("XLen", typeof(decimal));
				ret.Columns.Add("YLen", typeof(decimal));
				ret.Columns.Add("Length", typeof(decimal));
				ret.Columns.Add("Angle", typeof(decimal));
				ret.Columns.Add("XPt1", typeof(decimal));
				ret.Columns.Add("YPt1", typeof(decimal));
				ret.Columns.Add("XPt2", typeof(decimal));
				ret.Columns.Add("YPt2", typeof(decimal));
				ret.Columns.Add("Attach", typeof(string));
				ret.Columns.Add("Dist", typeof(decimal));
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
			return ret;
		}

		private DataTable ConstructRESpJumpTable()
		{
			DataTable resp = new DataTable();
			resp.Columns.Add("Record", typeof(int));
			resp.Columns.Add("Card", typeof(int));
			resp.Columns.Add("Sect", typeof(string));
			resp.Columns.Add("LineNo", typeof(int));
			resp.Columns.Add("Direct", typeof(string));
			resp.Columns.Add("XLen", typeof(decimal));
			resp.Columns.Add("YLen", typeof(decimal));
			resp.Columns.Add("Length", typeof(decimal));
			resp.Columns.Add("Angle", typeof(decimal));
			resp.Columns.Add("XPt1", typeof(decimal));
			resp.Columns.Add("YPt1", typeof(decimal));
			resp.Columns.Add("XPt2", typeof(decimal));
			resp.Columns.Add("YPt2", typeof(decimal));
			resp.Columns.Add("Attach", typeof(string));
			resp.Columns.Add("Dist", typeof(decimal));
			return resp;
		}

		private DataTable ConstructSectionLtrs()
		{
			DataTable sl = new DataTable();
			sl.Columns.Add("RecNo", typeof(int));
			sl.Columns.Add("CardNo", typeof(int));
			sl.Columns.Add("CurSecLtr", typeof(string));
			sl.Columns.Add("NewSecLtr", typeof(string));
			sl.Columns.Add("NewType", typeof(string));
			sl.Columns.Add("SectSize", typeof(decimal));
			return sl;
		}

		private DataTable ConstructSectionTable()
		{
			DataTable st = new DataTable();

			st.Columns.Add("Record", typeof(int));
			st.Columns.Add("Card", typeof(int));
			st.Columns.Add("Sect", typeof(string));
			st.Columns.Add("LineNo", typeof(int));
			st.Columns.Add("Direct", typeof(string));
			st.Columns.Add("XLen", typeof(decimal));
			st.Columns.Add("YLen", typeof(decimal));
			st.Columns.Add("Length", typeof(decimal));
			st.Columns.Add("Angle", typeof(decimal));
			st.Columns.Add("XPt1", typeof(decimal));
			st.Columns.Add("YPt1", typeof(decimal));
			st.Columns.Add("XPt2", typeof(decimal));
			st.Columns.Add("YPt2", typeof(decimal));
			st.Columns.Add("Attach", typeof(string));
			return st;
		}

		private DataTable ConstructSortDistanceTable()
		{
			sortDist = new DataTable();
			sortDist.Columns.Add("Sect", typeof(string));
			sortDist.Columns.Add("Line", typeof(int));
			sortDist.Columns.Add("Direct", typeof(string));
			sortDist.Columns.Add("Xdist", typeof(decimal));
			sortDist.Columns.Add("Ydist", typeof(decimal));
			sortDist.Columns.Add("Length", typeof(decimal));
			return sortDist;
		}

		private DataTable ConstructUndoPointsTable()
		{
			undoPoints = new DataTable();
			undoPoints.Columns.Add("Direct", typeof(string));
			undoPoints.Columns.Add("X1pt", typeof(int));
			undoPoints.Columns.Add("Y1pt", typeof(int));
			undoPoints.Columns.Add("X2pt", typeof(int));
			undoPoints.Columns.Add("Y2pt", typeof(int));
			return undoPoints;
		}

		#endregion DataTable Construction Refactored

		#region Refactored out from constructor

		private void AddJumpTableRow(float jx, float jy, float CurrentScale, DataSet lines, int i)
		{
			decimal Distance = 0;

			DataRow row = JumpTable.NewRow();
			row["Record"] = Convert.ToInt32(lines.Tables[0].Rows[i]["jlrecord"].ToString());
			row["Card"] = Convert.ToInt32(lines.Tables[0].Rows[i]["jldwell"].ToString());
			row["Sect"] = lines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
			row["LineNo"] = Convert.ToInt32(lines.Tables[0].Rows[i]["jlline#"].ToString());
			row["Direct"] = lines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
			row["XLen"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlxlen"].ToString());
			row["YLen"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlylen"].ToString());
			row["Length"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jllinelen"].ToString());
			row["Angle"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlangle"].ToString());
			row["XPt1"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt1x"].ToString());
			row["YPt1"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt1y"].ToString());
			row["XPt2"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2x"].ToString());
			row["YPt2"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2Y"].ToString());
			row["Attach"] = lines.Tables[0].Rows[i]["jlattach"].ToString();

			decimal xPt2 = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2x"].ToString());
			decimal yPt2 = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2y"].ToString());

			float xPoint = (ScaleBaseX + (Convert.ToSingle(xPt2) * CurrentScale));
			float yPoint = (ScaleBaseY + (Convert.ToSingle(yPt2) * CurrentScale));

			float xDiff = (jx - xPoint);
			float yDiff = (jy - yPoint);

			double xDiffSquared = Math.Pow(Convert.ToDouble(xDiff), 2);
			double yDiffSquared = Math.Pow(Convert.ToDouble(yDiff), 2);

			Distance = Convert.ToDecimal(Math.Sqrt(Math.Pow(Convert.ToDouble(xDiff), 2) + Math.Pow(Convert.ToDouble(yDiff), 2)));

			row["Dist"] = Distance;

			JumpTable.Rows.Add(row);
		}

		private void AddListItemsToJumpTableList(float jx, float jy, float CurrentScale, DataSet lines)
		{
			try
			{
				for (int i = 0; i < lines.Tables[0].Rows.Count; i++)
				{
					AddJumpTableRow(jx, jy, CurrentScale, lines, i);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		private void AddMaster()
		{
			decimal summedArea = 0;
			decimal baseStory = 0;

			StringBuilder sumArea = new StringBuilder();
			sumArea.Append(String.Format("select sum(jssqft) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
					  MainForm.localLib,
						  MainForm.localPreFix,

					   //MainForm.FClib,
					   //MainForm.FCprefix,
					   _currentParcel.Record,
					   _currentParcel.Card));

			try
			{
				summedArea = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(sumArea.ToString()));
			}
			catch
			{
			}

			StringBuilder getStory = new StringBuilder();
			getStory.Append(String.Format("select jsstory from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = 'A'  ",
					   MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.Record,
						_currentParcel.Card));

			try
			{
				baseStory = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(getStory.ToString()));
			}
			catch
			{
			}

			DataSet ds_master = UpdateMasterArea(summedArea);

			if (_deleteMaster == false)
			{
				InsertMasterRecord(summedArea, baseStory, ds_master);
			}
		}

		public void AddNewPoint()
		{
			this.SaveSketchData();
		}

		private void AddNewSection(int secCnt)
		{
			StringBuilder addSectSQL = new StringBuilder();
			addSectSQL.Append(String.Format("insert into {0}.{1}section ( jsrecord,jsdwell,jssect,jstype,jsstory,jsdesc,jssketch,jssqft, ", MainForm.localLib, MainForm.localPreFix));
			addSectSQL.Append(" js0depr,jsclass,jsvalue,jsfactor,jsdeprc ) ");
			addSectSQL.Append(String.Format("values ({0},{1},'{2}','{3}',{4},'{5}','{6}',{7},'{8}','{9}',{10},{11},{12} ) ",
				_currentParcel.Record,
				_currentParcel.Card,
				_nextSectLtr.Trim(),
				_nextSectType.Trim(),
				Math.Round(_nextStoryHeight, 2),
				" ",
				"Y",
			   _nextSectArea,
				" ",
				_currentParcel.mclass.Trim(),
				0,
				0,
				0));

			if (secCnt == 0)
			{
				dbConn.DBConnection.ExecuteNonSelectStatement(addSectSQL.ToString());
			}
		}

		private void AddSections()
		{
			_addSection = true;
			NewSectionPoints.Clear();
			lineCnt = 0;
			SectionTypes sktype = new SectionTypes(dbConn, _currentParcel, _addSection, lineCnt, _isNewSketch);
			sktype.ShowDialog(this);

			int test = _currentParcel.mcarpt;

			_nextSectLtr = SectionTypes._nextSectLtr;
			_nextSectType = SectionTypes._nextSectType;
			_nextStoryHeight = SectionTypes._nextSectStory;
			_nextLineCount = SectionTypes._nextLineCount;

			if (_nextSectLtr != "A")
			{
				_hasNewSketch = false;
			}
			if (_nextSectLtr == "A")
			{
				_hasNewSketch = true;
			}

			try
			{
				FieldText.Text = String.Format("Sect- {0}, {1} sty {2}", _nextSectLtr.Trim(), _nextStoryHeight.ToString("N2"), _nextSectType.Trim());
			}
			catch
			{
			}
		}

		private void AddSectionSQL(string dirct, float dist)
		{
			int secCnt = GetSectionsCount();

			//MessageBox.Show(String.Format("Insertint into Section Record - {0}, Card - {1} at 3229", _currentParcel.Record, _currentParcel.Card));

			if (_nextSectLtr != String.Empty)
			{
				AddNewSection(secCnt);
			}

			_currentSection = new SectionDataCollection(dbConn, _currentParcel.Record, _currentParcel.Card);

			LoadAttachmentPointsDataTable();

			LoadStartPointsDataTable();

			//TODO: Look at this stepping through
			SetNewSectionAttachmentPoint();

			TempAttSplineNo = _savedAttLine;

			decimal sptline = splitLineDist;

			if (splitLineDist > 0)
			{
				SplitLine();
			}
		}

		private int GetSectionsCount()
		{
			StringBuilder checkSect = new StringBuilder();
			checkSect.Append(String.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ",
						   MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card,
							_nextSectLtr));

			int secCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(checkSect.ToString()));
			return secCnt;
		}

		private void InsertMasterRecord(decimal summedArea, decimal baseStory, DataSet ds_master)
		{
			if (ds_master.Tables[0].Rows.Count == 0)
			{
				StringBuilder insMaster = new StringBuilder();
				insMaster.Append(String.Format("insert into {0}.{1}master (jmrecord,jmdwell,jmsketch,jmstory,jmstoryex,jmscale,jmtotsqft,jmesketch) ",
							  MainForm.localLib,
							   MainForm.localPreFix

								//MainForm.FClib,
								//MainForm.FCprefix
								));
				insMaster.Append(String.Format("values ({0},{1},'{2}',{3},'{4}',{5},{6},'{7}' ) ",
							_currentParcel.Record,
							_currentParcel.Card,
							"Y",
							baseStory,
							String.Empty,
							1.00,
							summedArea,
							String.Empty));

				dbConn.DBConnection.ExecuteNonSelectStatement(insMaster.ToString());
			}
		}

		private void LoadAttachmentPointsDataTable()
		{
			DataSet attachPointsDataSet = null;

			string attachSQL = String.Format("select jlsect,jlpt1x,jlpt1y,jlpt2x,jlpt2y from {0}.{1}line where jlline# = 1 and jlrecord = {2} and jldwell = {3} and jlsect <> 'A' ",
								  MainForm.localLib,
								  MainForm.localPreFix,
								   _currentParcel.Record,
								   _currentParcel.Card);

			attachPointsDataSet = dbConn.DBConnection.RunSelectStatement(attachSQL);

			if (attachPointsDataSet.Tables[0].Rows.Count > 0)
			{
				LoadAttPtsDT(attachPointsDataSet);
			}
		}

		private void LoadAttPtsDT(DataSet attachPointsDataSet)
		{
			AttachmentPointsDataTable.Clear();

			for (int i = 0; i < attachPointsDataSet.Tables[0].Rows.Count; i++)
			{
				DataRow row = AttachmentPointsDataTable.NewRow();
				row["Sect"] = attachPointsDataSet.Tables[0].Rows[i]["jlsect"].ToString();
				row["X1"] = Convert.ToDecimal(attachPointsDataSet.Tables[0].Rows[i]["jlpt1x"].ToString());
				row["Y1"] = Convert.ToDecimal(attachPointsDataSet.Tables[0].Rows[i]["jlpt1y"].ToString());
				row["X2"] = Convert.ToDecimal(attachPointsDataSet.Tables[0].Rows[i]["jlpt2x"].ToString());
				row["Y2"] = Convert.ToDecimal(attachPointsDataSet.Tables[0].Rows[i]["jlpt2y"].ToString());

				AttachmentPointsDataTable.Rows.Add(row);
			}
		}

		private void LoadStartPointsDataTable()
		{
			string startSQL = String.Format("select jlsect,jlpt1x,jlpt1y from {0}.{1}line where jlrecord = {2} and jlline# = 1 order by jlsect",
									  MainForm.localLib,
									  MainForm.localPreFix,

									   //MainForm.FClib,
									   //MainForm.FCprefix,
									   _currentParcel.Record);

			DataSet startPoints = null;
			startPoints = dbConn.DBConnection.RunSelectStatement(startSQL);

			if (startPoints.Tables[0].Rows.Count > 0)
			{
				StrtPts.Clear();

				for (int i = 0; i < startPoints.Tables[0].Rows.Count; i++)
				{
					DataRow row = StrtPts.NewRow();
					row["Sect"] = startPoints.Tables[0].Rows[i]["jlsect"].ToString();
					row["Sx1"] = Convert.ToDecimal(startPoints.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Sy1"] = Convert.ToDecimal(startPoints.Tables[0].Rows[i]["jlpt1y"].ToString());

					StrtPts.Rows.Add(row);
				}
			}
		}

		private void SetNewSectionAttachmentPoint()
		{
			//TODO: Look at this stepping through
			string updateSQL = String.Format("update {0}.{1}line set JLATTACH = '{2}' where JLRECORD = {3} and JLDWELL = {4} and JLSECT = '{5}' and JLLINE# = {6}",
				MainForm.localLib,
				MainForm.localPreFix,
				_nextSectLtr.Trim(),
				_currentParcel.Record,
				_currentParcel.Card,
				CurrentSecLtr,
				_savedAttLine);

			dbConn.DBConnection.ExecuteNonSelectStatement(updateSQL);
		}

		private DataSet UpdateMasterArea(decimal summedArea)
		{
			string checkMaster = string.Format("select * from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
				MainForm.localLib,
				MainForm.localPreFix,
				_currentParcel.Record,
				_currentParcel.Card);

			DataSet ds_master = dbConn.DBConnection.RunSelectStatement(checkMaster.ToString());

			if (ds_master.Tables[0].Rows.Count > 0)
			{
				string updateMasterSql = string.Format("update {0}.{1}master set jmtotsqft = {2} where jmrecord = {3} and jmdwell = {4} ",
							   MainForm.localLib,
							   MainForm.localPreFix,
							   summedArea,
							   _currentParcel.Record,
							   _currentParcel.Card);

				dbConn.DBConnection.ExecuteNonSelectStatement(updateMasterSql.ToString());
			}

			return ds_master;
		}

		#endregion Refactored out from constructor

		#region User Actions Response Methods

		#region Key Press event handlers

		private void DistText_KeyDown(object sender, KeyEventArgs e)
		{
			_isKeyValid = false;
			bool IsArrowKey = (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down);
			if (!IsArrowKey)
			{
				HandleNonArrowKeys(e);
			}
			else
			{
				HandleDirectionalKeys(e);
			}
		}

		private void DistText_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (_isKeyValid == true)
			{
				e.Handled = true;
			}
		}

		private void DistText_KeyUp(object sender, KeyEventArgs e)
		{
			if (_isKeyValid == true)
			{
				e.Handled = true;
			}
		}

		private void DistText_Leave(object sender, EventArgs e)
		{
			if (_isAngle == true)
			{
				MeasureAngle();
			}
		}

		private void HandleDirectionalKeys(KeyEventArgs e)
		{
			string legalDirectionName = string.Empty;
			switch (legalMoveDirection)
			{
				case "E":
					legalDirectionName = "East";
					break;

				case "S":
					legalDirectionName = "South";
					break;

				case "W":
					legalDirectionName = "West";
					break;

				case "N":
					legalDirectionName = "North";
					break;

				default:
					legalDirectionName = "in a clockwise direction, relative to the anchoring section";
					break;
			}

			if (e.KeyCode == Keys.Right || e.KeyCode == Keys.R || e.KeyCode == Keys.E)
			{
				_isKeyValid = IsValidDirection("E");
				if (_isKeyValid)
				{
					HandleEastKeys();
				}
				else
				{
					string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
					MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					_undoJump = true;
					UndoLastAction();
				}
			}
			if (e.KeyCode == Keys.Left || e.KeyCode == Keys.L || e.KeyCode == Keys.W)
			{
				_isKeyValid = IsValidDirection("W");
				if (_isKeyValid)
				{
					HandleWestKeys();
				}
				else
				{
					string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
					MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					_undoJump = true;
					UndoLastAction();
				}
			}
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.U || e.KeyCode == Keys.N)
			{
				_isKeyValid = IsValidDirection("N");
				if (_isKeyValid)
				{
					HandleNorthKeys();
				}
				else
				{
					string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
					MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					_undoJump = true;
					UndoLastAction();
				}
			}
			if (e.KeyCode == Keys.Down || e.KeyCode == Keys.D || e.KeyCode == Keys.S)
			{
				_isKeyValid = IsValidDirection("S");
				if (_isKeyValid)
				{
					HandleSouthKeys();
				}
				else
				{
					NotifyUserOfLegalMove(legalDirectionName);
				}
			}
		}

		private void HandleNonArrowKeys(KeyEventArgs e)
		{
			bool notNumPad = (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9);
			if (notNumPad)
			{
				#region Not Numberpad

				if (e.KeyCode == Keys.Tab)
				{
					//Ask Dave what should go here, if anything.
				}

				if (e.KeyCode != Keys.Back)
				{
					_isKeyValid = true;
				}
				bool isNumberKey = (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9 || e.KeyCode == Keys.D0);
				bool isPunctuation = (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod);
				{
					if (isNumberKey || isPunctuation)
					{
						_isKeyValid = false;
					}
					if (e.KeyCode == Keys.Oemcomma)
					{
						_isKeyValid = false;
						_isAngle = true;
					}
					if (e.KeyCode == Keys.Delete)
					{
						unDo(savpic, click);
						_isKeyValid = false;
					}
				}

				#endregion Not Numberpad
			}
		}

		private void NotifyUserOfLegalMove(string legalDirectionName)
		{
			try
			{
				string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
				MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				_undoJump = true;
				UndoLastAction();
				DistText.Text = String.Empty;
				distance = 0;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		#endregion Key Press event handlers

		private void AddSectionBtn_Click(object sender, EventArgs e)
		{
			checkDirection = true;
			AddSections();
			AddNewPoint();
			_deleteMaster = false;

			BeginSectionBtn.BackColor = Color.Orange;
			BeginSectionBtn.Text = "Begin";

			_isClosed = false;
		}

		private void addSectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddSections();
		}

		private void angleToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void AutoCloseBtn_Click(object sender, EventArgs e)
		{
			AutoClose(savpic, click, NextStartX, NextStartY);

			Reorder();
		}

		private void beginPointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			draw = true;

			DMouseClick();
		}

		private void BeginSectionBtn_Click(object sender, EventArgs e)
		{
			BeginSectionBtn.BackColor = Color.Cyan;
			BeginSectionBtn.Text = "Active";
			checkDirection = false;
			float xxx = NextStartX;
			float yyy = NextStartY;

			float tsx = BeginSplitX;
			float tsy = BeginSplitY;

			float tstdist = distanceD;

			string tstdirect = _lastDir;

			if (_addSection == false)
			{
				MessageBox.Show("Must select additon type ", "Missing Addition warning");
			}
			if (_addSection == true)
			{
				Xadj = (((ScaleBaseX - _mouseX) / _currentScale) * -1);
				Yadj = (((ScaleBaseY - _mouseY) / _currentScale) * -1);

				offsetDir = _lastDir;

				//if (NextStartX != 0 || Xadj != 0)
				//{
				//    Xadj = NextStartX;
				//}

				if (Xadj != NextStartX)
				{
					Xadj = NextStartX;
				}

				//if (NextStartY != 0 || Yadj != 0)
				//{
				//    NextStartY = Yadj;
				//}

				if (Yadj != NextStartY)
				{
					Yadj = NextStartY;
				}

				if (offsetDir == "E")
				{
					begNewSecX = Math.Round(Convert.ToDecimal(Xadj), 1);
					begNewSecY = Math.Round(Convert.ToDecimal(Yadj), 1);
				}

				if (offsetDir == "W")
				{
					begNewSecX = Math.Round(Convert.ToDecimal(Xadj), 1);
					begNewSecY = Math.Round(Convert.ToDecimal(Yadj), 1);
				}

				if (offsetDir == "N")
				{
					begNewSecX = Math.Round(Convert.ToDecimal(Xadj), 1);
					begNewSecY = Math.Round(Convert.ToDecimal(Yadj), 1);
				}

				if (offsetDir == "S")
				{
					begNewSecX = Math.Round(Convert.ToDecimal(Xadj), 1);
					begNewSecY = Math.Round(Convert.ToDecimal(Yadj), 1);
				}
				if (offsetDir == String.Empty)
				{
					begNewSecX = Math.Round(Convert.ToDecimal(Xadj), 1);
					begNewSecY = Math.Round(Convert.ToDecimal(Yadj), 1);
				}

				if (_hasNewSketch == true)
				{
					Xadj = 0;
					Yadj = 0;
				}

				if (_hasNewSketch == true)
				{
					AttSectLtr = "A";
				}
				if (AttSectLtr == String.Empty)
				{
					AttSectLtr = JumpTable.Rows[0]["Sect"].ToString().Trim();
				}

				AttSpLineDir = offsetDir;

				splitLineDist = distance;

				startSplitX = begNewSecX;
				startSplitY = begNewSecY;

				getSplitLine();

				int _attLineNo = AttSpLineNo;

				draw = true;

				lineCnt = 0;
				DMouseClick();
			}
		}

		private void changeSectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SketchSection sysect = new SketchSection(_currentParcel, dbConn, _currentSection);
			sysect.ShowDialog(this);

			ExpSketchPBox.Image = _mainimage;
		}

		private void deleteExistingSketchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_deleteThisSketch = false;
			_deleteMaster = true;
			DialogResult result;
			result = (MessageBox.Show("Do you REALLY want to Delete this entire Sketch", "Delete Existing Sketch Warning",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
			if (result == DialogResult.Yes)
			{
				DialogResult finalChk;
				finalChk = (MessageBox.Show("Are you Sure", "Final Delete Sketch Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));

				if (finalChk == DialogResult.Yes)
				{
					StringBuilder delSect = new StringBuilder();
					delSect.Append(String.Format("delete from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
							  MainForm.localLib,
						  MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								_currentParcel.Record,
								_currentParcel.Card));

					dbConn.DBConnection.ExecuteNonSelectStatement(delSect.ToString());

					StringBuilder delLine = new StringBuilder();
					delLine.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
								   MainForm.localLib,
						  MainForm.localPreFix,

								   //MainForm.FClib,
								   //MainForm.FCprefix,
								   _currentParcel.Record,
								   _currentParcel.Card));

					dbConn.DBConnection.ExecuteNonSelectStatement(delLine.ToString());

					StringBuilder delmaster = new StringBuilder();
					delmaster.Append(String.Format("delete from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
							  MainForm.localLib,
						  MainForm.localPreFix,

							   //MainForm.FClib,
							   //MainForm.FCprefix,
							   _currentParcel.Record,
							   _currentParcel.Card));

					dbConn.DBConnection.ExecuteNonSelectStatement(delmaster.ToString());
				}
				if (finalChk == DialogResult.No)
				{
				}

				RefreshEditImageBtn = true;
				_deleteThisSketch = true;
				_isClosed = true;

				DialogResult makeVacant;
				makeVacant = (MessageBox.Show("Do you want to clear Master File", "Clear Master File Question",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question));

				if (makeVacant == DialogResult.Yes)
				{
					StringBuilder clrMast2 = new StringBuilder();
					clrMast2.Append(String.Format("update {0}.{1}mast set moccup = 15, mstory = ' ', mage = 0, mcond = ' ', mclass = ' ', ",
							   MainForm.localLib,
							   MainForm.localPreFix

								//MainForm.FClib,
								//MainForm.FCprefix
								));
					clrMast2.Append(" mfactr = 0, mdeprc = 0, mfound = 0, mexwll = 0, mrooft = 0, mroofg = 0, m#dunt = 0, m#room = 0, m#br = 0, m#fbth = 0, m#hbth = 0 , mswl = 0, ");
					clrMast2.Append(" mfp2 = ' ', mheat = 0, mfuel = 0, mac = ' ', mfp1 = ' ', mekit = 0, mbastp = 0, mpbtot = 0, msbtot = 0, mpbfin = 0, msbfin = 0, mbrate = 0, ");
					clrMast2.Append(" m#flue = 0, mflutp = ' ', mgart = 0, mgar#c = 0, mcarpt = 0, mcar#c = 0, mbi#c = 0, mgart2 = 0, mgar#2 = 0, macpct = 0, m0depr = ' ',meffag = 0, ");
					clrMast2.Append(" mfairv = 0, mexwl2 = 0, mtbv = 0, mtbas = 0, mtfbas = 0, mtplum = 0, mtheat = 0, mtac = 0, mtfp = 0, mtfl = 0 , mtbi = 0 , mttadd = 0 , mnbadj = 0, ");
					clrMast2.Append(" mtsubt = 0, mtotbv = 0, mbasa = 0, mtota = 0, mpsf = 0, minwll = ' ', mfloor = ' ', myrblt = 0, mpcomp = 0, mfuncd = 0, mecond = 0, mimadj = 0, ");
					clrMast2.Append(" mtbimp = 0, mcvexp = 'Improvement Deleted', mqapch = 0, mqafil = ' ', mfp# = 0, msfp# = 0, mfl#= 0, msfl# = 0, mmfl# = 0, miofp# = 0,mstor# = 0, ");
					clrMast2.Append(String.Format(" moldoc = {0}, ", _currentParcel.orig_moccup));
					clrMast2.Append(String.Format(" mcvmo = {0}, mcvda = {1}, mcvyr = {2} ",
							   MainForm.Month,
							   MainForm.Today,
							   MainForm.Year

								//MainForm.Month,
								// MainForm.Today,
								// MainForm.Year
								));
					clrMast2.Append(String.Format(" where mrecno = {0} and mdwell = {1} ", _currentParcel.Record, _currentParcel.Card));

					dbConn.DBConnection.ExecuteNonSelectStatement(clrMast2.ToString());

					if (_currentParcel.GasLogFP > 0)
					{
						StringBuilder clrGasLg = new StringBuilder();
						clrGasLg.Append(String.Format("update {0}.{1}gaslg set gnogas = 0 where grecno = {2} and gdwell = {3} ",
						   MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card));

						dbConn.DBConnection.ExecuteNonSelectStatement(clrGasLg.ToString());
					}
				}
				if (makeVacant == DialogResult.No)
				{
				}
			}
			if (result == DialogResult.No)
			{
			}
		}

		private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DeleteSection();

			RefreshSketch();
		}

		private void DMouseClick()
		{
			StartX = _mouseX;
			StartY = _mouseY;
			BaseX = _mouseX;
			BaseY = _mouseY;
			PrevX = _mouseX;
			PrevY = _mouseY;
			EndX = 0;
			EndY = 0;
			txtLoc = 0;
			txtX = 0;
			txtY = 0;
			_lenString = String.Empty;
			_lastDir = String.Empty;

			int tclick = click;

			try
			{
				_StartX.Add(click, StartX);

				_StartY.Add(click, StartY);
			}
			catch
			{
			}

			if (_undoMode == true)
			{
				string reopestr = MainForm.reopenSec;

				unDo(savpic, click);
			}
			if (_undoMode == false)
			{
				DistText.Focus();
			}
		}

		private void DMouseMove(int X, int Y, bool jumpMode)
		{
			if (jumpMode == false)
			{
				_isJumpMode = false;
				draw = true;
				Graphics g = Graphics.FromImage(_mainimage);
				Pen pen1 = new Pen(Color.White, 4);
				g.DrawRectangle(pen1, X, Y, 1, 1);
				g.Save();

				ExpSketchPBox.Image = _mainimage;
				click++;
				savpic.Add(click, imageToByteArray(_mainimage));
			}
			if (jumpMode == true)
			{
				_isJumpMode = true;
				draw = true;
				_mouseX = X;
				_mouseY = Y;
			}
		}

		public void DrawSketch(int selectedPoint)
		{
		}

		private void EastDirBtn_Click(object sender, EventArgs e)
		{
			_isKeyValid = true;
			MoveEast(NextStartX, NextStartY);
			DistText.Focus();
		}

		private void endSectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ExpandoSketch_FormClosing(object sender, FormClosingEventArgs e)
		{
			ClrX();
			AddMaster();
		}

		private void exportSketchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WriteToTextFile(",", "Secfile.txt");
		}

		private void ExpSketchPBox_Click(object sender, EventArgs e)
		{
		}

		private void ExpSketchPbox_MouseClick(object sender, MouseEventArgs e)
		{
			if (!_isJumpMode)
			{
				_mouseX = e.X;
				_mouseY = e.Y;

				Graphics g = Graphics.FromImage(_mainimage);
				Pen pen1 = new Pen(Color.Red, 4);
				g.DrawRectangle(pen1, e.X, e.Y, 1, 1);
				g.Save();

				DMouseClick();
			}
		}

		private void ExpSketchPbox_MouseDown(object sender, MouseEventArgs e)
		{
			_isJumpMode = false;
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				_mouseX = e.X;
				_mouseY = e.Y;

				DMouseMove(e.X, e.Y, false);
			}
			else if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				_isJumpMode = true;
				_mouseX = e.X;
				_mouseY = e.Y;
				DMouseMove(e.X, e.Y, true);
			}
		}

		private void ExpSketchPbox_MouseMove(object sender, MouseEventArgs e)
		{
			if (draw && !_isJumpMode)
			{
				Graphics g = Graphics.FromImage(_mainimage);
				SolidBrush brush = new SolidBrush(Color.White);
				g.FillRectangle(brush, e.X, e.Y, s, s);
				g.Save();

				ExpSketchPBox.Image = _mainimage;
			}
		}

		private void ExpSketchPbox_MouseUp(object sender, MouseEventArgs e)
		{
			draw = false;
		}

		private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BeginSectionBtn.BackColor = Color.Orange;
			BeginSectionBtn.Text = "Begin";

			if (_isJumpMode)
			{
				int jx = _mouseX;
				int jy = _mouseY;

				float _scaleBaseX = ScaleBaseX;
				float _scaleBaseY = ScaleBaseY;
				float CurrentScale = _currentScale;

				draw = false;
				_isNewSketch = false;

				JumptoCorner();

				_undoJump = false;
			}
			_isJumpMode = false;
		}

		private void NorthDirBtn_Click(object sender, EventArgs e)
		{
			_isKeyValid = true;
			MoveNorth(NextStartX, NextStartY);
			DistText.Focus();
		}

		private void rotateSketchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//DialogResult result;
			//result = (MessageBox.Show("Do you want to Flip Sketch Left to Right", "Flip Sketch Warning",
			//    MessageBoxButtons.YesNo, MessageBoxIcon.Question));
			//if (result == DialogResult.Yes)
			//{
			//    FlipLeftRight();
			//}
			//if (result == DialogResult.No)
			//{
			//    DialogResult result2;
			//    result2 = (MessageBox.Show("Do you want to Flip Sketch Front to Back", "Flip Sketch Warning",
			//        MessageBoxButtons.YesNo, MessageBoxIcon.Question));
			//    if (result2 == DialogResult.Yes)
			//    {
			//        FlipUpDown();
			//    }

			//    if (result2 == DialogResult.No)
			//    {
			//    }
			//}

			FlipSketch fskt = new FlipSketch();
			fskt.ShowDialog();
			if (FlipSketch.FrontBack == true)
			{
				FlipUpDown();
			}
			if (FlipSketch.RightLeft == true)
			{
				FlipLeftRight();
			}
		}

		private void SouthDirBtn_Click(object sender, EventArgs e)
		{
			_isKeyValid = true;
			MoveSouth(NextStartX, NextStartY);
			DistText.Focus();
		}

		private void TextBtn_Click(object sender, EventArgs e)
		{
			if (FieldText.Text.Trim() != String.Empty)
			{
				Graphics g = Graphics.FromImage(_mainimage);
				SolidBrush brush = new SolidBrush(Color.Blue);
				Pen pen1 = new Pen(Color.Red, 2);
				Font f = new Font("Arial", 8, FontStyle.Bold);

				g.DrawString(FieldText.Text.Trim(), f, brush, new PointF(_mouseX + 5, _mouseY));

				FieldText.Text = String.Empty;
				FieldText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				savpic.Add(click, imageToByteArray(_mainimage));
			}
		}

		private void TextBtn_Click_1(object sender, EventArgs e)
		{
			if (FieldText.Text.Trim() != String.Empty)
			{
				Graphics g = Graphics.FromImage(_mainimage);
				SolidBrush brush = new SolidBrush(Color.Blue);
				Pen pen1 = new Pen(Color.Red, 2);
				Font f = new Font("Arial", 8, FontStyle.Bold);

				g.DrawString(FieldText.Text.Trim(), f, brush, new PointF(_mouseX + 5, _mouseY));

				FieldText.Text = String.Empty;
				FieldText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				savpic.Add(click, imageToByteArray(_mainimage));
			}
		}

		private void UnDoBtn_Click(object sender, EventArgs e)
		{
			UndoLastAction();
		}

		private void viewSectionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SketchSection sksect = new SketchSection(_currentParcel, dbConn, _currentSection);
			sksect.ShowDialog(this);

			_reOpenSec = false;

			if (MainForm.reopenSec != String.Empty)
			{
				_reOpenSec = true;

				ReOpenSec();
			}
		}

		private void WestDirBtn_Click(object sender, EventArgs e)
		{
			_isKeyValid = true;
			MoveWest(NextStartX, NextStartY);
			DistText.Focus();
		}

		#endregion User Actions Response Methods

		public ExpandoSketch(ParcelData currentParcel, string sketchFolder,
			string sketchRecord, string sketchCard, string _locality, SWallTech.CAMRA_Connection _fox,
			SectionDataCollection currentSection, bool hasSketch, Image sketchImage, bool hasNewSketch)
		{
			InitializeComponent();
			checkDirection = true;
			_currentParcel = currentParcel;
			_currentSection = currentSection;

			dbConn = _fox;

			_currentSection = new SectionDataCollection(_fox, _currentParcel.Record, _currentParcel.Card);

			Locality = _locality;

			_isNewSketch = false;
			_hasNewSketch = hasNewSketch;
			_isNewSketch = hasNewSketch;
			_addSection = false;
			click = 0;
			SketchFolder = sketchFolder;
			SketchRecord = sketchRecord;
			SketchCard = sketchCard;
			_hasSketch = hasSketch;

			savpic = new Dictionary<int, byte[]>();
			_StartX = new Dictionary<int, float>();
			_StartY = new Dictionary<int, float>();

			SectionTable = ConstructSectionTable();

			ConstructJumpTable();

			REJumpTable = ConstructREJumpTable();

			RESpJumpTable = ConstructRESpJumpTable();
			SectionLtrs = ConstructSectionLtrs();

			AreaTable = ConstructAreaTable();

			MulPts = ConstructMulPtsTable();

			undoPoints = ConstructUndoPointsTable();
			sortDist = ConstructSortDistanceTable();

			AttachmentPointsDataTable = ConstructAttachmentPointsDataTable();

			AttachPoints = ConstructAttachPointsDataTable();

			DupAttPoints = ConstructDupAttPointsTable();

			StrtPts = new DataTable();
			StrtPts.Columns.Add("Sect", typeof(string));
			StrtPts.Columns.Add("Sx1", typeof(decimal));
			StrtPts.Columns.Add("Sy1", typeof(decimal));

			dt = new DataTable();

			DataColumn col_sect = new DataColumn("Dir", Type.GetType("System.String"));
			dt.Columns.Add(col_sect);
			DataColumn col_desc = new DataColumn("North", Type.GetType("System.Decimal"));
			dt.Columns.Add(col_desc);
			DataColumn col_sqft = new DataColumn("East", Type.GetType("System.Decimal"));
			dt.Columns.Add(col_sqft);
			DataColumn col_att = new DataColumn("Att", Type.GetType("System.String"));
			dt.Columns.Add(col_att);

			DataGridTableStyle style = new DataGridTableStyle();
			DataGridTextBoxColumn SectColumn = new DataGridTextBoxColumn();
			SectColumn.MappingName = "Dir";
			SectColumn.HeaderText = "Dir";
			SectColumn.Width = 30;
			style.GridColumnStyles.Add(SectColumn);

			DataGridTextBoxColumn DescColumn = new DataGridTextBoxColumn();
			DescColumn.MappingName = "North";
			DescColumn.HeaderText = "North";
			DescColumn.Width = 50;
			style.GridColumnStyles.Add(DescColumn);

			DataGridTextBoxColumn SqftColumn = new DataGridTextBoxColumn();
			SqftColumn.MappingName = "East";
			SqftColumn.HeaderText = "East";
			SqftColumn.Width = 50;
			style.GridColumnStyles.Add(SqftColumn);

			DataGridTextBoxColumn AttColumn = new DataGridTextBoxColumn();
			AttColumn.MappingName = "Att";
			AttColumn.HeaderText = "Att";
			AttColumn.Width = 30;
			style.GridColumnStyles.Add(AttColumn);

			this.dgSections.DataSource = this.dt;

			float tstScale = _scale;

			if (hasSketch == false)
			{
				MainForm._hasSketch = true;
			}

			if (hasSketch == true)
			{
				_baseImage = currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height, 1000, 572, 400, out _scale);
				_currentScale = _scale;
				_mainimage = sketchImage;
				_mainimage = _baseImage;
			}
			if (hasSketch == false)
			{
				_mainimage = currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height, 1000, 572, 400, out _scale);
				_currentScale = _scale;
			}
			else
			{
				_currentScale = _scale;
				_isNewSketch = true;
			}

			ScaleBaseX = BuildingSketcher.basePtX;
			ScaleBaseY = BuildingSketcher.basePtY;

			if (_mainimage == null)
			{
				_mainimage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
				_vacantParcelSketch = true;
				_isNewSketch = true;
			}

			Graphics g = Graphics.FromImage(_mainimage);
			SolidBrush Lblbrush = new SolidBrush(Color.Black);
			SolidBrush FillBrush = new SolidBrush(Color.White);
			Pen whitePen = new Pen(Color.White, 2);
			Pen blackPen = new Pen(Color.Black, 2);

			Font LbLf = new Font("Arial", 10, FontStyle.Bold);
			Font TitleF = new Font("Arial", 10, FontStyle.Bold | FontStyle.Underline);
			Font MainTitle = new System.Drawing.Font("Arial", 15, FontStyle.Bold | FontStyle.Underline);
			char[] leadzero = new char[] { '0' };

			if (_vacantParcelSketch == true)
			{
				g.DrawRectangle(whitePen, 0, 0, 1000, 572);
				g.FillRectangle(FillBrush, 0, 0, 1000, 572);
				_currentScale = Convert.ToSingle(7.2);
			}

			g.DrawString(_locality, TitleF, Lblbrush, new PointF(10, 10));
			g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
			g.DrawString(String.Format("Record # - {0}", sketchRecord.TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
			g.DrawString(String.Format("Card # - {0}", sketchCard), LbLf, Lblbrush, new PointF(10, 45));

			g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));

			ExpSketchPBox.Image = _mainimage;
			if (click == 0)
			{
				click = -1;
			}

			click++;
			savpic.Add(click, imageToByteArray(_mainimage));
		}

		private void AddXLine(string thisSection)
		{
			StringBuilder addXLine = new StringBuilder();
			addXLine.Append(String.Format("insert into {0}.{1}line ", MainForm.localLib, MainForm.localPreFix));
			addXLine.Append(String.Format("values ( {0},{1},'{2}',{3},'X',0,0,0,0,0,0,0,0,' ') ", _currentParcel.Record, _currentParcel.Card, thisSection, (SecLineCnt + 1)));
			dbConn.DBConnection.ExecuteNonSelectStatement(addXLine.ToString());
		}

		private void AdjustLine(decimal newEndX, decimal newEndY, decimal newDistX, decimal newDistY, decimal EndEndX, decimal EndEndY, decimal finDist)
		{
			StringBuilder adjLine = new StringBuilder();
			adjLine.Append(String.Format("update {0}.{1}line set jldirect = '{2}',jlxlen = {3},jlylen = {4},jllinelen = {5}, ",
						   MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							CurrentAttDir,
							newDistX,
							newDistY,
							finDist));
			adjLine.Append(String.Format("jlpt1x = {0},jlpt1y = {1},jlpt2x = {2},jlpt2y = {3} ",
					newEndX, newEndY, EndEndX, EndEndY));
			adjLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
				_currentParcel.mrecno, _currentParcel.mdwell, _savedAttSection, (mylineNo + 1)));

			dbConn.DBConnection.ExecuteNonSelectStatement(adjLine.ToString());
		}

		private void AutoClose(Dictionary<int, byte[]> curpic, int curclick, float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			////UtilityMethods.LogMethodCall(fullStack, true);
#endif
			Cursor = Cursors.WaitCursor;

			savcnt = new List<int>();
			savpic = curpic;

			_isClosed = true;

			string stx = _nextSectLtr;

			click = curclick;

			float tx1 = NextStartX;

			float ty1 = NextStartY;

			ExpSketchPBox.Image = _mainimage;
			click++;
			savpic.Add(click, imageToByteArray(_mainimage));

			foreach (KeyValuePair<int, byte[]> pair in savpic)
			{
				savcnt.Add(pair.Key);
			}

			finalClick = click;

			_isclosing = true;

			string delXline = string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X' ", MainForm.localLib, MainForm.localPreFix, _currentParcel.Record, _currentParcel.Card);

			dbConn.DBConnection.ExecuteNonSelectStatement(delXline);

			Graphics g = Graphics.FromImage(_mainimage);
			SolidBrush brush = new SolidBrush(Color.Red);
			Pen pen1 = new Pen(Color.Red, 2);
			Pen pen2 = new Pen(Color.Green, 3);
			Font f = new Font("Arial", 8, FontStyle.Bold);

			string finalDirect = String.Empty;

			decimal finalDistance = 0;
			float finalDistanceF = 0;

			float calcDistXf = 0;
			float calcDistYf = 0;

			decimal calcDistX = 0;
			decimal calcDistY = 0;

			if (_addSection == true)
			{
#if DEBUG
				StringBuilder traceOut = new StringBuilder();
				int traceCounter = 1;
				traceOut.AppendLine(string.Format("*** {0}", "New Section Points"));
				foreach (PointF p in NewSectionPoints)
				{
					traceOut.AppendLine(string.Format("{0} - {1},{2}", traceCounter.ToString(), p.X, p.Y));
				}
				Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
				var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
				var sectionArea = sectionPolygon.Area;

				if (_nextStoryHeight < 1.0m)
				{
					_nextStoryHeight = 1;
				}
				if (_nextStoryHeight >= 1.0m)
				{
					_nextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
				}
			}

			StringBuilder garcp = new StringBuilder();
			garcp.Append(String.Format("select rsecto from {0}.{1}rat1 where rid = 'P' and rdesc like '%GAR%' and rrpsf <> 0 ",
							   MainForm.localLib,
						  MainForm.localPreFix

								//MainForm.FClib,
								//MainForm.FCprefix
								));

			DataSet ds = dbConn.DBConnection.RunSelectStatement(garcp.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				GarTypes = new List<string>();
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					string sect = ds.Tables[0].Rows[i]["rsecto"].ToString().Trim();

					GarTypes.Add(sect);
				}
			}

			StringBuilder cptype = new StringBuilder();
			cptype.Append(String.Format("select rsecto from {0}.{1}rat1 where rid = 'P' and rdesc like '%CAR%' and rrpsf <> 0 ",
					  MainForm.localLib,
						  MainForm.localPreFix

						//MainForm.FClib,
						//MainForm.FCprefix
						));

			DataSet ds1 = dbConn.DBConnection.RunSelectStatement(cptype.ToString());

			if (ds1.Tables[0].Rows.Count > 0)
			{
				CPTypes = new List<string>();
				for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
				{
					string sect = ds1.Tables[0].Rows[i]["rsecto"].ToString().Trim();

					CPTypes.Add(sect);
				}
			}

			StringBuilder garcode = new StringBuilder();
			garcode.Append(String.Format("select ttelem from {0}.{1}stab where ttid = 'GAR' and tdesc not like '%NONE%' and tdesc not like '%DETACHED%' ",
							   MainForm.localLib,
						  MainForm.localPreFix

								//MainForm.FClib,
								//MainForm.FCprefix
								));

			DataSet gc = dbConn.DBConnection.RunSelectStatement(garcode.ToString());

			if (gc.Tables[0].Rows.Count > 0)
			{
				GarCodes = new List<int>();
				for (int i = 0; i < gc.Tables[0].Rows.Count; i++)
				{
					int gcode = Convert.ToInt32(gc.Tables[0].Rows[i]["ttelem"].ToString());

					GarCodes.Add(gcode);
				}
			}

			StringBuilder cpcode = new StringBuilder();
			cpcode.Append(String.Format("select ttelem from {0}.{1}stab where ttid = 'CAR' and tdesc not like '%NONE%' and tdesc not like '%DETACHED%' ",
							  MainForm.localLib,
						  MainForm.localPreFix

								//MainForm.FClib,
								//MainForm.FCprefix
								));

			DataSet cp = dbConn.DBConnection.RunSelectStatement(cpcode.ToString());

			if (cp.Tables[0].Rows.Count > 0)
			{
				CPCodes = new List<int>();
				for (int i = 0; i < cp.Tables[0].Rows.Count; i++)
				{
					int cpcodeX = Convert.ToInt32(cp.Tables[0].Rows[i]["ttelem"].ToString());

					CPCodes.Add(cpcodeX);
				}
			}

			string newSect = _nextSectType.Trim();

			if (GarTypes.Contains(_nextSectType.Trim()) && !GarCodes.Contains(_currentParcel.mgart))
			{
				MessageBox.Show("Current Record does not include Garages ");

				MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, _nextSectArea, "GAR");
				missGar.ShowDialog();

				if (_currentParcel.mgart != _currentParcel.orig_mgart)
				{
					StringBuilder fixCp = new StringBuilder();
					fixCp.Append(String.Format("update {0}.{1}mast set mgart = {2},mgar#c = {3} ",
					  MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.mgart,
						_currentParcel.mgarNc));
					fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

					dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

					ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
				}
			}

			if (CPTypes.Contains(_nextSectType.Trim()) && !CPCodes.Contains(_currentParcel.mcarpt))
			{
				MessageBox.Show("Current Record does not include CarPorts ");

				MissingGarageData missCP = new MissingGarageData(dbConn, _currentParcel, _nextSectArea, "CP");
				missCP.ShowDialog();

				if (_currentParcel.mcarpt != _currentParcel.orig_mcarpt)
				{
					StringBuilder fixCp = new StringBuilder();
					fixCp.Append(String.Format("update {0}.{1}mast set mcarpt = {2},mcar#c = {3} ",
					   MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.mcarpt,
						_currentParcel.mcarNc));
					fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

					dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

					if (_currentParcel.mcarpt == 66)
					{
						_nextSectType = "WCP";
					}

					ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
				}
			}

			calcDistXf = ((BaseX - StartX) / _currentScale);

			calcDistYf = ((BaseY - StartY) / _currentScale);

			calcDistX = Convert.ToDecimal((((BaseX - StartX) / _currentScale)));

			calcDistY = Convert.ToDecimal((((BaseY - StartY) / _currentScale)));

			float tstclacDistfx = SecBeginX - startx;

			float tstcalcDistfy = SecBeginY - starty;

			if (startx > SecBeginX)
			{
				finalDirect = "W";
				finalDistance = Math.Abs(Convert.ToDecimal(SecBeginX - startx));
				finalDistanceF = (float)finalDistance;
				Xadj = NextStartX - finalDistanceF;
				Yadj = NextStartY;
			}
			if (startx < SecBeginX)
			{
				finalDirect = "E";
				finalDistance = Math.Abs(Convert.ToDecimal(SecBeginX - startx));
				finalDistanceF = (float)finalDistance;
				Xadj = NextStartX + finalDistanceF;
				Yadj = NextStartY;
			}
			if (starty < SecBeginY)
			{
				finalDirect = "S";
				finalDistance = Math.Abs(Convert.ToDecimal(SecBeginY - starty));
				finalDistanceF = (float)finalDistance;
				Yadj = NextStartY + finalDistanceF;
				Xadj = NextStartX;
			}
			if (starty > SecBeginY)
			{
				finalDirect = "N";
				finalDistance = Math.Abs(Convert.ToDecimal(SecBeginY - starty));
				finalDistanceF = (float)finalDistance;
				Yadj = NextStartY - finalDistanceF;
				Xadj = NextStartX;
			}

			_lenString = String.Format("{0} ft.", finalDistance.ToString("N1"));

			txtLoc = ((finalDistance * Convert.ToDecimal(_currentScale)) / 2);
			txtLocf = ((((finalDistanceF) * _currentScale) / 2));

			g.DrawLine(pen1, StartX, StartY, BaseX, BaseY);

			if (finalDirect == "N")
			{
				g.DrawString(_lenString, f, brush, new PointF((StartX + 15), (StartY - txtLocf)));
			}
			if (finalDirect == "S")
			{
				g.DrawString(_lenString, f, brush, new PointF((StartX + 15), (StartY + txtLocf)));
			}
			if (finalDirect == "E")
			{
				g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
			}
			if (finalDirect == "W")
			{
				g.DrawString(_lenString, f, brush, new PointF((StartX - txtLocf), (StartY - 15)));
			}

			float px = startx;

			float py = starty;

			float px1 = PrevStartX;

			float py1 = PrevStartY;

			lineCnt++;
			BuildAddSQL(StartX, StartY, finalDistanceF, finalDirect, lineCnt, _isclosing, Xadj, Yadj, NextStartX, NextStartY);

			_addSection = false;

			computeArea();

			AddSectionSQL(finalDirect, finalDistanceF);

			string finalDesc = String.Format("{0}, {1} sf",
				FieldText.Text.Trim(),
				_nextSectArea.ToString("N1"));

			FieldText.Text = finalDesc.Trim();

			ExpSketchPBox.Image = _mainimage;

			sortSection();

			setAttPnts();

			Cursor = Cursors.Default;

			this.Close();
		}

		private string BeginLineDirection(float nextStartX, float nextStartY)
		{
			string dir = string.Empty;
			for (int i = 0; i < JumpTable.Rows.Count - 1; i++)
			{
				DataRow dr = JumpTable.Rows[i];
				float x = 0.00f;
				float y = 0.00f;
				string sectionLetter = dr["sect"].ToString();
				string direction = dr["direct"].ToString();
				float.TryParse(dr["XPt2"].ToString(), out x);
				float.TryParse(dr["YPt2"].ToString(), out y);
				if (x == nextStartX && y == nextStartY)
				{
					dir = direction;
				}
			}

			return dir;
		}

		private void BuildAddSQL(float prevX, float prevY, float distD, string direction, int _lineCnt, bool closing, float startx, float starty, float prevstartx, float prevstarty)
		{
			_isclosing = closing;

			pt2X = 0;
			pt2Y = 0;

			float nx1 = NextStartX;

			float ny1 = NextStartY;

			float nx2 = Xadj;

			float ny2 = Yadj;

			float nx2P = XadjP;

			float ny2P = YadjP;

			decimal dste = Convert.ToDecimal(distD);

			Xadj = (((ScaleBaseX - prevX) / _currentScale) * -1);
			Yadj = (((ScaleBaseY - prevY) / _currentScale) * -1);

			NewSectionPoints.Add(new PointF(Xadj, Yadj));

			if (NextStartX != Xadj)
			{
				Xadj = NextStartX;
			}
			if (NextStartY != Yadj)
			{
				Yadj = NextStartY;
			}

			float lengthX = 0;
			float lengthY = 0;
			if (direction == "E")
			{
				if (_isclosing == true)
				{
					NextStartX = startx;
				}

				Xadj = NextStartX - distD;

				//Xadj = NextStartX;

				lengthX = distD;
				lengthY = 0;

				pt2X = Xadj + distD;
				pt2Y = Yadj;
			}

			if (direction == "W")
			{
				if (_isclosing == true)
				{
					NextStartX = startx;
				}

				Xadj = NextStartX + distD;

				//Xadj = NextStartX;

				lengthX = distD;
				lengthY = 0;

				pt2X = Xadj - distD;
				pt2Y = Yadj;
			}
			if (direction == "N")
			{
				if (_isclosing == true)
				{
					NextStartY = starty;
				}

				Yadj = NextStartY + distD;

				//Yadj = NextStartY;

				lengthX = 0;
				lengthY = distD;

				pt2X = Xadj;
				pt2Y = Yadj - distD;
			}
			if (direction == "S")
			{
				if (_isclosing == true)
				{
					NextStartY = starty;
				}

				Yadj = NextStartY - distD;

				//Yadj = NextStartY;

				lengthX = 0;
				lengthY = distD;

				pt2X = Xadj;
				pt2Y = Yadj + distD;

				//pt2Y = Yadj;
			}

			if (draw)
			{
				if (_lineCnt == 1)
				{
					if (_hasNewSketch == true)
					{
						Xadj = 0;
						Yadj = 0;
					}

					SecBeginX = Xadj;
					SecBeginY = Yadj;
				}

				decimal Tst1 = Convert.ToDecimal(Xadj);
				decimal Tst2 = Convert.ToDecimal(Yadj);
				decimal Ptx = Convert.ToDecimal(pt2X);
				decimal Pty = Convert.ToDecimal(pt2Y);

				decimal ptyT = Math.Round(Pty, 1);

				var rndTst1 = Math.Round(Tst1, 1);
				var rndTst2 = Math.Round(Tst2, 1);
				var rndPt2X = Math.Round(Ptx, 1);
				var rndPt2Y = Math.Round(Pty, 1);

				if (_hasNewSketch == true && _lineCnt == 1)
				{
					switch (direction)
					{
						case "N":

							rndTst1 = 0;
							rndTst2 = 0;
							rndPt2X = rndTst1;

							rndPt2Y = rndTst2 - (Convert.ToDecimal(lengthY));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						case "S":

							rndTst1 = 0;
							rndTst2 = 0;
							rndPt2X = rndTst1;

							rndPt2Y = rndTst2 + (Convert.ToDecimal(lengthY));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						case "E":
							rndTst1 = 0;
							rndTst2 = 0;
							rndPt2Y = rndTst2;

							rndPt2X = rndTst1 + (Convert.ToDecimal(lengthX));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						case "W":
							rndTst1 = 0;
							rndTst2 = 0;
							rndPt2Y = rndTst2;

							rndPt2X = rndTst1 - (Convert.ToDecimal(lengthX));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						default:
							throw new NotImplementedException(string.Format("Invalid line direction: '{0}", direction));
					}

					//decimal TprevTst1 = prevTst1;
					//decimal TprevTst2 = prevTst2;
					//decimal TprevPt2X = prevPt2X;
					//decimal TprevPt2Y = prevPt2Y;
				}

				if (_hasNewSketch == true && _lineCnt > 1)
				{
					//decimal TprevTst1 = prevTst1;
					//decimal TprevTst2 = prevTst2;
					//decimal TprevPt2X = prevPt2X;
					//decimal TprevPt2Y = prevPt2Y;
					switch (direction)
					{
						case "N":
							rndPt2Y = rndTst2 - (Convert.ToDecimal(lengthY));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						case "S":
							rndPt2Y = rndTst2 + (Convert.ToDecimal(lengthY));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;

							break;

						case "E":
							rndPt2X = rndTst1 + (Convert.ToDecimal(lengthX));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						case "W":
							rndPt2X = rndTst1 - (Convert.ToDecimal(lengthX));

							prevPt2X = rndPt2X;
							prevPt2Y = rndPt2Y;
							prevTst1 = rndPt2X;
							prevTst2 = rndPt2Y;
							break;

						default:

							throw new NotImplementedException(string.Format("Invalid line direction: '{0}", direction));
					}
				}

				if (_isclosing == true)
				{
					var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
					var sectionArea = sectionPolygon.Area;

					if (_nextStoryHeight < 1.0m)
					{
						_nextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
					}
					if (_nextStoryHeight >= 1.0m)
					{
						_nextSectArea = Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1);
					}
				}

				StringBuilder mxline = new StringBuilder();
				mxline.Append(String.Format("select max(jlline#) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
							  MainForm.localLib,
							  MainForm.localPreFix,
							   _currentParcel.Record,
							   _currentParcel.Card,
								   _nextSectLtr));

				try
				{
					_curLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(mxline.ToString()));
				}
				catch
				{
				}

				if (_curLineCnt == 0)
				{
					_curLineCnt = 0;
				}

				_lineCnt = _curLineCnt;

				lineCnt = _curLineCnt + 1;

				if (lineCnt == 19)
				{
					MessageBox.Show("Next Line will Max Section Lines", "Line Count Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				if (lineCnt > 20)
				{
					MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}

				//MessageBox.Show(String.Format("Insert into Line Record - {0}, Card - {1} at 2695", _currentParcel.Record, _currentParcel.Card));

				decimal t1 = rndTst1;
				decimal t2 = rndTst2;

				decimal tX1 = rndPt2X;
				decimal tY1 = rndPt2Y;

				if (lineCnt <= 20)
				{
					StringBuilder addSect = new StringBuilder();
					addSect.Append(String.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen, jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach) ",
						  MainForm.localLib,
						  MainForm.localPreFix));
					addSect.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
						_currentParcel.Record, //0
						_currentParcel.Card, // 1
						_nextSectLtr.Trim(), // 2
						lineCnt, //3
						direction.Trim(), //4
						lengthX, //5
						lengthY, //6
						distD, //7
						0, //8
						rndTst1,  // 9 jlpt1x  tst1
						rndTst2,  // 10 jlpt1y  tst2
						rndPt2X,  // 11 jlpt2x tst1x
						rndPt2Y,  // 12 jlpt2y tst2y
						" " //13
						));
#if DEBUG
					StringBuilder traceOut = new StringBuilder();
					traceOut.AppendLine(string.Format("Section Adding SQL: {0}", addSect.ToString()));
					Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
					NextStartX = (float)rndPt2X;
					NextStartY = (float)rndPt2Y;

					if (_undoLine == false)
					{
						if (Math.Abs(lengthX) > 0 || Math.Abs(lengthY) > 0)
						{
							try
							{
								dbConn.DBConnection.ExecuteNonSelectStatement(addSect.ToString());
							}
							catch (Exception ex)
							{
								string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
								Trace.WriteLine(errMessage);
								Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

								MessageBox.Show(errMessage);
#endif
							}
						}
					}
				}

				if (_undoLine == true)
				{
					_undoLine = false;
				}
			}
		}

		private void BuildAddSQLAng(float prevX, float prevY, decimal distDX, decimal distDY, string direction, decimal length, int _lineCnt, bool closing, float startx, float starty)
		{
			_isclosing = closing;
			_lastAngDir = direction;

			pt2X = 0;
			pt2Y = 0;

			Xadj = (((ScaleBaseX - prevX) / _currentScale) * -1);
			Yadj = (((ScaleBaseY - prevY) / _currentScale) * -1);

			NewSectionPoints.Add(new PointF(Xadj, Yadj));

			decimal xw2 = Convert.ToDecimal(Xadj);

			decimal yw2 = Convert.ToDecimal(Yadj);

			decimal XadjD = Math.Round((Convert.ToDecimal(Xadj)), 1);

			decimal YadjD = Math.Round((Convert.ToDecimal(Yadj)), 1);

			Xadj = (float)XadjD;
			Yadj = (float)YadjD;

			if (NextStartX != Xadj)
			{
				Xadj = NextStartX;
			}
			if (NextStartY != Yadj)
			{
				Yadj = NextStartY;
			}

			decimal lengthX = 0;
			decimal lengthY = 0;
			if (direction == "NE")
			{
				lengthX = distDX;
				lengthY = distDY;

				decimal pt2XD = Math.Round((Convert.ToDecimal(Xadj) + distDX), 1);

				decimal pt2YD = Math.Round((Convert.ToDecimal(Yadj) - distDY), 1);

				pt2X = Xadj + Convert.ToInt32(distDX);
				pt2Y = Yadj - Convert.ToInt32(distDY);

				pt2X = (float)pt2XD;
				pt2Y = (float)pt2YD;

				NextStartX = pt2X;
				NextStartY = pt2Y;
			}
			if (direction == "NW")
			{
				lengthX = distDX;
				lengthY = distDY;

				decimal pt2XD = Convert.ToDecimal(Xadj) - distDX;

				decimal pt2YD = Convert.ToDecimal(Yadj) - distDY;

				pt2X = Xadj - Convert.ToInt16(distDX);
				pt2Y = Yadj - Convert.ToInt16(distDY);

				pt2X = (float)pt2XD;
				pt2Y = (float)pt2YD;

				NextStartX = pt2X;
				NextStartY = pt2Y;
			}
			if (direction == "SE")
			{
				lengthX = distDX;
				lengthY = distDY;

				decimal pt2XD = Convert.ToDecimal(Xadj) + distDX;

				decimal pt2YD = Convert.ToDecimal(Yadj) + distDY;

				pt2X = Xadj + Convert.ToInt16(distDX);
				pt2Y = Yadj + Convert.ToInt16(distDY);

				pt2X = (float)pt2XD;
				pt2Y = (float)pt2YD;

				NextStartX = pt2X;
				NextStartY = pt2Y;
			}
			if (direction == "SW")
			{
				lengthX = distDX;
				lengthY = distDY;

				decimal pt2XD = Convert.ToDecimal(Xadj) - distDX;

				decimal pt2YD = Convert.ToDecimal(Yadj) + distDY;

				pt2X = Xadj - Convert.ToInt16(distDX);
				pt2Y = Yadj + Convert.ToInt16(distDY);

				pt2X = (float)pt2XD;
				pt2Y = (float)pt2YD;

				NextStartX = pt2X;
				NextStartY = pt2Y;
			}

			if (draw)
			{
				if (_lineCnt == 1)
				{
					if (_hasNewSketch == true)
					{
						Xadj = 0;
						Yadj = 0;
					}

					SecBeginX = Xadj;
					SecBeginY = Yadj;
				}

				decimal Tst1 = Convert.ToDecimal(Xadj);
				decimal Tst2 = Convert.ToDecimal(Yadj);
				decimal Ptx = Convert.ToDecimal(pt2X);
				decimal Pty = Convert.ToDecimal(pt2Y);

				var rndTst1 = Math.Round(Tst1, 1);
				var rndTst2 = Math.Round(Tst2, 1);
				var rndPt2X = Math.Round(Ptx, 1);
				var rndPt2Y = Math.Round(Pty, 1);

				if (_hasNewSketch == true && _lineCnt == 1)
				{
					if (direction == "NE")
					{
						rndTst1 = 0;
						rndTst2 = 0;

						rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
					if (direction == "SE")
					{
						rndTst1 = 0;
						rndTst2 = 0;

						rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
					if (direction == "NW")
					{
						rndTst1 = 0;
						rndTst2 = 0;

						rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
					if (direction == "SW")
					{
						rndTst1 = 0;
						rndTst2 = 0;

						rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}

					decimal TprevTst1 = prevTst1;
					decimal TprevTst2 = prevTst2;
					decimal TprevPt2X = prevPt2X;
					decimal TprevPt2Y = prevPt2Y;
				}

				if (_hasNewSketch == true && _lineCnt > 1)
				{
					decimal TprevTst1 = prevTst1;
					decimal TprevTst2 = prevTst2;
					decimal TprevPt2X = prevPt2X;
					decimal TprevPt2Y = prevPt2Y;

					if (direction == "NE")
					{
						rndTst1 = prevTst1;
						rndTst2 = prevTst2;

						rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
					if (direction == "SE")
					{
						rndTst1 = prevTst1;
						rndTst2 = prevTst2;

						rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
					if (direction == "NW")
					{
						rndTst1 = prevTst1;
						rndTst2 = prevTst2;

						rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
					if (direction == "SW")
					{
						rndTst1 = prevTst1;
						rndTst2 = prevTst2;

						rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

						rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

						prevPt2X = rndPt2X;
						prevPt2Y = rndPt2Y;
						prevTst1 = rndPt2X;
						prevTst2 = rndPt2Y;
					}
				}

				StringBuilder mxline2 = new StringBuilder();
				mxline2.Append(String.Format("select max(jlline#) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
							  MainForm.localLib,
						  MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								_currentParcel.Record,
								_currentParcel.Card,
								_nextSectLtr));

				try
				{
					_curLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(mxline2.ToString()));
				}
				catch
				{
				}

				if (_curLineCnt == 0)
				{
					_curLineCnt = 0;
				}

				_lineCnt = _curLineCnt;

				lineCnt = _curLineCnt + 1;

				if (lineCnt == 19)
				{
					MessageBox.Show("Next Line will Max Section Lines", "Line Count Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

				if (lineCnt > 20)
				{
					MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}

				if (_isclosing == true)
				{
					var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
					var sectionArea = sectionPolygon.Area;

					if (_nextStoryHeight < 1.0m)
					{
						_nextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
					}
					if (_nextStoryHeight >= 1.0m)
					{
						_nextSectArea = Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1);
					}
				}

				int checkcnt = lineCnt;

				if (lineCnt <= 20)
				{
					//MessageBox.Show(String.Format("Insert into Line Record - {0}, Card - {1} at 2416", _currentParcel.Record, _currentParcel.Card));

					StringBuilder addSectAng = new StringBuilder();
					addSectAng.Append(String.Format("insert into {0}.{1}line ( jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach) ",
								 MainForm.localLib,
						  MainForm.localPreFix

								//MainForm.FClib,
								//MainForm.FCprefix
								));
					addSectAng.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
						_currentParcel.Record,
						_currentParcel.Card,
						_nextSectLtr.Trim(),
						lineCnt,
						direction.Trim(),
						lengthX,
						lengthY,
						length,
						0,
						rndTst1,  // jlpt1x  tst1 /// 27.0
						rndTst2,  // jlpt1y  tst2 //// 0
						rndPt2X,  // jlpt2x tst1x ///// 13.5
						rndPt2Y,  // jlpt2y tst2y //// 3.0
						" "));

					NextStartX = (float)rndPt2X;
					NextStartY = (float)rndPt2Y;

					if (_undoLine == false)
					{
						if (Math.Abs(lengthX) > 0 || Math.Abs(lengthY) > 0)
						{
							dbConn.DBConnection.ExecuteNonSelectStatement(addSectAng.ToString());
						}
					}
				}

				if (_undoLine == true)
				{
					_undoLine = false;
				}
			}

			DistText.Focus();
		}

		private Image byteArrayToImage(byte[] byteArrayIn)
		{
			MemoryStream ms = new MemoryStream(byteArrayIn);
			Image returnImage = Image.FromStream(ms);
			return returnImage;
		}

		private void CalculateClosure(float _distX, float _distY)
		{
			float ewDist = (SecBeginX - _distX);
			float nsDist = (SecBeginY - _distY);

			string closeX = String.Empty;
			string closeY = String.Empty;

			_openForm = true;

			if (ewDist > 0)
			{
				closeY = "E";
			}
			if (ewDist < 0)
			{
				closeY = "W";
			}
			if (nsDist < 0)
			{
				closeX = "N";
			}
			if (nsDist > 0)
			{
				closeX = "S";
			}

			if (Math.Round(Convert.ToDecimal(ewDist), 1) == 0 && Math.Round(Convert.ToDecimal(nsDist), 1) == 0)
			{
				_openForm = false;

				decimal EWdist = Math.Round(Convert.ToDecimal(ewDist), 1);
				decimal NSdist = Math.Round(Convert.ToDecimal(nsDist), 1);

				ShowDistanceForm(closeY, EWdist, closeX, NSdist, _openForm);
			}

			if (Math.Round(Convert.ToDecimal(ewDist), 1) != 0 || Math.Round(Convert.ToDecimal(nsDist), 1) != 0)
			{
				_openForm = true;

				decimal EWdist = Math.Round(Convert.ToDecimal(ewDist), 1);
				decimal NSdist = Math.Round(Convert.ToDecimal(nsDist), 1);

				ShowDistanceForm(closeY, EWdist, closeX, NSdist, _openForm);
			}
		}

		private void calculateNewArea(int record, int card, string nextsec)
		{
			StringBuilder getLine = new StringBuilder();
			getLine.Append("select jlpt1x,jlpt1y,jlpt2x,jlpt2Y ");
			getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
					  MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.mrecno,
						_currentParcel.mdwell));
			getLine.Append(String.Format("and jlsect = '{0}' ", nextsec));

			DataSet arealines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());

			AreaTable.Clear();

			for (int i = 0; i < arealines.Tables[0].Rows.Count; i++)
			{
				DataRow row = AreaTable.NewRow();
				row["XPt1"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt1x"].ToString());
				row["YPt1"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt1y"].ToString());
				row["XPt2"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt2x"].ToString());
				row["YPt2"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt2Y"].ToString());

				AreaTable.Rows.Add(row);
			}

			decimal sumareacalc = 0;
			decimal x1 = 0;
			decimal y2 = 0;
			decimal y1 = 0;
			decimal x2 = 0;

			for (int i = 0; i < AreaTable.Rows.Count; i++)
			{
				x1 = Convert.ToDecimal(AreaTable.Rows[i]["XPt1"].ToString());

				if ((i + 1) == AreaTable.Rows.Count)
				{
					y2 = Convert.ToDecimal(AreaTable.Rows[0]["YPt1"].ToString());
				}
				if (i < AreaTable.Rows.Count && (i + 1) != AreaTable.Rows.Count)
				{
					y2 = Convert.ToDecimal(AreaTable.Rows[i + 1]["YPt1"].ToString());
				}

				sumareacalc = sumareacalc + (x1 * y2);
			}

			for (int i = 0; i < AreaTable.Rows.Count; i++)
			{
				y1 = Convert.ToDecimal(AreaTable.Rows[i]["YPt1"].ToString());

				if ((i + 1) == AreaTable.Rows.Count)
				{
					x2 = Convert.ToDecimal(AreaTable.Rows[0]["XPt1"].ToString());
				}

				if (i < AreaTable.Rows.Count && (i + 1) < AreaTable.Rows.Count)
				{
					x2 = Convert.ToDecimal(AreaTable.Rows[i + 1]["XPt1"].ToString());
				}

				sumareacalc = sumareacalc - (y1 * x2);
			}

			_calcNextSectArea = Math.Round(Convert.ToDecimal((sumareacalc / 2.0m)), 1);

			if (_calcNextSectArea < 0)
			{
				_calcNextSectArea = (_calcNextSectArea * -1);
			}
		}

		private void ClrX()
		{
			if (draw != false)
			{
				StringBuilder delXdir = new StringBuilder();
				delXdir.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X'",
							   MainForm.localLib,
						  MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								_currentParcel.Record,
								_currentParcel.Card));

				dbConn.DBConnection.ExecuteNonSelectStatement(delXdir.ToString());
			}
		}

		private void computeArea()
		{
			var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
			var sectionArea = sectionPolygon.Area;

			calculateNewArea(_currentParcel.Record, _currentParcel.Card, _nextSectLtr);

			if (_nextStoryHeight < 1.0m)
			{
				_nextStoryHeight = 1;
			}
			if (_nextStoryHeight >= 1.0m)
			{
				_nextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
			}

			_nextSectArea = (Math.Round((_calcNextSectArea * _nextStoryHeight), 1));
		}

		private void CountLines(string thisSection)
		{
			string curlincnt = string.Format("select count(*) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ", MainForm.localLib, MainForm.localPreFix, _currentParcel.Record, _currentParcel.Card, thisSection);

			SecLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(curlincnt.ToString()));
		}

		private int CountSections()
		{
			try
			{
				string seccnt = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", MainForm.localLib, MainForm.localPreFix, _currentParcel.Record, _currentParcel.Card);

				int secItemCnt = 0;
				Int32.TryParse(dbConn.DBConnection.ExecuteScalar(seccnt).ToString(), out secItemCnt);
				return secItemCnt;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));

				throw;
			}
		}

		private void DeleteLineSection()
		{
			StringBuilder deletelinesect = new StringBuilder();
			deletelinesect.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
						   MainForm.localLib,
						   MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							CurrentSecLtr));

			dbConn.DBConnection.ExecuteNonSelectStatement(deletelinesect.ToString());
		}

		private void DeleteSection()
		{
			SketchSection sksect = new SketchSection(_currentParcel, dbConn, _currentSection);
			sksect.ShowDialog(this);

			_currentSection = new SectionDataCollection(dbConn, _currentParcel.Record, _currentParcel.Card);
		}

		private string FindClosestCorner(float CurrentScale, ref string curltr, List<string> AttSecLtrList)
		{
			string secltr;
			decimal dist1 = 0;
			decimal dist1x = 0;
			decimal dist2 = 0;
			decimal distX = 0;
			int rowindex = 0;

			//was called dv--renamed for clarity
			DataView SortedJumpTableDataView = new DataView(JumpTable);
			SortedJumpTableDataView.Sort = "Dist ASC";

			BeginSplitX = (float)(Convert.ToDecimal(SortedJumpTableDataView[0]["XPt2"].ToString()));
			BeginSplitY = (float)(Convert.ToDecimal(SortedJumpTableDataView[0]["YPt2"].ToString()));

			NextStartX = BeginSplitX;
			NextStartY = BeginSplitY;

			for (int i = 0; i < SortedJumpTableDataView.Count; i++)
			{
				dist1 = Convert.ToDecimal(JumpTable.Rows[i]["Dist"].ToString());
				dist1x = Convert.ToDecimal(SortedJumpTableDataView[i]["Dist"].ToString());

				if (i == 0)
				{
					dist2 = dist1;
					rowindex = i;
				}

				if (dist1 <= dist2 && i > 0)
				{
					dist2 = dist1;
					rowindex = i;
				}
			}

			distX = Convert.ToDecimal(SortedJumpTableDataView[0]["Dist"].ToString());

			secltr = SortedJumpTableDataView[0]["Sect"].ToString();
			AttSecLtrList.Add(secltr);
			int cntsec = 0;

			for (int i = 1; i < SortedJumpTableDataView.Count; i++)
			{
				decimal distx2 = Convert.ToDecimal(SortedJumpTableDataView[i]["Dist"].ToString());
				curltr = SortedJumpTableDataView[i]["Sect"].ToString();

				if (distx2 == distX)
				{
					cntsec++;
					AttSecLtrList.Add(curltr);
				}
			}

			/* Joey's attempt to simplify the determination of the closest points and populate the multi-attach section if there are more than one.

			List<PointWithComparisons> possibleAttachmentPoints = ClosestPoints(JumpTable, new PointF(JumpX, JumpY));
			if (possibleAttachmentPoints.Count > 1)
			{
				AttSecLtrList.Clear();
				foreach (PointWithComparisons p in possibleAttachmentPoints)
				{
					AttSecLtrList.Add(p.PointLabel);
				}
			}
			else
			{
				secltr = possibleAttachmentPoints.FirstOrDefault<PointWithComparisons>().PointLabel;
			}

End Joey's alternative Code */

			string multisectatch = MultiPointsAvailable(AttSecLtrList);

			SaveJumpPointsAndOldSectionEndPoints(CurrentScale, rowindex, SortedJumpTableDataView);

			string _CurrentSecLtr = JumpTable.Rows[rowindex]["Sect"].ToString();

			//Ask Dave why this is set here if it is set differently below
			//  Rube Goldberg code. Value is set again below, so I commented this one out.
			//	CurrentSecLtr = SortedJumpTableDataView[0]["Sect"].ToString();

			int savedAttLine = Convert.ToInt32(JumpTable.Rows[rowindex]["LineNo"].ToString());

			_savedAttLine = Convert.ToInt32(SortedJumpTableDataView[0]["LineNo"].ToString());
			Trace.WriteLine(string.Format("_savedAttLine = Convert.ToInt32(JumpTable.Rows[rowindex][LineNo]={0}", _savedAttLine));
			Trace.WriteLine(string.Format("************ ({0} is not subsequently used.******** ", _savedAttLine));
			Trace.WriteLine(string.Format("_savedAttLine = Convert.ToInt32(SortedJumpTableDataView[0][LineNo]={0}", _savedAttLine));

			//Ask Dave why this is set here if it is set differently above
			//Ask Dave why sometimes the rowindex of the JumpTable is used and othertimes the row[0] of the Sorted Jump Table
			CurrentSecLtr = _CurrentSecLtr;

			if (AttSecLtrList.Count > 1)
			{
				CurrentSecLtr = multisectatch;
			}

			string priorDirection = JumpTable.Rows[rowindex]["Direct"].ToString();

			string savedAttSection = JumpTable.Rows[rowindex]["Sect"].ToString();
			int _CurrentAttLine = Convert.ToInt32(JumpTable.Rows[rowindex]["LineNo"].ToString());

			startSplitX = Convert.ToDecimal(SortedJumpTableDataView[0]["XPt1"].ToString());
			startSplitY = Convert.ToDecimal(SortedJumpTableDataView[0]["YPt1"].ToString());
			Trace.WriteLine(string.Format("Start split point: {0},{1}", startSplitX, startSplitY));
			/* More Rube Goldberg code. These values are set, but then they are not used anywhere.
			 -JMM
						decimal tsplit2 = Convert.ToDecimal(SortedJumpTableDataView[0]["XPt2"].ToString());
						decimal tsplit3 = Convert.ToDecimal(SortedJumpTableDataView[0]["YPt2"].ToString());
			*/
			_priorDirection = SortedJumpTableDataView[0]["Direct"].ToString();
			_savedAttSection = SortedJumpTableDataView[0]["Sect"].ToString();
			CurrentAttLine = Convert.ToInt32(SortedJumpTableDataView[0]["LineNo"].ToString());

			//TODO: Find the last moved direction and the direction of the Current AttLine. If they are not the same call undo and return to main screen.
			_mouseX = Convert.ToInt32(JumpX);
			_mouseY = Convert.ToInt32(JumpY);
			Trace.WriteLine(string.Format("Mouse moved to {0},{1}", JumpX, JumpY));
			Trace.WriteLine(string.Format("Section attachment is {0} Line {1}, _priorDirection is {2}", _savedAttSection, CurrentAttLine, _priorDirection));
			legalMoveDirection = AttachLineDirection(_savedAttSection, CurrentAttLine);
			MoveCursor();
			return secltr;
		}

		private string AttachLineDirection(string attachSection, int attachLineNumber)
		{
			//Find the line that begins where the line in the saved section ends.
			string lineDirection = string.Empty;
			decimal lastLineEndX = 0M;
			decimal lastLineEndY = 0M;
			decimal nextLineStartX = 0M;
			decimal nextLineStartY = 0M;
			string checkRowSection = string.Empty;
			int checkRowLine = 0;
			for (int i = 0; i < JumpTable.Rows.Count; i++)
			{
				DataRow checkRow = JumpTable.Rows[i];
				checkRowSection = checkRow["Sect"].ToString().Trim();
				Int32.TryParse(checkRow["LineNo"].ToString(), out checkRowLine);
				if (checkRowSection == attachSection && checkRowLine == attachLineNumber) // this is the row whose END points are the start point of the line with the legal direction
				{
					decimal.TryParse(checkRow["XPt2"].ToString(), out lastLineEndX);
					decimal.TryParse(checkRow["YPt2"].ToString(), out lastLineEndY);
				}
			}

			// Now get the line that starts with those end point, in the same section.
			for (int i = 0; i < JumpTable.Rows.Count; i++)
			{
				DataRow checkRow = JumpTable.Rows[i];
				checkRowSection = checkRow["Sect"].ToString().Trim();
				decimal.TryParse(checkRow["XPt1"].ToString(), out nextLineStartX);
				decimal.TryParse(checkRow["YPt1"].ToString(), out nextLineStartY);

				if (checkRowSection == attachSection && nextLineStartX == lastLineEndX && nextLineStartY == lastLineEndY) // this is the row whose direction we need
				{
					lineDirection = checkRow["Direct"].ToString().Trim();
				}
			}
			return lineDirection;
		}

		public void findends()
		{
			delStartX = 0;
			delStartY = 0;
			_lastDir = String.Empty;

			StringBuilder cntLine = new StringBuilder();
			cntLine.Append(String.Format("select max(jlline#) from {0}.{1}line ", MainForm.localLib, MainForm.localPreFix));
			cntLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ", _currentParcel.mrecno, _currentParcel.mdwell, _nextSectLtr));

			try
			{
				int jlinecnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(cntLine.ToString()));

				StringBuilder curLine = new StringBuilder();
				curLine.Append(String.Format("select jldirect,jlpt1x,jlpt1y from {0}.{1}line ", MainForm.localLib, MainForm.localPreFix));
				curLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
								_currentParcel.mrecno,
								_currentParcel.mdwell,
								_nextSectLtr,
								jlinecnt));
				DataSet dsl = dbConn.DBConnection.RunSelectStatement(curLine.ToString());

				if (dsl.Tables[0].Rows.Count > 0)
				{
					_lastDir = dsl.Tables[0].Rows[0]["jldirect"].ToString();

					delStartX = (float)(Convert.ToDecimal(dsl.Tables[0].Rows[0]["jlpt1x"].ToString()));
					delStartY = (float)(Convert.ToDecimal(dsl.Tables[0].Rows[0]["jlpt1y"].ToString()));
				}
			}
			catch
			{
			}
		}

		private void FixOrigLine()
		{
			StringBuilder fixOrigLine = new StringBuilder();
			fixOrigLine.Append(String.Format("update {0}.{1}line ", MainForm.localLib, MainForm.localPreFix));
			fixOrigLine.Append(String.Format("set jlxlen = {0},jlylen = {1}, jllinelen = {2}, jlpt2x = {3}, jlpt2y = {4} ",
									adjNewSecX,
									adjNewSecY,
									RemainderLineLength,
									begNewSecX,
									begNewSecY));
			fixOrigLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							CurrentSecLtr,
							_savedAttLine));

			dbConn.DBConnection.ExecuteNonSelectStatement(fixOrigLine.ToString());
		}

		private void FlipLeftRight()
		{
			StringBuilder sectable = new StringBuilder();
			sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
			sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3}  ",
						  MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card));

			DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

			if (scl.Tables[0].Rows.Count > 0)
			{
				SectionTable.Clear();

				for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
				{
					DataRow row = SectionTable.NewRow();
					row["Record"] = _currentParcel.mrecno;
					row["Card"] = _currentParcel.mdwell;
					row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
					row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
					row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
					row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
					row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
					row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
					row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

					string testd = scl.Tables[0].Rows[i]["jldirect"].ToString();

					string mytestdir = row["Direct"].ToString();

					if (mytestdir == "E")
					{
						row["Direct"] = "W";
					}
					if (mytestdir == "NE")
					{
						row["Direct"] = "NW";
					}
					if (mytestdir == "SE")
					{
						row["Direct"] = "SW";
					}
					if (mytestdir == "W")
					{
						row["Direct"] = "E";
					}
					if (mytestdir == "NW")
					{
						row["Direct"] = "NE";
					}
					if (mytestdir == "SW")
					{
						row["Direct"] = "SE";
					}

					if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString()) != 0)
					{
						row["Xpt1"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString()) * -1);
					}
					if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString()) != 0)
					{
						row["Xpt2"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString()) * -1);
					}

					SectionTable.Rows.Add(row);
				}
			}

			for (int i = 0; i < SectionTable.Rows.Count; i++)
			{
				string fsect = SectionTable.Rows[i]["Sect"].ToString().Trim();
				int flineno = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());
				string fdirect = SectionTable.Rows[i]["Direct"].ToString().Trim();
				decimal fXpt1 = Convert.ToDecimal(SectionTable.Rows[i]["Xpt1"].ToString());
				decimal fXpt2 = Convert.ToDecimal(SectionTable.Rows[i]["Xpt2"].ToString());

				StringBuilder flipit = new StringBuilder();
				flipit.Append(String.Format("update {0}.{1}line set jldirect = '{2}', jlpt1x = {3}, jlpt2x = {4} ",
							   MainForm.localLib,
							   MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								fdirect,
								fXpt1,
								fXpt2));
				flipit.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
								_currentParcel.mrecno, _currentParcel.mdwell, fsect, flineno));

				dbConn.DBConnection.ExecuteNonSelectStatement(flipit.ToString());
			}

			_closeSketch = true;

			RefreshSketch();
		}

		private void FlipUpDown()
		{
			StringBuilder sectable = new StringBuilder();
			sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
			sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3}  ",
						  MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card));

			DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

			if (scl.Tables[0].Rows.Count > 0)
			{
				SectionTable.Clear();

				for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
				{
					DataRow row = SectionTable.NewRow();
					row["Record"] = _currentParcel.mrecno;
					row["Card"] = _currentParcel.mdwell;
					row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
					row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
					row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
					row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
					row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
					row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
					row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

					string testd = scl.Tables[0].Rows[i]["jldirect"].ToString();

					string mytestdir = row["Direct"].ToString();

					if (mytestdir == "N")
					{
						row["Direct"] = "S";
					}
					if (mytestdir == "NE")
					{
						row["Direct"] = "SE";
					}
					if (mytestdir == "NW")
					{
						row["Direct"] = "SW";
					}

					if (mytestdir == "S")
					{
						row["Direct"] = "N";
					}
					if (mytestdir == "SE")
					{
						row["Direct"] = "NE";
					}
					if (mytestdir == "SW")
					{
						row["Direct"] = "NW";
					}

					if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString()) != 0)
					{
						row["Ypt1"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString()) * -1);
					}
					if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString()) != 0)
					{
						row["Ypt2"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString()) * -1);
					}

					SectionTable.Rows.Add(row);
				}
			}

			for (int i = 0; i < SectionTable.Rows.Count; i++)
			{
				string fsect = SectionTable.Rows[i]["Sect"].ToString().Trim();
				int flineno = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());
				string fdirect = SectionTable.Rows[i]["Direct"].ToString().Trim();
				decimal fYpt1 = Convert.ToDecimal(SectionTable.Rows[i]["Ypt1"].ToString());
				decimal fYpt2 = Convert.ToDecimal(SectionTable.Rows[i]["Ypt2"].ToString());

				StringBuilder flipitFB = new StringBuilder();
				flipitFB.Append(String.Format("update {0}.{1}line set jldirect = '{2}', jlpt1y = {3}, jlpt2y = {4} ",
								   MainForm.localLib,
								   MainForm.localPreFix,

									 //MainForm.FClib,
									 //MainForm.FCprefix,
									 fdirect,
									 fYpt1,
									 fYpt2));
				flipitFB.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
								_currentParcel.mrecno, _currentParcel.mdwell, fsect, flineno));

				dbConn.DBConnection.ExecuteNonSelectStatement(flipitFB.ToString());
			}

			_closeSketch = true;

			RefreshSketch();
		}

		private DataSet GetLinesData(int crrec, int crcard)
		{
			DataSet lines;
			string getLine = string.Format("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} ", MainForm.localLib, MainForm.localPreFix, crrec, crcard);

			lines = dbConn.DBConnection.RunSelectStatement(getLine);
			return lines;
		}

		private DataSet GetSectionLines(int crrec, int crcard)
		{
			DataSet lines;
			StringBuilder getLine = new StringBuilder();
			getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
			getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
			getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
					   MainForm.localLib,
					   MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						crrec,
						crcard));
			getLine.Append("and jldirect <> 'X' ");

			lines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());
			return lines;
		}

		public void getSplitLine()
		{
			int Sprowindex = 0;

			ConnectSec = String.Empty;

			if (MultiSectionSelection.adjsec != String.Empty)
			{
				ConnectSec = MultiSectionSelection.adjsec;
			}
			if (MultiSectionSelection.adjsec == String.Empty)
			{
				ConnectSec = _savedAttSection;
			}

			mylineNo = 0;

			decimal startsplitx1 = startSplitX;
			decimal startsplity1 = startSplitY;

			if (BeginSplitX != (float)startSplitX)
			{
				NextStartX = (float)startSplitX;
			}
			if (BeginSplitY != (float)startSplitY)
			{
				NextStartY = (float)startSplitY;
			}

			DataSet Sprolines = null;

			StringBuilder getSpLine = new StringBuilder();
			getSpLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
			getSpLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
			getSpLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
						   MainForm.localLib,
						  MainForm.localPreFix,

						   //MainForm.FClib,
						   //MainForm.FCprefix,
						   _currentParcel.mrecno,
						   _currentParcel.mdwell,
						   ConnectSec));

			Sprolines = dbConn.DBConnection.RunSelectStatement(getSpLine.ToString());

			int maxsecline = Sprolines.Tables[0].Rows.Count;
			if (Sprolines.Tables[0].Rows.Count > 0)
			{
				RESpJumpTable.Clear();

				for (int i = 0; i < Sprolines.Tables[0].Rows.Count; i++)
				{
					DataRow row = RESpJumpTable.NewRow();
					row["Record"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlrecord"].ToString());
					row["Card"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jldwell"].ToString());
					row["Sect"] = Sprolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["LineNo"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlline#"].ToString());
					row["Direct"] = Sprolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["XLen"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlxlen"].ToString());
					row["YLen"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlylen"].ToString());
					row["Length"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jllinelen"].ToString());
					row["Angle"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlangle"].ToString());
					row["XPt1"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["YPt1"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["XPt2"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["YPt2"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
					row["Attach"] = Sprolines.Tables[0].Rows[i]["jlattach"].ToString();

					decimal xpt2 = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2x"].ToString());
					decimal ypt2 = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2y"].ToString());

					float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
					float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

					Sprowindex = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlline#"].ToString());

					RESpJumpTable.Rows.Add(row);
				}
			}

			if (RESpJumpTable.Rows.Count > 0)
			{
				AttSpLineNo = 0;

				bool foundLine = false;

				int RESpJumpTableIndex = 0;

				if (RESpJumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < RESpJumpTable.Rows.Count; i++)
					{
						if (offsetDir == "N" || offsetDir == "S")
						{
							float txtsx = NextStartX;

							float txtsy = NextStartY;

							decimal tstxadjr = XadjR;

							XadjR = Convert.ToDecimal(NextStartX);
							YadjR = Convert.ToDecimal(NextStartY);

							decimal testdit = distance;

							string dirsect = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
							decimal x1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["XLen"].ToString());
							decimal y1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["YLen"].ToString());
							decimal x1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt1"].ToString());
							decimal y1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt1"].ToString());
							decimal x2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt2"].ToString());
							decimal y2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt2"].ToString());
							int lnNbr = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
							string atsect = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
							decimal origLen = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());

							decimal testX = startSplitX;
							decimal testY = startSplitY;

							decimal check = (y2 - distance);

							if (offsetDir == "N" && NextStartY != (float)(y2 - distance))
							{
								//startSplitY = (y2 - distance);
							}

							if (offsetDir == "S")
							{
								//startSplitY = (y2 + distance);
							}

							if (x2 != startSplitX)
							{
								NextStartX = (float)startSplitX;
							}

							if (NextStartY != (float)startSplitY)
							{
								NextStartY = (float)startSplitY;
							}

							int dex1 = x1.ToString().IndexOf(".");
							int dex2 = x2.ToString().IndexOf(".");

							int xxt = i;

							if (NextStartX == (float)x2 && (i + 1) < RESpJumpTable.Rows.Count)
							{
								int newLineNbr = Convert.ToInt32(RESpJumpTable.Rows[i + 1]["LineNo"].ToString());

								if (startSplitY >= y2 && startSplitY <= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString()))
								{
									_savedAttSection = atsect;

									OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["YLen"].ToString());

									begSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt1"].ToString());

									begSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt1"].ToString());

									EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
									EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

									CurrentSecLtr = atsect;
									RemainderLineLength = OrigLineLength - splitLineDist;
								}
							}

							decimal x1x = (x1 + .5m);

							decimal x1B = (x1 - .5m);

							decimal x2x = (x2 + .5m);

							decimal x2B = (x2 - .5m);

							if (NextStartY < (float)y1 && NextStartY > (float)y2 && dirsect == "N" && NextStartX >= (float)x1B && NextStartX <= (float)x1x)
							{
								breakLineNbr = lnNbr;
								AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
								OffSetAttSpLineDir = offsetDir;
								RESpJumpTableIndex = i;

								// foundLine = true; No usages found
								begNewSecX = startSplitX;
								begNewSecY = startSplitY;
								adjNewSecX = x1Len;
								adjNewSecY = y1Len;
								OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
								OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
								EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
								EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
								OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
								AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
								mylineNo = AttLineNo;
								break;
							}

							if (NextStartY > (float)y1 && NextStartY < (float)y2 && dirsect == "S" && NextStartX >= (float)x1B && NextStartX <= (float)x1x)
							{
								breakLineNbr = lnNbr;
								AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
								OffSetAttSpLineDir = offsetDir;
								RESpJumpTableIndex = i;

								// foundLine = true; No usages found
								begNewSecX = startSplitX;
								begNewSecY = startSplitY;
								adjNewSecX = x1Len;
								adjNewSecY = y1Len;
								OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
								OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
								EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
								EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
								OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
								AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
								mylineNo = AttLineNo;
								break;
							}
						}
						if (offsetDir == "E" || offsetDir == "W")
						{
							float txtsx = NextStartX;

							float txtsy = NextStartY;

							decimal tstyadjr = YadjR;

							XadjR = Convert.ToDecimal(NextStartX);
							YadjR = Convert.ToDecimal(NextStartY);

							string dirsect = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
							decimal x1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["XLen"].ToString());
							decimal y1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["YLen"].ToString());
							decimal x1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt1"].ToString());
							decimal y1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt1"].ToString());
							decimal x2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt2"].ToString());
							decimal y2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt2"].ToString());
							int lnNbr = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
							string atsect = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
							decimal origLen = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());

							decimal TestX = startSplitX;
							decimal TestY = startSplitY;

							if (y2 != startSplitY)
							{
								//NextStartY = (float)startSplitY;
							}

							if (x2 != startSplitX)
							{
								//NextStartX = (float)startSplitX;
							}

							int dey1 = y1.ToString().IndexOf(".");
							int dey2 = y2.ToString().IndexOf(".");

							decimal mytest = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());

							if (startSplitY == y2)
							{
								if (startSplitX >= x2 && startSplitX <= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString()) && offsetDir == "E")
								{
									_savedAttSection = atsect;

									OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

									EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
									EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

									decimal d2 = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

									CurrentSecLtr = atsect;

									RemainderLineLength = d2 - splitLineDist;
								}

								if (startSplitX <= x2 && startSplitX >= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString()) && offsetDir == "W")
								{
									_savedAttSection = atsect;

									OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

									EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
									EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

									decimal d2 = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

									CurrentSecLtr = atsect;

									RemainderLineLength = d2 - splitLineDist;
								}
							}

							decimal y1x = (y1 + .5m);

							decimal y2x = (y2 + .5m);

							decimal y1B = (y1 - .5m);

							decimal y2B = (y2 - .5m);

							decimal x1x = (x1 + .5m);

							decimal x1B = (x1 - .5m);

							decimal x2x = (x2 + .5m);

							decimal x2B = (x2 - .5m);

							if (NextStartX >= (float)x1 && NextStartX <= (float)x2 && dirsect == "E" && NextStartY >= (float)y2B && NextStartY <= (float)y2x)
							{
								breakLineNbr = lnNbr;
								AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
								OffSetAttSpLineDir = offsetDir;
								RESpJumpTableIndex = i;

								// foundLine = true; No usages found
								begNewSecX = startSplitX;
								begNewSecY = startSplitY;
								adjNewSecX = x1Len;
								adjNewSecY = y1Len;
								OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
								OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
								EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
								EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
								OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
								AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
								mylineNo = AttLineNo;
								break;
							}

							if (NextStartX < (float)x1 && NextStartX > (float)x2 && dirsect == "W" && NextStartY >= (float)y2B && NextStartY <= (float)y2x)
							{
								breakLineNbr = lnNbr;
								AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
								AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
								OffSetAttSpLineDir = offsetDir;
								RESpJumpTableIndex = i;

								// foundLine = true; No usages found
								begNewSecX = startSplitX;
								begNewSecY = startSplitY;
								adjNewSecX = x1Len;
								adjNewSecY = y1Len;
								OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
								OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
								EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
								EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
								OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
								AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
								mylineNo = AttLineNo;
								break;
							}
						}
					}

					BeginSplitX = NextStartX;

					BeginSplitY = NextStartY;

					if (draw)
					{
						if (_lastDir == "E")
						{
							NextStartX = NextStartX + distanceD;

							float ty = NextStartY;
						}
						if (_lastDir == "W")
						{
							NextStartX = NextStartX - distanceD;

							float ty = NextStartY;
						}

						if (_lastDir == "N")
						{
							NextStartY = NextStartY - distanceD;

							float tx = NextStartX;
						}
						if (_lastDir == "S")
						{
							NextStartY = NextStartY + distanceD;

							float tx = NextStartX;
						}
					}

					BeginSplitX = NextStartX;

					BeginSplitY = NextStartY;
				}
			}
		}

		private void GetStartCorner()
		{
			_undoMode = true;

			undoPoints.Clear();

			int rowindex = 0;

			for (int i = 0; i < REJumpTable.Rows.Count; i++)
			{
				DataRow row = undoPoints.NewRow();

				float _JumpX1 = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[i]["XPt1"].ToString()) * _currentScale)); //  change XPt1 to XPt2
				float _JumpY1 = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[i]["YPT1"].ToString()) * _currentScale));
				float _JumpX2 = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[i]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
				float _JumpY2 = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[i]["YPT2"].ToString()) * _currentScale));

				JumpX = _JumpX1;
				JumpY = _JumpY1;
				float JumpX2 = _JumpX2;
				float JumpY2 = _JumpY2;

				int _mouseX1 = Convert.ToInt32(JumpX);
				int _mouseY1 = Convert.ToInt32(JumpY);
				int _mouseX2 = Convert.ToInt32(JumpX2);
				int _mouseY2 = Convert.ToInt32(JumpY2);

				row["Direct"] = REJumpTable.Rows[i]["Direct"].ToString();
				row["X1pt"] = _mouseX1;
				row["Y1pt"] = _mouseY1;
				row["X2pt"] = _mouseX2;
				row["Y2pt"] = _mouseY2;

				undoPoints.Rows.Add(row);
			}

			RedrawSection();
		}

		private byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			MemoryStream dh = new MemoryStream();
			imageIn.Save(dh, System.Drawing.Imaging.ImageFormat.Jpeg);
			return dh.ToArray();
		}

		private void InsertLine(string CurAttDir, decimal newEndX, decimal newEndY, decimal StartEndX, decimal StartEndY, decimal splitLength)
		{
			StringBuilder insertLine = new StringBuilder();
			insertLine.Append(String.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen, ",
					  MainForm.localLib, MainForm.localPreFix));
			insertLine.Append("jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ) values ( ");
			insertLine.Append(String.Format(" {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' )", _currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr,
				mylineNo,
				CurAttDir,
				Math.Abs(adjNewSecX),
				Math.Abs(adjNewSecY),
				Math.Abs(splitLength),
				0,
				StartEndX,
				StartEndY,
				newEndX,
				newEndY,
				_nextSectLtr
				));

			dbConn.DBConnection.ExecuteNonSelectStatement(insertLine.ToString());
		}

		private bool IsMovementAllowed(MoveDirections direction)
		{
			bool isAllowed = (this.dt.Rows.Count > 0);

			if (isAllowed && this.isInAddNewPointMode)
			{
				isAllowed = false;
			}

			if (isAllowed && "".Equals(this.DistText.Text))
			{
				this.ShowMessageBox("Please indicate a length");
				isAllowed = false;
			}

			if (isAllowed)
			{
				string dir = dt.Rows[this.dgSections.CurrentRow.Index]["Dir"].ToString();
				if (!dir.Equals(direction.ToString()))
				{
					this.ShowMessageBox("Illegal Direction");
					isAllowed = false;
					this.RefreshSketch();
				}
			}

			if (isAllowed)
			{
				decimal len = 0M;
				switch (direction)
				{
					case MoveDirections.N:
					case MoveDirections.S:
						len = Convert.ToDecimal(dt.Rows[this.dgSections.CurrentRow.Index]["North"].ToString());
						break;

					case MoveDirections.E:
					case MoveDirections.W:
						len = Convert.ToDecimal(dt.Rows[this.dgSections.CurrentRow.Index]["East"].ToString());
						break;

					default:
						break;
				}

				if (len == 0M)
				{
					isAllowed = false;
				}
				else if (len <= Convert.ToDecimal(this.DistText.Text))
				{
					this.ShowMessageBox("Illegal Distance");
					isAllowed = false;
					this.RefreshSketch();
				}
			}

			return isAllowed;
		}

		private bool IsValidDirection(string moveDirection)
		{
			bool goodDir = (moveDirection == legalMoveDirection || BeginSectionBtn.Text == "Active" || !checkDirection);
			return goodDir;
		}

		private void JumptoCorner()
		{
			float txtx = NextStartX;
			float txty = NextStartY;
			float jx = _mouseX;
			float jy = _mouseY;
			float _scaleBaseX = ScaleBaseX;
			float _scaleBaseY = ScaleBaseY;
			float CurrentScale = _currentScale;
			int crrec = _currentParcel.Record;
			int crcard = _currentParcel.Card;

			CurrentSecLtr = String.Empty;
			_newIndex = 0;
			CurrentAttLine = 0;

			DataSet lines = null;
			if (_isNewSketch == false)
			{
				lines = GetLinesData(crrec, crcard);
			}

			bool sketchHasLineData = lines.Tables[0].Rows.Count > 0;
			if (sketchHasLineData)
			{
				SecItemCnt = CountSections();
				PopulateSectionList();
				for (int i = 0; i < SecItemCnt; i++)
				{
					string thisSection = SecLetters[i].ToString();
					if (SecItemCnt >= 1)
					{
						CountLines(thisSection);

						AddXLine(thisSection);

						lines = GetSectionLines(crrec, crcard);
					}
				}
				JumpTable = ConstructJumpTable();
				JumpTable.Clear();

				AddListItemsToJumpTableList(jx, jy, CurrentScale, lines);

				string secltr = String.Empty;
				string curltr = String.Empty;

				List<string> AttSecLtrList = new List<string>();

				if (JumpTable.Rows.Count > 0)
				{
					secltr = FindClosestCorner(CurrentScale, ref curltr, AttSecLtrList);
				}
			}
		}

		private void LoadSection()
		{
			this.dt.Rows.Clear();
			if (this.section.SectionLines != null)
			{
				foreach (var line in this.section.SectionLines)
				{
					DataRow dr = this.dt.NewRow();
					dr["Dir"] = line.Directional.Trim();
					dr["North"] = line.YLength.ToString();
					dr["East"] = line.XLength.ToString();
					dr["Att"] = line.Attachment.Trim();
					this.dt.Rows.Add(dr);
				}
			}
		}

		private int MaximumLineCount()
		{
			int maxLineCnt;
			StringBuilder lineCntx = new StringBuilder();
			lineCntx.Append(String.Format("select max(jlline#) from {0}.{1}line ",
					   MainForm.localLib,
						  MainForm.localPreFix

						//MainForm.FClib,
						//MainForm.FCprefix
						));
			lineCntx.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ",
				_currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr));

			maxLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(lineCntx.ToString()));
			return maxLineCnt;
		}

		public void MeasureAngle()
		{
			string anglecalls = DistText.Text.Trim();

			int commaCnt = anglecalls.IndexOf(",");

			string D1 = anglecalls.Substring(0, commaCnt).Trim();

			string D2 = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();

			AngD2 = Convert.ToDecimal(D1);

			AngD1 = Convert.ToDecimal(D2);

			AngleForm _findCode = new AngleForm();
			_findCode.ShowDialog();

			if (_isKeyValid == false)
			{
				_isKeyValid = true;
			}

			if (AngleForm.NorthWest == true)
			{
				MoveNorthWest(NextStartX, NextStartY);
			}

			if (AngleForm.NorthEast == true)
			{
				MoveNorthEast(NextStartX, NextStartY);
			}
			if (AngleForm.SouthWest == true)
			{
				MoveSouthWest(NextStartX, NextStartY);
			}
			if (AngleForm.SouthEast == true)
			{
				MoveSouthEast(NextStartX, NextStartY);
			}
		}

		private void MoveCursor()
		{
			this.Cursor = new Cursor(Cursor.Current.Handle);
			Cursor.Position = new Point(Convert.ToInt32(JumpX) - 50, Convert.ToInt32(JumpY) - 50);

			if (_undoMode == true)
			{
				Graphics g = Graphics.FromImage(_mainimage);
				Pen pen1 = new Pen(Color.Red, 4);
				g.DrawRectangle(pen1, Convert.ToInt32(JumpX), Convert.ToInt32(JumpY), 1, 1);
				g.Save();

				ExpSketchPBox.Image = _mainimage;

				DMouseClick();
			}

			if (!draw && _undoMode == false)
			{
				Graphics g = Graphics.FromImage(_mainimage);
				Pen pen1 = new Pen(Color.Black, 4);
				g.DrawRectangle(pen1, Convert.ToInt32(JumpX), Convert.ToInt32(JumpY), 1, 1);
				g.Save();

				ExpSketchPBox.Image = _mainimage;

				DMouseClick();
			}

			if (draw)
			{
				Graphics g = Graphics.FromImage(_mainimage);
				Pen pen1 = new Pen(Color.Red, 4);
				g.DrawRectangle(pen1, Convert.ToInt32(JumpX), Convert.ToInt32(JumpY), 1, 1);
				g.Save();

				ExpSketchPBox.Image = _mainimage;

				DMouseClick();
			}
		}

		private string MultiPointsAvailable(List<string> AttSecLtrList)
		{
			string multisectatch = String.Empty;

			if (AttSecLtrList.Count > 1)
			{
				MulPts.Clear();

				MultiSectionSelection attsecltr = new MultiSectionSelection(AttSecLtrList, _currentParcel.mrecno, _currentParcel.mdwell, dbConn);
				attsecltr.ShowDialog(this);

				multisectatch = MultiSectionSelection.adjsec;

				MulPts = MultiSectionSelection.mulattpts;

				List<string> test = new List<string>();

				//	test = MultiSectionSelection.attsec;

				_hasMultiSection = true;
			}

			return multisectatch;
		}

		private decimal OriginalDistanceX()
		{
			decimal origDistX = 0;

			StringBuilder orgLen = new StringBuilder();
			orgLen.Append(String.Format("select jllinelen from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
					  MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.mrecno,
						_currentParcel.mdwell
						));
			orgLen.Append(String.Format("and jlsect = '{0}' and jlline# = {1} ",
				CurrentSecLtr, AttSpLineNo));

			origDistX = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(orgLen.ToString()));
			return origDistX;
		}

		private void PerformMoveLength(MoveDirections direction)
		{
			if (this.IsMovementAllowed(direction))
			{
				int cr = dgSections.CurrentRow.Index;
				try
				{
					this.pts = this.section.SectionPoints;
					Point[] adjPts = new Point[this.section.SectionPoints.Length + 1];

					decimal length = Convert.ToDecimal(this.DistText.Text);

					DataRow dr = this.dt.NewRow();
					string dir = "";

					for (int i = 0; i < this.pts.Length; i++)
					{
						if ((i < this.dgSections.CurrentRow.Index) ||
							(i == this.dgSections.CurrentRow.Index && this.dgSections.CurrentRow.Index != this.pts.Length - 1))
						{
							adjPts[i] = this.pts[i];
						}
						else if ((i == this.dgSections.CurrentRow.Index + 1) ||
							(i == this.dgSections.CurrentRow.Index && this.dgSections.CurrentRow.Index == this.pts.Length - 1))
						{
							this.isLastLine = (i == this.dgSections.CurrentRow.Index && this.dgSections.CurrentRow.Index == this.pts.Length - 1);
							bool isLineAdded = false;
							switch (direction)
							{
								case MoveDirections.NW:
									dir = "NW";
									break;

								case MoveDirections.NE:
									dir = "NE";
									break;

								case MoveDirections.SE:
									dir = "SE";
									break;

								case MoveDirections.SW:
									dir = "SW";
									break;

								case MoveDirections.N:
									dir = "N";
									dr["North"] = length.ToString();
									dr["East"] = "0.0";
									decimal old = Convert.ToDecimal(this.dt.Rows[this.dgSections.CurrentRow.Index]["North"]);
									old -= length;
									dt.Rows[this.dgSections.CurrentRow.Index]["North"] = old.ToString();
									int adjLength = 0 - Convert.ToInt32(length);
									if (isLastLine)
									{
										adjPts[i] = this.pts[i];
										adjPts[i + 1] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
									}
									else
									{
										adjPts[i] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
										adjPts[i + 1] = this.pts[i];
									}
									isLineAdded = true;
									break;

								case MoveDirections.S:
									dir = "S";
									dr["North"] = length.ToString();
									dr["East"] = "0.0";
									old = Convert.ToDecimal(this.dt.Rows[this.dgSections.CurrentRow.Index]["North"]);
									old -= length;
									dt.Rows[this.dgSections.CurrentRow.Index]["North"] = old.ToString();
									adjLength = Convert.ToInt32(length);
									if (isLastLine)
									{
										adjPts[i] = this.pts[i];
										adjPts[i + 1] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
									}
									else
									{
										adjPts[i] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
										adjPts[i + 1] = this.pts[i];
									}
									isLineAdded = true;
									break;

								case MoveDirections.E:
									dir = "E";
									dr["North"] = "0.0";
									dr["East"] = length.ToString();
									old = Convert.ToDecimal(this.dt.Rows[this.dgSections.CurrentRow.Index]["East"]);
									old -= length;
									dt.Rows[this.dgSections.CurrentRow.Index]["East"] = old.ToString();
									adjLength = Convert.ToInt32(length);
									if (isLastLine)
									{
										adjPts[i] = this.pts[i];
										adjPts[i + 1] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
									}
									else
									{
										adjPts[i] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
										adjPts[i + 1] = this.pts[i];
									}
									isLineAdded = true;
									break;

								case MoveDirections.W:
									dir = "W";
									dr["North"] = "0.0";
									dr["East"] = length.ToString();
									old = Convert.ToDecimal(this.dt.Rows[this.dgSections.CurrentRow.Index]["East"]);
									old -= length;
									dt.Rows[this.dgSections.CurrentRow.Index]["East"] = old.ToString();
									adjLength = 0 - Convert.ToInt32(length);
									if (isLastLine)
									{
										adjPts[i] = this.pts[i];
										adjPts[i + 1] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
									}
									else
									{
										adjPts[i] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
										adjPts[i + 1] = this.pts[i];
									}
									isLineAdded = true;
									break;

								default:
									break;
							}
							if (isLineAdded)
							{
								this.SetAddNewPointButton(true);
								if (this.isLastLine)
								{
									this.NewPointIndex = ++i;
								}
								else
								{
									this.NewPointIndex = i;
								}
							}
						}
						else
						{
							adjPts[i + 1] = this.pts[i];
						}
					}

					this.unadj_pts = adjPts;

					dr["Dir"] = dir;
					dr["Att"] = "";

					this.dt.Rows.InsertAt(dr, this.dgSections.CurrentRow.Index);

					this.DistText.Text = "";
					this.DrawSketch(this.dgSections.CurrentRow.Index);
				}
				catch (System.FormatException)
				{
					this.ShowMessageBox("Invalid Length");
					this.RefreshSketch();
				}
			}
		}

		private void PopulateSectionList()
		{
			string seclst = string.Format("SELECT JLSECT FROM {0}.{1}LINE WHERE JLRECORD = {2} AND JLDWELL = {3} AND JLLINE# = 1 ", MainForm.localLib, MainForm.localPreFix, _currentParcel.Record, _currentParcel.Card);

			try
			{
				DataSet ds_sec = dbConn.DBConnection.RunSelectStatement(seclst);
				if (ds_sec.Tables[0].Rows.Count > 0)
				{
					SecLetters = new List<string>();
					for (int i = 0; i < ds_sec.Tables[0].Rows.Count; i++)
					{
						string sect = ds_sec.Tables[0].Rows[i]["jlsect"].ToString().Trim();
						SecLetters.Add(sect);
					}
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		private void RedrawSection()
		{
			int tsteclick = click;

			checkRedraw = true;

			for (int i = 0; i < undoPoints.Rows.Count; i++)
			{
				float redist = 0;
				string undodirect = String.Empty;
				undodirect = undoPoints.Rows[i]["Direct"].ToString();

				float x1 = Convert.ToSingle(undoPoints.Rows[i]["X1pt"].ToString());

				float y1 = Convert.ToSingle(undoPoints.Rows[i]["Y1pt"].ToString());

				float x2 = Convert.ToSingle(undoPoints.Rows[i]["X2pt"].ToString());

				float y2 = Convert.ToSingle(undoPoints.Rows[i]["Y2pt"].ToString());

				if (undodirect == "N" || undodirect == "S" || undodirect == "E" || undodirect == "W")
				{
					if (x1 == x2)
					{
						redist = Math.Abs((y1 - y2) / _currentScale);
					}
					if (y1 == y2)
					{
						redist = Math.Abs((x1 - x2) / _currentScale);
					}
				}
				if (undodirect == "NE" || undodirect == "SE" || undodirect == "NW" || undodirect == "SW")
				{
					float x1f = Math.Abs((x1 - x2) / _currentScale);
					float y1f = Math.Abs((y1 - y2) / _currentScale);

					distanceD = Convert.ToInt32(Math.Sqrt(x1f + y1f));

					decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(x1f + y1f)), 2);

					distance = Convert.ToDecimal(distanceD1);

					AngD1 = Convert.ToDecimal(x1f);
					AngD2 = Convert.ToDecimal(y1f);
				}

				Graphics g = Graphics.FromImage(_mainimage);
				g.Save();

				StartX = x1;
				StartY = y1;

				ExpSketchPBox.Image = _mainimage;
				click++;
				savpic.Add(click, imageToByteArray(_mainimage));

				if (undoPoints.Rows[i]["Direct"].ToString() == "E")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, (StartX + (redist * _currentScale)), StartY);

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "N")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, StartX, (StartY - (redist * _currentScale)));

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "S")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, StartX, (StartY + (redist * _currentScale)));

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "W")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, (StartX - (redist * _currentScale)), StartY);

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "NW")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "NE")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "SW")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
				if (undoPoints.Rows[i]["Direct"].ToString() == "SE")
				{
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1x = new Pen(Color.Red, 2);
					Pen pen1w = new Pen(Color.White, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

					ExpSketchPBox.Image = _mainimage;
					click++;
					savpic.Add(click, imageToByteArray(_mainimage));
				}
			}

			ExpSketchPBox.Image = _mainimage;
			click++;
			savpic.Add(click, imageToByteArray(_mainimage));
		}

		public void RefreshSketch()
		{
			ExpSketchPBox.Refresh();
			_mainimage = null;

			_mainimage = _currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height,
				1000, 572, 400, out _scale);
			_currentScale = _scale;

			Graphics g = Graphics.FromImage(_mainimage);
			SolidBrush Lblbrush = new SolidBrush(Color.Black);
			SolidBrush FillBrush = new SolidBrush(Color.White);
			Pen whitePen = new Pen(Color.White, 2);
			Pen blackPen = new Pen(Color.Black, 2);

			Font LbLf = new Font("Arial", 10, FontStyle.Bold);
			Font TitleF = new Font("Arial", 10, FontStyle.Bold | FontStyle.Underline);
			Font MainTitle = new System.Drawing.Font("Arial", 15, FontStyle.Bold | FontStyle.Underline);
			char[] leadzero = new char[] { '0' };

			g.DrawString(Locality, TitleF, Lblbrush, new PointF(10, 10));
			g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
			g.DrawString(String.Format("Record # - {0}", SketchRecord.TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
			g.DrawString(String.Format("Card # - {0}", SketchCard), LbLf, Lblbrush, new PointF(10, 45));

			g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));

			ExpSketchPBox.Image = _mainimage;

			if (_closeSketch == true)
			{
				this.Close();
			}
		}

		private void ReOpenSec()
		{
			int rowindex = 0;

			DataSet rolines = null;

			StringBuilder getLine = new StringBuilder();
			getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
			getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
			getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
					   MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.mrecno,
						_currentParcel.mdwell,
						MainForm.reopenSec));

			rolines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());

			int maxsecline = rolines.Tables[0].Rows.Count;
			if (rolines.Tables[0].Rows.Count > 0)
			{
				REJumpTable.Clear();

				for (int i = 0; i < rolines.Tables[0].Rows.Count; i++)
				{
					decimal Distance = 0;

					DataRow row = REJumpTable.NewRow();
					row["Record"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlrecord"].ToString());
					row["Card"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jldwell"].ToString());
					row["Sect"] = rolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["LineNo"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());
					row["Direct"] = rolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["XLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlxlen"].ToString());
					row["YLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlylen"].ToString());
					row["Length"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jllinelen"].ToString());
					row["Angle"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlangle"].ToString());
					row["XPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["YPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["XPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["YPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
					row["Attach"] = rolines.Tables[0].Rows[i]["jlattach"].ToString();

					decimal xpt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
					decimal ypt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2y"].ToString());

					float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
					float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

					rowindex = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());

					_StartX.Add(rowindex, xPoint);

					_StartY.Add(rowindex, yPoint);

					REJumpTable.Rows.Add(row);
				}

				float _JumpXT = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale));

				float _JumpX = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
				float _JumpY = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["YPT2"].ToString()) * _currentScale));

				JumpX = _JumpX;
				JumpY = _JumpY;

				GetStartCorner();
			}
		}

		private void Reorder()
		{
			Garcnt = 0;
			GarSize = 0;
			CPcnt = 0;
			CPSize = 0;

			int tg = _currentParcel.mgart;

			int tg2 = _currentParcel.mgart2;

			int tc = _currentParcel.mcarpt;

			StringBuilder getSect = new StringBuilder();
			getSect.Append(String.Format("select jsrecord,jsdwell,jssect,jstype,jssqft from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
							  MainForm.FClib, MainForm.FCprefix, _currentParcel.Record, _currentParcel.Card));
			getSect.Append(" order by jssect ");

			DataSet ds = dbConn.DBConnection.RunSelectStatement(getSect.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					DataRow row = SectionLtrs.NewRow();
					row["RecNo"] = _currentParcel.Record;
					row["CardNo"] = _currentParcel.Card;

					//row["CurSecLtr"] = ds.Tables[0].Rows[i]["jssect"].ToString();
					//row["NewSecLtr"] = Letters[i].ToString();
					row["CurSecLtr"] = CurrentSecLtr;
					row["NewSecLtr"] = _nextSectLtr;
					row["NewType"] = ds.Tables[0].Rows[i]["jstype"].ToString();
					row["SectSize"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());

					SectionLtrs.Rows.Add(row);

					if (CamraSupport.GarageTypes.Contains(ds.Tables[0].Rows[i]["jstype"].ToString().Trim()))
					{
						Garcnt++;

						GarSize = Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());
					}
					if (CamraSupport.CarPortTypes.Contains(ds.Tables[0].Rows[i]["jstype"].ToString().Trim()))
					{
						CPcnt++;

						CPSize = CPSize + Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());
					}
				}
			}

			if (Garcnt == 0)
			{
				StringBuilder zerogar = new StringBuilder();
				zerogar.Append(String.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0,mgart2 = 0,mgar#2 = 0 where mrecno = {2} and mdwell = {3} ",
										MainForm.localLib,
										MainForm.localPreFix,
										_currentParcel.mrecno,
										_currentParcel.mdwell));

				dbConn.DBConnection.ExecuteNonSelectStatement(zerogar.ToString());

				ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
			}
			if (CPcnt == 0)
			{
				StringBuilder zerocp = new StringBuilder();
				zerocp.Append(String.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
										MainForm.localLib,
										MainForm.localPreFix,
										_currentParcel.mrecno,
										_currentParcel.mdwell));

				dbConn.DBConnection.ExecuteNonSelectStatement(zerocp.ToString());

				ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
			}

			if (Garcnt > 0)
			{
				if (Garcnt == 1 && _currentParcel.mgart <= 60 || Garcnt == 1 && _currentParcel.mgart == 63 || Garcnt == 1 && _currentParcel.mgart == 64)
				{
					MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, GarSize, "GAR");
					missGar.ShowDialog();

					if (MissingGarageData.GarCode != _currentParcel.orig_mgart)
					{
						StringBuilder fixCp = new StringBuilder();
						fixCp.Append(String.Format("update {0}.{1}mast set mgart = {2},mgar#c = {3},mgart2 = 0,mgar#2 = 0 ",
						  MainForm.localLib,
							  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							MissingGarageData.GarCode,
							MissingGarageData.GarNbr));
						fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

						dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

						ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
					}
				}
				if (Garcnt > 1 && _currentParcel.mgart2 == 0)
				{
					MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, GarSize, "GAR");
					missGar.ShowDialog();

					if (MissingGarageData.GarCode != _currentParcel.orig_mgart2)
					{
						StringBuilder fixCp = new StringBuilder();
						fixCp.Append(String.Format("update {0}.{1}mast set mgart2 = {2},mgar#2 = {3} ",
						  MainForm.localLib,
							  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							MissingGarageData.GarCode,
							MissingGarageData.GarNbr));
						fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

						dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

						ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
					}
				}
				if (Garcnt > 2)
				{
					MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, GarSize, "GAR");
					missGar.ShowDialog();

					int newgarcnt = _currentParcel.mgarN2 + MissingGarageData.GarNbr;

					StringBuilder addcp = new StringBuilder();
					addcp.Append(String.Format("update {0}.{1}mast set mgar#2 = {2} where mrecno = {3} and mdwell = {4} ",
							MainForm.localLib,
							MainForm.localPreFix,
							newgarcnt,
							_currentParcel.mrecno,
							_currentParcel.mdwell));

					dbConn.DBConnection.ExecuteNonSelectStatement(addcp.ToString());

					ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
				}
			}
			if (CPcnt > 0)
			{
				if (CPcnt > 0 && _currentParcel.mcarpt == 0 || CPcnt > 0 && _currentParcel.mcarpt == 67)
				{
					MissingGarageData missCP = new MissingGarageData(dbConn, _currentParcel, CPSize, "CP");
					missCP.ShowDialog();

					if (MissingGarageData.CPCode != _currentParcel.orig_mcarpt)
					{
						StringBuilder fixCp = new StringBuilder();
						fixCp.Append(String.Format("update {0}.{1}mast set mcarpt = {2},mcar#c = {3} ",
						   MainForm.localLib,
							  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							MissingGarageData.CPCode,
							MissingGarageData.CpNbr));
						fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

						dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

						ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
					}
				}

				if (CPcnt > 1 && _currentParcel.mcarpt != 0 || CPcnt > 1 && _currentParcel.mcarpt != 67)
				{
					MissingGarageData missCPx = new MissingGarageData(dbConn, _currentParcel, CPSize, "CP");
					missCPx.ShowDialog();

					int newcpcnt = _currentParcel.mcarNc + MissingGarageData.CpNbr;

					StringBuilder addcp = new StringBuilder();
					addcp.Append(String.Format("update {0}.{1}mast set mcar#c = {2} where mrecno = {3} and mdwell = {4} ",
							MainForm.localLib,
							MainForm.localPreFix,
							newcpcnt,
							_currentParcel.mrecno,
							_currentParcel.mdwell));

					dbConn.DBConnection.ExecuteNonSelectStatement(addcp.ToString());

					ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
				}
			}

			for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
			{
				StringBuilder fixSect = new StringBuilder();
				fixSect.Append(String.Format("update {0}.{1}section set jssect = '{2}' where jsrecord = {3} and jsdwell = {4} ", MainForm.FClib, MainForm.FCprefix,
					SectionLtrs.Rows[j]["NewSecLtr"].ToString().Trim(), _currentParcel.Record, _currentParcel.Card));
				fixSect.Append(String.Format(" and jssect = '{0}' ", SectionLtrs.Rows[j]["CurSecLtr"].ToString().Trim()));

				//fox.DBConnection.ExecuteNonSelectStatement(fixSect.ToString());
			}

			string newLineLtr = String.Empty;
			string oldLineLtr = String.Empty;
			for (int k = 0; k < SectionLtrs.Rows.Count; k++)
			{
				//newLineLtr = SectionLtrs.Rows[k]["NewSecLtr"].ToString().Trim();

				//oldLineLtr = SectionLtrs.Rows[k]["CurSecLtr"].ToString().Trim();

				newLineLtr = _nextSectLtr.Trim();

				oldLineLtr = CurrentSecLtr.Trim();

				//upDlineLtr(newLineLtr, oldLineLtr);
			}
		}

		private void SaveJumpPointsAndOldSectionEndPoints(float CurrentScale, int rowindex, DataView SortedJumpTableDataView)
		{
			try
			{
				float _JumpX = (ScaleBaseX + (Convert.ToSingle(JumpTable.Rows[rowindex]["XPt2"].ToString()) * CurrentScale)); //  change XPt1 to XPt2
				float _JumpY = (ScaleBaseY + (Convert.ToSingle(JumpTable.Rows[rowindex]["YPT2"].ToString()) * CurrentScale));

				JumpX = (ScaleBaseX + (Convert.ToSingle(SortedJumpTableDataView[0]["XPt2"].ToString()) * CurrentScale));  //  change XPt1 to XPt2
				JumpY = (ScaleBaseY + (Convert.ToSingle(SortedJumpTableDataView[0]["YPt2"].ToString()) * CurrentScale));

				float _endOldSecX = (ScaleBaseX + (Convert.ToSingle(JumpTable.Rows[rowindex]["XPt1"].ToString()) * CurrentScale));//   change XPt2 to XPt1
				float _endOldSecY = (ScaleBaseY + (Convert.ToSingle(JumpTable.Rows[rowindex]["YPt1"].ToString()) * CurrentScale)); // ScaleBaseY was ScaleBaseX ??

				endOldSecX = (ScaleBaseX + (Convert.ToSingle(SortedJumpTableDataView[0]["XPt2"].ToString()) * CurrentScale));//   change XPt2 to XPt1
				endOldSecY = (ScaleBaseY + (Convert.ToSingle(SortedJumpTableDataView[0]["YPt2"].ToString()) * CurrentScale));  // ScaleBaseY was ScaleBaseX ??
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		private void SaveSketchData()
		{
			if (this.isInAddNewPointMode)
			{
				if (this.isLastLine)
				{
					this.section.SectionLines.TrimExcess();
					int lastLine = this.section.SectionLines.Count;
					int lastRow = this.dt.Rows.Count - 1;

					var prevLine = this.section.SectionLines[lastLine];
					prevLine.YLength = Convert.ToDecimal(this.dt.Rows[lastRow]["North"].ToString());
					prevLine.XLength = Convert.ToDecimal(this.dt.Rows[lastRow]["East"].ToString());
					prevLine.Point1X = Convert.ToDecimal(this.unadj_pts[lastRow].X);
					prevLine.Point1Y = Convert.ToDecimal(this.unadj_pts[lastRow].Y);
					prevLine.Point2X = Convert.ToDecimal(this.unadj_pts[0].X);
					prevLine.Point2Y = Convert.ToDecimal(this.unadj_pts[0].Y);
					prevLine.Update();

					this.section.SectionLines[lastLine].IncrementLineNumber();

					var newLine = new BuildingLine();
					newLine.Record = this.section.Record;
					newLine.Card = this.section.Card;
					newLine.SectionLetter = this.section.SectionLetter;
					newLine.LineNumber = lastLine;
					newLine.Directional = this.dt.Rows[lastRow - 1]["Dir"].ToString();
					newLine.YLength = Convert.ToDecimal(this.dt.Rows[lastRow - 1]["North"].ToString());
					newLine.XLength = Convert.ToDecimal(this.dt.Rows[lastRow - 1]["East"].ToString());
					newLine.Point1X = Convert.ToDecimal(this.unadj_pts[lastRow - 1].X);
					newLine.Point1Y = Convert.ToDecimal(this.unadj_pts[lastRow - 1].Y);
					newLine.Point2X = Convert.ToDecimal(this.unadj_pts[lastRow].X);
					newLine.Point2Y = Convert.ToDecimal(this.unadj_pts[lastRow].Y);
					newLine.Insert();
				}
				else
				{
					var prevLine = this.section.SectionLines[NewPointIndex];
					prevLine.YLength = Convert.ToDecimal(this.dt.Rows[NewPointIndex]["North"].ToString());
					prevLine.XLength = Convert.ToDecimal(this.dt.Rows[NewPointIndex]["East"].ToString());
					prevLine.Point1X = Convert.ToDecimal(this.unadj_pts[NewPointIndex].X);
					prevLine.Point1Y = Convert.ToDecimal(this.unadj_pts[NewPointIndex].Y);
					prevLine.Point2X = Convert.ToDecimal(this.unadj_pts[NewPointIndex + 1].X);
					prevLine.Point2Y = Convert.ToDecimal(this.unadj_pts[NewPointIndex + 1].Y);
					prevLine.Update();

					this.section.IncrementAllLines(this.NewPointIndex);

					var newLine = new BuildingLine();
					newLine.Record = this.section.Record;
					newLine.Card = this.section.Card;
					newLine.SectionLetter = this.section.SectionLetter;
					newLine.LineNumber = this.NewPointIndex;
					newLine.Directional = this.dt.Rows[NewPointIndex - 1]["Dir"].ToString();
					newLine.YLength = Convert.ToDecimal(this.dt.Rows[NewPointIndex - 1]["North"].ToString());
					newLine.XLength = Convert.ToDecimal(this.dt.Rows[NewPointIndex - 1]["East"].ToString());
					newLine.Point1X = Convert.ToDecimal(this.unadj_pts[NewPointIndex - 1].X);
					newLine.Point1Y = Convert.ToDecimal(this.unadj_pts[NewPointIndex - 1].Y);
					newLine.Point2X = Convert.ToDecimal(this.unadj_pts[NewPointIndex].X);
					newLine.Point2Y = Convert.ToDecimal(this.unadj_pts[NewPointIndex].Y);
					newLine.Insert();
				}

				this.SetAddNewPointButton(false);
			}
		}

		private void SetAddNewPointButton(bool enabled)
		{
			this.isInAddNewPointMode = enabled;
		}

		public void setAttPnts()
		{
			StringBuilder attPnts = new StringBuilder();
			attPnts.Append(String.Format("select jlrecord,jldwell,jlsect,jldirect,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach from {0}.{1}line ",
						  MainForm.localLib,
						  MainForm.localPreFix

							//MainForm.FClib,
							//MainForm.FCprefix
							));
			attPnts.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlline# = 1 and jlsect <> 'A' ", _currentParcel.Record, _currentParcel.Card));

			DataSet ap = dbConn.DBConnection.RunSelectStatement(attPnts.ToString());

			if (ap.Tables[0].Rows.Count > 0)
			{
				AttachPoints.Clear();

				for (int i = 0; i < ap.Tables[0].Rows.Count; i++)
				{
					DataRow row = AttachPoints.NewRow();
					row["RecNo"] = _currentParcel.Record;
					row["CardNo"] = _currentParcel.Card;
					row["Sect"] = ap.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["Direct"] = ap.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["Xpt1"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Ypt1"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["Xpt2"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Ypt2"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt2y"].ToString());
					row["Attch"] = ap.Tables[0].Rows[i]["jlattach"].ToString().Trim();

					AttachPoints.Rows.Add(row);
				}
			}

			if (AttachPoints.Rows.Count > 0)
			{
				//MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 4356", _currentParcel.Record, _currentParcel.Card));

				string jkw = _nextSectLtr;

				StringBuilder delapts = new StringBuilder();
				delapts.Append(String.Format("update {0}.{1}line set jlattach = ' ' where jlrecord = {2} and jldwell = {3} and jlattach = '{4}' ",
							   MainForm.localLib,
						  MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								_currentParcel.mrecno,
								_currentParcel.mdwell,
								_nextSectLtr));

				dbConn.DBConnection.ExecuteNonSelectStatement(delapts.ToString());

				if (MultiSectionSelection.adjsec == String.Empty)
				{
					ConnectSec = "A";
				}
				if (MultiSectionSelection.adjsec != String.Empty)
				{
					ConnectSec = MultiSectionSelection.adjsec;
				}

				for (int i = 0; i < AttachPoints.Rows.Count; i++)
				{
					int record = Convert.ToInt32(AttachPoints.Rows[i]["RecNo"].ToString());
					int card = Convert.ToInt32(AttachPoints.Rows[i]["CardNo"].ToString());
					string curSect = AttachPoints.Rows[i]["Sect"].ToString().Trim();
					decimal X1 = Convert.ToDecimal(AttachPoints.Rows[i]["Xpt1"].ToString());
					decimal Y1 = Convert.ToDecimal(AttachPoints.Rows[i]["Ypt1"].ToString());
					decimal X2 = Convert.ToDecimal(AttachPoints.Rows[i]["Xpt2"].ToString());
					decimal Y2 = Convert.ToDecimal(AttachPoints.Rows[i]["Ypt2"].ToString());

					//MessageBox.Show(String.Format("Update Line Record - {0}, Card - {1} at 4373", _currentParcel.Record, _currentParcel.Card));

					if (curSect == _nextSectLtr)
					{
						ConnectSec = CurrentSecLtr;
					}

					StringBuilder addAttPnt = new StringBuilder();
					addAttPnt.Append(String.Format("update {0}.{1}line set jlattach = '{2}' ",
								   MainForm.localLib,
								   MainForm.localPreFix,

									//MainForm.FClib,
									//MainForm.FCprefix,
									curSect));
					addAttPnt.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlpt2x = {2} and jlpt2y = {3} ", record, card, X1, Y1));
					addAttPnt.Append(String.Format(" and jlsect <> '{0}' ", curSect));
					addAttPnt.Append(String.Format(" and jlsect = '{0}' ", ConnectSec));
					addAttPnt.Append(" and jlattach = ' ' ");

					dbConn.DBConnection.ExecuteNonSelectStatement(addAttPnt.ToString());
				}
			}

			/// start nogo here

			//StringBuilder DupattPnts = new StringBuilder();
			//DupattPnts.Append(String.Format("select jlrecord,jldwell,jlsect,jldirect,jlline#,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach from {0}.{1}line ",
			//                MainForm.FClib, MainForm.FCprefix));
			//DupattPnts.Append(String.Format(" where jlrecord = {0} and jldwell = {1}  and jlattach <> ' ' ", _currentParcel.Record, _currentParcel.Card));
			//DupattPnts.Append("order by jlattach,jlsect  ");

			//DataSet Dupap = fox.DBConnection.RunSelectStatement(DupattPnts.ToString());

			//if (Dupap.Tables[0].Rows.Count > 0)
			//{
			//    DupAttPoints.Clear();

			//    for (int j = 0; j < Dupap.Tables[0].Rows.Count; j++)
			//    {
			//        DataRow row = DupAttPoints.NewRow();
			//        row["RecNo"] = _currentParcel.Record;
			//        row["CardNo"] = _currentParcel.Card;
			//        row["Sect"] = Dupap.Tables[0].Rows[j]["jlsect"].ToString().Trim();
			//        row["LineNo"] = Convert.ToInt32(Dupap.Tables[0].Rows[j]["jlline#"].ToString());
			//        row["Direct"] = Dupap.Tables[0].Rows[j]["jldirect"].ToString().Trim();
			//        row["Xpt1"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt1x"].ToString());
			//        row["Ypt1"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt1y"].ToString());
			//        row["Xpt2"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt2x"].ToString());
			//        row["Ypt2"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt2y"].ToString());
			//        row["Attch"] = Dupap.Tables[0].Rows[j]["jlattach"].ToString().Trim();
			//        row["Index"] = 1;

			//        DupAttPoints.Rows.Add(row);

			//    }

			//}

			//attList = new List<string>();
			//attList.Clear();

			//StringBuilder sortAtt = new StringBuilder();
			//sortAtt.Append(String.Format("select jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlattach <> ' ' ",
			//               MainForm.FClib,
			//               MainForm.FCprefix,
			//               _currentParcel.Record,
			//               _currentParcel.Card));

			//DataSet sa = fox.DBConnection.RunSelectStatement(sortAtt.ToString());

			//if (sa.Tables[0].Rows.Count > 0)
			//{
			//    for (int i = 0; i < sa.Tables[0].Rows.Count; i++)
			//    {
			//        attList.Add(sa.Tables[0].Rows[i]["jlattach"].ToString().Trim());
			//    }

			//}

			////MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 4467", _currentParcel.Record, _currentParcel.Card));

			//StringBuilder delAtt = new StringBuilder();
			//delAtt.Append(String.Format("update {0}.{1}line set jlattach = ' ' where jlrecord = {2} and jldwell = {3} ",
			//               MainForm.FClib,
			//               MainForm.FCprefix,
			//               _currentParcel.Record,
			//               _currentParcel.Card));

			////fox.DBConnection.ExecuteNonSelectStatement(delAtt.ToString());

			//if (AttPts.Rows.Count > 0)
			//{
			//    for (int i = 0; i < AttPts.Rows.Count; i++)
			//    {
			//        int record = _currentParcel.Record;
			//        int card = _currentParcel.Card;
			//        string curSection = AttPts.Rows[i]["Sect"].ToString().Trim();
			//        decimal X1 = Convert.ToDecimal(AttPts.Rows[i]["X1"].ToString());
			//        decimal Y1 = Convert.ToDecimal(AttPts.Rows[i]["Y1"].ToString());
			//        decimal X2 = Convert.ToDecimal(AttPts.Rows[i]["X2"].ToString());
			//        decimal Y2 = Convert.ToDecimal(AttPts.Rows[i]["Y2"].ToString());

			//        // MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} atg 4492", _currentParcel.Record, _currentParcel.Card));

			//        StringBuilder addAttPnt1 = new StringBuilder();
			//        addAttPnt1.Append(String.Format("update {0}.{1}line set jlattach = '{2}' ", MainForm.FClib, MainForm.FCprefix, curSection));
			//        addAttPnt1.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlpt1x = {2} and jlpt1y = {3} ", record, card, X1, Y1));
			//        addAttPnt1.Append(String.Format(" and jlpt2x = {0} and jlpt2y = {1} ", X2, Y2));
			//        addAttPnt1.Append(String.Format(" and jlsect <> '{0}' ", curSection));
			//        addAttPnt1.Append("and jlattach = ' ' ");

			//        //fox.DBConnection.ExecuteNonSelectStatement(addAttPnt1.ToString());

			//    }

			//}

			//if (DupAttPoints.Rows.Count > 0)
			//{
			//    for (int i = 0; i < DupAttPoints.Rows.Count; i++)
			//    {
			//        int record = _currentParcel.Record;
			//        int card = _currentParcel.Card;
			//        string curSect = DupAttPoints.Rows[i]["Attch"].ToString().Trim();
			//        string curSection = DupAttPoints.Rows[i]["Sect"].ToString().Trim();
			//        decimal X1 = Convert.ToDecimal(DupAttPoints.Rows[i]["Xpt1"].ToString());
			//        decimal Y1 = Convert.ToDecimal(DupAttPoints.Rows[i]["Ypt1"].ToString());
			//        decimal X2 = Convert.ToDecimal(DupAttPoints.Rows[i]["Xpt2"].ToString());
			//        decimal Y2 = Convert.ToDecimal(DupAttPoints.Rows[i]["Ypt2"].ToString());

			//        if (Convert.ToInt32(DupAttPoints.Rows[i]["Index"].ToString()) == 1)
			//        {
			//            //MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 4527", _currentParcel.Record, _currentParcel.Card));

			//            StringBuilder addAttPnt = new StringBuilder();
			//            addAttPnt.Append(String.Format("update {0}.{1}line set jlattach = '{2}' ", MainForm.FClib, MainForm.FCprefix, curSect));
			//            addAttPnt.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlpt1x = {2} and jlpt1y = {3} ", record, card, X1, Y1));
			//            addAttPnt.Append(String.Format(" and jlpt2x = {0} and jlpt2y = {1} ", X2, Y2));
			//            addAttPnt.Append(String.Format(" and jlsect = '{0}' ", curSection));
			//            addAttPnt.Append("and jlattach = ' ' ");

			//            fox.DBConnection.ExecuteNonSelectStatement(addAttPnt.ToString());

			//        }
			//    }
			//}
		}

		public void ShowDistanceForm(string _closeY, decimal _ewDist, string _closeX, decimal _nsDist, bool openForm)
		{
			if (openForm == true)
			{
				SketchDistance sketDist = new SketchDistance(_closeY, _ewDist, _closeX, _nsDist);
				sketDist.Show(this);

				sketDist.Close();
			}
		}

		private void ShowMessageBox(string s)
		{
			MessageBox.Show(s);
		}

		//private void upDlineLtr(string newLtr, string old)
		//{
		//    StringBuilder fixLine = new StringBuilder();
		//    fixLine.Append(String.Format("update {0}.{1}line set jlsect = '{2}' where jlrecord = {3} and jldwell = {4} ", MainForm.FClib, MainForm.FCprefix,
		//         newLtr, _currentParcel.Record, _currentParcel.Card));
		//    fixLine.Append(String.Format(" and jlsect = '{0}' ", old));

		//    fox.DBConnection.ExecuteNonSelectStatement(fixLine.ToString());

		//}

		private void sortSection()
		{
			FixSect = new List<string>();

			StringBuilder addFix = new StringBuilder();
			addFix.Append(String.Format("select jlsect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = 1 ",
					  MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.Record,
						_currentParcel.Card));

			DataSet fs = dbConn.DBConnection.RunSelectStatement(addFix.ToString());

			if (fs.Tables[0].Rows.Count > 0)
			{
				FixSect.Clear();

				for (int i = 0; i < fs.Tables[0].Rows.Count; i++)
				{
					FixSect.Add(fs.Tables[0].Rows[i]["jlsect"].ToString());
				}

				if (FixSect.Count > 0)
				{
					sortDist.Clear();

					StringBuilder chkLen = new StringBuilder();
					chkLen.Append("select jlsect,jlline#,jldirect, case when jldirect in ( 'N','S') then abs(jlpt1y-jlpt2y) when jldirect in ( 'E','W') then abs(jlpt1x-jlpt2x) ");
					chkLen.Append("when jldirect in ( 'NE','SE','NW','SW') then sqrt(abs(jlpt1y-jlpt2y)*abs(jlpt1y-jlpt2y)+abs(jlpt1x-jlpt2x)*abs(jlpt1x-jlpt2x)) ");
					chkLen.Append(String.Format("end as LineLen, abs(jlpt1x-jlpt2x) as Xlen, abs(jlpt1y-jlpt2y) as Ylen from {0}.{1}line ",
								  MainForm.localLib,
						  MainForm.localPreFix

									//MainForm.FClib,
									//MainForm.FCprefix
									));
					chkLen.Append(String.Format("where jlrecord = {0} and jldwell = {1} order by jlsect,jlline# ", _currentParcel.Record, _currentParcel.Card));

					DataSet fixl = dbConn.DBConnection.RunSelectStatement(chkLen.ToString());

					if (fixl.Tables[0].Rows.Count > 0)
					{
						for (int i = 0; i < fixl.Tables[0].Rows.Count; i++)
						{
							//MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 3177", _currentParcel.Record, _currentParcel.Card));

							StringBuilder updLine = new StringBuilder();
							updLine.Append(String.Format("update {0}.{1}line set jlxlen = {2},jlylen = {3},jllinelen = {4} ",
										   MainForm.localLib,
										   MainForm.localPreFix,

											//MainForm.FClib,
											//MainForm.FCprefix,
											Convert.ToDecimal(fixl.Tables[0].Rows[i]["Xlen"].ToString()),
											Convert.ToDecimal(fixl.Tables[0].Rows[i]["Ylen"].ToString()),
											Convert.ToDecimal(fixl.Tables[0].Rows[i]["LineLen"].ToString())));
							updLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
									_currentParcel.Record,
									_currentParcel.Card,
									fixl.Tables[0].Rows[i]["jlsect"].ToString(),
									Convert.ToInt32(fixl.Tables[0].Rows[i]["jlline#"].ToString())));

							dbConn.DBConnection.ExecuteNonSelectStatement(updLine.ToString());
						}
					}
				}
			}
		}

		public void SplitLine()
		{
			int ln = _savedAttLine;
			string asec = _savedAttSection;
			string newSec = _nextSectLtr;
			string tstdir = CurrentAttDir;
			decimal begx = OrigStartX;
			decimal begy = OrigStartY;
			decimal OldbegX = startSplitX;
			decimal OldbegY = startSplitY;
			decimal OldbegX1 = OrigStartX;
			decimal OldbegY1 = OrigStartY;
			float oldx = endOldSecX;
			float oldy = endOldSecY;
			string splitLineDir = offsetDir;
			string CurAttDir = String.Empty;
			string NextAttDir = String.Empty;
			decimal OrigY = OrigStartY;
			decimal newEndX = 0;
			decimal newEndY = 0;
			decimal newEndX1 = EndSplitX;
			decimal newEndY1 = EndSplitY;

			//TODO: See if setting this to absolute value will correct the problem.
			decimal finalNewLen = (OrigLineLength - splitLineDist);

			float mySx = BeginSplitX;

			float mySy = BeginSplitY;

			RemainderLineLength = finalNewLen;

			JumpTable = new DataTable();

			int chekattline = TempAttSplineNo;

			_savedAttLine = AttSpLineNo;
			if (AttSpLineNo == 0)
			{
				_savedAttLine = TempAttSplineNo;
				AttSpLineNo = TempAttSplineNo;
			}

			if (MultiSectionSelection.adjsec != String.Empty)
			{
				_savedAttSection = MultiSectionSelection.adjsec;
			}

			StringBuilder sectable = new StringBuilder();
			sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
			sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
						   MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card,
							_savedAttSection));

			DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

			if (scl.Tables[0].Rows.Count > 0)
			{
				SectionTable.Clear();

				for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
				{
					DataRow row = SectionTable.NewRow();
					row["Record"] = _currentParcel.mrecno;
					row["Card"] = _currentParcel.mdwell;
					row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
					row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
					row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
					row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
					row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
					row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
					row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

					SectionTable.Rows.Add(row);
				}
			}

			CurrentAttDir = offsetDir;

			// TODO: Remove if not needed:	string tstOffset = offsetDir;

			CurAttDir = CurrentAttDir;

			if (CurAttDir.Trim() != offsetDir.Trim())
			{
				string getNCurDir = string.Format("select jldirect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' and jlline#= {5} ", MainForm.localLib, MainForm.localPreFix, _currentParcel.Record, _currentParcel.Card, _savedAttSection, AttSpLineNo);

				CurrentAttDir = dbConn.DBConnection.ExecuteScalar(getNCurDir.ToString()).ToString();
			}

			_savedAttLine = AttSpLineNo;

			if (_savedAttLine == 0)
			{
			}

			if (CurAttDir.Trim() != offsetDir.Trim())
			{
				StringBuilder nxtAttDir = new StringBuilder();
				nxtAttDir.Append(String.Format("select jldirect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' and jlline# = {5} ",
							   MainForm.localLib,
						  MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								_currentParcel.mrecno,
								_currentParcel.mdwell,
								CurrentSecLtr,
								_savedAttLine));

				NextAttDir = dbConn.DBConnection.ExecuteScalar(nxtAttDir.ToString()).ToString();
			}

			decimal myoriglen = 0;
			mylineNo = 0;

			if (CurAttDir.Trim() == offsetDir.Trim())
			{
				StringBuilder getoriglen = new StringBuilder();
				getoriglen.Append(String.Format("select jllinelen,jlline# from {0}.{1}line where jlrecord = {2} and jldwell = {3}  and jlsect = '{4}' and jlline# = {5}",
						  MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							CurrentSecLtr,
							_savedAttLine));
				getoriglen.Append(String.Format(" and jldirect = '{0}'", offsetDir));

				DataSet myds = dbConn.DBConnection.RunSelectStatement(getoriglen.ToString());

				if (myds.Tables[0].Rows.Count > 0)
				{
					myoriglen = Convert.ToDecimal(myds.Tables[0].Rows[0]["jllinelen"].ToString());
					mylineNo = Convert.ToInt32(myds.Tables[0].Rows[0]["jlline#"].ToString());
				}
			}

			int maxLineCnt = 0;

			decimal origEndx = 0;
			decimal origEndy = 0;
			decimal origDist = 0;
			decimal newDistX = 0;
			decimal newDistY = 0;
			decimal StartEndX = 0;
			decimal StartEndY = 0;
			decimal EndEndX = 0;
			decimal EndEndY = 0;
			decimal origLen = 0;
			decimal origXlen = 0;
			decimal origYlen = 0;
			decimal NewSplitDist = 0;

			StringBuilder getOrigEnds = new StringBuilder();
			getOrigEnds.Append(String.Format("select jldirect,jlsect,jlline#,jlxlen,jlylen,jllinelen,jlpt1x,jlpt1y,jlpt2x,jlpt2y from {0}.{1}line ",
						  MainForm.localLib,
						  MainForm.localPreFix

							//MainForm.FClib,
							//MainForm.FCprefix
							));
			getOrigEnds.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
				_currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr, mylineNo));

			DataSet orgLin = dbConn.DBConnection.RunSelectStatement(getOrigEnds.ToString());

			if (orgLin.Tables[0].Rows.Count > 0)
			{
				origEndx = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2x"].ToString());
				origEndy = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2y"].ToString());

				endOldSecX = Convert.ToSingle(origEndx);
				endOldSecY = Convert.ToSingle(origEndy);

				StartEndX = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt1x"].ToString());
				StartEndY = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt1y"].ToString());

				EndEndX = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2x"].ToString());
				EndEndY = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2y"].ToString());

				CurrentAttDir = orgLin.Tables[0].Rows[0]["jldirect"].ToString().Trim();
				origLen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jllinelen"].ToString());

				origXlen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlxlen"].ToString());
				origYlen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlylen"].ToString());

				origDist = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jllinelen"].ToString());

				NewSplitLIneDist = origLen - splitLineDist;
			}

			NewSplitDist = Math.Abs(myoriglen - splitLineDist);

			decimal origDistX = OriginalDistanceX();

			if (origDistX != myoriglen)
			{
				origDistX = myoriglen;
			}

			maxLineCnt = MaximumLineCount();

			if (maxLineCnt > 0 && mylineNo <= maxLineCnt)
			{
				decimal ttue = splitLineDist;
				StringBuilder incrLine = new StringBuilder();
				incrLine.Append(String.Format("update {0}.{1}line set jlline# = jlline#+1 ",
						  MainForm.localLib, MainForm.localPreFix));
				incrLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# > {3} ", _currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr, _savedAttLine));

				//Ask Dave why this is not excecuting. Can this whole section be removed?
				//fox.DBConnection.ExecuteNonSelectStatement(incrLine.ToString());

				if (_savedAttLine > 0 && RemainderLineLength != 0)
				{
					for (int i = _savedAttLine - 1; i < SectionTable.Rows.Count; i++)
					{
						int tske = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());

						SectionTable.Rows[i]["LineNo"] = tske + 1;
					}
				}

				DeleteLineSection();

				for (int i = 0; i < SectionTable.Rows.Count; i++)
				{
					StringBuilder instLine = new StringBuilder();
					instLine.Append(String.Format("insert into {0}.{1}line ",
								  MainForm.localLib, MainForm.localPreFix));
					instLine.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
										Convert.ToInt32(SectionTable.Rows[i]["Record"].ToString()),
										Convert.ToInt32(SectionTable.Rows[i]["Card"].ToString()),
										SectionTable.Rows[i]["Sect"].ToString().Trim(),
										Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()),
										SectionTable.Rows[i]["Direct"].ToString().Trim(),
										Convert.ToDecimal(SectionTable.Rows[i]["Xlen"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["Ylen"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["Length"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["Angle"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["XPt1"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["YPt1"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["XPt2"].ToString()),
										Convert.ToDecimal(SectionTable.Rows[i]["YPt2"].ToString()),
										SectionTable.Rows[i]["Attach"].ToString().Trim()));

					decimal td1 = Convert.ToDecimal(SectionTable.Rows[i]["Length"].ToString());

					decimal td2 = Convert.ToDecimal(SectionTable.Rows[i]["XPt1"].ToString());

					decimal td3 = Convert.ToDecimal(SectionTable.Rows[i]["YPt1"].ToString());

					decimal td4 = Convert.ToDecimal(SectionTable.Rows[i]["XPt2"].ToString());

					decimal td5 = Convert.ToDecimal(SectionTable.Rows[i]["YPt2"].ToString());

					if (Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()) <= 20)
					{
						dbConn.DBConnection.ExecuteNonSelectStatement(instLine.ToString());
					}
					if (Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()) > 20)
					{
						MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
						break;
					}
				}
			}

			decimal xlen = 0;
			decimal ylen = 0;

			if (offsetDir == "N" || offsetDir == "S")  // offsetDir
			{
				xlen = 0;
				ylen = splitLineDist;
				newDistX = 0;
				newDistY = origDist - splitLineDist;
				adjNewSecX = 0;
				adjNewSecY = splitLineDist;
			}
			if (offsetDir == "E" || offsetDir == "W")   // offsetDir
			{
				xlen = splitLineDist;
				ylen = 0;
				newDistX = origDist - splitLineDist;
				newDistY = 0;
				adjNewSecY = 0;
				adjNewSecX = splitLineDist;
			}

			decimal x1t = adjNewSecX;
			decimal y1t = adjNewSecY;

			decimal splitLength = splitLineDist;

			if (OrigLineLength != (RemainderLineLength + splitLineDist))
			{
				OrigLineLength = (RemainderLineLength + splitLineDist);
			}

			if (CurAttDir.Trim() == offsetDir.Trim())
			{
				decimal utp = splitLength;

				decimal ttue = splitLineDist;
				StringBuilder fixOrigLine = new StringBuilder();
				fixOrigLine.Append(String.Format("update {0}.{1}line ",
						  MainForm.localLib,
						  MainForm.localPreFix

							//MainForm.FClib,
							//MainForm.FCprefix
							));
				fixOrigLine.Append(String.Format("set jlxlen = {0},jlylen = {1}, jllinelen = {2}, jlpt2x = {3}, jlpt2y = {4} ",
										adjNewSecX,
										adjNewSecY,
										splitLength,
										begNewSecX,
										begNewSecY));
				fixOrigLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
								_currentParcel.mrecno,
								_currentParcel.mdwell,
								CurrentSecLtr,
								_savedAttLine));

				//Ask Dave why this is not being called. Can it be removed?
				//fox.DBConnection.ExecuteNonSelectStatement(fixOrigLine.ToString());
			}

			if (CurAttDir.Trim() != offsetDir.Trim())
			{
				if (offsetDir == "N" || offsetDir == "S")
				{
					RemainderLineLength = OrigLineLength - adjNewSecY;
				}
				if (offsetDir == "W" || offsetDir == "E")
				{
					adjNewSecX = RemainderLineLength;
				}
				adjNewSecY = RemainderLineLength;

				if (RemainderLineLength > 0)
				{
					FixOrigLine();
				}
			}

			if (offsetDir == "N" || offsetDir == "S")
			{
				adjOldSecX = 0;
				adjOldSecY = RemainderLineLength;
			}

			if (offsetDir == "E" || offsetDir == "W")
			{
				adjOldSecY = 0;
				adjOldSecX = RemainderLineLength;
			}

			int newLineNbr = _savedAttLine + 1;

			if (CurAttDir.Trim() != splitLineDir.Trim())
			{
				splitLineDir = CurAttDir;
			}

			decimal endNewSecX = adjNewSecX;
			switch (AttSpLineDir)
			{
				case "E":
					newEndX = (OrigStartX + splitLineDist);
					newEndY = OrigStartY;
					break;

				case "W":
					endNewSecX = adjNewSecX * -1;
					newEndX = (OrigStartX - splitLineDist);
					newEndY = OrigStartY;
					break;

				case "N":
					newEndY = (OrigStartY - splitLineDist);
					newEndX = OrigStartX;
					break;

				case "S":

					EndSplitY = EndSplitY * -1;
					newEndY = (OrigStartY + splitLineDist);
					newEndX = OrigStartX;
					break;

				default:
					Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}. AttSpLineDir not in NEWS. ", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
					break;
			}

			if (RemainderLineLength > 0)
			{
				InsertLine(CurAttDir, newEndX, newEndY, StartEndX, StartEndY, splitLength);
			}

			decimal jjel = OrigLineLength;

			decimal finEndX = 0;
			decimal finEndY = 0;
			decimal finDist = 0;
			switch (CurrentAttDir)
			{
				case "N":

					finDist = (OrigStartY - splitLineDist);

					finDist = newDistY;
					break;

				case "S":

					finDist = (OrigStartY + splitLineDist);
					finDist = newDistY;
					break;

				case "E":

					finDist = (OrigStartX + splitLineDist);

					finDist = newDistX;
					break;

				case "W":

					finDist = (OrigStartX - splitLineDist);
					finDist = newDistX;
					break;

				default:
					Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}. CurrentAttDir not in NEWS.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
					break;
			}
			AdjustLine(newEndX, newEndY, newDistX, newDistY, EndEndX, EndEndY, finDist);
		}

		public void unDo(Dictionary<int, byte[]> curpic, int curclick)
		{
			savpic = curpic;
			findends();

			decimal tstx = Convert.ToDecimal(delStartX);

			decimal tsty = Convert.ToDecimal(delStartY);

			bool ttsmm = draw;

			string tstdir = _lastDir.Trim();

			float tstdist = distanceD;

			float tstdistNS = distanceDYF;

			float tstdistEW = distanceDXF;

			float tstNextx = NextStartX;

			float tstNexty = NextStartY;

			if (_lastDir.Trim() == "N")
			{
				// NextStartY = NextStartY + distanceD;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}
			if (_lastDir.Trim() == "S")
			{
				// NextStartY = NextStartY - distanceD;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}
			if (_lastDir.Trim() == "E")
			{
				// NextStartX = NextStartX - distanceD;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}
			if (_lastDir.Trim() == "W")
			{
				//NextStartX = NextStartX + distanceD;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}

			if (_lastDir.Trim() == "NW")
			{
				//NextStartX = NextStartX - distanceDXF;
				//NextStartY = NextStartY + distanceDYF;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}
			if (_lastDir.Trim() == "NE")
			{
				//NextStartX = NextStartX + distanceDXF;
				//NextStartY = NextStartY + distanceDYF;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}
			if (_lastDir.Trim() == "SE")
			{
				//NextStartX = NextStartX + distanceDXF;
				//NextStartY = NextStartY - distanceDYF;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}
			if (_lastDir.Trim() == "SW")
			{
				//NextStartX = NextStartX - distanceDXF;
				//NextStartY = NextStartY - distanceDYF;

				NextStartX = delStartX;
				NextStartY = delStartY;
			}

			click = curclick;

			int mxcnt = 0;
			if (checkRedraw == true && savcnt.Count > 0)
			{
				DeleteReview delrev = new DeleteReview(savpic, savcnt, curclick);
				delrev.ShowDialog();

				mxcnt = savcnt.Max();

				savcnt.Remove(mxcnt);
			}

			if (_undoLine == false)
			{
				_undoLine = true;
			}

			_undoLine = true;

			bool redraw = false;

			if (click > 0 && checkRedraw == true)
			{
				foreach (KeyValuePair<int, byte[]> pair in savpic)
				{
					if (pair.Key == mxcnt)
					{
						ms = pair.Value;
						redraw = true;
					}
				}
			}

			if (click > 0 && checkRedraw == false)
			{
				savpic.Remove(click--);
			}

			ClrX();

			int maxLineCnt = 0;

			bool nonextsect = false;

			if (draw != false)
			{
				BeginSectionBtn.BackColor = Color.Cyan;
				BeginSectionBtn.Text = "Begin";
			}

			if (MainForm.reopenSec != null)
			{
				_nextSectLtr = MainForm.reopenSec;

				NewSectionPoints.Clear();
				lineCnt = undoPoints.Rows.Count;

				int stxcnt = _StartX.Count;

				for (int i = 0; i < undoPoints.Rows.Count; i++)
				{
					float x1 = Convert.ToSingle(undoPoints.Rows[i]["X1pt"].ToString());

					float y1 = Convert.ToSingle(undoPoints.Rows[i]["Y1pt"].ToString());

					float x2 = Convert.ToSingle(undoPoints.Rows[i]["X2pt"].ToString());

					float y2 = Convert.ToSingle(undoPoints.Rows[i]["Y2pt"].ToString());

					float x1s = ((x1 - ScaleBaseX) / _currentScale);

					float y1s = ((y1 - ScaleBaseY) / _currentScale);

					float x2s = ((x2 - ScaleBaseX) / _currentScale);

					float y2s = ((y2 - ScaleBaseY) / _currentScale);

					NewSectionPoints.Add(new PointF(x1s, y1s));
				}
			}

			string thisnextSect = _nextSectLtr;

			if (_nextSectLtr == String.Empty)
			{
				_nextSectLtr = "A";
				nonextsect = true;
			}
			if (thisnextSect != "A" && nonextsect == true)
			{
				nonextsect = false;
			}

			StringBuilder delLine2 = new StringBuilder();
			delLine2.Append(String.Format("select max(jlline#) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
						   MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card,
							_nextSectLtr));

			try
			{
				maxLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(delLine2.ToString()));
			}
			catch
			{
			}

			if (nonextsect == false && _undoMode == true)
			{
				StringBuilder removelinex = new StringBuilder();
				removelinex.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = {4} and jlsect = '{5}'  ",
							  MainForm.localLib,
						  MainForm.localPreFix,

								 // MainForm.FClib,
								 // MainForm.FCprefix,
								 _currentParcel.Record,
								 _currentParcel.Card,
								 maxLineCnt,
								 _nextSectLtr));

				if (thisnextSect != String.Empty)
				{
					dbConn.DBConnection.ExecuteNonSelectStatement(removelinex.ToString());
					maxLineCnt--;
				}

				if (maxLineCnt <= 0)
				{
					StringBuilder remsec = new StringBuilder();
					remsec.Append(String.Format("delete from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ",
					  MainForm.localLib,
						  MainForm.localPreFix,

						//MainForm.FClib,
						//MainForm.FCprefix,
						_currentParcel.mrecno,
						_currentParcel.mdwell,
						_nextSectLtr));

					dbConn.DBConnection.ExecuteNonSelectStatement(remsec.ToString());
				}

				try
				{
					int tstmax = maxLineCnt;

					if (maxLineCnt < 0)
					{
						maxLineCnt = 0;
					}

					NewSectionPoints.RemoveAt(maxLineCnt);

					lineCnt--;

					float txtsx = _StartX[maxLineCnt];
					float txtsy = _StartY[maxLineCnt];

					int tstclk = click;

					if (redraw == false)
					{
						ms = savpic[click];
					}

					StartX = _StartX[maxLineCnt - 1];
					StartY = _StartY[maxLineCnt - 1];

					CalculateClosure(pt2X, pt2Y);

					_mainimage = byteArrayToImage(ms);
					ExpSketchPBox.Image = _mainimage;

					if (maxLineCnt == 0)
					{
						this.Close();
					}
				}
				catch (Exception)
				{
				}
			}

			if (nonextsect == false && _undoMode != true)
			{
				StringBuilder removeLine = new StringBuilder();
				removeLine.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = {4} and jlsect = '{5}'  ",
							   MainForm.localLib,
						  MainForm.localPreFix,

								//MainForm.FClib,
								//MainForm.FCprefix,
								_currentParcel.Record,
								_currentParcel.Card,
								maxLineCnt,
								_nextSectLtr));

				if (thisnextSect != String.Empty)
				{
					dbConn.DBConnection.ExecuteNonSelectStatement(removeLine.ToString());
					lineCnt--;
				}

				try
				{
					NewSectionPoints.RemoveAt(lineCnt);

					ms = savpic[click];

					StartX = _StartX[click + 1];
					StartY = _StartY[click + 1];

					CalculateClosure(pt2X, pt2Y);

					_mainimage = byteArrayToImage(ms);
					ExpSketchPBox.Image = _mainimage;

					if (lineCnt == 0)
					{
						this.Close();
					}
				}
				catch (Exception)
				{
				}
			}

			_undoLine = false;
		}

		private void UndoLastAction()
		{
			try
			{
				if (draw != false)
				{
					BeginSectionBtn.BackColor = Color.Orange;
					BeginSectionBtn.Text = "Begin";
				}

				if (_undoJump == false)
				{
					_undoLine = true;
					unDo(savpic, click);
					_undoJump = true;
				}
				if (_undoJump == true)
				{
					if (draw != false)
					{
						BeginSectionBtn.BackColor = Color.Cyan;
						BeginSectionBtn.Text = "Begin";
					}

					StringBuilder delXdir = new StringBuilder();
					delXdir.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X'",
							   MainForm.localLib,
							  MainForm.localPreFix,
									_currentParcel.Record,
									_currentParcel.Card));

					dbConn.DBConnection.ExecuteNonSelectStatement(delXdir.ToString());

					_undoJump = false;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
				throw;
			}
		}

		private void undoRedraw(Dictionary<int, byte[]> redrawsav, int thisclick)
		{
			savpic = redrawsav;

			click = thisclick;

			int telwe = savcnt.Count;

			if (MainForm.reopenSec != null)
			{
				_nextSectLtr = MainForm.reopenSec;

				NewSectionPoints.Clear();
				lineCnt = undoPoints.Rows.Count;

				int stxcnt = _StartX.Count;

				for (int i = 0; i < undoPoints.Rows.Count; i++)
				{
					float x1 = Convert.ToSingle(undoPoints.Rows[i]["X1pt"].ToString());

					float y1 = Convert.ToSingle(undoPoints.Rows[i]["Y1pt"].ToString());

					float x2 = Convert.ToSingle(undoPoints.Rows[i]["X2pt"].ToString());

					float y2 = Convert.ToSingle(undoPoints.Rows[i]["Y2pt"].ToString());

					float x1s = ((x1 - ScaleBaseX) / _currentScale);

					float y1s = ((y1 - ScaleBaseY) / _currentScale);

					float x2s = ((x2 - ScaleBaseX) / _currentScale);

					float y2s = ((y2 - ScaleBaseY) / _currentScale);

					NewSectionPoints.Add(new PointF(x1s, y1s));
				}
			}

			int tstclk = lineCnt;

			int dclick = click;

			click--;

			click--;

			float txtsx = _StartX[lineCnt];
			float txtsy = _StartY[lineCnt];

			ms = savpic[click];

			StartX = _StartX[lineCnt];
			StartY = _StartY[lineCnt];

			_mainimage = byteArrayToImage(ms);
			ExpSketchPBox.Image = _mainimage;
		}

		public void WriteToTextFile(string separator, string filename)
		{
			string sep = separator;

			string Folder = String.Format(@"{0}:\{1}\{2}\{3}",
				"C",
			  MainForm.localLib,
			   MainForm.localPreFix,

				//MainForm.FClib,
				//MainForm.FCprefix,
				"upload");

			filename = String.Format(@"{0}:\{1}\{2}\{3}\{4}",
				"C",
			   MainForm.localLib,
			   MainForm.localPreFix,

			   //MainForm.FClib,
			   //MainForm.FCprefix,
			   "upload",
			   "Section.txt");

			if (File.Exists(filename))
			{
				File.Delete(filename);
			}

			string query_sec = String.Format("select * from {0}.{1}section where jsrecord = {2} and jsdwell  = {3} ",
					   MainForm.localLib,
						  MainForm.localPreFix,

					   //MainForm.FClib,
					   //MainForm.FCprefix,
					   _currentParcel.Record,
					   _currentParcel.Card);

			DataSet dp_sec = dbConn.DBConnection.RunSelectStatement(query_sec);

			int rowcnt_Sec = dp_sec.Tables[0].Rows.Count;

			int colcnt_Sec = dp_sec.Tables[0].Columns.Count;

			for (int i = 0; i < dp_sec.Tables[0].Rows.Count; i++)
			{
				StringBuilder SectionRecs = new StringBuilder();
				for (int j = 0; j < dp_sec.Tables[0].Columns.Count; j++)
				{
					SectionRecs.Append(String.Format("{0}\t", dp_sec.Tables[0].Rows[i][j].ToString()));
				}

				StreamWriter WrtSect = new StreamWriter(filename, true);
				WrtSect.WriteLine(SectionRecs);
				WrtSect.Close();
			}

			string query_line = String.Format("select * from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
						   MainForm.localLib,
						  MainForm.localPreFix,

							//MainForm.FClib,
							//MainForm.FCprefix,
							_currentParcel.Record,
							_currentParcel.Card);

			DataSet dp_line = dbConn.DBConnection.RunSelectStatement(query_line);

			filename = String.Format(@"{0}:\{1}\{2}\{3}\{4}",
				"C",
			  MainForm.localLib,
			   MainForm.localPreFix,

				//MainForm.FClib,
				//MainForm.FCprefix,
				"upload",
				"line.txt");

			if (File.Exists(filename))
			{
				File.Delete(filename);
			}

			for (int i = 0; i < dp_line.Tables[0].Rows.Count; i++)
			{
				StringBuilder LineRecs = new StringBuilder();
				for (int j = 0; j < dp_line.Tables[0].Columns.Count; j++)
				{
					LineRecs.Append(String.Format("{0}\t", dp_line.Tables[0].Rows[i][j].ToString()));
				}

				StreamWriter WrtLine = new StreamWriter(filename, true);
				WrtLine.WriteLine(LineRecs);
				WrtLine.Close();
			}
		}
	}
}