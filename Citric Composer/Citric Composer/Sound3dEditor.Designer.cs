namespace Citric_Composer {
    partial class Sound3dEditor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sound3dEditor));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.volume = new System.Windows.Forms.CheckBox();
            this.priority = new System.Windows.Forms.CheckBox();
            this.pan = new System.Windows.Forms.CheckBox();
            this.surroundPan = new System.Windows.Forms.CheckBox();
            this.filter = new System.Windows.Forms.CheckBox();
            this.attenuationRate = new System.Windows.Forms.NumericUpDown();
            this.dopplerFactor = new System.Windows.Forms.NumericUpDown();
            this.attenuationCurve = new System.Windows.Forms.ComboBox();
            this.unknownFlag = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.attenuationRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dopplerFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.unknownFlag, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.dopplerFactor, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.volume, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.priority, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pan, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.surroundPan, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.filter, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.attenuationRate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.attenuationCurve, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.okButton, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(236, 186);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "3d Flags:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(121, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Attenuation Rate:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(121, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Attenuation Curve:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(121, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "Doppler Factor:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // volume
            // 
            this.volume.AutoSize = true;
            this.volume.Location = new System.Drawing.Point(3, 29);
            this.volume.Name = "volume";
            this.volume.Size = new System.Drawing.Size(61, 17);
            this.volume.TabIndex = 4;
            this.volume.Text = "Volume";
            this.volume.UseVisualStyleBackColor = true;
            // 
            // priority
            // 
            this.priority.AutoSize = true;
            this.priority.Location = new System.Drawing.Point(3, 55);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(57, 17);
            this.priority.TabIndex = 5;
            this.priority.Text = "Priority";
            this.priority.UseVisualStyleBackColor = true;
            // 
            // pan
            // 
            this.pan.AutoSize = true;
            this.pan.Location = new System.Drawing.Point(3, 81);
            this.pan.Name = "pan";
            this.pan.Size = new System.Drawing.Size(45, 17);
            this.pan.TabIndex = 6;
            this.pan.Text = "Pan";
            this.pan.UseVisualStyleBackColor = true;
            // 
            // surroundPan
            // 
            this.surroundPan.AutoSize = true;
            this.surroundPan.Location = new System.Drawing.Point(3, 107);
            this.surroundPan.Name = "surroundPan";
            this.surroundPan.Size = new System.Drawing.Size(91, 17);
            this.surroundPan.TabIndex = 7;
            this.surroundPan.Text = "Surround Pan";
            this.surroundPan.UseVisualStyleBackColor = true;
            // 
            // filter
            // 
            this.filter.AutoSize = true;
            this.filter.Location = new System.Drawing.Point(3, 133);
            this.filter.Name = "filter";
            this.filter.Size = new System.Drawing.Size(48, 17);
            this.filter.TabIndex = 8;
            this.filter.Text = "Filter";
            this.filter.UseVisualStyleBackColor = true;
            // 
            // attenuationRate
            // 
            this.attenuationRate.DecimalPlaces = 6;
            this.attenuationRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attenuationRate.Location = new System.Drawing.Point(121, 29);
            this.attenuationRate.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.attenuationRate.Name = "attenuationRate";
            this.attenuationRate.Size = new System.Drawing.Size(112, 20);
            this.attenuationRate.TabIndex = 9;
            this.attenuationRate.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // dopplerFactor
            // 
            this.dopplerFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dopplerFactor.Location = new System.Drawing.Point(121, 133);
            this.dopplerFactor.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.dopplerFactor.Name = "dopplerFactor";
            this.dopplerFactor.Size = new System.Drawing.Size(112, 20);
            this.dopplerFactor.TabIndex = 10;
            // 
            // attenuationCurve
            // 
            this.attenuationCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.attenuationCurve.FormattingEnabled = true;
            this.attenuationCurve.Items.AddRange(new object[] {
            "Logarithmic",
            "Linear"});
            this.attenuationCurve.Location = new System.Drawing.Point(121, 81);
            this.attenuationCurve.Name = "attenuationCurve";
            this.attenuationCurve.Size = new System.Drawing.Size(112, 21);
            this.attenuationCurve.TabIndex = 11;
            // 
            // unknownFlag
            // 
            this.unknownFlag.AutoSize = true;
            this.unknownFlag.Location = new System.Drawing.Point(3, 159);
            this.unknownFlag.Name = "unknownFlag";
            this.unknownFlag.Size = new System.Drawing.Size(72, 17);
            this.unknownFlag.TabIndex = 12;
            this.unknownFlag.Text = "Unknown";
            this.unknownFlag.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(121, 159);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(112, 24);
            this.okButton.TabIndex = 13;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click_1);
            // 
            // Sound3dEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 186);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sound3dEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sound 3d Editor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.attenuationRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dopplerFactor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox volume;
        private System.Windows.Forms.CheckBox priority;
        private System.Windows.Forms.CheckBox pan;
        private System.Windows.Forms.CheckBox surroundPan;
        private System.Windows.Forms.CheckBox filter;
        private System.Windows.Forms.NumericUpDown dopplerFactor;
        private System.Windows.Forms.NumericUpDown attenuationRate;
        private System.Windows.Forms.ComboBox attenuationCurve;
        private System.Windows.Forms.CheckBox unknownFlag;
        private System.Windows.Forms.Button okButton;
    }
}