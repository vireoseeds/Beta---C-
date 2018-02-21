using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace SQL
{
    [Guid("ad529f26-a377-4a17-9e72-428e67d7f7f1"), ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(ISQLEvent))]
    public class SQLFunc : ISQL, ISQLEvent
    {
        public event EventHandler<IErrorEventArgs> Err;
        private SqlConnection MyConn = new SqlConnection();
        private SqlDataReader MyReader;
        private string ServerAddress;
        private string Database;

        public SQLFunc(string ServerAddress, string Database)
        {
            this.ServerAddress = ServerAddress;
            this.Database = Database;
        }

        public void Connect()
        {
            try
            {
                MyConn.ConnectionString = "Server=" + ServerAddress + ";Initial Catalog=" + Database + ";Integrated Security=True;";
                this.MyConn.Open();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "Connect function"));
            }
        }

        public void Disconnect()
        {
            try
            {
                this.MyConn.Close();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "Disconnect function"));
            }
        }

        public SubResults Get1Data(string TableName, string Data2Get, string Where = "")
        {
            SubResults _Subresult = new SubResults();

            SqlCommand MyCmd = this.MyConn.CreateCommand();

            if (Where.Equals(""))
            {
                MyCmd.CommandText = "Select TOP 1 " + Data2Get + " from " + TableName;
            }
            else
            {
                MyCmd.CommandText = "Select TOP 1 " + Data2Get + " from " + TableName + " where " + Where;
            }


            try
            {
                MyReader = MyCmd.ExecuteReader();

                while (MyReader.Read())
                {
                    for (int i = 0; i < MyReader.FieldCount; i++)
                    {
                        _Subresult.Add(MyReader.GetValue(i));
                    }

                }
                MyReader.Close();
            }
            catch
            {
                if (MyReader != null) { MyReader.Close(); }
            }
            return _Subresult;
        }

        public List<SubResults> GetData(string TableName, string Data2Get, string Where = "")
        {
            List<SubResults> Results = new List<SubResults>();
            SubResults _Subresult;

            SqlCommand MyCmd = this.MyConn.CreateCommand();

            if (Where.Equals(""))
            {
                MyCmd.CommandText = "Select " + Data2Get + " from " + TableName;
            }
            else
            {
                MyCmd.CommandText = "Select " + Data2Get + " from " + TableName + " where " + Where;
            }


            try
            {
                MyReader = MyCmd.ExecuteReader();

                while (MyReader.Read())
                {
                    _Subresult = new SubResults();

                    for (int i = 0; i < MyReader.FieldCount; i++)
                    {
                        _Subresult.Add(MyReader.GetValue(i));
                    }

                    Results.Add(_Subresult);
                }
                MyReader.Close();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "GetData function in " + TableName + " on " + this.Database));
                if (MyReader != null) { MyReader.Close(); }
            }
            return Results;
        }

        public List<SubResults> Get_TblFields(string TableName)
        {
            List<SubResults> Results = new List<SubResults>();
            SubResults _Subresult;
            SqlCommand MyCmd = MyConn.CreateCommand();

            MyCmd.CommandText = "Select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + TableName + "'";

            try
            {
                MyReader = MyCmd.ExecuteReader();

                while (MyReader.Read())
                {
                    _Subresult = new SubResults();

                    for (int i = 0; i < MyReader.FieldCount; i++)
                    {
                        _Subresult.Add(MyReader.GetValue(i));
                    }

                    Results.Add(_Subresult);
                }
                MyReader.Close();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "Get_TblFields function with " + TableName + " on " + this.Database));
                if (MyReader != null) { MyReader.Close(); }
            }

            return Results;
        }

        public void InsertData(string TableName, string Data2Insert)
        {
            SqlCommand MyCmd = this.MyConn.CreateCommand();

            MyCmd.CommandText = "Insert into " + TableName + " Values (" + Data2Insert + ")";

            try
            {
                MyCmd.ExecuteNonQuery();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "InsertData function in " + TableName + " on " + this.Database));
            }
        }

        public void ClearTable(string TableName, string Where = "")
        {
            SqlCommand MyCmd = this.MyConn.CreateCommand();

            if (Where.Equals(""))
            {
                MyCmd.CommandText = "Delete from " + TableName;
            }
            else
            {
                MyCmd.CommandText = "Delete from " + TableName + " where " + Where;
            }

            try
            {
                MyCmd.ExecuteNonQuery();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "ClearTable function in " + TableName + " on " + this.Database));
            }
        }

        public void UpdateTable(string TableName, string Set, string Where = "")
        {
            SqlCommand MyCmd = this.MyConn.CreateCommand();

            if (Where.Equals(""))
            {
                MyCmd.CommandText = "Update " + TableName + " Set " + Set;
            }
            else
            {
                MyCmd.CommandText = "Update " + TableName + " Set " + Set + " Where " + Where;
            }

            try
            {
                MyCmd.ExecuteNonQuery();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "UpdateTable function in " + TableName + " on " + this.Database));
            }
        }

        public void Delete(string TableName, string Where)
        {
            SqlCommand MyCmd = this.MyConn.CreateCommand();

            MyCmd.CommandText = "Delete from " + TableName + " Where " + Where;

            try
            {
                MyCmd.ExecuteNonQuery();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "Delete function in " + TableName + " on " + this.Database));
            }
        }

        public void TransferData(string FromTable, string ToTable)
        {
            SqlCommand MyCmd = this.MyConn.CreateCommand();

            MyCmd.CommandText = "Insert into " + ToTable + " Select * from " + FromTable;

            try
            {
                MyCmd.ExecuteNonQuery();
            }
            catch (Exception MyEx)
            {
                Err(this, new IErrorEventArgs(MyEx, "Transfer function"));
            }
        }

        public class SubResults
        {
            public List<object> _SubResults;

            public SubResults()
            {
                _SubResults = new List<object>();
            }

            public void Add(object Data)
            {
                _SubResults.Add(Data);
            }


        }
    }
}
