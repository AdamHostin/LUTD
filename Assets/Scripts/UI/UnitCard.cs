using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab = null;
    [SerializeField]
    GameObject transparentUnitPrefab = null;
    [SerializeField]
    int cost = 0;

    private void Start()
    {
        if (unitPrefab == null) Debug.LogError("UnitCard: unit prefab not set");
        if (transparentUnitPrefab == null) Debug.LogError("UnitCard: transparent unit prefab not set");
    }

    public void OnClicked()
    {
        if (!App.unitCardManager.GetActiveCard() == this)
        {
            App.unitCardManager.SwitchToCard(this);
            App.player.SetUnitPrefab(unitPrefab, transparentUnitPrefab, cost);
        }
        else
        {
            App.unitCardManager.SwitchToCard(null);
            App.player.canPlace = false;
            App.player.DeleteTransparentUnit();
        }
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