using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public float damageRatioDelta;
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
        public int duration;
        public int speed;
        public int resistant;
        public int sensorDistance;
        public int number;
    }

    public class Nest
    {
        protected Game game;
        public float[,] weightsForSurvive;
        public float[,] weightsForCake;
        protected float[,] weightsForNest;
        protected float hurtLearningRate = 0.05f;
        protected float formationLearningRate = 0.2f;

        public float[,] formationCakeWeight;
        public float[,] formationWeight;

        public AntGrowthProperties GrowthProperties { get; protected set; }

        public Nest(AntGrowthProperties growthProperties)
        {
            GrowthProperties = growthProperties;
            weightsForSurvive = new float[96, 60];
            weightsForCake = new float[96, 60];
            weightsForNest = new float[96, 60];
            formationWeight = new float[96, 60];
            formationCakeWeight = new float[96, 60];
            for (int x = 0; x < 96; x++)
            {
                weightsForSurvive[x, 0] = float.MinValue;
                weightsForSurvive[x, 59] = float.MinValue;
            }
            for (int y = 0; y < 60; y++)
            {
                weightsForSurvive[0, y] = float.MinValue;
                weightsForSurvive[95, y] = float.MinValue;
            }
        }
        public void BindGame(Game game)
        {
            this.game = game;
            int cakeBlockX = Convert.ToInt32(game.FoodPlatePositionX/10) + 48;
            int cakeBlockY = Convert.ToInt32(game.FoodPlatePositionY/10) + 30;
            int nestBlockX = Convert.ToInt32(game.NestPositionX/10) + 48;
            int nestBlockY = Convert.ToInt32(game.NestPositionY/10) + 30;
            for (int x = 0; x < 96; x++)
            {
                for(int y = 0; y < 60; y++)
                {
                    weightsForCake[x, y] = Convert.ToSingle(0.65 - 0.02 * Math.Sqrt(Math.Pow(cakeBlockX - x, 2) + Math.Pow(cakeBlockY - y, 2)));
                    weightsForNest[x, y] = Convert.ToSingle(0.75 - 0.02 * Math.Sqrt(Math.Pow(nestBlockX - x, 2) + Math.Pow(nestBlockY - y, 2)));
                }
            }
        }
        public float GetProperRotation(Ant ant)
        {
            Random annelingFactorGenerator = new Random(Guid.NewGuid().GetHashCode());
            double annelingFactor = annelingFactorGenerator.NextDouble() * ant.HP / ant.MaxHP * 0.6;
            if(ant.IsTakingFood)
            {
                annelingFactor *= 0.2;
            }
            int sensorDistance = 2;
            int antBlockX = Convert.ToInt32(ant.PositionX / 10) + 48;
            int antBlockY = Convert.ToInt32(ant.PositionY / 10) + 30;
            int formationX, formationY;
            GetFormationIndex(ant, out formationX, out formationY);
            List<BlockInfo> blockInfos = new List<BlockInfo>();
            for (int x = -sensorDistance; x <= sensorDistance; x++)
            {
                if (antBlockX + x < 0 || x == 0 || antBlockX + x >= 96)
                    continue;
                for(int y = -sensorDistance; y <= sensorDistance; y++)
                {
                    if (antBlockY + y < 0 || y == 0 || antBlockY + y >= 60)
                        continue;
                    float extraWeight = 0;
                    float goalFormationWeight = 0;
                    if(formationX + x >= 0 && formationX + x < 96 && formationY + y >= 0 && formationY + y < 60)
                    {
                        if(ant.IsTakingFood)
                            goalFormationWeight = formationCakeWeight[formationX + x, formationY + y];
                        else
                            goalFormationWeight = formationWeight[formationX + x, formationY + y];
                    }
                    if(ant.IsTakingFood)
                    {
                        extraWeight = weightsForNest[antBlockX + x, antBlockY + y];
                    }
                    else if(game.FoodFactory.RemainedFoodCount > 0)
                    {
                        extraWeight = weightsForCake[antBlockX + x, antBlockY + y];
                    }
                    blockInfos.Add(new BlockInfo { x = x, y = y, weight = weightsForSurvive[antBlockX + x, antBlockY + y] + extraWeight + goalFormationWeight });
                }
            }
            blockInfos.Sort();
            int selectIndex = blockInfos.Count - 1 - annelingFactorGenerator.Next(Convert.ToInt32((blockInfos.Count - 1) * annelingFactor));
            BlockInfo bestBlock = blockInfos[selectIndex];
            return Convert.ToSingle(Math.Atan2(bestBlock.y, bestBlock.x) * 180 / Math.PI + 45 * annelingFactor);
        }
        public void AntStay(Dictionary<PositionInfo,AntStayInfo> antStayInfos)
        {
            foreach (var antStayInfoPair in antStayInfos)
            {
                int antBlockX = Convert.ToInt32(antStayInfoPair.Key.x / 10) + 48;
                int antBlockY = Convert.ToInt32(antStayInfoPair.Key.y / 10) + 30;
                int sensorDistance = 1;
                float damage = antStayInfoPair.Value.damageRatioDelta / antStayInfoPair.Value.timeDelta;
                for (int x = -sensorDistance; x <= sensorDistance; x++)
                {
                    if (antBlockX + x < 0 || antBlockX + x >= 96)
                        continue;
                    for (int y = -sensorDistance; y <= sensorDistance; y++)
                    {
                        if (antBlockY + y < 0 || antBlockY + y >= 60)
                            continue;
                        weightsForSurvive[antBlockX + x, antBlockY + y] += (-damage - weightsForSurvive[antBlockX + x, antBlockY + y]) * hurtLearningRate / (1 + (float)(Math.Sqrt(x * x + y * y)*10));
                    }
                }
            }
        }
        public void GetFormationIndex(Ant ant, out int blockX, out int blockY)
        {
            var info = game.AntFactory.GetFormationInfo(ant).Find(x => x.ant == ant);
            if(info.ant != null)
            {
                blockX = info.blockX;
                blockY = info.blockY;
            }
            else
            {
                blockX = 48;
                blockY = 30;
            }
        }
        public void FormationTraning(Ant ant, float paneltyDelta)
        {
            if(ant != null)
            {
                int formationX, formationY;
                GetFormationIndex(ant, out formationX, out formationY);
                int sensorDistance = 4;
                for (int x = -sensorDistance; x <= sensorDistance; x++)
                {
                    if (formationX + x < 0 || formationX + x >= 96)
                        continue;
                    for (int y = -sensorDistance; y <= sensorDistance; y++)
                    {
                        if (formationY + y < 0 || formationY + y >= 60)
                            continue;
                        if (ant.IsTakingFood)
                        {
                            formationCakeWeight[formationX + x, formationY + y] += (-paneltyDelta - formationCakeWeight[formationX + x, formationY + y]) * formationLearningRate / (1 + (float)(Math.Sqrt(x * x + y * y)));
                        }
                        else
                        {
                            formationWeight[formationX + x, formationY + y] += (-paneltyDelta - formationWeight[formationX + x, formationY + y]) * formationLearningRate / (1 + (float)(Math.Sqrt(x * x + y * y)));
                        }
                    }
                }
            }
        }
    }
}
