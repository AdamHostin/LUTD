using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScreen : ScreenBase
{
    public void RestartLevel()
    {
        App.audioManager.Play("UIButtonClicked");
        App.audioManager.StopLevelFailedSound();
        App.gameManager.StartSceneUnloading(App.screenManager.GetSceneToUnload());
        App.gameManager.StartSceneLoading(App.screenManager.GetSceneToUnload());
        Time.timeScale = 1;
        Hide();
    }

    public void Menu()
    {
        App.audioManager.Play("UIButtonClicked");
        App.audioManager.StopLevelFailedSound();
        App.gameManager.StartSceneUnloading(App.screenManager.GetSceneToUnload());
        App.screenManager.Hide<InGameScreen>();
        App.screenManager.Show<MenuScreen>();
        Hide();
    }
}