namespace CenturyFinCorpApp
{
    partial class frmCustomerTransaction
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
            this.btnLoan = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNextDayTxn = new System.Windows.Forms.Button();
            this.btnAddTxn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCollectionAmount = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnBalance = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblLastDate = new System.Windows.Forms.Label();
            this.rdbAsc = new System.Windows.Forms.RadioButton();
            this.rdbDesc = new System.Windows.Forms.RadioButton();
            this.lblNoOfDays = new System.Windows.Forms.Label();
            this.lblPercentageGain = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.chkByBalance = new System.Windows.Forms.CheckBox();
            this.btnCreditScore = new System.Windows.Forms.Button();
            this.cmbReturnType = new System.Windows.Forms.ComboBox();
            this.cmbReturnDay = new System.Windows.Forms.ComboBox();
            this.cmbCollectionSpot = new System.Windows.Forms.ComboBox();
            this.btnCorrect = new System.Windows.Forms.Button();
            this.btnInterest = new System.Windows.Forms.Button();
            this.grpMerge = new System.Windows.Forms.GroupBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnReOpen = new System.Windows.Forms.Button();
            this.btnForceClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTopupInterest = new System.Windows.Forms.TextBox();
            this.txtTopupAmount = new System.Windows.Forms.TextBox();
            this.btnTopup = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNewInterest = new System.Windows.Forms.TextBox();
            this.txtNewAmount = new System.Windows.Forms.TextBox();
            this.btnConvertToMonthly = new System.Windows.Forms.Button();
            this.cmbExistingCustomer = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.grpMerge.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoan
            // 
            this.btnLoan.Location = new System.Drawing.Point(3, 28);
            this.btnLoan.Name = "btnLoan";
            this.btnLoan.Size = new System.Drawing.Size(112, 50);
            this.btnLoan.TabIndex = 1;
            this.btnLoan.Text = "[Loan]";
            this.btnLoan.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnNextDayTxn);
            this.groupBox1.Controls.Add(this.btnAddTxn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCollectionAmount);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Location = new System.Drawing.Point(356, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 96);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add New Transaction";
            // 
            // btnNextDayTxn
            // 
            this.btnNextDayTxn.Location = new System.Drawing.Point(241, 49);
            this.btnNextDayTxn.Name = "btnNextDayTxn";
            this.btnNextDayTxn.Size = new System.Drawing.Size(75, 23);
            this.btnNextDayTxn.TabIndex = 5;
            this.btnNextDayTxn.Text = "Next Day";
            this.btnNextDayTxn.UseVisualStyleBackColor = true;
            this.btnNextDayTxn.Click += new System.EventHandler(this.btnNextDayTxn_Click);
            // 
            // btnAddTxn
            // 
            this.btnAddTxn.Location = new System.Drawing.Point(92, 49);
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
            this.label1.Location = new System.Drawing.Point(216, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Collection Amount";
            // 
            // txtCollectionAmount
            // 
            this.txtCollectionAmount.Location = new System.Drawing.Point(216, 24);
            this.txtCollectionAmount.Name = "txtCollectionAmount";
            this.txtCollectionAmount.Size = new System.Drawing.Size(100, 20);
            this.txtCollectionAmount.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(4, 23);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // btnBalance
            // 
            this.btnBalance.Location = new System.Drawing.Point(121, 28);
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
            this.dataGridView1.Size = new System.Drawing.Size(467, 307);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.Red;
            this.lblStartDate.Location = new System.Drawing.Point(353, 120);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(69, 13);
            this.lblStartDate.TabIndex = 7;
            this.lblStartDate.Text = "Start Date:";
            // 
            // lblLastDate
            // 
            this.lblLastDate.AutoSize = true;
            this.lblLastDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastDate.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblLastDate.Location = new System.Drawing.Point(353, 147);
            this.lblLastDate.Name = "lblLastDate";
            this.lblLastDate.Size = new System.Drawing.Size(66, 13);
            this.lblLastDate.TabIndex = 8;
            this.lblLastDate.Text = "Last Date:";
            // 
            // rdbAsc
            // 
            this.rdbAsc.AutoSize = true;
            this.rdbAsc.Location = new System.Drawing.Point(18, 143);
            this.rdbAsc.Name = "rdbAsc";
            this.rdbAsc.Size = new System.Drawing.Size(61, 17);
            this.rdbAsc.TabIndex = 9;
            this.rdbAsc.Text = "By ASC";
            this.rdbAsc.UseVisualStyleBackColor = true;
            this.rdbAsc.CheckedChanged += new System.EventHandler(this.rdbAsc_CheckedChanged);
            // 
            // rdbDesc
            // 
            this.rdbDesc.AutoSize = true;
            this.rdbDesc.Checked = true;
            this.rdbDesc.Location = new System.Drawing.Point(120, 143);
            this.rdbDesc.Name = "rdbDesc";
            this.rdbDesc.Size = new System.Drawing.Size(69, 17);
            this.rdbDesc.TabIndex = 10;
            this.rdbDesc.TabStop = true;
            this.rdbDesc.Text = "By DESC";
            this.rdbDesc.UseVisualStyleBackColor = true;
            this.rdbDesc.CheckedChanged += new System.EventHandler(this.rdbDesc_CheckedChanged);
            // 
            // lblNoOfDays
            // 
            this.lblNoOfDays.AutoSize = true;
            this.lblNoOfDays.Location = new System.Drawing.Point(576, 120);
            this.lblNoOfDays.Name = "lblNoOfDays";
            this.lblNoOfDays.Size = new System.Drawing.Size(62, 13);
            this.lblNoOfDays.TabIndex = 11;
            this.lblNoOfDays.Text = "No Of Days";
            // 
            // lblPercentageGain
            // 
            this.lblPercentageGain.AutoSize = true;
            this.lblPercentageGain.Location = new System.Drawing.Point(576, 147);
            this.lblPercentageGain.Name = "lblPercentageGain";
            this.lblPercentageGain.Size = new System.Drawing.Size(94, 13);
            this.lblPercentageGain.TabIndex = 12;
            this.lblPercentageGain.Text = "lblPercentageGain";
            // 
            // lblDetail
            // 
            this.lblDetail.AutoSize = true;
            this.lblDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetail.Location = new System.Drawing.Point(30, 6);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(101, 13);
            this.lblDetail.TabIndex = 13;
            this.lblDetail.Text = "[customer detail]";
            // 
            // chkByBalance
            // 
            this.chkByBalance.AutoSize = true;
            this.chkByBalance.Location = new System.Drawing.Point(201, 143);
            this.chkByBalance.Name = "chkByBalance";
            this.chkByBalance.Size = new System.Drawing.Size(109, 17);
            this.chkByBalance.TabIndex = 14;
            this.chkByBalance.Text = "Order By Balance";
            this.chkByBalance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkByBalance.UseVisualStyleBackColor = true;
            this.chkByBalance.CheckedChanged += new System.EventHandler(this.chkByBalance_CheckedChanged);
            // 
            // btnCreditScore
            // 
            this.btnCreditScore.BackColor = System.Drawing.Color.Gold;
            this.btnCreditScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreditScore.Location = new System.Drawing.Point(536, 359);
            this.btnCreditScore.Name = "btnCreditScore";
            this.btnCreditScore.Size = new System.Drawing.Size(175, 91);
            this.btnCreditScore.TabIndex = 15;
            this.btnCreditScore.Text = "[CREDIT SCORE]";
            this.btnCreditScore.UseVisualStyleBackColor = false;
            // 
            // cmbReturnType
            // 
            this.cmbReturnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnType.FormattingEnabled = true;
            this.cmbReturnType.Location = new System.Drawing.Point(536, 234);
            this.cmbReturnType.Name = "cmbReturnType";
            this.cmbReturnType.Size = new System.Drawing.Size(121, 21);
            this.cmbReturnType.TabIndex = 16;
            // 
            // cmbReturnDay
            // 
            this.cmbReturnDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnDay.FormattingEnabled = true;
            this.cmbReturnDay.Location = new System.Drawing.Point(536, 280);
            this.cmbReturnDay.Name = "cmbReturnDay";
            this.cmbReturnDay.Size = new System.Drawing.Size(121, 21);
            this.cmbReturnDay.TabIndex = 17;
            // 
            // cmbCollectionSpot
            // 
            this.cmbCollectionSpot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollectionSpot.FormattingEnabled = true;
            this.cmbCollectionSpot.Location = new System.Drawing.Point(536, 318);
            this.cmbCollectionSpot.Name = "cmbCollectionSpot";
            this.cmbCollectionSpot.Size = new System.Drawing.Size(250, 21);
            this.cmbCollectionSpot.TabIndex = 18;
            // 
            // btnCorrect
            // 
            this.btnCorrect.Location = new System.Drawing.Point(536, 467);
            this.btnCorrect.Name = "btnCorrect";
            this.btnCorrect.Size = new System.Drawing.Size(157, 23);
            this.btnCorrect.TabIndex = 19;
            this.btnCorrect.Text = "Correct Data";
            this.btnCorrect.UseVisualStyleBackColor = true;
            this.btnCorrect.Click += new System.EventHandler(this.btnCorrect_Click);
            // 
            // btnInterest
            // 
            this.btnInterest.Location = new System.Drawing.Point(239, 28);
            this.btnInterest.Name = "btnInterest";
            this.btnInterest.Size = new System.Drawing.Size(112, 50);
            this.btnInterest.TabIndex = 20;
            this.btnInterest.Text = "[Interest]";
            this.btnInterest.UseVisualStyleBackColor = true;
            // 
            // grpMerge
            // 
            this.grpMerge.Controls.Add(this.cmbExistingCustomer);
            this.grpMerge.Controls.Add(this.btnMerge);
            this.grpMerge.Location = new System.Drawing.Point(866, 7);
            this.grpMerge.Name = "grpMerge";
            this.grpMerge.Size = new System.Drawing.Size(330, 85);
            this.grpMerge.TabIndex = 21;
            this.grpMerge.TabStop = false;
            this.grpMerge.Text = "Merge";
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(19, 25);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(174, 21);
            this.btnMerge.TabIndex = 0;
            this.btnMerge.Text = "Merge To below account";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnReOpen
            // 
            this.btnReOpen.Location = new System.Drawing.Point(746, 368);
            this.btnReOpen.Name = "btnReOpen";
            this.btnReOpen.Size = new System.Drawing.Size(75, 57);
            this.btnReOpen.TabIndex = 22;
            this.btnReOpen.Text = "RE-OPEN";
            this.btnReOpen.UseVisualStyleBackColor = true;
            // 
            // btnForceClose
            // 
            this.btnForceClose.Location = new System.Drawing.Point(723, 467);
            this.btnForceClose.Name = "btnForceClose";
            this.btnForceClose.Size = new System.Drawing.Size(157, 23);
            this.btnForceClose.TabIndex = 23;
            this.btnForceClose.Text = "Force Close";
            this.btnForceClose.UseVisualStyleBackColor = true;
            this.btnForceClose.Click += new System.EventHandler(this.btnForceClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTopupInterest);
            this.groupBox2.Controls.Add(this.txtTopupAmount);
            this.groupBox2.Controls.Add(this.btnTopup);
            this.groupBox2.Location = new System.Drawing.Point(989, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 112);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TOP-UP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Interest";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Amount";
            // 
            // txtTopupInterest
            // 
            this.txtTopupInterest.Location = new System.Drawing.Point(85, 46);
            this.txtTopupInterest.Name = "txtTopupInterest";
            this.txtTopupInterest.Size = new System.Drawing.Size(100, 20);
            this.txtTopupInterest.TabIndex = 7;
            // 
            // txtTopupAmount
            // 
            this.txtTopupAmount.Location = new System.Drawing.Point(85, 19);
            this.txtTopupAmount.Name = "txtTopupAmount";
            this.txtTopupAmount.Size = new System.Drawing.Size(100, 20);
            this.txtTopupAmount.TabIndex = 6;
            this.txtTopupAmount.Leave += new System.EventHandler(this.txtTopupAmount_Leave);
            // 
            // btnTopup
            // 
            this.btnTopup.Location = new System.Drawing.Point(34, 85);
            this.btnTopup.Name = "btnTopup";
            this.btnTopup.Size = new System.Drawing.Size(119, 21);
            this.btnTopup.TabIndex = 0;
            this.btnTopup.Text = "TOP UP";
            this.btnTopup.UseVisualStyleBackColor = true;
            this.btnTopup.Click += new System.EventHandler(this.btnTopup_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtNewInterest);
            this.groupBox3.Controls.Add(this.txtNewAmount);
            this.groupBox3.Controls.Add(this.btnConvertToMonthly);
            this.groupBox3.Location = new System.Drawing.Point(989, 280);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 112);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Daily to Monthly";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "New Interest";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "New Amount";
            // 
            // txtNewInterest
            // 
            this.txtNewInterest.Location = new System.Drawing.Point(85, 46);
            this.txtNewInterest.Name = "txtNewInterest";
            this.txtNewInterest.Size = new System.Drawing.Size(100, 20);
            this.txtNewInterest.TabIndex = 7;
            // 
            // txtNewAmount
            // 
            this.txtNewAmount.Location = new System.Drawing.Point(85, 19);
            this.txtNewAmount.Name = "txtNewAmount";
            this.txtNewAmount.Size = new System.Drawing.Size(100, 20);
            this.txtNewAmount.TabIndex = 6;
            // 
            // btnConvertToMonthly
            // 
            this.btnConvertToMonthly.Location = new System.Drawing.Point(34, 85);
            this.btnConvertToMonthly.Name = "btnConvertToMonthly";
            this.btnConvertToMonthly.Size = new System.Drawing.Size(119, 21);
            this.btnConvertToMonthly.TabIndex = 0;
            this.btnConvertToMonthly.Text = "Convert";
            this.btnConvertToMonthly.UseVisualStyleBackColor = true;
            this.btnConvertToMonthly.Click += new System.EventHandler(this.btnConvertToMonthly_Click);
            // 
            // cmbExistingCustomer
            // 
            this.cmbExistingCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbExistingCustomer.FormattingEnabled = true;
            this.cmbExistingCustomer.Location = new System.Drawing.Point(19, 52);
            this.cmbExistingCustomer.Name = "cmbExistingCustomer";
            this.cmbExistingCustomer.Size = new System.Drawing.Size(294, 24);
            this.cmbExistingCustomer.TabIndex = 24;
            this.cmbExistingCustomer.TextChanged += new System.EventHandler(this.cmbExistingCustomer_TextChanged);
            // 
            // frmCustomerTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnForceClose);
            this.Controls.Add(this.btnReOpen);
            this.Controls.Add(this.grpMerge);
            this.Controls.Add(this.btnInterest);
            this.Controls.Add(this.btnCorrect);
            this.Controls.Add(this.cmbCollectionSpot);
            this.Controls.Add(this.cmbReturnDay);
            this.Controls.Add(this.cmbReturnType);
            this.Controls.Add(this.btnCreditScore);
            this.Controls.Add(this.chkByBalance);
            this.Controls.Add(this.lblDetail);
            this.Controls.Add(this.lblPercentageGain);
            this.Controls.Add(this.lblNoOfDays);
            this.Controls.Add(this.rdbDesc);
            this.Controls.Add(this.rdbAsc);
            this.Controls.Add(this.lblLastDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnBalance);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLoan);
            this.Name = "frmCustomerTransaction";
            this.Size = new System.Drawing.Size(1206, 517);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.grpMerge.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.RadioButton rdbAsc;
        private System.Windows.Forms.RadioButton rdbDesc;
        private System.Windows.Forms.Label lblNoOfDays;
        private System.Windows.Forms.Label lblPercentageGain;
        private System.Windows.Forms.Button btnNextDayTxn;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.CheckBox chkByBalance;
        private System.Windows.Forms.Button btnCreditScore;
        private System.Windows.Forms.ComboBox cmbReturnType;
        private System.Windows.Forms.ComboBox cmbReturnDay;
        private System.Windows.Forms.ComboBox cmbCollectionSpot;
        private System.Windows.Forms.Button btnCorrect;
        private System.Windows.Forms.Button btnInterest;
        private System.Windows.Forms.GroupBox grpMerge;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnReOpen;
        private System.Windows.Forms.Button btnForceClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTopupInterest;
        private System.Windows.Forms.TextBox txtTopupAmount;
        private System.Windows.Forms.Button btnTopup;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNewInterest;
        private System.Windows.Forms.TextBox txtNewAmount;
        private System.Windows.Forms.Button btnConvertToMonthly;
        private System.Windows.Forms.ComboBox cmbExistingCustomer;
    }
}