using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class App 
{
    public static ScreenManager screenManager;
    public static GameManager gameManager;
    public static LevelManager levelManager;
    public static Player player;
    public static UnitCardManager unitCardManager;
    public static CameraManager CameraManager;
    public static SaveSystem SaveSystem = new SaveSystem();
    public static AudioManager audioManager;
}