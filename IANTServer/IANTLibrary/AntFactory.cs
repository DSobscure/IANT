using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public class AntFactory
    {
        protected Dictionary<int, Ant> antDictionary;
        protected Ant antPrefab;
        protected int totalAntCount = 0;
        protected float nestPositionX;
        protected float nestPositionY;

        public AntFactory(float nestPositionX, float nestPositionY)
        {
            this.nestPositionX = nestPositionX;
            this.nestPositionY = nestPositionY;
            antDictionary = new Dictionary<int, Ant>();
            antPrefab = new Ant(new AntProperties()
            {
                positionX = nestPositionX,
                positionY = nestPositionY,
                food = null,
                hp = 3,
                maxHP = 3
            });
        }

        public virtual Ant InstantiateNewAnt()
        {
            totalAntCount++;
            Ant ant = antPrefab.Duplicate();
            antDictionary.Add(ant.ID, ant);
            return ant;
        }
    }
}
