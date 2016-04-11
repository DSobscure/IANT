using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public class Game
    {
        public FoodFactory FoodFactory { get; protected set; }
        public AntFactory AntFactory { get; protected set; }

        public Game(float nestPositionX, float nestPositionY)
        {
            FoodFactory = new FoodFactory();
            AntFactory = new AntFactory(nestPositionX, nestPositionY);
        }
        public void SetFoods(Food food, int count)
        {
            FoodFactory.FillFood(food, count);
        }
        public void StartGame(int antCount, Food food, int foodCount)
        {
            for(int i = 0; i < antCount; i++)
            {
                AntFactory.InstantiateNewAnt();
            }
            SetFoods(food, foodCount);
        }
    }
}
