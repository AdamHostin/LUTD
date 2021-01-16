using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        App.screenManager.Hide<MenuScreen>();
        App.screenManager.Show<InGameScreen>();
    }
}
