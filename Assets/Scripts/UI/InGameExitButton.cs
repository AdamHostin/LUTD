using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameExitButton : MonoBehaviour
{
    public void OnClick()
    {
        App.player.vaccinationEndedEvent.Invoke();
        App.player.healingEndedEvent.Invoke();
        App.unitCardManager.SwitchToCard(null);
    }
}