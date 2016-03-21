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
        ConnectionState Connect(string hostName, string userName, string password, string database);

        bool InsertData(List<string> columns, List<string> values, string table);
        List<string> ReadDataByUniqueID(int uniqueID, List<string> columns, string table);
        bool UpdataDataByUniqueID(int uniqueID, List<string> columns, List<string> values, string table);
        bool DeleteDataByUniqueID(int uniqueID, string table);
    }
}
