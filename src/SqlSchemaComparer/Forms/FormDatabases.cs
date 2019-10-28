using SqlSchemaComparer.AppData;
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
    public partial class FormDatabases : Form
    {
        public FormDatabases()
        {
            InitializeComponent();
        }

        private void FormDatabases_Load(object sender, EventArgs e)
        {
            lstConnections.Items.Clear();
            foreach (DatabaseConnection c in AppDataContext.DB.DatabaseConnections.OrderBy(c => c.Name))
            {
                lstConnections.Items.Add(c);
            }
        }
        private void lstConnections_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ListBox lb = (ListBox)sender;
            DatabaseConnection obj = (DatabaseConnection)lb.Items[e.Index];
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDatabase f = new FormDatabase();
            if (f.ShowDialog() == DialogResult.OK)
            {
                DatabaseConnection newDB = new DatabaseConnection
                {
                    Name = f.DatabaseName,
                    Server = f.Host,
                    Username = f.Username,
                    Password = f.Password,
                    Database = f.Database,
					AllowChanges = f.AllowChanges
                };
                AppDataContext.DB.DatabaseConnections.Add(newDB);
                AppDataContext.DB.SaveChanges();
                FormDatabases_Load(sender, e);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstConnections.SelectedItem == null) return;

            DatabaseConnection dbConnection = (DatabaseConnection)lstConnections.SelectedItem;
            FormDatabase f = new FormDatabase();
            f.DatabaseName = dbConnection.Name;
            f.Host = dbConnection.Server;
            f.Username= dbConnection.Username;
            f.Password = dbConnection.Password;
            f.Database= dbConnection.Database;
			f.AllowChanges = dbConnection.AllowChanges;
            if (f.ShowDialog() == DialogResult.OK)
            {
                dbConnection.Name = f.DatabaseName;
                dbConnection.Server = f.Host;
                dbConnection.Username = f.Username;
                dbConnection.Password = f.Password;
                dbConnection.Database = f.Database;
				dbConnection.AllowChanges = f.AllowChanges;
                AppDataContext.DB.SaveChanges();
                FormDatabases_Load(sender, e);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstConnections.SelectedItem == null) return;

            DatabaseConnection dbConnection = (DatabaseConnection)lstConnections.SelectedItem;

            if (MessageBox.Show(
                string.Format("Do you really want to delete the {0} database connection?", dbConnection.Name),
                "",
                MessageBoxButtons.YesNo
                ) == DialogResult.Yes)
            {
                AppDataContext.DB.DatabaseConnections.Remove(dbConnection);
                AppDataContext.DB.SaveChanges();
                FormDatabases_Load(sender, e);
            }
        }

        private void lstConnections_DoubleClick(object sender, EventArgs e)
        {
            editToolStripMenuItem_Click(sender, e);
        }
    }
}
