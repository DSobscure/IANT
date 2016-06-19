using System;
using System.Collections.Generic;
using IANTServer.OperationHandlers;
using IANTLibrary;

namespace IANTServer
{
    public interface IDatabase : IDisposable
    {
        bool Connect(string hostName, string userName, string password, string database);

        bool InsertData(string[] columns, object[] values, string table);
        string[] ReadDataByUniqueID(int uniqueID, string[] columns, string table, string joinCondition = "");
        object[] GetDataByUniqueID(int uniqueID, string[] requestItems, TypeCode[] requestTypes, string table, string joinCondition = "");
        bool UpdataDataByUniqueID(int uniqueID, List<string> columns, List<string> values, string table);
        bool UpdataDataByUniqueID(int uniqueID, string[] columns, object[] values, string table);
        bool UpdataDataByID(int uniqueID, string[] columns, object[] values, string table, string IDName);
        bool DeleteDataByUniqueID(int uniqueID, string table);
        bool ContainsPlayer(long facebookID, out int uniqueID);
        bool Register(long facebookID);
        Dictionary<long, ChallengePlayerInfo> FetchRandomNPlayerInfo(int n);
        ChallengePlayerInfo FetchPlayerInfoWithFacebookID(long facebookID);
        Nest GetNest(long facebookID);
        string GetDefenceDataString(long facebookID);
        Dictionary<long, HarvestPlayerInfo> FetchRandomNHarvestPlayerInfo(int n);
        HarvestPlayerInfo FetchHarvestPlayerInfoWithFacebookID(long facebookID);
    }
}
