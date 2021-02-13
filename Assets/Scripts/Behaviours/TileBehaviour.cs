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
        model = new Tile(GetComponentInChildren<Transform>().position + new Vector3(0, verticalSpawnOffset, 0), this);
    }

    public void OnMouseDown()
    {
        if (App.player.canPlace && !isOccupied)
        {
            App.player.PlaceUnit(model.GetSpawnPosition());
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
}