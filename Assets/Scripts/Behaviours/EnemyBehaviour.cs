using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Models;


public class EnemyBehaviour : MonoBehaviour
{

    public NavMeshAgent agent;
    private Enemy model;
    private Rigidbody rb;

    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] int toxicity;
    [SerializeField] int xp;
    [SerializeField] int minCoins;
    [SerializeField] int maxCoins;
    [SerializeField] float attackFrequency;
    [SerializeField] float attackRange;
    [SerializeField] float awarenessRange;
    [SerializeField] string[] attackableTags;

    public Animator animator;
   
    [Header("programing stuf")]
    public BarController hpBar;
    [Tooltip("Wimbly wombly timey wimey")]
    public GameObject target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();        
        model = new Enemy(hp, attack, attackRange, awarenessRange, toxicity , xp, Random.Range(minCoins,maxCoins) ,attackableTags, this);
        agent.updateUpAxis = true;
        agent.updateRotation = true;
    }

    private void Update()
    {
        if (!agent.isStopped && agent.path.corners.Length>1)
        {
            FaceTarget();
        }
        
    }
    //https://thehangarter.wordpress.com/2017/05/25/rotating-the-character-with-navmeshagent-navigation/
    private void FaceTarget()
    {
        var targetPosition = agent.path.corners[1];
        var targetPoint = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        var direction = (targetPoint - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.velocity.magnitude);

    }

    public void StartAttack()
    {
        StartCoroutine(AproachingTarget());
    }

    IEnumerator AproachingTarget()
    {
        while (model.IsBlocked())
        {
            yield return new WaitForSeconds(0.25f);
            if (target == null) yield break;
        }
        model.ChangeState(EnemyState.attacking);
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        while (model.state == EnemyState.attacking)
        {
            yield return new WaitForSeconds(attackFrequency);
            if (model.state == EnemyState.attacking) model.Attack();
        }
    }

    public void StartDying()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        App.audioManager.Play("EnemyDeath");
        //TODO: do some magic
        
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);
        model.behaviour = null;
        Destroy(gameObject);
    }

    public Enemy GetModel()
    {
        return model;
    }
}