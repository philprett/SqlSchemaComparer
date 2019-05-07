using SqlSchemaComparer.AppData;
using SqlSchemaComparer.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSchemaComparer.Forms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
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

        private void FormMain_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "";
            RefreshDatabases();

            SavedValue db1 = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "database1");
            SavedValue db2 = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "database2");

            if (db1 != null) foreach (DatabaseConnection c in cmbDatabase1.Items) if (c.Id.ToString() == db1.Value) cmbDatabase1.SelectedItem = c;
            if (db2 != null) foreach (DatabaseConnection c in cmbDatabase2.Items) if (c.Id.ToString() == db2.Value) cmbDatabase2.SelectedItem = c;
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

            DatabaseConnection dbSelected1 = (DatabaseConnection)cmbDatabase1.SelectedItem;
            DatabaseConnection dbSelected2 = (DatabaseConnection)cmbDatabase2.SelectedItem;

            db1.Value = dbSelected1 == null ? "" : dbSelected1.Id.ToString();
            db2.Value = dbSelected2 == null ? "" : dbSelected2.Id.ToString();
            AppDataContext.DB.SaveChanges();
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseConnection dbSelected1 = (DatabaseConnection)cmbDatabase1.SelectedItem;
            DatabaseConnection dbSelected2 = (DatabaseConnection)cmbDatabase2.SelectedItem;

            if (dbSelected1 == null || dbSelected2 == null) return;

            DatabaseSource database1 = new DatabaseSource(dbSelected1);
            database1.DatabaseObjectScanProgress += new DatabaseSource.DatabaseObjectScanProgressHandler(Database1ScanProgressMade);
            database1.GetDatabaseObjects();

            DatabaseSource database2 = new DatabaseSource(dbSelected2);
            database2.DatabaseObjectScanProgress += new DatabaseSource.DatabaseObjectScanProgressHandler(Database2ScanProgressMade);
            database2.GetDatabaseObjects();

            Compare(database1, database2);
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
            toolStripStatusLabel.Text = string.Format("Comparing...");
            Application.DoEvents();

            foreach (DatabaseObject obj2 in database2.DatabaseObjects.OrderBy(o => o.Name))
            {
                DatabaseObject obj1 = database2.DatabaseObjects.FirstOrDefault(o => o.Name == obj2.Name);
                if (obj1 == null)
                {
                    AddGridRow(obj1, obj2);
                }
            }
            foreach (DatabaseObject obj1 in database1.DatabaseObjects.OrderBy(o => o.Name))
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

            cell1.Value = obj1 == null ? "" : obj1.Name;
            cell2.Value = obj2 == null ? "" : obj2.Name;

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
                else if (obj1.CreateSQL.ReduceWhiteSpace().ToString() != obj2.CreateSQL.ReduceWhiteSpace().ToString())
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
            Grid.Rows.Add(row);
        }
    }
}
