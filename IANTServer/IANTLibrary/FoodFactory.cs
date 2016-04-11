using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public class FoodFactory
    {
        protected List<Food> foodList;
        protected List<Food> storedFoodList;
        public int RemainedFoodCount { get { return storedFoodList.Count; } }
        public int TotalFoodCount { get { return foodList.Count; } }

        public event Action<int> OnRemainedFoodCountChange;

        public FoodFactory()
        {
            foodList = new List<Food>();
            storedFoodList = new List<Food>();
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
            if(!ant.IsTakingFood && storedFoodList.Count > 0)
            {
                Food food = storedFoodList.First();
                ant.TakeFood(food);
                OnRemainedFoodCountChange?.Invoke(RemainedFoodCount-1);
                return storedFoodList.Remove(food);
            }
            else
            {
                return false;
            }
        }
        public void ReplaceFood(Food food)
        {
            storedFoodList.Add(food);
            OnRemainedFoodCountChange?.Invoke(RemainedFoodCount);
        }
    }
}
