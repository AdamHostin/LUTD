using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCardManager : MonoBehaviour
{
    private UnitCard[] unitCards;

    private void Start()
    {
        App.unitCardManager = this;
        unitCards = GetComponentsInChildren<UnitCard>(true);
    }

    public void DehighlightAll()
    {
        foreach (UnitCard card in unitCards)
        {
            card.Dehighlight();
        }
    }
}