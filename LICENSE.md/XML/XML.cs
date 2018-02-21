using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using ProjectLib;

namespace XML
{
    static public class XMLFunctions
    {
        static public string GetLinearModelData(string strXML, string Model = "")
        {
            if(Model.Equals(""))
            {
                Model = "Parameter";
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(strXML);

                XmlNodeList BasicParams = xmlDoc.GetElementsByTagName(Model);

                foreach (XmlNode BasicParam in BasicParams)
                {
                    return BasicParam.InnerText;
                }
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("XML.GetLinearModelData\n" + Ex.Message);
            }

            return "";
        }

        static public List<string[]> GetNonLinearModelData(string strXML, string Model = "")
        {
            List<string[]> result;

            string Node1 = "Col0";
            string Node2 = "Col1";
            string Node3 = "Col2";

            if(Model.Equals(""))
            {
                Model = "Parameters";
                Node1 = "Start";
                Node2 = "End";
                Node3 = "Value";
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(strXML);

                XmlNodeList BasicParams = xmlDoc.GetElementsByTagName(Model);

                XmlNode BasicParam = BasicParams.Item(0);

                result = new List<string[]>();

                foreach (XmlNode InnerParam in BasicParam)
                {
                    XmlNodeList InnerParams = InnerParam.ChildNodes;
                    string[] subResult = new string[3];

                    for(int i = 0; i < InnerParams.Count; i++)
                    {
                        if (InnerParams[i].Name.Equals(Node1)) subResult[0] = InnerParams[i].InnerText;
                        if (InnerParams[i].Name.Equals(Node2)) subResult[1] = InnerParams[i].InnerText;
                        if (InnerParams[i].Name.Equals(Node3)) subResult[2] = InnerParams[i].InnerText;
                    }

                    result.Add(subResult);
                }
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("XML.GetNonLinearModelData\n" + Ex.Message);
                return new List<string[]>(1);
            }

            return result;
        }

