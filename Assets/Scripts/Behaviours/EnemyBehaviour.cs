using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//TODO: change enemies so they can attack units and other destructible objects
//TODO: add enemy types soo they can destruct some objects(windovs) in the map
//TODO: enemies should be able to die
public class EnemyBehaviour : MonoBehaviour
{

    private NavMeshAgent agent;
    private Enemy model;
    private Rigidbody rb;

    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] float attackFrequency;
    [SerializeField] float attackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();        
        model = new Enemy(hp,attack);
        agent.destination = this.model.GetTargetPosition();
    }


    public void StartAttack()
    {
        StartCoroutine(AproachingTarget());
    }

    IEnumerator AproachingTarget()
    {
        while (Vector3.Distance(model.GetTargetPosition(), transform.position) > attackRange) yield return null;
        model.state = EnemyState.attacking;
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        while (model.state == EnemyState.attacking)
        {
            yield return new WaitForSeconds(attackFrequency);
            model.AttackBase();
        }
        agent.isStopped = false;
    }
}