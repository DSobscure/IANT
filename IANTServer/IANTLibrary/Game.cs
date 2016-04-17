using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public struct GameConfiguration
    {
        public float nestPositionX;
        public float nestPositionY;
        public float foodPlatePositionX;
        public float foodPlatePositionY;
        public float leastTowerSpan;
        public Tower towerPrefab;
        public Ant antPrefab;
    }

    public abstract class Game
    {
        public FoodFactory FoodFactory { get; protected set; }
        public AntFactory AntFactory { get; protected set; }
        public TowerFactory TowerFactory { get; protected set; }
        private int money;
        public int Money
        {
            get { return money; }
            set
            {
                money = value;
                OnMoneyChange?.Invoke(money);
            }
        }
        protected GameConfiguration configuration;
        public event Action<int> OnMoneyChange;

        public Game(GameConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SetFoods(Food food, int count)
        {
            FoodFactory.FillFood(food, count);
        }
        public void StartGame(int antCount, Food food, int foodCount, int money)
        {
            for(int i = 0; i < antCount; i++)
            {
                AntFactory.InstantiateNewAnt();
            }
            SetFoods(food, foodCount);
            Money = money;
        }
    }
}
