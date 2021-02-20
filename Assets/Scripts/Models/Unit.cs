using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Models
{
    public class Unit : IDamagable
    {
        int hp;
        int maxHp;
        int toxicityResistance;
        int maxToxicityResistance;
        int attack;
        float range;

        int unitLvl = 0;
        int unitxp = 0;

        List<int> xpToNxtLvl;
        List<Enemy> enemiesInRange = new List<Enemy>();

        Vector3 gunPos;
        UnitBehaviour behaviour;
        //UnitTriggerAdapter adapter;

        public class RangeChangeEvent : UnityEvent<float> { }
        public RangeChangeEvent rangeChangeEvent = new RangeChangeEvent();
        public class DamagableDeathEvent : UnityEvent<IDamagable> { }
        public DamagableDeathEvent onDamagableDeath = new DamagableDeathEvent();


        public UnitState state;

        public Unit(int hp, int toxicityResistance, int attack, float range, Vector3 gunPos, List<int> xpToNxtLvl, UnitBehaviour behaviour)
        {
            this.hp = maxHp = hp;
            this.toxicityResistance = maxToxicityResistance = toxicityResistance;
            this.attack = attack;
            this.range = range;
            this.xpToNxtLvl = xpToNxtLvl;
            this.gunPos = gunPos;
            this.behaviour = behaviour;
            state = UnitState.idle;

            behaviour.hpBar.OnUIUpdate(1f, maxHp, maxHp);
            behaviour.toxicityBar.OnUIUpdate(0f, toxicityResistance, toxicityResistance);
            behaviour.xpBar.OnUIUpdate(1f, 0, xpToNxtLvl[1]);

        }

        public void SetAdapter(UnitTriggerAdapter adapter)
        {
            //this.adapter = adapter;
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
                    if (hit.transform.tag == "Enemy") return e;
                }
            }
            return null;
        }
        // returns true if enemy was hit 
        public bool Shoot()
        {
            Enemy target = GetTarget();
            if (target == null) return false;

            Addxp(target.GetDamage(attack));

            return true;
        }

        void Addxp(int xp)
        {
            if (unitLvl == xpToNxtLvl.Count-1) return;
            unitxp += xp;
            if (unitxp >= xpToNxtLvl[unitLvl+1])
            {
                //TODO: something
                unitLvl++;
                Debug.Log("Current lvl " + unitLvl);
            }
            behaviour.xpBar.OnUIUpdate(((float)(unitxp-xpToNxtLvl[unitLvl]) / (xpToNxtLvl[unitLvl + 1] - xpToNxtLvl[unitLvl])), unitxp, xpToNxtLvl[unitLvl + 1]);

        }

        public bool GetDamage(int damage, int infection = 0)
        {
            hp -= damage;
            toxicityResistance -= infection;
            //Debug.Log("Unit infection: " + toxicityResistance + " Unit hp: " + hp);
            behaviour.hpBar.OnUIUpdate(((float)hp / maxHp),hp,maxHp);
            behaviour.toxicityBar.OnUIUpdate(((float)(maxToxicityResistance - toxicityResistance + Mathf.Epsilon ) / maxToxicityResistance),  toxicityResistance, maxToxicityResistance);
            if (hp <= 0)
            {
                if (behaviour != null) behaviour.Die();
                onDamagableDeath.Invoke(this);
                return false;
            }
            if (toxicityResistance <= 0)
            {
                if (behaviour != null) behaviour.GetInfected();
                onDamagableDeath.Invoke(this);
                return false;
            }

            return true;
        }

        public Vector3 GetPosition()
        {
            return behaviour.transform.position;
        }

        public void SubscribeToDeathEvent(Enemy enemy)
        {
            onDamagableDeath.AddListener(enemy.OnDamagableInAwarenessRangeDeath);
        }

        public void UnsubscribeToDeathEvent(Enemy enemy)
        {
            onDamagableDeath.RemoveListener(enemy.OnDamagableInAwarenessRangeDeath);
        }

        public void OnUnitPlace()
        {

        }

        public void OnUnitPick()
        {
            behaviour.gameObject.SetActive(false);
            onDamagableDeath.Invoke(this);
        }
    }
}


