using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    private Base playerBase;    
    private int currentWave = 1;
    private int countOfEnemiesInCurrentWawe = 0;

    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private int waveCount = 1;

    public class PrepareWaveStartEvent : UnityEvent<int>{}
    public PrepareWaveStartEvent prepareWaveStartEvent = new PrepareWaveStartEvent();

    public UnityEvent startWaveEvent = new UnityEvent();

    private void Awake()
    {
        App.levelManager = this;
        App.screenManager.Hide<MenuScreen>();
        App.screenManager.Show<InGameScreen>();
        

    }

    private void Start()
    {
        StartCoroutine(StartWave());

    }
    IEnumerator StartWave()
    {
        prepareWaveStartEvent.Invoke(currentWave);
        //TODO: ask Matej how to call back listeners

        yield return new WaitForSeconds(timeBetweenWaves);

        startWaveEvent.Invoke();
    }


    public Base GetPlayerBase()
    {
        return playerBase;
    }

    public void SetPlayerBase(Base playerBase)
    {
        this.playerBase = playerBase;
    }

    public void AddEnemies(int enemyCount)
    {
        countOfEnemiesInCurrentWawe += enemyCount; 
    }

    public void EnemyDied()
    {
        countOfEnemiesInCurrentWawe--;
        if (countOfEnemiesInCurrentWawe > 0) return;
        EndWave();
    }

    void EndWave()
    {
        //Heandle end of wave
        currentWave++;
        if (currentWave > waveCount) Debug.Log("Level end Success");
        else StartCoroutine(StartWave());
    }
    

}
