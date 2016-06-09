using IANTProtocol;
using Photon.SocketServer;
using System;
using System.Collections.Generic;

namespace IANTServer.OperationHandlers
{
    class TakeCakeHandler : OperationHandler
    {
        public TakeCakeHandler(Peer peer) : base(peer)
        { }
        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 0;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if (base.Handle(operationRequest))
            {
                TimeSpan duration = DateTime.Now - peer.Player.LastTakeCakeTime;
                int cakeCount = 0;
                DateTime lastTime = DateTime.Now;
                if(duration.TotalMinutes >= 5*10)
                {
                    cakeCount = 10;
                }
                else if(duration.TotalMinutes < 0)
                {
                    cakeCount = 0;
                }
                else
                {
                    cakeCount = ((int)duration.TotalMinutes / 5);
                }
                peer.Player.GainCake(cakeCount, lastTime);
                SendResponse(operationRequest.OperationCode, new Dictionary<byte, object>()
                {
                    { (byte)TakeCakeResponseParameterCode.CakeCount, cakeCount },
                    { (byte)TakeCakeResponseParameterCode.LastTakeCakeTime, lastTime.ToBinary() }
                });
                return true;
            }
            else
            {
                SendError(operationRequest.OperationCode, StatusCode.InvalidParameter, "參數錯誤");
                return false;
            }
        }
    }
}
