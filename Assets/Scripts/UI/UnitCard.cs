using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab = null;
    [SerializeField]
    int cost = 0;
    bool isOn = false;

    public void OnClicked()
    {
        if (!isOn)
        {
            App.unitCardManager.DehighlightAll();
            Highlight();
            SetUnitPrefab();
            isOn = true;
        }
        else
        {
            Dehighlight();
            App.player.canPlace = false;
            isOn = false;
        }
    }

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
        //Add highlight the card
    }

    public void Dehighlight()
    {
        //Add dehighlight the card
    }
}