using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenBase
{
    [SerializeField] GameObject continnueButton;

    public override void Show()
    {
        App.screenManager.SetGameState(GameState.menu);
        App.audioManager.PlayMenuSound();
        base.Show();
        if (App.gameManager.GetSceneIndex() != 0) continnueButton.SetActive(true);
        else continnueButton.SetActive(false);
    }

    public override void Hide()
    {
        App.screenManager.SetGameState(GameState.running);
        App.audioManager.StopMenuSound();
        base.Hide();
    }

    public void PlayButtonClicked()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartLoadingFirstScene();
        App.gameManager.ReInitPlayer();
        Time.timeScale = 1;
        StartCoroutine(App.audioManager.PlayAmbient());
    }

    public void TestLevelButtonClicked()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartSceneLoading("TestLevel");
        Time.timeScale = 1;
        StartCoroutine(App.audioManager.PlayAmbient());
    }

    public void CointinueButtonClicked()
    {
        App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartCurrentSceneLoading();
        Time.timeScale = 1;
        StartCoroutine(App.audioManager.PlayAmbient());
    }

    public void SettingsButtonClicked()
    {
        App.screenManager.Show<SettingsScreen>();
    }
}