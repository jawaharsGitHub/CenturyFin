namespace CenturyFinCorpApp
{
    partial class frmDynamicReport
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
            this.dgReports = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblSeverity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgReports)).BeginInit();
            this.SuspendLayout();
            // 
            // dgReports
            // 
            this.dgReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgReports.Location = new System.Drawing.Point(3, 159);
            this.dgReports.Name = "dgReports";
            this.dgReports.Size = new System.Drawing.Size(931, 367);
            this.dgReports.TabIndex = 21;
            this.dgReports.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgReports_CellFormatting);
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "Value";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(411, 122);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(278, 21);
            this.comboBox1.TabIndex = 23;
            this.comboBox1.ValueMember = "Key";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblDetails
            // 
            this.lblDetails.Location = new System.Drawing.Point(0, 0);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(100, 23);
            this.lblDetails.TabIndex = 0;
            // 
            // lblSeverity
            // 
            this.lblSeverity.AutoSize = true;
            this.lblSeverity.Location = new System.Drawing.Point(735, 122);
            this.lblSeverity.Name = "lblSeverity";
            this.lblSeverity.Size = new System.Drawing.Size(51, 13);
            this.lblSeverity.TabIndex = 24;
            this.lblSeverity.Text = "[Severity]";
            // 
            // frmDynamicReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSeverity);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dgReports);
            this.Name = "frmDynamicReport";
            this.Size = new System.Drawing.Size(1266, 583);
            ((System.ComponentModel.ISupportInitialize)(this.dgReports)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgReports;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Label lblSeverity;
    }
}
