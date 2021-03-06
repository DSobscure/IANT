﻿using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using IANTLibrary;
using log4net.Config;
using Photon.SocketServer;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using IANTServer.OperationHandlers;

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
                    "Level","EXP","LastTakeCakeTime","CakeCount","Duration","Speed","Resistant","Population","Sensitivity","DistributionMap1","DistributionMap2","DistributionMap3","DefenceTowersDataString","UsedDefenceBudget"
                };
                TypeCode[] requestTypes = new TypeCode[]
                {
                    TypeCode.Int32, TypeCode.Int32, TypeCode.DateTime, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.Int32, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.String, TypeCode.Int32
                };
                object[] results = database.GetDataByUniqueID(uniqueID, requestItems, requestTypes, "player,nest", "player.UniqueID=nest.PlayerID");
                ServerPlayer player = new ServerPlayer(uniqueID, new PlayerProperties
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
                    lastTakeCakeTime = (DateTime)results[2],
                    defenceDataString = (string)results[12],
                    usedDefenceBudget = (int)results[13]
                }, peer);
                if(player.DefenceDataString != null && player.DefenceDataString[0] != '<')
                {
                    player.DefenceDataString = player.DefenceDataString.Substring(1);
                }
                if(!(results[9] == null || results[10] == null || results[11] == null))
                {
                    player.Nests[0].Load3DistributionMap((string)results[9], (string)results[10], (string)results[11]);
                }
                return player;
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
            string distributionMap1, distributionMap2, distributionMap3;
            player.Nests[0].Serialize3DistributionMap(out distributionMap1, out distributionMap2, out distributionMap3);
            if (!database.UpdataDataByID(player.UniqueID,
                new string[]
                {
                    "Duration","Speed","Resistant","Population","Sensitivity","DistributionMap1","DistributionMap2","DistributionMap3"
                },
                new object[]
                {
                    properties.duration, properties.speed, properties.resistant, properties.population, properties.sensitivity, distributionMap1, distributionMap2, distributionMap3
                }, "nest", "PlayerID"))
            {
                Log.ErrorFormat("error when player offline save nest data to database");
            }
            return playerManager.ErasePlayer(player);
        }
        public ChallengePlayerInfo[] FetchNChallengePlayerInfoWithKnownFriends(int n, long[] friendsFBIDs, long selfFacebookID)
        {
            var infos = database.FetchRandomNPlayerInfo(n);
            if(infos.ContainsKey(selfFacebookID))
            {
                infos.Remove(selfFacebookID);
            }
            List<ChallengePlayerInfo> friendsInfo = new List<ChallengePlayerInfo>();
            foreach(long facebookID in friendsFBIDs)
            {
                if(!infos.ContainsKey(facebookID))
                {
                    friendsInfo.Add(database.FetchPlayerInfoWithFacebookID(facebookID));
                }
            }
            foreach(ChallengePlayerInfo info in friendsInfo)
            {
                infos.Add(info.facebookID, info);
            }
            return infos.Values.ToArray();
        }
        public Nest GetNest(long facebookID)
        {
            return database.GetNest(facebookID);
        }
        public HarvestPlayerInfo[] FetchNHarvestPlayerInfoWithKnownFriends(int n, long[] friendsFBIDs, long selfFacebookID)
        {
            var infos = database.FetchRandomNHarvestPlayerInfo(n);
            if (infos.ContainsKey(selfFacebookID))
            {
                infos.Remove(selfFacebookID);
            }
            List<HarvestPlayerInfo> friendsInfo = new List<HarvestPlayerInfo>();
            foreach (long facebookID in friendsFBIDs)
            {
                if (!infos.ContainsKey(facebookID))
                {
                    friendsInfo.Add(database.FetchHarvestPlayerInfoWithFacebookID(facebookID));
                }
            }
            foreach (HarvestPlayerInfo info in friendsInfo)
            {
                infos.Add(info.facebookID, info);
            }
            return infos.Values.ToArray();
        }
        public void SaveDefence(int uniqueID, string defenceDataString, int usedBudget)
        {
            if (!database.UpdataDataByUniqueID(uniqueID,
                new string[]
                {
                    "DefenceTowersDataString", "UsedDefenceBudget"
                },
                new object[]
                {
                    defenceDataString, usedBudget
                }, "player"))
            {
                Log.ErrorFormat("error when player saving player defence to database");
            }
        }
        public string GetDefenceDataString(long facebookID)
        {
            return database.GetDefenceDataString(facebookID);
        }
        public void SavePlayerCakeCount(int uniqueID, int cakeCount)
        {
            if (!database.UpdataDataByUniqueID(uniqueID,
                new string[]
                {
                    "CakeCount"
                },
                new object[]
                {
                    cakeCount
                }, "player"))
            {
                Log.ErrorFormat("error when player save cake count to database");
            }
            playerManager.InformCakeNumberChange(uniqueID, cakeCount);
        }
        public void SaveHarvestResult(long defenderFacebookID, int harvestResult)
        {
            int uniqueID;
            database.ContainsPlayer(defenderFacebookID, out uniqueID);
            if(!playerManager.Harvested(uniqueID, harvestResult))
            {
                object[] result = database.GetDataByUniqueID(uniqueID, new string[]{ "CakeCount" }, new TypeCode[] { TypeCode.Int32 }, "player");
                if(result != null && result.Length == 1)
                {
                    int cakeCount = Math.Max((int)result[0] - harvestResult, 0);
                    SavePlayerCakeCount(uniqueID, cakeCount);
                }
            }
        }
    }
}
