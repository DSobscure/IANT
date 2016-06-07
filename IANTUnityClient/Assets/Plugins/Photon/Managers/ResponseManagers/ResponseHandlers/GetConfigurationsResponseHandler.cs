using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using IANTProtocol;
using IANTLibrary;
using System.IO;
using System.Xml.Serialization;
using System.Text;

public class GetConfigurationsResponseHandler : ResponseHandler
{
    public override bool Handle(OperationResponse operationResponse)
    {
        if (base.Handle(operationResponse))
        {
            string towerUpgradeConfigurationXmlString = (string)operationResponse.Parameters[(byte)GetConfigurationsResponseParameterCode.TowerUpgradeConfigurationXMLString];
            XmlSerializer serializer = new XmlSerializer(typeof(TowerUpgradeConfigurationContent));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(towerUpgradeConfigurationXmlString ?? "")))
            {
                IANTGame.ResponseManager.FetchDataResponseManager.CallGetConfigurationsResponse((TowerUpgradeConfigurationContent)serializer.Deserialize(ms));
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool CheckError(OperationResponse operationResponse)
    {
        OperationCode operationCode = (OperationCode)operationResponse.OperationCode;
        if (operationResponse.ReturnCode != (short)IANTProtocol.StatusCode.Correct)
        {
            return false;
        }
        if (operationResponse.Parameters.Count != 1)
        {
            return false;
        }
        return true;
    }
}
