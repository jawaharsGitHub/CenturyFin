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
            this.btnClosedTxn = new System.Windows.Forms.Button();
            this.dgReports = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgReports)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClosedTxn
            // 
            this.btnClosedTxn.BackColor = System.Drawing.Color.Aqua;
            this.btnClosedTxn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosedTxn.Location = new System.Drawing.Point(92, 21);
            this.btnClosedTxn.Name = "btnClosedTxn";
            this.btnClosedTxn.Size = new System.Drawing.Size(229, 84);
            this.btnClosedTxn.TabIndex = 5;
            this.btnClosedTxn.Text = "Run Closed Txn";
            this.btnClosedTxn.UseVisualStyleBackColor = false;
            this.btnClosedTxn.Click += new System.EventHandler(this.btnClosedTxn_Click);
            // 
            // dgReports
            // 
            this.dgReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgReports.Location = new System.Drawing.Point(3, 149);
            this.dgReports.Name = "dgReports";
            this.dgReports.Size = new System.Drawing.Size(763, 367);
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
            // frmDynamicReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dgReports);
            this.Controls.Add(this.btnClosedTxn);
            this.Name = "frmDynamicReport";
            this.Size = new System.Drawing.Size(1266, 519);
            ((System.ComponentModel.ISupportInitialize)(this.dgReports)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClosedTxn;
        private System.Windows.Forms.DataGridView dgReports;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
