namespace Coin_Valuation_Tool
{
    partial class CalculateRateAfterCP
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.expectedFirstDivBox = new System.Windows.Forms.TextBox();
            this.rateDuringCPBox = new System.Windows.Forms.TextBox();
            this.conversionBox = new System.Windows.Forms.TextBox();
            this.equityPayingInterestBox = new System.Windows.Forms.TextBox();
            this.computeBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.rateAfterCPLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rate During CP (%):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Conversion (%):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Equity Paying Interest  (%):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Expected First ROE (%):";
            // 
            // expectedFirstDivBox
            // 
            this.expectedFirstDivBox.Location = new System.Drawing.Point(157, 94);
            this.expectedFirstDivBox.MaxLength = 5;
            this.expectedFirstDivBox.Name = "expectedFirstDivBox";
            this.expectedFirstDivBox.Size = new System.Drawing.Size(45, 20);
            this.expectedFirstDivBox.TabIndex = 4;
            // 
            // rateDuringCPBox
            // 
            this.rateDuringCPBox.Location = new System.Drawing.Point(157, 16);
            this.rateDuringCPBox.MaxLength = 5;
            this.rateDuringCPBox.Name = "rateDuringCPBox";
            this.rateDuringCPBox.Size = new System.Drawing.Size(45, 20);
            this.rateDuringCPBox.TabIndex = 1;
            // 
            // conversionBox
            // 
            this.conversionBox.Location = new System.Drawing.Point(157, 42);
            this.conversionBox.MaxLength = 5;
            this.conversionBox.Name = "conversionBox";
            this.conversionBox.Size = new System.Drawing.Size(45, 20);
            this.conversionBox.TabIndex = 2;
            // 
            // equityPayingInterestBox
            // 
            this.equityPayingInterestBox.Location = new System.Drawing.Point(157, 68);
            this.equityPayingInterestBox.MaxLength = 5;
            this.equityPayingInterestBox.Name = "equityPayingInterestBox";
            this.equityPayingInterestBox.Size = new System.Drawing.Size(45, 20);
            this.equityPayingInterestBox.TabIndex = 3;
            // 
            // computeBtn
            // 
            this.computeBtn.Location = new System.Drawing.Point(127, 120);
            this.computeBtn.Name = "computeBtn";
            this.computeBtn.Size = new System.Drawing.Size(75, 23);
            this.computeBtn.TabIndex = 5;
            this.computeBtn.Text = "Get Rate";
            this.computeBtn.UseVisualStyleBackColor = true;
            this.computeBtn.Click += new System.EventHandler(this.computeBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Rate After CP should be:";
            // 
            // rateAfterCPLbl
            // 
            this.rateAfterCPLbl.AutoSize = true;
            this.rateAfterCPLbl.Location = new System.Drawing.Point(142, 160);
            this.rateAfterCPLbl.Name = "rateAfterCPLbl";
            this.rateAfterCPLbl.Size = new System.Drawing.Size(10, 13);
            this.rateAfterCPLbl.TabIndex = 6;
            this.rateAfterCPLbl.Text = "-";
            // 
            // CalculateRateAfterCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 187);
            this.Controls.Add(this.rateAfterCPLbl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.computeBtn);
            this.Controls.Add(this.equityPayingInterestBox);
            this.Controls.Add(this.conversionBox);
            this.Controls.Add(this.rateDuringCPBox);
            this.Controls.Add(this.expectedFirstDivBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(235, 225);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(235, 225);
            this.Name = "CalculateRateAfterCP";
            this.Text = "Calculate Rate after CP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox expectedFirstDivBox;
        private System.Windows.Forms.TextBox rateDuringCPBox;
        private System.Windows.Forms.TextBox conversionBox;
        private System.Windows.Forms.TextBox equityPayingInterestBox;
        private System.Windows.Forms.Button computeBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label rateAfterCPLbl;
    }
}