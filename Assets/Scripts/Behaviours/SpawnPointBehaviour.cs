using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{

    private SpawnPoint model;

    
    [SerializeField] private int startInWave;
    [SerializeField] private int activeWaveCount;
    //TODO: resolve if scaler should be property of level manager
    [SerializeField] private float difficultyScaler;
    [SerializeField] private float frequency;

    [Header("Pre wawe Seting")]
    [Tooltip("First enemy type to be spawned and his count")]
    [SerializeField] private GameObject preWavePrefab;
    [SerializeField] private int preWaveCount;

    [Header("Wawe Seting")]
    [Tooltip("List of enemies and their counts (these enemies are spawned in random order)")]
    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    [SerializeField] List<int> enemyCount = new List<int>();

    [Header("Post wawe Seting")]
    [Tooltip("Last enemy type to be spawned and his count")]
    [SerializeField] private GameObject postWavePrefab;
    [SerializeField] private int postWaveCount;

    private void Awake()
    {
        if (enemyCount.Count > prefabs.Count) Debug.LogError("Missing prefabs on SpawnPoint");
        App.levelManager.prepareWaveStartEvent.AddListener(PrepareWave);
        this.model = new SpawnPoint(frequency, transform.position,enemyCount,preWaveCount, postWaveCount,startInWave, activeWaveCount);
    }

    private void SpawnEnemy()
    {

        switch (model.state)
        {
            case SpawnPointState.prewave:

                if (model.isOutOfEnemies())
                {
                    model.ChangeState(SpawnPointState.wave);
                    SpawnEnemy();
                }
                else
                {
                    Instantiate(preWavePrefab, transform);
                    App.audioManager.Play("EnemySpawn");
                    model.currentTotalCountOfEnemies--;
                }
                break;
            case SpawnPointState.wave:

                if (model.isOutOfEnemies())
                {
                    model.ChangeState(SpawnPointState.postwave);
                    SpawnEnemy();
                }
                else
                {
                    Instantiate(prefabs[model.GetEnemyIndex()], transform);
                }
                break;
            case SpawnPointState.postwave:

                if (model.isOutOfEnemies()) CancelInvoke("SpawnEnemy");

                else
                {
                    Instantiate(postWavePrefab, transform);
                    model.currentTotalCountOfEnemies--;
                }
                break;
        }

    }

    public void PrepareWave(int wave)
    {
        if (wave == model.startWave) App.levelManager.startWaveEvent.AddListener(StartSpawning);
        if (model.IsActiveInThisWawe(wave))
        {
            //TODO: if scaler stays property of spawnpoint move it to model
            if (wave != model.startWave) model.ScaleEnemyCount(difficultyScaler);
            App.levelManager.AddEnemies(model.GetCountOfEnemiesToSpawnInWawe());
        }
        if(wave >= (model.startWave + model.countOfWaves))
        {
            App.levelManager.startWaveEvent.RemoveListener(StartSpawning);
            App.levelManager.prepareWaveStartEvent.RemoveListener(PrepareWave);
        }

             
        model.ChangeState(SpawnPointState.prewave);

    }

    public void StartSpawning()
    {    
        InvokeRepeating("SpawnEnemy", 0, model.frequency);
    }

}