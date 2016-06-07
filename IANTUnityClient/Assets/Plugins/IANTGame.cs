using Managers;
using IANTLibrary;
using UnityEngine;
using System.Collections.Generic;

public static class IANTGame
{
    public static readonly PhotonService Service = new PhotonService();
    public static readonly ActionManager ActionManager = new ActionManager();
    public static readonly ResponseManager ResponseManager = new ResponseManager();
    public static readonly InformManager InformManager = new InformManager();
    public static readonly string ServerName = "IANTServer";
    public static readonly string ServerAddress = "doorofsoul.duckdns.org";
    public static readonly string UdpPort = "5055";
    public static readonly string WebSocketPort = "19090";

    public static Game Game;
    public static Player Player;
    public static bool IsLoadedConfigurations = false;
    public static string FacebookID = null;
    public static string FacebookAccessToken = null;
    public static IDictionary<string, object> ProfileResult = null;
    public static Texture2D ProfilePicture = null;
}
