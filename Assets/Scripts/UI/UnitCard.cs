using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab = null;
    [SerializeField]
    int cost = 0;

    public void SetUnitPrefab()
    {
        if (unitPrefab)
        {
            App.player.SetUnitPrefab(unitPrefab, cost);
        }
        else
            Debug.LogError("UnitCard: unit prefab not set");
    }

    public void Highlight()
    {
        //highlight the card
    }

    public void Dehighlight()
    {
        //opposite
    }
}