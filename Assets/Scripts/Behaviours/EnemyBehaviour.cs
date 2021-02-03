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

    [SerializeField] int hp;
    [SerializeField] int attack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        
        model = new Enemy(hp,attack);
        agent.destination = this.model.GetTargetPosition();
    }

    private void Start()
    {
        
    }


    public void StartAttack()
    {
        agent.isStopped = true;
    }
}