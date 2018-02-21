using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using Valuation;


namespace ProjectLib
{
    [Guid("f38681b5-2134-443f-9064-5be309ece4ce"), ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(IProjectEvents))]
    public class Project : IProject, IProjectEvents
    {
        //Events
        public event EventHandler<IMessage> Message;
        public event EventHandler<IIncreaseCoinNominal> CoinNominalIncrease;
        public event EventHandler<IConvergenceStatus> ConvergenceStatus;

        //Other Params
        private bool IsPricing;
        private int MaxIndex;
        private int LoopAvoider1 = -101;
        private double InterestCPDelay = 0.0;
        private double InterestCoverage = 0.0;
        private double tmpInterests = 0.0;
        private double ProjectSize = 0.0;
        private double ConstructionCostNetInterest = 0.0;
        private double PrevCoinNominal = 0.0;
        private bool ConsiderFinancialCosts;
        private int AdjCoin = 0;
        private CoinType coinType;

        //Inputs
        public double ConcP;
        private ConstructionParams Const_Params;
        private double[,] Const_CF;
        private double MaxEnergyProduction;
        private double[] PPA;
        private double[] GlobalRisk;
        private EquityData POEquity;
        private EquityData OtherInvestEquity;
        public List<Loan> Loans;
        public Coin Coins;
        private double[,] OM;
        private SGA SGAs;
        private double[,] RoyaltiesRate;
        private double[,] TaxeRate;
        private ProjectDetail ProjectDetails;
        private DateTime StartDate;
        private DateTime COD;
        private DateTime Today;
        private double[] poEquityWithD;
        private double[] otherInvestEquityWithD;
        private double[] CoinWithD;
        private List<double[]> LoanWithD;
        public double FirstDiv;

        //Outputs
        public double[] Revenues;
        public double[] GrossIncomes;
        public double[] NetIncomes;
        public double[] CashForConstruction;
        public double[] Loans_OustandingNominals;
        public double[] InterestsFromProject;
        public double[] AuditorCost;
        public double[] FinancialCosts;
        public double[] FreeCashFlowsBeforeTaxe;
        public double[] FreeCashFlows;
        public double[] Royalties;
        public double[] Taxes;
        public double[] IncomeCMC_FromProject;
        public double[] CoinOutstandingBalance;
        public Hashtable Params = new Hashtable();
        public double TotalCPInterests;
        public Withdrawal Withdrawals = new Withdrawal();
        public List<double[]> CoinDeposit;
        public double[] InterestOnCoinDeposit;
        public double[] IncomeCMC_Total;
        public double[] IncomeCMC_TotalFormatted;
        public double EquityPortionCoin = 0.0;
        public double[] Dividends;
        public double AverageDiv = 0.0;

        //Risk outputs
        public double[] IncomeCMC_TotalFormatted_GlobalRiskIncrease;


        /// <summary>
        /// Use this function for pricing purpose only - no date are considered
        /// </summary>
        /// <param name="ProjectDetails"></param>
        /// <param name="ConcP"></param>
        /// <param name="Const_Params"></param>
        /// <param name="Const_CF"></param>
        /// <param name="MaxEnergyProduction"></param>
        /// <param name="PPA"></param>
        /// <param name="GlobalRisk"></param>
        /// <param name="POEquity"></param>
        /// <param name="OtherInvestEquity"></param>
        /// <param name="Loans"></param>
        /// <param name="Coins"></param>
        /// <param name="OM"></param>
        /// <param name="SGAs"></param>
        /// <param name="RoyaltiesRate"></param>
        /// <param name="TaxeRate"></param>
        /// <param name="Params"></param>
        /// <param name="ConsiderFinancialCosts"></param>
        /// <param name="coinType"></param>
        public Project(ProjectDetail ProjectDetails, double ConcP, ConstructionParams Const_Params, double[,] Const_CF, double MaxEnergyProduction, double[] PPA, double[] GlobalRisk,
                EquityData POEquity, EquityData OtherInvestEquity, List<Loan> Loans, Coin Coins, double[,] OM, SGA SGAs, double[,] RoyaltiesRate, double[,] TaxeRate, Hashtable Params, bool ConsiderFinancialCosts, CoinType coinType)
        {
            this.ProjectDetails = ProjectDetails;
            this.ConcP = ConcP;
            this.Const_Params = Const_Params;
            this.Const_CF = Const_CF;
            this.MaxEnergyProduction = MaxEnergyProduction;
            this.PPA = PPA;
            this.GlobalRisk = GlobalRisk;
            this.POEquity = POEquity;
            this.OtherInvestEquity = OtherInvestEquity;
            this.Loans = Loans;
            this.Coins = Coins;
            this.OM = OM;
            this.SGAs = SGAs;
            this.RoyaltiesRate = RoyaltiesRate;
            this.TaxeRate = TaxeRate;
            this.ConsiderFinancialCosts = ConsiderFinancialCosts;
            this.Params = Params;
            this.coinType = coinType;

            this.IsPricing = true;
        }

        /// <summary>
        /// Use this function for the real time pricing, once project is launched
        /// </summary>
        /// <param name="StartDate">Construction start date</param>
        /// <param name="Today"></param>
        /// <param name="COD">Operation date of the project, either forecast or real COD date</param>
        /// <param name="ProjectSize">As initially planned; to pay auditors</param>
        /// <param name="ProjectDetails"></param>
        /// <param name="ConcP"></param>
        /// <param name="Const_Params"></param>
        /// <param name="newConst_CF">Updated construction CF</param>
        /// <param name="newMaxEnergyProduction"></param>
        /// <param name="newPPA">If PPA has changed, new PPA; otherwise, initial PPA</param>
        /// <param name="newGlobalRisk">If risk has changed, otherwise, initial risk forecast</param>
        /// <param name="POEquity"></param>
        /// <param name="poEquityWithD"></param>
        /// <param name="OtherInvestEquity">The schedule of Equity withdrawal for PO</param>
        /// <param name="otherInvestEquityWithD">The schedule of Equity withdrawal for other investors</param>
        /// <param name="Loans"></param>
        /// <param name="LoanWithD">Current withdrawals of loans</param>
        /// <param name="Coins"></param>
        /// <param name="CoinWithD">Current withdrawals of coins</param>
        /// <param name="newOM"></param>
        /// <param name="newSGAs"></param>
        /// <param name="newRoyaltiesRate"></param>
        /// <param name="newTaxeRate"></param>
        /// <param name="Params"></param>
        /// <param name="ConsiderFinancialCosts"></param>
        /// <param name="coinType"></param>
        public Project(DateTime StartDate, DateTime Today, DateTime COD, double ProjectSize, ProjectDetail ProjectDetails, double ConcP, ConstructionParams Const_Params, double[,] newConst_CF, double newMaxEnergyProduction, double[] newPPA, double[] newGlobalRisk,
                EquityData POEquity, double[] poEquityWithD, EquityData OtherInvestEquity, double[] otherInvestEquityWithD, List<Loan> Loans, List<double[]> LoanWithD, Coin Coins, double[] CoinWithD, double[,] newOM, SGA newSGAs, double[,] newRoyaltiesRate, double[,] newTaxeRate, double[] newDividends, Hashtable Params, bool ConsiderFinancialCosts, CoinType coinType)
        {
            this.StartDate = StartDate;
            this.Today = Today;
            this.COD = COD;
            this.ProjectSize = ProjectSize;
            this.ProjectDetails = ProjectDetails;
            this.ConcP = ConcP;
            this.Const_Params = Const_Params;
            this.Const_CF = newConst_CF;
            this.MaxEnergyProduction = newMaxEnergyProduction;
            this.PPA = newPPA;
            this.GlobalRisk = newGlobalRisk;
            this.POEquity = POEquity;
            this.OtherInvestEquity = OtherInvestEquity;
            this.Loans = Loans;
            this.Coins = Coins;
            this.OM = newOM;
            this.SGAs = newSGAs;
            this.RoyaltiesRate = newRoyaltiesRate;
            this.TaxeRate = newTaxeRate;
            this.ConsiderFinancialCosts = ConsiderFinancialCosts;
            this.Params = Params;
            this.coinType = coinType;
            this.poEquityWithD = poEquityWithD;
            this.otherInvestEquityWithD = otherInvestEquityWithD;
            this.CoinWithD = CoinWithD;
            this.LoanWithD = LoanWithD;
            this.Dividends = newDividends;

            this.IsPricing = false;
        }

        public void ComputeCF_Pricing()
        {
            ResetData();
            
            string Step = "";
            try
            {
                SendMessage("\n" + DateTime.Now.ToShortTimeString() + " - Start Cash Flow Computation\n");

                InitVar();

                for (int i = 0; i < Const_Params.CP; i++)
                {
                    //Computation of ProjectSize for auditor cost
                    ProjectSize += Const_CF[i, 0];
                }

                Step = "Cash for Construction";
                //This cash for construction includes the minimum working capital + calculation of ProjectSize for auditor fee
                Compute_Cash4Construction();
                Step = "Revenues";
                Compute_NetIncomes();
                Step = "Outstanding Loans Nominals";
                Compute_LoansOutstandingNominals();
                Step = "Clean Data";
                CleanData();
                Step = "Computation of Cash Flows";
                ComputeCashFlows();
                
            }
            catch
            {
                Message(this, new IMessage("Computation issue with Cash Flows for " + Step + "\n"));
            }
            
        }

        public void ComputeCF_CP()
        {
            MaxIndex = (int)(ConcP * 12);

            InitVar();

            this.EquityPortionCoin = Math.Round(Coins.Nominal * Coins.Conversion / (Coins.Nominal * Coins.Conversion + POEquity.Amount * Math.Pow(1 + POEquity.Appreciation, Const_Params.CP) + OtherInvestEquity.Amount * Math.Pow(1 + OtherInvestEquity.Appreciation, Const_Params.CP)), 4);
            
            string Step = "Cash for Construction";

            try
            {
                Compute_Cash4Construction(true, false);
                Step = "Revenues";
                Compute_NetIncomes();
                Step = "Outstanding Loans Nominals";
                throw new Exception("Need to test Compute_LoansOutstandingNominals function for RT calculation");
                Compute_LoansOutstandingNominals(StartDate, Today, poEquityWithD, otherInvestEquityWithD, CoinWithD, LoanWithD);
                Step = "Clean Data";
                CleanData();
                Step = "Computation of Cash Flows";
                ComputeCashFlows();
            }
            catch
            {
                Message(this, new IMessage("Computation issue with Cash Flows for " + Step + "\n"));
            }
        }

        private void InitVar()
        {
            MaxIndex = (int)(ConcP * 12);

            Withdrawals.Coins = new double[MaxIndex];
            Withdrawals.EquityPO = new double[MaxIndex];
            Withdrawals.Loans = new List<double[]>();
            Withdrawals.TotalWithdrawal = new double[MaxIndex];
            Withdrawals.OtherEquity = new double[MaxIndex];

            CashForConstruction = new double[MaxIndex];
            AuditorCost = new double[MaxIndex];
            Revenues = new double[MaxIndex];
            GrossIncomes = new double[MaxIndex];
            NetIncomes = new double[MaxIndex];
            InterestsFromProject = new double[MaxIndex];

            FinancialCosts = new double[MaxIndex];
            FreeCashFlowsBeforeTaxe = new double[MaxIndex];
            Royalties = new double[MaxIndex];
            Taxes = new double[MaxIndex];
            FreeCashFlows = new double[MaxIndex];
            CoinOutstandingBalance = new double[MaxIndex];
            IncomeCMC_FromProject = new double[MaxIndex];
        }

        private void ResetData()
        {
            LoopAvoider1 = -101;
            PrevCoinNominal = 0.0;
            InterestCPDelay = 0.0;
            InterestCoverage = 0.0;
    }

        private void SendMessage(string Msg)
        {
            Message(this, new IMessage(Msg + "\n"));
        }

        private void Compute_NetIncomes()
        {
            switch (SGAs.Type)
            {
                case SGAType.TotalRevenues:
                    for (int i = 0; i < MaxIndex; i++)
                    {
                        Revenues[i] = Math.Round(PPA[i] * MaxEnergyProduction / 12.0 * GlobalRisk[i], MidpointRounding.ToEven);
                        GrossIncomes[i] = Revenues[i] * (1 - Get_OMRate(i));
                        if (i < Const_Params.CP + Const_Params.Delay) NetIncomes[i] = 0;
                        else
                        {
                            AuditorCost[i] = Math.Round(ProjectSize * ProjectDetails.AuditorFee / 12, 2);
                            NetIncomes[i] = Math.Round(GrossIncomes[i] - Math.Max(SGAs.MinCost / 12.0, SGAs.Rate * Revenues[i]), 2) - AuditorCost[i];
                        }
                    }
                    break;
                case SGAType.GrossIncome:
                    for (int i = 0; i < MaxIndex; i++)
                    {
                        Revenues[i] = Math.Round(PPA[i] * MaxEnergyProduction / 12.0 * GlobalRisk[i], MidpointRounding.ToEven);
                        GrossIncomes[i] = Revenues[i] * (1 - Get_OMRate(i));
                        if (i < Const_Params.CP + Const_Params.Delay) NetIncomes[i] = 0;
                        else
                        {
                            AuditorCost[i] = Math.Round(ProjectSize * ProjectDetails.AuditorFee / 12, 2);
                            NetIncomes[i] = Math.Round(GrossIncomes[i] - Math.Max(SGAs.MinCost / 12.0, SGAs.Rate * GrossIncomes[i]), 2) - AuditorCost[i];
                        }
                    }
                    break;
            }
        }

        private void Compute_Cash4Construction(bool IncludeMinWorkingCapital = true, bool IncludeCAPEX = true)
        {
            double CapexImpact = 0.0;
            double DelayImpact = 0.0;

            double minWorkingCapital = 0.0;
            if(IncludeMinWorkingCapital) minWorkingCapital = Math.Round(Const_Params.MinWorkingCapital / 12.0, MidpointRounding.AwayFromZero);

            if(IncludeCAPEX)
            {
                for (int i = 0; i < Const_CF.GetUpperBound(0) + 1; i++)
                {
                    CapexImpact += Const_CF[i, 0];
                }
                CapexImpact *= Const_Params.CAPEXIncrease;
            }

            int PeriodIncrease = Math.Min(Const_Params.CP, Const_Params.Delay) * 2;
            CapexImpact = Math.Round(CapexImpact / PeriodIncrease, MidpointRounding.AwayFromZero);

            for (int i = Const_CF.GetUpperBound(0) - PeriodIncrease / 2 + 1; i < Const_CF.GetUpperBound(0) + 1; i++)
            {
                DelayImpact += Const_CF[i, 0] + Const_CF[i, 1];
            }
            DelayImpact /= PeriodIncrease;

            int maxIndex = Const_Params.CP + Const_Params.Delay;

            ConstructionCostNetInterest = 0.0;

            for (int i = 0; i < maxIndex; i++)
            {
                AuditorCost[i] = Math.Round(ProjectSize * ProjectDetails.AuditorFee / 12.0, 2, MidpointRounding.AwayFromZero);
                if (i <= maxIndex - PeriodIncrease - 1) CashForConstruction[i] = Const_CF[i, 0] + Const_CF[i, 1] + AuditorCost[i] + minWorkingCapital;
                if (i > maxIndex - PeriodIncrease - 1) CashForConstruction[i] += CapexImpact + AuditorCost[i] + Math.Round(DelayImpact, MidpointRounding.AwayFromZero) + minWorkingCapital;
                ConstructionCostNetInterest += CashForConstruction[i];
            }
        }

        public void Increase_CoinNominal(double NewCoinNominal)
        {
            if (LoopAvoider1 < -100) LoopAvoider1 = Convert.ToInt32(Params["MaxNominalIncrease"]);
            Coins.Nominal = NewCoinNominal;
            CashForConstruction = new double[MaxIndex];
            Compute_Cash4Construction();
            SendMessage("Increasing Coin nominal to " + NewCoinNominal.ToString());
            Compute_LoansOutstandingNominals();
        }

        private void Compute_LoansOutstandingNominals()
        {
            if(LoopAvoider1 == 0)
            {
                SendMessage("Loop with Coin Nominal increase");
                return;
            }
            LoopAvoider1--;

            int[] AdjLoan = new int[Loans.Count];

            int tmpLoopAvoider = 0;

            //Classify loans by start date and rate
            Loans.Sort(sortLoans_byStartandRate);

            Recalc:
            tmpLoopAvoider++;
            if (tmpLoopAvoider == Convert.ToInt32(Params["MaxConstructionLoop"]))
            {
                SendMessage("Loop with construction financing calculation");
                return;
            }

            //We separate Coin and Loans as a portion of Coins may not get interests
            List<double[]> LoanWithD = new List<double[]>();
            List<double> LoanRates = new List<double>();
            List<int> LoanStartDates = new List<int>();
            List<int> LoanEndGP = new List<int>();
            double[] TotWithdraw = new double[MaxIndex];
            double[] CoinWithD = new double[MaxIndex];
            double[] poEquityWithD = new double[MaxIndex];
            double[] otherInvestEquityWithD = new double[MaxIndex];

            //Definition of equity withdrawal
            DefineEquity(ref poEquityWithD, ref otherInvestEquityWithD);

            //Compute basic linear withdrawals
            GetWithdrawals(ref LoanWithD, ref LoanRates, ref LoanStartDates, ref LoanEndGP, ref CoinWithD, ref AdjLoan, ref AdjCoin);

            double TotalConstructionCost = GetCashForConstruction();

            //double[] tmpWith = (double[])LoanWithD[0].Clone();
            double _totWithDebt = 0.0;

            //We compute cash allocation and withdrawal for loans and equity

            Valo.CFduringCP_Pricing(Const_Params.CP, Const_Params.Delay, ref LoanWithD, LoanRates, LoanStartDates, LoanEndGP, ref CoinWithD, Coins.RateDuringCP, ref poEquityWithD, POEquity.Amount,
                POEquity.Interest, ref otherInvestEquityWithD, OtherInvestEquity.Amount, OtherInvestEquity.Interest, CashForConstruction, ref TotWithdraw, out _totWithDebt);

            //Last interest for interest coverage after CP and Delay is added
            CoinWithD[Const_Params.CP + Const_Params.Delay] += InterestCoverage;
            TotWithdraw[Const_Params.CP + Const_Params.Delay] += InterestCoverage;

            //Do adjustment on equity value and calculate the need of additional equity
            double NominalAdjustment = 0.0;
            EquityResult res = CheckWithDraw(_totWithDebt, TotalConstructionCost, ref NominalAdjustment);

            switch(res)
            {
                case EquityResult.NominalAdjustment:
                    CoinNominalIncrease(this, new IIncreaseCoinNominal(NominalAdjustment));
                    return;
                case EquityResult.ReCalc:
                    goto Recalc;
                default:
                    break;
            }
            
            //We have matched the construction costs, so now we will compute interests
            //First we check that we don't withdraw any loan before its start date and cash above loan nominal
            for(int i = 0; i < Loans.Count; i++)
            {
                if (!CheckLoan(LoanWithD[i], i))
                {
                    SendMessage("Issue with withdraw of loan " + (i + 1).ToString());
                    return;
                }
            }
            
            GetLoanRepayment(Const_Params.CP, Const_Params.Delay, ref LoanWithD, ref CoinWithD, AdjLoan, AdjCoin);
            InterestsFromProject = new double[MaxIndex];
            
            //We calculate interests, including the coverage ratio
            GetInterests(Const_Params.CP, Const_Params.Delay, OtherInvestEquity.Interest, LoanWithD, CoinWithD, AdjLoan, AdjCoin, otherInvestEquityWithD);

            double tmpConstructionCost = 0.0;

            for (int i = 0; i < Const_Params.CP + Const_Params.Delay; i = i + 3)
            {
                tmpConstructionCost += CashForConstruction[i] + CashForConstruction[i + 1] + CashForConstruction[i + 2];
            }

            //We calculate the previous interest
            tmpInterests = tmpConstructionCost - ConstructionCostNetInterest;

            //We calculate total cash at disposal for construction + interests
            double RemainingCash = POEquity.Amount + OtherInvestEquity.Amount + Coins.Nominal - tmpConstructionCost;

            for(int i = 0; i < Loans.Count; i++)
            {
                RemainingCash += Loans[i].Nominal;
            }

            //Finally we calculate the increase in interests to be paid
            double SumInterests = InterestCPDelay + InterestCoverage - tmpInterests;

            if (SumInterests > RemainingCash + 1000 && (Coins.Nominal - PrevCoinNominal) > 50)
            {
                SendMessage("Not enough cash to cover project's interests");
                PrevCoinNominal = Coins.Nominal;
                CoinNominalIncrease(this, new IIncreaseCoinNominal(SumInterests - RemainingCash));
                return;
            }

            //If we cover interests, we calculate the interests net of coverage ratio - i.e. the additional cash we use to cover interest payments
            InterestsFromProject = new double[MaxIndex];
            GetInterests(Const_Params.CP, Const_Params.Delay, OtherInvestEquity.Interest, LoanWithD, CoinWithD, AdjLoan, AdjCoin, otherInvestEquityWithD, false);

            //SendMessage("\nProject is long cash after construction period: " + ProjectDetails.RefCurrency + FormatText(Math.Max(RemainingCash - SumInterests, 0)));

            Withdrawals.Coins = CoinWithD;
            Withdrawals.EquityPO = poEquityWithD;
            Withdrawals.Loans = LoanWithD;
            Withdrawals.TotalWithdrawal = TotWithdraw;
            Withdrawals.OtherEquity = otherInvestEquityWithD;

            TotalCPInterests = InterestCPDelay;

            this.EquityPortionCoin = Math.Min(ProjectDetails.EquityCap, Math.Round(Coins.Nominal * Coins.Conversion / (Coins.Nominal * Coins.Conversion + POEquity.Amount * Math.Pow(1 + POEquity.Appreciation, Const_Params.CP / 12.0) + OtherInvestEquity.Amount * Math.Pow(1 + OtherInvestEquity.Appreciation, Const_Params.CP / 12.0)), 4));
        }

        private void Compute_LoansOutstandingNominals(DateTime StartDate, DateTime Today, double[] poEquityWithD, double[] otherInvestEquityWithD, double[] CoinWithD, List<double[]> LoanWithD)
        {
            bool IncludeInterests = false;

            int[] AdjLoan = new int[Loans.Count];

            //Classify loans by start date and rate
            Loans.Sort(sortLoans_byStartandRate);

            Recalc:
            //We separate Coin and Loans as a portion of Coins may not get interests
            List<double> LoanRates = new List<double>();
            List<int> LoanStartDates = new List<int>();
            List<int> LoanEndGP = new List<int>();
            double[] TotWithdraw = new double[MaxIndex];

            //Compute basic linear withdrawals
            GetWithdrawals(ref LoanWithD, ref LoanRates, ref LoanStartDates, ref LoanEndGP, ref CoinWithD, ref AdjLoan, ref AdjCoin);

            double TotalConstructionCost = GetCashForConstruction();

            double[] tmpWith = (double[])LoanWithD[0].Clone();
            double _totWithDebt = 0.0;

            //We compute cash allocation and withdrawal for loans and equity

            Valo.CFduringCP_CP(StartDate, Today, Const_Params.CP, Const_Params.Delay, ref LoanWithD, LoanRates, LoanStartDates, LoanEndGP, ref CoinWithD, Coins.RateDuringCP, ref poEquityWithD, POEquity.Amount,
                POEquity.Interest, ref otherInvestEquityWithD, OtherInvestEquity.Amount, OtherInvestEquity.Interest, CashForConstruction, ref TotWithdraw, out _totWithDebt);

            //Last interest for interest coverage after CP and Delay is added
            CoinWithD[Const_Params.CP + Const_Params.Delay] += InterestCoverage;
            TotWithdraw[Const_Params.CP + Const_Params.Delay] += InterestCoverage;

            GetLoanRepayment(Const_Params.CP, Const_Params.Delay, ref LoanWithD, ref CoinWithD, AdjLoan, AdjCoin);
            InterestsFromProject = new double[MaxIndex];

            //We calculate interests, including the coverage ratio
            GetInterests(Const_Params.CP, Const_Params.Delay, OtherInvestEquity.Interest, LoanWithD, CoinWithD, AdjLoan, AdjCoin, otherInvestEquityWithD);

            if (!IncludeInterests) { IncludeInterests = true; goto Recalc; }

            Withdrawals.Coins = CoinWithD;
            Withdrawals.EquityPO = poEquityWithD;
            Withdrawals.Loans = LoanWithD;
            Withdrawals.TotalWithdrawal = TotWithdraw;
            Withdrawals.OtherEquity = otherInvestEquityWithD;

            Coins.Nominal = Math.Round(Coins.Nominal, 2);
            
            TotalCPInterests = InterestCPDelay;
        }

        private void DefineEquity(ref double[] poEquityWithD, ref double[] otherInvestEquityWithD)
        {
            if (POEquity.HasSchedule) poEquityWithD = POEquity.Schedule;
            else poEquityWithD[0] = POEquity.Amount;

            if (OtherInvestEquity.HasSchedule) otherInvestEquityWithD = OtherInvestEquity.Schedule;
            else otherInvestEquityWithD[0] = OtherInvestEquity.Amount;
        }

        private enum EquityResult
        {
            NominalAdjustment,
            ReCalc,
            Continue
        }

        private EquityResult CheckWithDraw(double totWithDebt, double TotalConstructionCost, ref double NominalAdjustment)
        {
            if (totWithDebt > TotalConstructionCost + 1000)
            {
                SendMessage("Too much cash to launch the project");
                NominalAdjustment = TotalConstructionCost - totWithDebt;
                return EquityResult.NominalAdjustment;
            }

            if(totWithDebt < TotalConstructionCost - 1000)
            {
                SendMessage("Not enough cash to launch the project");
                NominalAdjustment = TotalConstructionCost - totWithDebt;
                return EquityResult.NominalAdjustment;
            }

            return EquityResult.Continue;
        }

        private void CleanCoinWithD(List<double[]> LoanWithD, double[] TotWithD, double[] POEquity, double[] EquityOther, out double[] CoinWithD)
        {
            CoinWithD = new double[MaxIndex];

            for (int i = 0; i < Const_Params.CP + Const_Params.Delay; i = i + 3)
            {
                if(i < Const_Params.CP) CoinWithD[i] = TotWithD[i] - POEquity[i] - EquityOther[i];
                else CoinWithD[i] = TotWithD[i];

                for (int j = 0; j < LoanWithD.Count; j++)
                {
                    CoinWithD[i] -= LoanWithD[j][i];
                }
            }
        }

        private void GetWithdrawals(ref List<double[]> LoanWithD, ref List<double> LoanRates, ref List<int> LoanStartDates, ref List<int> LoanEndGP, ref double[] CoinWithD, ref int[] AdjLoan, ref int AdjCoin)
        {
            //We calculate the withrawal of all other loans
            for (int i = 0; i < Loans.Count; i++)
            {
                Loan loan = Loans[i];
                LoanWithD.Add(new double[MaxIndex]);
                LoanRates.Add(loan.Rate);
                LoanStartDates.Add(loan.Start);
                LoanEndGP.Add((int)(loan.GracePeriod * 12) + loan.Start);
                LoanWithD[i][loan.Start] = loan.Nominal * (1 - loan.UpfrontFee);

                int GracePeriod = (int)(loan.GracePeriod * 12);
                int EndGracePeriod = loan.Start + GracePeriod;
                int Maturity = loan.Start + (int)(loan.Tenor * 12);

                switch (Loans[i].Frequency)
                {
                    case Frequency.Quarterly:
                        AdjLoan[i] = 3;
                        break;
                    case Frequency.SemiAnnually:
                        AdjLoan[i] = 6;
                        break;
                    case Frequency.Annually:
                        AdjLoan[i] = 12;
                        break;
                }
            }

            //We calculate the withdrawal of coins
            CoinWithD[0] = Coins.Nominal - InterestCoverage;

            switch (Coins.Frequency)
            {
                case Frequency.Quarterly:
                    AdjCoin = 3;
                    break;
                case Frequency.SemiAnnually:
                    AdjCoin = 6;
                    break;
                case Frequency.Annually:
                    AdjCoin = 12;
                    break;
            }
        }

        private double GetCashForConstruction()
        {
            double result = 0.0;

            for (int i = 0; i < Const_Params.CP + Const_Params.Delay; i = i + 3)
            {
                CashForConstruction[i] = CashForConstruction[i] + CashForConstruction[i + 1] + CashForConstruction[i + 2] + InterestsFromProject[i];
                result += CashForConstruction[i];
                CashForConstruction[i + 1] = 0;
                CashForConstruction[i + 2] = 0;
            }

            return result;
        }

        private int sortLoans_byStartandRate(Loan a, Loan b)
        {
            if (a.Start < b.Start) return -1;
            if (a.Start == b.Start)
            {
                if (a.Rate < b.Rate) return -1;
                else if (a.Rate > b.Rate) return 1;
                return 0;
            }
            else return 1;
        }

        private double Get_OMRate(int time)
        {
            for (int i = 0; i < OM.GetUpperBound(1) + 1; i++)
            {
                if (time >= OM[0, i] && time <= OM[1, i]) return OM[2, i];
            }

            return 0.0;
        }

        private double Get_RoyaltiesRate(int time)
        {
            for (int i = 0; i < RoyaltiesRate.GetUpperBound(1) + 1; i++)
            {
                if (time >= RoyaltiesRate[0, i] && time <= RoyaltiesRate[1, i]) return RoyaltiesRate[2, i];
            }

            return 0.0;
        }

        private double Get_TaxeRate(int time)
        {
            for (int i = 0; i < TaxeRate.GetUpperBound(1) + 1; i++)
            {
                if (time >= TaxeRate[0, i] && time <= TaxeRate[1, i]) return TaxeRate[2, i];
            }

            return 0.0;
        }

        private void GetLoanRepayment(int CP, int Delay, ref List<double[]> LoanWithD, ref double[] CoinWithD, int[] AdjLoan, int AdjCoin)
        {
            for (int i = 0; i < Loans.Count; i++)
            {
                int GracePeriod = (int)(Loans[i].GracePeriod * 12);
                int EndGracePeriod = Loans[i].Start + GracePeriod;
                int Maturity = Loans[i].Start + (int)(Loans[i].Tenor * 12);

                if(Loans[i].IsBullet)
                {
                    for (int j = EndGracePeriod + AdjLoan[i]; j < Maturity + AdjLoan[i] - 1; j = j + AdjLoan[i])
                    {
                        LoanWithD[i][j] = 0;
                    }

                    LoanWithD[i][Maturity + AdjLoan[i] - 1] = -Loans[i].Nominal;
                }
                else
                {
                    double LoanRepayment = Math.Round(Loans[i].Nominal / (12.0 / AdjLoan[i] * (Loans[i].Tenor - Loans[i].GracePeriod)), 2);

                    //We calculate already the repayment of nominals
                    for (int j = EndGracePeriod + AdjLoan[i]; j < Maturity + AdjLoan[i]; j = j + AdjLoan[i])
                    {
                        LoanWithD[i][j] = -LoanRepayment;
                    }
                }
            }

            //We calculate Coins repayment knowing that a portion will be converted into Equities

            if (Coins.IsBullet)
            {
                for (int i = CP + AdjCoin; i < Coins.DebtTenor * 12 + AdjCoin - 1; i = i + AdjCoin)
                {
                    CoinWithD[i] -= 0;
                }

                CoinWithD[(int)(Coins.DebtTenor * 12 + AdjCoin - 1)] = -Coins.Nominal;
            }
            else
            {
                double CoinRepayment = Math.Round(Coins.Nominal * (1 - Coins.Conversion) / (12.0 / AdjCoin * (Coins.DebtTenor - CP / 12.0)), 2);

                for (int i = CP + AdjCoin; i < Coins.DebtTenor * 12 + AdjCoin; i = i + AdjCoin)
                {
                    CoinWithD[i] -= CoinRepayment;
                }
            }
                
        }

        private void GetInterests(int CP, int Delay, double OtherInvestEquity_Interest, List<double[]> LoanWithD, double[] CoinWithD, int[] AdjLoan, int AdjCoin, double[] otherInvestEquityWithD, bool IncludeInterestCoverage = true)
        {
            Loans_OustandingNominals = new double[MaxIndex];

            double CumulPos = 0.0;
            double PrevCumulPos = 0.0;
            bool IsRepayment = false;
            double SumInterest = 0.0;
            double tmpInterest;
            double tmpEquity = 0.0;

            double interestCoverageRatio = 1.0;

            if (IncludeInterestCoverage) interestCoverageRatio = Const_Params.InterestCoverageRatio;

            for (int i = 0; i < CP + Delay; i = i + 6)
            {
                InterestsFromProject[i + 6] += Math.Round(tmpEquity * 0.5 * OtherInvestEquity_Interest, 2);
                SumInterest += InterestsFromProject[i + 6];

                for (int j = i; j < i + 6; j++)
                {
                    double tmpDbl = Math.Round(interestCoverageRatio * otherInvestEquityWithD[j] * (6 - (j - i)) * OtherInvestEquity_Interest / 12.0, 2);
                    InterestsFromProject[i + 6] += tmpDbl;
                    SumInterest += tmpDbl;
                    tmpEquity += otherInvestEquityWithD[j];
                }
            }

            for (int i = 0; i < (int)(Coins.DebtTenor * 12) + AdjCoin; i = i + AdjCoin)
            {
                if (IsRepayment)
                {
                    Loans_OustandingNominals[i] = Math.Round(Loans_OustandingNominals[i - AdjCoin] + CoinWithD[i], 2);
                    InterestsFromProject[i + AdjCoin] += Math.Round(interestCoverageRatio * Loans_OustandingNominals[i] * AdjCoin * Coins.RateAfterCP / 12.0, 2);
                }
                else
                {
                    for (int j = i; j < i + AdjCoin; j++)
                    {
                        if (CoinWithD[j] < 0)
                        {
                            IsRepayment = true;
                            Loans_OustandingNominals[i - AdjCoin] = Math.Round(Coins.Nominal * (1 - Coins.Conversion), 2);
                            InterestsFromProject[i] -= interestCoverageRatio * PrevCumulPos * AdjCoin * Coins.RateDuringCP / 12.0;
                            PrevCumulPos = Loans_OustandingNominals[i - AdjCoin];
                            InterestsFromProject[i] += interestCoverageRatio * Math.Round(PrevCumulPos * AdjCoin * Coins.RateAfterCP / 12.0, 2);
                            PrevCumulPos = Math.Round(Loans_OustandingNominals[i - AdjCoin] + CoinWithD[j], 2);
                            Loans_OustandingNominals[i] = PrevCumulPos;
                            break;
                        }

                        if (Coins.PayEqtyInterestDuringCP)
                        {
                            tmpInterest = Math.Round(interestCoverageRatio * CoinWithD[j] * (AdjCoin - (j - i)) * Coins.RateDuringCP / 12.0, 2);
                            InterestsFromProject[i + AdjCoin] += tmpInterest;
                            SumInterest += tmpInterest;
                            CumulPos += CoinWithD[j];
                        }
                        else
                        {
                            tmpInterest = Math.Round(interestCoverageRatio * CoinWithD[j] * (1 - Coins.Conversion * (1 - Coins.PortionPayingEqty)) * (AdjCoin - (j - i)) * Coins.RateDuringCP / 12.0, 2);
                            InterestsFromProject[i + AdjCoin] += tmpInterest;
                            SumInterest += tmpInterest;
                            CumulPos += Math.Round(CoinWithD[j] * (1 - Coins.Conversion * (1 - Coins.PortionPayingEqty)), 0, MidpointRounding.AwayFromZero);
                        }
                    }

                    if (IsRepayment)
                    {
                        InterestsFromProject[i + AdjCoin] += Math.Round(interestCoverageRatio * PrevCumulPos * AdjCoin * Coins.RateAfterCP / 12.0, 2);
                    }
                    else
                    {
                        tmpInterest = Math.Round(interestCoverageRatio * PrevCumulPos * AdjCoin * Coins.RateDuringCP / 12.0, 2);
                        InterestsFromProject[i + AdjCoin] += tmpInterest;
                        SumInterest += tmpInterest;
                    }

                    PrevCumulPos = CumulPos;
                }
            }

            for (int i = 0; i < LoanWithD.Count; i++)
            {
                double[] tmpNom = new double[Loans_OustandingNominals.Length];

                IsRepayment = false;
                CumulPos = 0.0;
                PrevCumulPos = 0.0;

                for (int j = Loans[i].Start; j < Loans[i].Start + Loans[i].Tenor * 12 + AdjLoan[i]; j = j + AdjLoan[i])
                {
                    if (IsRepayment)
                    {
                        PrevCumulPos += LoanWithD[i][j];
                        InterestsFromProject[j + AdjLoan[i]] += Math.Round(interestCoverageRatio * PrevCumulPos * AdjLoan[i] * Loans[i].Rate / 12.0, 2);
                        tmpNom[j] += PrevCumulPos;
                    }
                    else
                    {
                        for (int k = j; k < j + AdjLoan[i]; k++)
                        {
                            if (LoanWithD[i][k] < 0)
                            {
                                IsRepayment = true;
                                tmpNom[j - AdjLoan[i]] += PrevCumulPos;
                                CumulPos += LoanWithD[i][k];
                                PrevCumulPos = CumulPos;
                                tmpNom[j] += CumulPos;
                                break;
                            }
                            tmpInterest = Math.Round(interestCoverageRatio * (LoanWithD[i][k] * AdjLoan[i] - (k - j)) * Loans[i].Rate / 12.0, 2);
                            InterestsFromProject[j + AdjLoan[i]] += tmpInterest;
                            SumInterest += tmpInterest;
                            CumulPos += LoanWithD[i][k];
                            tmpInterest = Math.Round(interestCoverageRatio * (Loans[i].Nominal - CumulPos) * (AdjLoan[i] - (k - j)) * Loans[i].CommitmentFee / 12.0, 2);
                            InterestsFromProject[j + AdjLoan[i]] += tmpInterest;
                            SumInterest += tmpInterest;
                        }

                        tmpInterest = Math.Round(interestCoverageRatio * (PrevCumulPos * AdjLoan[i] * Loans[i].Rate / 12.0 + (Loans[i].Nominal - PrevCumulPos - CumulPos) * AdjLoan[i] * Loans[i].CommitmentFee / 12.0), 2);
                        InterestsFromProject[j + AdjLoan[i]] += tmpInterest;
                        if (!IsRepayment) SumInterest += tmpInterest;
                        PrevCumulPos = CumulPos;
                    }
                }

                for(int j = Loans[i].Start; j < Loans[i].Start + Loans[i].Tenor * 12 + AdjLoan[i]; j++)
                {
                    Loans_OustandingNominals[j] += tmpNom[j];
                }
            }

            GetInterests_CP(CP, Delay);

            //return SumInterest;
        }

        private void GetInterests_CP(int CP, int Delay)
        {
            int Adj = 0;

            switch (Const_Params.InterestCoveragePeriod)
            {
                case InterestCoveragePeriod._3M:
                    Adj = 3;
                    break;
                case InterestCoveragePeriod._6M:
                    Adj = 6;
                    break;
                case InterestCoveragePeriod._9M:
                    Adj = 9;
                    break;
                case InterestCoveragePeriod._12M:
                    Adj = 12;
                    break;
                case InterestCoveragePeriod._15M:
                    Adj = 15;
                    break;
                case InterestCoveragePeriod._18M:
                    Adj = 18;
                    break;
            }

            InterestCPDelay = 0;
            InterestCoverage = 0;

            for (int i = 0; i < CP + Delay; i++)
            {
                InterestCPDelay += InterestsFromProject[i];
            }

            for (int i = CP + Delay; i < CP + Delay + Adj; i++)
            {
                InterestCoverage += InterestsFromProject[i];
            }
        }

        private void CleanData()
        {
            SendMessage("Remove interests and minimum Working Capital from Construction Cash Flows");

            CashForConstruction = new double[MaxIndex];

            Compute_Cash4Construction(false);
        }

        private bool CheckLoan(double[] LoanWithD, int LoanID)
        {
            double totWithdraw = 0.0;

            for(int i = 0; i < (int)(Loans[LoanID].GracePeriod * 12); i++)
            {
                totWithdraw += LoanWithD[i];

                if (totWithdraw > Loans[LoanID].Nominal + 100)
                    return false;
            }

            return true;
        }

        private void ComputeCashFlows()
        {
            CoinOutstandingBalance[0] = Math.Round(Coins.Nominal, 0);

            for (int i = 0; i < MaxIndex; i++)
            {
                FinancialCosts[i] = InterestsFromProject[i];

                if (Withdrawals.Coins[i] < 0) FinancialCosts[i] -= Withdrawals.Coins[i];
                else FreeCashFlowsBeforeTaxe[i] += Withdrawals.Coins[i];

                for (int j = 0; j < Withdrawals.Loans.Count; j++)
                {
                    if (Withdrawals.Loans[j][i] < 0) FinancialCosts[i] -= Withdrawals.Loans[j][i];
                    else FreeCashFlowsBeforeTaxe[i] += Withdrawals.Loans[j][i];
                }

                FreeCashFlowsBeforeTaxe[i] += NetIncomes[i] - FinancialCosts[i] - CashForConstruction[i];

                if (i < Withdrawals.EquityPO.Length) FreeCashFlowsBeforeTaxe[i] += Withdrawals.EquityPO[i];
                if(i < Withdrawals.OtherEquity.Length) FreeCashFlowsBeforeTaxe[i] += Withdrawals.OtherEquity[i];

                if (ConsiderFinancialCosts) Royalties[i] = Math.Round(FreeCashFlowsBeforeTaxe[i] * Get_RoyaltiesRate(i), 2, MidpointRounding.AwayFromZero);
                else Royalties[i] = Math.Max(0, Math.Round(NetIncomes[i] * Get_RoyaltiesRate(i), 2, MidpointRounding.AwayFromZero));

                if(!ProjectDetails.DivPaidBeforeTaxes)
                {
                    Taxes[i] = Math.Max(0, Math.Round((FreeCashFlowsBeforeTaxe[i] - Royalties[i]) * Get_TaxeRate(i), 2, MidpointRounding.AwayFromZero));
                }

                FreeCashFlows[i] = FreeCashFlowsBeforeTaxe[i] - Royalties[i] - Taxes[i];

                if (i == 0) CoinOutstandingBalance[i] -= Math.Round(Withdrawals.Coins[i], 0);
                else CoinOutstandingBalance[i] = Math.Round(-Withdrawals.Coins[i] + CoinOutstandingBalance[i - 1], 0);
            }
        }

        public double GetFirstDiv()
        {
            if (IsPricing) FirstDiv = Valo.GetFirstDividend(Coins.RateDuringCP, Coins.RateAfterCP, Coins.PayEqtyInterestDuringCP ? 1 : Coins.PortionPayingEqty, Coins.Conversion);
            else FirstDiv = Valo.GetFirstDividend(Dividends);

            return FirstDiv;
        }

        public void ComputeDepositAndTotalIncomeCMC(double[] DepositRates, double DepositRateDecrease, double[] IncomeCMC_FromProject, DateTime StartDate, DateTime COD, DateTime Today, DateTime FirstDivPayment, int DivPaymentFrequency, int MaxDepositTenor, List<double[]> CoinDeposit = null, double[] InterestOnCoinDeposit = null)
        {
            double[,] MatrixDepositRate;
            Valo.Resize_DepositRates(ref DepositRates, MaxDepositTenor, DepositRateDecrease, Today, ConcP, out MatrixDepositRate);

            if (CoinDeposit == null)
            {
                this.CoinDeposit = new List<double[]>(DepositRates.Length);
                for(int i = 0; i < DepositRates.Length; i++)
                {
                    this.CoinDeposit.Add(new double[CoinOutstandingBalance.Length]);
                }
                this.InterestOnCoinDeposit = new double[CoinOutstandingBalance.Length];
            }
            else
            {
                this.CoinDeposit = CoinDeposit;
                this.InterestOnCoinDeposit = InterestOnCoinDeposit;
            }

            int NextDivPaymentIndex;
            int CODIndex;
            int AdjDiv;

            Valo.GetDeposit_CoinNominal(StartDate, COD, Today, FirstDivPayment, DivPaymentFrequency, AdjCoin, CoinOutstandingBalance, DepositRates, MatrixDepositRate, ref this.CoinDeposit, ref this.InterestOnCoinDeposit, out NextDivPaymentIndex, out AdjDiv, out CODIndex);

            Valo.ComputeTotalIncome(this.EquityPortionCoin, InterestsFromProject, FreeCashFlows, ProjectDetails.PortionCash4CMC, Const_Params.MinWorkingCapital, this.InterestOnCoinDeposit, CODIndex, AdjCoin, DivPaymentFrequency, AdjDiv, out IncomeCMC_Total);

            FormatIncomeData(StartDate, COD, Today, ConcP);

            double[,] CFM;
            Valo.DepositRates2CapFactors(IncomeCMC_TotalFormatted.Length, DepositRates, MaxDepositTenor, DepositRateDecrease, out CFM);

            double[,] DivDeposit;
            int icase;

            double LastDiv;
            
            Valo.GetBestDiv(Coins.Nominal, ref FirstDiv, Coins.MinDivIncrease, Coins.Conversion, IncomeCMC_TotalFormatted.Length, IncomeCMC_TotalFormatted, CFM.GetUpperBound(1) + 1, CFM, out Dividends, out DivDeposit, out LastDiv, out icase);

            ConvergenceStatus(this, new IConvergenceStatus(icase == 0 ? true : false, LastDiv, Coins.MinDivIncrease));
            AverageDiv = Valo.AverageDiv(Dividends, Coins.Nominal);
        }
        
        private void FormatIncomeData(DateTime StartDate, DateTime COD, DateTime Today, double ConcP)
        {
            int StartIndex;
            int RemainingMonths;

            if (Today.CompareTo(COD) < 0)
            {
                StartIndex = (COD.Year - StartDate.Year) * 12 + COD.Month - StartDate.Month;
                RemainingMonths = 12 - COD.Month;
            }
            else
            {
                StartIndex = (Today.Year - StartDate.Year) * 12 + Today.Month - StartDate.Month;
                RemainingMonths = 12 - Today.Month;
            }
            
            int PastYears = StartIndex / 12;

            int RemainingYears = (int)(ConcP * 12) / 12 - PastYears;

            IncomeCMC_TotalFormatted = new double[RemainingYears];

            /*for (int i = Math.Max(12 - RemainingMonths - 1, 0); i < RemainingMonths; i++)
            {
                IncomeCMC_TotalFormatted[0] += Math.Round(IncomeCMC_Total[StartIndex + i]);
            }*/

            int LastIndex = 0;

            for (int i = StartIndex + RemainingMonths; i < IncomeCMC_Total.Length - 12; i = i + 12)
            {
                for(int j = 0; j < 12; j++)
                {
                    IncomeCMC_TotalFormatted[(i - (StartIndex + RemainingMonths)) / 12] += Math.Round(IncomeCMC_Total[i + j], 0);
                }
                LastIndex = i;
            }

            for(int i = LastIndex + 11; i< IncomeCMC_Total.Length; i++)
            {
                IncomeCMC_TotalFormatted[IncomeCMC_TotalFormatted.GetUpperBound(0)] += Math.Round(IncomeCMC_Total[i], 0);
            }
        }

        public void RiskAnalysis_Pricing(double[] RiskParams)
        {
            List<double[]> SavedData = SaveData();

            Test(GlobalRisk, RiskParams[0], true, out IncomeCMC_TotalFormatted_GlobalRiskIncrease);
            

            
        }

        private void Test(double[] DataToTest, double RiskParam, bool BasisPoints, out double[] Result)
        {
            double[] SavedTestedData = DataToTest.Clone() as double[];

            if(BasisPoints)
            {
                for (int i = 0; i < DataToTest.Length; i++)
                {
                    DataToTest[i] = Math.Max(0, DataToTest[i] + RiskParam);
                }
            }
            else
            {
                for (int i = 0; i < DataToTest.Length; i++)
                {
                    DataToTest[i] = Math.Round(DataToTest[i] * (1 - RiskParam), 5);
                }
            }

            ComputeCF_Pricing();

            //ComputeDepositAndTotalIncomeCMC(, DepositRateDecrease, IncomeCMC_FromProject, StartDate, COD, StartDate, CoinParams.FirstPayment, CoinParams.DivPaymentFrequency, CoinParams.MaxDepositTenor);

            Result = IncomeCMC_TotalFormatted.Clone() as double[];

            DataToTest = SavedTestedData.Clone() as double[];
        }

        private List<double[]> SaveData()
        {
            List<double[]> data = new List<double[]>();

            data.Add(IncomeCMC_Total.Clone() as double[]);
            data.Add(IncomeCMC_TotalFormatted.Clone() as double[]);

            return data;
        }

        public enum CoinType
        {
            DebtEquity,
            Equity,
            Debt
        }

        public enum Frequency
        {
            Quarterly,
            SemiAnnually,
            Annually
        }

        public enum SGAType
        {
            TotalRevenues,
            GrossIncome
        }

        public enum rateType
        {
            Fixed,
            Variable
        }

        public struct Loan
        {
            public double Nominal;
            public int Start;
            public double Tenor;
            public double GracePeriod;
            public bool LinearWithdraw;
            public double Rate;
            public double UpfrontFee;
            public double CommitmentFee;
            public rateType RateType;
            public Frequency Frequency;
            public bool IsBullet;
        }

        public struct Coin
        {
            public double IssuanceCost;
            public double Nominal;
            public double RateDuringCP;
            public double RateAfterCP;
            public double Conversion;
            public double PortionPayingEqty;
            public Frequency Frequency;
            public double DebtTenor;
            public bool IsBullet;
            public bool PayEqtyInterestDuringCP;
            public double LastDividend;
            public double MinDivIncrease;
        }

        public struct SGA
        {
            public double Rate;
            public double MinCost;
            public SGAType Type;
        }

        public struct ProjectDetail
        {
            public string ProjectName;
            public string Sector;
            public string SubSector;
            public string Country;
            public string POName;
            public string EPCName;
            public string Auditor;
            public string RefCurrency;
            public string PPACurrency;
            public bool DivPaidBeforeTaxes;
            public double PortionCash4CMC;
            public double AuditorFee;
            public double EquityCap;
            public double EquityPortion;
        }

        public struct ConstructionParams
        {
            public int CP;
            public double CAPEXIncrease;
            public double MinWorkingCapital;
            public double InterestCoverageRatio;
            public int Delay;
            public InterestCoveragePeriod InterestCoveragePeriod;
        }

        public struct EquityData
        {
            public double Amount;
            public double Appreciation;
            public double Interest;
            public bool HasSchedule;
            public double[] Schedule;
        }

        public struct Withdrawal
        {
            public double[] Coins;
            public List<double[]> Loans;
            public double[] EquityPO;
            public double[] OtherEquity;
            public double[] TotalWithdrawal;
        }

        public enum InterestCoveragePeriod
        {
            _0M,
            _3M,
            _6M,
            _9M,
            _12M,
            _15M,
            _18M
        }

        static private string FormatText(double dblData)
        {
            string strData = dblData.ToString();

            try
            {
                string newFormat = "";
                double text = Convert.ToDouble(strData);
                text = Math.Round(text);
                double dec = Math.Floor(text / 1000);
                while (dec >= 1)
                {
                    if (newFormat.Equals(""))
                    {
                        double toWrite = text - dec * 1000;
                        if (toWrite < 10) { newFormat = "00" + toWrite.ToString(); }
                        else if (toWrite < 100) { newFormat = "0" + toWrite.ToString(); }
                        else { newFormat = toWrite.ToString(); }
                    }
                    else
                    {
                        double toWrite = text - dec * 1000;
                        if (toWrite < 10) { newFormat = "00" + toWrite.ToString() + "," + newFormat; }
                        else if (toWrite < 100) { newFormat = "0" + toWrite.ToString() + "," + newFormat; }
                        else { newFormat = toWrite.ToString() + "," + newFormat; }
                    }
                    text = dec;
                    dec = Math.Floor(text / 1000);
                }
                if (newFormat.Equals("")) { newFormat = text.ToString(); }
                else { newFormat = text.ToString() + "," + newFormat; }

                return newFormat;
            }
            catch
            { return ""; }
        }
    }
}
