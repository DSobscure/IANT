using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuralNetworkLibrary;

namespace IANTLibrary
{
    struct BlockInfo : IComparable<BlockInfo>
    {
        public int x;
        public int y;
        public float weight;

        public int CompareTo(BlockInfo obj)
        {
            return weight.CompareTo(obj.weight);
        }

        public override bool Equals(object obj)
        {
            if (obj is BlockInfo)
            {
                BlockInfo b = (BlockInfo)obj;
                return x == b.x && y == b.y;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }
    }
    public struct PositionInfo
    {
        public float x;
        public float y;
        public override bool Equals(object obj)
        {
            if (obj is PositionInfo)
            {
                PositionInfo b = (PositionInfo)obj;
                return x == b.x && y == b.y;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }
    }
    public struct AntStayInfo
    {
        public float hpRatioDelta;
        public float timeDelta;
    }
    public struct FormationInfo
    {
        public Ant ant;
        public int blockX;
        public int blockY;
    }

    public struct AntGrowthProperties
    {
        public int Level
        {
            get
            {
                return duration + speed + resistant + sensitivity + population + 1;
            }
        }
        public int duration;
        public int speed;
        public int resistant;
        public int sensitivity;
        public int population;
    }

    public class Nest
    {
        protected Game game;
        public float[,] weightsForSurvive;
        protected float hurtLearningRate { get { return 0.01f + 0.01f * GrowthProperties.sensitivity; } }
        public List<DistributionMap> distributionMaps;
        public DistributionMap distributionMap;
        private HashSet<PositionInfo> searchHelper;

        protected AntGrowthProperties growthProperties;
        public AntGrowthProperties GrowthProperties
        {
            get { return growthProperties; }
            set
            {
                growthProperties = value;
                OnGrowthPropertiesChange?.Invoke(value);
            }
        }

        public Action<AntGrowthProperties> OnGrowthPropertiesChange;

