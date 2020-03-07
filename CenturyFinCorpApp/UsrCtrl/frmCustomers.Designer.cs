namespace CenturyFinCorpApp
{
    partial class frmCustomers
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
            this.rdbActive = new System.Windows.Forms.RadioButton();
            this.rdbClosed = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFilters = new System.Windows.Forms.ComboBox();
            this.chkAllColumns = new System.Windows.Forms.CheckBox();
            this.lblRowCount = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmbReturnTypes = new System.Windows.Forms.ComboBox();
            this.chkFriends = new System.Windows.Forms.CheckBox();
            this.btnLatestCollection = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnClosedTxn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 114);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1237, 527);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            //this.dataGridView1.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellLeave);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyUp);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // rdbActive
            // 
            this.rdbActive.AutoSize = true;
            this.rdbActive.Location = new System.Drawing.Point(8, 14);
            this.rdbActive.Name = "rdbActive";
            this.rdbActive.Size = new System.Drawing.Size(116, 17);
            this.rdbActive.TabIndex = 1;
            this.rdbActive.Tag = "RN";
            this.rdbActive.Text = "RUNNING NOTES";
            this.rdbActive.UseVisualStyleBackColor = true;
            this.rdbActive.CheckedChanged += new System.EventHandler(this.rdbActive_CheckedChanged);
            // 
            // rdbClosed
            // 
            this.rdbClosed.AutoSize = true;
            this.rdbClosed.Location = new System.Drawing.Point(146, 13);
            this.rdbClosed.Name = "rdbClosed";
            this.rdbClosed.Size = new System.Drawing.Size(88, 17);
            this.rdbClosed.TabIndex = 2;
            this.rdbClosed.Tag = "CN";
            this.rdbClosed.Text = "Closed Notes";
            this.rdbClosed.UseVisualStyleBackColor = true;
            this.rdbClosed.CheckedChanged += new System.EventHandler(this.rdbClosed_CheckedChanged);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.Location = new System.Drawing.Point(282, 14);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(67, 17);
            this.rdbAll.TabIndex = 3;
            this.rdbAll.Tag = "AN";
            this.rdbAll.Text = "All Notes";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbAll_CheckedChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(457, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(190, 26);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(677, 13);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 5;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(677, 39);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(879, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            // 
            // cmbFilters
            // 
            this.cmbFilters.DisplayMember = "value";
            this.cmbFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilters.FormattingEnabled = true;
            this.cmbFilters.Location = new System.Drawing.Point(24, 77);
            this.cmbFilters.Name = "cmbFilters";
            this.cmbFilters.Size = new System.Drawing.Size(176, 21);
            this.cmbFilters.TabIndex = 11;
            this.cmbFilters.ValueMember = "key";
            this.cmbFilters.SelectedIndexChanged += new System.EventHandler(this.cmbFilters_SelectedIndexChanged);
            // 
            // chkAllColumns
            // 
            this.chkAllColumns.AutoSize = true;
            this.chkAllColumns.Checked = true;
            this.chkAllColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllColumns.Location = new System.Drawing.Point(445, 77);
            this.chkAllColumns.Name = "chkAllColumns";
            this.chkAllColumns.Size = new System.Drawing.Size(110, 17);
            this.chkAllColumns.TabIndex = 12;
            this.chkAllColumns.Text = "Show All Columns";
            this.chkAllColumns.UseVisualStyleBackColor = true;
            this.chkAllColumns.CheckedChanged += new System.EventHandler(this.chkAllColumns_CheckedChanged);
            // 
            // lblRowCount
            // 
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Location = new System.Drawing.Point(541, 98);
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(67, 13);
            this.lblRowCount.TabIndex = 13;
            this.lblRowCount.Text = "lblRowCount";
            // 
            // cmbReturnTypes
            // 
            this.cmbReturnTypes.DisplayMember = "value";
            this.cmbReturnTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReturnTypes.FormattingEnabled = true;
            this.cmbReturnTypes.Location = new System.Drawing.Point(239, 77);
            this.cmbReturnTypes.Name = "cmbReturnTypes";
            this.cmbReturnTypes.Size = new System.Drawing.Size(176, 21);
            this.cmbReturnTypes.TabIndex = 14;
            this.cmbReturnTypes.ValueMember = "key";
            this.cmbReturnTypes.SelectedIndexChanged += new System.EventHandler(this.cmbReturnTypes_SelectedIndexChanged);
            // 
            // chkFriends
            // 
            this.chkFriends.AutoSize = true;
            this.chkFriends.Location = new System.Drawing.Point(578, 77);
            this.chkFriends.Name = "chkFriends";
            this.chkFriends.Size = new System.Drawing.Size(83, 17);
            this.chkFriends.TabIndex = 15;
            this.chkFriends.Text = "Friends Also";
            this.chkFriends.UseVisualStyleBackColor = true;
            this.chkFriends.CheckedChanged += new System.EventHandler(this.chkFriends_CheckedChanged);
            // 
            // btnLatestCollection
            // 
            this.btnLatestCollection.Location = new System.Drawing.Point(781, 39);
            this.btnLatestCollection.Name = "btnLatestCollection";
            this.btnLatestCollection.Size = new System.Drawing.Size(96, 23);
            this.btnLatestCollection.TabIndex = 16;
            this.btnLatestCollection.Text = "Latest Cxn?";
            this.btnLatestCollection.UseVisualStyleBackColor = true;
            this.btnLatestCollection.Click += new System.EventHandler(this.btnLatestCollection_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1124, 176);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(128, 217);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = "Daily = 0\r\nAlternate = 1\r\n";
            // 
            // btnClosedTxn
            // 
            this.btnClosedTxn.BackColor = System.Drawing.Color.Aqua;
            this.btnClosedTxn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosedTxn.Location = new System.Drawing.Point(1124, 79);
            this.btnClosedTxn.Name = "btnClosedTxn";
            this.btnClosedTxn.Size = new System.Drawing.Size(144, 32);
            this.btnClosedTxn.TabIndex = 18;
            this.btnClosedTxn.Text = "Run Closed Txn";
            this.btnClosedTxn.UseVisualStyleBackColor = false;
            this.btnClosedTxn.Click += new System.EventHandler(this.btnClosedTxn_Click);
            // 
            // frmCustomers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClosedTxn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnLatestCollection);
            this.Controls.Add(this.chkFriends);
            this.Controls.Add(this.cmbReturnTypes);
            this.Controls.Add(this.lblRowCount);
            this.Controls.Add(this.chkAllColumns);
            this.Controls.Add(this.cmbFilters);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.rdbAll);
            this.Controls.Add(this.rdbClosed);
            this.Controls.Add(this.rdbActive);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmCustomers";
            this.Size = new System.Drawing.Size(1268, 641);
            this.Load += new System.EventHandler(this.frmCustomers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton rdbActive;
        private System.Windows.Forms.RadioButton rdbClosed;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFilters;
        private System.Windows.Forms.CheckBox chkAllColumns;
        private System.Windows.Forms.Label lblRowCount;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cmbReturnTypes;
        private System.Windows.Forms.CheckBox chkFriends;
        private System.Windows.Forms.Button btnLatestCollection;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnClosedTxn;
    }
}