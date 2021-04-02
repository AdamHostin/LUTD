using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerInfoPanelController : MonoBehaviour
{
    [Header("programer stuf")]
    public TextMeshProUGUI coinTMP;
    public TextMeshProUGUI vaccinesTMP;
    public TextMeshProUGUI medkitsTMP;

    private void OnEnable()
    {
        App.player.SetPlayerInfoUI(this);
    }

    public void UpdateCoinText(int newCoinVal)
    {
        coinTMP.text = newCoinVal.ToString();
    }

    public void UpdateVaccineText(int newVaccineVal)
    {
        vaccinesTMP.text = newVaccineVal.ToString();
    }

    public void UpdateMedkitsText(int newMedkitsVal)
    {
        medkitsTMP.text = newMedkitsVal.ToString();
    }
}
