using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Player
{
    GameObject pickedUnitPrefab;
    GameObject transparentUnitInstance;
    Transform transparentUnitTransform;
    public bool canPlace = false;
    public int tempCost = 0;

    private void Awake()
    {
        App.player = this;
    }

    public void SetUnitPrefab(GameObject prefab, GameObject transparentPrefab, int cost)
    {
        pickedUnitPrefab = prefab;
        if (cost <= coins)
        {
            canPlace = true;
            tempCost = cost;
            transparentUnitInstance = Instantiate(transparentPrefab);
            transparentUnitTransform = transparentUnitInstance.GetComponent<Transform>();
        }
        else
        {
            canPlace = false;
        }   
    }

    public void PlaceUnit(Vector3 position)
    {
        Instantiate(pickedUnitPrefab, position, Quaternion.identity);
        coins -= tempCost;
        canPlace = false;
        App.unitCardManager.SwitchToCard(null);
        DeleteTransparentUnit();
    }

    public void SetTransparentUnitPosition(Vector3 position)
    {
        transparentUnitTransform.position = position;
    }

    public void DeleteTransparentUnit()
    {
        Destroy(transparentUnitInstance);
    }
}