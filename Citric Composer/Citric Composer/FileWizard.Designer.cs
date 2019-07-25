namespace Citric_Composer {
    partial class FileWizard {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileWizard));
            this.okButton = new System.Windows.Forms.Button();
            this.useExistingFile = new System.Windows.Forms.RadioButton();
            this.newFile = new System.Windows.Forms.RadioButton();
            this.existingFiles = new System.Windows.Forms.ComboBox();
            this.newFilePath = new System.Windows.Forms.TextBox();
            this.browse = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.referenceFileExternally = new System.Windows.Forms.CheckBox();
            this.nullFile = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.blankFile = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(159, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(150, 24);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // useExistingFile
            // 
            this.useExistingFile.AutoSize = true;
            this.useExistingFile.Location = new System.Drawing.Point(12, 13);
            this.useExistingFile.Name = "useExistingFile";
            this.useExistingFile.Size = new System.Drawing.Size(102, 17);
            this.useExistingFile.TabIndex = 25;
            this.useExistingFile.TabStop = true;
            this.useExistingFile.Text = "Use Existing File";
            this.useExistingFile.UseVisualStyleBackColor = true;
            this.useExistingFile.CheckedChanged += new System.EventHandler(this.UseExistingFile_CheckedChanged);
            // 
            // newFile
            // 
            this.newFile.AutoSize = true;
            this.newFile.Location = new System.Drawing.Point(12, 43);
            this.newFile.Name = "newFile";
            this.newFile.Size = new System.Drawing.Size(66, 17);
            this.newFile.TabIndex = 26;
            this.newFile.TabStop = true;
            this.newFile.Text = "New File";
            this.newFile.UseVisualStyleBackColor = true;
            this.newFile.CheckedChanged += new System.EventHandler(this.NewFile_CheckedChanged);
            // 
            // existingFiles
            // 
            this.existingFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.existingFiles.FormattingEnabled = true;
            this.existingFiles.Location = new System.Drawing.Point(120, 12);
            this.existingFiles.Name = "existingFiles";
            this.existingFiles.Size = new System.Drawing.Size(201, 21);
            this.existingFiles.TabIndex = 27;
            this.existingFiles.SelectedIndexChanged += new System.EventHandler(this.ExistingFiles_SelectedIndexChanged);
            // 
            // newFilePath
            // 
            this.newFilePath.Enabled = false;
            this.newFilePath.Location = new System.Drawing.Point(84, 42);
            this.newFilePath.Name = "newFilePath";
            this.newFilePath.Size = new System.Drawing.Size(201, 20);
            this.newFilePath.TabIndex = 28;
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(291, 39);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(30, 25);
            this.browse.TabIndex = 29;
            this.browse.Text = "...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(3, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cancelButton.Size = new System.Drawing.Size(150, 24);
            this.cancelButton.TabIndex = 30;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // referenceFileExternally
            // 
            this.referenceFileExternally.AutoSize = true;
            this.referenceFileExternally.Location = new System.Drawing.Point(178, 70);
            this.referenceFileExternally.Name = "referenceFileExternally";
            this.referenceFileExternally.Size = new System.Drawing.Size(143, 17);
            this.referenceFileExternally.TabIndex = 31;
            this.referenceFileExternally.Text = "Reference File Externally";
            this.referenceFileExternally.UseVisualStyleBackColor = true;
            this.referenceFileExternally.Visible = false;
            // 
            // nullFile
            // 
            this.nullFile.AutoSize = true;
            this.nullFile.Location = new System.Drawing.Point(12, 103);
            this.nullFile.Name = "nullFile";
            this.nullFile.Size = new System.Drawing.Size(62, 17);
            this.nullFile.TabIndex = 32;
            this.nullFile.TabStop = true;
            this.nullFile.Text = "Null File";
            this.nullFile.UseVisualStyleBackColor = true;
            this.nullFile.CheckedChanged += new System.EventHandler(this.NullFile_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.okButton, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 127);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(312, 30);
            this.tableLayoutPanel1.TabIndex = 34;
            // 
            // blankFile
            // 
            this.blankFile.AutoSize = true;
            this.blankFile.Location = new System.Drawing.Point(12, 73);
            this.blankFile.Name = "blankFile";
            this.blankFile.Size = new System.Drawing.Size(71, 17);
            this.blankFile.TabIndex = 35;
            this.blankFile.TabStop = true;
            this.blankFile.Text = "Blank File";
            this.blankFile.UseVisualStyleBackColor = true;
            this.blankFile.CheckedChanged += new System.EventHandler(this.BlankFile_CheckedChanged);
            // 
            // FileWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 164);
            this.Controls.Add(this.blankFile);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.nullFile);
            this.Controls.Add(this.referenceFileExternally);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.newFilePath);
            this.Controls.Add(this.existingFiles);
            this.Controls.Add(this.newFile);
            this.Controls.Add(this.useExistingFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Wizard";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.RadioButton useExistingFile;
        private System.Windows.Forms.RadioButton newFile;
        private System.Windows.Forms.ComboBox existingFiles;
        private System.Windows.Forms.TextBox newFilePath;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox referenceFileExternally;
        private System.Windows.Forms.RadioButton nullFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton blankFile;
    }
}