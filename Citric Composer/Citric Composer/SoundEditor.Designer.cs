namespace Citric_Composer {
    partial class SoundEditor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.remoteFilter = new System.Windows.Forms.NumericUpDown();
            this.volume = new System.Windows.Forms.NumericUpDown();
            this.playerActorId = new System.Windows.Forms.ComboBox();
            this.player = new System.Windows.Forms.ComboBox();
            this.panMode = new System.Windows.Forms.ComboBox();
            this.p3 = new System.Windows.Forms.NumericUpDown();
            this.p1 = new System.Windows.Forms.NumericUpDown();
            this.p2 = new System.Windows.Forms.NumericUpDown();
            this.p4 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.playerPriority = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.frontBypass = new System.Windows.Forms.CheckBox();
            this.p1Enable = new System.Windows.Forms.CheckBox();
            this.panCurve = new System.Windows.Forms.ComboBox();
            this.p2Enable = new System.Windows.Forms.CheckBox();
            this.p3Enable = new System.Windows.Forms.CheckBox();
            this.p4Enable = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.remoteFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerPriority)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.okButton);
            this.splitContainer1.Size = new System.Drawing.Size(607, 199);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 23;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.9303F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.4168F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.24053F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.08731F));
            this.tableLayoutPanel1.Controls.Add(this.remoteFilter, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.volume, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.playerActorId, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.player, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panMode, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.p3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.p1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.p2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.p4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.playerPriority, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.frontBypass, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.p1Enable, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panCurve, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.p2Enable, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.p3Enable, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.p4Enable, 3, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66944F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(607, 172);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // remoteFilter
            // 
            this.remoteFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteFilter.Location = new System.Drawing.Point(345, 87);
            this.remoteFilter.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.remoteFilter.Name = "remoteFilter";
            this.remoteFilter.Size = new System.Drawing.Size(129, 20);
            this.remoteFilter.TabIndex = 30;
            // 
            // volume
            // 
            this.volume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volume.Location = new System.Drawing.Point(480, 31);
            this.volume.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.volume.Name = "volume";
            this.volume.Size = new System.Drawing.Size(124, 20);
            this.volume.TabIndex = 29;
            // 
            // playerActorId
            // 
            this.playerActorId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerActorId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerActorId.FormattingEnabled = true;
            this.playerActorId.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.playerActorId.Location = new System.Drawing.Point(345, 31);
            this.playerActorId.Name = "playerActorId";
            this.playerActorId.Size = new System.Drawing.Size(129, 21);
            this.playerActorId.TabIndex = 28;
            // 
            // player
            // 
            this.player.Dock = System.Windows.Forms.DockStyle.Fill;
            this.player.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.player.FormattingEnabled = true;
            this.player.Location = new System.Drawing.Point(3, 31);
            this.player.Name = "player";
            this.player.Size = new System.Drawing.Size(206, 21);
            this.player.TabIndex = 27;
            // 
            // panMode
            // 
            this.panMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.panMode.FormattingEnabled = true;
            this.panMode.Items.AddRange(new object[] {
            "Dual",
            "Balance"});
            this.panMode.Location = new System.Drawing.Point(3, 87);
            this.panMode.Name = "panMode";
            this.panMode.Size = new System.Drawing.Size(206, 21);
            this.panMode.TabIndex = 26;
            // 
            // p3
            // 
            this.p3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p3.Location = new System.Drawing.Point(345, 143);
            this.p3.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.p3.Name = "p3";
            this.p3.Size = new System.Drawing.Size(129, 20);
            this.p3.TabIndex = 25;
            // 
            // p1
            // 
            this.p1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p1.Location = new System.Drawing.Point(3, 143);
            this.p1.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.p1.Name = "p1";
            this.p1.Size = new System.Drawing.Size(206, 20);
            this.p1.TabIndex = 24;
            // 
            // p2
            // 
            this.p2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p2.Location = new System.Drawing.Point(215, 143);
            this.p2.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.p2.Name = "p2";
            this.p2.Size = new System.Drawing.Size(124, 20);
            this.p2.TabIndex = 23;
            // 
            // p4
            // 
            this.p4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p4.Location = new System.Drawing.Point(480, 143);
            this.p4.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.p4.Name = "p4";
            this.p4.Size = new System.Drawing.Size(124, 20);
            this.p4.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(345, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 28);
            this.label8.TabIndex = 17;
            this.label8.Text = "Remote Filter:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(480, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 28);
            this.label7.TabIndex = 16;
            this.label7.Text = "Volume:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(345, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 28);
            this.label6.TabIndex = 15;
            this.label6.Text = "Player Actor Id:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(206, 28);
            this.label5.TabIndex = 14;
            this.label5.Text = "Pan Mode:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playerPriority
            // 
            this.playerPriority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerPriority.Location = new System.Drawing.Point(215, 31);
            this.playerPriority.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.playerPriority.Name = "playerPriority";
            this.playerPriority.Size = new System.Drawing.Size(124, 20);
            this.playerPriority.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(215, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pan Curve:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(215, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Player Priority:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Player:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frontBypass
            // 
            this.frontBypass.AutoSize = true;
            this.frontBypass.Location = new System.Drawing.Point(480, 87);
            this.frontBypass.Name = "frontBypass";
            this.frontBypass.Size = new System.Drawing.Size(87, 17);
            this.frontBypass.TabIndex = 6;
            this.frontBypass.Text = "Front Bypass";
            this.frontBypass.UseVisualStyleBackColor = true;
            // 
            // p1Enable
            // 
            this.p1Enable.AutoSize = true;
            this.p1Enable.Location = new System.Drawing.Point(3, 115);
            this.p1Enable.Name = "p1Enable";
            this.p1Enable.Size = new System.Drawing.Size(122, 17);
            this.p1Enable.TabIndex = 8;
            this.p1Enable.Text = "Enable Parameter 1:";
            this.p1Enable.UseVisualStyleBackColor = true;
            this.p1Enable.CheckedChanged += new System.EventHandler(this.P1Enable_CheckedChanged);
            // 
            // panCurve
            // 
            this.panCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.panCurve.FormattingEnabled = true;
            this.panCurve.Items.AddRange(new object[] {
            "Sqrt (-3dB)",
            "Sqrt (0dB)",
            "Sqrt (0dB, Clamp)",
            "SinCos (-3dB)",
            "SinCos (0dB)",
            "SinCos (0dB, Clamp)",
            "Linear (-6dB)",
            "Linear (0dB)",
            "Linear (0dB, Clamp)"});
            this.panCurve.Location = new System.Drawing.Point(215, 87);
            this.panCurve.Name = "panCurve";
            this.panCurve.Size = new System.Drawing.Size(124, 21);
            this.panCurve.TabIndex = 11;
            // 
            // p2Enable
            // 
            this.p2Enable.AutoSize = true;
            this.p2Enable.Location = new System.Drawing.Point(215, 115);
            this.p2Enable.Name = "p2Enable";
            this.p2Enable.Size = new System.Drawing.Size(122, 17);
            this.p2Enable.TabIndex = 19;
            this.p2Enable.Text = "Enable Parameter 2:";
            this.p2Enable.UseVisualStyleBackColor = true;
            this.p2Enable.CheckedChanged += new System.EventHandler(this.P2Enable_CheckedChanged);
            // 
            // p3Enable
            // 
            this.p3Enable.AutoSize = true;
            this.p3Enable.Location = new System.Drawing.Point(345, 115);
            this.p3Enable.Name = "p3Enable";
            this.p3Enable.Size = new System.Drawing.Size(122, 17);
            this.p3Enable.TabIndex = 20;
            this.p3Enable.Text = "Enable Parameter 3:";
            this.p3Enable.UseVisualStyleBackColor = true;
            this.p3Enable.CheckedChanged += new System.EventHandler(this.P3Enable_CheckedChanged);
            // 
            // p4Enable
            // 
            this.p4Enable.AutoSize = true;
            this.p4Enable.Location = new System.Drawing.Point(480, 115);
            this.p4Enable.Name = "p4Enable";
            this.p4Enable.Size = new System.Drawing.Size(122, 17);
            this.p4Enable.TabIndex = 21;
            this.p4Enable.Text = "Enable Parameter 4:";
            this.p4Enable.UseVisualStyleBackColor = true;
            this.p4Enable.CheckedChanged += new System.EventHandler(this.P4Enable_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(0, 0);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(607, 25);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // SoundEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 199);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoundEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sound Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.remoteFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerPriority)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown p3;
        private System.Windows.Forms.NumericUpDown p1;
        private System.Windows.Forms.NumericUpDown p2;
        private System.Windows.Forms.NumericUpDown p4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown playerPriority;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox frontBypass;
        private System.Windows.Forms.CheckBox p1Enable;
        private System.Windows.Forms.ComboBox panCurve;
        private System.Windows.Forms.CheckBox p2Enable;
        private System.Windows.Forms.CheckBox p3Enable;
        private System.Windows.Forms.CheckBox p4Enable;
        private System.Windows.Forms.ComboBox panMode;
        private System.Windows.Forms.ComboBox player;
        private System.Windows.Forms.ComboBox playerActorId;
        private System.Windows.Forms.NumericUpDown remoteFilter;
        private System.Windows.Forms.NumericUpDown volume;
    }
}