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
    public partial class CalculateRateAfterCP : Form
    {
        public CalculateRateAfterCP()
        {
            InitializeComponent();
        }

        private void computeBtn_Click(object sender, EventArgs e)
        {
            double RateDuringCP = GlobalFunc.ToDouble(rateDuringCPBox.Text) / 100;
            double Conversion = GlobalFunc.ToDouble(conversionBox.Text) / 100;
            double EquityPayingInterest = GlobalFunc.ToDouble(equityPayingInterestBox.Text) / 100;
            double ExpectedFirstDiv = GlobalFunc.ToDouble(expectedFirstDivBox.Text) / 100;

            double tmp1 = Conversion / (1 - Conversion) * EquityPayingInterest * RateDuringCP;
            double tmp2 = Conversion / (1 - Conversion) * ExpectedFirstDiv;

            double result = Math.Round(RateDuringCP + tmp1 - tmp2, 4) * 100;

            rateAfterCPLbl.Text = result.ToString() + "%";
        }
    }
}
