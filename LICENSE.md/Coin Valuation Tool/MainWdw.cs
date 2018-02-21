using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SQL;
using System.Windows.Forms;
using System.Collections;

namespace Coin_Valuation_Tool
{
    public partial class MainWdw : Form
    {
        private string ServerAddress = Properties.Settings.Default["ServerAddress"].ToString();
        private string Database = Properties.Settings.Default["Database"].ToString();
        private Hashtable DBTables = new Hashtable(20, 0.5f);
        private SQLFunc mySQL;
        private bool LoginFailed = false;
        private DateTime today;
        private CoinParam CoinParams = new CoinParam();
        private int Row;

        public MainWdw()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            InitializeComponent();

            /*Thread myThread = Thread.CurrentThread;

            LoginWdw login = new LoginWdw(ServerAddress, Database);
            login.ShowDialog();

            if(login == null || login.Result != myThread.ManagedThreadId) { LoginFailed = true; }*/

            mySQL = new SQLFunc(ServerAddress, Database);
            mySQL.Err += MySQL_Err;
            mySQL.Connect();
            
            SetTables();
            SetCoinParams();

            UpdateProjectInScope();
        }

        private void MySQL_Err(object sender, IErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void MainWdw_Layout(object sender, LayoutEventArgs e)
        {
            if(LoginFailed) { this.Close(); }
        }

        private void thirdPartiesMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDBParam thirdParty = new ChangeDBParam(DBTables["ThirdParties"].ToString(), mySQL, ChangeDBParam.ParameterToChange.ThirdParty);
            thirdParty.ShowDialog();

            if (thirdParty != null) thirdParty.Close();
        }

        private void mixDebtEquityMenuItem_Click(object sender, EventArgs e)
        {
            NewMDEProject newProject = new NewMDEProject(DBTables, mySQL, today, CoinParams);
            newProject.ShowDialog();

            if (newProject != null) newProject.Close();
        }

        private void fXRiskMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<SQLFunc.SubResults> myData = mySQL.GetData(DBTables["FXRisk"].ToString(), "*");
                List<SQLFunc.SubResults> Headers = mySQL.Get_TblFields(DBTables["FXRisk"].ToString());

                if(myData.Count == 0)
                {
                    MessageBox.Show("No data for this table");
                    return;
                }

