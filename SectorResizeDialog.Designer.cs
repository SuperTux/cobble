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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.udOffsetY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.udOffsetX = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.udWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetX)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(254, 174);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(173, 174);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // udWidth
            // 
            this.udWidth.Location = new System.Drawing.Point(24, 33);
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
            this.udHeight.Location = new System.Drawing.Point(168, 33);
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
            this.label1.Location = new System.Drawing.Point(150, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "x";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.udHeight);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.udWidth);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(317, 75);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resize To";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.udOffsetY);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.udOffsetX);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(317, 75);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Offset By";
            // 
            // udOffsetY
            // 
            this.udOffsetY.Location = new System.Drawing.Point(168, 33);
            this.udOffsetY.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.udOffsetY.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            -2147483648});
            this.udOffsetY.Name = "udOffsetY";
            this.udOffsetY.Size = new System.Drawing.Size(120, 20);
            this.udOffsetY.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = ",";
            // 
            // udOffsetX
            // 
            this.udOffsetX.Location = new System.Drawing.Point(24, 33);
            this.udOffsetX.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.udOffsetX.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            -2147483648});
            this.udOffsetX.Name = "udOffsetX";
            this.udOffsetX.Size = new System.Drawing.Size(120, 20);
            this.udOffsetX.TabIndex = 2;
            // 
            // SectorResizeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 210);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "SectorResizeDialog";
            this.Text = "Sector Manipulation";
            ((System.ComponentModel.ISupportInitialize)(this.udWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOffsetX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown udWidth;
        private System.Windows.Forms.NumericUpDown udHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown udOffsetY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown udOffsetX;
    }
}