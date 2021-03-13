using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScreen : ScreenBase
{
    public void RestartLevel()
    {
        App.gameManager.StartSceneUnloading(App.screenManager.GetSceneToUnload());
        App.gameManager.StartSceneLoading(App.screenManager.GetSceneToUnload());
        Hide();
    }

    public void Menu()
    {
        App.gameManager.StartSceneUnloading(App.screenManager.GetSceneToUnload());
        App.screenManager.Hide<InGameScreen>();
        App.screenManager.Show<MenuScreen>();
        Hide();
    }
}