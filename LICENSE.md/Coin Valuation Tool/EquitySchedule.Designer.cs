namespace Coin_Valuation_Tool
{
    partial class EquitySchedule
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
            this.equityScheduleGrid = new System.Windows.Forms.DataGridView();
            this.EquityAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.equityScheduleGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // equityScheduleGrid
            // 
            this.equityScheduleGrid.AllowUserToAddRows = false;
            this.equityScheduleGrid.AllowUserToDeleteRows = false;
            this.equityScheduleGrid.AllowUserToResizeRows = false;
            this.equityScheduleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.equityScheduleGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EquityAmount});
            this.equityScheduleGrid.Location = new System.Drawing.Point(12, 12);
            this.equityScheduleGrid.Name = "equityScheduleGrid";
            this.equityScheduleGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.equityScheduleGrid.Size = new System.Drawing.Size(260, 207);
            this.equityScheduleGrid.TabIndex = 0;
            // 
            // EquityAmount
            // 
            this.EquityAmount.HeaderText = "Equity Amount";
            this.EquityAmount.MaxInputLength = 12;
            this.EquityAmount.Name = "EquityAmount";
            this.EquityAmount.Width = 120;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(12, 227);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // EquitySchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.equityScheduleGrid);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "EquitySchedule";
            this.Text = "Equity Schedule";
            ((System.ComponentModel.ISupportInitialize)(this.equityScheduleGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView equityScheduleGrid;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EquityAmount;
    }
}