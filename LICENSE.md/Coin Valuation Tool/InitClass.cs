using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL;

namespace Coin_Valuation_Tool
{
    public partial class MainWdw
    {
        private void SetTables()
        {
            try
            {
                DBTables["ThirdParties"] = Properties.Settings.Default["ThirdPartyTbl"].ToString();
                DBTables["pCur"] = Properties.Settings.Default["pCurTbl"].ToString();
                DBTables["iCur"] = Properties.Settings.Default["iCurTbl"].ToString();
                DBTables["DegradationModels"] = Properties.Settings.Default["DegradModelsTbl"].ToString();
                DBTables["ClimateModels"] = Properties.Settings.Default["ClimateModelsTbl"].ToString();
                DBTables["ClimateRisk"] = Properties.Settings.Default["ClimateRiskTbl"].ToString();
                DBTables["FX"] = Properties.Settings.Default["FXTbl"].ToString();
                DBTables["FXModels"] = Properties.Settings.Default["FXModelsTbl"].ToString();
                DBTables["FXRisk"] = Properties.Settings.Default["FXRiskTbl"].ToString();
                DBTables["ProjectType"] = Properties.Settings.Default["ProjectTypeTbl"].ToString();
                DBTables["ProjectSubType"] = Properties.Settings.Default["ProjectSubTypeTbl"].ToString();
                DBTables["DegradationRisk"] = Properties.Settings.Default["DegradationRiskTbl"].ToString();
                DBTables["Countries"] = Properties.Settings.Default["CountriesTbl"].ToString();
                DBTables["FinancingParam"] = Properties.Settings.Default["FinancingParamTbl"].ToString();
                DBTables["Projects"] = Properties.Settings.Default["ProjectsTbl"].ToString();
                DBTables["AuditorFee"] = Properties.Settings.Default["AuditorFeeTbl"].ToString();
                DBTables["DepositRates"] = Properties.Settings.Default["DepositRatesTbl"].ToString();
                DBTables["CoinParams"] = Properties.Settings.Default["CoinParamsTbl"].ToString();
                DBTables["DepositRateDecrease"] = Properties.Settings.Default["DepositRateDecreaseTbl"].ToString();
                DBTables["RiskTests"] = Properties.Settings.Default["RiskTestsTbl"].ToString();
                DBTables["ProjectInScope"] = Properties.Settings.Default["ProjectInScopeTbl"].ToString();

                bool IsLive = Convert.ToBoolean(Properties.Settings.Default["IsLive"]);
                if (IsLive) today = DateTime.Today;
                else today = new DateTime(2010, 10, 18);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void SetCoinParams()
        {
            List<SQLFunc.SubResults> data = mySQL.GetData(DBTables["CoinParams"].ToString(), "*");
            CoinParams.DivPaymentFrequency = Convert.ToInt32(data[0]._SubResults[0]);
            CoinParams.FirstPayment = Convert.ToDateTime(data[0]._SubResults[1]);
            CoinParams.MaxDepositTenor = Convert.ToInt32(data[0]._SubResults[2]);
        }

    }
}
