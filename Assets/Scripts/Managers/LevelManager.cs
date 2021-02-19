using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.Events;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    private Base playerBase;
    private List<Unit> units =  new List<Unit>();
    private List<IDamagable> obstacles = new List<IDamagable>();

    private int currentWave = 1;
    private int countOfEnemiesInCurrentWawe = 0;

    [Header("Level boundrees setting")]
    [SerializeField] Vector3 maxClampPos;
    [SerializeField] Vector3 minClampPos;
    [SerializeField] Vector3 minClampZoom;
    [SerializeField] Vector3 maxClampZoom;

    [SerializeField] float minOrtograficSize;
    [SerializeField] float maxOrtograficSize;

    [Header("Wave setting")]
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
        App.CameraManager.SetNewWorlBoundrees(maxClampPos,minClampPos,
                                              maxClampZoom, minClampZoom, 
                                              minOrtograficSize,maxOrtograficSize);
        App.CameraManager.EnableCamera();
        

    }

    private void Start()
    {
        StartCoroutine(StartWave());

    }
    IEnumerator StartWave()
    {
        prepareWaveStartEvent.Invoke(currentWave);

        Debug.Log(countOfEnemiesInCurrentWawe);
        yield return new WaitForSeconds(timeBetweenWaves);

        startWaveEvent.Invoke();
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void SubstractUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void AddObstacke(IDamagable obstacle)
    {
        obstacles.Add(obstacle);
    }

    public void SubstractObstacle(IDamagable obstacle)
    {
        obstacles.Remove(obstacle);
    }

    public Base GetPlayerBase()
    {
        return playerBase;
    }

    public void SetPlayerBase(Base playerBase)
    {
        this.playerBase = playerBase;
    }

    public void AddEnemies(int enemyCount=1)
    {
        countOfEnemiesInCurrentWawe += enemyCount; 
    }

    public void EnemyDied()
    {
        countOfEnemiesInCurrentWawe--;
        if (countOfEnemiesInCurrentWawe > 0) return;
        EndWave();
    }

    private void EndWave()
    {
        //Heandle end of wave
        currentWave++;
        if (currentWave > waveCount) EndLevel(true);
        else StartCoroutine(StartWave());
    }
    
    public void EndLevel(bool res)
    {
        if (res) Debug.Log("Level end Success");
        else Debug.Log("Level end failure");
    }

}
