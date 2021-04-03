using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

namespace Models
{
    public class Enemy
    {
        private int attack;
        private int toxicity;
        private int maxHp;
        private int hp;
        private int xp;
        private int coins;
        private float attackRange;
        private float awarenessRange;
        private IDamagable target = null;

        private string[] attackables;

        private List<IDamagable> damagablesInAwarenessRange = new List<IDamagable>();

        public EnemyState state;

        public EnemyBehaviour behaviour;

        public class RangeChangeEvent : UnityEvent<float> { }
        public RangeChangeEvent awarenessRangeChangeEvent = new RangeChangeEvent();
        public class EnemyDeathEvent : UnityEvent<Enemy> { }
        public EnemyDeathEvent enemyDeathEvent = new EnemyDeathEvent();
        public RangeChangeEvent attackRangeChangeEvent = new RangeChangeEvent();

        public Enemy(int hp, int attack, float attackRange, float awarenessRange, int toxicity, int xp, int coins, string[] attackables ,EnemyBehaviour behaviour)
        {
            this.hp = this.maxHp = hp;
            this.attack = attack;
            this.toxicity = toxicity;
            this.xp = xp;
            this.coins = coins;
            this.attackRange = attackRange;
            this.awarenessRange = awarenessRange;
            this.behaviour = behaviour;
            this.attackables = attackables;

            behaviour.hpBar.OnUIUpdate(1f, hp, maxHp);

            damagablesInAwarenessRange.Add(App.levelManager.GetPlayerBase());
            ChangeState(EnemyState.moving);
            App.levelManager.damagablePlacedEvent.AddListener(ResponseOnDamagablePlaced);
            CheckDamagablesOnSpawn();
        }

        private void CheckDamagablesOnSpawn()
        {
            Collider[] colls = Physics.OverlapSphere(GetPosition(), awarenessRange);
            foreach (var coll in colls)
            {
                foreach (var attackableTag in attackables)
                {
                    if (coll.transform.tag == attackableTag)
                    {
                        AddDamagable(coll.GetComponent<IDamagableBehaviour>().GetDamagableModel());
                    }
                }
            }
        }

        public void ResponseOnDamagablePlaced(IDamagable damagable)
        {
            if (Vector3.Distance(damagable.GetPosition(),GetPosition()) < awarenessRange)
            {
                AddDamagable(damagable);
            }
        }

        public void SetAttackRangeAdapter(EnemyRangeTriggerAdapter adapter)
        {
            attackRangeChangeEvent.AddListener(adapter.SetNewRange);
            adapter.SetAttackables(attackables);
            attackRangeChangeEvent.Invoke(attackRange);
        }

        public void SetAwarenessRangeAdapter(EnemyAwarenessTriggerAdapter adapter)
        {
            awarenessRangeChangeEvent.AddListener(adapter.SetNewRange);
            adapter.SetAttackables(attackables);
            awarenessRangeChangeEvent.Invoke(awarenessRange);
        }

        private void SetNewTarget(IDamagable damagable)
        {
            target = damagable;
            behaviour.agent.SetDestination(GetTargetPosition());
        }

        public void AddDamagable(IDamagable damagable)
        {
            
            if (!damagablesInAwarenessRange.Contains(damagable))
            {
                damagablesInAwarenessRange.Add(damagable);
                damagable.SubscribeToDeathEvent(this);
            }

            if (state == EnemyState.moving) SetNewTarget(GetNxtTarget());
        }

        public void SubstractDamagable(IDamagable damagable)
        {
            damagablesInAwarenessRange.Remove(damagable);
            damagable.UnsubscribeToDeathEvent(this);
        }

        public void OnDamagableAttackRangeEnter(IDamagable damagable)
        {
            if (state == EnemyState.dying) return;
            
            if (damagable == target)
            {
                behaviour.StartAttack();
            }
        }

