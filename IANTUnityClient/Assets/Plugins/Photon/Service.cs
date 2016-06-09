using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PhotonService : IPhotonPeerListener
{
    private PhotonPeer peer;
    public bool ServerConnected { get; private set; }

    public PhotonService()
    {
        peer = null;
        ServerConnected = false;
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        IANTGame.InformManager.SystemInformManager.CallDebugReturn(message);
    }

    public void OnEvent(EventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        IANTGame.ResponseManager.Operate(operationResponse);
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                DebugReturn( DebugLevel.INFO,"establishing connect....");
                peer.EstablishEncryption();
                break;
            case StatusCode.Disconnect:
                peer = null;
                ServerConnected = false;
                IANTGame.ResponseManager.SystemResponseManager.CallConnectResponse(false);
                break;
            case StatusCode.EncryptionEstablished:
                ServerConnected = true;
                IANTGame.ResponseManager.SystemResponseManager.CallConnectResponse(true);
                break;
        }
    }

    public void Connect()
    {
        try
        {
            string serverAddress = IANTGame.ServerAddress;
#if UNITY_WEBGL && !UNITY_EDITOR
            peer = new PhotonPeer(this, ConnectionProtocol.WebSocketSecure);
            peer.SocketImplementation = typeof(SocketWebTcp);
            serverAddress = "wss://" + serverAddress + ":" + IANTGame.WebSocketPort;
#else
            peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            serverAddress += ":" + IANTGame.UdpPort;
#endif
            if (!peer.Connect(serverAddress, IANTGame.ServerName))
            {
                IANTGame.Service.DebugReturn(DebugLevel.ERROR, "Connect fail in peer.Connect");
                IANTGame.ResponseManager.SystemResponseManager.CallConnectResponse(false);
            }
        }
        catch (Exception ex)
        {
            IANTGame.ResponseManager.SystemResponseManager.CallConnectResponse(false);
            IANTGame.Service.DebugReturn(DebugLevel.ERROR, ex.Message);
            IANTGame.Service.DebugReturn(DebugLevel.ERROR, ex.StackTrace);
        }
    }

    public void Disconnect()
    {
        try
        {
            peer.Disconnect();
        }
        catch (Exception ex)
        {
            DebugReturn(DebugLevel.ERROR, ex.Message);
            DebugReturn(DebugLevel.ERROR, ex.StackTrace);
        }
    }

    public void Service()
    {
        try
        {
            peer.Service();
        }
        catch (Exception ex)
        {
            DebugReturn(DebugLevel.ERROR, ex.Message);
            DebugReturn(DebugLevel.ERROR, ex.StackTrace);
        }
    }
    public void SendOperation(IANTProtocol.OperationCode oprationCode, Dictionary<byte, object> parameter)
    {
        if(peer.IsEncryptionAvailable)
        {
            peer.OpCustom((byte)oprationCode, parameter, true, 0, true);
        }
        else
        {
            DebugReturn(DebugLevel.ERROR, "communication still not establish encryption");
        }
    }
}
