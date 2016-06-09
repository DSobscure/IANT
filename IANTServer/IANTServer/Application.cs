using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using IANTLibrary;
using log4net.Config;
using Photon.SocketServer;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IANTServer
{
    public class Application : ApplicationBase
    {
        private static Application instance;
        public static Application ServerInstance
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
            return new Peer(initRequest);
        }

        protected override void Setup()
        {
            instance = this;
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
            if (database.Connect("localhost", "root", "IANT", "iant"))
            {
                Log.Info("Database Connect successiful!.......");
            }
            else
            {
                Log.Error("Database connect fail");
            }
            Log.Info("Server Setup successiful!.......");
            TowerUpgradeConfiguration.Load();
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

        public ServerPlayer InstantiatePlayerWithFacebookID(long facebookID, Peer peer)
        {
            int uniqueID;
            if(database.ContainsPlayer(facebookID, out uniqueID))
            {
                string[] requestItems = new string[]
                {
                    "Level","EXP","LastTakeCakeTime","CakeCount","Duration","Speed","Resistant","Population","Sensitivity"
                };
                TypeCode[] requestTypes = new TypeCode[]
                {
                    TypeCode.Int32, TypeCode.Int32, TypeCode.DateTime, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32
                };
                object[] results = database.GetDataByUniqueID(uniqueID, requestItems, requestTypes, "player,nest", "player.UniqueID=nest.PlayerID");
                return new ServerPlayer(uniqueID, new PlayerProperties
                {
                    facebookID = facebookID,
                    level = (int)results[0],
                    exp = (int)results[1],
                    foodInfos = new List<FoodInfo>()
                    {
                        new FoodInfo { food = new Cake(), count =  (int)results[3]}
                    },
                    nests = new List<Nest>()
                    {
                        new Nest(new AntGrowthProperties
                        {
                            duration = (int)results[4],
                            speed = (int)results[5],
                            resistant = (int)results[6],
                            population = (int)results[7],
                            sensitivity = (int)results[8]
                        })
                    },
                    lastTakeCakeTime = (DateTime)results[2]
                }, peer);
            }
            else
            {
                if(database.Register(facebookID))
                {
                    return InstantiatePlayerWithFacebookID(facebookID, peer);
                }
                else
                {
                    return null;
                }
            }
        }
        public bool PlayrOnline(Player player)
        {
            return playerManager.RegisterPlayer(player);
        }
        public bool PlayerOffline(Player player)
        {
            if(!database.UpdataDataByUniqueID(player.UniqueID, 
                new string[] 
                {
                    "Level","EXP","LastTakeCakeTime","CakeCount"
                }, 
                new object[]
                {
                    player.Level, player.EXP, player.LastTakeCakeTime, (player.FoodInfos.Count > 0) ? player.FoodInfos.First(x => x.food is Cake).count : 0
                }, "player"))
            {
                Log.ErrorFormat("error when player offline save player data to database");
            }
            AntGrowthProperties properties = player.Nests[0].GrowthProperties;
            if (!database.UpdataDataByID(player.UniqueID,
                new string[]
                {
                    "Duration","Speed","Resistant","Population","Sensitivity"
                },
                new object[]
                {
                    properties.duration, properties.speed, properties.resistant, properties.population, properties.sensitivity
                }, "nest", "PlayerID"))
            {
                Log.ErrorFormat("error when player offline save nest data to database");
            }
            return playerManager.ErasePlayer(player);
        }
    }
}
