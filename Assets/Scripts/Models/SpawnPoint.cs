using UnityEngine;
using System.Collections.Generic;

namespace Models
{
    public class SpawnPoint
    {
        public float frequency;
        public Vector3 pos;
        public SpawnPointState state;

        private List<int> enemyCount;
        private int totalCountOfEnemies = 0;

        private int preWaveEnemyCount;
        private int postWaveEnemyCount;

        public int currentTotalCountOfEnemies;
        private List<int> currentEnemyCount;

        public SpawnPoint(float frequency, Vector3 pos, List<int> enemyCount, int preWaveEnemyCount, int postWaveEnemyCount)
        {
            this.frequency = frequency;
            this.pos = pos;
            this.enemyCount = new List<int>(enemyCount);
            enemyCount.ForEach(i => totalCountOfEnemies += i);

            this.preWaveEnemyCount = preWaveEnemyCount;
            this.postWaveEnemyCount = postWaveEnemyCount;
        }

        public void ChangeState(SpawnPointState newState)
        {
            switch (newState)
            {
                case SpawnPointState.prewave:
                    currentTotalCountOfEnemies = preWaveEnemyCount;
                    state = SpawnPointState.prewave;
                    break;
                case SpawnPointState.wave:
                    currentTotalCountOfEnemies = totalCountOfEnemies;
                    currentEnemyCount = new List<int>(enemyCount);
                    state = SpawnPointState.wave;
                    break;
                case SpawnPointState.postwave:
                    currentTotalCountOfEnemies = postWaveEnemyCount;
                    state = SpawnPointState.postwave;
                    break;
            }
        }

        public bool isOutOfEnemies()
        {
            if (currentTotalCountOfEnemies <= 0) return true;
            else return false;
        }

        public void ScaleEnemyCount(float scaler)
        {
            enemyCount.ForEach(i => { i = (int)(i * scaler); totalCountOfEnemies += i; });
            preWaveEnemyCount = (int)(preWaveEnemyCount * scaler);
            postWaveEnemyCount = (int)(postWaveEnemyCount * scaler);
        }

        


        public int GetEnemyIndex()
        {
            if (currentTotalCountOfEnemies <= 0) return -1;

            float x = Random.Range(0.0f, 1.0f);
            float y = 0f;
            for (int i = 0; i < enemyCount.Count; i++)
            {
               
                y += (currentEnemyCount[i] / (float)currentTotalCountOfEnemies);
                if (y >= x)
                {
                    currentEnemyCount[i] -= 1;
                    currentTotalCountOfEnemies -= 1;
                    return i;
                }
            }
            Debug.Log("be carefull with recursion dummy");
            if (currentTotalCountOfEnemies != 0) return GetEnemyIndex();
            else return -1;
        }
    }
}