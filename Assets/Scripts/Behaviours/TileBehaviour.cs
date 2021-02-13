using System.Collections;
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
        model = new Tile(transform.position, this);
    }

    public void OnMouseDown()
    {
        if (App.player.canPlace && !isOccupied)
        {
            App.player.PlaceUnit(GetSpawnPosition());
            isOccupied = true;
            //Add dehighlight
        } 
    }

    public void OnMouseEnter()
    {
        if (App.player.canPlace && !isOccupied)
        {
            //Add highlight
        }
    }

    public void OnMouseExit()
    {
        //Add dehighlight
    }

    public Vector3 GetSpawnPosition()
    {
        //Second part will be deleted when model has pivot up in the center
        return GetComponentInChildren<Transform>().position + new Vector3(0, verticalSpawnOffset, 0);
    }
}