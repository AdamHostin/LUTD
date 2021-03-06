using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab = null;
    [SerializeField]
    GameObject transparentUnitPrefab = null;
    [SerializeField]
    int cost = 0;

    GameObject transparentUnit;

    private void Start()
    {
        if (unitPrefab == null) Debug.LogError("UnitCard: unit prefab not set");
        if (transparentUnitPrefab == null) Debug.LogError("UnitCard: transparent unit prefab not set");

        transparentUnit = Instantiate(transparentUnitPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
    }

    public void OnClicked()
    {
        if (App.player.ComparePlayerState(PlayerState.relocating))
            App.player.GetPickedUnit().GetComponent<UnitBehaviour>().DeselectUnit();

        if (!App.unitCardManager.GetActiveCard() == this)
        {
            App.unitCardManager.SwitchToCard(this);
            App.player.SetUnitPrefab(unitPrefab, transparentUnit, cost);
        }
        else
        {
            App.unitCardManager.SwitchToCard(null);
            App.player.DeleteTransparentUnit();
        }
    }

    public void Highlight()
    {
        GetComponent<Image>().color = new Color32(0, 255, 255, 255);
    }

    public void Dehighlight()
    {
        GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public GameObject GetTransparentUnit()
    {
        return transparentUnit;
    }
}