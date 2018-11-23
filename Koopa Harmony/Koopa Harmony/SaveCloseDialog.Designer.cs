namespace KoopaHarmony
{
    partial class SaveCloseDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.NoButton = new System.Windows.Forms.Button();
            this.YesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(73, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Do you want to save before you close the file?\r\n";
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(214, 47);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(95, 30);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NoButton
            // 
            this.NoButton.Location = new System.Drawing.Point(113, 47);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(95, 30);
            this.NoButton.TabIndex = 5;
            this.NoButton.Text = "Close";
            this.NoButton.UseVisualStyleBackColor = true;
            this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
            // 
            // YesButton
            // 
            this.YesButton.Location = new System.Drawing.Point(12, 47);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(95, 30);
            this.YesButton.TabIndex = 4;
            this.YesButton.Text = "Save and Close";
            this.YesButton.UseVisualStyleBackColor = true;
            this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // SaveCloseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 87);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.YesButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveCloseDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Warning";
            this.Load += new System.EventHandler(this.SaveCloseDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button NoButton;
        private System.Windows.Forms.Button YesButton;
    }
}