using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScreen : ScreenBase
{
    public void ReturnToMenu()
    {
        App.gameManager.StartSceneUnloading("Level1");
    }
}