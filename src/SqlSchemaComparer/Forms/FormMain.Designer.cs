namespace SqlSchemaComparer.Forms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDatabase2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDatabase1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createScriptForSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createScriptForAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.Object1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ObjectStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Object2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbDatabase2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbDatabase1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(942, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Databases";
            // 
            // cmbDatabase2
            // 
            this.cmbDatabase2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabase2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDatabase2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase2.FormattingEnabled = true;
            this.cmbDatabase2.Location = new System.Drawing.Point(92, 46);
            this.cmbDatabase2.Name = "cmbDatabase2";
            this.cmbDatabase2.Size = new System.Drawing.Size(844, 21);
            this.cmbDatabase2.TabIndex = 3;
            this.cmbDatabase2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbDatabase2_DrawItem);
            // 
            // label2
            // 
            this.label2.Image = global::SqlSchemaComparer.Properties.Resources.database;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "      Database 2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDatabase1
            // 
            this.cmbDatabase1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabase1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDatabase1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase1.FormattingEnabled = true;
            this.cmbDatabase1.Location = new System.Drawing.Point(92, 19);
            this.cmbDatabase1.Name = "cmbDatabase1";
            this.cmbDatabase1.Size = new System.Drawing.Size(844, 21);
            this.cmbDatabase1.TabIndex = 1;
            this.cmbDatabase1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbDatabase1_DrawItem);
            // 
            // label1
            // 
            this.label1.Image = global::SqlSchemaComparer.Properties.Resources.database;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "      Database 1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.databasesToolStripMenuItem,
            this.compareToolStripMenuItem,
            this.createScriptForAllToolStripMenuItem,
            this.createScriptForSelectedToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(967, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitToolStripMenuItem.Image = global::SqlSchemaComparer.Properties.Resources.door_in;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.optionsToolStripMenuItem.Image = global::SqlSchemaComparer.Properties.Resources.cog;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // databasesToolStripMenuItem
            // 
            this.databasesToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.databasesToolStripMenuItem.Image = global::SqlSchemaComparer.Properties.Resources.database_gear;
            this.databasesToolStripMenuItem.Name = "databasesToolStripMenuItem";
            this.databasesToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.databasesToolStripMenuItem.Text = "Databases";
            this.databasesToolStripMenuItem.Click += new System.EventHandler(this.databasesToolStripMenuItem_Click);
            // 
            // compareToolStripMenuItem
            // 
            this.compareToolStripMenuItem.Image = global::SqlSchemaComparer.Properties.Resources.arrow_refresh;
            this.compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            this.compareToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.compareToolStripMenuItem.Text = "&Compare";
            this.compareToolStripMenuItem.Click += new System.EventHandler(this.compareToolStripMenuItem_Click);
            // 
            // createScriptForSelectedToolStripMenuItem
            // 
            this.createScriptForSelectedToolStripMenuItem.Image = global::SqlSchemaComparer.Properties.Resources.script;
            this.createScriptForSelectedToolStripMenuItem.Name = "createScriptForSelectedToolStripMenuItem";
            this.createScriptForSelectedToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
            this.createScriptForSelectedToolStripMenuItem.Text = "Create script for selected";
            this.createScriptForSelectedToolStripMenuItem.Click += new System.EventHandler(this.createScriptForSelectedToolStripMenuItem_Click);
            // 
            // createScriptForAllToolStripMenuItem
            // 
            this.createScriptForAllToolStripMenuItem.Image = global::SqlSchemaComparer.Properties.Resources.script;
            this.createScriptForAllToolStripMenuItem.Name = "createScriptForAllToolStripMenuItem";
            this.createScriptForAllToolStripMenuItem.Size = new System.Drawing.Size(134, 20);
            this.createScriptForAllToolStripMenuItem.Text = "Create script for all";
            this.createScriptForAllToolStripMenuItem.Click += new System.EventHandler(this.createScriptForAllToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 609);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(967, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.AllowUserToResizeRows = false;
            this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid.BackgroundColor = System.Drawing.Color.White;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Object1,
            this.ObjectStatus,
            this.Object2});
            this.Grid.Location = new System.Drawing.Point(12, 111);
            this.Grid.Name = "Grid";
            this.Grid.ReadOnly = true;
            this.Grid.RowHeadersVisible = false;
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.Size = new System.Drawing.Size(943, 495);
            this.Grid.TabIndex = 3;
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            // 
            // Object1
            // 
            this.Object1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Object1.HeaderText = "Database 1";
            this.Object1.Name = "Object1";
            this.Object1.ReadOnly = true;
            // 
            // ObjectStatus
            // 
            this.ObjectStatus.HeaderText = "Status";
            this.ObjectStatus.Name = "ObjectStatus";
            this.ObjectStatus.ReadOnly = true;
            // 
            // Object2
            // 
            this.Object2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Object2.HeaderText = "Database 2";
            this.Object2.Name = "Object2";
            this.Object2.ReadOnly = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 631);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(636, 345);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Server Schema Comparer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbDatabase1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDatabase2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databasesToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Object1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Object2;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createScriptForSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createScriptForAllToolStripMenuItem;
    }
}

