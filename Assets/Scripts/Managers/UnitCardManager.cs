using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCardManager : MonoBehaviour
{
    UnitCard activeCard = null;

    private void Awake()
    {
        App.unitCardManager = this;
    }

    public void SwitchToCard(UnitCard newCard)
    {
        activeCard?.Dehighlight();
        
        if (newCard != null)
        {
            newCard.Highlight();
            activeCard = newCard;
        }
        else
            activeCard = null;
    }

    public UnitCard GetActiveCard()
    {
        return activeCard;
    }
}