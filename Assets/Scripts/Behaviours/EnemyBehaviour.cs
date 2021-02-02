using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    private Enemy model;

    public void Initialize(Enemy model)
    {
        agent = GetComponent<NavMeshAgent>();
        this.model = model;
        agent.destination = this.model.GetTargetPosition();
    }
}