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
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            SavedValue ignoreComments = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorecomments");
            SavedValue ignoreGo = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorego");
            SavedValue caseSensitive = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "casesensitive");
            SavedValue showIdentical = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "showidentical");
            SavedValue includeDropActions = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "includedropactions");

            chkIgnoreComments.Checked = ignoreComments.Value == "1";
            chkIgnoreGo.Checked = ignoreGo.Value == "1";
            chkCaseSensitive.Checked = caseSensitive.Value == "1";
            chkShowIdenticalObjects.Checked = showIdentical.Value == "1";
            chkIncludeDropActions.Checked = includeDropActions.Value == "1";
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            SavedValue ignoreComments = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorecomments");
            SavedValue ignoreGo = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "ignorego");
            SavedValue caseSensitive = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "casesensitive");
            SavedValue showIdentical = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "showidentical");
            SavedValue includeDropActions = AppDataContext.DB.SavedValues.FirstOrDefault(v => v.Name == "includedropactions");

            ignoreComments.Value = chkIgnoreComments.Checked ? "1" : "0";
            ignoreGo.Value = chkIgnoreGo.Checked ? "1" : "0";
            caseSensitive.Value = chkCaseSensitive.Checked ? "1" : "0";
            showIdentical.Value = chkShowIdenticalObjects.Checked ? "1" : "0";
            includeDropActions.Value = chkIncludeDropActions.Checked ? "1" : "0";

            AppDataContext.DB.SaveChanges();

            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
