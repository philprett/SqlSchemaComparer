namespace SqlSchemaComparer.Forms
{
    partial class FormOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.label1 = new System.Windows.Forms.Label();
            this.chkIgnoreComments = new System.Windows.Forms.CheckBox();
            this.chkShowIdenticalObjects = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.butSave = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.chkIgnoreGo = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkIncludeDropActions = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ignore comments?";
            // 
            // chkIgnoreComments
            // 
            this.chkIgnoreComments.AutoSize = true;
            this.chkIgnoreComments.Location = new System.Drawing.Point(185, 23);
            this.chkIgnoreComments.Name = "chkIgnoreComments";
            this.chkIgnoreComments.Size = new System.Drawing.Size(15, 14);
            this.chkIgnoreComments.TabIndex = 6;
            this.chkIgnoreComments.UseVisualStyleBackColor = true;
            // 
            // chkShowIdenticalObjects
            // 
            this.chkShowIdenticalObjects.AutoSize = true;
            this.chkShowIdenticalObjects.Location = new System.Drawing.Point(185, 83);
            this.chkShowIdenticalObjects.Name = "chkShowIdenticalObjects";
            this.chkShowIdenticalObjects.Size = new System.Drawing.Size(15, 14);
            this.chkShowIdenticalObjects.TabIndex = 8;
            this.chkShowIdenticalObjects.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Show identical objects in results?";
            // 
            // butSave
            // 
            this.butSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.butSave.Image = global::SqlSchemaComparer.Properties.Resources.disk;
            this.butSave.Location = new System.Drawing.Point(44, 144);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(48, 48);
            this.butSave.TabIndex = 9;
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Image = global::SqlSchemaComparer.Properties.Resources.cancel;
            this.butCancel.Location = new System.Drawing.Point(131, 144);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(48, 48);
            this.butCancel.TabIndex = 10;
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // chkIgnoreGo
            // 
            this.chkIgnoreGo.AutoSize = true;
            this.chkIgnoreGo.Location = new System.Drawing.Point(185, 43);
            this.chkIgnoreGo.Name = "chkIgnoreGo";
            this.chkIgnoreGo.Size = new System.Drawing.Size(15, 14);
            this.chkIgnoreGo.TabIndex = 12;
            this.chkIgnoreGo.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Ignore GO?";
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.AutoSize = true;
            this.chkCaseSensitive.Location = new System.Drawing.Point(185, 63);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(15, 14);
            this.chkCaseSensitive.TabIndex = 14;
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Case sensitive?";
            // 
            // chkIncludeDropActions
            // 
            this.chkIncludeDropActions.AutoSize = true;
            this.chkIncludeDropActions.Location = new System.Drawing.Point(185, 103);
            this.chkIncludeDropActions.Name = "chkIncludeDropActions";
            this.chkIncludeDropActions.Size = new System.Drawing.Size(15, 14);
            this.chkIncludeDropActions.TabIndex = 16;
            this.chkIncludeDropActions.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Include drop actions?";
            // 
            // FormOptions
            // 
            this.AcceptButton = this.butSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(222, 215);
            this.Controls.Add(this.chkIncludeDropActions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkCaseSensitive);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkIgnoreGo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.chkShowIdenticalObjects);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkIgnoreComments);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(238, 297);
            this.MinimumSize = new System.Drawing.Size(238, 197);
            this.Name = "FormOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkIgnoreComments;
        private System.Windows.Forms.CheckBox chkShowIdenticalObjects;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox chkIgnoreGo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkIncludeDropActions;
        private System.Windows.Forms.Label label4;
    }
}