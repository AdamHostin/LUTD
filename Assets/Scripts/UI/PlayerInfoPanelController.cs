using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerInfoPanelController : MonoBehaviour
{
    public string coinPrefix = "coins: ";
    public string vaccinePrefix = "vaccines: ";

    [Header("programer stuf")]
    public TextMeshProUGUI coinTMP;
    public TextMeshProUGUI vaccinesTMP;

    private void OnEnable()
    {
        App.player.SetPlayerInfoUI(this);
    }

    public void UpdateCoinText(int newCoinVal)
    {
        coinTMP.text = coinPrefix + newCoinVal;
    }

    public void UpdateVaccineText(int newVaccineVal)
    {
        vaccinesTMP.text = vaccinePrefix + newVaccineVal;
    }
}
