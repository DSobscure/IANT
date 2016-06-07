using System.Collections.Generic;
using System.Linq;

namespace IANTLibrary
{
    public struct PlayerProperties
    {
        public long facebookID;
        public int level;
        public int exp;
        public List<FoodInfo> foodInfos;
        public List<Nest> nests;
    }
    public class Player
    {
        public int UniqueID { get; }
        protected PlayerProperties properties;
        public long FacebookID { get { return properties.facebookID; } protected set { properties.facebookID = value; } }
        public int Level { get { return properties.level; } protected set { properties.level = value; } }
        public int EXP { get { return properties.exp; } protected set { properties.exp = value; } }
        public List<FoodInfo> FoodInfos { get { return properties.foodInfos.ToList(); } protected set { properties.foodInfos = value; } }
        public List<Nest> Nests { get { return properties.nests; } protected set { properties.nests = value; } }

        public Player(int uniqueID, PlayerProperties properties)
        {
            UniqueID = uniqueID;
            this.properties = properties;
        }
    }
}
