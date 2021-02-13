using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class Enemy
{
    private int attack;
    private int hp;
    private int xp;
    private Base target;
    public EnemyState state;

    public EnemyBehaviour behaviour;

    public Enemy(int hp, int attack, int xp ,EnemyBehaviour behaviour)
    {
        this.hp = hp;
        this.attack = attack;
        this.xp = xp;
        this.behaviour = behaviour;

        //TODO: change Target searching
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

    public Vector3 GetPosition()
    {
        return behaviour.transform.position;
    }

    public int GetDamage(int damage)
    {
        if (behaviour == null) return 0;
        hp -= damage;
        if (hp > 0) return 0;
        Debug.Log("Enemy death");
        App.levelManager.EnemyDied();
        behaviour.StartDying();
        return xp;
    }
}