using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using IANTLibrary;
using System.IO;

namespace IANTServer
{
    public class Application : ApplicationBase
    {
        private static Application instance;
        public static Application Instance
        {
            get
            {
                return instance;
            }
        }
        protected IDatabase database;
        protected PlayerManager playerManager;

        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new Peer(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] =
                Path.Combine(this.ApplicationPath, "log");

            string path = Path.Combine(this.BinaryPath, "log4net.config");
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
            playerManager = new PlayerManager();
            database = new MySQLDatabase();
            if(database.Connect("", "", "", "") == System.Data.ConnectionState.Connecting)
            {
                Log.Info("Database Connect successiful!.......");
            }
            Log.Info("Server Setup successiful!.......");
        }

        protected override void TearDown()
        {
            database?.Dispose();
        }

        public bool EstablishConnection(Peer peer)
        {
            return playerManager.RegisterConnection(peer);
        }
        public bool TerminateConnection(Peer peer)
        {
            return playerManager.EraseConnection(peer);
        }

        public bool PlayrOnline(Player player)
        {
            return playerManager.RegisterPlayer(player);
        }

        public bool PlayerOffline(Player player)
        {
            return playerManager.RegisterPlayer(player);
        }
    }
}
