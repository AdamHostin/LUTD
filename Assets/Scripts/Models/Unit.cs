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
            OnUnitPlace();
        }

        public void AddEnemy(Enemy enemy)
        {
            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
                enemy.enemyDeathEvent.AddListener(SubstractEnemy);
                behaviour.StartShooting();
            }            
        }

        public void SubstractEnemy(Enemy enemy)
        {
            enemiesInRange.Remove(enemy);
            enemy.enemyDeathEvent.RemoveListener(SubstractEnemy);
            if (enemiesInRange.Count == 0) state = UnitState.idle;
        }

        public Enemy GetTarget()
        {
            RaycastHit hit;
            foreach (Enemy e in enemiesInRange)
            {
                if (e.state == EnemyState.dying) continue;
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
            Mathf.Clamp(unitxp, 0, xpToNxtLvl[xpToNxtLvl.Count - 1]);
            if (unitxp >= xpToNxtLvl[unitLvl+1])
            {
                //TODO: something
                unitLvl++;
                Debug.Log("Current lvl " + unitLvl);
            }
            if (unitLvl == xpToNxtLvl.Count-1)
            {
                behaviour.xpBar.OnUIUpdate(1f, unitxp, xpToNxtLvl[unitLvl]);
            }
            else
            {
                behaviour.xpBar.OnUIUpdate(((float)(unitxp - xpToNxtLvl[unitLvl]) / (xpToNxtLvl[unitLvl + 1] - xpToNxtLvl[unitLvl])), unitxp, xpToNxtLvl[unitLvl + 1]);

            }

        }

        public bool GetDamage(int damage, int infection = 0)
        {
            hp -= damage;
            toxicityResistance -= infection;

            if (hp <= 0)
            {
                if (behaviour != null)
                {
                    
                    onDamagableDeath.Invoke(this);
                    
                    behaviour.Die();
                }
                return false;
            }
            else
            {
                behaviour.hpBar.OnUIUpdate(((float)hp / maxHp), hp, maxHp);
            }
            if (toxicityResistance <= 0)
            {
                if (behaviour != null)
                {                   
                    onDamagableDeath.Invoke(this);
                    behaviour.GetInfected();
                }
                return false;
            }
            else
            {
                behaviour.toxicityBar.OnUIUpdate(((float)(maxToxicityResistance - toxicityResistance) / maxToxicityResistance), toxicityResistance, maxToxicityResistance);

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
            App.levelManager.damagablePlacedEvent.Invoke(this);
            CheckEnemiesOnPlace();
        }

        public void OnUnitPick()
        {
            behaviour.gameObject.SetActive(false);
            onDamagableDeath.Invoke(this);
        }

        private void CheckEnemiesOnPlace()
        {
            Collider[] colls = Physics.OverlapSphere(GetPosition(), range);
            foreach (var coll in colls)
            {                
                if (coll.transform.tag == "Enemy")
                {
                    AddEnemy(coll.GetComponent<EnemyBehaviour>().GetModel());
                }
                
            }
        }
    }
}