                Parameters myParam = new Parameters(Headers, myData, false, "Period for deviation are: [0, 6M], [6M, 1Y], [1Y, 2Y], ...");
                myParam.ShowDialog();
                if (myParam != null && myParam.IsSaved)
                {
                    mySQL.ClearTable(DBTables["FXRisk"].ToString());
                    for(int i = 0; i < myParam.Data2Write.Count; i++)
                    {
                        mySQL.InsertData(DBTables["FXRisk"].ToString(), myParam.Data2Write[i]);
                    }
                    myParam.Close();
                }
            }
            catch
            { MessageBox.Show("Issue while getting data for FX Risk management"); }
        }

        private void degradationMenuItem_Click(object sender, EventArgs e)
        {
            RiskMgt degradation = new RiskMgt(mySQL, DBTables,RiskMgt.ModelCategory.Degradation);
            degradation.ShowDialog();

            if(degradation != null) degradation.Close();
        }

        private void climateImpactMenuItem_Click(object sender, EventArgs e)
        {
            RiskMgt degradation = new RiskMgt(mySQL, DBTables, RiskMgt.ModelCategory.Climate);
            degradation.ShowDialog();

            if (degradation != null) degradation.Close();
        }

        private void openProjectMenuItem_Click(object sender, EventArgs e)
        {
            List<SQLFunc.SubResults> sqlData = mySQL.GetData(DBTables["Projects"].ToString(), "ProjectName,Country,Sector,SubSector,pCur,ProjectID");

            OpenProjects ProjectOpen = new OpenProjects(sqlData);
            ProjectOpen.ShowDialog();

            if (ProjectOpen == null || ProjectOpen.Data2LookFor == null) return;

            SQLFunc.SubResults sql1Data = mySQL.Get1Data(DBTables["Projects"].ToString(), "*", ProjectOpen.Data2LookFor);
            ProjectOpen.Close();

            switch (ProjectOpen.Action)
            {
                case OpenProjects.ActionType.OpenProject:
                    NewMDEProject OpenProject = new NewMDEProject(DBTables, mySQL, today, sql1Data, CoinParams);
                    OpenProject.ShowDialog();
                    break;
                case OpenProjects.ActionType.ValidateProject:
                    SQLFunc.SubResults checkData = mySQL.Get1Data(DBTables["ProjectInScope"].ToString(), "ProjectID", "ProjectID='" + sql1Data._SubResults[25].ToString() + "'");
                    bool Exists = false;
                    if (checkData._SubResults.Count != 0) Exists = true;

                    ProjectValidation ValidateProject = new ProjectValidation(DBTables, mySQL, sql1Data, today, Exists);
                    ValidateProject.ShowDialog();

                    if (ValidateProject == null) return;

                    if(ValidateProject.IsOpen) { OpenProject = new NewMDEProject(DBTables, mySQL, today, sql1Data, CoinParams); OpenProject.ShowDialog(); }

                    if (ValidateProject.IsValidated)
                    {
                        string data2save = "'" + ValidateProject.Project_ID.ToString() + "','Validated','" + FormatDate(ValidateProject.StartDate) + "','" + FormatDate(ValidateProject.COD) + "','" + FormatDate(ValidateProject.EndDate) + "','','','','','','','','','','','','',''";
                        mySQL.InsertData(DBTables["ProjectInScope"].ToString(), data2save);

                        UpdateProjectInScope();
                    }

                    ValidateProject.Close();
                    break;
            }

            
        }

        private void UpdateProjectInScope()
        {
            projectInScopeGrid.Rows.Clear();

            List<SQLFunc.SubResults> data = mySQL.GetData(DBTables["ProjectInScope"].ToString(), "ProjectID", "Status='Validated'");

            for (int i = 0; i < data.Count; i++)
            {
                SQLFunc.SubResults sqlData = mySQL.Get1Data(DBTables["Projects"].ToString(), "ProjectName,Country,Sector,SubSector,iCur", "ProjectID='" + data[i]._SubResults[0].ToString() + "'");
                string[] rowData = new string[6];

                rowData[0] = data[i]._SubResults[0].ToString();

                for (int j = 0; j < rowData.Length - 1; j++)
                {
                    rowData[j + 1] = sqlData._SubResults[j].ToString();
                }

                projectInScopeGrid.Rows.Add(rowData);
            }
        }

        private string FormatDate(DateTime Date)
        {
            return Date.Year + "-" + Date.Month + "-" + Date.Day;
        }

        private void auditorFeeMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDBParam thirdParty = new ChangeDBParam(DBTables["AuditorFee"].ToString(), mySQL, ChangeDBParam.ParameterToChange.ThirdParty);
            thirdParty.ShowDialog();

            if (thirdParty != null) thirdParty.Close();
        }

        private void depositRatesMenuItem_Click(object sender, EventArgs e)
        {
            RateManagement DepositRates = new RateManagement(mySQL, DBTables, today);
            DepositRates.ShowDialog();

            if (DepositRates != null) DepositRates.Close();
        }

        private void coinSetupMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDBParam CoinSetup = new ChangeDBParam(DBTables["CoinParams"].ToString(), mySQL, ChangeDBParam.ParameterToChange.CoinParams);
            CoinSetup.ShowDialog();

            if (CoinSetup != null) CoinSetup.Close();
        }

        public struct CoinParam
        {
            public int DivPaymentFrequency;
            public DateTime FirstPayment;
            public int MaxDepositTenor;
        }

        private void depositRateDecreaseMenuItem_Click(object sender, EventArgs e)
        {
            DepositRateDecrease depositRate = new DepositRateDecrease(mySQL, DBTables);
            depositRate.ShowDialog();

            if (depositRate != null) depositRate.Close();
        }

        private void riskTestsMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDBParam changeRiskTest = new ChangeDBParam(DBTables["RiskTests"].ToString(), mySQL, ChangeDBParam.ParameterToChange.RiskTests, DBTables["Countries"].ToString(), DBTables["ProjectType"].ToString(), DBTables["ProjectSubType"].ToString());

            changeRiskTest.ShowDialog();

            if(changeRiskTest != null) { changeRiskTest.Close(); }
        }

        private void ProjectInScopeGrid_MouseClick1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Row = this.projectInScopeGrid.HitTest(e.X, e.Y).RowIndex;
                contextMenuProjectInScope.Show(new Point(e.X + projectInScopeGrid.Location.X + +this.Location.X + 10, e.Y + projectInScopeGrid.Location.Y + this.Location.Y + 20));
            }
        }

        private void GroupSeedsGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Row = this.groupSeedsGrid.HitTest(e.X, e.Y).RowIndex;
                groupSeedsMenuStrip.Show(new Point(e.X + groupSeedsGrid.Location.X + this.Location.X + 10, e.Y + groupSeedsGrid.Location.Y + this.Location.Y + 20));
            }
        }

        private void projectInScopeGrid_MouseEnter(object sender, EventArgs e)
        {
            projectInScopeGrid.MouseClick += ProjectInScopeGrid_MouseClick1;
        }

        private void projectInScopeGrid_MouseLeave(object sender, EventArgs e)
        {
            projectInScopeGrid.MouseClick -= null;
        }

        private void groupSeedsGrid_MouseEnter(object sender, EventArgs e)
        {
            groupSeedsGrid.MouseClick += GroupSeedsGrid_MouseClick;
        }

        private void groupSeedsGrid_MouseLeave(object sender, EventArgs e)
        {
            groupSeedsGrid.MouseClick -= null;
        }
    }
}
