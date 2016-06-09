using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public static class NestLevelFoodTable
    {
        private static List<int> foodTable;
        static NestLevelFoodTable()
        {
            foodTable = new List<int>()
            {
                10,
                30,
                100,
                360,
                1000,
                3000
            };
        }
        public static int FoodForUpgrade(int level)
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
