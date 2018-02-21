using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL;

namespace Coin_Valuation_Tool
{
    public partial class DepositRateDecrease : Form
    {
        private SQLFunc sql;
        private string DecreaseRateTable;
        private bool InfoExists = false;

        public DepositRateDecrease(SQLFunc sql, Hashtable TblData)
        {
            InitializeComponent();

            List<SQLFunc.SubResults> data = sql.GetData(TblData["pCur"].ToString(), "*");

            for(int i = 0; i < data.Count; i++)
            {
                currencyBox.Items.Add(data[i]._SubResults[0].ToString());
            }

            this.sql = sql;
            this.DecreaseRateTable = TblData["DepositRateDecrease"].ToString();
        }

        private void currencyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            decreaseBox.Text = "";

            SQLFunc.SubResults data = sql.Get1Data(DecreaseRateTable, "Value", "pCur='" + currencyBox.Text + "'");

            if (data._SubResults.Count == 0) { decreaseBox.Text = "0"; InfoExists = false; }
            else
            {
                decreaseBox.Text = (Convert.ToDouble(data._SubResults[0]) * 100).ToString();
                InfoExists = true;
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (currencyBox.Text.Equals("")) return;
            double value;

            try
            {
                value = Convert.ToDouble(decreaseBox.Text) / 100;
            }
            catch
            {
                value = 0.0;
            }

            if (InfoExists) sql.UpdateTable(DecreaseRateTable, "Value='" + value + "'", "pCur='" + currencyBox.Text + "'");
            else sql.InsertData(DecreaseRateTable, "'" + currencyBox.Text + "','" + value + "'");
        }
    }
}
