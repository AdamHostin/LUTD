using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : EndLevelScreen
{
    public void NextLevel()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartSceneUnloading(App.screenManager.GetSceneToUnload());
        App.gameManager.StartSceneLoading(App.gameManager.GetNextLevel());
        Hide();
    }
}