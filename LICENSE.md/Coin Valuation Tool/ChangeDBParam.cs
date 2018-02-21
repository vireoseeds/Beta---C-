using System;
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
    public partial class ChangeDBParam : Form
    {
        private string TPTbl;
        private List<SQLFunc.SubResults> myData;
        private SQLFunc sql;
        private bool IsCoinParams = false;
        private bool IsRiskTests = false;
        private string CountryTbl;
        private string SectorTbl;
        private string SubSectorTbl;
        private int ExistingRiskTests = 0;

        public enum ParameterToChange
        {
            Auditor,
            ThirdParty,
            CoinParams,
            RiskTests
        }

        public ChangeDBParam(string TPTbl, SQLFunc sql, ParameterToChange param, string CountryTbl = "", string SectorTbl = "", string SubSectorTbl = "")
        {
            InitializeComponent();
            
            this.TPTbl = TPTbl;
            this.sql = sql;

            this.sql.Err += Sql_Err;

            switch(param)
            {
                case ParameterToChange.ThirdParty:
                    this.Text = "Change Third Party data";
                    break;
                case ParameterToChange.Auditor:
                    this.Text = "Change Auditor data";
                    break;
                case ParameterToChange.CoinParams:
                    this.Text = "Change Coin Params";
                    dataGrid.AllowUserToAddRows = false;
                    dataGrid.AllowUserToDeleteRows = false;
                    IsCoinParams = true;
                    break;
                case ParameterToChange.RiskTests:
                    this.Text = "Change Risk Tests";
                    dataGrid.CellValueChanged += DataGrid_CellValueChanged;
                    dataGrid.RowsAdded += DataGrid_RowsAdded;
                    IsRiskTests = true;
                    break;
            }

            this.CountryTbl = CountryTbl;
            this.SectorTbl = SectorTbl;
            this.SubSectorTbl = SubSectorTbl;

            SetGrid();
            GetTblData();
        }

        private void DataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex == 0) return;

            DataGridViewComboBoxCell cellBox = dataGrid.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell;

            List<SQLFunc.SubResults> data = sql.GetData(SectorTbl, "*");

            for (int j = 0; j < data.Count; j++)
            {
                cellBox.Items.Add(data[j]._SubResults[0].ToString());
            }

        }

        private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                DataGridViewComboBoxCell cellBox = dataGrid.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell;
                cellBox.Items.Clear();

                List<SQLFunc.SubResults> data = sql.GetData(SubSectorTbl, "*", "ProjectType='" + dataGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");

                for(int i = 0; i < data.Count; i++)
                {
                    cellBox.Items.Add(data[i]._SubResults[1].ToString());
                }
            }
        }

        private void Sql_Err(object sender, IErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void SetGrid()
        {
            myData = sql.Get_TblFields(TPTbl);

            DataGridViewCell cell = new DataGridViewTextBoxCell();

            for (int i = 0; i < myData.Count; i++)
            {
                DataGridViewColumn myCol;
                DataGridViewComboBoxCell cellBox = new DataGridViewComboBoxCell();

                if (IsRiskTests && i < 3)
                {
                    List<SQLFunc.SubResults> data;

                    if (i == 0)
                    {
                        data = sql.GetData(CountryTbl, "*");
                    }
                    else if(i == 1)
                    {
                        data = sql.GetData(SectorTbl, "*");
                    }
                    else
                    {
                        data = new List<SQLFunc.SubResults>();
                    }

                    for (int j = 0; j < data.Count; j++)
                    {
                        cellBox.Items.Add(data[j]._SubResults[0].ToString());
                    }

                    myCol = new DataGridViewColumn(cellBox);
                }
                else
                {
                    myCol = new DataGridViewColumn(cell);
                }

                myCol.Name = (string)myData[i]._SubResults[3];
                myCol.HeaderText = myCol.Name;

                dataGrid.Columns.Add(myCol);
            }
        }

        private void GetTblData()
        {
            myData = sql.GetData(TPTbl, "*");
            ExistingRiskTests = myData.Count;

            for (int i = 0; i < myData.Count; i++)
            {
                string[] data = new string[myData[i]._SubResults.Count];
                string tmpStr = "";

                for (int j = 0; j < data.Length; j++)
                {
                    if (IsRiskTests)
                    {
                        if (j <= 1) data[j] = myData[i]._SubResults[j].ToString();
                        if (j == 2) tmpStr = myData[i]._SubResults[j].ToString();
                        if (j > 2) data[j] = (GlobalFunc.ToDouble(myData[i]._SubResults[j].ToString()) * 100).ToString();
                    }
                    else
                    {
                        data[j] = myData[i]._SubResults[j].ToString();
                        if (IsCoinParams && j == 1) data[j] = ToStrDate(data[j]);
                    }

                }

                dataGrid.Rows.Add(data);

                if (IsRiskTests)
                {
                    List<SQLFunc.SubResults> DBdata = sql.GetData(SubSectorTbl, "ProjectSubType", "ProjectType='" + data[1] + "'");
                    DataGridViewComboBoxCell cellBox = new DataGridViewComboBoxCell();

                    for (int k = 0; k < DBdata.Count; k++)
                    {
                        cellBox.Items.Add(DBdata[k]._SubResults[0].ToString());
                    }

                    dataGrid.Rows[i].Cells[2] = cellBox;
                    dataGrid.Rows[i].Cells[2].Value = myData[i]._SubResults[2].ToString();
                }
            }
        }

        private string ToStrDate(string DBDateFormat)
        {
            string Month = DBDateFormat.Substring(0, DBDateFormat.IndexOf("/"));
            if (Month.Length == 1) Month = "0" + Month;

            DBDateFormat = DBDateFormat.Substring(DBDateFormat.IndexOf("/") + 1, DBDateFormat.Length - (DBDateFormat.IndexOf("/") + 1));
            string Day = DBDateFormat.Substring(0, DBDateFormat.IndexOf("/"));
            if (Day.Length == 1) Day = "0" + Day;

            DBDateFormat = DBDateFormat.Substring(DBDateFormat.IndexOf("/") + 1, DBDateFormat.Length - (DBDateFormat.IndexOf("/") + 1));
            string Year = DBDateFormat.Substring(0, 4);

            return Year + "-" + Month + "-" + Day;
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if(IsCoinParams)
            {
                int Res;
                int Div = Math.DivRem(Convert.ToInt32(dataGrid.Rows[0].Cells[0].Value), 3, out Res);

                if(Res != 0 || Div == 0) { MessageBox.Show("Dividend payment frequency has to be eithr 3, 6 or 12 months"); return; }

                string data = "";

                data += "PaymentFrequency='" + dataGrid.Rows[0].Cells[0].Value.ToString() + "'";
                data += ",FirstPaymentDate='" + dataGrid.Rows[0].Cells[1].Value.ToString() + "'";

                sql.UpdateTable(TPTbl, data);
            }
            else if(IsRiskTests)
            {
                for(int i = 0; i < dataGrid.Rows.Count - 1; i++)
                {
                    string Country = dataGrid.Rows[i].Cells[0].Value.ToString();
                    string Sector = dataGrid.Rows[i].Cells[1].Value.ToString();
                    string SubSector = dataGrid.Rows[i].Cells[2].Value.ToString();

                    if(Country.Equals("")) { MessageBox.Show("Country field is empty line " + i.ToString()); return; }
                    if (Sector.Equals("")) { MessageBox.Show("Sector field is empty line " + i.ToString()); return; }
                    if (SubSector.Equals("")) { MessageBox.Show("SubSector field is empty line " + i.ToString()); return; }

                    SQLFunc.SubResults sqlData = sql.Get1Data(TPTbl, "Country", "Country='" + Country + "' AND Sector='" + Sector + "' AND Subsector='" + SubSector + "'");

                    if (sqlData._SubResults.Count == 0)
                    {
                        string data = "";
                        data += "'" + Country + "','" + Sector + "','" + SubSector + "'";

                        for(int k = 3; k < dataGrid.Columns.Count; k++)
                        {
                            data += ",'" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[k].Value.ToString()) / 100).ToString() + "'";
                        }

                        sql.InsertData(TPTbl, data);
                    }
                    else
                    {
                        if(i > ExistingRiskTests - 1 && ExistingRiskTests != 0) { MessageBox.Show("The scheme line " + i.ToString() + " already exists"); return; }

                        string data = "";

                        data += "EnergyProductionDecrease='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[3].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseFXRisk='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[4].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseDegradationRisk='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[5].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseClimateRisk='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[6].Value.ToString()) / 100).ToString() + "'";
                        data += ",DecreaseDepositRates='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[7].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseOM='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[8].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseSGA='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[9].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseRoyalties='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[10].Value.ToString()) / 100).ToString() + "'";
                        data += ",IncreaseTaxes='" + (GlobalFunc.ToDouble(dataGrid.Rows[i].Cells[11].Value.ToString()) / 100).ToString() + "'";

                        sql.UpdateTable(TPTbl, data, "Country='" + Country + "' AND Sector='" + Sector + "' AND Subsector='" + SubSector + "'");
                    }
                }
            }
            else
            {
                sql.ClearTable(TPTbl);

                for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                {
                    string data = "";

                    for (int j = 0; j < dataGrid.Columns.Count - 1; j++)
                    {
                        data += "'" + dataGrid.Rows[i].Cells[j].Value.ToString() + "',";
                    }
                    data += "'" + dataGrid.Rows[i].Cells[dataGrid.Columns.Count - 1].Value.ToString() + "'";

                    sql.InsertData(TPTbl, data);
                }
            }
        }
    }
}
