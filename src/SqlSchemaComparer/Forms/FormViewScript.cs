using SqlSchemaComparer.AppData;
using SqlSchemaComparer.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSchemaComparer.Forms
{
    internal partial class FormViewScript : Form
    {
		public bool AutoExecute { get; set; }
        public string Script
        {
            get { return txtScript.Text; }
            set
            {
                txtScript.Text = value;
                txtScript.SelectionStart = 0;
                txtScript.SelectionLength = 0;
            }
        }

        public DatabaseConnection DatabaseConnection { get; set; }

		public FormViewScript()
		{
			InitializeComponent();
			AutoExecute = false;
		}
		public FormViewScript(DatabaseConnection databaseConnection, string script)
		{
			InitializeComponent();
			DatabaseConnection = databaseConnection;
			Script = script;
			AutoExecute = true;
		}

		private void FormViewScript_Load(object sender, EventArgs e)
        {
            txtScript.SelectionStart = 0;
            txtScript.SelectionLength = 0;
            toolStripStatusLabel.Text = "";
			if (AutoExecute)
			{
				executeToolStripMenuItem_Click(sender, e);
			}
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string defaultFilename = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Updates.sql";

            SaveFileDialog f = new SaveFileDialog();

            SavedValue lastFilename = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "lastsavefilename");
            if (lastFilename == null)
            {
                f.FileName = defaultFilename;
            }
            else
            {
                FileInfo file = new FileInfo(lastFilename.Value);
                f.FileName = Path.Combine(file.Directory.FullName, defaultFilename);
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(f.FileName, txtScript.Text);
                if (lastFilename == null)
                    lastFilename = new SavedValue() { Id = Utils.GetRandomLong(), Name = "lastsavefilename", Value = f.FileName };
                else
                    lastFilename.Value = f.FileName;
                AppDataContext.DB.SaveChanges();
                Close();
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtScript.Text);
            Close();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!DatabaseConnection.IsSqlServer)
            {
                MessageBox.Show("Database 2 is not a SQL Server");
            }
            else
            {
                if (!AutoExecute &&
					MessageBox.Show(
                    string.Format("Do you really want to run this script on {0} on {1}?", DatabaseConnection.Database, DatabaseConnection.Server),
                    "",
                    MessageBoxButtons.YesNo
                    ) != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection sql = new SqlConnection(DatabaseConnection.ConnectionString))
                {
                    try
                    {
                        toolStripStatusLabel.Text = string.Format("Connecting to {0} on {1}...", DatabaseConnection.Database, DatabaseConnection.Server);
                        Application.DoEvents();

                        sql.Open();

                        string[] statements = txtScript.Text.Split(new[] { "\nGO" }, StringSplitOptions.None);

                        int counter = 0;
                        foreach (string statement in statements)
                        {
                            counter++;
                            toolStripStatusLabel.Text = string.Format("Executing statement {0} from {1}...", counter, statements.Length);
                            Application.DoEvents();

                            if (!string.IsNullOrWhiteSpace(statement))
                            {
                                try
                                {
                                    using (SqlCommand com = sql.CreateCommand())
                                    {
                                        com.CommandText = statement;
                                        com.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(string.Format("Error executing statement: {0}", ex.Message));
                                    break;
                                }
                            }
                        }
                        toolStripStatusLabel.Text = string.Format("Execution successful");
                        Application.DoEvents();
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error connecting to {0}: {1}", DatabaseConnection.Server, ex.Message));
                    }
                }
            }

        }

        private void SqlServerProgressMade(object sender, DatabaseSqlServer.ProgressEventArgs e)
        {
            toolStripStatusLabel.Text = e.Message;
            Application.DoEvents();
        }
    }
}
