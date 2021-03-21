using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScreen : ScreenBase
{
    public void ReturnToMenu()
    {
        App.gameManager.StartSceneUnloading("Level1");
    }

    public override void Show()
    {
        base.Show();

    }

    public override void Hide()
    {
        base.Hide();
        App.CameraManager.DisableCamera();

    }

    public void PauseButtonClicked()
    {
        App.screenManager.Show<PauseMenuScreen>();
    }
}