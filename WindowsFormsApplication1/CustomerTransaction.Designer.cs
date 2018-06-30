namespace WindowsFormsApplication1
{
    partial class CustomerTransaction
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
            this.btnTxnCount = new System.Windows.Forms.Button();
            this.btnLoan = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddTxn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCollectionAmount = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnBalance = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblLastDate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTxnCount
            // 
            this.btnTxnCount.Location = new System.Drawing.Point(2, 23);
            this.btnTxnCount.Name = "btnTxnCount";
            this.btnTxnCount.Size = new System.Drawing.Size(112, 50);
            this.btnTxnCount.TabIndex = 0;
            this.btnTxnCount.Text = "[NoOfTxn]";
            this.btnTxnCount.UseVisualStyleBackColor = true;
            // 
            // btnLoan
            // 
            this.btnLoan.Location = new System.Drawing.Point(120, 23);
            this.btnLoan.Name = "btnLoan";
            this.btnLoan.Size = new System.Drawing.Size(112, 50);
            this.btnLoan.TabIndex = 1;
            this.btnLoan.Text = "[Loan]";
            this.btnLoan.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddTxn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCollectionAmount);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Location = new System.Drawing.Point(356, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 96);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add New Transaction";
            // 
            // btnAddTxn
            // 
            this.btnAddTxn.Location = new System.Drawing.Point(146, 49);
            this.btnAddTxn.Name = "btnAddTxn";
            this.btnAddTxn.Size = new System.Drawing.Size(112, 26);
            this.btnAddTxn.TabIndex = 4;
            this.btnAddTxn.Text = "Add Transaction";
            this.btnAddTxn.UseVisualStyleBackColor = true;
            this.btnAddTxn.Click += new System.EventHandler(this.btnAddTxn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Collection Amount";
            // 
            // txtCollectionAmount
            // 
            this.txtCollectionAmount.Location = new System.Drawing.Point(276, 24);
            this.txtCollectionAmount.Name = "txtCollectionAmount";
            this.txtCollectionAmount.Size = new System.Drawing.Size(100, 20);
            this.txtCollectionAmount.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(38, 23);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // btnBalance
            // 
            this.btnBalance.Location = new System.Drawing.Point(238, 23);
            this.btnBalance.Name = "btnBalance";
            this.btnBalance.Size = new System.Drawing.Size(112, 50);
            this.btnBalance.TabIndex = 4;
            this.btnBalance.Text = "[balance]";
            this.btnBalance.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(12, 90);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(86, 31);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "label2";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 183);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(714, 307);
            this.dataGridView1.TabIndex = 6;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(477, 121);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 13);
            this.lblStartDate.TabIndex = 7;
            this.lblStartDate.Text = "Start Date:";
            // 
            // lblLastDate
            // 
            this.lblLastDate.AutoSize = true;
            this.lblLastDate.Location = new System.Drawing.Point(477, 148);
            this.lblLastDate.Name = "lblLastDate";
            this.lblLastDate.Size = new System.Drawing.Size(56, 13);
            this.lblLastDate.TabIndex = 8;
            this.lblLastDate.Text = "Last Date:";
            // 
            // CustomerTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 517);
            this.Controls.Add(this.lblLastDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnBalance);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLoan);
            this.Controls.Add(this.btnTxnCount);
            this.Name = "CustomerTransaction";
            this.Text = "CustomerDetail";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTxnCount;
        private System.Windows.Forms.Button btnLoan;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCollectionAmount;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnAddTxn;
        private System.Windows.Forms.Button btnBalance;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblLastDate;
    }
}