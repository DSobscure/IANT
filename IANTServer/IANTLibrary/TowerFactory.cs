﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace IANTLibrary
{
    public abstract class TowerFactory
    {
        protected Game game;
        protected Tower towerPerfab;
        protected float leastTowerSpan;
        protected Dictionary<int, Tower> towerDictionary;
        public int NextTowerCost
        {
            get
            {
                return (int)(100 + 50 * Math.Pow(towerDictionary.Count, 1.6));
            }
        }

        public event Action<int> OnBuildTowerCostChange;

        public TowerFactory(Tower towerPerfab, float leastTowerSpan, Game game)
        {
            this.towerPerfab = towerPerfab;
            this.leastTowerSpan = leastTowerSpan;
            this.game = game;
            towerDictionary = new Dictionary<int, Tower>();
        }
        public bool IsPositionLegal(float x, float y)
        {
            bool towerBetweenLegal = !towerDictionary.Values.Any(tower => tower.DistanceFrom(x, y) < leastTowerSpan);
            bool foodPlateLegal = Math.Sqrt(Math.Pow(x - game.FoodPlatePositionX, 2) + Math.Pow(y - game.FoodPlatePositionY, 2)) > game.FoodPlateRadius + 40f;
            bool nestLegal = Math.Sqrt(Math.Pow(x - game.NestPositionX, 2) + Math.Pow(y - game.NestPositionY, 2)) > game.NestRadius + 40f;
            return towerBetweenLegal && foodPlateLegal && nestLegal;
        }
        public virtual bool BuildTower(float positionX, float positionY, Game game, out Tower tower, out string errorMessage)
        {
            tower = null;
            errorMessage = "";
            if(!IsPositionLegal(positionX, positionY))
            {
                errorMessage = "塔與塔之間的距離太近了!";
                return false;
            }
            else if(game.Money < NextTowerCost)
            {
                errorMessage = "金錢不足!";
                return false;
            }
            else
            {
                tower = towerPerfab.Duplicate();
                tower.Locate(positionX, positionY);
                game.Money -= NextTowerCost;
                towerDictionary.Add(tower.UniqueID, tower);
                OnBuildTowerCostChange?.Invoke(NextTowerCost);
                return true; ;
            }
        }
        public virtual bool DestroyTower(Tower tower, Game game)
        {
            if(towerDictionary.ContainsKey(tower.UniqueID))
            {
                towerDictionary.Remove(tower.UniqueID);
                game.Money += tower.DestroyReturn;
                OnBuildTowerCostChange?.Invoke(NextTowerCost);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AimOnTarget(Ant ant)
        {
            foreach(Tower tower in towerDictionary.Values)
            {
                tower.AimOnTarget(ant);
            }
        }
        public string SerializeTowers()
        {
            List<TowerProperties> towers = towerDictionary.Values.Select(x => x.Properties).ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<TowerProperties>));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, towers);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        public void LoadTowers(string serializationString)
        {
            if(serializationString != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TowerProperties>));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(serializationString)))
                {
                    List<TowerProperties> towers = serializer.Deserialize(ms) as List<TowerProperties>;
                    foreach (TowerProperties properties in towers)
                    {
                        Tower tower = towerPerfab.Instantiate(properties);
                        towerDictionary.Add(tower.UniqueID, tower);
                    }
                }
            }
        }
    }
}
