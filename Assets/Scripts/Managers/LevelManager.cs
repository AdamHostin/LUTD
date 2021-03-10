using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.Events;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    private Base playerBase;

    private int currentWave = 1;
    private int countOfEnemiesInCurrentWawe = 0;

    private LevelState levelState = LevelState.betweenWave;      //Set to betweenWave fot testing

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
    public class DamagablePlacedEvent : UnityEvent<IDamagable> { }
    public DamagablePlacedEvent damagablePlacedEvent = new DamagablePlacedEvent();

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

    public void InstatiateUnit(GameObject prefab, Vector3 position, GameObject transparentSelf, TileBehaviour tile)
    {
        GameObject unit = Instantiate(prefab, position, Quaternion.identity);
        unit.GetComponent<UnitBehaviour>().GetModel().SetTransparentSelf(transparentSelf);
        unit.GetComponent<UnitBehaviour>().GetModel().SetCurrentTile(tile);
    }

    public void SetLevelState(LevelState state)
    {
        levelState = state;
    }

    public bool CompareLevelState(LevelState state)
    {
        return levelState == state ? true : false;
    }
}