        public Nest(AntGrowthProperties growthProperties)
        {
            GrowthProperties = growthProperties;
            weightsForSurvive = new float[48, 30];
            for (int x = 0; x < 48; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    weightsForSurvive[x, y] = -0.2f;
                }
            }
            for (int x = 0; x < 48; x++)
            {
                weightsForSurvive[x, 0] = float.MinValue;
                weightsForSurvive[x, 29] = float.MinValue;
            }
            for (int y = 0; y < 30; y++)
            {
                weightsForSurvive[0, y] = float.MinValue;
                weightsForSurvive[47, y] = float.MinValue;
            }
            distributionMaps = new List<DistributionMap>();
            distributionMaps.Add(new DistributionMap());
            distributionMaps.Add(new DistributionMap());
            distributionMaps.Add(new DistributionMap());
        }
        public void BindGame(Game game)
        {
            this.game = game;
            int cakeBlockX = Convert.ToInt32(game.FoodPlatePositionX / 20) + 24;
            int cakeBlockY = Convert.ToInt32(game.FoodPlatePositionY / 20) + 15;
            int nestBlockX = Convert.ToInt32(game.NestPositionX / 20) + 24;
            int nestBlockY = Convert.ToInt32(game.NestPositionY / 20) + 15;
        }
        public float GetProperRotation(Ant ant)
        {
            Random annelingFactorGenerator = new Random(Guid.NewGuid().GetHashCode());
            double annelingFactor = annelingFactorGenerator.NextDouble() * ant.HP / ant.MaxHP * 0.6;
            if (ant.IsTakingFood)
            {
                annelingFactor *= 0.2;
            }
            int sensorDistance = 1 + GrowthProperties.sensitivity;
            int antBlockX = Convert.ToInt32(ant.PositionX / 20) + 24;
            int antBlockY = Convert.ToInt32(ant.PositionY / 20) + 15;
            int formationX, formationY;
            GetFormationIndex(ant, out formationX, out formationY);
            List<BlockInfo> blockInfos = new List<BlockInfo>();
            for (int x = -sensorDistance; x <= sensorDistance; x++)
            {
                if (antBlockX + x < 0 || x == 0 || antBlockX + x >= 48)
                    continue;
                for (int y = -sensorDistance; y <= sensorDistance; y++)
                {
                    if (antBlockY + y < 0 || y == 0 || antBlockY + y >= 30)
                        continue;
                    float extraWeight = 0;
                    if (ant.IsTakingFood)
                    {
                        if(Math.Sqrt(Math.Pow(ant.PositionX + x - game.NestPositionX, 2)+Math.Pow(ant.PositionY + y - game.NestPositionY, 2)) < Math.Sqrt(Math.Pow(ant.PositionX - game.NestPositionX, 2)+Math.Pow(ant.PositionY - game.NestPositionY, 2)))
                            extraWeight = 0.1f;
                    }
                    else if (game.FoodFactory.RemainedFoodCount > 0)
                    {
                        if (Math.Sqrt(Math.Pow(ant.PositionX + x - game.FoodPlatePositionX, 2) + Math.Pow(ant.PositionY + y - game.FoodPlatePositionY, 2)) < Math.Sqrt(Math.Pow(ant.PositionX - game.FoodPlatePositionX, 2) + Math.Pow(ant.PositionY - game.FoodPlatePositionY, 2)))
                            extraWeight = 0.1f;
                    }
                    float formationWeight = 0;
                    if(ant.IsTakingFood)
                    {
                        formationWeight = distributionMap.GetDirectionWeight(ant, game.AntFactory.GetFormationInfo(ant), x, y) / 2;
                    }
                    else
                    {
                        formationWeight = distributionMap.GetDirectionWeight(ant, game.AntFactory.GetFormationInfo(ant), x, y);
                    }
                    searchHelper = new HashSet<PositionInfo>();
                    blockInfos.Add(new BlockInfo { x = x, y = y, weight = GetLocalSurvive(antBlockX + x, antBlockY + y, 1 + GrowthProperties.sensitivity)/(1+(float)Math.Sqrt(x*x+y*y)/10) + extraWeight + formationWeight });
                }
            }
            blockInfos.Sort();
            int selectIndex = blockInfos.Count - 1 - annelingFactorGenerator.Next(Convert.ToInt32((blockInfos.Count - 1) * annelingFactor));
            BlockInfo bestBlock = blockInfos[selectIndex];
            if (bestBlock.x == 0 && bestBlock.y == 0)
                return -1;
            else
                return (Convert.ToSingle(Math.Atan2(bestBlock.y, bestBlock.x) * 180 / Math.PI + 45 * annelingFactor) + 360f) % 360f;
        }
        public void AntStay(Dictionary<PositionInfo, AntStayInfo> antStayInfos)
        {
            foreach (var antStayInfoPair in antStayInfos)
            {
                int antBlockX = Convert.ToInt32(antStayInfoPair.Key.x / 20) + 24;
                int antBlockY = Convert.ToInt32(antStayInfoPair.Key.y / 20) + 15;
                int sensorDistance = 1;
                float hpRatioDelta = antStayInfoPair.Value.hpRatioDelta / antStayInfoPair.Value.timeDelta;
                for (int x = -sensorDistance; x <= sensorDistance; x++)
                {
                    if (antBlockX + x < 0 || antBlockX + x >= 48)
                        continue;
                    for (int y = -sensorDistance; y <= sensorDistance; y++)
                    {
                        if (antBlockY + y < 0 || antBlockY + y >= 30)
                            continue;
                        float delta = hpRatioDelta - weightsForSurvive[antBlockX + x, antBlockY + y];
                        weightsForSurvive[antBlockX + x, antBlockY + y] += (delta - Math.Sign(delta)*(float)(Math.Sqrt(x * x + y * y) * 0.01 * delta))*hurtLearningRate;
                    }
                }
            }
        }
        public void GetFormationIndex(Ant ant, out int blockX, out int blockY)
        {
            var info = game.AntFactory.GetFormationInfo(ant).Find(x => x.ant == ant);
            if (info.ant != null)
            {
                blockX = info.blockX;
                blockY = info.blockY;
            }
            else
            {
                blockX = 24;
                blockY = 15;
            }
        }
        public bool UpgradeNest(AntGrowthDirection direction, Player player)
        {
            if (player.UseCake(NestLevelFoodTable.FoodForUpgrade(GrowthProperties.Level)))
            {
                switch (direction)
                {
                    case AntGrowthDirection.Duration:
                        growthProperties.duration++;
                        break;
                    case AntGrowthDirection.Speed:
                        growthProperties.speed++;
                        break;
                    case AntGrowthDirection.Resistant:
                        growthProperties.resistant++;
                        break;
                    case AntGrowthDirection.Sensitivity:
                        growthProperties.sensitivity++;
                        break;
                    case AntGrowthDirection.Population:
                        growthProperties.population++;
                        break;
                    default:
                        return false;
                }
                OnGrowthPropertiesChange?.Invoke(GrowthProperties);
                return true;
            }
            else
            {
                return false;
            }
        }
        public float GetLocalSurvive(int x, int y, int deep)
        {
            List<float> surviveValues = new List<float>();
            float self;
            if (x < 48 && x >= 0 && y < 30 && y >= 0)
                self = (weightsForSurvive[x, y]);
            else
                self = (-1);
            if(searchHelper.Contains(new PositionInfo { x = x , y = y }))
            {
                return self;
            }
            else
            {
                searchHelper.Add(new PositionInfo { x = x, y = y });
            }
            if (deep == 1)
            {
                surviveValues.Add(self);
            }
            else
            {
                surviveValues.Add(self + GetLocalSurvive(x + 1, y + 1, deep - 1));
                surviveValues.Add(self + GetLocalSurvive(x + 1, y - 1, deep - 1));
                surviveValues.Add(self + GetLocalSurvive(x - 1, y + 1, deep - 1));
                surviveValues.Add(self + GetLocalSurvive(x - 1, y - 1, deep - 1));
            }
            return surviveValues.Max();
        }
        public void GoodFormation(double value)
        {
            distributionMap.weight += value;
        }
        public void Load3DistributionMap(string map1, string map2, string map3)
        {
            string[] dataStrings = map1.Split(',');
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    distributionMaps[0].distributionWeight[x, y] = double.Parse(dataStrings[x * 11 + y]);
                }
            }
            dataStrings = map2.Split(',');
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    distributionMaps[1].distributionWeight[x, y] = double.Parse(dataStrings[x * 11 + y]);
                }
            }
            dataStrings = map3.Split(',');
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    distributionMaps[2].distributionWeight[x, y] = double.Parse(dataStrings[x * 11 + y]);
                }
            }
        }
        public void Serialize3DistributionMap(out string map1, out string map2, out string map3)
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    sb.Append(distributionMaps[0].distributionWeight[x, y]);
                    sb.Append(',');
                }
            }
            map1 = sb.ToString();

            sb = new StringBuilder();
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    sb.Append(distributionMaps[1].distributionWeight[x, y]);
                    sb.Append(',');
                }
            }
            map2 = sb.ToString();

            sb = new StringBuilder();
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    sb.Append(distributionMaps[2].distributionWeight[x, y]);
                    sb.Append(',');
                }
            }
            map3 = sb.ToString();
        }
    }
}