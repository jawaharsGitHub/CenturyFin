namespace WindowsFormsApplication1
{
    partial class frmInHand
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCollectionAmount = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtInterest = new System.Windows.Forms.TextBox();
            this.txtGivenAmount = new System.Windows.Forms.TextBox();
            this.txtTmrNeeded = new System.Windows.Forms.TextBox();
            this.txtOpened = new System.Windows.Forms.TextBox();
            this.txtClosed = new System.Windows.Forms.TextBox();
            this.txtTakenFromBank = new System.Windows.Forms.TextBox();
            this.txtBankTxnOut = new System.Windows.Forms.TextBox();
            this.txtSentFromUSA = new System.Windows.Forms.TextBox();
            this.txtSanthanam = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnTodayInHand = new System.Windows.Forms.Button();
            this.btnInBank = new System.Windows.Forms.Button();
            this.btnTmrWanted = new System.Windows.Forms.Button();
            this.btnYesterdayInHand = new System.Windows.Forms.Button();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDate);
            this.groupBox1.Controls.Add(this.txtCollectionAmount);
            this.groupBox1.Controls.Add(this.txtInterest);
            this.groupBox1.Controls.Add(this.txtGivenAmount);
            this.groupBox1.Controls.Add(this.txtTmrNeeded);
            this.groupBox1.Controls.Add(this.txtOpened);
            this.groupBox1.Controls.Add(this.txtClosed);
            this.groupBox1.Controls.Add(this.txtTakenFromBank);
            this.groupBox1.Controls.Add(this.txtBankTxnOut);
            this.groupBox1.Controls.Add(this.txtSentFromUSA);
            this.groupBox1.Controls.Add(this.txtSanthanam);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(35, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 468);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // txtCollectionAmount
            // 
            this.txtCollectionAmount.Location = new System.Drawing.Point(156, 224);
            this.txtCollectionAmount.Name = "txtCollectionAmount";
            this.txtCollectionAmount.Size = new System.Drawing.Size(100, 20);
            this.txtCollectionAmount.TabIndex = 5;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(499, 46);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 52);
            this.btnUpdate.TabIndex = 23;
            this.btnUpdate.Text = "UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtInterest
            // 
            this.txtInterest.Location = new System.Drawing.Point(153, 292);
            this.txtInterest.Name = "txtInterest";
            this.txtInterest.Size = new System.Drawing.Size(100, 20);
            this.txtInterest.TabIndex = 7;
            // 
            // txtGivenAmount
            // 
            this.txtGivenAmount.Location = new System.Drawing.Point(153, 258);
            this.txtGivenAmount.Name = "txtGivenAmount";
            this.txtGivenAmount.Size = new System.Drawing.Size(100, 20);
            this.txtGivenAmount.TabIndex = 6;
            // 
            // txtTmrNeeded
            // 
            this.txtTmrNeeded.Location = new System.Drawing.Point(153, 416);
            this.txtTmrNeeded.Name = "txtTmrNeeded";
            this.txtTmrNeeded.Size = new System.Drawing.Size(100, 20);
            this.txtTmrNeeded.TabIndex = 10;
            // 
            // txtOpened
            // 
            this.txtOpened.Location = new System.Drawing.Point(153, 380);
            this.txtOpened.Name = "txtOpened";
            this.txtOpened.Size = new System.Drawing.Size(100, 20);
            this.txtOpened.TabIndex = 9;
            // 
            // txtClosed
            // 
            this.txtClosed.Location = new System.Drawing.Point(153, 349);
            this.txtClosed.Name = "txtClosed";
            this.txtClosed.Size = new System.Drawing.Size(100, 20);
            this.txtClosed.TabIndex = 8;
            // 
            // txtTakenFromBank
            // 
            this.txtTakenFromBank.Location = new System.Drawing.Point(153, 193);
            this.txtTakenFromBank.Name = "txtTakenFromBank";
            this.txtTakenFromBank.Size = new System.Drawing.Size(100, 20);
            this.txtTakenFromBank.TabIndex = 4;
            // 
            // txtBankTxnOut
            // 
            this.txtBankTxnOut.Location = new System.Drawing.Point(153, 153);
            this.txtBankTxnOut.Name = "txtBankTxnOut";
            this.txtBankTxnOut.Size = new System.Drawing.Size(100, 20);
            this.txtBankTxnOut.TabIndex = 3;
            // 
            // txtSentFromUSA
            // 
            this.txtSentFromUSA.Location = new System.Drawing.Point(153, 83);
            this.txtSentFromUSA.Name = "txtSentFromUSA";
            this.txtSentFromUSA.Size = new System.Drawing.Size(100, 20);
            this.txtSentFromUSA.TabIndex = 2;
            // 
            // txtSanthanam
            // 
            this.txtSanthanam.Location = new System.Drawing.Point(153, 51);
            this.txtSanthanam.Name = "txtSanthanam";
            this.txtSanthanam.Size = new System.Drawing.Size(100, 20);
            this.txtSanthanam.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(35, 416);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "TomorrowNeed";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(39, 380);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "OpenedAccounts";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(36, 349);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "ClosedAccounts";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 295);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Interest";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 261);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "GivenAmount";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "CollectionAmount";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "TakenFromBank";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "BankTxnOut";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "SendFromUSA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "SanthanamUncle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(513, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "YesterdayAmountInHand";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(533, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "InBank";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(533, 241);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "TodayInHand";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(533, 313);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(95, 13);
            this.label15.TabIndex = 13;
            this.label15.Text = "Tomorrow Wanted";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(484, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // btnTodayInHand
            // 
            this.btnTodayInHand.Location = new System.Drawing.Point(633, 241);
            this.btnTodayInHand.Name = "btnTodayInHand";
            this.btnTodayInHand.Size = new System.Drawing.Size(75, 23);
            this.btnTodayInHand.TabIndex = 15;
            this.btnTodayInHand.Text = "button1";
            this.btnTodayInHand.UseVisualStyleBackColor = true;
            // 
            // btnInBank
            // 
            this.btnInBank.Location = new System.Drawing.Point(633, 270);
            this.btnInBank.Name = "btnInBank";
            this.btnInBank.Size = new System.Drawing.Size(75, 23);
            this.btnInBank.TabIndex = 16;
            this.btnInBank.Text = "button1";
            this.btnInBank.UseVisualStyleBackColor = true;
            // 
            // btnTmrWanted
            // 
            this.btnTmrWanted.Location = new System.Drawing.Point(635, 313);
            this.btnTmrWanted.Name = "btnTmrWanted";
            this.btnTmrWanted.Size = new System.Drawing.Size(75, 23);
            this.btnTmrWanted.TabIndex = 17;
            this.btnTmrWanted.Text = "button1";
            this.btnTmrWanted.UseVisualStyleBackColor = true;
            // 
            // btnYesterdayInHand
            // 
            this.btnYesterdayInHand.Location = new System.Drawing.Point(644, 200);
            this.btnYesterdayInHand.Name = "btnYesterdayInHand";
            this.btnYesterdayInHand.Size = new System.Drawing.Size(75, 23);
            this.btnYesterdayInHand.TabIndex = 18;
            this.btnYesterdayInHand.Text = "button1";
            this.btnYesterdayInHand.UseVisualStyleBackColor = true;
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(516, 370);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(192, 89);
            this.txtComments.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(516, 351);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Comments";
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(609, 46);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 52);
            this.btnShow.TabIndex = 20;
            this.btnShow.Text = "SHOW";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(81, 16);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(36, 13);
            this.lblDate.TabIndex = 24;
            this.lblDate.Text = "[Date]";
            // 
            // frmInHand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 528);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.btnYesterdayInHand);
            this.Controls.Add(this.btnTmrWanted);
            this.Controls.Add(this.btnInBank);
            this.Controls.Add(this.btnTodayInHand);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label1);
            this.Name = "frmInHand";
            this.Text = "In Hand Money Details";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtTmrNeeded;
        private System.Windows.Forms.TextBox txtOpened;
        private System.Windows.Forms.TextBox txtClosed;
        private System.Windows.Forms.TextBox txtTakenFromBank;
        private System.Windows.Forms.TextBox txtBankTxnOut;
        private System.Windows.Forms.TextBox txtSentFromUSA;
        private System.Windows.Forms.TextBox txtSanthanam;
        private System.Windows.Forms.TextBox txtInterest;
        private System.Windows.Forms.TextBox txtGivenAmount;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnTodayInHand;
        private System.Windows.Forms.Button btnInBank;
        private System.Windows.Forms.Button btnTmrWanted;
        private System.Windows.Forms.Button btnYesterdayInHand;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtCollectionAmount;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label lblDate;
    }
}