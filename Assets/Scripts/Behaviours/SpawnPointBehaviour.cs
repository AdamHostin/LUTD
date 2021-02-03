using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{

    private SpawnPoint model;

    
    public int StartInWave;
    public int frequency;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<int> enemyCount = new List<int>();

    private void Awake()
    {
        if (enemyCount.Count > prefabs.Count) Debug.LogError("Missing prefabs on SpawnPoint");
        App.levelManager.waveEvent.AddListener(Initialize);       
    }

    private void SpawnEnemy()
    {
        
        int idx = model.GetEnemyIndex();
        if (idx < 0)
        {
            Debug.Log("Stop spawning");
            CancelInvoke("SpawnEnemy");
            return;
        }
        GameObject enemyObject = Instantiate(prefabs[idx],transform);

    }

    public void Initialize(int wave)
    {
        if (wave < StartInWave) return;
        this.model = new SpawnPoint(frequency,transform.position, enemyCount);
        InvokeRepeating("SpawnEnemy", 0, model.frequency);
    }

    private void GetNewEnemyCount()
    {

    }
}