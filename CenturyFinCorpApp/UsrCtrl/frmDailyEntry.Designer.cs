﻿namespace CenturyFinCorpApp
{
    partial class frmDailyEntry
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkNotGivenCustomer = new System.Windows.Forms.CheckBox();
            this.dgvAllDailyCollection = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.dgAvgPerDay = new System.Windows.Forms.DataGridView();
            this.lblCollSpot = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmdFilter = new System.Windows.Forms.ComboBox();
            this.cmbAmountFilter = new System.Windows.Forms.ComboBox();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllDailyCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgAvgPerDay)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 245);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(578, 323);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(57, 9);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(440, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "ShowFromBatch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Coral;
            this.label1.Location = new System.Drawing.Point(52, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Coral;
            this.label2.Location = new System.Drawing.Point(52, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 29);
            this.label2.TabIndex = 9;
            this.label2.Text = "label2";
            // 
            // chkNotGivenCustomer
            // 
            this.chkNotGivenCustomer.AutoSize = true;
            this.chkNotGivenCustomer.Location = new System.Drawing.Point(492, 207);
            this.chkNotGivenCustomer.Name = "chkNotGivenCustomer";
            this.chkNotGivenCustomer.Size = new System.Drawing.Size(104, 17);
            this.chkNotGivenCustomer.TabIndex = 12;
            this.chkNotGivenCustomer.Text = "Show Not Given";
            this.chkNotGivenCustomer.UseVisualStyleBackColor = true;
            this.chkNotGivenCustomer.CheckedChanged += new System.EventHandler(this.chkNotGivenCustomer_CheckedChanged);
            // 
            // dgvAllDailyCollection
            // 
            this.dgvAllDailyCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllDailyCollection.Location = new System.Drawing.Point(623, 245);
            this.dgvAllDailyCollection.Name = "dgvAllDailyCollection";
            this.dgvAllDailyCollection.Size = new System.Drawing.Size(569, 361);
            this.dgvAllDailyCollection.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(626, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Daily Collection History";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(313, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "Show V-0";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(18, 9);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(33, 23);
            this.btnPrev.TabIndex = 16;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(263, 11);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(33, 23);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // dgAvgPerDay
            // 
            this.dgAvgPerDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAvgPerDay.Location = new System.Drawing.Point(851, 3);
            this.dgAvgPerDay.Name = "dgAvgPerDay";
            this.dgAvgPerDay.Size = new System.Drawing.Size(341, 204);
            this.dgAvgPerDay.TabIndex = 18;
            // 
            // lblCollSpot
            // 
            this.lblCollSpot.AutoSize = true;
            this.lblCollSpot.Location = new System.Drawing.Point(26, 571);
            this.lblCollSpot.Name = "lblCollSpot";
            this.lblCollSpot.Size = new System.Drawing.Size(111, 13);
            this.lblCollSpot.TabIndex = 19;
            this.lblCollSpot.Text = "[Total Collection Spot]";
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMax.ForeColor = System.Drawing.Color.Coral;
            this.lblMax.Location = new System.Drawing.Point(746, 207);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(66, 24);
            this.lblMax.TabIndex = 20;
            this.lblMax.Text = "label4";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(18, 216);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(145, 20);
            this.txtSearch.TabIndex = 21;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // cmdFilter
            // 
            this.cmdFilter.DisplayMember = "Value";
            this.cmdFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdFilter.FormattingEnabled = true;
            this.cmdFilter.Location = new System.Drawing.Point(190, 218);
            this.cmdFilter.Name = "cmdFilter";
            this.cmdFilter.Size = new System.Drawing.Size(121, 21);
            this.cmdFilter.TabIndex = 22;
            this.cmdFilter.ValueMember = "Key";
            this.cmdFilter.SelectedIndexChanged += new System.EventHandler(this.cmdFilter_SelectedIndexChanged);
            // 
            // cmbAmountFilter
            // 
            this.cmbAmountFilter.DisplayMember = "Value";
            this.cmbAmountFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAmountFilter.FormattingEnabled = true;
            this.cmbAmountFilter.Location = new System.Drawing.Point(341, 216);
            this.cmbAmountFilter.Name = "cmbAmountFilter";
            this.cmbAmountFilter.Size = new System.Drawing.Size(102, 21);
            this.cmbAmountFilter.TabIndex = 23;
            this.cmbAmountFilter.ValueMember = "Key";
            // 
            // cmbFilter
            // 
            this.cmbFilter.DisplayMember = "Value";
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(629, 186);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(102, 21);
            this.cmbFilter.TabIndex = 24;
            this.cmbFilter.ValueMember = "Key";
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // frmDailyEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.cmbAmountFilter);
            this.Controls.Add(this.cmdFilter);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblCollSpot);
            this.Controls.Add(this.dgAvgPerDay);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvAllDailyCollection);
            this.Controls.Add(this.chkNotGivenCustomer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmDailyEntry";
            this.Size = new System.Drawing.Size(1281, 615);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllDailyCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgAvgPerDay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkNotGivenCustomer;
        private System.Windows.Forms.DataGridView dgvAllDailyCollection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridView dgAvgPerDay;
        private System.Windows.Forms.Label lblCollSpot;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmdFilter;
        private System.Windows.Forms.ComboBox cmbAmountFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
    }
}