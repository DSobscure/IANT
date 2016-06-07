using Photon.SocketServer;
using System.Net;
using System.IO;
using IANTProtocol;
using System.Collections.Generic;
using System;

namespace IANTServer.OperationHandlers
{
    class LoginHandler : OperationHandler
    {
        public LoginHandler(Peer peer) : base(peer)
        { }

        public override bool CheckParameter(OperationRequest operationRequest)
        {
            return operationRequest.Parameters.Count == 2;
        }
        public override bool Handle(OperationRequest operationRequest)
        {
            if(base.Handle(operationRequest))
            {
                string facebookID = (string)operationRequest.Parameters[(byte)LoginParameterCode.FacebookUserID];
                string accessToken = (string)operationRequest.Parameters[(byte)LoginParameterCode.AccessToken];
                WebClient webClient = new WebClient();
                Stream data = null;
                StreamReader reader = null;
                string responseText;

                try
                {
                    data = webClient.OpenRead(string.Format("https://graph.facebook.com/{0}/permissions?access_token={1}", facebookID, accessToken));
                    reader = new StreamReader(data);
                    responseText = reader.ReadToEnd();
                    Application.Log.Info(responseText);
                    data?.Close();
                    reader?.Close();
                    ServerPlayer player = Application.ServerInstance.InstantiatePlayerWithFacebookID(Convert.ToInt64(facebookID), peer);
                    peer.BindPlayer(player);
                    Application.ServerInstance.PlayrOnline(player);
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>()
                    {
                        { (byte)LoginResponseParameterCode.UniqueID, player.UniqueID },
                        { (byte)LoginResponseParameterCode.Level, player.Level },
                        { (byte)LoginResponseParameterCode.EXP, player.EXP}
                    };
                    SendResponse(operationRequest.OperationCode, parameter);
                    return true;
                }
                catch (WebException exception)
                {
                    using (var readerEX = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        responseText = readerEX.ReadToEnd();
                        Application.Log.Info(responseText);
                    }
                    data?.Close();
                    reader?.Close();
                    SendError(operationRequest.OperationCode, StatusCode.PermissionDeny, "id或access token錯誤");
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
