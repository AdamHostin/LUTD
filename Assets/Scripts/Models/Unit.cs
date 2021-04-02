using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Models
{
    public class Unit : IDamagable, IPlacebla
    {
        protected int hp;
        protected int maxHp;        
        protected int attack;
        protected float range;
        protected float scaler;

        protected int unitLvl = 0;
        protected int unitxp = 0;

        protected LayerMask layerMask;

        protected List<int> xpToNxtLvl;
        protected List<Enemy> enemiesInRange = new List<Enemy>();

        protected Vector3 gunPos;
        protected UnitBehaviour behaviour;
        //UnitTriggerAdapter adapter;

        //For relocating
        GameObject transparentSelf;
        TileBehaviour currentTile;

        public Enemy target;

        public class RangeChangeEvent : UnityEvent<float> { }
        public RangeChangeEvent rangeChangeEvent = new RangeChangeEvent();
        public class DamagableDeathEvent : UnityEvent<IDamagable> { }
        public DamagableDeathEvent onDamagableDeath = new DamagableDeathEvent();


        public UnitState state;

        public Unit(int hp, int attack, float range, Vector3 gunPos, float scaler, List<int> xpToNxtLvl, UnitBehaviour behaviour)
        {
            this.hp = maxHp = hp;
            this.attack = attack;
            this.range = range;
            this.xpToNxtLvl = xpToNxtLvl;
            this.gunPos = gunPos;
            this.scaler = scaler;
            this.behaviour = behaviour;
            ChangeState(UnitState.idle);

            behaviour.hpBar.OnUIUpdate(1f, maxHp, maxHp);            
            behaviour.xpBar?.OnUIUpdate(1f, 0, xpToNxtLvl[1]);

            layerMask = 0;
            layerMask = (1<<LayerMask.NameToLayer("Default") | 1<<LayerMask.NameToLayer("Enemy"));
        }

        public virtual void SetAdapter(UnitTriggerAdapter adapter)
        {
            rangeChangeEvent.AddListener(adapter.SetNewRange);
            rangeChangeEvent.Invoke(range);
            OnUnitPlace();
        }

        public virtual void AddEnemy(Enemy enemy)
        {
            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
                enemy.enemyDeathEvent.AddListener(SubstractEnemy);
                behaviour.StartShooting();
            }            
        }

        public virtual void SubstractEnemy(Enemy enemy)
        {
            enemiesInRange.Remove(enemy);
            enemy.enemyDeathEvent.RemoveListener(SubstractEnemy);
            if (enemiesInRange.Count == 0) ChangeState(UnitState.idle);
        }

        public void ChangeState(UnitState newState)
        {
            if (state == UnitState.dying) return;

            switch (newState)
            {
                case UnitState.idle:
                    break;
                case UnitState.shooting:
                    break;
                case UnitState.dying:
                    onDamagableDeath.Invoke(this);
                    behaviour.Die();
                    break;
                default:
                    break;
            }

            state = newState;
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
        public virtual bool Shoot()
        {
            target = GetTarget();
            if (target == null || target.behaviour==null) return false;

            Addxp(target.GetDamage(attack));

            return true;
        }

        public Vector3 VectorToLookAt()
        {
            Vector3 res = target.GetPosition();
            res.y = behaviour.transform.position.y;
            return res;
        }

        protected void Addxp(int xp)
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

        protected virtual void LevelUp()
        {
            //Improve hp
            int newMaxHp = (int) Mathf.Ceil((scaler * maxHp));
            if (newMaxHp > maxHp) hp += (newMaxHp - maxHp);
            if (hp > newMaxHp) hp = newMaxHp;
            maxHp = newMaxHp;
            UpdateHpBar(); 

            //Improve range
            range *= scaler;
            rangeChangeEvent.Invoke(range);
            CheckEnemiesOnPlace();

            unitLvl++;
            Debug.Log("Current lvl " + unitLvl);
        }

        public virtual bool GetDamage(int damage, int infection = 0)
        {
            hp -= damage;

            if (hp <= 0)
            {
                if (behaviour != null)
                {
                    ChangeState(UnitState.dying);
                }
                return false;
            }
            else
            {
                UpdateHpBar();
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
                behaviour.xpBar?.OnUIUpdate(1f, unitxp, xpToNxtLvl[unitLvl]);
            }
            else
            {
                behaviour.xpBar?.OnUIUpdate(((float)(unitxp - xpToNxtLvl[unitLvl]) / (xpToNxtLvl[unitLvl + 1] - xpToNxtLvl[unitLvl])), unitxp, xpToNxtLvl[unitLvl + 1]);

            }
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

        public bool Healing()
        {
            if (hp == maxHp) return false;

            App.audioManager.Play("MedkitUsed");
            hp += App.player.medkitEffectivness;
            Debug.Log("Medkit used");
            hp = Mathf.Clamp(hp, 0, maxHp);
            UpdateHpBar();
            App.player.UseMedkit();

            return true;
        }
    }
}


