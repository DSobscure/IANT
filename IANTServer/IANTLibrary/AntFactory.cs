using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public abstract class AntFactory
    {
        protected Dictionary<int, Ant> antDictionary;
        protected Game game;
        protected Ant antPrefab;
        protected int totalAntCount = 0;
        protected float nestPositionX;
        protected float nestPositionY;
        protected float nestRadius;
        protected Nest nest;

        protected float formationValue = 0;
        protected float cakeFormationValue = 0;
        protected int formationAntNumber = 0;
        protected int cakeFormationAntNumber = 0;

        public AntFactory(Ant antPrefab, Game game)
        {
            this.nestPositionX = game.NestPositionX;
            this.nestPositionY = game.NestPositionY;
            this.nestRadius = game.NestRadius;
            antDictionary = new Dictionary<int, Ant>();
            this.antPrefab = antPrefab.Duplicate();
            this.game = game;
            game.OnWaveChange += (wave) => 
            {
                if(((wave-20) % 15) == 0)
                {
                    nest.distributionMaps.Sort();
                    DistributionMap good_good = DistributionMap.Evolution(nest.distributionMaps[2], nest.distributionMaps[2]);
                    DistributionMap good_middle1 = DistributionMap.Evolution(nest.distributionMaps[2], nest.distributionMaps[1]);
                    DistributionMap good_middle2 = DistributionMap.Evolution(nest.distributionMaps[1], nest.distributionMaps[2]);
                    nest.distributionMaps[0] = good_good;
                    nest.distributionMaps[1] = good_middle1;
                    nest.distributionMaps[2] = good_middle2;
                }
                if(wave % 5 == 0)
                {
                    nest.distributionMap = nest.distributionMaps[(wave/5-1) % 3];
                }
            };
        }
        public virtual Ant InstantiateNewAnt()
        {
            totalAntCount++;
            if(totalAntCount % (game.AntNumber + nest.GrowthProperties.population * game.Wave / 20) == 0)
            {
                antPrefab.LevelUp();
                game.Wave += 1;
            }
            Ant ant = antPrefab.Duplicate();
            Random randomGenerator = new Random();
            double distance = game.NestRadius * randomGenerator.NextDouble() * 0.8;
            double rotation = 360 * randomGenerator.NextDouble();
            ant.UpdateTransform(nestPositionX + (float)(distance*Math.Cos(rotation)), nestPositionY + (float)(distance * Math.Sin(rotation)), (float)rotation);
            antDictionary.Add(ant.ID, ant);
            return ant;
        }
        public virtual void NotifyAntDead(Ant ant, FoodFactory foodFactory)
        {
            antDictionary.Remove(ant.ID);
            if (ant.IsTakingFood)
                ant.ReleaseFood(foodFactory);
            while(antDictionary.Count < game.AntNumber + nest.GrowthProperties.population * game.Wave / 20)
                InstantiateNewAnt();
        }
        public List<FormationInfo> GetFormationInfo(Ant criticalAnt)
        {
            List<FormationInfo> formationInfo = new List<FormationInfo>();
            float centerX = antDictionary.Values.Where(x => x.DistanceBetween(criticalAnt) < 300).Average(x => x.PositionX);
            float centerY = antDictionary.Values.Where(x => x.DistanceBetween(criticalAnt) < 300).Average(x => x.PositionY);
            foreach (Ant ant in antDictionary.Values.Where(x => x.DistanceBetween(criticalAnt) < 300))
            {
                formationInfo.Add(new FormationInfo
                {
                    ant = ant,
                    blockX = Math.Max(Math.Min((int)((ant.PositionX - centerX) / 20), 5), -5),
                    blockY = Math.Max(Math.Min((int)((ant.PositionY - centerY) / 20), 5), -5)
                });
            }
            return formationInfo;
        }
        public void BindNest(Nest nest)
        {
            this.nest = nest;
            nest.distributionMap = nest.distributionMaps[0];
        }
        public List<PositionInfo> GetOtherAntPositionInfo(Ant ant)
        {
            List<PositionInfo> infos = new List<PositionInfo>();
            foreach(var a in antDictionary.Values)
            {
                if(a != ant)
                {
                    infos.Add(new PositionInfo { x = a.PositionX, y = a.PositionY });
                }
            }
            return infos;
        }
    }
}