        static public string[] GetCyclicCurvData(string strXML, string Model = "")
        {
            string[] result = new string[5];

            string Node1 = "Row0";
            string Node2 = "Row1";
            string Node3 = "Row2";
            string Node4 = "Row3";
            string Node5 = "Row4";

            if (Model.Equals(""))
            {
                Model = "Parameters";
                Node1 = "TrendPeriod";
                Node2 = "TrendAmplitude";
                Node3 = "CycleLength";
                Node4 = "CycleVariation";
                Node5 = "CycleAmplitude";
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(strXML);

                XmlNodeList BasicParams = xmlDoc.GetElementsByTagName(Model);
                XmlNode BasicParam = BasicParams.Item(0);

                XmlNodeList InnerParams = BasicParam.ChildNodes;

                for (int i = 0; i < InnerParams.Count; i++)
                {
                    if (InnerParams[i].Name.Equals(Node1)) result[0] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node2)) result[1] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node3)) result[2] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node4)) result[3] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node5)) result[4] = InnerParams[i].InnerText;
                }

                return result;
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("XML.GetCyclicCurvData\n" + Ex.Message);
                return result;
            }
        }

        static public string[] GetOtherCyclicData(string strXML, string Model = "")
        {
            string[] result = new string[5];

            string Node1 = "Row0";
            string Node2 = "Row1";
            string Node3 = "Row2";
            string Node4 = "Row3";
            string Node5 = "Row4";
            string Node6 = "Row4";

            if (Model.Equals(""))
            {
                Model = "Parameters";
                Node1 = "Trend";
                Node2 = "CycleLength";
                Node3 = "CycleVariation";
                Node4 = "CycleAmplitude";
                Node5 = "CycleExpansion";
                Node6 = "CycleAmortization";
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(strXML);

                XmlNodeList BasicParams = xmlDoc.GetElementsByTagName(Model);
                XmlNode BasicParam = BasicParams.Item(0);

                XmlNodeList InnerParams = BasicParam.ChildNodes;

                for (int i = 0; i < InnerParams.Count; i++)
                {
                    if (InnerParams[i].Name.Equals(Node1)) result[0] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node2)) result[1] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node3)) result[2] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node4)) result[3] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node5)) result[4] = InnerParams[i].InnerText;
                    if (InnerParams[i].Name.Equals(Node6)) result[4] = InnerParams[i].InnerText;
                }

                return result;
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("XML.GetOtherCyclicData\n" + Ex.Message);
                return result;
            }
        }

        static public string Create_OtherDetailsXML(Project.ProjectDetail ProjectDetails)
        {
            string result = "<OtherDetails>";

            result += "<PO>" + ProjectDetails.POName + "</PO>";
            result += "<Auditor>" + ProjectDetails.Auditor + "</Auditor>";
            result += "<EPC>" + ProjectDetails.EPCName + "</EPC>";
            result += "<DivTaxe>" + ProjectDetails.DivPaidBeforeTaxes.ToString() + "</DivTaxe>";
            result += "<PortionCashCMC>" + ProjectDetails.PortionCash4CMC.ToString() + "</PortionCashCMC>";
            result += "<AuditorFee>" + ProjectDetails.AuditorFee.ToString() + "</AuditorFee>";
            result += "<EquityCap>" + ProjectDetails.EquityCap.ToString() + "</EquityCap>";
            result += "<EquityPortion>" + ProjectDetails.EquityPortion.ToString() + "</EquityPortion>";
            result += "</OtherDetails>";

            return result;
        }

        static public string[] GetData_FromXMLOtherDetails(string strXML)
        {
            string[] result = new string[8];

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNodeList BasicParams = xmlDoc.GetElementsByTagName("OtherDetails");
            XmlNode BasicParam = BasicParams.Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("PO")) result[0] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("Auditor")) result[1] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("EPC")) result[2] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("DivTaxe")) result[3] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("PortionCashCMC")) result[4] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("AuditorFee")) result[5] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("EquityCap")) result[6] = InnerParams[i].InnerText;
                if (InnerParams[i].Name.Equals("EquityPortion")) result[7] = InnerParams[i].InnerText;
            }

            return result;
        }

        static public string Create_ConstParamXML(Project.ConstructionParams Const_Params)
        {
            string result = "<ConstParams>";

            result += "<CP>" + Const_Params.CP + "</CP>";
            result += "<CAPEXIncrease>" + Const_Params.CAPEXIncrease + "</CAPEXIncrease>";
            result += "<Delay>" + Const_Params.Delay + "</Delay>";
            result += "<IRCoverage>" + Const_Params.InterestCoverageRatio + "</IRCoverage>";
            result += "<WorkingCap>" + Const_Params.MinWorkingCapital + "</WorkingCap>";

            string strInterestCoveragePeriod = "";
            switch(Const_Params.InterestCoveragePeriod)
            {
                case Project.InterestCoveragePeriod._0M:
                    strInterestCoveragePeriod = "0M";
                    break;
                case Project.InterestCoveragePeriod._3M:
                    strInterestCoveragePeriod = "3M";
                    break;
                case Project.InterestCoveragePeriod._6M:
                    strInterestCoveragePeriod = "6M";
                    break;
                case Project.InterestCoveragePeriod._9M:
                    strInterestCoveragePeriod = "9M";
                    break;
                case Project.InterestCoveragePeriod._12M:
                    strInterestCoveragePeriod = "12M";
                    break;
                case Project.InterestCoveragePeriod._15M:
                    strInterestCoveragePeriod = "15M";
                    break;
                case Project.InterestCoveragePeriod._18M:
                    strInterestCoveragePeriod = "18M";
                    break;
            }

            result += "<InterestCoveragePeriod>" + strInterestCoveragePeriod + "</InterestCoveragePeriod>";

            result += "</ConstParams>";

            return result;
        }

        static public Project.ConstructionParams GetData_FromConstrParamXML(string strXML)
        {
            Project.ConstructionParams result = new Project.ConstructionParams();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNodeList BasicParams = xmlDoc.GetElementsByTagName("ConstParams");
            XmlNode BasicParam = BasicParams.Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("CP")) result.CP = Convert.ToInt32(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("CAPEXIncrease")) result.CAPEXIncrease = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Delay")) result.Delay = Convert.ToInt32(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("IRCoverage")) result.InterestCoverageRatio = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("WorkingCap")) result.MinWorkingCapital = ToDouble(InnerParams[i].InnerText);
                if(InnerParams[i].Name.Equals("InterestCoveragePeriod"))
                {
                    switch(InnerParams[i].InnerText)
                    {
                        case "0M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._0M;
                            break;
                        case "3M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._3M;
                            break;
                        case "6M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._6M;
                            break;
                        case "9M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._9M;
                            break;
                        case "12M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._12M;
                            break;
                        case "15M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._15M;
                            break;
                        case "18M":
                            result.InterestCoveragePeriod = Project.InterestCoveragePeriod._18M;
                            break;
                    }
                }
            }

            return result;
        }

        static public string Create_EnergyProductionXML(double Power, double PlantFactor, double EnergyProduction)
        {
            string result = "<EnergyProduction>";
            result += "<Power>" + Power.ToString() + "</Power>";
            result += "<PlantFactor>" + PlantFactor.ToString() + "</PlantFactor>";
            result += "<TotalEnergy>" + EnergyProduction.ToString() + "</TotalEnergy>";

            result += "</EnergyProduction>";

            return result;
        }

        static public double[] GetData_FromEnergyProductionXML(string strXML)
        {
            double[] result = new double[3];

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNodeList BasicParams = xmlDoc.GetElementsByTagName("EnergyProduction");
            XmlNode BasicParam = BasicParams.Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("Power")) result[0] = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("PlantFactor")) result[1] = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("TotalEnergy")) result[2] = ToDouble(InnerParams[i].InnerText);
            }

            return result;
        }

        static public string Create_XMLFromMatrix(double[,] data, string FromModel = "")
        {
            string result;

            if (FromModel.Equals("")) result = "<Data>";
            else result = "<" + FromModel + ">";

            for (int i = 0; i < data.GetUpperBound(0) + 1; i++)
            {
                result += "<Row" + i + ">";

                for (int j = 0; j < data.GetUpperBound(1) + 1; j++)
                {
                    result += "<Col" + j + ">" + data[i, j].ToString() + "</Col" + j + ">";
                }

                result += "</Row" + i + ">";
            }

            if (FromModel.Equals("")) result += "</Data>";
            else result += "</" + FromModel + ">";

            return result;
        }

        static public double[,] GetData_MatrixFromXML(string strXML, ref string Model)
        {
            double[,] result = new double[1,1];
            List<double[]> tmpResult = new List<double[]>();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            if(xmlDoc.GetElementsByTagName("Data").Count != 0) tmpResult = GetInfo_List(xmlDoc.GetElementsByTagName("Data"));
            else if(xmlDoc.GetElementsByTagName("Multi-Linear").Count != 0)
            {
                tmpResult = GetInfo_List(xmlDoc.GetElementsByTagName("Multi-Linear"));
                Model = "Multi-Linear";
            }
            else if(xmlDoc.GetElementsByTagName("Stepped").Count != 0)
            {
                tmpResult = GetInfo_List(xmlDoc.GetElementsByTagName("Stepped"));
                Model = "Stepped";
            }
            else
            {
                Model = "Other";
                return result;
            }

            result = new double[tmpResult.Count, tmpResult[0].Length];
            for (int i = 0; i < tmpResult.Count; i++)
            {
                for (int j = 0; j < tmpResult[0].Length; j++)
                {
                    result[i, j] = tmpResult[i][j];
                }
            }

            return result;
        }

        static private List<double[]> GetInfo_List(XmlNodeList BasicParams)
        {
            List<double[]> tmpResult = new List<double[]>();

            foreach (XmlNode SubBasicParam in BasicParams)
            {
                XmlNodeList InnerParams = SubBasicParam.ChildNodes;
                
                foreach (XmlNode ListParam in InnerParams)
                {
                    tmpResult.Add(new double[ListParam.ChildNodes.Count]);

                    for (int i = 0; i < ListParam.ChildNodes.Count; i++)
                    {
                        tmpResult[tmpResult.Count - 1][i] = ToDouble(ListParam.ChildNodes[i].InnerText);
                    }
                }

                
            }

            return tmpResult;
        }

        static public string Create_XMLFromMatrix(double[] data, string FromModel = "")
        {
            string result;

            if (FromModel.Equals("")) result = "<Data>";
            else result = "<" + FromModel + ">";

            for (int i = 0; i < data.Length; i++)
            {
                result += "<Row" + i + ">" + data[i].ToString() + "</Row" + i + ">";
            }

            if (FromModel.Equals("")) result += "</Data>";
            else result += "</" + FromModel + ">";

            return result;
        }

        static public double[] GetData_VectorFromXML(string strXML, ref string Model)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            if (xmlDoc.GetElementsByTagName("Data").Count != 0)
            {
                return GetInfo_Array(xmlDoc.GetElementsByTagName("Data").Item(0));
            }
            if (xmlDoc.GetElementsByTagName("Linear").Count != 0)
            {
                Model = "Linear";
                return GetInfo_Array(xmlDoc.GetElementsByTagName("Linear").Item(0));
            }
            if (xmlDoc.GetElementsByTagName("CyclicCollapse").Count != 0)
            {
                Model = "Cyclic Collapse";
                return GetInfo_Array(xmlDoc.GetElementsByTagName("CyclicCollapse").Item(0));
            }
            if (xmlDoc.GetElementsByTagName("CyclicExpansion").Count != 0)
            {
                Model = "Cyclic Expansion";
                return GetInfo_Array(xmlDoc.GetElementsByTagName("CyclicExpansion").Item(0));
            }
            if (xmlDoc.GetElementsByTagName("CyclicCurv").Count != 0)
            {
                Model = "Cyclic Curv";
                return GetInfo_Array(xmlDoc.GetElementsByTagName("CyclicCurv").Item(0));
            }
            else return new double[1];
        }

        static private double[] GetInfo_Array(XmlNode BasicParams)
        {
            double[] result;

            List<double> tmpResult = new List<double>();

            XmlNodeList InnerParams = BasicParams.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                tmpResult.Add(ToDouble(InnerParams[i].InnerText));
            }

            result = new double[tmpResult.Count];

            for (int i = 0; i < tmpResult.Count; i++)
            {
                result[i] = tmpResult[i];
            }

            return result;
        }

        static public string Create_EquityXML(Project.EquityData data)
        {
            string result = "<EquityData>";

            result += "<Amount>" + data.Amount + "</Amount>";
            result += "<IncrRate>" + data.Appreciation + "</IncrRate>";
            result += "<HasSchedule>" + data.HasSchedule + "</HasSchedule>";
            result += "<Interest>" + data.Interest + "</Interest>";
            result += "<Schedule>";

            if(data.HasSchedule)
            {
                for (int i = 0; i < data.Schedule.Length; i++)
                {
                    result += "<Month" + (i + 1).ToString() + ">" + data.Schedule[i].ToString() + "</Month" + (i + 1).ToString() + ">";
                }
            }

            result += "</Schedule>";

            result += "</EquityData>";

            return result;
        }

        static public Project.EquityData GetData_FromEquityXML(string strXML)
        {
            Project.EquityData result = new Project.EquityData();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNodeList BasicParams = xmlDoc.GetElementsByTagName("EquityData");
            XmlNode BasicParam = BasicParams.Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            XmlNode tmpStr = InnerParams[0];

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("Amount")) result.Amount = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("IncrRate")) result.Appreciation = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("HasSchedule")) result.HasSchedule = Convert.ToBoolean(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Interest")) result.Interest = ToDouble(InnerParams[i].InnerText);
                if(InnerParams[i].Name.Equals("Schedule")) tmpStr = InnerParams[i];
            }

            if(result.HasSchedule)
            {
                result.Schedule = new double[tmpStr.ChildNodes.Count];
                int count = 0;

                foreach(XmlNode SubParam in tmpStr.ChildNodes)
                {
                    result.Schedule[count] = ToDouble(SubParam.InnerText);
                    count++;
                }
            }

            return result;
        }

        static public string Create_LoanXML(List<Project.Loan> loan)
        {
            string result = "<Loans>";
            int Adj = 0;

            for (int i = 0; i < loan.Count; i++)
            {
                if(loan[i].Nominal == 0)
                {
                    Adj++;
                    goto NextLoan;
                }
                result += "<Loan" + (i - Adj) + ">";
                result += "<Nominal>" + loan[i].Nominal + "</Nominal>";
                result += "<Start>" + loan[i].Start + "</Start>";
                result += "<Tenor>" + loan[i].Tenor + "</Tenor>";
                result += "<GP>" + loan[i].GracePeriod + "</GP>";
                result += "<Linear>" + loan[i].LinearWithdraw + "</Linear>";
                result += "<Rate>" + loan[i].Rate + "</Rate>";
                result += "<UF>" + loan[i].UpfrontFee + "</UF>";
                result += "<CommitFee>" + loan[i].CommitmentFee + "</CommitFee>";
                result += "<Type>" + loan[i].RateType + "</Type>";
                result += "<Frequency>" + loan[i].Frequency + "</Frequency>";
                result += "<Bullet>" + loan[i].IsBullet + "</Bullet>";
                result += "</Loan" + (i - Adj) + ">";
                NextLoan:
                int j = 0;
            }

            result += "</Loans>";

            return result;
        }

        static public List<Project.Loan> GetData_FromLoanXML(string strXML)
        {
            List<Project.Loan> result = new List<Project.Loan>();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNodeList BasicParams;

            BasicParams = xmlDoc.GetElementsByTagName("Loans");

            foreach (XmlNode SubBasicParam in BasicParams)
            {
                foreach(XmlNode LoanData in SubBasicParam)
                {
                    XmlNodeList InnerParams = LoanData.ChildNodes;

                    Project.Loan loan = new Project.Loan();

                    for (int i = 0; i < InnerParams.Count; i++)
                    {
                        if (InnerParams[i].Name.Equals("Nominal")) loan.Nominal = ToDouble(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("Start")) loan.Start = Convert.ToInt32(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("Tenor")) loan.Tenor = ToDouble(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("GP")) loan.GracePeriod = ToDouble(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("Linear")) loan.LinearWithdraw = Convert.ToBoolean(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("Rate")) loan.Rate = ToDouble(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("UF")) loan.UpfrontFee = ToDouble(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("CommitFee")) loan.CommitmentFee = ToDouble(InnerParams[i].InnerText);
                        if (InnerParams[i].Name.Equals("Type"))
                        {
                            switch (InnerParams[i].InnerText)
                            {
                                case "Fixed":
                                    loan.RateType = Project.rateType.Fixed;
                                    break;
                                case "Variable":
                                    loan.RateType = Project.rateType.Variable;
                                    break;
                            }
                        }
                        if (InnerParams[i].Name.Equals("Frequency"))
                        {
                            switch (InnerParams[i].InnerText)
                            {
                                case "Quarterly":
                                    loan.Frequency = Project.Frequency.Quarterly;
                                    break;
                                case "Semi Annually":
                                    loan.Frequency = Project.Frequency.SemiAnnually;
                                    break;
                                case "SemiAnnually":
                                    loan.Frequency = Project.Frequency.SemiAnnually;
                                    break;
                                case "Annually":
                                    loan.Frequency = Project.Frequency.Annually;
                                    break;
                            }
                        }
                        if (InnerParams[i].Name.Equals("Bullet")) loan.IsBullet = Convert.ToBoolean(InnerParams[i].InnerText);
                    }

                    result.Add(loan);
                }
            }

            return result;
        }

        static public string Create_CoinXML(Project.Coin coin)
        {
            string result = "<Coin>";

            result += "<Nominal>" + coin.Nominal + "</Nominal>";
            result += "<Tenor>" + coin.DebtTenor + "</Tenor>";
            result += "<RateCP>" + coin.RateDuringCP + "</RateCP>";
            result += "<RateAfterCP>" + coin.RateAfterCP + "</RateAfterCP>";
            result += "<Conversion>" + coin.Conversion + "</Conversion>";
            result += "<Frequency>" + coin.Frequency + "</Frequency>";
            result += "<IssuanceCost>" + coin.IssuanceCost + "</IssuanceCost>";
            result += "<Bullet>" + coin.IsBullet + "</Bullet>";
            result += "<PayEquity>" + coin.PayEqtyInterestDuringCP + "</PayEquity>";
            result += "<PortionEquity>" + coin.PortionPayingEqty + "</PortionEquity>";
            result += "<LastDividend>" + coin.LastDividend + "</LastDividend>";
            result += "<MinDivIncrease>" + coin.MinDivIncrease + "</MinDivIncrease>";

            result += "</Coin>";

            return result;
        }

        static public Project.Coin GetData_FromCoinXML(string strXML)
        {
            Project.Coin result = new Project.Coin();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);
            
            XmlNode BasicParam = xmlDoc.GetElementsByTagName("Coin").Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("Nominal")) result.Nominal = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Tenor")) result.DebtTenor = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("RateCP")) result.RateDuringCP = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("RateAfterCP")) result.RateAfterCP = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Conversion")) result.Conversion = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Frequency"))
                {
                    switch (InnerParams[i].InnerText)
                    {
                        case "Quarterly":
                            result.Frequency = Project.Frequency.Quarterly;
                            break;
                        case "Semi Annually":
                            result.Frequency = Project.Frequency.SemiAnnually;
                            break;
                        case "SemiAnnually":
                            result.Frequency = Project.Frequency.SemiAnnually;
                            break;
                        case "Annually":
                            result.Frequency = Project.Frequency.Annually;
                            break;
                    }
                }
                if (InnerParams[i].Name.Equals("IssuanceCost")) result.IssuanceCost = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Bullet")) result.IsBullet = Convert.ToBoolean(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("PayEquity")) result.PayEqtyInterestDuringCP = Convert.ToBoolean(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("PortionEquity")) result.PortionPayingEqty = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("LastDividend")) result.LastDividend = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("MinDivIncrease")) result.MinDivIncrease = ToDouble(InnerParams[i].InnerText);
            }

            return result;
        }

        static public string Create_SGAXML(Project.SGA sga)
        {
            string result = "<SGA>";

            result += "<Rate>" + sga.Rate + "</Rate>";
            result += "<Type>" + sga.Type + "</Type>";
            result += "<MinCost>" + sga.MinCost + "</MinCost>";

            result += "</SGA>";

            return result;
        }

        static public Project.SGA GetData_FromSGAXML(string strXML)
        {
            Project.SGA result = new Project.SGA();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);
            
            XmlNode BasicParam = xmlDoc.GetElementsByTagName("SGA").Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("Rate")) result.Rate = ToDouble(InnerParams[i].InnerText);
                if (InnerParams[i].Name.Equals("Type"))
                {
                    switch(InnerParams[i].InnerText)
                    {
                        case "Total Revenues":
                            result.Type = Project.SGAType.TotalRevenues;
                            break;
                        case "Gross Income":
                            result.Type = Project.SGAType.GrossIncome;
                            break;
                    }
                }
                if (InnerParams[i].Name.Equals("MinCost")) result.MinCost = ToDouble(InnerParams[i].InnerText);
            }

            return result;
        }

        static public string Create_ModelParamXML(string Model, object Params)
        {
            switch (Model)
            {
                case "Cyclic Collapse":
                    return Create_XMLFromMatrix((double[])Params, Model.Replace(" ", ""));
                case "Cyclic Curv":
                    return Create_XMLFromMatrix((double[])Params, Model.Replace(" ", ""));
                case "Cyclic Expansion":
                    return Create_XMLFromMatrix((double[])Params, Model.Replace(" ", ""));
                case "Linear":
                    string result = "<" + Model + ">";
                    result += "<Rate>" + (double)Params + "</Rate>";
                    result += "</" + Model + ">";
                    return result;
                case "Multi-Linear":
                    return Create_XMLFromMatrix((double[,])Params, Model);
                case "Stepped":
                    return Create_XMLFromMatrix((double[,])Params, Model);
                default:
                    return "";
            }
        }

        static public string Create_ParametersXML(string FXModel, Hashtable OtherParams)
        {
            string result = "<Parameters>";
            result += "<FXModel>" + FXModel + "</FXModel>";

            foreach (string key in OtherParams.Keys)
            {
                result += "<" + key + ">" + OtherParams[key].ToString() + "</" + key + ">";
            }

            return result + "</Parameters>";
        }

        static public Hashtable GetData_FromParametersXML(string strXML, ref string FXModel)
        {
            Hashtable result = new Hashtable();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNode BasicParam = xmlDoc.GetElementsByTagName("Parameters").Item(0);

            XmlNodeList InnerParams = BasicParam.ChildNodes;

            for (int i = 0; i < InnerParams.Count; i++)
            {
                if (InnerParams[i].Name.Equals("FXModel")) FXModel = InnerParams[i].InnerText;
                else
                {
                    result[InnerParams[i].Name] = InnerParams[i].InnerText;
                }
            }

            return result;
        }

        static public double ToDouble(string data)
        {
            try
            {
                data.Replace(",", ".");
                double tmp = Convert.ToDouble(data);
                if (double.IsNaN(tmp) || double.IsInfinity(tmp)) return 0.0;
                else return tmp;
            }
            catch
            { return 0.0; }
        }
    }
}