        public bool IsBlocked()
        {
            RaycastHit hit;
            Vector3 targetPos = target.GetPosition();
            targetPos.y = GetPosition().y;
            //ignore layer 5 (UI), 8 (Enemy), 10 (CUinteractable)
            int mask = ~((1<<5) | (1<<8) | (1<<10));
            //int mask = (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("CInteractable"));
            Ray sight = new Ray(GetPosition(), (targetPos - GetPosition()));
            if (Physics.Raycast(sight, out hit, Mathf.Infinity, mask ))
            {
                foreach (var item in attackables) if (item == hit.transform.tag) return false;
            }
            return true;
        }

        public void ChangeState(EnemyState newState)
        {
            state = newState;
            switch (newState)
            {
                case EnemyState.moving:
                    SetNewTarget(GetNxtTarget());
                    behaviour.agent.isStopped = false;
                    behaviour?.animator.SetBool("Attack",false);
                    break;
                case EnemyState.attacking:
                    behaviour.agent.isStopped = true;
                    behaviour?.animator.SetBool("Attack", true);
                    break;
                case EnemyState.dying:
                    behaviour.agent.isStopped = true;
                    behaviour?.animator.SetTrigger("Die");
                    damagablesInAwarenessRange.Clear();
                    enemyDeathEvent.Invoke(this);
                    target = null;
                    App.levelManager.damagablePlacedEvent.RemoveListener(ResponseOnDamagablePlaced);
                    App.levelManager.EnemyDied();
                    App.player.EarnCoins(coins);
                    behaviour.StartDying();
                    break;
                default:
                    break;
            }

        }

        public void Attack()
        {
            if ((target==null) || (target?.GetDamage(attack, toxicity) == false))
            {
                ChangeState(EnemyState.moving);                
            }
        }

        public Vector3 GetTargetPosition()
        {
            Vector3 targetPos = target.GetPosition();
            targetPos.y = 0;
            return targetPos;
        }

        public Vector3 GetPosition()
        {
            return behaviour.transform.position;
        }

        public int GetDamage(int damage)
        {
            if (state==EnemyState.dying) return 0;
            hp -= damage;
            Mathf.Clamp(hp, 0, maxHp);
            behaviour.hpBar.OnUIUpdate((float)hp / maxHp, hp, maxHp);
            if (hp > 0) return 0;            
            ChangeState(EnemyState.dying);            
            return xp;
        }

        public void OnDamagableInAwarenessRangeDeath(IDamagable damagable)
        {
            if (state == EnemyState.dying) return;
            damagablesInAwarenessRange.Remove(damagable);
            if (damagable == target)
            {
                ChangeState(EnemyState.moving);
            }
                      
        }

        public void OnDamagableInattackRange(IDamagable damagable)
        {
            if (damagable==target)
            {
                behaviour.StartAttack();
            }            
        }

        //https://forum.unity.com/threads/unity-navmesh-get-remaining-distance-infinity.415814/
        public float RemainingDistance(Vector3[] points)
        {
            if (points.Length < 2) return 0;
            float distance = 0;
            for (int i = 0; i < points.Length - 1; i++)
                distance += Vector3.Distance(points[i], points[i + 1]);
            return distance;
        }


        private IDamagable GetNxtTarget()
        {            
            if (damagablesInAwarenessRange.Count > 0)
            {
                float dist = float.MaxValue;
                int bestIndex = 0;
                NavMeshPath path = new NavMeshPath();
                NavMeshHit hit;
                for (int i = 0; i < damagablesInAwarenessRange.Count; i++)
                {                    
                    if (!NavMesh.SamplePosition(damagablesInAwarenessRange[i].GetPosition(), out hit, 1.0f, NavMesh.AllAreas)) continue;
                    
                    NavMesh.CalculatePath(GetPosition(), hit.position, NavMesh.AllAreas, path);
                    if (path.status!=NavMeshPathStatus.PathComplete)
                    {
                        continue;
                    }
                    float newDist = RemainingDistance(path.corners);

                    if (newDist < dist)
                    {
                        dist = newDist;
                        bestIndex = i;
                    }
                }
                return damagablesInAwarenessRange[bestIndex];
            }
            return App.levelManager.GetPlayerBase();
        }
    }
}

