using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class Enemy
{
    private int attack = 1;
    private int hp = 10;
    private int range = 2;
    private Base target;
    private int range = 1;

    public Enemy()
    {
        target = App.levelManager.GetPlayerBase();
    }

    private void AttackBase()
    {
        target?.ResolveAttack(attack);
    }

    public Vector3 GetTargetPosition()
    {
        return new Vector3(target.pos.x,0,target.pos.z);
    }
}