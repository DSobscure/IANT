using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public class NestLevelFoodTable
    {
        private List<int> foodTable;
        public NestLevelFoodTable()
        {
            foodTable = new List<int>()
            {
                6,
                30,
                120,
                360,
                720,
                1000
            };
        }
        public NestLevelFoodTable(List<int> foodTable)
        {
            this.foodTable = foodTable;
        }
        public int FoodForUpgrade(int level)
        {
            if (level > 0 && level < foodTable.Count + 1)
            {
                return foodTable[level - 1];
            }
            else
            {
                return int.MaxValue;
            }
        }
    }
}
