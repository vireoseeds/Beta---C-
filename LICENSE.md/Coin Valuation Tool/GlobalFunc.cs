using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL;
using ProjectLib;

namespace Coin_Valuation_Tool
{
    static public class GlobalFunc
    {
        public enum FormatType
        {
            NumberNoDec,
            NumberWithDec
        }

        static public double ToDouble(string data)
        {
            try
            {
                data.Replace(",", ".");
                return Convert.ToDouble(data);
            }
            catch
            { return 0.0; }
        }

        static public void FormatTextBox(TextBox textBox, FormatType format)
        {
            try
            {
                switch (format)
                {
                    case FormatType.NumberNoDec:
                        string newFormat = "";
                        double text = Convert.ToDouble(textBox.Text);
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

                        textBox.Text = newFormat;
                        break;

                    case FormatType.NumberWithDec:
                        newFormat = "";
                        text = Convert.ToDouble(textBox.Text);
                        
                        double after = Math.Round(text, 2) - Math.Round(text);
                        text = Math.Round(text);

                        dec = Math.Floor(text / 1000);
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
                        textBox.Text = newFormat + after.ToString().Substring(1, after.ToString().Length - 1);
                        break;
                }
            }
            catch
            { }
        }

        static public string FormatText(string strData, FormatType format)
        {
            double dblData = ToDouble(strData);
            bool IsNeg = false;

            if(dblData < 0)
            {
                IsNeg = true;
                dblData = -dblData;
                strData = dblData.ToString();
            }

            try
            {
                switch (format)
                {
                    case FormatType.NumberNoDec:
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

                        if (IsNeg) newFormat = "-" + newFormat;
                        return newFormat;
                    case FormatType.NumberWithDec:
                        newFormat = "";
                        text = Convert.ToDouble(strData);

                        double after = Math.Round(text, 2) - Math.Round(text);
                        text = Math.Round(text);

                        dec = Math.Floor(text / 1000);
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
                        newFormat  = newFormat + after.ToString().Substring(1, after.ToString().Length - 1);
                        if (IsNeg) newFormat = "-" + newFormat;
                        return newFormat;
                }
                return "";
            }
            catch
            { return ""; }
        }

        /// <summary>
        /// Get a template of PPA
        /// </summary>
        /// <param name="StartPrice">Initial price of PPA</param>
        /// <param name="StartPeriod">Period length in YEARS of initial price</param>
        /// <param name="AveragePPA">Expected average PPA</param>
        /// <param name="ConcP">Concession period in YEARS</param>
        /// <param name="CP">Construction period in MONTHS</param>
        /// <returns></returns>
        static public double[] PPAprice(double StartPrice, int StartPeriod, double AveragePPA, double ConcP, int CP, out double[] PPA, ref double SumPPA)
        {
            int Rounding = 1;
            SumPPA = 0;
            
            while(StartPrice * Math.Pow(10, Rounding) - Math.Round(StartPrice * Math.Pow(10, Rounding)) != 0)
            {
                Rounding++;
            }

            Rounding += 2;

            double[] result = new double[(int)(ConcP * 4)];

            double TPPAd = Math.Round(StartPeriod / 2.0) * 2;
            double startPeriod = StartPeriod * 4;

            double Tt = ConcP * 4 - Math.Round(CP / 3.0);
            double Tl = Tt - startPeriod - TPPAd * 4;

            double Beta = 1;
            double AdjustBeta = 0.05;

            double Temp1 = AveragePPA * Tt / (4 * StartPrice) - startPeriod / 4;
            double Temp = Temp1 - PPATimePoly(Beta, TPPAd, Tl / 4);

            int GF = 0;

            while(Temp > 0.01 && GF < 500)
            {
                GF++;
                Beta -= AdjustBeta;
                Temp = Temp1 - PPATimePoly(Beta, TPPAd, Tl / 4);
                if (Temp < 0)
                {
                    Beta += AdjustBeta;
                    AdjustBeta = AdjustBeta / 2;
                }
                Temp = Temp1 - PPATimePoly(Beta, TPPAd, Tl / 4);
            }

            for(int i = (int)Math.Round(CP / 3.0); i < Math.Round(CP / 3.0) + startPeriod; i++)
            {
                result[i] = StartPrice;
            }

            int startIndex = (int)(Math.Round(CP / 3.0) + startPeriod);
            int lastIndex = (int)(Math.Round(CP / 3.0) + startPeriod + TPPAd * 4);

            for (int i = startIndex; i < lastIndex; i = i + 4)
            {
                result[i] = Math.Round(StartPrice * Math.Pow(1 - Beta, (i - startIndex) / 4 + 1), Rounding);
                if (i == result.Length - 1) break;
                result[i + 1] = result[i];
                if (i == result.Length - 2) break;
                result[i + 2] = result[i];
                if (i == result.Length - 3) break;
                result[i + 3] = result[i];
            }

            double PPAf = Math.Round(StartPrice * Math.Pow(1 - Beta, TPPAd + 1), Rounding);

            for (int i = (int)(Math.Round(CP / 3.0) + startPeriod + TPPAd * 4); i < result.Length; i++)
            {
                result[i] = PPAf;
            }

            PPA = new double[(int)(ConcP * 12)];

            for(int i = 0; i < result.Length; i++)
            {
                SumPPA += result[i];
                PPA[3 * i] = result[i];
                PPA[3 * i + 1] = result[i];
                PPA[3 * i + 2] = result[i];
            }

            return result;
        }

