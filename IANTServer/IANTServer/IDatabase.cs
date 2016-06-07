using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IANTServer
{
    public interface IDatabase : IDisposable
    {
        bool Connect(string hostName, string userName, string password, string database);

        bool InsertData(string[] columns, object[] values, string table);
        string[] ReadDataByUniqueID(int uniqueID, string[] columns, string table);
        object[] GetDataByUniqueID(int uniqueID, string[] requestItems, TypeCode[] requestTypes, string table);
        bool UpdataDataByUniqueID(int uniqueID, List<string> columns, List<string> values, string table);
        bool DeleteDataByUniqueID(int uniqueID, string table);
        bool ContainsPlayer(long facebookID, out int uniqueID);
    }
}
