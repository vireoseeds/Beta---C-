using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using chart = System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using XML;
using SQL;
using Valuation;
using ProjectLib;


namespace Coin_Valuation_Tool
{
    public partial class NewMDEProject : Form
    {
        private SQLFunc sql;
        private Hashtable TblData;
        private delegate void updateConstCFGrid(GridType gridType);
        private delegate void updatePPAChart();
        private double AveragePrice = 0.0;
        private double StartPrice = 0.0;
        private int StartPeriod = 0;
        private DateTime StartDate;
        private string iCur;
        private bool pCurChosen = false;
        private bool CountryChosen = false;
        private double PPAcheck = 0.0;
        private int[] SetPPABtnLoc = new int[2];
        private int[] ChangeDegradBtnLoc = new int[2];
        private int[] ChangeClimBtnLoc = new int[2];
        private int[] PercentageOfEquityPaidBoxLoc = new int[2];
        private List<Project.Loan> myLoans = new List<Project.Loan>();
        private Project.Coin myCoin = new Project.Coin();
        private Project.SGA mySGA = new Project.SGA();
        private DateTime EndDate;
        private DateTime COD;
        private bool IsLoading = false;
        private bool ConsiderFinancialCosts;
        private MainWdw.CoinParam CoinParams;
        private double DepositRateDecrease = 0.0;
        private bool IsSaved = false;

        //Output Data
        public Project myData;
        private Project.EquityData EquityPO = new Project.EquityData();
        private Project.EquityData EquityOtherInvest = new Project.EquityData();
        private Hashtable Params = new Hashtable();
        private double ConcP = 0;
        private double[,] ConstCost;
        private double[] PPA_Q;
        private double[] PPA;
        private Project.ConstructionParams Const_Params = new Project.ConstructionParams();
        private double[,] PowerProductionIncrease = new double[2,1];
        private double Power = 0.0;
        private double PlantFactor = 0.0;
        private double EnergyProduction = 0.0;
        private int PORating = 9;
        private int EPCRating = 9;
        private string pCur;
        private double[] FXImpact;
        private double[] DegradationImpact;
        private object ParamDegradation;
        private double[] ClimateImpact;
        private object ParamClimateImpact;
        private double[] GlobalRisk;
        private double DivPaidDuringCP = 0.0;
        private double[,] OM = new double[3, 1];
        private double[,] Royalties = new double[3, 1];
        private double[,] Taxes = new double[3, 1];
        private Project.ProjectDetail ProjectDetails = new Project.ProjectDetail();
        private double TotalCPInterests = -1.0;
        private int ProjectID;

        public NewMDEProject(Hashtable TblData, SQLFunc sql, DateTime StartDate, MainWdw.CoinParam CoinParams)
        {
            InitializeComponent();

            this.sql = sql;
            this.sql.Err += Sql_Err;
            this.TblData = TblData;
            this.StartDate = StartDate;

            myCoin.PortionPayingEqty = 0.5;
            PowerProductionIncrease[0, 0] = 0.0;
            PowerProductionIncrease[1, 0] = 1.0;

            COD = StartDate;
            ProjectDetails.DivPaidBeforeTaxes = false;

            InitBoxes();
            InitModelParams();
            InitBtnLoc();
            InitBtnGraph();
            this.ProjectID = GetProjectID();
            this.CoinParams = CoinParams;
            myCoin.LastDividend = 0.2;
        }

        private int GetProjectID()
        {
            SQLFunc.SubResults sqlData = sql.Get1Data(TblData["Projects"].ToString(), "ProjectID", "ProjectID=(SELECT MAX(ProjectID) FROM " + TblData["Projects"].ToString() + ")");
            if (sqlData._SubResults.Count == 0) return 1;
            return Convert.ToInt32(sqlData._SubResults[0].ToString()) + 1;
        }

        public NewMDEProject(Hashtable TblData, SQLFunc sql,DateTime StartDate, SQLFunc.SubResults loadedData, MainWdw.CoinParam CoinParams)
        {
            InitializeComponent();
            IsLoading = true;

            this.sql = sql;
            this.sql.Err += Sql_Err;
            this.TblData = TblData;
            this.StartDate = StartDate;

            this.CoinParams = CoinParams;

            myCoin.PortionPayingEqty = 0.5;
            PowerProductionIncrease[0, 0] = 0.0;
            PowerProductionIncrease[1, 0] = 1.0;

            InitBoxes();
            InitBtnLoc();
            InitBtnGraph();

            ProjectDetails.ProjectName = loadedData._SubResults[0].ToString();
            projectNameBox.Text = loadedData._SubResults[0].ToString();
            ProjectDetails.Sector = loadedData._SubResults[2].ToString();
            projectTypeBox.Text = loadedData._SubResults[2].ToString();
            ProjectDetails.SubSector = loadedData._SubResults[3].ToString();
            projectSubTypeBox.Text = loadedData._SubResults[3].ToString();
            ProjectDetails.Country = loadedData._SubResults[1].ToString();
            countryBox.Text = loadedData._SubResults[1].ToString();
            ProjectDetails.RefCurrency = loadedData._SubResults[4].ToString();
            referenceCurBox.Text = loadedData._SubResults[4].ToString();
            ProjectDetails.PPACurrency = loadedData._SubResults[5].ToString();
            curPPABox.Text = loadedData._SubResults[5].ToString();

            try
            {
                string[] OtherData = XMLFunctions.GetData_FromXMLOtherDetails(loadedData._SubResults[6].ToString());
                ProjectDetails.POName = OtherData[0];
                POBox.Text = OtherData[0];
                ProjectDetails.Auditor = OtherData[1];
                auditorBox.Text = OtherData[1];
                ProjectDetails.EPCName = OtherData[2];
                EPCBox.Text = OtherData[2];
                
                divPaidBeforeTaxesBox.Checked = Convert.ToBoolean(OtherData[3]);
                ProjectDetails.DivPaidBeforeTaxes = divPaidBeforeTaxesBox.Checked;
                ProjectDetails.PortionCash4CMC = GlobalFunc.ToDouble(OtherData[4]);
                portionCash4CMCBox.Text = (ProjectDetails.PortionCash4CMC * 100).ToString();
                ProjectDetails.AuditorFee = GlobalFunc.ToDouble(OtherData[5]);
                ProjectDetails.EquityCap = GlobalFunc.ToDouble(OtherData[6]);
                ProjectDetails.EquityPortion = GlobalFunc.ToDouble(OtherData[7]);
                equityCapBox.Text = (ProjectDetails.EquityCap * 100).ToString();
                equityPortionCoinLbl.Text = (ProjectDetails.EquityPortion * 100).ToString() + "%";
                

                ConcPBox.Text = loadedData._SubResults[7].ToString();
                ConcP = GlobalFunc.ToDouble(ConcPBox.Text);
                EndDate = StartDate.AddMonths((int)(ConcP * 12));

                string Model = "";
                ConstCost = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[8].ToString(), ref Model);
                ConstCFGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                for (int i = 0; i < ConstCost.GetUpperBound(0) + 1; i++)
                {
                    string[] rowData = new string[] { ConstCost[i, 0].ToString(), ConstCost[i, 1].ToString() };
                    ConstCFGrid.Rows.Add(rowData);
                    ConstCFGrid.Rows[i].HeaderCell.Value = "Month " + (i + 1).ToString();
                }
                
                ConstCFGrid.Visible = true;

                projectData.Visible = true;

                Const_Params = XMLFunctions.GetData_FromConstrParamXML(loadedData._SubResults[9].ToString());
                COD = StartDate.AddMonths(Const_Params.CP + Const_Params.Delay);
                CPBox.Text = Const_Params.CP.ToString();
                capexIncreaseBox.Text = (Const_Params.CAPEXIncrease * 100).ToString();
                capexIncreaseLbl.Visible = true;
                capexIncreaseBox.Visible = true;
                delayBox.Text = Const_Params.Delay.ToString();
                delayLbl.Visible = true;
                delayBox.Visible = true;
                interestCoverageBox.Text = (Const_Params.InterestCoverageRatio * 100).ToString();
                interestCoverageLbl.Visible = true;
                interestCoverageBox.Visible = true;
                workingCapitalCPBox.Text = GlobalFunc.FormatText(Const_Params.MinWorkingCapital.ToString(), GlobalFunc.FormatType.NumberNoDec);
                workingCapitalCPLbl.Visible = true;
                workingCapitalCPBox.Visible = true;

                switch (Const_Params.InterestCoveragePeriod)
                {
                    case Project.InterestCoveragePeriod._3M:
                        interestCoveragePeriodBox.Text = "3M";
                        break;
                    case Project.InterestCoveragePeriod._6M:
                        interestCoveragePeriodBox.Text = "6M";
                        break;
                    case Project.InterestCoveragePeriod._9M:
                        interestCoveragePeriodBox.Text = "9M";
                        break;
                    case Project.InterestCoveragePeriod._12M:
                        interestCoveragePeriodBox.Text = "12M";
                        break;
                    case Project.InterestCoveragePeriod._15M:
                        interestCoveragePeriodBox.Text = "15M";
                        break;
                    case Project.InterestCoveragePeriod._18M:
                        interestCoveragePeriodBox.Text = "18M";
                        break;
                    default:
                        interestCoveragePeriodBox.Text = "0M";
                        break;
                }

                double[] tmpDblM = XMLFunctions.GetData_FromEnergyProductionXML(loadedData._SubResults[10].ToString());
                Power = tmpDblM[0];
                powerBox.Text = Power.ToString();
                PlantFactor = tmpDblM[1];
                plantFactorBox.Text = (PlantFactor * 100).ToString();
                EnergyProduction = tmpDblM[2];
                energyProductionBox.Text = EnergyProduction.ToString();

                PPA = XMLFunctions.GetData_VectorFromXML(loadedData._SubResults[11].ToString(), ref Model);

                PPA_Q = new double[(int)(ConcP * 4)];
                for(int i = 0; i < PPA_Q.Length; i++)
                {
                    PPA_Q[i] = PPA[3 * i];
                }
                UpdateGrid(GridType.PPA);
                DoGraph(PPAChart, "PPA", PPA, chart.SeriesChartType.Line, Color.Blue);

                GlobalRisk = XMLFunctions.GetData_VectorFromXML(loadedData._SubResults[12].ToString(), ref Model);

                EquityPO = XMLFunctions.GetData_FromEquityXML(loadedData._SubResults[13].ToString());
                equitiesFromPOBox.Text = GlobalFunc.FormatText(EquityPO.Amount.ToString(), GlobalFunc.FormatType.NumberNoDec);
                equityPOApprecBox.Text = (EquityPO.Appreciation * 100).ToString();

                EquityOtherInvest = XMLFunctions.GetData_FromEquityXML(loadedData._SubResults[14].ToString());
                equitiesFromOtherInvestorsBox.Text = GlobalFunc.FormatText(EquityOtherInvest.Amount.ToString(), GlobalFunc.FormatType.NumberNoDec);
                equityOtherInvestApprecBox.Text = (EquityOtherInvest.Appreciation * 100).ToString();

                myLoans = XMLFunctions.GetData_FromLoanXML(loadedData._SubResults[15].ToString());

                for (int i = 0; i < myLoans.Count; i++)
                {
                    loanGrid.Rows.Add();
                    loanGrid.Rows[i].HeaderCell.Value = "Loan " + (i + 1);
                    loanGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                }
                
                UpdateLoanGrid();

                loanGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                
                for (int i = 0; i < myLoans.Count; i++)
                {
                    if(myLoans[i].Nominal == 0) { goto NextLoan; }
                    loanGrid.Rows[i].Cells[0].Value = GlobalFunc.FormatText(myLoans[i].Nominal.ToString(), GlobalFunc.FormatType.NumberNoDec);
                    loanGrid.Rows[i].Cells[1].Value = "Q" + ((int)(myLoans[i].Start / 3 + 1)).ToString();
                    loanGrid.Rows[i].Cells[2].Value = myLoans[i].Tenor.ToString();
                    loanGrid.Rows[i].Cells[3].Value = myLoans[i].GracePeriod.ToString();
                    loanGrid.Rows[i].Cells[4].Value = myLoans[i].LinearWithdraw;
                    loanGrid.Rows[i].Cells[5].Value = (myLoans[i].Rate * 100).ToString();
                    loanGrid.Rows[i].Cells[6].Value = (myLoans[i].UpfrontFee * 100).ToString();
                    loanGrid.Rows[i].Cells[7].Value = (myLoans[i].CommitmentFee * 100).ToString();
                    switch(myLoans[i].RateType)
                    {
                        case Project.rateType.Fixed:
                            loanGrid.Rows[i].Cells[8].Value = "Fixed";
                            break;
                        case Project.rateType.Variable:
                            loanGrid.Rows[i].Cells[8].Value = "Variable";
                            break;
                    }
                    switch (myLoans[i].Frequency)
                    {
                        case Project.Frequency.Quarterly:
                            loanGrid.Rows[i].Cells[9].Value = "Quarterly";
                            break;
                        case Project.Frequency.SemiAnnually:
                            loanGrid.Rows[i].Cells[9].Value = "Semi Annually";
                            break;
                        case Project.Frequency.Annually:
                            loanGrid.Rows[i].Cells[9].Value = "Annually";
                            break;
                    }
                    loanGrid.Rows[i].Cells[10].Value = myLoans[i].IsBullet;

                    NextLoan:
                    int k = 0;
                }

                myCoin = XMLFunctions.GetData_FromCoinXML(loadedData._SubResults[16].ToString());
                coinNominalBox.Text = GlobalFunc.FormatText(myCoin.Nominal.ToString(), GlobalFunc.FormatType.NumberNoDec);
                coinsDebtTenorBox.Text = myCoin.DebtTenor.ToString();
                coinIssuanceCostBox.Text = (myCoin.IssuanceCost * 100).ToString();
                coinConversionRateBox.Text = (myCoin.Conversion * 100).ToString();
                coinInterestAfterCPBox.Text = (myCoin.RateAfterCP * 100).ToString();
                coinInterestBeforeCPBox.Text = (myCoin.RateDuringCP * 100).ToString();
                payInterestOnEquityDuringCPBox.Checked = myCoin.PayEqtyInterestDuringCP;
                percentageOfEquityPaidBox.Text = (myCoin.PortionPayingEqty * 100).ToString();
                lastDivPaidCoinsBox.Text = (myCoin.LastDividend * 100).ToString();
                minDivIncreaseBox.Text = (myCoin.MinDivIncrease * 100).ToString();

                switch(myCoin.Frequency)
                {
                    case Project.Frequency.Quarterly:
                        coinsFrequencyBox.Text = "Quarterly";
                        break;
                    case Project.Frequency.SemiAnnually:
                        coinsFrequencyBox.Text = "Semi Annually";
                        break;
                    case Project.Frequency.Annually:
                        coinsFrequencyBox.Text = "Annually";
                        break;
                }
                if (myCoin.IsBullet) coinDebtTypeBox.Text = "Bullet";
                else coinDebtTypeBox.Text = "Term Loan";

                OM = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[17].ToString(), ref Model);
                for(int i = 0; i < OM.GetUpperBound(1) + 1;i++)
                {
                    string[] data = new string[] { ((int)(OM[0, i] / 12 + 1)).ToString(), ((int)(OM[1, i] / 12)).ToString(), (OM[2, i] * 100).ToString() };
                    OMGrid.Rows.Add(data);
                }

