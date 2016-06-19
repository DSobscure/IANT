using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public class DistributionMap : IComparable<DistributionMap>
    {
        public double[,] distributionWeight;
        public double[,] cuw;
        public double dev;
        public double weight;

        public DistributionMap(double[,] distributionWeight)
        {
            this.distributionWeight = distributionWeight;
        }
        public DistributionMap()
        {
            distributionWeight = new double[11, 11];
            Randomize();
            //for (int x = 1; x < 4; x++)
            //{
            //    for (int y = 1; y < 4; y++)
            //    {
            //        distributionWeight[x, y] = 1.0 / 9;
            //    }
            //}
            //for (int i = 0; i < 11; i++)
            //{
            //    distributionWeight[0, i] = 1/22.0;
            //    distributionWeight[10, i] = 1/22.0;
            //}
            //distributionWeight[0, 0] = 0.1;
            //distributionWeight[0, 1] = 0.05;
            //distributionWeight[1, 0] = 0.05;
            //distributionWeight[1, 1] = 0.04;

            //distributionWeight[0, 4] = 0.1;
            //distributionWeight[0, 3] = 0.05;
            //distributionWeight[1, 4] = 0.05;
            //distributionWeight[1, 3] = 0.04;

            //distributionWeight[4, 4] = 0.1;
            //distributionWeight[4, 3] = 0.05;
            //distributionWeight[3, 4] = 0.05;
            //distributionWeight[3, 3] = 0.04;

            //distributionWeight[4, 0] = 0.1;
            //distributionWeight[4, 1] = 0.05;
            //distributionWeight[3, 0] = 0.05;
            //distributionWeight[3, 1] = 0.04;

            //distributionWeight[2, 1] = 0.01;
            //distributionWeight[2, 3] = 0.01;
            //distributionWeight[1, 2] = 0.01;
            //distributionWeight[3, 2] = 0.01;
        }
        public float GetProperRotation(Ant criticalAnt, List<FormationInfo> allFormationInfos, out double deviation)
        {
            FormationInfo criticalFormationInfo = allFormationInfos.Find(x => x.ant == criticalAnt);
            double[,] currentDistributionWeight = new double[11, 11];
            cuw = new double[11, 11];
            foreach (FormationInfo info in allFormationInfos)
            {
                currentDistributionWeight[5 + info.blockX, 5 + info.blockY] += 1.0 / allFormationInfos.Count;
                cuw[5 + info.blockX, 5 + info.blockY] += 1.0 / allFormationInfos.Count;
            }
            deviation = Math.Sqrt(Variance(distributionWeight, currentDistributionWeight));
            dev = deviation;
            List<BlockInfo> blockInfos = new List<BlockInfo>();
            for(int x = -5; x <= 5; x++)
            {
                for(int y = -5; y <= 5; y++)
                {
                    int newBlockX = Math.Min(Math.Max(criticalFormationInfo.blockX + x, -5), 5);
                    int newBlockY = Math.Min(Math.Max(criticalFormationInfo.blockY + y, -5), 5);
                    currentDistributionWeight[5 + newBlockX, 5 + newBlockY] -= 1.0 / allFormationInfos.Count;
                    blockInfos.Add(new BlockInfo { x = newBlockX, y = newBlockY, weight = Variance(currentDistributionWeight, distributionWeight) });
                    currentDistributionWeight[5 + newBlockX, 5 + newBlockY] += 1.0 / allFormationInfos.Count;
                }
            }
            float minWeight = blockInfos.Min().weight;
            var blocks = blockInfos.FindAll(x => x.weight == minWeight);
            BlockInfo bestBlock = blocks.OrderBy(x => Guid.NewGuid().GetHashCode()).FirstOrDefault();
            if (bestBlock.x == 0 && bestBlock.y == 0)
                return -1f;
            else
            {
                Random r = new Random(Guid.NewGuid().GetHashCode());
                return (MathTool.Direction2DToRotation2D(bestBlock.x + r.NextDouble()/2-0.25, bestBlock.y + r.NextDouble() / 2 - 0.25) + 360f) % 360f;
            }
        }
        public float GetDirectionWeight(Ant criticalAnt, List<FormationInfo> allFormationInfos, int directionX, int directionY)
        {
            FormationInfo criticalFormationInfo = allFormationInfos.Find(x => x.ant == criticalAnt);
            double[,] currentDistributionWeight = new double[11, 11];
            foreach (FormationInfo info in allFormationInfos)
            {
                currentDistributionWeight[5 + info.blockX, 5 + info.blockY] += 1.0 / allFormationInfos.Count;
            }
            int newBlockX = Math.Min(Math.Max(5 + criticalFormationInfo.blockX + directionX, 0), 10);
            int newBlockY = Math.Min(Math.Max(5 + criticalFormationInfo.blockY + directionY, 0), 10);
            currentDistributionWeight[5 + criticalFormationInfo.blockX, 5 + criticalFormationInfo.blockY] -= 1.0 / allFormationInfos.Count;
            currentDistributionWeight[newBlockX, newBlockY] += 1.0 / allFormationInfos.Count;
            return 1 - (Variance(distributionWeight, currentDistributionWeight));
        }
        private static float Variance(double[,] l, double[,] r)
        {
            double total = 0;
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    total += Math.Pow(l[x, y] - r[x, y], 2);
                }
            }
            return (float)total;
        }
        public void Randomize()
        {
            Random generator = new Random(Guid.NewGuid().GetHashCode());
            for(int x = 0; x < 11; x++)
            {
                for(int y = 0; y < 11; y++)
                {
                    distributionWeight[x, y] = generator.NextDouble();
                }
            }
            Normalize();
        }
        public void Normalize()
        {
            double sum = 0;
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    sum += distributionWeight[x, y];
                }
            }
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    distributionWeight[x, y] /= sum;
                }
            }
        }

        public int CompareTo(DistributionMap other)
        {
            return weight.CompareTo(other.weight);
        }

        public static DistributionMap Evolution(DistributionMap father, DistributionMap mother)
        {
            double[,] newDistribution = new double[11, 11];
            Random generator = new Random();
            for(int x = 0; x < 11; x++)
            {
                for(int y = 0; y < 11; y++)
                {
                    int randomNumber = generator.Next(0, 10);
                    if (randomNumber == 10)
                    {
                        newDistribution[x, y] = generator.NextDouble();
                    }
                    else if(randomNumber % 2 == 0)
                    {
                        newDistribution[x, y] = Math.Pow(mother.distributionWeight[x, y],2);
                    }
                    else
                    {
                        newDistribution[x, y] = Math.Pow(father.distributionWeight[x, y],2);
                    }
                }
            }
            DistributionMap newMap = new DistributionMap(newDistribution);
            newMap.Normalize();
            return newMap;
        }
    }
}
