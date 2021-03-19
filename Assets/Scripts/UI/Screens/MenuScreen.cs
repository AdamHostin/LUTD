using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenBase
{
    [SerializeField] GameObject continnueButton;

    public override void Show()
    {
        App.screenManager.SetGameState(GameState.menu);
        base.Show();
        if (App.gameManager.GetSceneIndex() != 0) continnueButton.SetActive(true);
        else continnueButton.SetActive(false);
    }

    public override void Hide()
    {
        App.screenManager.SetGameState(GameState.running);
        base.Hide();
    }

    public void PlayButtonClicked()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartLoadingFirstScene();
        App.gameManager.ReInitPlayer();
        Time.timeScale = 1;
    }

    public void TestLevelButtonClicked()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartSceneLoading("TestLevel");
        Time.timeScale = 1;
    }

    public void CointinueButtonClicked()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartCurrentSceneLoading();
        Time.timeScale = 1;
    }

    public void SettingsButtonClicked()
    {
        App.screenManager.Show<SettingsScreen>();
    }
}