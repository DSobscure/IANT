using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public abstract class TowerFactory
    {
        protected Tower towerPerfab;
        protected float leastTowerSpan;
        protected Dictionary<int, Tower> towerDictionary;
        private int nextTowerCost;
        public int NextTowerCost
        {
            get { return nextTowerCost; }
            protected set
            {
                nextTowerCost = value;
                OnBuildTowerCostChange?.Invoke(nextTowerCost);
            }
        }

        public event Action<int> OnBuildTowerCostChange;

        public TowerFactory(Tower towerPerfab, float leastTowerSpan)
        {
            this.towerPerfab = towerPerfab;
            this.leastTowerSpan = leastTowerSpan;
            towerDictionary = new Dictionary<int, Tower>();
            NextTowerCost = 20;
        }
        public bool IsPositionLegal(float x, float y)
        {
            return !towerDictionary.Values.Any(tower => tower.DistanceFrom(x,y) < leastTowerSpan);
        }
        public virtual bool BuildTower(float positionX, float positionY, Game game, out Tower tower, out string errorMessage)
        {
            tower = null;
            errorMessage = "";
            if(!IsPositionLegal(positionX, positionY))
            {
                errorMessage = "position is too close to other towers!";
                return false;
            }
            else if(game.Money < NextTowerCost)
            {
                errorMessage = "not enough money!";
                return false;
            }
            else
            {
                tower = towerPerfab.Duplicate();
                tower.Locate(positionX, positionY);
                game.Money -= NextTowerCost;
                towerDictionary.Add(tower.UniqueID, tower);
                NextTowerCost += 10;
                return true; ;
            }
        }
        public virtual bool DestroyTower(Tower tower, Game game)
        {
            if(towerDictionary.ContainsKey(tower.UniqueID))
            {
                towerDictionary.Remove(tower.UniqueID);
                game.Money += tower.DestroyReturn;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
