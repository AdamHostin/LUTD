using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenBase
{
    public void StartButtonClicked()
    {
        App.gameManager.StartSceneLoading("Level1");
    }
}