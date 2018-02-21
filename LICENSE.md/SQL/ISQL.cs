using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SQL
{
    [Guid("44d77967-2784-432d-9642-e8fe1ead19f5")]
    interface ISQL
    {
        void Connect();
        void Disconnect();
        List<SQLFunc.SubResults> GetData(string TableName, string Data2Get, string Where = "");
        List<SQLFunc.SubResults> Get_TblFields(string TableName);
        SQLFunc.SubResults Get1Data(string TableName, string Data2Get, string Where = "");
        void InsertData(string TableName, string Data2Insert);
        void UpdateTable(string TableName, string Set, string Where = "");
        void Delete(string TableName, string Where);
    }

    [Guid("eba8b0c6-379c-4ca2-96e9-5f5a3af9618d"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ISQLEvent
    {
        event EventHandler<IErrorEventArgs> Err;
    }

    public class IErrorEventArgs : EventArgs
    {
        public string Message;
        public string Source;
        public string InternalSource;

        public IErrorEventArgs(Exception Ex, string InternalSource)
        {
            this.Message = Ex.Message;
            this.Source = Ex.Source;
            this.InternalSource = InternalSource;
        }
    }
}
