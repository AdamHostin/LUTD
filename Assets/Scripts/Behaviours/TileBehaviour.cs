﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    Tile model;
    [SerializeField]
    private float verticalSpawnOffset = 0.5f;
    private bool isOccupied = false;

    private void Awake()
    {
        model = new Tile(GetComponentInChildren<Transform>().position + new Vector3(0, verticalSpawnOffset, 0), this);
    }

    public void OnMouseDown()
    {
        if (!isOccupied)
        {
            if (App.player.ComparePlayerState(PlayerState.placing))
            {
                App.player.PlaceUnit(model.GetSpawnPosition());
                isOccupied = true;
            }
            else if (App.player.ComparePlayerState(PlayerState.relocating))
            {
                App.player.GetPickedUnit().GetComponent<UnitBehaviour>().Relocate(model.GetSpawnPosition());
                App.player.DeleteTransparentUnit();
            }
        } 
    }

    public void OnMouseEnter()
    {
        if (!App.player.ComparePlayerState(PlayerState.idle) && !isOccupied)
        {
            App.player.SetTransparentUnitPosition(model.GetSpawnPosition());
        }
    }
}