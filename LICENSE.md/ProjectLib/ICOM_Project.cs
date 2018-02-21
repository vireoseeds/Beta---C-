using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ProjectLib
{
    [Guid("04834cfc-7507-4af3-a7df-4587c5e8bb60")]
    public interface IProject
    {
        void ComputeCF_Pricing();
        void Increase_CoinNominal(double NewCoinNominal);
        void ComputeDepositAndTotalIncomeCMC(double[] RateFactors, double DepositRateDecrease, double[] IncomeCMC_FromProject, DateTime StartDate, DateTime COD, DateTime Today, DateTime FirstDivPayment, int DivPaymentFrequency, int MaxDepositTenor, List<double[]> CoinDeposit = null, double[] InterestOnCoinDeposit = null);
        double GetFirstDiv();
        void RiskAnalysis_Pricing(double[] RiskParams);
    }

    [Guid("16a27028-bfac-4e20-a7d2-0da4b24590db"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IProjectEvents
    {
        event EventHandler<IMessage> Message;
        event EventHandler<IIncreaseCoinNominal> CoinNominalIncrease;
        event EventHandler<IConvergenceStatus> ConvergenceStatus;

    }

    public class IConvergenceStatus: EventArgs
    {
        public bool HasConverged;
        public double MinIncrease;
        public double LasDiv;

        public IConvergenceStatus(bool HasConverged, double LastDiv, double MinDivIncrease)
        {
            this.HasConverged = HasConverged;
            this.MinIncrease = MinIncrease;
            this.LasDiv = LastDiv;
        }
    }

    public class IMessage : EventArgs
    {
        public string message;

        public IMessage(string message)
        {
            this.message = message;
        }
    }

    public class IIncreaseCoinNominal : EventArgs
    {
        public double Amount;

        public IIncreaseCoinNominal(double Amount)
        {
            this.Amount = Amount;
        }
    }
}