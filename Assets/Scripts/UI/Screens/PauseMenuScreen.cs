using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScreen : ScreenBase
{
    public override void Show()
    {
        App.screenManager.SetGameState(GameState.paused);
        App.CameraManager.DisableCamera();
        Time.timeScale = 0;
        base.Show();
    }

    public override void Hide()
    {
        App.screenManager.SetGameState(GameState.running);
        App.CameraManager.EnableCamera();
        Time.timeScale = 1;
        base.Hide();
    }

    public void HideToMenu()
    {
        App.screenManager.SetGameState(GameState.menu);
        base.Hide();
        App.screenManager.Hide<InGameScreen>();
        App.screenManager.Show<MenuScreen>();
        App.gameManager.StartCurrentSceneUnloading();
    }
}