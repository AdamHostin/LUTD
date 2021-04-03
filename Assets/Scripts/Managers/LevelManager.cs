using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private Base playerBase;

    private int currentWave = 1;
    private int countOfEnemiesInCurrentWawe = 0;

    private LevelState levelState = LevelState.betweenWave;      //Set to betweenWave fot testing

    [SerializeField] Vector3 cameraStartingPos;
    [SerializeField] Vector3 cameraStartingRotation ;

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
    [SerializeField] private float startingOffset;

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
        App.CameraManager.transform.position = cameraStartingPos;
        App.CameraManager.transform.rotation = Quaternion.Euler(cameraStartingRotation);
        StartCoroutine(StartingOffset());
    }

    IEnumerator StartingOffset()
    {
        yield return new WaitForSeconds(startingOffset);
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        prepareWaveStartEvent.Invoke(currentWave);

        yield return new WaitForSeconds(timeBetweenWaves);
        App.audioManager.Play("WaveStart");
        levelState = LevelState.wave;
        App.player.StopRelocating();
        if (countOfEnemiesInCurrentWawe == 0)
        {
            Debug.Log("!!!ALERT!!! WAVE WITH NO ENEMIES");
            EndWave();
        }
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
        App.audioManager.Play("WaveEnd");
        levelState = LevelState.betweenWave;
        //Heandle end of wave
        currentWave++;
        if (currentWave > waveCount) EndLevel(true);
        else StartCoroutine(StartWave());
    }
    
    public void EndLevel(bool res)
    {
        App.screenManager.SetSceneToUnload(SceneManager.GetSceneAt(2).name);
        if (res) 
        {
            App.audioManager.Play("LevelCompleted");
            App.screenManager.Show<WinScreen>();
        } 
        else
        {
            App.audioManager.Play("LevelFailed");
            Time.timeScale = 0f;
            App.screenManager.Show<DeathScreen>();
        }
    }

    public void InstatiateUnit(GameObject prefab, Vector3 position, GameObject transparentSelf, TileBehaviour tile)
    {
        GameObject unit = Instantiate(prefab, position, Quaternion.identity);
        unit.transform.rotation = transparentSelf.transform.rotation; ;
        unit.transform.parent = transform;
        var behaviour = unit.GetComponent<IPlacebleBehaviour>();
        IPlacebla unitModel = unit.GetComponent<IPlacebleBehaviour>().GetPlaceblaModel();
        unitModel.SetTransparentSelf(transparentSelf);
        unitModel.SetCurrentTile(tile);
        App.audioManager.Play("UnitSpawn");
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
