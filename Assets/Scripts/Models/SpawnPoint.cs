using UnityEngine;
using System.Collections.Generic;

namespace Models
{
    public class SpawnPoint
    {
        public int frequency;
        public Vector3 pos;
        private List<int> enemyCount;
        private int totalCountOfEnemies = 0;

        public SpawnPoint(int frequency, Vector3 pos, List<int> enemyCount)
        {
            this.frequency = frequency;
            this.pos = pos;
            this.enemyCount = new List<int>(enemyCount);
            enemyCount.ForEach(i => totalCountOfEnemies += i);

        }

        public int GetEnemyIndex()
        {
            if (totalCountOfEnemies <= 0) return -1;
            float x = Random.Range(0.0f, 1.0f);
            float y = 0f;
            for (int i = 0; i < enemyCount.Count; i++)
            {
               
                y += (enemyCount[i] / (float)totalCountOfEnemies);
                if (y >= x)
                {
                    enemyCount[i] -= 1;
                    totalCountOfEnemies -= 1;
                    return i;
                }
            }
            Debug.Log("be carefull with recursion dummy");
            if (totalCountOfEnemies != 0) return GetEnemyIndex();
            else return -1;
        }
    }
}