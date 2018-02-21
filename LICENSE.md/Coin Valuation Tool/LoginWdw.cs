using System;
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

namespace Coin_Valuation_Tool
{
    public partial class LoginWdw : Form
    {
        private string ServerAddress;
        private string Database;
        private int NbMissed = 0;
        public int Result;

        public LoginWdw(string ServerAddress, string Database)
        {
            this.ServerAddress = ServerAddress;
            this.Database = Database;
            InitializeComponent();
        }

        private void validateBtn_Click(object sender, EventArgs e)
        {
            SQLFunc mySql = new SQLFunc(ServerAddress, Database);
            mySql.Err += MySql_Err;
            mySql.Connect();

            SQLFunc.SubResults myData = mySql.Get1Data("Login", "Password", "Login='" + this.loginBox.Text + "'");

            if (myData == null || myData._SubResults.Count == 0)
            {
                MessageBox.Show("Wrong Login");
                NbMissed++;
                if (NbMissed == 3)
                {
                    Random _check = new Random();
                    Result = Result = (int)Math.Round((double)_check.Next(1000), 0);
                    this.Hide();
                }
            }
            else
            {
                string pwd = (string)myData._SubResults[0];

                if (!pwd.Equals(passwordBox.Text))
                {
                    MessageBox.Show("Wrong Password");
                    NbMissed++;
                    if (NbMissed == 3)
                    {
                        Random _check = new Random();
                        Result = (int)Math.Round((double)_check.Next(1000), 0);
                        this.Hide();
                    }
                }
                else
                {
                    Result = Thread.CurrentThread.ManagedThreadId;
                    this.Hide();
                }
            }
        }

        private void MySql_Err(object sender, IErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
