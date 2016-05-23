using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public abstract class FoodFactory
    {
        protected Game game;
        protected List<Food> foodList;
        protected List<Food> storedFoodList;
        public int RemainedFoodCount { get { return storedFoodList.Count; } }
        public int TotalFoodCount { get { return foodList.Count; } }
        public float FoodPlatePositionX { get; protected set; }
        public float FoodPlatePositionY { get; protected set; }
        public float Radius { get; protected set; }

        public event Action<int> OnRemainedFoodCountChange;

        public FoodFactory(Game game)
        {
            foodList = new List<Food>();
            storedFoodList = new List<Food>();
            FoodPlatePositionX = game.FoodPlatePositionX;
            FoodPlatePositionY = game.FoodPlatePositionY;
            Radius = game.FoodPlateRadius;
            this.game = game;
        }

        public void FillFood(Food food, int count)
        {
            for(int foodCount = 0; foodCount < count; foodCount++)
            {
                Food newFood = food.Duplicate();
                foodList.Add(newFood);
                storedFoodList.Add(newFood);
            }
            OnRemainedFoodCountChange?.Invoke(RemainedFoodCount);
        }
        public bool TakeFood(Ant ant)
        {
            if (!ant.IsTakingFood && storedFoodList.Count > 0)
            {
                Food food = storedFoodList[0];
                if(ant.TakeFood(food))
                {
                    storedFoodList.RemoveAt(0);
                    OnRemainedFoodCountChange?.Invoke(RemainedFoodCount);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void ReplaceFood(Food food)
        {
            if(food != null)
            {
                storedFoodList.Add(food);
                OnRemainedFoodCountChange?.Invoke(RemainedFoodCount);
            }
        }
        public void EraseFood(Food food)
        {
            foodList.Remove(food);
            if(foodList.Count == 0)
            {
                game.GameOver();
            }
        }
    }
}
