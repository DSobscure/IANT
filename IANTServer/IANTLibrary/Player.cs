using System.Collections.Generic;
using System.Linq;
using System;

namespace IANTLibrary
{
    public struct PlayerProperties
    {
        public long facebookID;
        public int level;
        public int exp;
        public List<FoodInfo> foodInfos;
        public List<Nest> nests;
        public DateTime lastTakeCakeTime;
    }
    public class Player
    {
        public int UniqueID { get; }
        protected PlayerProperties properties;
        public long FacebookID { get { return properties.facebookID; } protected set { properties.facebookID = value; } }
        public int Level
        {
            get { return properties.level; }
            set
            {
                properties.level = value;
                OnLevelChange?.Invoke(Level);
            }
        }
        public int EXP
        {
            get { return properties.exp; }
            set
            {
                properties.exp = value;
                if(LevelEXPTable.IsUpgrade(Level, value))
                {
                    Level++;
                    LevelEXPTable.GetLevel(Level, out properties.exp);
                }
                OnEXPChange?.Invoke(EXP);
            }
        }
        public List<FoodInfo> FoodInfos { get { return properties.foodInfos.ToList(); } protected set { properties.foodInfos = value; } }
        public List<Nest> Nests { get { return properties.nests; } protected set { properties.nests = value; } }
        public DateTime LastTakeCakeTime { get { return properties.lastTakeCakeTime; } protected set { properties.lastTakeCakeTime = value; } }
        public int CakeCount
        {
            get { return (properties.foodInfos.Count > 0) ? properties.foodInfos.First(x => x.food is Cake).count : 0; }
            set
            {
                if (FoodInfos.Count > 0)
                {
                    int index = FoodInfos.FindIndex(x => x.food is Cake);
                    if (index != -1)
                    {
                        properties.foodInfos[index] = new FoodInfo
                        {
                            food = new Cake(),
                            count = value
                        };
                    }
                    OnCakeCountChange?.Invoke(CakeCount);
                }
            }
        }

        public Action<int> OnCakeCountChange;
        public Action<int> OnLevelChange;
        public Action<int> OnEXPChange;

        public Player(int uniqueID, PlayerProperties properties)
        {
            UniqueID = uniqueID;
            this.properties = properties;
        }
        public void GainCake(int cakeCount, DateTime lastTakeCakeTime)
        {
            if(FoodInfos.Count > 0)
            {
                int index = FoodInfos.FindIndex(x => x.food is Cake);
                if(index != -1)
                {
                    properties.foodInfos[index] = new FoodInfo
                    {
                        food = new Cake(),
                        count = FoodInfos[index].count + cakeCount
                    };
                }
                LastTakeCakeTime = lastTakeCakeTime;
                OnCakeCountChange?.Invoke(CakeCount);
            }
        }
        public bool UseCake(int number)
        {
            if (CakeCount >= number && FoodInfos.Count > 0)
            {
                int index = FoodInfos.FindIndex(x => x.food is Cake);
                if (index != -1)
                {
                    properties.foodInfos[index] = new FoodInfo
                    {
                        food = new Cake(),
                        count = FoodInfos[index].count - number
                    };
                }
                OnCakeCountChange?.Invoke(CakeCount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
