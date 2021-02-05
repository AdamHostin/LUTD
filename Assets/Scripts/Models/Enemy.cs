using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class Enemy
{
    private int attack;
    private int hp;
    private Base target;
    public EnemyState state;

    public Enemy(int hp, int attack)
    {
        this.hp = hp;
        this.attack = attack;
        target = App.levelManager.GetPlayerBase();
        state = EnemyState.moving;
    }

    public void AttackBase()
    {
        if (target == null)
        {
            state = EnemyState.moving;
            return;
        }
        if (target?.ResolveAttack(attack) == false)
        {
            target = null;
        }

    }

    public Vector3 GetTargetPosition()
    {
        return new Vector3(target.pos.x,0,target.pos.z);
    }
}