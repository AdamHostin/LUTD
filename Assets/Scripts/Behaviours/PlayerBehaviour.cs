using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Player
{
    GameObject pickedUnitPrefab;
    public bool canPlace = false;
    public int tempCost = 0;

    private void Awake()
    {
        App.player = this;
    }

    public void SetUnitPrefab(GameObject prefab, int cost)
    {
        pickedUnitPrefab = prefab;
        if (cost <= coins)
        {
            canPlace = true;
            tempCost = cost;
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
    }
}