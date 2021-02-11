using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private TileBehaviour behaviour;
    private Vector3 position;

    public Tile(Vector3 position, TileBehaviour behaviour)
    {
        this.position = position;
        this.behaviour = behaviour;
    }
}
