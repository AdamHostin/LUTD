using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class FlamethrowerTowerUnit : Unit
{
    List<Enemy> enemiesToRemove = new List<Enemy>(); 

    public FlamethrowerTowerUnit(int hp, int attack, float range, Vector3 gunPos, float scaler, List<int> xpToNxtLvl, UnitBehaviour behaviour) : 
        base(hp, attack, range, gunPos, scaler, xpToNxtLvl, behaviour)
    {
    }


    public override bool Shoot()
    {
        RemoveDeadEnemies();
        foreach (Enemy target in enemiesInRange)
        {
            
            if (target == null || target.behaviour == null) continue;

            Addxp(target.GetDamage(attack));
            App.audioManager.Play("UnitShoot");
        }
        RemoveDeadEnemies();

        return true;
    }

    private void RemoveDeadEnemies()
    {
        foreach (Enemy e in enemiesToRemove)
        {
            enemiesInRange.Remove(e);
        }
    }

    public override void SubstractEnemy(Enemy enemy)
    {
        //enemiesInRange.Remove(enemy);
        enemiesToRemove.Add(enemy);
        enemy.enemyDeathEvent.RemoveListener(SubstractEnemy);
        if (enemiesInRange.Count == 0) ChangeState(UnitState.idle);
    }

    public override void SetAdapter(UnitTriggerAdapter adapter)
    {
        rangeChangeEvent.AddListener(((FlamethrowerTriggerAdapter) adapter).SetNewRange);
        rangeChangeEvent.Invoke(range);
        OnUnitPlace();
    }
}
