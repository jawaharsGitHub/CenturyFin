namespace CenturyFinCorpApp
{
    partial class frmReport
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblOutStanding = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalAsset = new System.Windows.Forms.Label();
            this.btnClosedTxn = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "OutStanding Money";
            // 
            // lblOutStanding
            // 
            this.lblOutStanding.AutoSize = true;
            this.lblOutStanding.Location = new System.Drawing.Point(182, 37);
            this.lblOutStanding.Name = "lblOutStanding";
            this.lblOutStanding.Size = new System.Drawing.Size(111, 13);
            this.lblOutStanding.TabIndex = 1;
            this.lblOutStanding.Text = "[OutSTanding Money]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total Asset";
            // 
            // lblTotalAsset
            // 
            this.lblTotalAsset.AutoSize = true;
            this.lblTotalAsset.Location = new System.Drawing.Point(185, 81);
            this.lblTotalAsset.Name = "lblTotalAsset";
            this.lblTotalAsset.Size = new System.Drawing.Size(63, 13);
            this.lblTotalAsset.TabIndex = 3;
            this.lblTotalAsset.Text = "[TotalAsset]";
            // 
            // btnClosedTxn
            // 
            this.btnClosedTxn.BackColor = System.Drawing.Color.Aqua;
            this.btnClosedTxn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosedTxn.Location = new System.Drawing.Point(568, 10);
            this.btnClosedTxn.Name = "btnClosedTxn";
            this.btnClosedTxn.Size = new System.Drawing.Size(229, 84);
            this.btnClosedTxn.TabIndex = 5;
            this.btnClosedTxn.Text = "Run Closed Txn";
            this.btnClosedTxn.UseVisualStyleBackColor = false;
            this.btnClosedTxn.Click += new System.EventHandler(this.btnClosedTxn_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Aqua;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(319, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(229, 84);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "Refresh Data";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(837, 152);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(335, 367);
            this.dataGridView1.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(837, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "NOTE PER MONTH:";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(3, 149);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(697, 367);
            this.dataGridView2.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "TO BE CLOSED SOON:";
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClosedTxn);
            this.Controls.Add(this.lblTotalAsset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblOutStanding);
            this.Controls.Add(this.label1);
            this.Name = "frmReport";
            this.Size = new System.Drawing.Size(1266, 519);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOutStanding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalAsset;
        private System.Windows.Forms.Button btnClosedTxn;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label4;
    }
}
