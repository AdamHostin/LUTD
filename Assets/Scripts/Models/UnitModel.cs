using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitModel
{
    int hp;
    int attack;
    float range;

    int unitLvl = 0;
    int unitxp = 0;

    List<int> xpToNxtLvl;
    List<Enemy> enemiesInRange = new List<Enemy>();

    Vector3 gunPos;
    UnitBehaviour behaviour;
    UnitTriggerAdapter adapter;

    public class RangeChangeEvent : UnityEvent<float> { }
    public RangeChangeEvent rangeChangeEvent = new RangeChangeEvent();


    public UnitState state;

    public UnitModel(int hp, int attack, float range ,Vector3 gunPos ,List<int> xpToNxtLvl, UnitBehaviour behaviour)
    {
        this.hp = hp;
        this.attack = attack;
        this.range = range;
        this.xpToNxtLvl = xpToNxtLvl;
        this.gunPos = gunPos;
        this.behaviour = behaviour;
        state = UnitState.idle;
        
    }

    public void SetAdapter(UnitTriggerAdapter adapter)
    {
        this.adapter = adapter;
        rangeChangeEvent.AddListener(adapter.SetNewRange);
        rangeChangeEvent.Invoke(range);
    }

    public void AddEnemy(Enemy enemy)
    {
        enemiesInRange.Add(enemy);
        behaviour.StartShooting();
        
    }

    public void SubstractEnemy(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
        if (enemiesInRange.Count == 0) state = UnitState.idle;
    }

    bool Enemycheck(Enemy e)
    {
        return (e.behaviour == null);
    }

    public Enemy GetTarget()
    {
        RaycastHit hit;
        enemiesInRange.RemoveAll(Enemycheck);
        foreach (Enemy e in enemiesInRange)
        {

            Ray shootingRay = new Ray(gunPos, (e.GetPosition() - gunPos));
            if (Physics.Raycast(shootingRay, out hit))
            {
               if(hit.transform.tag == "Enemy") return e;
            }
        }
        state = UnitState.idle;
        return null;
    }

    public void shoot()
    {
        Enemy target = GetTarget();
        if (target == null) return;

        Addxp(target.GetDamage(attack));
    }

    void Addxp(int xp)
    {
        if (unitLvl == xpToNxtLvl.Count) return;
        unitxp += xp;
        if (unitxp >= xpToNxtLvl[unitLvl])
        {
            //TODO: something
            unitLvl++;
            Debug.Log("Current lvl " + unitLvl);
        }
        
    }
}
