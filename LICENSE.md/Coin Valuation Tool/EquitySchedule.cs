using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coin_Valuation_Tool
{
    public partial class EquitySchedule : Form
    {
        public double TotalEquity;
        public double[] Schedule;

        public EquitySchedule(int CP, double EquityValue)
        {
            InitializeComponent();

            for (int i = 0; i < CP; i++)
            {
                equityScheduleGrid.Rows.Add();
                equityScheduleGrid.Rows[i].HeaderCell.Value = "Month " + (i + 1).ToString();
            }

            equityScheduleGrid.Rows[0].Cells[0].Value = EquityValue.ToString();
            
            Schedule = new double[CP];
            Schedule[0] = EquityValue;
        }

        public EquitySchedule(double[] EquityValue)
        {
            InitializeComponent();

            for (int i = 0; i < EquityValue.Length; i++)
            {
                equityScheduleGrid.Rows.Add();
                equityScheduleGrid.Rows[i].HeaderCell.Value = "Month " + (i + 1).ToString();
                equityScheduleGrid.Rows[i].Cells[0].Value = EquityValue[i].ToString();
            }

            Schedule = EquityValue;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            TotalEquity = 0;

            for (int i = 0; i< equityScheduleGrid.Rows.Count; i++)
            {
                if (equityScheduleGrid.Rows[i].Cells[0].Value == null) Schedule[i] = 0;
                else Schedule[i] = Math.Round(GlobalFunc.ToDouble(equityScheduleGrid.Rows[i].Cells[0].Value.ToString()), 2);
                TotalEquity += Schedule[i];
            }

            this.Hide();
        }
    }
}
