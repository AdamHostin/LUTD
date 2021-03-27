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
        float scaler;

        int unitLvl = 0;
        int unitxp = 0;

        LayerMask layerMask;

        List<int> xpToNxtLvl;
        List<Enemy> enemiesInRange = new List<Enemy>();

        Vector3 gunPos;
        UnitBehaviour behaviour;
        //UnitTriggerAdapter adapter;

        //For relocating
        GameObject transparentSelf;
        TileBehaviour currentTile;

        string shootSound;

        public class RangeChangeEvent : UnityEvent<float> { }
        public RangeChangeEvent rangeChangeEvent = new RangeChangeEvent();
        public class DamagableDeathEvent : UnityEvent<IDamagable> { }
        public DamagableDeathEvent onDamagableDeath = new DamagableDeathEvent();


        public UnitState state;

        public Unit(int hp, int toxicityResistance, int attack, float range, Vector3 gunPos, float scaler, List<int> xpToNxtLvl, UnitBehaviour behaviour, string shootSound)
        {
            this.hp = maxHp = hp;
            this.toxicityResistance = maxToxicityResistance = toxicityResistance;
            this.attack = attack;
            this.range = range;
            this.xpToNxtLvl = xpToNxtLvl;
            this.gunPos = gunPos;
            this.scaler = scaler;
            this.behaviour = behaviour;
            this.shootSound = shootSound;
            state = UnitState.idle;

            behaviour.hpBar.OnUIUpdate(1f, maxHp, maxHp);
            behaviour.toxicityBar.OnUIUpdate(0f, toxicityResistance, toxicityResistance);
            behaviour.xpBar.OnUIUpdate(1f, 0, xpToNxtLvl[1]);

            layerMask = 0;
            layerMask = (1<<LayerMask.NameToLayer("Default") | 1<<LayerMask.NameToLayer("Enemy"));
        }

        public void SetAdapter(UnitTriggerAdapter adapter)
        {
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
                if (Physics.Raycast(shootingRay, out hit, layerMask))
                {
                    if (hit.collider.tag == "Enemy") return e;
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
            App.audioManager.Play(shootSound);

            return true;
        }

        void Addxp(int xp)
        {
            if (unitLvl == xpToNxtLvl.Count-1) return;
            unitxp += xp;
            Mathf.Clamp(unitxp, 0, xpToNxtLvl[xpToNxtLvl.Count - 1]);
            if (unitxp >= xpToNxtLvl[unitLvl+1])
            {
                LevelUp();
            }
            UpdateXpBar();
            
        }

        void LevelUp()
        {
            //Improve hp
            int newMaxHp = (int) Mathf.Ceil((scaler * maxHp));
            if (newMaxHp > maxHp) hp += (newMaxHp - maxHp);
            if (hp > newMaxHp) hp = newMaxHp;
            maxHp = newMaxHp;
            UpdateHpBar();

            //Improve toxicity resistance
            int newMaxToxicityResistance = (int)Mathf.Ceil((scaler * maxToxicityResistance));
            if (newMaxToxicityResistance > maxToxicityResistance) hp += (newMaxToxicityResistance - maxToxicityResistance);
            if (toxicityResistance > newMaxToxicityResistance) toxicityResistance = newMaxToxicityResistance;
            maxToxicityResistance = newMaxToxicityResistance;
            UpdateToxicityBar();

            //Improve range
            range *= scaler;
            rangeChangeEvent.Invoke(range);
            CheckEnemiesOnPlace();

            unitLvl++;
            Debug.Log("Current lvl " + unitLvl);
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
                UpdateHpBar();
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
                UpdateToxicityBar();
            }
            return true;
        }

        void UpdateHpBar()
        {
            behaviour.hpBar.OnUIUpdate(((float)hp / maxHp), hp, maxHp);
        }

        void UpdateXpBar()
        {
            if (unitLvl == xpToNxtLvl.Count - 1)
            {
                behaviour.xpBar.OnUIUpdate(1f, unitxp, xpToNxtLvl[unitLvl]);
            }
            else
            {
                behaviour.xpBar.OnUIUpdate(((float)(unitxp - xpToNxtLvl[unitLvl]) / (xpToNxtLvl[unitLvl + 1] - xpToNxtLvl[unitLvl])), unitxp, xpToNxtLvl[unitLvl + 1]);

            }
        }

        void UpdateToxicityBar()
        {
            behaviour.toxicityBar.OnUIUpdate(((float)(maxToxicityResistance - toxicityResistance) / maxToxicityResistance), toxicityResistance, maxToxicityResistance);
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

        public void SetTransparentSelf(GameObject transparentSelf)
        {
            this.transparentSelf = transparentSelf;
        }

        public GameObject GetTransparentSelf()
        {
            return transparentSelf;
        }

        public void SetCurrentTile(TileBehaviour tile)
        {
            this.currentTile = tile;
        }

        public TileBehaviour GetCurrentTile()
        {
            return currentTile;
        }

        public void SwitchToTile(TileBehaviour newTile)
        {
            currentTile.SetOccupied(false);
            currentTile = newTile;
            currentTile.SetOccupied(true);
        }

        public bool Vaccinating()
        {
            if (maxToxicityResistance == toxicityResistance) return false;

            App.audioManager.Play("VaccineUsed");
            toxicityResistance += App.player.vaccineEffectivnes;
            toxicityResistance = Mathf.Clamp(toxicityResistance, 0,maxToxicityResistance);
            UpdateToxicityBar();
            App.player.useVaccine();

            return true;
        }
    }
}


