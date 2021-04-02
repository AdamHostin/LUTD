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

    private Image image;
    private Sprite originalImage;
    [SerializeField] private Sprite highlightImage;

    private void Start()
    {
        if (unitPrefab == null) Debug.LogError("UnitCard: unit prefab not set");
        if (transparentUnitPrefab == null) Debug.LogError("UnitCard: transparent unit prefab not set");

        transparentUnit = Instantiate(transparentUnitPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);

        image = GetComponent<Image>();
        originalImage = image.sprite;
    }

    public void OnClicked()
    {
        App.audioManager.Play("UIButtonClicked");

        if (App.player.ComparePlayerState(PlayerState.relocating))
            App.player.GetPickedUnit().GetComponent<UnitBehaviour>().DeselectUnit(false);

        if (App.unitCardManager.GetActiveCard() != this)
        {
            App.unitCardManager.SwitchToCard(this);
            App.player.SetUnitPrefab(unitPrefab, transparentUnit, cost);
        }
        else
        {
            App.unitCardManager.SwitchToCard(null);
            App.player.DeleteTransparentUnit();
            App.player.ChangeState(PlayerState.idle);
        }
    }

    public void Highlight()
    {
        image.sprite = highlightImage;
        App.audioManager.Play("UnitSelect");
    }

    public void Dehighlight()
    {
        image.sprite = originalImage;
    }

    public GameObject GetTransparentUnit()
    {
        return transparentUnit;
    }
}