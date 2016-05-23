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
        public float nestRadius;
        public float foodPlatePositionX;
        public float foodPlatePositionY;
        public float foodPlateRadius;
        public float leastTowerSpan;
        public Tower towerPrefab;
        public Ant antPrefab;
        public int startMoney;
        public int antNumber;
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
        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                OnScoreChange?.Invoke(score);
            }
        }
        private int wave;
        public int Wave
        {
            get { return wave; }
            set
            {
                wave = value;
                OnWaveChange?.Invoke(wave);
            }
        }
        protected GameConfiguration configuration;
        public float NestPositionX { get { return configuration.nestPositionX; } }
        public float NestPositionY { get { return configuration.nestPositionY; } }
        public float NestRadius { get { return configuration.nestRadius; } }
        public float FoodPlatePositionX { get { return configuration.foodPlatePositionX; } }
        public float FoodPlatePositionY { get { return configuration.foodPlatePositionY; } }
        public float FoodPlateRadius { get { return configuration.foodPlateRadius; } }
        public int AntNumber { get { return configuration.antNumber; } }

        public event Action<int> OnWaveChange;
        public event Action<int> OnScoreChange;
        public event Action<int> OnMoneyChange;
        public event Action OnGameOver;

        public Game(GameConfiguration configuration)
        {
            this.configuration = configuration;
            Score = 0;
            Money = configuration.startMoney;
        }
        public void SetFoods(Food food, int count)
        {
            FoodFactory.FillFood(food, count);
        }
        public void StartGame(Food food, int foodCount)
        {
            for(int i = 0; i < AntNumber; i++)
            {
                AntFactory.InstantiateNewAnt();
            }
            SetFoods(food, foodCount);
            Money = configuration.startMoney;
        }
        public void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}
