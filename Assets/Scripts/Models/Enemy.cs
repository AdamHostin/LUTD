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
        private float attackRange;
        private float awarenessRange;
        private IDamagable target = null;

        private string[] attackables;

        private List<IDamagable> damagablesInAwarenessRange = new List<IDamagable>();

        public EnemyState state;

        public EnemyBehaviour behaviour;

        public class RangeChangeEvent : UnityEvent<float> { }
        public RangeChangeEvent awarenessRangeChangeEvent = new RangeChangeEvent();
        public RangeChangeEvent attackRangeChangeEvent = new RangeChangeEvent();

        public Enemy(int hp, int attack, float attackRange, float awarenessRange, int toxicity, int xp , string[] attackables ,EnemyBehaviour behaviour)
        {
            this.hp = hp;
            this.attack = attack;
            this.toxicity = toxicity;
            this.xp = xp;
            this.attackRange = attackRange;
            this.awarenessRange = awarenessRange;
            this.behaviour = behaviour;
            this.attackables = attackables;

            //TODO: cast sphere
            //TODO: change Target searching
            damagablesInAwarenessRange.Add(App.levelManager.GetPlayerBase());
            SetNewTarget(GetNxtTarget());
            state = EnemyState.moving;
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
            attackRangeChangeEvent.Invoke(awarenessRange);
        }

        private void SetNewTarget(IDamagable damagable)
        {
            target = damagable;
            Debug.Log(target.GetType());
            behaviour.agent.SetDestination(GetTargetPosition());
        }

        public void AddDamagable(IDamagable damagable)
        {
            
            if (!damagablesInAwarenessRange.Contains(damagable))
            {
                damagablesInAwarenessRange.Add(damagable);
                damagable.SubscribeToDeathEvent(this);
            }
            
            SetNewTarget(GetNxtTarget());
        }

        public void SubstractDamagable(IDamagable damagable)
        {
            damagablesInAwarenessRange.Remove(damagable);
            damagable.UnsubscribeToDeathEvent(this);
        }

        public void OnDamagableAttackRangeEnter(IDamagable damagable)
        {
            if (damagable == target)
            {
                behaviour.StartAttack();
            }
        }

        public bool IsBlocked()
        {
           /*
            NavMeshHit hit;
            Vector3 targetPos = target.GetPosition();
            targetPos.y = GetPosition().y;

            return NavMesh.Raycast(GetPosition(), targetPos, out hit, NavMesh.AllAreas);
            */
            RaycastHit hit;
            Vector3 targetPos = target.GetPosition();
            targetPos.y = GetPosition().y;
            Ray sight = new Ray(GetPosition(), (targetPos - GetPosition()));
            if (Physics.Raycast(sight, out hit))
            {
                Debug.Log("i hit " + hit.transform.tag);
                foreach (var item in attackables) if (item == hit.transform.tag) return false;
            }
            return true;
            //*/
        }

        //TODO: using switch on state change?
        public void ChangeState(EnemyState newState)
        {
            state = newState;
            switch (newState)
            {
                case EnemyState.moving:
                    SetNewTarget(GetNxtTarget());
                    behaviour.agent.isStopped = false;
                    break;
                case EnemyState.attacking:
                    behaviour.agent.isStopped = true;
                    break;
                case EnemyState.dying:
                    behaviour.agent.isStopped = true;
                    break;
                default:
                    break;
            }

        }

        public void Attack()
        {
            if (target?.GetDamage(attack, toxicity) == false)
            {
                target = null;
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
            if (hp > 0) return 0;
            ChangeState(EnemyState.dying);
            App.levelManager.EnemyDied();
            behaviour.StartDying();
            return xp;
        }

        public void OnDamagableInAwarenessRangeDeath(IDamagable damagable)
        {
            damagablesInAwarenessRange.Remove(damagable);
            if (damagable == target)
            {
                SetNewTarget(GetNxtTarget());
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

                for (int i = 0; i < damagablesInAwarenessRange.Count; i++)
                {
                    NavMesh.CalculatePath(GetPosition(), damagablesInAwarenessRange[i].GetPosition(), NavMesh.AllAreas, path);

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

