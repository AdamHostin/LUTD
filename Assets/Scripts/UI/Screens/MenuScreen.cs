using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenBase
{
    [SerializeField]
    string level1;
    [SerializeField]
    string testLevel;

    public override void Show()
    {
        App.screenManager.SetGameState(GameState.menu);
        base.Show();
    }

    public override void Hide()
    {
        App.screenManager.SetGameState(GameState.running);
        base.Hide();
    }

    public void StartButtonClicked()
    {
        App.gameManager.StartSceneLoading(level1);
    }

    public void TestLevelButtonClicked()
    {
        App.gameManager.StartSceneLoading(testLevel);
    }
}