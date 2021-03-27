using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScreen : ScreenBase
{
    public override void Show()
    {
        App.screenManager.SetGameState(GameState.paused);
        App.CameraManager.DisableCamera();
        App.audioManager.StopAmbient();
        App.audioManager.Play("PauseSound");
        Time.timeScale = 0;
        base.Show();
    }

    public override void Hide()
    {
        App.audioManager.Play("UIButtonClicked");
        App.screenManager.SetGameState(GameState.running);
        App.CameraManager.EnableCamera();
        Time.timeScale = 1;
        App.audioManager.Stop("PauseSound");
        StartCoroutine(App.audioManager.PlayAmbient());
        base.Hide();
    }

    public void HideToMenu()
    {
        App.audioManager.Play("UIButtonClicked");
        App.screenManager.SetGameState(GameState.menu);
        base.Hide();
        App.screenManager.Hide<InGameScreen>();
        App.screenManager.Show<MenuScreen>();
        App.gameManager.StartCurrentSceneUnloading();
    }

    public void SettingsButtonClicked()
    {
        App.screenManager.Show<SettingsScreen>();
    }
}