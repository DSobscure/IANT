using Photon.SocketServer;
using System.Xml.Serialization;
using IANTProtocol;
using IANTLibrary;
using System.IO;
using System.Collections.Generic;

namespace IANTServer.OperationHandlers
{
    class GetConfigurationsHandler : OperationHandler
    {
        public GetConfigurationsHandler(Peer peer) : base(peer)
        { }

        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 0;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                if(File.Exists("config/TowerUpgrade.config"))
                {
                    SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                    {
                        { (byte)GetConfigurationsResponseParameterCode.TowerUpgradeConfigurationXMLString, File.ReadAllText("config/TowerUpgrade.config")}
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
