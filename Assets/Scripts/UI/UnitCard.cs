using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab = null;
    [SerializeField]
    int cost = 0;

    public void OnClicked()
    {
        if (!App.unitCardManager.GetActiveCard() == this)
        {
            App.unitCardManager.SwitchToCard(this);
            SetUnitPrefab();
        }
        else
        {
            App.unitCardManager.SwitchToCard(null);
            App.player.canPlace = false;
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