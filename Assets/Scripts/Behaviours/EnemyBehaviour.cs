using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    private Enemy model;
    private Rigidbody rb;

    public void Initialize(Enemy model)
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        this.model = model;
        agent.destination = this.model.GetTargetPosition();
    }

    public void StartAttack()
    {
        agent.isStopped = true;
    }
}