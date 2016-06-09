using IANTProtocol;
using Photon.SocketServer;
using System;
using System.Collections.Generic;

namespace IANTServer.OperationHandlers
{
    class StartGameHandler : OperationHandler
    {
        public StartGameHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 1;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                int usedCakeNumber = (int)operationRequest.Parameters[(byte)StartGameParameterCode.UsedCakeNumber];
                if (peer.Player.UseCake(usedCakeNumber))
                {
                    SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                    {
                        { (byte)StartGameResponseParameterCode.UsedCakeNumber, usedCakeNumber }
                    });
                    return true;
                }
                else
                {
                    SendError(operationRequest.OperationCode, StatusCode.PermissionDeny, "蛋糕不夠");
                    return false;
                }
            }
            else
            {
                SendError(operationRequest.OperationCode, StatusCode.InvalidParameter, "參數錯誤");
                return false;
            }
        }
    }
}
