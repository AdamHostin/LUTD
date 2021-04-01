using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int sceneNumber;
    public int coins;
    public int vaccines;
    public int medkits;

    public PlayerData(int sceneNumber, int coins, int vaccines, int medkits)
    {
        this.sceneNumber = sceneNumber;
        this.coins = coins;
        this.vaccines = vaccines;
        this.medkits = medkits;
    }
}
