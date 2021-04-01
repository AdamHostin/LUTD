using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Models;

public class GameManager : MonoBehaviour
{
    public DefaultPlayerValues defaultVals;

    [SerializeField]
    private string[] levels;
    private int sceneIndex = 0;
    private PlayerData saveData;
    private bool hasSave = false;

    private void Start()
    {
        App.gameManager = this;
        StartCoroutine(LoadSelectedScene("UIScene"));
        saveData = App.SaveSystem.Load();
        if (saveData != null)
        {
            App.player = new Player(saveData.coins, saveData.vaccines, defaultVals.vaccineEffectivnes, saveData.medkits, defaultVals.medkitEffectivness);
            sceneIndex = saveData.sceneNumber;
            hasSave = true;
        }
        else
        {
            App.player = new Player(defaultVals.coins, defaultVals.vaccines, defaultVals.vaccineEffectivnes, defaultVals.medkits, defaultVals.medkitEffectivness);
        }
        
    }

    IEnumerator LoadSelectedScene(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
        }

    }

    IEnumerator UnloadSelectedScene(string sceneName)
    {
        AsyncOperation loading = SceneManager.UnloadSceneAsync(sceneName);
        while (!loading.isDone)
        {
            yield return null;
        }

    }

    public void StartLoadingFirstScene()
    {
        sceneIndex = 1;
        StartSceneLoading(levels[sceneIndex]);
    }

    public void StartSceneLoading(string sceneName)
    {
        StartCoroutine(LoadSelectedScene(sceneName));
    }

    public void StartSceneUnloading(string sceneName)
    {
        StartCoroutine(UnloadSelectedScene(sceneName));
    }

    public void StartCurrentSceneLoading()
    {
        StartCoroutine(LoadSelectedScene(levels[sceneIndex]));
    }

    public void StartCurrentSceneUnloading()
    {
        StartCoroutine(UnloadSelectedScene(levels[sceneIndex]));
    }

    public string GetNextLevel()
    {
        sceneIndex++;
        App.SaveSystem.Save();
        hasSave = true;
        return levels[sceneIndex];
    }

    public int GetSceneIndex()
    {
        return sceneIndex;
    }

    public void ReInitPlayer()
    {
        App.player.ReInitPlayer(defaultVals.coins, defaultVals.vaccines);
    }
}