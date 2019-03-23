namespace Citric_Composer {
    partial class VersionSelector {
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
            this.maj = new System.Windows.Forms.NumericUpDown();
            this.min = new System.Windows.Forms.NumericUpDown();
            this.rev = new System.Windows.Forms.NumericUpDown();
            this.setVer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.maj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rev)).BeginInit();
            this.SuspendLayout();
            // 
            // maj
            // 
            this.maj.Location = new System.Drawing.Point(12, 12);
            this.maj.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maj.Name = "maj";
            this.maj.Size = new System.Drawing.Size(89, 20);
            this.maj.TabIndex = 0;
            // 
            // min
            // 
            this.min.Location = new System.Drawing.Point(107, 12);
            this.min.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(89, 20);
            this.min.TabIndex = 1;
            // 
            // rev
            // 
            this.rev.Location = new System.Drawing.Point(202, 12);
            this.rev.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.rev.Name = "rev";
            this.rev.Size = new System.Drawing.Size(89, 20);
            this.rev.TabIndex = 2;
            // 
            // setVer
            // 
            this.setVer.Location = new System.Drawing.Point(12, 38);
            this.setVer.Name = "setVer";
            this.setVer.Size = new System.Drawing.Size(279, 23);
            this.setVer.TabIndex = 3;
            this.setVer.Text = "Set Version";
            this.setVer.UseVisualStyleBackColor = true;
            this.setVer.Click += new System.EventHandler(this.setVer_Click);
            // 
            // VersionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 67);
            this.ControlBox = false;
            this.Controls.Add(this.setVer);
            this.Controls.Add(this.rev);
            this.Controls.Add(this.min);
            this.Controls.Add(this.maj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "VersionSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Version Selector";
            ((System.ComponentModel.ISupportInitialize)(this.maj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rev)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.NumericUpDown maj;
        public System.Windows.Forms.NumericUpDown min;
        public System.Windows.Forms.NumericUpDown rev;
        private System.Windows.Forms.Button setVer;
    }
}