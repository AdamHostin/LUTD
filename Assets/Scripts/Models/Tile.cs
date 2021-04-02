using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private TileBehaviour behaviour;
    private Vector3 spawnPosition;

    public Tile(Vector3 position, TileBehaviour behaviour)
    {
        this.spawnPosition = position;
        spawnPosition.y = .5f;
        this.behaviour = behaviour;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
