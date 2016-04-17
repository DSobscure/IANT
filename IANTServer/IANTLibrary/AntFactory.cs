using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public abstract class AntFactory
    {
        protected Dictionary<int, Ant> antDictionary;
        protected Ant antPrefab;
        protected int totalAntCount = 0;
        protected float nestPositionX;
        protected float nestPositionY;

        public AntFactory(float nestPositionX, float nestPositionY, Ant antPrefab)
        {
            this.nestPositionX = nestPositionX;
            this.nestPositionY = nestPositionY;
            antDictionary = new Dictionary<int, Ant>();
            this.antPrefab = antPrefab.Duplicate();
        }

        public virtual Ant InstantiateNewAnt()
        {
            totalAntCount++;
            if(totalAntCount%6 == 0)
            {
                antPrefab.LevelUp();
            }
            Ant ant = antPrefab.Duplicate();
            ant.UpdatePosition(nestPositionX, nestPositionY);
            antDictionary.Add(ant.ID, ant);
            return ant;
        }

        public virtual void NotifyAntDead(Ant ant)
        {
            antDictionary.Remove(ant.ID);
        }
    }
}
