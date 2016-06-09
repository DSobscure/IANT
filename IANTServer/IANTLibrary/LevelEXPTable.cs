using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTLibrary
{
    public static class LevelEXPTable
    {
        private static List<int> expTable;
        static LevelEXPTable()
        {
            expTable = new List<int>()
            {
                60,
                300,
                1200,
                3600,
                7200,
                10000
            };
        }
        public static int EXPForUpgrade(int level)
        {
            if(level > 0 && level < expTable.Count + 1)
            {
                return expTable[level-1];
            }
            else
            {
                return int.MaxValue;
            }
        }
        public static int GetLevel(int exp, out int remainedEXP)
        {
            int level = 1;
            if (exp >= 0)
            {
                for (int i = 0; i < expTable.Count; i++)
                {
                    if (exp - expTable[i] < 0)
                    {
                        break;
                    }
                    else
                    {
                        exp -= expTable[i];
                        level++;
                    }
                }
            }
            remainedEXP = exp;
            return level;
        }
        public static bool IsUpgrade(int level, int exp)
        {
            if (level > 0 && level < expTable.Count + 1)
            {
                for (int i = 0; i < level; i++)
                {
                    exp -= expTable[i];
                }
                return exp >= 0;
            }
            else
            {
                return false;
            }
        }
    }
}
