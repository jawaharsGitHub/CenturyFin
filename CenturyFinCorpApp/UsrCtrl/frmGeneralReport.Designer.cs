namespace CenturyFinCorpApp.UsrCtrl
{
    partial class frmGeneralReport
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvIncome = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblActual = new System.Windows.Forms.Label();
            this.lblExpected = new System.Windows.Forms.Label();
            this.chkAddSalary = new System.Windows.Forms.CheckBox();
            this.dgvNotePerMonth = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIncome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotePerMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvIncome
            // 
            this.dgvIncome.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIncome.Location = new System.Drawing.Point(15, 107);
            this.dgvIncome.Name = "dgvIncome";
            this.dgvIncome.Size = new System.Drawing.Size(445, 405);
            this.dgvIncome.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(170, 76);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(42, 13);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "TOTAL";
            // 
            // lblActual
            // 
            this.lblActual.AutoSize = true;
            this.lblActual.Location = new System.Drawing.Point(167, 49);
            this.lblActual.Name = "lblActual";
            this.lblActual.Size = new System.Drawing.Size(37, 13);
            this.lblActual.TabIndex = 11;
            this.lblActual.Text = "Actual";
            // 
            // lblExpected
            // 
            this.lblExpected.AutoSize = true;
            this.lblExpected.Location = new System.Drawing.Point(167, 18);
            this.lblExpected.Name = "lblExpected";
            this.lblExpected.Size = new System.Drawing.Size(52, 13);
            this.lblExpected.TabIndex = 10;
            this.lblExpected.Text = "Expected";
            // 
            // chkAddSalary
            // 
            this.chkAddSalary.AutoSize = true;
            this.chkAddSalary.Location = new System.Drawing.Point(40, 17);
            this.chkAddSalary.Name = "chkAddSalary";
            this.chkAddSalary.Size = new System.Drawing.Size(105, 17);
            this.chkAddSalary.TabIndex = 9;
            this.chkAddSalary.Text = "Consider Salary?";
            this.chkAddSalary.UseVisualStyleBackColor = true;
            this.chkAddSalary.CheckedChanged += new System.EventHandler(this.chkAddSalary_CheckedChanged);
            // 
            // dgvNotePerMonth
            // 
            this.dgvNotePerMonth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotePerMonth.Location = new System.Drawing.Point(521, 107);
            this.dgvNotePerMonth.Name = "dgvNotePerMonth";
            this.dgvNotePerMonth.Size = new System.Drawing.Size(547, 405);
            this.dgvNotePerMonth.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(543, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "NOTE PER MONTH:";
            // 
            // frmGeneralReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvNotePerMonth);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblActual);
            this.Controls.Add(this.lblExpected);
            this.Controls.Add(this.chkAddSalary);
            this.Controls.Add(this.dgvIncome);
            this.Name = "frmGeneralReport";
            this.Size = new System.Drawing.Size(1071, 530);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIncome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotePerMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIncome;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblActual;
        private System.Windows.Forms.Label lblExpected;
        private System.Windows.Forms.CheckBox chkAddSalary;
        private System.Windows.Forms.DataGridView dgvNotePerMonth;
        private System.Windows.Forms.Label label3;
    }
}