                mySGA = XMLFunctions.GetData_FromSGAXML(loadedData._SubResults[18].ToString());
                SGABox.Text = (mySGA.Rate * 100).ToString();
                switch(mySGA.Type)
                {
                    case Project.SGAType.GrossIncome:
                        SGATypeBox.Text = "Gross Income";
                        break;
                    case Project.SGAType.TotalRevenues:
                        SGATypeBox.Text = "Total Revenues";
                        break;
                }
                SGAMinCostBox.Text = GlobalFunc.FormatText(mySGA.MinCost.ToString(), GlobalFunc.FormatType.NumberNoDec);

                Royalties = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[19].ToString(), ref Model);
                for (int i = 0; i < Royalties.GetUpperBound(1) + 1; i++)
                {
                    if (Royalties[0, i] == 0) break;
                    string[] data = new string[] { ((int)(Royalties[0, i] / 12 + 1)).ToString(), ((int)(Royalties[1, i] / 12)).ToString(), (Royalties[2, i] * 100).ToString() };
                    royaltiesGrid.Rows.Add(data);
                }

                Taxes = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[20].ToString(), ref Model);
                for (int i = 0; i < Taxes.GetUpperBound(1) + 1; i++)
                {
                    if (Taxes[0, i] == 0) break;
                    string[] data = new string[] { ((int)(Taxes[0, i] / 12 + 1)).ToString(), ((int)(Taxes[1, i] / 12)).ToString(), (Taxes[2, i] * 100).ToString() };
                    taxesGrid.Rows.Add(data);
                }

                PowerProductionIncrease = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[21].ToString(), ref Model);
                for(int i = 0; i < PowerProductionIncrease.GetUpperBound(1) + 1; i++)
                {
                    string[] rowData = new string[] { PowerProductionIncrease[0, i].ToString(), (PowerProductionIncrease[1, i] * 100).ToString() };
                    powerProductionIncreaseGrid.Rows.Add(rowData);
                }

                ParamDegradation = XMLFunctions.GetData_VectorFromXML(loadedData._SubResults[22].ToString(), ref Model);
                if(((double[])ParamDegradation).Length == 1 && !Model.Equals("Linear")) ParamDegradation = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[22].ToString(), ref Model);
                degradationBox.Text = Model;
                if(Model.Equals("Linear")) DegradationImpact = Risk.Risk.DegradationRisk(COD, StartDate, EndDate, Model, PowerProductionIncrease, ((double[])ParamDegradation)[0]);
                else DegradationImpact = Risk.Risk.DegradationRisk(COD, StartDate, EndDate, Model, PowerProductionIncrease, ParamDegradation);
                degradationImpactChart.Series.Clear();
                DoGraph(degradationImpactChart, "Degradation", DegradationImpact, chart.SeriesChartType.Line, Color.Blue);

                changeDegradationBtn.Visible = true;
                changeDegradationBtn.Location = new System.Drawing.Point(ChangeDegradBtnLoc[0] - energyProdTab.HorizontalScroll.Value, ChangeDegradBtnLoc[1] - energyProdTab.VerticalScroll.Value);

                ParamClimateImpact = XMLFunctions.GetData_VectorFromXML(loadedData._SubResults[23].ToString(), ref Model);
                if (((double[])ParamClimateImpact).Length == 1 && !Model.Equals("Linear")) ParamClimateImpact = XMLFunctions.GetData_MatrixFromXML(loadedData._SubResults[23].ToString(), ref Model);
                climateImpactBox.Text = Model;
                if(Model.Equals("Linear")) ClimateImpact = Risk.Risk.ClimateImpactRisk(COD, StartDate, EndDate, Model, ((double[])ParamDegradation)[0]);
                else ClimateImpact = Risk.Risk.ClimateImpactRisk(COD, StartDate, EndDate, Model, ParamClimateImpact);
                climateImpactChart.Series.Clear();
                DoGraph(climateImpactChart, "Climate", ClimateImpact, chart.SeriesChartType.Line, Color.Blue);

                changeClimateImpactBtn.Visible = true;
                changeClimateImpactBtn.Location = new System.Drawing.Point(ChangeClimBtnLoc[0] - energyProdTab.HorizontalScroll.Value, ChangeClimBtnLoc[1] - energyProdTab.VerticalScroll.Value);

                string tmpStr = "";
                this.Params = XMLFunctions.GetData_FromParametersXML(loadedData._SubResults[24].ToString(), ref tmpStr);
                FXModelBox.Text = tmpStr;
                FXImpact = Risk.Risk.FXRisk(sql, TblData, iCur, pCur, StartDate, EndDate, FXModelBox.Text);
                fXImpactChart.Series.Clear();
                DoGraph(fXImpactChart, "FX Impact", FXImpact, chart.SeriesChartType.Line, Color.Blue);

                if(GlobalRisk[0] == 0)
                {
                    for(int i = 0; i < GlobalRisk.Length; i++)
                    {
                        GlobalRisk[i] = ClimateImpact[i] * DegradationImpact[i] * FXImpact[i];
                    }
                }

                DoGraph(globalRiskChart, "GlobalRisk", GlobalRisk, chart.SeriesChartType.Line, Color.Red);

                ProjectID = Convert.ToInt32(loadedData._SubResults[25]);

                IsLoading = false;
                auditorBox.Enabled = false;
                countryBox.Enabled = false;
                projectTypeBox.Enabled = false;
                projectSubTypeBox.Enabled = false;

                GetDepositRateDecrease();
            }
            catch
            {
                LoadingProcessAborted("Issue while reading XMLs");
            }
        }

        private void GetDepositRateDecrease()
        {
            SQLFunc.SubResults data = sql.Get1Data(TblData["DepositRateDecrease"].ToString(), "Value", "pCur='" + referenceCurBox.Text + "'");

            if (data._SubResults.Count == 0) depositRateDecreaseBox.Text = "0";
            else { DepositRateDecrease = GlobalFunc.ToDouble(data._SubResults[0].ToString()); depositRateDecreaseBox.Text = (DepositRateDecrease * 100).ToString(); }
        }

        private void Sql_Err(object sender, IErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void InitBoxes()
        {
            List<SQLFunc.SubResults> sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Name,Country", "Field='EPC' or Field='PO'");
            List<string> sortData = Sort(sqlData, false);

            for (int i= 0; i< sqlData.Count;i++)
            {
                POBox.Items.Add(sortData[i]);
                POBox.Text = POBox.Items[0].ToString();
                ProjectDetails.POName = POBox.Text;
            }

            sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Name", "Field='Audit'");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                auditorBox.Items.Add(sortData[i]);
                auditorBox.Text = auditorBox.Items[0].ToString();
                ProjectDetails.Auditor = auditorBox.Text;
            }

            sqlData = sql.GetData(TblData["pCur"].ToString(), "Name");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                referenceCurBox.Items.Add(sortData[i]);
            }

            sqlData = sql.GetData(TblData["iCur"].ToString(), "Name");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                curPPABox.Items.Add(sortData[i]);
            }

            sqlData = sql.GetData(TblData["DegradationModels"].ToString(), "Model");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                degradationBox.Items.Add(sortData[i]);
            }

            sqlData = sql.GetData(TblData["ClimateModels"].ToString(), "Model");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                climateImpactBox.Items.Add(sortData[i]);
            }

            sqlData = sql.GetData(TblData["FXModels"].ToString(), "Name");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                FXModelBox.Items.Add(sortData[i]);
            }

            sqlData = sql.GetData(TblData["ProjectType"].ToString(), "*");
            sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                projectTypeBox.Items.Add(sortData[i]);
            }

            coinDebtTypeBox.Items.Add("Bullet");
            coinDebtTypeBox.Items.Add("Term Loan");
            coinDebtTypeBox.Text = coinDebtTypeBox.Items[1].ToString();

            SGATypeBox.Items.Add("Total Revenues");
            SGATypeBox.Items.Add("Gross Income");
            SGATypeBox.Text = SGATypeBox.Items[0].ToString();

            coinsFrequencyBox.Items.Add("Quarterly");
            coinsFrequencyBox.Items.Add("Semi Annually");
            coinsFrequencyBox.Items.Add("Annually");

            interestCoveragePeriodBox.Items.Add("0M");
            interestCoveragePeriodBox.Items.Add("3M");
            interestCoveragePeriodBox.Items.Add("6M");
            interestCoveragePeriodBox.Items.Add("9M");
            interestCoveragePeriodBox.Items.Add("12M");
            interestCoveragePeriodBox.Items.Add("15M");
            interestCoveragePeriodBox.Items.Add("18M");
            interestCoveragePeriodBox.Text = interestCoveragePeriodBox.Items[0].ToString();
        }

        private void InitModelParams()
        {
            Params["MaxNominalIncrease"] = 50;
            Params["MaxConstructionLoop"] = 5;
        }

        private void InitBtnLoc()
        {
            SetPPABtnLoc[0] = setPPABtn.Location.X;
            SetPPABtnLoc[1] = setPPABtn.Location.Y;

            ChangeDegradBtnLoc[0] = changeDegradationBtn.Location.X;
            ChangeDegradBtnLoc[1] = changeDegradationBtn.Location.Y;

            ChangeClimBtnLoc[0] = changeClimateImpactBtn.Location.X;
            ChangeClimBtnLoc[1] = changeClimateImpactBtn.Location.Y;

            PercentageOfEquityPaidBoxLoc[0] = percentageOfEquityPaidBox.Location.X;
            PercentageOfEquityPaidBoxLoc[1] = percentageOfEquityPaidBox.Location.Y;
        }

        private void InitBtnGraph()
        {
            string tmp = "\u2191";
            byte[] tmpb = UTF32Encoding.UTF32.GetBytes(tmp);
            goupGlobalRiskBtn.Text = UnicodeEncoding.UTF32.GetString(tmpb);

            tmp = "\u2193";
            tmpb = UTF32Encoding.UTF32.GetBytes(tmp);
            godownGlobalRiskBtn.Text = UnicodeEncoding.UTF32.GetString(tmpb);

            tmp = "\u2190";
            tmpb = UTF32Encoding.UTF32.GetBytes(tmp);
            goleftGlobalRiskBtn.Text = UnicodeEncoding.UTF32.GetString(tmpb);

            tmp = "\u2192";
            tmpb = UTF32Encoding.UTF32.GetBytes(tmp);
            gorightGlobalRiskBtn.Text = UnicodeEncoding.UTF32.GetString(tmpb);
        }

        #region Textboxes enter
        private void ConcPBox_Enter(object sender, EventArgs e)
        {
            indicationConcPLbl1.Visible = true;
            indicationConcPLbl2.Visible = true;
            indicationConcPLbl1.Text = "Concession Period means the total number of years of";
            indicationConcPLbl2.Text = "activity, including the Construction Period";
        }
        #endregion

        #region Grid leave
        private void loanGrid_Leave(object sender, EventArgs e)
        {
            myLoans = new List<Project.Loan>();
            errorMsgLbl.Visible = false;

            try
            {
                for (int i = 0; i < loanGrid.Rows.Count; i++)
                {
                    Project.Loan loan = new Project.Loan();
                    string tmp = loanGrid.Rows[i].Cells[0].Value.ToString().Replace(",", "");
                    loan.Nominal = GlobalFunc.ToDouble(tmp);
                    loanGrid.Rows[i].Cells[0].Value = GlobalFunc.FormatText(tmp, GlobalFunc.FormatType.NumberNoDec);
                    tmp = loanGrid.Rows[i].Cells[1].Value.ToString();
                    loan.Start = (Convert.ToInt32(tmp.Substring(1, tmp.Length - 1)) - 1) * 3;
                    loan.Tenor = GlobalFunc.ToDouble(loanGrid.Rows[i].Cells[2].Value.ToString());
                    loan.GracePeriod = GlobalFunc.ToDouble(loanGrid.Rows[i].Cells[3].Value.ToString());
                    if (loan.GracePeriod > loan.Tenor) MessageBox.Show("Grace Period for loan " + (i + 1) + " is bigger than Tenor");
                    loan.LinearWithdraw = Convert.ToBoolean(loanGrid.Rows[i].Cells[4].Value);
                    loan.Rate = GlobalFunc.ToDouble(loanGrid.Rows[i].Cells[5].Value.ToString()) / 100;
                    if (loan.Rate > 0.15) MessageBox.Show("Are you sure about rate for loan " + (i + 1) + "?");
                    loan.UpfrontFee = GlobalFunc.ToDouble(loanGrid.Rows[i].Cells[6].Value.ToString()) / 100;
                    if (loan.UpfrontFee > 0.02) MessageBox.Show("Are you sure about Upfront Fee for loan " + (i + 1) + "?");
                    loan.CommitmentFee = GlobalFunc.ToDouble(loanGrid.Rows[i].Cells[7].Value.ToString()) / 100;
                    if (loan.CommitmentFee > 0.01) MessageBox.Show("Are you sure about Commitment Fee for loan " + (i + 1) + "?");
                    if(loan.Rate < 0 || loan.UpfrontFee < 0 || loan.CommitmentFee < 0)
                    {
                        MessageBox.Show("Process aborted for loan " + (i + 1) + " as some rates are negative.");
                        myLoans = new List<Project.Loan>();
                        return;
                    }
                    tmp = loanGrid.Rows[i].Cells[8].Value.ToString();
                    switch (tmp)
                    {
                        case "Variable":
                            loan.RateType = Project.rateType.Variable;
                            break;
                        default:
                            loan.RateType = Project.rateType.Fixed;
                            break;
                    }
                    tmp = loanGrid.Rows[i].Cells[9].Value.ToString();
                    switch (tmp)
                    {
                        case "Quarterly":
                            loan.Frequency = Project.Frequency.Quarterly;
                            break;
                        case "Semi Annually":
                            loan.Frequency = Project.Frequency.SemiAnnually;
                            break;
                        case "Annually":
                            loan.Frequency = Project.Frequency.Annually;
                            break;
                    }
                    loan.IsBullet = Convert.ToBoolean(loanGrid.Rows[i].Cells[10].Value);

                    myLoans.Add(loan);
                }
            }
            catch
            {
                errorMsgLbl.Visible = true;
                errorMsgLbl.Text = "Issue while converting loan data";
            }

        }

        private void OMGrid_Leave(object sender, EventArgs e)
        {
            if (OMGrid.Rows.Count == 1)
            {
                OM = new double[3, 1];
                OMGrid.Rows.Add();
                OMGrid.Rows[0].Cells[0].Value = ((int)(Const_Params.CP / 12) + 1).ToString();
                OMGrid.Rows[0].Cells[1].Value = ConcP.ToString();
                OMGrid.Rows[0].Cells[2].Value = "0";

                OM[0, 0] = Const_Params.CP;
                OM[1, 0] = ConcP * 12;
                OM[2, 0] = 0.0;
            }
            else
            {
                OM = new double[3, OMGrid.Rows.Count - 1];

                for (int i = 0; i < OMGrid.Rows.Count - 1; i++)
                {
                    OM[0, i] = Math.Max((GlobalFunc.ToDouble(OMGrid.Rows[i].Cells[0].Value.ToString()) - 1) * 12, Const_Params.CP);
                    OM[1, i] = Math.Max(GlobalFunc.ToDouble(OMGrid.Rows[i].Cells[1].Value.ToString()) * 12, Const_Params.CP);
                    OM[2, i] = GlobalFunc.ToDouble(OMGrid.Rows[i].Cells[2].Value.ToString()) / 100;
                    if (OM[2, i] > 0.5) MessageBox.Show("Are you sure about O&M at line " + (i + 1) + "?");
                    if (OM[2, i] < 0)
                    {
                        OM[2, i] = 0;
                        royaltiesGrid.Rows[i].Cells[2].Value = OM[2, i].ToString();
                    }
                }
            }
        }

        private void royaltiesGrid_Leave(object sender, EventArgs e)
        {
            try
            {
                if (royaltiesGrid.Rows.Count == 1)
                {
                    Royalties = new double[3, 1];
                    royaltiesGrid.Rows.Add();
                    royaltiesGrid.Rows[0].Cells[0].Value = ((int)(Const_Params.CP / 12) + 1).ToString();
                    royaltiesGrid.Rows[0].Cells[1].Value = ConcP.ToString();
                    royaltiesGrid.Rows[0].Cells[2].Value = "0";
                    Royalties[0, 0] = Const_Params.CP;
                    Royalties[1, 0] = ConcP * 12;
                    Royalties[2, 0] = 0.0;
                }
                else
                {
                    Royalties = new double[3, royaltiesGrid.Rows.Count - 1];

                    for (int i = 0; i < royaltiesGrid.Rows.Count - 1; i++)
                    {
                        Royalties[0, i] = Math.Max((GlobalFunc.ToDouble(royaltiesGrid.Rows[i].Cells[0].Value.ToString()) - 1) * 12, Const_Params.CP);
                        Royalties[1, i] = Math.Max(GlobalFunc.ToDouble(royaltiesGrid.Rows[i].Cells[1].Value.ToString()) * 12, Const_Params.CP);
                        Royalties[2, i] = GlobalFunc.ToDouble(royaltiesGrid.Rows[i].Cells[2].Value.ToString()) / 100;
                        if (Royalties[2, i] > 0.5) MessageBox.Show("Are you sure about royalty fee at line " + (i + 1) + "?");
                        if (Royalties[2, i] < 0)
                        {
                            Royalties[2, i] = 0;
                            royaltiesGrid.Rows[i].Cells[2].Value = Royalties[2, i].ToString();
                        }
                    }
                }
            }
            catch
            {
                commScreen.AppendText("\nIssue while processing the Royalties.\n");
                projectData.SelectTab("outputTab");
                return;
            }
        }

        private void taxesGrid_Leave(object sender, EventArgs e)
        {
            try
            {
                if (taxesGrid.Rows.Count == 1)
                {
                    Taxes = new double[3, 1];
                    taxesGrid.Rows.Add();
                    taxesGrid.Rows[0].Cells[0].Value = ((int)(Const_Params.CP / 12) + 1).ToString();
                    taxesGrid.Rows[0].Cells[1].Value = ConcP.ToString();
                    taxesGrid.Rows[0].Cells[2].Value = "0";
                    Taxes[0, 0] = Const_Params.CP;
                    Taxes[1, 0] = ConcP * 12;
                    Taxes[2, 0] = 0.0;
                }
                else
                {
                    Taxes = new double[3, taxesGrid.Rows.Count - 1];

                    for (int i = 0; i < taxesGrid.Rows.Count - 1; i++)
                    {
                        Taxes[0, i] = Math.Max((GlobalFunc.ToDouble(taxesGrid.Rows[i].Cells[0].Value.ToString()) - 1) * 12, Const_Params.CP);
                        Taxes[1, i] = Math.Max(GlobalFunc.ToDouble(taxesGrid.Rows[i].Cells[1].Value.ToString()) * 12, Const_Params.CP);
                        Taxes[2, i] = GlobalFunc.ToDouble(taxesGrid.Rows[i].Cells[2].Value.ToString()) / 100;
                        if (Taxes[2, i] > 0.5) MessageBox.Show("Are you sure about tax fee at line " + (i + 1) + "?");
                        if (Taxes[2, i] < 0)
                        {
                            Taxes[2, i] = 0;
                            taxesGrid.Rows[i].Cells[2].Value = Taxes[2, i].ToString();
                        }
                    }
                }
            }
            catch
            {
                commScreen.AppendText("\nIssue while processing the Taxes.\n");
                projectData.SelectTab("outputTab");
                return;
            }
        }

        private void powerIncreaseGrid_Leave(object sender, EventArgs e)
        {
            int NbRows = powerProductionIncreaseGrid.Rows.Count - 1;
            if (NbRows == 0) return;
            PowerProductionIncrease = new double[2, NbRows];
            double PreviousValue = 0.0;

            for (int i = 0; i < NbRows; i++)
            {
                PowerProductionIncrease[0, i] = GlobalFunc.ToDouble(powerProductionIncreaseGrid.Rows[i].Cells[0].Value.ToString()) + PreviousValue;
                PowerProductionIncrease[1, i] = GlobalFunc.ToDouble(powerProductionIncreaseGrid.Rows[i].Cells[1].Value.ToString()) / 100;
                PreviousValue = PowerProductionIncrease[0, i];
            }
        }

        private void ConstCFGrid_Leave(object sender, EventArgs e)
        {
            ConstCost = new double[Const_Params.CP, 2];

            for (int i = 0; i < ConstCFGrid.Rows.Count; i++)
            {
                ConstCost[i, 0] = GlobalFunc.ToDouble(ConstCFGrid.Rows[i].Cells[0].Value.ToString());
                ConstCost[i, 1] = GlobalFunc.ToDouble(ConstCFGrid.Rows[i].Cells[1].Value.ToString());
            }
        }

        #endregion

        #region Textboxes leave
        private void ConcPBox_Leave(object sender, EventArgs e)
        {
            indicationConcPLbl1.Visible = false;
            indicationConcPLbl2.Visible = false;

            ConcP = GlobalFunc.ToDouble(ConcPBox.Text);
            if(ConcP < 0)
            {
                ConcP = 0;
                ConcPBox.Text = "0";
            }
            GlobalRisk = new double[(int)(ConcP * 12)];
            for (int i = 0; i < GlobalRisk.Length; i++)
            {
                GlobalRisk[i] = 1.0;
            }

            EndDate = StartDate.AddMonths((int)(ConcP * 12));

            if (projectData.Visible)
            {
                if (!FXModelBox.Text.Equals("")) UpdateFX();
                if (!degradationBox.Text.Equals("")) UpdateDegradation();
                if (!climateImpactBox.Text.Equals("")) UpdateClimate();
                UpdateGrid(GridType.PPA);
                UpdateLoanGrid();
                UpdateOMGrid();
            }
        }

        private void CPBox_Leave(object sender, EventArgs e)
        {
            try
            {
                Const_Params.CP = Convert.ToInt32(CPBox.Text);
                if(Const_Params.CP < 0)
                {
                    Const_Params.CP = 0;
                    CPBox.Text = "0";
                }

                COD = StartDate.AddMonths(Const_Params.CP + Const_Params.Delay);
                if (!FXModelBox.Text.Equals("")) UpdateFX();
                if(!degradationBox.Text.Equals("")) UpdateDegradation();
                if (!climateImpactBox.Text.Equals("")) UpdateClimate();
                UpdatePPAGraph();
                UpdateOMGrid();
            }
            catch
            { }
        }

        private void workingCapitalCPBox_Leave(object sender, EventArgs e)
        {
            Const_Params.MinWorkingCapital = GlobalFunc.ToDouble(workingCapitalCPBox.Text.Replace(",", ""));
            GlobalFunc.FormatTextBox(workingCapitalCPBox, GlobalFunc.FormatType.NumberNoDec);
        }

        private void coinIssuanceCostBox_Leave(object sender, EventArgs e)
        {
            myCoin.IssuanceCost = GlobalFunc.ToDouble(coinIssuanceCostBox.Text) / 100;
            if (myCoin.IssuanceCost > 0.05) MessageBox.Show("Are you sure about coin issuance rate?");
            if (myCoin.IssuanceCost < 0)
            {
                myCoin.IssuanceCost = 0;
                coinIssuanceCostBox.Text = "0";
            }
        }

        private void capexIncreaseBox_Leave(object sender, EventArgs e)
        {
            Const_Params.CAPEXIncrease = GlobalFunc.ToDouble(capexIncreaseBox.Text) / 100;
            if (Const_Params.CAPEXIncrease > 0.15) MessageBox.Show("Are you sure about CAPEX increase rate?");
            if (Const_Params.CAPEXIncrease < 0)
            {
                Const_Params.CAPEXIncrease = 0.0;
                capexIncreaseBox.Text = "0";
            }
        }

        private void delayBox_Leave(object sender, EventArgs e)
        {
            try
            {
                Const_Params.Delay = Convert.ToInt32(delayBox.Text);
                if(Const_Params.Delay < 0)
                {
                    Const_Params.Delay = 0;
                    delayBox.Text = "0";
                }
                if (Const_Params.Delay > 6) MessageBox.Show("Are you sure about delay bigger than 6 months?");

                COD = StartDate.AddMonths(Const_Params.CP + Const_Params.Delay);

                if (!FXModelBox.Text.Equals("")) UpdateFX();
                if (!degradationBox.Text.Equals("")) UpdateDegradation();
                if (!climateImpactBox.Text.Equals("")) UpdateClimate();
            }
            catch
            {
                delayBox.Text = "0";
                Const_Params.Delay = 0;
            }
        }

        private void powerBox_Leave(object sender, EventArgs e)
        {
            Power = GlobalFunc.ToDouble(powerBox.Text);
            GlobalFunc.FormatTextBox(powerBox, GlobalFunc.FormatType.NumberWithDec);

            if (PlantFactor != 0) { EnergyProduction = Math.Round(365 * 24 * Power * PlantFactor); energyProductionBox.Text = EnergyProduction.ToString(); GlobalFunc.FormatTextBox(energyProductionBox, GlobalFunc.FormatType.NumberWithDec); }
        }

        private void plantFactorBox_Leave(object sender, EventArgs e)
        {
            PlantFactor = GlobalFunc.ToDouble(plantFactorBox.Text) / 100;
            if(PlantFactor < 0)
            {
                PlantFactor = 0;
                plantFactorBox.Text = "0";
            }

            if (Power != 0) { EnergyProduction = Math.Round(365 * 24 * Power * PlantFactor); energyProductionBox.Text = EnergyProduction.ToString(); GlobalFunc.FormatTextBox(energyProductionBox, GlobalFunc.FormatType.NumberWithDec); }
        }

        private void averagePriceBox_Leave(object sender, EventArgs e)
        {
            AveragePrice = GlobalFunc.ToDouble(averagePriceBox.Text);

            if (AveragePrice != 0 && StartPrice != 0 && StartPeriod != 0)
            {
                setPPABtn.Visible = true;
                setPPABtn.Location = new System.Drawing.Point(SetPPABtnLoc[0] - energyProdTab.HorizontalScroll.Value, SetPPABtnLoc[1] - energyProdTab.VerticalScroll.Value);
            }
            else
            {
                setPPABtn.Visible = false;
            }
        }

        private void startPriceBox_Leave(object sender, EventArgs e)
        {
            StartPrice = GlobalFunc.ToDouble(startPriceBox.Text);

            if (AveragePrice != 0 && StartPrice != 0 && StartPeriod != 0)
            {
                setPPABtn.Visible = true;
                setPPABtn.Location = new System.Drawing.Point(SetPPABtnLoc[0] - energyProdTab.HorizontalScroll.Value, SetPPABtnLoc[1] - energyProdTab.VerticalScroll.Value);
            }
            else
            {
                setPPABtn.Visible = false;
            }
        }

        private void startPeriodLengthBox_Leave(object sender, EventArgs e)
        {
            try
            {
                StartPeriod = Convert.ToInt32(startPeriodLengthBox.Text);

                if (AveragePrice != 0 && StartPrice != 0 && StartPeriod != 0)
                {
                    setPPABtn.Visible = true;
                    setPPABtn.Location = new System.Drawing.Point(SetPPABtnLoc[0] - energyProdTab.HorizontalScroll.Value, SetPPABtnLoc[1] - energyProdTab.VerticalScroll.Value);
                }
                else
                {
                    setPPABtn.Visible = false;
                }
            }
            catch
            { }
        }

        private void interestCoverageBox_Leave(object sender, EventArgs e)
        {
            Const_Params.InterestCoverageRatio = GlobalFunc.ToDouble(interestCoverageBox.Text) / 100;
        }

        private void equitiesFromPOBox_Leave(object sender, EventArgs e)
        {
            EquityPO.Amount = GlobalFunc.ToDouble(equitiesFromPOBox.Text);
            EquityPO.HasSchedule = false;
            GlobalFunc.FormatTextBox(equitiesFromPOBox, GlobalFunc.FormatType.NumberNoDec);
        }

        private void equityPOApprecBox_Leave(object sender, EventArgs e)
        {
           double tmp = GlobalFunc.ToDouble(equityPOApprecBox.Text) / 100;
           if (tmp > 0.25) MessageBox.Show("Are you sure about appreciation rate of PO's equity?");
            if (tmp < 0)
            {
                tmp = 0;
                equitiesFromPOBox.Text = "0";
            }

            EquityPO.Appreciation = tmp;
        }

        private void equitiesFromOtherInvestorsBox_Leave(object sender, EventArgs e)
        {
            EquityOtherInvest.Amount = GlobalFunc.ToDouble(equitiesFromOtherInvestorsBox.Text);
            EquityOtherInvest.HasSchedule = false;
            GlobalFunc.FormatTextBox(equitiesFromOtherInvestorsBox, GlobalFunc.FormatType.NumberNoDec);
        }

        private void equityOtherInvestApprecBox_Leave(object sender, EventArgs e)
        {
            double tmp = GlobalFunc.ToDouble(equityOtherInvestApprecBox.Text) / 100;
            if (tmp > 0.25) MessageBox.Show("Are you sure about appreciation rate of other investors' equity?");
            if (tmp < 0)
            {
                tmp = 0;
                equityOtherInvestApprecBox.Text = "0";
            }

            EquityOtherInvest.Appreciation = tmp;
        }

        private void coinNominalBox_Leave(object sender, EventArgs e)
        {
            myCoin.Nominal = GlobalFunc.ToDouble(coinNominalBox.Text.Replace(",", ""));
            GlobalFunc.FormatTextBox(coinNominalBox, GlobalFunc.FormatType.NumberNoDec);
        }

        private void coinInterestBeforeCPBox_Leave(object sender, EventArgs e)
        {
            myCoin.RateDuringCP = GlobalFunc.ToDouble(coinInterestBeforeCPBox.Text) / 100;
            if (myCoin.RateDuringCP > 0.10) MessageBox.Show("Are you sure about rate during CP?");
            if (myCoin.RateDuringCP < 0)
            {
                myCoin.RateDuringCP = 0.0;
                coinInterestBeforeCPBox.Text = "0";
            }
        }

        private void coinInterestAfterCPBox_Leave(object sender, EventArgs e)
        {
            myCoin.RateAfterCP = GlobalFunc.ToDouble(coinInterestAfterCPBox.Text) / 100;
            if (myCoin.RateAfterCP > 0.10) MessageBox.Show("Are you sure about rate after CP?");
            if (myCoin.RateAfterCP < 0)
            {
                myCoin.RateAfterCP = 0.0;
                coinInterestAfterCPBox.Text = "0";
            }
        }

        private void coinConversionRateBox_Leave(object sender, EventArgs e)
        {
            myCoin.Conversion = GlobalFunc.ToDouble(coinConversionRateBox.Text) / 100;
            if (myCoin.Conversion > 1.0) { myCoin.Conversion = 1.0; coinConversionRateBox.Text = "100"; }
            if (myCoin.Conversion < 0)
            {
                myCoin.Conversion = 0.0;
                coinConversionRateBox.Text = "0";
            }
        }

        private void coinsDebtTenorBox_Leave(object sender, EventArgs e)
        {
            myCoin.DebtTenor = GlobalFunc.ToDouble(coinsDebtTenorBox.Text);
            if (myCoin.DebtTenor < 0)
            {
                myCoin.DebtTenor = 0.0;
                coinsDebtTenorBox.Text = "0";
            }
        }

        private void SGABox_Leave(object sender, EventArgs e)
        {
            mySGA.Rate = GlobalFunc.ToDouble(SGABox.Text) / 100;
            if(mySGA.Rate < 0)
            {
                mySGA.Rate = 0;
                SGABox.Text = "0";
            }
            if (mySGA.Rate > 0.30) MessageBox.Show("Are you sure about SG&A rate?");
        }

        private void percentageOfEquityPaidBox_Leave(object sender, EventArgs e)
        {
            DivPaidDuringCP = DivPaidDuringCP / (1 - myCoin.Conversion * (1 - myCoin.PortionPayingEqty));

            myCoin.PortionPayingEqty = GlobalFunc.ToDouble(percentageOfEquityPaidBox.Text) / 100;
            if (myCoin.PortionPayingEqty > 1) myCoin.PortionPayingEqty = 1.0;
            if (myCoin.PortionPayingEqty < 0) myCoin.PortionPayingEqty = 0.0;
            percentageOfEquityPaidBox.Text = (myCoin.PortionPayingEqty * 100).ToString();

            //DivPaidDuringCP = DivPaidDuringCP * (1 - myCoin.Conversion * (1 - myCoin.PortionPayingEqty));

            //UpdateDividendsDuringCP(true);
        }

        private void SGAMinCostBox_Leave(object sender, EventArgs e)
        {
            mySGA.MinCost = GlobalFunc.ToDouble(SGAMinCostBox.Text);
            GlobalFunc.FormatTextBox(SGAMinCostBox, GlobalFunc.FormatType.NumberNoDec);

            if (mySGA.MinCost < 0)
            {
                mySGA.MinCost = 0;
                SGAMinCostBox.Text = "0";
            }
        }
        
        private void projectNameBox_Leave(object sender, EventArgs e)
        {
            ProjectDetails.ProjectName = projectNameBox.Text;
        }

        private void interestOnEquityBox_Leave(object sender, EventArgs e)
        {
            EquityOtherInvest.Interest = GlobalFunc.ToDouble(interestOtherEquityBox.Text) / 100;
        }

        private void interestEquityPOBox_Leave(object sender, EventArgs e)
        {
            EquityPO.Interest = GlobalFunc.ToDouble(interestEquityPOBox.Text) / 100;
        }

        private void equityCapBox_Leave(object sender, EventArgs e)
        {
            ProjectDetails.EquityCap = GlobalFunc.ToDouble(equityCapBox.Text) / 100;

            if (ProjectDetails.EquityCap < 0) { ProjectDetails.EquityCap = 0; equityCapBox.Text = "0"; }
            if (ProjectDetails.EquityCap > 1) { ProjectDetails.EquityCap = 1; equityCapBox.Text = "100"; }
        }


        #endregion

        #region Comboboxes change
        private void POBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string POName = POBox.Text;
            string country = POName.Substring(POName.IndexOf("-") + 2, POName.Length - POName.IndexOf("-") - 2);
            POName = POName.Substring(0, POName.IndexOf("-") - 1);

            List<SQLFunc.SubResults> sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Field", "Name='" + POName + "' AND Country='" + country + "'");

            if (sqlData[0]._SubResults[0].ToString().Equals("PO"))
            {
                sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Name, Country", "Field='EPC'");
                List<string> sortData = Sort(sqlData, false);

                EPCBox.Items.Clear();
                EPCBox.Visible = true;
                EPCLbl.Visible = true;

                for (int i = 0; i < sqlData.Count; i++)
                {
                    EPCBox.Items.Add(sortData[i]);
                }

                sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Rating", "Name='" + POName + "' AND Country='" + country + "' AND Field='PO'");
                PORating = Convert.ToInt32(sqlData[0]._SubResults[0]);
            }
            else
            {
                sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Rating", "Name='" + POName + "' AND Country='" + country + "' AND Field='EPC'");
                PORating = Convert.ToInt32(sqlData[0]._SubResults[0]);
                EPCRating = PORating;

                EPCBox.Visible = false;
                EPCLbl.Visible = false;
            }
        }

        private void EPCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string EPCName = EPCBox.Text;
            string country = EPCName.Substring(EPCName.IndexOf("-") + 2, EPCName.Length - EPCName.IndexOf("-") - 2);
            EPCName = EPCName.Substring(0, EPCName.IndexOf("-") - 1);

            List<SQLFunc.SubResults> sqlData = sql.GetData(TblData["ThirdParties"].ToString(), "Rating", "Name='" + EPCName + "' AND Country='" + country + "' AND Field='EPC'");
            EPCRating = Convert.ToInt32(sqlData[0]._SubResults[0]);
        }

        private void referenceCurBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CountryChosen) inputBtn.Visible = true;
            pCurChosen = true;

            avgPriceLbl.Text = "Avg Price (" + referenceCurBox.Text + " per kWh):";
            startPriceLbl.Text = "Start Price(" + referenceCurBox.Text + " per kWh):";

            pCur = referenceCurBox.Text;
            if(iCur != null) if (!iCur.Equals("") && !FXModelBox.Text.Equals("")) UpdateFX();

            ProjectDetails.RefCurrency = referenceCurBox.Text;

            depositRateGrid.Rows.Clear();

            List<SQLFunc.SubResults> Data = sql.GetData(TblData["DepositRates"].ToString(), "Date,Tenor,Value", "pCur='" + referenceCurBox.Text + "' AND [Date]=(Select Max(Date) from " + TblData["DepositRates"].ToString() + ")");

            if (Data.Count == 0) return;

            string[] row = new string[Data[0]._SubResults.Count];

            for (int i = 0; i < Data.Count; i++)
            {
                row[0] = FormatDate(Convert.ToDateTime(Data[i]._SubResults[0]));
                row[1] = Data[i]._SubResults[1].ToString();
                row[2] = (GlobalFunc.ToDouble(Data[i]._SubResults[2].ToString()) * 100).ToString();

                depositRateGrid.Rows.Add(row);
            }

            GetDepositRateDecrease();
        }

        private string FormatDate(DateTime Date)
        {
            return Date.Year + "-" + Date.Month + "-" + Date.Day;
        }

        private void curPPABox_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCur = curPPABox.Text;

            if (!FXModelBox.Text.Equals("")) UpdateFX();

            ProjectDetails.PPACurrency = iCur;
        }
        
        private void projectTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (projectTypeBox.Text.Equals("") || projectTypeBox.Text.Equals(ProjectDetails.Sector)) return;

            projectSubTypeBox.Items.Clear();
            projectSubTypeBox.Text = "";
            countryBox.Items.Clear();
            countryBox.Text = "";

            List<SQLFunc.SubResults> sqlData = sql.GetData(TblData["ProjectSubType"].ToString(), "ProjectSubType", "ProjectType='" + projectTypeBox.Text + "'");
            List<string> sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                projectSubTypeBox.Items.Add(sortData[i]);
            }

            ProjectDetails.Sector = projectTypeBox.Text;

        }

        private void projectSubTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (projectSubTypeBox.Text.Equals("") || projectSubTypeBox.Text.Equals(ProjectDetails.SubSector)) return;

            countryBox.Items.Clear();
            countryBox.Text = "";

            List<SQLFunc.SubResults> sqlData = sql.GetData(TblData["Countries"].ToString(), "Country");
            List<string> sortData = Sort(sqlData);

            for (int i = 0; i < sqlData.Count; i++)
            {
                countryBox.Items.Add(sortData[i]);
            }

            ProjectDetails.SubSector = projectSubTypeBox.Text;
        }

        private void countryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (countryBox.Text.Equals("") || countryBox.Text.Equals(ProjectDetails.Country)) return;

            if (pCurChosen) inputBtn.Visible = true;
            CountryChosen = true;

            ProjectDetails.Country = countryBox.Text;
        }

        private void degradationBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                UpdateDegradation();
                degradationImpactChart.Select();
            }
        }

        private void FXModelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!IsLoading)
            {
                if (pCur == null)
                { MessageBox.Show("You need to select the payment currency"); return; }

                UpdateFX();
            }
        }

        private void climateImpactBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!IsLoading)
            {
                if (!climateImpactBox.Text.Equals("")) UpdateClimate();
                climateImpactChart.Select();
            }
        }

        private void coinsFrequencyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (coinsFrequencyBox.Text)
            {
                case "Quarterly":
                    myCoin.Frequency = Project.Frequency.Quarterly;
                    break;
                case "Semi Annually":
                    myCoin.Frequency = Project.Frequency.SemiAnnually;
                    break;
                case "Annually":
                    myCoin.Frequency = Project.Frequency.Annually;
                    break;
            }
        }

        private void coinDebtTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (coinDebtTypeBox.Text)
            {
                case "Bullet":
                    myCoin.IsBullet = true;
                    break;
                case "Term Loan":
                    myCoin.IsBullet = false;
                    break;
            }
        }

        private void SGATypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SGATypeBox.Text)
            {
                case "Total Revenues":
                    mySGA.Type = Project.SGAType.TotalRevenues;
                    break;
                case "Gross Income":
                    mySGA.Type = Project.SGAType.GrossIncome;
                    break;
            }
        }

        private void interestCoveragePeriodBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(interestCoveragePeriodBox.Text)
            {
                case "0M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._0M;
                    break;
                case "3M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._3M;
                    break;
                case "6M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._6M;
                    break;
                case "9M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._9M;
                    break;
                case "12M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._12M;
                    break;
                case "15M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._15M;
                    break;
                case "18M":
                    Const_Params.InterestCoveragePeriod = Project.InterestCoveragePeriod._18M;
                    break;
            }
        }

        #endregion

        #region Check change
        private void payInterestOnEquityDuringCPBox_CheckedChanged(object sender, EventArgs e)
        {
            if (payInterestOnEquityDuringCPBox.Checked)
            {
                DivPaidDuringCP = myCoin.RateDuringCP;
                myCoin.PayEqtyInterestDuringCP = true;
                percentageOfEquityPaidLbl.Visible = false;
                percentageOfEquityPaidBox.Visible = false;
                myCoin.PortionPayingEqty = 0.5;
            }
            else
            {
                myCoin.PayEqtyInterestDuringCP = false;
                percentageOfEquityPaidBox.Text = (myCoin.PortionPayingEqty * 100).ToString();

                DivPaidDuringCP = DivPaidDuringCP * (1 - myCoin.Conversion * ( 1 - myCoin.PortionPayingEqty));

                percentageOfEquityPaidLbl.Visible = true;
                percentageOfEquityPaidBox.Visible = true;
                percentageOfEquityPaidBox.Location = new System.Drawing.Point(PercentageOfEquityPaidBoxLoc[0] + financialTab.AutoScrollPosition.X, PercentageOfEquityPaidBoxLoc[1] + financialTab.AutoScrollPosition.Y);
            }
            
            divPaidDuringCPBox.Text = (DivPaidDuringCP * 100).ToString();
            
        }

        private void considerFinancialCostsBox_CheckedChanged(object sender, EventArgs e)
        {
            ConsiderFinancialCosts = considerFinancialCostsBox.Checked;
        }

        private void divPaidBeforeTaxesBox_CheckedChanged(object sender, EventArgs e)
        {
            ProjectDetails.DivPaidBeforeTaxes = divPaidBeforeTaxesBox.Checked;
        }

        #endregion

        #region Events
        private void inputBtn_Click(object sender, EventArgs e)
        {
            if (ConcPBox.Text.Equals("")) { return; }
            SQLFunc.SubResults data = sql.Get1Data(TblData["AuditorFee"].ToString(), "Fee", "Auditor='" + auditorBox.Text + "' AND Country='" + countryBox.Text + "' AND Sector='" + projectTypeBox.Text + "' AND SubSector='" + projectSubTypeBox.Text + "'");
            if (data._SubResults.Count == 0) { MessageBox.Show("No Auditor registered for " + projectSubTypeBox.Text + " in " + countryBox.Text + "."); return; }
            ConcP = GlobalFunc.ToDouble(ConcPBox.Text);
            ProjectDetails.AuditorFee = GlobalFunc.ToDouble(data._SubResults[0].ToString());

            auditorBox.Enabled = false;
            countryBox.Enabled = false;
            projectTypeBox.Enabled = false;
            projectSubTypeBox.Enabled = false;

            projectData.Visible = true;

        }

        private void validateCPBtn_Click(object sender, EventArgs e)
        {
            if (CPBox.Text.Equals("")) { return; }
            Const_Params.CP = Convert.ToInt32(CPBox.Text);

            Microsoft.Office.Interop.Excel.Application myXL = new Microsoft.Office.Interop.Excel.Application();
            myXL.WorkbookBeforeClose += MyXL_WorkbookBeforeClose;
            myXL.Workbooks.Add();

            Workbook myWB = myXL.Workbooks[1];
            Worksheet myWS = myWB.Worksheets[1];

            myWS.Cells[2, 2].Value = "Costs are considered paid at the beginning of the month";
            myWS.Cells[2, 2].Font.Bold = true;

            myWS.Cells[4, 3].Value = "Construction Cost";
            myWS.Cells[4, 3].Font.Bold = true;
            myWS.Cells[4, 3].Font.Size = 10;
            myWS.Cells[4, 4].Value = "Contingency Cost";
            myWS.Cells[4, 4].Font.Bold = true;
            myWS.Cells[4, 4].Font.Size = 10;
            myWS.Columns[3].ColumnWidth = 15;
            myWS.Columns[4].ColumnWidth = 15;

            for (int i = 0; i < Const_Params.CP; i++)
            {
                myWS.Cells[5 + i, 2].Value = "Month " + (i + 1);
                myWS.Cells[5 + i, 2].Font.Bold = true;
                myWS.Cells[5 + i, 2].Font.Size = 10;
                myWS.Cells[5 + i, 3].Font.Size = 9;
                myWS.Cells[5 + i, 4].Font.Size = 9;
            }

            myXL.Visible = true;

            ConstCost = new double[Const_Params.CP, 2];
        }

        private void MyXL_WorkbookBeforeClose(Workbook Wb, ref bool Cancel)
        {
            Worksheet myWS = Wb.Worksheets[1];

            try
            {
                for (int i = 0; i < Const_Params.CP; i++)
                {
                    if (myWS.Cells[5 + i, 3].Value == null) { ConstCost[i, 0] = 0; }
                    else { ConstCost[i, 0] = (double)myWS.Cells[5 + i, 3].Value; }
                    if (myWS.Cells[5 + i, 4].Value == null) { ConstCost[i, 1] = 0; }
                    else { ConstCost[i, 1] = (double)myWS.Cells[5 + i, 4].Value; }
                }

                UpdateGrid(GridType.ConstCF);
                Wb.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void setPPABtn_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application myXL1 = new Microsoft.Office.Interop.Excel.Application();
                myXL1.WorkbookBeforeClose += MyXL1_WorkbookBeforeClose;
                myXL1.Workbooks.Add();

                Workbook myWB = myXL1.Workbooks[1];
                Worksheet myWS = myWB.Worksheets[1];

                myWS.Cells[2, 2].Value = "Input PPA prices in " + referenceCurBox.Text;
                myWS.Cells[2, 2].Font.Bold = true;

                myWS.Cells[4, 3].Value = "PPA Price";
                myWS.Cells[4, 3].Font.Bold = true;
                myWS.Cells[4, 3].Font.Size = 10;
                myWS.Columns[3].ColumnWidth = 15;

                int startIndex = (int)Math.Round(Const_Params.CP / 3.0) + 1;
                int endIndex = (int)(ConcP * 4);

                PPA_Q = GlobalFunc.PPAprice(StartPrice, StartPeriod, AveragePrice, ConcP, Const_Params.CP, out PPA, ref PPAcheck);

                for (int i = startIndex; i < ConcP * 4; i++)
                {
                    myWS.Cells[5 + i - startIndex, 2].Value = "Q" + i;
                    myWS.Cells[5 + i - startIndex, 3].Value = PPA_Q[i];
                }

                myXL1.Visible = true;
            }
            catch { MessageBox.Show("Issue with the setup of PPA XL interface"); }
        }

        private void MyXL1_WorkbookBeforeClose(Workbook Wb, ref bool Cancel)
        {
            UpdatePPAGraph();
            UpdateGrid(GridType.PPA);
            Wb.Close();
        }

        private void PPAGrid_Leave(object sender, EventArgs e)
        {
            for (int i = 0; i < PPAGrid.Rows.Count; i++)
            {
                PPA[3 * i + Const_Params.CP] = GlobalFunc.ToDouble(PPAGrid.Rows[i].Cells[0].Value.ToString());
                PPA[3 * i + 1 + Const_Params.CP] = GlobalFunc.ToDouble(PPAGrid.Rows[i].Cells[0].Value.ToString());
                PPA[3 * i + 2 + Const_Params.CP] = GlobalFunc.ToDouble(PPAGrid.Rows[i].Cells[0].Value.ToString());
            }

            PPAChart.Series.Clear();
            DoGraph(PPAChart, "PPA", PPA, chart.SeriesChartType.Line, Color.Blue);
        }

        private void addLoanBtn_Click(object sender, EventArgs e)
        {
            loanGrid.Rows.Add();

            int RowNb = loanGrid.Rows.Count - 1;
            ((DataGridViewCheckBoxCell)loanGrid.Rows[RowNb].Cells[4]).Value = true;

            loanGrid.Rows[RowNb].HeaderCell.Value = "Loan " + (RowNb + 1).ToString();
            loanGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            try
            {
                DataGridViewComboBoxCell tmpCell = (DataGridViewComboBoxCell)loanGrid.Rows[RowNb].Cells[1];
                for (int j = 0; j < ConcP * 4; j++)
                {
                    tmpCell.Items.Add("Q" + (j + 1).ToString());
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void loanGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            for (int i = 0; i< loanGrid.Rows.Count; i++)
            {
                loanGrid.Rows[i].HeaderCell.Value = "Loan " + (i + 1).ToString();
                
            }

            loanGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void zoomGlobalRiskBtn_Click(object sender, EventArgs e)
        {
            chart.ChartArea myArea = globalRiskChart.ChartAreas[0];

            myArea.AxisY.Minimum = Math.Min(myArea.AxisY.Minimum + myArea.AxisY.Interval, myArea.AxisY.Maximum - myArea.AxisY.Interval);
        }

        private void dezoomGlobalRiskBtn_Click(object sender, EventArgs e)
        {
            chart.ChartArea myArea = globalRiskChart.ChartAreas[0];

            myArea.AxisY.Minimum = Math.Max(myArea.AxisY.Minimum - myArea.AxisY.Interval, 0);
        }

        private void goupGlobalRiskBtn_Click(object sender, EventArgs e)
        {
            chart.ChartArea myArea = globalRiskChart.ChartAreas[0];

            myArea.AxisY.Maximum = myArea.AxisY.Maximum + myArea.AxisY.Interval;
            myArea.AxisY.Minimum = myArea.AxisY.Minimum + myArea.AxisY.Interval;
        }

        private void godownGlobalRiskBtn_Click(object sender, EventArgs e)
        {
            chart.ChartArea myArea = globalRiskChart.ChartAreas[0];

            myArea.AxisY.Maximum = myArea.AxisY.Maximum - myArea.AxisY.Interval;
            myArea.AxisY.Minimum = myArea.AxisY.Minimum - myArea.AxisY.Interval;
        }

        private void goleftGlobalRiskBtn_Click(object sender, EventArgs e)
        {
            chart.ChartArea myArea = globalRiskChart.ChartAreas[0];

            myArea.AxisX.Maximum = myArea.AxisX.Maximum - myArea.AxisX.Interval;
            myArea.AxisX.Minimum = myArea.AxisX.Minimum - myArea.AxisX.Interval;
        }

        private void gorightGlobalRiskBtn_Click(object sender, EventArgs e)
        {
            chart.ChartArea myArea = globalRiskChart.ChartAreas[0];

            myArea.AxisX.Maximum = myArea.AxisX.Maximum + myArea.AxisX.Interval;
            myArea.AxisX.Minimum = myArea.AxisX.Minimum + myArea.AxisX.Interval;
        }

        private void MyData_Message(object sender, IMessage e)
        {
            commScreen.AppendText(e.message);
            projectData.SelectTab("outputTab");
        }

        private void MyData_CoinNominalIncrease(object sender, IIncreaseCoinNominal e)
        {
            myCoin.Nominal += e.Amount;
            this.coinNominalBox.Text = GlobalFunc.FormatText(myCoin.Nominal.ToString(), GlobalFunc.FormatType.NumberNoDec);
            myData.Increase_CoinNominal(myCoin.Nominal);
        }

        private void scheduleEquityPOBtn_Click(object sender, EventArgs e)
        {
            EquitySchedule POSchedule;

            if (EquityPO.HasSchedule) POSchedule = new EquitySchedule(EquityPO.Schedule);
            else POSchedule = new EquitySchedule(Const_Params.CP, EquityPO.Amount);
            POSchedule.ShowDialog();

            if (POSchedule.TotalEquity == 0) return;

            EquityPO.Amount = POSchedule.TotalEquity;
            equitiesFromPOBox.Text = GlobalFunc.FormatText(EquityPO.Amount.ToString(), GlobalFunc.FormatType.NumberNoDec);
            EquityPO.Schedule = POSchedule.Schedule;
            EquityPO.HasSchedule = true;

            POSchedule.Close();
        }

        private void scheduleOtherInvestEquityBtn_Click(object sender, EventArgs e)
        {
            EquitySchedule OtherSchedule;
            if (EquityOtherInvest.HasSchedule) OtherSchedule = new EquitySchedule(EquityOtherInvest.Schedule);
            else OtherSchedule = new EquitySchedule(Const_Params.CP, EquityOtherInvest.Amount);
            OtherSchedule.ShowDialog();

            if (OtherSchedule.TotalEquity == 0) return;

            EquityOtherInvest.Amount = OtherSchedule.TotalEquity;
            equitiesFromOtherInvestorsBox.Text = GlobalFunc.FormatText(EquityOtherInvest.Amount.ToString(), GlobalFunc.FormatType.NumberNoDec);
            EquityOtherInvest.Schedule = OtherSchedule.Schedule;
            EquityOtherInvest.HasSchedule = true;

            OtherSchedule.Close();
        }

        private void saveProjectMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void NewMDEProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsSaved)
            {
                DialogResult Save = MessageBox.Show("Do you want to save project before leaving?", "Last project data not saved", MessageBoxButtons.YesNo);
                if (Save == DialogResult.Yes)
                {
                    SaveProject();
                }
            }
        }

        private void MyData_ConvergenceStatus(object sender, IConvergenceStatus e)
        {
            if (e.HasConverged)
            {
                convergenceStatusLbl.Text = "Has converged";
                convergenceStatusLbl.ForeColor = Color.Green;
                lastDivPaidCoinsBox.Text = (e.LasDiv * 100).ToString();

            }
            else
            {
                convergenceStatusLbl.Text = "Not Converged";
                convergenceStatusLbl.ForeColor = Color.Red;
            }
        }

        #endregion

        private List<string> Sort(List<SQLFunc.SubResults> data, bool oneField = true)
        {
            List<string> result;
            if (data == null) { return result = new List<string>(0); }
            result = new List<string>(data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                if(oneField) { result.Add(data[i]._SubResults[0].ToString()); }
                else { result.Add(data[i]._SubResults[0].ToString() + " - " + data[i]._SubResults[1].ToString()); }
                
            }
            result.Sort();

            return result;
        }
        
        private enum GridType
        {
            ConstCF,
            PPA
        }

        private void UpdateGrid(GridType gridType)
        {
            switch(gridType)
            {
                case GridType.ConstCF:
                    if (ConstCFGrid.InvokeRequired)
                    {
                        updateConstCFGrid updateGrid = new updateConstCFGrid(UpdateGrid);

                        this.Invoke(updateGrid, new object[] { GridType.ConstCF });
                    }
                    else
                    {
                        try
                        {
                            ConstCFGrid.Rows.Clear();
                            for (int i = 0; i < Const_Params.CP; i++)
                            {
                                string[] Data = new string[] { ConstCost[i, 0].ToString(), ConstCost[i, 1].ToString() };

                                ConstCFGrid.RowHeadersWidth = 90;
                                ConstCFGrid.Rows.Add(Data);
                                ConstCFGrid.Rows[i].HeaderCell.Value = "Month " + (i + 1);
                            }

                            ConstCFGrid.Visible = true;
                            capexIncreaseLbl.Visible = true;
                            capexIncreaseBox.Visible = true;
                            coinIssuanceCostBox.Visible = true;
                            coinIssuanceCostLbl.Visible = true;
                            workingCapitalCPLbl.Visible = true;
                            workingCapitalCPBox.Visible = true;
                            interestCoverageBox.Visible = true;
                            interestCoverageLbl.Visible = true;
                            delayLbl.Visible = true;
                            delayBox.Visible = true;
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }
                    }
                    break;
                case GridType.PPA:
                    if (PPAGrid.InvokeRequired)
                    {
                        updateConstCFGrid updateGrid = new updateConstCFGrid(UpdateGrid);

                        this.Invoke(updateGrid, new object[] { GridType.PPA });
                    }
                    else
                    {
                        try
                        {
                            PPAGrid.Rows.Clear();
                            int startIndex = (int)(Const_Params.CP / 3);

                            for (int i = startIndex; i < ConcP * 4; i++)
                            {
                                string[] Data = new string[] { PPA_Q[i].ToString() };

                                PPAGrid.RowHeadersWidth = 90;
                                PPAGrid.Rows.Add(Data);
                                PPAGrid.Rows[i - startIndex].HeaderCell.Value = "Q" + (i + 1);
                            }
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message);
                        }
                    }
                    break;
            }
           
        }
        
        private void UpdatePPAGraph()
        {
            try
            {
                if(this.InvokeRequired)
                {
                    updatePPAChart UpdatePPA = new updatePPAChart(UpdatePPAGraph);
                    this.Invoke(UpdatePPA);
                }
                else
                {
                    PPA_Q = GlobalFunc.PPAprice(StartPrice, StartPeriod, AveragePrice, ConcP, Const_Params.CP, out PPA, ref PPAcheck);
                    UpdateGrid(GridType.PPA);

                    if (PPAcheck != 0)
                    {
                        PPAChart.Series.Clear();
                        DoGraph(PPAChart, "PPA", PPA, chart.SeriesChartType.Line, Color.Blue);
                    }
                }
            }
            catch (Exception Ex)
            { MessageBox.Show(Ex.Message); }
        }

        private void UpdateFX()
        {
            if (FXImpact != null)
            {
                for (int i = 0; i < GlobalRisk.Length; i++)
                {
                    GlobalRisk[i] /= FXImpact[i];
                }
            }

            FXImpact = Risk.Risk.FXRisk(sql, TblData, iCur, pCur, StartDate, EndDate, FXModelBox.Text);

            for (int i = 0; i < GlobalRisk.Length; i++)
            {
                GlobalRisk[i] = Math.Round(GlobalRisk[i] * FXImpact[i], 6);
            }

            fXImpactChart.Series.Clear();
            DoGraph(fXImpactChart, "FX Impact", FXImpact, chart.SeriesChartType.Line, Color.Blue);
            globalRiskChart.Series.Clear();
            DoGraph(globalRiskChart, "Global Risk", GlobalRisk, chart.SeriesChartType.Line, Color.Red);

            fXImpactChart.Select();
        }

        private void UpdateDegradation()
        {
            if (DegradationImpact != null)
            {
                for (int i = 0; i < GlobalRisk.Length; i++)
                {
                    if (DegradationImpact[i] != 0) GlobalRisk[i] /= DegradationImpact[i];
                }
            }

            DegradationImpact = Risk.Risk.DegradationRisk(sql, TblData, COD, StartDate, EndDate, projectTypeBox.Text, projectSubTypeBox.Text, countryBox.Text, degradationBox.Text, PowerProductionIncrease, out ParamDegradation);

            for (int i = 0; i < GlobalRisk.Length; i++)
            {
                GlobalRisk[i] = Math.Round(GlobalRisk[i] * DegradationImpact[i], 6);
            }

            changeDegradationBtn.Visible = true;
            changeDegradationBtn.Location = new System.Drawing.Point(ChangeDegradBtnLoc[0] - energyProdTab.HorizontalScroll.Value, ChangeDegradBtnLoc[1] - energyProdTab.VerticalScroll.Value);

            degradationImpactChart.Series.Clear();
            DoGraph(degradationImpactChart, "Degradation", DegradationImpact, chart.SeriesChartType.Line, Color.Blue);
            globalRiskChart.Series.Clear();
            DoGraph(globalRiskChart, "Global Risk", GlobalRisk, chart.SeriesChartType.Line, Color.Red);
        }

        private void UpdateClimate()
        {
            if (ClimateImpact != null)
            {
                for (int i = 0; i < GlobalRisk.Length; i++)
                {
                    GlobalRisk[i] /= ClimateImpact[i];
                }
            }

            ClimateImpact = Risk.Risk.ClimateImpactRisk(sql, TblData, COD, StartDate, EndDate, projectTypeBox.Text, projectSubTypeBox.Text, countryBox.Text, climateImpactBox.Text, out ParamClimateImpact);

            for (int i = 0; i < GlobalRisk.Length; i++)
            {
                GlobalRisk[i] = Math.Round(GlobalRisk[i] * ClimateImpact[i], 6);
            }

            changeClimateImpactBtn.Visible = true;
            changeClimateImpactBtn.Location = new System.Drawing.Point(ChangeClimBtnLoc[0] - energyProdTab.HorizontalScroll.Value, ChangeClimBtnLoc[1] - energyProdTab.VerticalScroll.Value);

            climateImpactChart.Series.Clear();
            DoGraph(climateImpactChart, "Climate", ClimateImpact, chart.SeriesChartType.Line, Color.Blue);
            globalRiskChart.Series.Clear();
            DoGraph(globalRiskChart, "Global Risk", GlobalRisk, chart.SeriesChartType.Line, Color.Red);
        }

        private void UpdateLoanGrid()
        {
            for(int i = 0; i< loanGrid.Rows.Count; i++)
            {
                DataGridViewComboBoxCell tmpCell = (DataGridViewComboBoxCell)loanGrid.Rows[i].Cells[1];
                tmpCell.Items.Clear();

                for (int j = 0; j < ConcP * 4; j++)
                {
                    tmpCell.Items.Add("Q" + (j + 1).ToString());
                }
            }
        }

        private void UpdateOMGrid()
        {
            if (OMGrid.Rows.Count == 1)
            {
                string[] tmpData = new string[3] { ((int)(Const_Params.CP / 12.0) + 1).ToString(), ConcP.ToString(), "0" };
                OMGrid.Rows.Add(tmpData);
                OM = new double[3, 1];
                OM[0, 0] = Const_Params.CP;
                OM[1, 0] = ConcP * 12;
                OM[2, 0] = 0.0;
            }
            else
            {
                OMGrid.Rows[OMGrid.Rows.Count - 2].Cells[0].Value = ((Const_Params.CP / 12.0) + 1).ToString();
                OMGrid.Rows[OMGrid.Rows.Count - 2].Cells[1].Value = ConcP.ToString();
            }
        }

        private void DoGraph(chart.Chart _chart, string SerieName, double[] data, chart.SeriesChartType style, System.Drawing.Color color)
        {
            try
            {
                double Max = data[0];
                double Min = data[0];

                _chart.Series.Add(SerieName);

                _chart.Series[0].ChartType = style;
                _chart.Series[0].Color = color;

                for (int i = 0; i < data.Length; i++)
                {
                    double newData = data[i];
                    if (newData < Min) Min = newData;
                    if (newData > Max) Max = newData;
                    _chart.Series[0].Points.AddY(newData);
                }

                chart.ChartArea myArea = _chart.ChartAreas[0];

                myArea.AxisY.Interval = Math.Round((Max - 0.95 * Min) / 5, 2);
                myArea.AxisY.MajorTickMark.Interval = myArea.AxisY.Interval;
                myArea.AxisY.Minimum = Math.Round(0.95 * Min, 2);
                myArea.AxisY.Maximum = Max;
                myArea.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8f);
                myArea.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8f);
                if(data.Length > 180) myArea.AxisX.Interval = 24.0;
                else myArea.AxisX.Interval = 12.0;
                myArea.AxisX.Minimum = 0;
                myArea.AxisX.Maximum = data.Length;
                myArea.Position = new chart.ElementPosition(0, 5, 95, 95);// _chart.Size.Width - 145, _chart.Size.Height - 55);

                foreach(chart.Legend legend in _chart.Legends)
                {
                    legend.Position = new chart.ElementPosition(legend.Position.X, 60, legend.Position.Width, legend.Position.Height);
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            
        }
        
        private void changeDegradationBtn_Click(object sender, EventArgs e)
        {
            RiskMgt EditSpecialParam = new RiskMgt(projectTypeBox.Text, projectSubTypeBox.Text, countryBox.Text, ProjectID, RiskMgt.ModelCategory.Degradation, degradationBox.Text, sql, TblData);
            EditSpecialParam.ShowDialog();

            UpdateDegradation();
        }

        private void changeClimateImpactBtn_Click(object sender, EventArgs e)
        {
            RiskMgt EditSpecialParam = new RiskMgt(projectTypeBox.Text, projectSubTypeBox.Text, countryBox.Text, ProjectID, RiskMgt.ModelCategory.Climate, climateImpactBox.Text, sql, TblData);
            EditSpecialParam.ShowDialog();

            UpdateClimate();
        }

        private void launchDivCalculationBtn_Click(object sender, EventArgs e)
        {
            IsSaved = false;

            List<SQLFunc.SubResults> dbData = sql.GetData(TblData["DepositRates"].ToString(), "Value", "pCur='" + ProjectDetails.RefCurrency + "' AND [Date]=(Select Max(Date) from " + TblData["DepositRates"].ToString() + ")");
            if (dbData.Count == 0)
            {
                commScreen.AppendText("\nNo Deposit Rates for " + ProjectDetails.RefCurrency + ".\n");
                projectData.SelectTab("outputTab");
                return;
            }

            projectCFGrid.Rows.Clear();
            projectCFGrid.Columns.Clear();
            CMCCFGrid.Rows.Clear();
            CMCCFGrid.Columns.Clear();
            dividendGrid.Rows.Clear();
            dividendGrid.Columns.Clear();

            switch (GlobalFunc.DoBasicChecks(ConcP, Const_Params.CP, Const_Params.Delay, myCoin, EquityOtherInvest))
            {
                case GlobalFunc.BasicErrors.CoinGP_Bigger_Tenor:
                    commScreen.AppendText("Issue with coin tenor");
                    projectData.SelectTab("outputTab");
                    return;
                case GlobalFunc.BasicErrors.ConstructionPeriodAndDelay_Bigger_ConcP:
                    commScreen.AppendText("Issue with Concession Period: smaller than Construction Period and 6 Months delay");
                    projectData.SelectTab("outputTab");
                    return;
                case GlobalFunc.BasicErrors.ConstructionPeriod_Bigger_ConcP:
                    commScreen.AppendText("Issue with Concession Period: smaller than Construction Period");
                    projectData.SelectTab("outputTab");
                    return;
                case GlobalFunc.BasicErrors.ConstructionPeriod_Smaller_Delay:
                    commScreen.AppendText("Issue with Construction Period: smaller than delay");
                    projectData.SelectTab("outputTab");
                    return;
                case GlobalFunc.BasicErrors.OtherInvestorEquity_Bigger_CoinInterest:
                    DialogResult Continue = MessageBox.Show("Interests on Equity from other investors are bigger than interests on the coins", "Continue process", MessageBoxButtons.OKCancel);
                    if (Continue == DialogResult.Cancel) return;
                    break;
            }

            myCoin.Nominal -= TotalCPInterests;

            myData = new Project(ProjectDetails, ConcP, Const_Params, ConstCost, EnergyProduction, PPA, GlobalRisk, EquityPO, EquityOtherInvest,
                myLoans, myCoin, OM, mySGA, Royalties, Taxes, Params, ConsiderFinancialCosts, Project.CoinType.DebtEquity);

            myData.Message += MyData_Message;
            myData.CoinNominalIncrease += MyData_CoinNominalIncrease;
            myData.ConvergenceStatus += MyData_ConvergenceStatus;

            //divPaidDuringCPBox.Text = (myData.GetFirstDiv() * 100).ToString();

            double PreviousNominal = myCoin.Nominal;
            bool HasConverged = false;
            int Counter = 0;

            while(Counter < 30 && !HasConverged)
            {
                myData.ComputeCF_Pricing();

                if (TotalCPInterests == -1.0)
                {
                    TotalCPInterests = myData.TotalCPInterests;
                    PreviousNominal = myData.Coins.Nominal;
                    myData.Coins.Nominal -= TotalCPInterests;
                }
                else if (myData.TotalCPInterests > TotalCPInterests)
                {
                    myData.Coins.Nominal = PreviousNominal - TotalCPInterests;
                    myData.ComputeCF_Pricing();
                    TotalCPInterests = myData.TotalCPInterests;
                    HasConverged = true;
                }
                else
                {
                    TotalCPInterests = myData.TotalCPInterests;
                    PreviousNominal = myData.Coins.Nominal;
                    myData.Coins.Nominal -= TotalCPInterests;
                }

                Counter++;
            }

            myData.FirstDiv = Math.Round(Convert.ToDouble(divPaidDuringCPBox.Text), 2) / 100;

            UpdateEquityPortionCoin(myData.EquityPortionCoin);

            myData.ComputeDepositAndTotalIncomeCMC(GlobalFunc.DBDepositRates2Vector(dbData), DepositRateDecrease, myData.IncomeCMC_FromProject, StartDate, COD, StartDate, CoinParams.FirstPayment, CoinParams.DivPaymentFrequency, CoinParams.MaxDepositTenor);

            //divPaidDuringCPBox.Text = Math.Round(myData.FirstDiv * 100, 2).ToString();
            
            avgDivLbl.Text = (myData.AverageDiv * 100).ToString() + "%";

            TotalCPInterests = myData.TotalCPInterests;

            try
            {
                WriteProjectData(myData);
                projectData.SelectTab("dividendsTab");
            }
            catch { }
        }

        private void UpdateEquityPortionCoin(double EquityPortionCoin)
        {
            equityPortionCoinLbl.Text = (EquityPortionCoin * 100).ToString() + "%";
            ProjectDetails.EquityPortion = EquityPortionCoin;

            SQLFunc.SubResults data = sql.Get1Data(TblData["Projects"].ToString(), "ProjectName", "Country='" + ProjectDetails.Country + "' AND Sector='" + ProjectDetails.Sector + "' AND SubSector='" + ProjectDetails.SubSector + "' AND ProjectID='" + ProjectID + "'");
            if(data._SubResults.Count != 0) sql.UpdateTable(TblData["Projects"].ToString(), "OtherDetails='" + XMLFunctions.Create_OtherDetailsXML(ProjectDetails) + "'", "Country='" + ProjectDetails.Country + "' AND Sector='" + ProjectDetails.Sector + "' AND SubSector='" + ProjectDetails.SubSector + "' AND ProjectID='" + ProjectID + "'");
        }

        private void WriteProjectData(Project myData)
        {
            int AnalyisPeriod = GetMinPeriod4Interests(myData);
            string HeaderTxt = "Y";

            switch(AnalyisPeriod)
            {
                case 3:
                    HeaderTxt = "Q";
                    break;
                case 6:
                    HeaderTxt = "S";
                    break;
            }

            for(int i = 1; i <= myData.ConcP * 12 / AnalyisPeriod; i++)
            {
                string ColName = "Col" + i;
                projectCFGrid.Columns.Add(ColName, HeaderTxt + i.ToString());
                CMCCFGrid.Columns.Add(ColName, HeaderTxt + i.ToString());
            }


            projectCFGrid.Rows.Add(ConvertArray(myData.CashForConstruction, AnalyisPeriod));
            projectCFGrid.Rows[0].HeaderCell.Value = "Construction CF";

            projectCFGrid.Rows.Add(ConvertArray(myData.Withdrawals.EquityPO, AnalyisPeriod));
            projectCFGrid.Rows[1].HeaderCell.Value = "Equity PO Withdrawal";
            projectCFGrid.Rows.Add(ConvertArray(myData.Withdrawals.OtherEquity, AnalyisPeriod));
            projectCFGrid.Rows[2].HeaderCell.Value = "Other Invest Equity Withdrawal";

            projectCFGrid.Rows.Add(ConvertArray(myData.Withdrawals.Coins, AnalyisPeriod));
            projectCFGrid.Rows[3].HeaderCell.Value = "Coins Withdrawal";

            int NbLoans = myData.Withdrawals.Loans.Count;

            for (int i = 0; i < NbLoans; i++)
            {
                projectCFGrid.Rows.Add(ConvertArray(myData.Withdrawals.Loans[i], AnalyisPeriod));
                projectCFGrid.Rows[4 + i].HeaderCell.Value = "Loan " + (i + 1).ToString() + " Withdrawal";
            }

            projectCFGrid.Rows.Add(ConvertArray(myData.Withdrawals.TotalWithdrawal, AnalyisPeriod));
            projectCFGrid.Rows[4 + NbLoans].HeaderCell.Value = "Total Withdrawals";


            projectCFGrid.Rows.Add(ConvertArray(myData.Loans_OustandingNominals, AnalyisPeriod));
            projectCFGrid.Rows[5 + NbLoans].HeaderCell.Value = "Loans Outstanding Nominal";

            projectCFGrid.Rows.Add(ConvertArray(myData.InterestsFromProject, AnalyisPeriod));
            projectCFGrid.Rows[6 + NbLoans].HeaderCell.Value = "Interests";

            projectCFGrid.Rows.Add(ConvertArray(myData.Revenues, AnalyisPeriod));
            projectCFGrid.Rows[7 + NbLoans].HeaderCell.Value = "Revenues";
            projectCFGrid.Rows.Add(ConvertArray(myData.GrossIncomes, AnalyisPeriod));
            projectCFGrid.Rows[8 + NbLoans].HeaderCell.Value = "Gross Income";
            projectCFGrid.Rows.Add(ConvertArray(myData.AuditorCost, AnalyisPeriod));
            projectCFGrid.Rows[9 + NbLoans].HeaderCell.Value = "Auditor Fee";
            projectCFGrid.Rows.Add(ConvertArray(myData.NetIncomes, AnalyisPeriod));
            projectCFGrid.Rows[10 + NbLoans].HeaderCell.Value = "Net Income";
            projectCFGrid.Rows.Add(ConvertArray(myData.FinancialCosts, AnalyisPeriod));
            projectCFGrid.Rows[11 + NbLoans].HeaderCell.Value = "Financing Costs";

            projectCFGrid.Rows.Add(ConvertArray(myData.FreeCashFlowsBeforeTaxe, AnalyisPeriod));
            projectCFGrid.Rows[12 + NbLoans].HeaderCell.Value = "FCF Before Taxe";
            projectCFGrid.Rows.Add(ConvertArray(myData.Royalties, AnalyisPeriod));
            projectCFGrid.Rows[13 + NbLoans].HeaderCell.Value = "Royalties";
            projectCFGrid.Rows.Add(ConvertArray(myData.Taxes, AnalyisPeriod));
            projectCFGrid.Rows[14 + NbLoans].HeaderCell.Value = "Taxes";
            projectCFGrid.Rows.Add(ConvertArray(myData.FreeCashFlows, AnalyisPeriod));
            projectCFGrid.Rows[15 + NbLoans].HeaderCell.Value = "FCF";

            projectCFGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            CMCCFGrid.Rows.Add(ConvertArray(myData.CoinOutstandingBalance, AnalyisPeriod, true));
            CMCCFGrid.Rows[0].HeaderCell.Value = "Coin Outstanding Balance";

            for(int i = 0; i < myData.CoinDeposit.Count; i++)
            {
                CMCCFGrid.Rows.Add(ConvertArray(myData.CoinDeposit[i], AnalyisPeriod));
                if(i == 0) CMCCFGrid.Rows[1 + i].HeaderCell.Value = "Deposit 1M";
                else CMCCFGrid.Rows[1 + i].HeaderCell.Value = "Deposit " + i * 3 + "M";
            }

            CMCCFGrid.Rows.Add(ConvertArray(myData.InterestOnCoinDeposit, AnalyisPeriod));
            CMCCFGrid.Rows[myData.CoinDeposit.Count + 1].HeaderCell.Value = "Interest on Deposit";

            CMCCFGrid.Rows.Add(ConvertArray(myData.IncomeCMC_Total, AnalyisPeriod));
            CMCCFGrid.Rows[myData.CoinDeposit.Count + 2].HeaderCell.Value = "Incomes";


            CMCCFGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            double[] DivPercentage = new double[myData.Dividends.Length];

            for(int i = 0; i < myData.Dividends.Length; i++)
            {
                dividendGrid.Columns.Add("div" + i.ToString(), "Dividend 1");
                DivPercentage[i] = Math.Round(myData.Dividends[i] / myData.Coins.Nominal, 4);
            }

            dividendGrid.Rows.Add(ConvertArray(myData.Dividends, 1));
            dividendGrid.Rows.Add(ConvertArray(DivPercentage, 1, false, true));

            dividendChart.Series.Clear();
            if(myData.Dividends[0] != 0) DoGraph(dividendChart, "Dividends", myData.Dividends, System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line, Color.Green);
        }

        private string[] ConvertArray(double[] input, int AnalysisPeriod, bool IsAccu = false, bool IsPerentage = false)
        {
            string[] result = new string[input.Length / AnalysisPeriod];

            switch (AnalysisPeriod)
            {
                case 3:
                    for(int i = 0; i < result.Length; i++)
                    {
                        if (IsAccu) result[i] = GlobalFunc.FormatText(input[3 * i + 2].ToString(), GlobalFunc.FormatType.NumberNoDec);
                        else result[i] = GlobalFunc.FormatText((input[3 * i] + input[3 * i + 1] + input[3 * i + 2]).ToString(), GlobalFunc.FormatType.NumberNoDec);
                    }
                    return result;
                case 6:
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (IsAccu) result[i] = GlobalFunc.FormatText(input[6 * i + 5].ToString(), GlobalFunc.FormatType.NumberNoDec);
                        else result[i] = GlobalFunc.FormatText((input[6 * i] + input[6 * i + 1] + input[6 * i + 2] +
                            input[6 * i + 3] + input[6 * i + 4] + input[6 * i + 5]).ToString(), GlobalFunc.FormatType.NumberNoDec);
                    }
                    return result;
                case 12:
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (IsAccu) result[i] = GlobalFunc.FormatText(input[12 * i + 11].ToString(), GlobalFunc.FormatType.NumberNoDec);
                        else result[i] = GlobalFunc.FormatText((input[12 * i] + input[12 * i + 1] + input[12 * i + 2] + input[12 * i + 3] +
                            input[12 * i + 4] + input[12 * i + 5] + input[12 * i + 6] + input[12 * i + 7] +
                            input[12 * i + 8] + input[12 * i + 9] + input[12 * i + 10] + input[12 * i + 11]).ToString(), GlobalFunc.FormatType.NumberNoDec);
                    }
                    return result;
                case 1:
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (IsPerentage)
                        {
                            result[i] = (input[i] * 100).ToString() + "%";
                        }
                        else
                        {
                            result[i] = GlobalFunc.FormatText(input[i].ToString(), GlobalFunc.FormatType.NumberNoDec);
                        }
                    }
                    return result;
            }

            return result;
        }

        private int GetMinPeriod4Interests(Project myData)
        {
            int result = 12;

            for(int i = 0; i < myData.Loans.Count; i++)
            {
                if (myData.Loans[i].Frequency == Project.Frequency.Quarterly) result = Math.Min(result, 3);
                if (myData.Loans[i].Frequency == Project.Frequency.SemiAnnually) result = Math.Min(result, 6);
            }

            if (myData.Coins.Frequency == Project.Frequency.Quarterly) result = Math.Min(result, 3);
            if(myData.Coins.Frequency == Project.Frequency.SemiAnnually) result = Math.Min(result, 6);

            return result;
        }

        private void SaveProject()
        {
            SQLFunc.SubResults IsExisting = sql.Get1Data(TblData["Projects"].ToString(), "*", "ProjectName='" + ProjectDetails.ProjectName + "' AND Country='" + ProjectDetails.Country + "' AND Sector='" + ProjectDetails.Sector + "' AND SubSector='" + ProjectDetails.SubSector + "'");
            if (IsExisting._SubResults.Count == 0)
            {
                string SavedData = "";

                if (ProjectDetails.ProjectName == null || ProjectDetails.ProjectName.Equals("")) { SavingProcessAborted("Project Name missing"); return; }
                SavedData = "'" + ProjectDetails.ProjectName + "'";
                SavedData += ",'" + ProjectDetails.Country + "','" + ProjectDetails.Sector + "','" + ProjectDetails.SubSector + "','" + ProjectDetails.RefCurrency + "'";
                if (ProjectDetails.PPACurrency.Equals("")) { SavingProcessAborted("PPA Currency missing"); return; }
                SavedData += ",'" + ProjectDetails.PPACurrency + "'";
                SavedData += ",'" + XMLFunctions.Create_OtherDetailsXML(ProjectDetails) + "'";
                if (ConcP == 0) { SavingProcessAborted("Concession Period is equal to 0"); return; }
                SavedData += ",'" + ConcP.ToString() + "'";
                if (ConstCFGrid.Rows.Count != Const_Params.CP) { SavingProcessAborted("Issue with Construction Cash Flows not equal to Construction Period"); return; }
                SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(ConstCost) + "'";
                SavedData += ",'" + XMLFunctions.Create_ConstParamXML(Const_Params) + "'";
                SavedData += ",'" + XMLFunctions.Create_EnergyProductionXML(Power, PlantFactor, EnergyProduction) + "'";
                if (PPA.Length == 0) { SavingProcessAborted("PPA information missing"); return; }

                try
                {
                    SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(PPA) + "'";
                    SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(GlobalRisk) + "'";
                    SavedData += ",'" + XMLFunctions.Create_EquityXML(EquityPO) + "'";
                    SavedData += ",'" + XMLFunctions.Create_EquityXML(EquityOtherInvest) + "'";
                    SavedData += ",'" + XMLFunctions.Create_LoanXML(myLoans) + "'";
                    SavedData += ",'" + XMLFunctions.Create_CoinXML(myCoin) + "'";
                    SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(OM) + "'";
                    SavedData += ",'" + XMLFunctions.Create_SGAXML(mySGA) + "'";
                    SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(Royalties) + "'";
                    SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(Taxes) + "'";
                    SavedData += ",'" + XMLFunctions.Create_XMLFromMatrix(PowerProductionIncrease) + "'";
                    if (degradationBox.Text.Equals("")) { SavingProcessAborted("Degradation model missing"); return; }
                    SavedData += ",'" + XMLFunctions.Create_ModelParamXML(degradationBox.Text, ParamDegradation) + "'";
                    if (climateImpactBox.Text.Equals("")) { SavingProcessAborted("Climate model missing"); return; }
                    SavedData += ",'" + XMLFunctions.Create_ModelParamXML(climateImpactBox.Text, ParamClimateImpact) + "'";
                    if (FXModelBox.Text.Equals("")) { SavingProcessAborted("FX model missing"); return; }
                    SavedData += ",'" + XMLFunctions.Create_ParametersXML(FXModelBox.Text, Params) + "'";
                    SavedData += ",'" + ProjectID.ToString() + "'";
                }
                catch
                {
                    SavingProcessAborted("Issue while creating XMLs");
                }

                sql.InsertData(TblData["Projects"].ToString(), SavedData);
            }
            else
            {
                string SavedData = "pCur='" + ProjectDetails.RefCurrency + "',iCur='" + ProjectDetails.PPACurrency + "',";
                SavedData += "OtherDetails='" + XMLFunctions.Create_OtherDetailsXML(ProjectDetails) + "'";
                SavedData += ",ConcP='" + ConcP.ToString() + "'";
                SavedData += ",ConstCost='" + XMLFunctions.Create_XMLFromMatrix(ConstCost) + "'";
                SavedData += ",ConstParams='" + XMLFunctions.Create_ConstParamXML(Const_Params) + "'";
                SavedData += ",EnergyProduction='" + XMLFunctions.Create_EnergyProductionXML(Power, PlantFactor, EnergyProduction) + "'";
                SavedData += ",PPA='" + XMLFunctions.Create_XMLFromMatrix(PPA) + "'";
                SavedData += ",GlobalRisk='" + XMLFunctions.Create_XMLFromMatrix(GlobalRisk) + "'";
                SavedData += ",EquityPO='" + XMLFunctions.Create_EquityXML(EquityPO) + "'";
                SavedData += ",EquityOtherInvest='" + XMLFunctions.Create_EquityXML(EquityOtherInvest) + "'";
                SavedData += ",Loans='" + XMLFunctions.Create_LoanXML(myLoans) + "'";
                SavedData += ",Coin='" + XMLFunctions.Create_CoinXML(myCoin) + "'";
                SavedData += ",OM='" + XMLFunctions.Create_XMLFromMatrix(OM) + "'";
                SavedData += ",SGA='" + XMLFunctions.Create_SGAXML(mySGA) + "'";
                SavedData += ",Royalties='" + XMLFunctions.Create_XMLFromMatrix(Royalties) + "'";
                SavedData += ",Taxes='" + XMLFunctions.Create_XMLFromMatrix(Taxes) + "'";
                SavedData += ",PowerIncrease='" + XMLFunctions.Create_XMLFromMatrix(PowerProductionIncrease) + "'";
                SavedData += ",DegradationRisk='" + XMLFunctions.Create_ModelParamXML(degradationBox.Text, ParamDegradation) + "'";
                SavedData += ",ClimateRisk='" + XMLFunctions.Create_ModelParamXML(climateImpactBox.Text, ParamClimateImpact) + "'";
                SavedData += ",ModelParams='" + XMLFunctions.Create_ParametersXML(FXModelBox.Text, Params) + "'";

                sql.UpdateTable(TblData["Projects"].ToString(), SavedData, "ProjectID='" + ProjectID.ToString() + "'");
            }

            IsSaved = true;
        }

        private void SavingProcessAborted(string message)
        {
            commScreen.AppendText("Saving process aborted: " + message);
            projectData.SelectTab("outputTab");
        }

        private void LoadingProcessAborted(string message)
        {
            commScreen.AppendText("Loading process aborted: " + message);
            projectData.SelectTab("outputTab");
        }

        private void parametersMenuItem_Click(object sender, EventArgs e)
        {
            ProjectParam myParams = new ProjectParam(Params);
            myParams.ShowDialog();

            if(myParams != null)
            {
                Params = myParams.newParams;
            }

            myParams.Close();
        }

        private void portionCash4CMCBox_Leave(object sender, EventArgs e)
        {
            ProjectDetails.PortionCash4CMC = GlobalFunc.ToDouble(portionCash4CMCBox.Text) / 100;
        }

        private void lastDivPaidCoinsBox_Leave(object sender, EventArgs e)
        {
            myCoin.LastDividend = GlobalFunc.ToDouble(lastDivPaidCoinsBox.Text) / 100;

            if(myCoin.LastDividend > 1) { myCoin.LastDividend = 1.0; lastDivPaidCoinsBox.Text = "100"; }
            if(myCoin.LastDividend < 0) { myCoin.LastDividend = 0.0; lastDivPaidCoinsBox.Text = "0"; }
        }

        private void minDivIncreaseBox_Leave(object sender, EventArgs e)
        {
            myCoin.MinDivIncrease = GlobalFunc.ToDouble(minDivIncreaseBox.Text) / 100;

            if (myCoin.MinDivIncrease > 1) { myCoin.MinDivIncrease = 1.0; minDivIncreaseBox.Text = "100"; }
            if (myCoin.MinDivIncrease < 0) { myCoin.MinDivIncrease = 0.0; minDivIncreaseBox.Text = "0"; }
        }

        private void calculateExpectedDivBtn_Click(object sender, EventArgs e)
        {
            CalculateRateAfterCP ExpectedRate = new CalculateRateAfterCP();
            ExpectedRate.ShowDialog();
        }
    }
}
