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
    public partial class FormDatabase : Form
    {
        public string DatabaseName { get { return txtName.Text; } set { txtName.Text = value; } }
        public string Host { get { return txtHost.Text; } set { txtHost.Text = value; } }
        public string Username { get { return txtUsername.Text; } set { txtUsername.Text = value; } }
        public string Password { get { return txtPassword.Text; } set { txtPassword.Text = value; } }
		public string Database { get { return txtDatabase.Text; } set { txtDatabase.Text = value; } }
		public bool AllowChanges { get { return chkAllowChanges.Checked; } set { chkAllowChanges.Checked = value; } }


		public FormDatabase()
        {
            InitializeComponent();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

	}
}
