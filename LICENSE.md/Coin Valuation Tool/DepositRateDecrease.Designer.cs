namespace Coin_Valuation_Tool
{
    partial class DepositRateDecrease
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
            this.currencyBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.decreaseBox = new System.Windows.Forms.TextBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Currency:";
            // 
            // currencyBox
            // 
            this.currencyBox.FormattingEnabled = true;
            this.currencyBox.Location = new System.Drawing.Point(70, 6);
            this.currencyBox.Name = "currencyBox";
            this.currencyBox.Size = new System.Drawing.Size(54, 21);
            this.currencyBox.TabIndex = 1;
            this.currencyBox.SelectedIndexChanged += new System.EventHandler(this.currencyBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Decrease Rate (% p.a.):";
            // 
            // decreaseBox
            // 
            this.decreaseBox.Location = new System.Drawing.Point(138, 33);
            this.decreaseBox.MaxLength = 5;
            this.decreaseBox.Name = "decreaseBox";
            this.decreaseBox.Size = new System.Drawing.Size(38, 20);
            this.decreaseBox.TabIndex = 2;
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(15, 68);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(75, 23);
            this.updateBtn.TabIndex = 3;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // DepositRateDecrease
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 102);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.decreaseBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.currencyBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(235, 140);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(235, 140);
            this.Name = "DepositRateDecrease";
            this.Text = "Deposit Rate Decrease - 1Y";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox currencyBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox decreaseBox;
        private System.Windows.Forms.Button updateBtn;
    }
}