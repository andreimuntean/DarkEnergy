using System.Collections.Generic;
using DarkEnergy.Characters;

namespace DarkEnergy
{
    public class EnemyList
    {
        /// <summary>
        /// Contains the possible sizes of the enemy group.
        /// </summary>
        public List<int> Count { get; set; }

        /// <summary>
        /// Contains the ids of the potential enemies.
        /// </summary>
        public List<int> IdList { get; set; }

        /// <summary>
        /// Contains the levels of the potential enemies.
        /// </summary>
        public List<int[]> LevelList { get; set; }

        public EnemyList(params int[] id)
        {
            Count = new List<int>() { 1, 2, 3 };
            IdList = new List<int>(id);
        }

        public void SetCountList(params int[] count)
        {
            Count = new List<int>(count);
        }

        public void SetIdList(params int[] id)
        {
            IdList = new List<int>(id);
        }

        public void SetLevelList(params int[][] level)
        {
            LevelList = new List<int[]>(level);
        }

        public void SetLevelList(int start, int end)
        {
            LevelList = new List<int[]>();

            foreach (var id in IdList)
            {
                LevelList.Add(new int[] { RandomManager.GetInt(start, end + 1) });
            }
        }

        public List<Character> GetList()
        {
            var count = Count[RandomManager.GetInt(Count.Count)];
            var enemyList = new List<Character>();

            count %= 4;

            for (var i = 0; i < count; ++i)
            {
                var k = RandomManager.GetInt(IdList.Count);
                var enemy = DataManager.Load<Enemy>(IdList[k]);
                enemy.Level = LevelList[k][RandomManager.GetInt(LevelList[k].Length)];

                enemyList.Add(enemy);
            }

            return enemyList;
        }
    }
}
