namespace CenturyFinCorpApp.UsrCtrl
{
    partial class frmPetrol
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
            this.dgvPetrol = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPetrol)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPetrol
            // 
            this.dgvPetrol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPetrol.Location = new System.Drawing.Point(164, 124);
            this.dgvPetrol.Name = "dgvPetrol";
            this.dgvPetrol.Size = new System.Drawing.Size(555, 338);
            this.dgvPetrol.TabIndex = 20;
            // 
            // frmPetrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvPetrol);
            this.Name = "frmPetrol";
            this.Size = new System.Drawing.Size(958, 654);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPetrol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPetrol;
    }
}
