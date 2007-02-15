namespace Cobble {
    partial class TileSearchDialog {
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.udNewId = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.udOldId = new System.Windows.Forms.NumericUpDown();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udNewId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOldId)).BeginInit();
            this.SuspendLayout();
            //
            // btnOk
            //
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(254, 105);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(173, 105);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.udNewId);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.udOldId);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(317, 75);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Replace Tile Ids";
            //
            // udNewId
            //
            this.udNewId.Location = new System.Drawing.Point(168, 33);
            this.udNewId.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.udNewId.Name = "udNewId";
            this.udNewId.Size = new System.Drawing.Size(120, 20);
            this.udNewId.TabIndex = 3;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "->";
            //
            // udOldId
            //
            this.udOldId.Location = new System.Drawing.Point(24, 33);
            this.udOldId.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.udOldId.Name = "udOldId";
            this.udOldId.Size = new System.Drawing.Size(120, 20);
            this.udOldId.TabIndex = 2;
            //
            // TileSearchDialog
            //
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(341, 140);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "TileSearchDialog";
            this.Text = "Search and Replace Tiles";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udNewId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udOldId)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown udNewId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown udOldId;
    }
}
