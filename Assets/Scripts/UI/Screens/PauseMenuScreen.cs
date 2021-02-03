using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScreen : ScreenBase
{
    public override void Show()
    {
        App.screenManager.SetGameState(GameState.paused);
        Time.timeScale = 0;
        base.Show();
    }

    public override void Hide()
    {
        App.screenManager.SetGameState(GameState.running);
        Time.timeScale = 1;
        base.Hide();
    }
}