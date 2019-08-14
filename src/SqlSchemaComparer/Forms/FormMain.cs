using SqlSchemaComparer.AppData;
using SqlSchemaComparer.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSchemaComparer.Forms
{
	public partial class FormMain : Form
	{
		bool ignoreComments;
		bool ignoreGo;
		bool caseSensitive;
		bool showIdentical;
		bool includeDropActions;

		DatabaseConnection dbSelected1;
		DatabaseConnection dbSelected2;
		string winMerge;

		public FormMain()
		{
			InitializeComponent();

			AppDataContext.DB.InitialValues();
			ignoreComments = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorecomments").Value == "1";
			ignoreGo = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorego").Value == "1";
			caseSensitive = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "casesensitive").Value == "1";
			showIdentical = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "showidentical").Value == "1";
			includeDropActions = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "includedropactions").Value == "1";

			winMerge = string.Empty;
			string tryWinmerge = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WinMerge", "WinMergeU.exe");
			if (string.IsNullOrEmpty(winMerge) && File.Exists(tryWinmerge)) winMerge = tryWinmerge;

			tryWinmerge = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "WinMerge", "WinMergeU.exe");
			if (string.IsNullOrEmpty(winMerge) && File.Exists(tryWinmerge)) winMerge = tryWinmerge;

			tryWinmerge = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "WinMerge", "WinMergeU.exe");
			if (string.IsNullOrEmpty(winMerge) && File.Exists(tryWinmerge)) winMerge = tryWinmerge;
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			Text += " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

			toolStripStatusLabel.Text = "";
			RefreshDatabases();

			SavedValue db1 = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "database1");
			SavedValue db2 = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "database2");

			if (db1 != null) foreach (DatabaseConnection c in cmbDatabase1.Items) if (c.Id.ToString() == db1.Value) cmbDatabase1.SelectedItem = c;
			if (db2 != null) foreach (DatabaseConnection c in cmbDatabase2.Items) if (c.Id.ToString() == db2.Value) cmbDatabase2.SelectedItem = c;
		}

		private void databasesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormDatabases f = new FormDatabases();
			f.ShowDialog();
			RefreshDatabases();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void RefreshDatabases()
		{
			DatabaseConnection db1 = (DatabaseConnection)cmbDatabase1.SelectedItem;
			DatabaseConnection db2 = (DatabaseConnection)cmbDatabase2.SelectedItem;

			cmbDatabase1.Items.Clear();
			cmbDatabase2.Items.Clear();
			foreach (DatabaseConnection c in AppDataContext.DB.DatabaseConnections.OrderBy(c => c.Name))
			{
				cmbDatabase1.Items.Add(c);
				cmbDatabase2.Items.Add(c);
			}

			if (db1 != null) foreach (DatabaseConnection c in cmbDatabase1.Items) if (c.Id == db1.Id) cmbDatabase1.SelectedItem = c;
			if (db2 != null) foreach (DatabaseConnection c in cmbDatabase2.Items) if (c.Id == db2.Id) cmbDatabase2.SelectedItem = c;
		}

		private void cmbDatabase1_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			ComboBox cmb = (ComboBox)sender;
			DatabaseConnection obj = (DatabaseConnection)cmb.Items[e.Index];
			string text = string.Format("{0} ({1})", obj.Name, obj.Server);

			e.DrawBackground();

			Graphics g = e.Graphics;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				g.FillRectangle(SystemBrushes.Highlight, e.Bounds);
			}
			else
			{
				g.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
			}
			g.DrawString(text, e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));

			e.DrawFocusRectangle();
		}

		private void cmbDatabase2_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			ComboBox cmb = (ComboBox)sender;
			DatabaseConnection obj = (DatabaseConnection)cmb.Items[e.Index];
			string text = string.Format("{0} ({1})", obj.Name, obj.Server);

			e.DrawBackground();

			Graphics g = e.Graphics;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				g.FillRectangle(SystemBrushes.Highlight, e.Bounds);
			}
			else
			{
				g.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
			}
			g.DrawString(text, e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));

			e.DrawFocusRectangle();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SavedValue db1 = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "database1");
			SavedValue db2 = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "database2");
			if (db1 == null) { db1 = new SavedValue { Name = "database1" }; AppDataContext.DB.SavedValues.Add(db1); }
			if (db2 == null) { db2 = new SavedValue { Name = "database2" }; AppDataContext.DB.SavedValues.Add(db2); }

			dbSelected1 = (DatabaseConnection)cmbDatabase1.SelectedItem;
			dbSelected2 = (DatabaseConnection)cmbDatabase2.SelectedItem;

			db1.Value = dbSelected1 == null ? "" : dbSelected1.Id.ToString();
			db2.Value = dbSelected2 == null ? "" : dbSelected2.Id.ToString();
			AppDataContext.DB.SaveChanges();
		}

		private void compareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dbSelected1 = (DatabaseConnection)cmbDatabase1.SelectedItem;
			dbSelected2 = (DatabaseConnection)cmbDatabase2.SelectedItem;

			if (dbSelected1 == null || dbSelected2 == null) return;

			Cursor = Cursors.WaitCursor;
			menuStrip1.Enabled = false;
			cmbDatabase1.Enabled = false;
			cmbDatabase2.Enabled = false;
			Grid.Enabled = false;
			Application.DoEvents();

			Object1.HeaderText = dbSelected1.Name;
			Object2.HeaderText = dbSelected2.Name;
			Application.DoEvents();

			DatabaseSource database1 = new DatabaseSource(dbSelected1);
			database1.DatabaseObjectScanProgress += new DatabaseSource.DatabaseObjectScanProgressHandler(Database1ScanProgressMade);
			database1.GetDatabaseObjects(true);

			DatabaseSource database2 = new DatabaseSource(dbSelected2);
			database2.DatabaseObjectScanProgress += new DatabaseSource.DatabaseObjectScanProgressHandler(Database2ScanProgressMade);
			database2.GetDatabaseObjects(false);

			Compare(database1, database2);

			menuStrip1.Enabled = true;
			cmbDatabase1.Enabled = true;
			cmbDatabase2.Enabled = true;
			Grid.Enabled = true;
			Cursor = Cursors.Default;
		}

		private void Database1ScanProgressMade(object sender, DatabaseSource.ScanProgressEventArgs e)
		{
			toolStripStatusLabel.Text = string.Format("Database 1: {0}", e.Message);
			Application.DoEvents();
		}
		private void Database2ScanProgressMade(object sender, DatabaseSource.ScanProgressEventArgs e)
		{
			toolStripStatusLabel.Text = string.Format("Database 2: {0}", e.Message);
			Application.DoEvents();
		}

		private void Compare(DatabaseSource database1, DatabaseSource database2)
		{
			Grid.Rows.Clear();
			toolStripStatusLabel.Text = string.Format("Comparing objects...");
			Application.DoEvents();

			if (includeDropActions)
			{
				foreach (DatabaseObject obj2 in database2.DatabaseObjects)
				{
					DatabaseObject obj1 = database1.DatabaseObjects.FirstOrDefault(o => o.Name == obj2.Name);
					if (obj1 == null)
					{
						AddGridRow(obj1, obj2);
					}
				}
			}
			foreach (DatabaseObject obj1 in database1.DatabaseObjects)
			{
				DatabaseObject obj2 = database2.DatabaseObjects.FirstOrDefault(o => o.Name == obj1.Name);
				AddGridRow(obj1, obj2);
			}

			toolStripStatusLabel.Text = string.Format("Finished");
			Application.DoEvents();

		}

		private void AddGridRow(DatabaseObject obj1, DatabaseObject obj2)
		{
			DataGridViewRow row = new DataGridViewRow();
			DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
			DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
			DataGridViewTextBoxCell cellStatus = new DataGridViewTextBoxCell();

			cell1.Tag = obj1;
			cell2.Tag = obj2;

			cell1.Value = obj1 == null ? "" : string.Format("{0}.{1}", obj1.Schema, obj1.Name);
			cell2.Value = obj2 == null ? "" : string.Format("{0}.{1}", obj2.Schema, obj2.Name);

			if (obj1 == null)
			{
				cellStatus.Value = obj2 != null ? "DROP " + obj2.ObjectTypeNice : "";
			}
			else
			{
				if (obj2 == null)
				{
					cellStatus.Value = "CREATE " + obj1.ObjectTypeNice;
				}
				else if (!obj1.CreateSQL.FunctionallyEquals(obj2.CreateSQL, ignoreComments, ignoreGo, caseSensitive))
				{
					cellStatus.Value = "ALTER " + obj1.ObjectTypeNice;
				}
				else
				{
					cellStatus.Value = "";
				}
			}
			row.Cells.Add(cell1);
			row.Cells.Add(cellStatus);
			row.Cells.Add(cell2);
			if (showIdentical || !string.IsNullOrWhiteSpace(cellStatus.Value.ToString()))
			{
				Grid.Rows.Add(row);
			}
		}

		private void Grid_DoubleClick(object sender, EventArgs e)
		{
			if (Grid.SelectedRows.Count != 1) return;

			DatabaseObject obj1 = (DatabaseObject)Grid.SelectedRows[0].Cells[0].Tag;
			DatabaseObject obj2 = (DatabaseObject)Grid.SelectedRows[0].Cells[2].Tag;

			string file1;
			string file2;
			if (obj1 != null && !string.IsNullOrWhiteSpace(obj1.Filename))
			{
				file1 = obj1.Filename;
			}
			else
			{
				file1 = Path.GetTempFileName();
				File.Delete(file1);
				file1 += ".sql";
				File.WriteAllText(file1, obj1 != null ? obj1.CreateSQL.ToString() : "");
			}
			if (obj2 != null && !string.IsNullOrWhiteSpace(obj2.Filename))
			{
				file2 = obj2.Filename;
			}
			else
			{
				file2 = Path.GetTempFileName();
				File.Delete(file2);
				file2 += ".sql";
				File.WriteAllText(file2, obj2 != null ? obj2.CreateSQL.ToString() : "");
			}

			ProcessStartInfo proc = new ProcessStartInfo(
				winMerge,
				string.Format("\"{0}\" \"{1}\"", file1, file2)
				);
			Process.Start(proc);
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormOptions f = new FormOptions();
			f.ShowDialog();
			ignoreComments = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorecomments").Value == "1";
			ignoreGo = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorego").Value == "1";
			caseSensitive = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "casesensitive").Value == "1";
			showIdentical = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "showidentical").Value == "1";
			includeDropActions = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "includedropactions").Value == "1";

		}

		private void createScriptForAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			createScript(true);
			Application.DoEvents();
			compareToolStripMenuItem_Click(sender, e);
			Application.DoEvents();
		}

		private void createScriptForSelectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			createScript(false);
			Application.DoEvents();
			compareToolStripMenuItem_Click(sender, e);
			Application.DoEvents();
		}

		private void createScript(bool all, bool autoExecute = false)
		{
			StringBuilder script = new StringBuilder();
			script.Append(string.Format("------------------------------------------------------------\r\n"));
			script.Append(string.Format("-- Deploy script for {0}\r\n", dbSelected2.Server));
			script.Append(string.Format("------------------------------------------------------------\r\n\r\n"));
			script.Append(string.Format("USE {0}\r\nGO\r\n\r\n", dbSelected2.Database));

			List<DataGridViewRow> rows = new List<DataGridViewRow>();
			if (all)
				foreach (DataGridViewRow r in Grid.Rows) rows.Add(r);
			else
				foreach (DataGridViewRow r in Grid.Rows) if (Grid.SelectedRows.Contains(r)) rows.Add(r);

			foreach (DataGridViewRow row in rows)
			{
				DatabaseObject obj1 = (DatabaseObject)row.Cells[0].Tag;
				string action = (string)row.Cells[1].Value;
				DatabaseObject obj2 = (DatabaseObject)row.Cells[2].Tag;
				if (action.StartsWith("CREATE", StringComparison.CurrentCultureIgnoreCase))
				{
					script.Append(obj1.DropSQL);
					script.Append(obj1.CreateSQL);
					if (!obj1.CreateSQL.Str.Trim().EndsWith("GO", StringComparison.CurrentCultureIgnoreCase))
					{
						if (!obj1.CreateSQL.Str.EndsWith("\n"))
						{
							script.Append("\r\n");
						}
						script.Append("GO\r\n");
					}
					else if (!obj1.CreateSQL.Str.EndsWith("\n"))
					{
						script.Append("\r\n");
					}
				}
				else if (action.StartsWith("ALTER", StringComparison.CurrentCultureIgnoreCase))
				{
					//string alterSql = obj1.ObjectType != "U" ? obj1.CreateSQL.ChangeCreateToAlter().Str : obj1.CreateSQL.ChangeCreateToDropCreate().Str;
					//script.Append(alterSql);
					script.Append(obj1.DropSQL);
					script.Append(obj1.CreateSQL);
					if (!obj1.CreateSQL.Str.EndsWith("\n"))
					{
						script.Append("\r\n");
					}
					script.Append("GO\r\n");
				}
				else if (action.StartsWith("DROP", StringComparison.CurrentCultureIgnoreCase))
				{
					//script.Append(string.Format("{0} {1}.{2}\r\nGO\r\n", action, obj2.Schema, obj2.Name));
					script.Append(obj2.DropSQL);
				}
			}

			if (autoExecute)
			{
				FormViewScript f = new FormViewScript(dbSelected2, script.ToString());
				f.DatabaseConnection = dbSelected2;
				f.ShowDialog();
				compareToolStripMenuItem_Click(null, null);
			}
			else
			{
				FormViewScript f = new FormViewScript();
				f.Script = script.ToString();
				f.DatabaseConnection = dbSelected2;
				f.ShowDialog();
			}
		}

		private void compareAndExecuteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			compareToolStripMenuItem_Click(sender, e);
			Application.DoEvents();
			createScript(true, true);
			Application.DoEvents();
		}

	}
}
