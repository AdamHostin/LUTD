using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class HumanoidBehaviour : UnitBehaviour
{
    [SerializeField] protected GameObject zombiePrefab;
    [SerializeField] protected int toxicityResistance;
    public Animator animator;

    public BarController toxicityBar;

    protected override void Awake()
    {
        model = new HumanoidUnit(hp, toxicityResistance, attack, range, gunPos.position, scaler, xpToNxtLvl, this);
        //base.model = model;
    }

    protected override IEnumerator Shooting()
    { 
        model.ChangeState(UnitState.shooting);
        while (model.state == UnitState.shooting)
        {

            if (model.Shoot())
            {
                Debug.Log(model.target);
                if (model.target != null)
                {
                    transform.LookAt(model.VectorToLookAt(), gunPos.up);
                    animator.SetBool("Shoot", true);
                }

            }
            else
            {
                animator.SetBool("Shoot", false);
            }
            yield return new WaitForSeconds(timeBetweenHits);
        }

    }
    public override void Die()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) return;

        gameObject.tag = "Untagged";
        App.audioManager.Play("UnitDeath");
        
        animator.SetBool("Die",true);
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        StartCoroutine(HandleDeath());
}

    public void GetInfected()
    {
        //TODO: particles

        App.audioManager.Play("UnitInfected");
        gameObject.tag = "Untagged";
        App.levelManager.AddEnemies();
        GameObject zombie = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        zombie.transform.parent = App.levelManager.transform;
        Debug.Log("I am a zobie now");
        Destroy(gameObject);
    }

    protected IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(/*animator.GetCurrentAnimatorClipInfo(0).Length*/ 3f);
        Destroy(gameObject);
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if ((App.player.ComparePlayerState(PlayerState.vaccinating) && !EventSystem.current.IsPointerOverGameObject()))
        {
            ((HumanoidUnit) model).Vaccinating();
        }
    }
}
