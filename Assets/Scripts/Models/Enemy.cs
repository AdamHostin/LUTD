using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class Enemy
{
    private int attack;
    private int hp;
    private Base target;

    public Enemy(int hp, int attack)
    {
        this.hp = hp;
        this.attack = attack;
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