        static private double PPATimePoly(double Beta, double TPPAd, double Tl)
        {
            double result = 0.0;

            for(int i = 1; i < TPPAd + 1; i++)
            {
                result += Math.Pow(1 - Beta, i);
            }

            result += Math.Pow(1 - Beta, TPPAd + 1) * Tl;
            return result;
        }

        public enum BasicErrors
        {
            ConstructionPeriod_Bigger_ConcP,
            ConstructionPeriodAndDelay_Bigger_ConcP,
            ConstructionPeriod_Smaller_Delay,
            CoinGP_Bigger_Tenor,
            OtherInvestorEquity_Bigger_CoinInterest,
            NoError
        }

        static public BasicErrors DoBasicChecks(double ConcP, int CP, int Delay, Project.Coin coin, Project.EquityData equityData)
        {
            if (CP / 12.0 > ConcP) return BasicErrors.ConstructionPeriod_Bigger_ConcP;
            if (CP < Delay) return BasicErrors.ConstructionPeriod_Smaller_Delay;
            if ((CP + Delay) / 12.0 > ConcP) return BasicErrors.ConstructionPeriodAndDelay_Bigger_ConcP;
            if (coin.DebtTenor < (CP + Delay) / 12) return BasicErrors.CoinGP_Bigger_Tenor;
            if (equityData.Interest > coin.RateDuringCP) return BasicErrors.OtherInvestorEquity_Bigger_CoinInterest;
            return BasicErrors.NoError;
        }

        static public double[] DBDepositRates2Vector(List<SQLFunc.SubResults> DBDepositRates)
        {
            int Max = DBDepositRates.Count;
            double[] result = new double[Max];

            for(int i = 0; i < DBDepositRates.Count; i++)
            {
                result[i] = ToDouble(DBDepositRates[i]._SubResults[0].ToString());
            }

            return result;
        }

        static public string FormatDate(DateTime Date)
        {
            return Date.Year + "-" + Date.Month + "-" + Date.Day;
        }

        static public void WriteInfo(string Text, DateTime Today)
        {
            string date = Today.Year.ToString() + Today.Month.ToString() + Today.Day.ToString();

            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Log\");

            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt", true))
                { doc.WriteLine(Text); }
            }
            else
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
                { doc.WriteLine(Text); }
            }

        }

        static public void WriteInfo(double dblData, DateTime Today)
        {
            string date = Today.Year.ToString() + Today.Month.ToString() + Today.Day.ToString();

            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Log\");

            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt", true))
                { doc.WriteLine(dblData.ToString()); }
            }
            else
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
                { doc.WriteLine(dblData.ToString()); }
            }
        }

        static public void WriteInfo(double[] vData, DateTime Today)
        {
            string data2write = "{";
            string date = Today.Year.ToString() + Today.Month.ToString() + Today.Day.ToString();

            for (int i = 0; i < vData.Length; i++)
            {
                if (i != vData.Length - 1) data2write += vData[i].ToString() + ",";
                else data2write += vData[i].ToString() + "}";
            }

            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Log\");

            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt", true))
                { doc.WriteLine(data2write); }
            }
            else
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
                { doc.WriteLine(data2write); }
            }
        }

        static public void WriteInfo(double[,] mData, DateTime Today)
        {
            string data2write = "{";
            string date = Today.Year.ToString() + Today.Month.ToString() + Today.Day.ToString();

            for (int i = 0; i < mData.GetUpperBound(0) + 1; i++)
            {
                data2write += "{";

                for (int j = 0; j < mData.GetUpperBound(1) + 1; j++)
                {
                    if (j != mData.GetUpperBound(1)) data2write += mData[i, j].ToString() + ",";
                    else data2write += mData[i, j].ToString() + "}";
                }
                if (i != mData.GetUpperBound(0)) data2write += ",";
            }

            data2write += "}";

            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Log\");

            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt", true))
                { doc.WriteLine(data2write); }
            }
            else
            {
                using (System.IO.StreamWriter doc = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\Log\" + date + " - Data.txt"))
                { doc.WriteLine(data2write); }
            }



        }
    }
}
