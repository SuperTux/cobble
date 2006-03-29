namespace Cobble {
    partial class SectorResizeDialog {
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.udWidth = new System.Windows.Forms.NumericUpDown();
            this.udHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(202, 50);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(121, 50);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // udWidth
            // 
            this.udWidth.Location = new System.Drawing.Point(12, 12);
            this.udWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udWidth.Name = "udWidth";
            this.udWidth.Size = new System.Drawing.Size(120, 20);
            this.udWidth.TabIndex = 2;
            this.udWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // udHeight
            // 
            this.udHeight.Location = new System.Drawing.Point(156, 12);
            this.udHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udHeight.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udHeight.Name = "udHeight";
            this.udHeight.Size = new System.Drawing.Size(120, 20);
            this.udHeight.TabIndex = 3;
            this.udHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "x";
            // 
            // SectorResizeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 85);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udHeight);
            this.Controls.Add(this.udWidth);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "SectorResizeDialog";
            this.Text = "SectorResizeDialog";
            ((System.ComponentModel.ISupportInitialize)(this.udWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown udWidth;
        private System.Windows.Forms.NumericUpDown udHeight;
        private System.Windows.Forms.Label label1;
    }
}