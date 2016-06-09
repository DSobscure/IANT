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
            if (criticalAnt.IsTakingFood)
            {
                float centerX = antDictionary.Values.Where(x => x.IsTakingFood).Average(x => x.PositionX);
                float centerY = antDictionary.Values.Where(x => x.IsTakingFood).Average(x => x.PositionY);
                foreach (Ant ant in antDictionary.Values.Where(x => x.IsTakingFood))
                {
                    formationInfo.Add(new FormationInfo
                    {
                        ant = ant,
                        blockX = Math.Max(Math.Min((int)((ant.PositionX - centerX) / 10) + 48, 95), 0),
                        blockY = Math.Max(Math.Min((int)((ant.PositionY - centerY) / 10) + 30, 59), 0)
                    });
                }
            }
            else
            {
                float centerX = antDictionary.Values.Where(x => !x.IsTakingFood).Average(x => x.PositionX);
                float centerY = antDictionary.Values.Where(x => !x.IsTakingFood).Average(x => x.PositionY);
                foreach (Ant ant in antDictionary.Values.Where(x => !x.IsTakingFood))
                {
                    formationInfo.Add(new FormationInfo
                    {
                        ant = ant,
                        blockX = Math.Max(Math.Min((int)((ant.PositionX - centerX) / 10) + 48, 95),0),
                        blockY = Math.Max(Math.Min((int)((ant.PositionY - centerY) / 10) + 30, 59),0)
                    });
                }
            }
            return formationInfo;
        }
        public void BindNest(Nest nest)
        {
            this.nest = nest;
        }
        public void FormationTraning(float timeDelta)
        {
            if(antDictionary.Values.Any(x => !x.IsTakingFood))
            {
                float newFormationValue = antDictionary.Values.Where(x => !x.IsTakingFood).Average(ant => (float)Math.Pow(ant.HP / (double)ant.MaxHP, 4)) * (antDictionary.Values.Count(x => !x.IsTakingFood) + 2)/(formationAntNumber+1);
                nest.FormationTraning(antDictionary.Values.First(x => !x.IsTakingFood), (newFormationValue - formationValue) / timeDelta);
                formationValue = newFormationValue;
            }
            if (antDictionary.Values.Any(x => x.IsTakingFood))
            {
                float newCakeFormationValue = antDictionary.Values.Where(x => x.IsTakingFood).Average(ant => (float)Math.Pow(ant.HP / (double)ant.MaxHP, 4)) * (antDictionary.Values.Count(x => x.IsTakingFood) + 2) / (cakeFormationAntNumber + 1); ;
                nest.FormationTraning(antDictionary.Values.First(x => x.IsTakingFood), (newCakeFormationValue - cakeFormationValue) / timeDelta);
                cakeFormationValue = newCakeFormationValue;
            }
            formationAntNumber = antDictionary.Values.Count(x => !x.IsTakingFood);
            cakeFormationAntNumber = antDictionary.Values.Count(x => x.IsTakingFood);
        }
    }
}
