using SqlSchemaComparer.AppData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSchemaComparer.Forms
{
    public partial class FormViewScript : Form
    {
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

        public FormViewScript()
        {
            InitializeComponent();
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

        private void FormViewScript_Load(object sender, EventArgs e)
        {
            txtScript.SelectionStart = 0;
            txtScript.SelectionLength = 0;
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtScript.Text);
            Close();
        }
    }
}
