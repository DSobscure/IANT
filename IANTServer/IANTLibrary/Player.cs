using System.Collections.Generic;

namespace IANTLibrary
{
    public struct PlayerProperties
    {
        public int level;
        public int experiencePoints;
        public int honorPoints;
        public List<Food> foods;
        public List<Nest> nests;
    }
    public class Player
    {
        public int UniqueID { get; }
        protected PlayerProperties properties;
        public int Level { get { return properties.level; } protected set { properties.level = value; } }
        public int ExperiencePoints { get { return properties.experiencePoints; } protected set { properties.experiencePoints = value; } }
        public int UpgradeExperiencePoints { get { return Level * 100; } }
        public int HonorPoints { get { return properties.honorPoints; } protected set { properties.honorPoints = value; } }
        public List<Food> Foods { get { return properties.foods; } protected set { properties.foods = value; } }
        public List<Nest> Nests { get { return properties.nests; } protected set { properties.nests = value; } }

        public Player(int uniqueID, PlayerProperties properties)
        {
            UniqueID = uniqueID;
            this.properties = properties;
        }
    }
}
