﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    private Base playerBase;    
    private int currentWave = 1;

    public List<SpawnPointBehaviour> SpawnPoints = new List<SpawnPointBehaviour>();
    public float timeBetweenWaves = 10f;

    public class StartWaveEvent : UnityEvent<int>{}
    public StartWaveEvent startWaveEvent = new StartWaveEvent();

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
        yield return new WaitForSeconds(timeBetweenWaves);
        startWaveEvent.Invoke(currentWave);
    }


    public Base GetPlayerBase()
    {
        return playerBase;
    }

    public void SetPlayerBase(Base playerBase)
    {
        this.playerBase = playerBase;
    }



}
