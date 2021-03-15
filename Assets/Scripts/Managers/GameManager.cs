using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Models;

public class GameManager : MonoBehaviour
{
    public DefaultPlayerValues defaultVals;

    private void Start()
    {
        App.gameManager = this;
        StartCoroutine(LoadSelectedScene("UIScene"));
        App.player = new Player(defaultVals.coins, defaultVals.vaccines, defaultVals.vaccineEffectivnes);
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

    public void StartSceneLoading(string sceneName)
    {
        StartCoroutine(LoadSelectedScene(sceneName));
    }

    public void StartSceneUnloading(string sceneName)
    {
        StartCoroutine(UnloadSelectedScene(sceneName));
    }
}