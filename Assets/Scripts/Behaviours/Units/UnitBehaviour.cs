using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class UnitBehaviour : MonoBehaviour, IDamagableBehaviour 
{
    protected Unit model;
    [SerializeField] protected int hp;
    [SerializeField] protected int toxicityResistance;
    [SerializeField] protected int attack;
    [SerializeField] protected float range;
    [Tooltip("Value by which is maxhp, maxToxicityResistancce and range multiplyed on level up")]
    [Range(1f, 10f)] [SerializeField] protected float scaler = 1.5f;
    [SerializeField] protected float timeBetweenHits = 0.5f;
    [SerializeField] protected Transform gunPos;
    [SerializeField] protected GameObject zombiePrefab;


    [SerializeField] protected List<int> xpToNxtLvl;

    public BarController hpBar;
    public BarController toxicityBar;
    public BarController xpBar;
    public Animator animator;

    [SerializeField]
    protected bool isRelocatable;



    //TODO: add unit to activeUnitList in LevelManager?
    protected void Awake()
    {
        Debug.Log(gunPos.position);
        model = new Unit(hp, toxicityResistance, attack, range, gunPos.position, scaler, xpToNxtLvl, this);
    }

    public IDamagable GetDamagableModel()
    {
        return model;
    }

    public Unit GetModel()
    {
        return model;
    }

    public void StartShooting()
    {
        Debug.Log("Start Shooting");
        if (model.state == UnitState.shooting) return;
        StartCoroutine(Shooting());
    }

    protected virtual IEnumerator Shooting()
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
                }
            }
            yield return new WaitForSeconds(timeBetweenHits);
        }
    }
   

    public void Die()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) return;
        Debug.Log("Dead");
        Debug.Log(model.state);
        gameObject.tag = "Untagged";
        App.audioManager.Play("UnitDeath");
        
        animator.SetBool("Die",true);
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(/*animator.GetCurrentAnimatorClipInfo(0).Length*/ 3f);
        Destroy(gameObject);
    }

    public void GetInfected()
    {
        App.audioManager.Play("UnitInfected");
        gameObject.tag = "Untagged";
        App.levelManager.AddEnemies();
        GameObject zombie = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        zombie.transform.parent = App.levelManager.transform;
        Debug.Log("I am a zobie now");
        Destroy(gameObject);
    }

    protected void OnMouseDown()
    {
        if (App.levelManager.CompareLevelState(LevelState.betweenWave) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (isRelocatable)
            {
                if (App.player.ComparePlayerState(PlayerState.idle))
                {
                    SelectUnit();
                }
                else if (App.player.ComparePlayerState(PlayerState.relocating))
                {

                    if (App.player.GetPickedUnit() != this.gameObject)
                    {
                        App.player.GetPickedUnit().GetComponent<UnitBehaviour>().DeselectUnit(false);
                        SelectUnit();
                    }
                    else
                        DeselectUnit(true);
                }
                else
                {
                    App.unitCardManager.SwitchToCard(null);
                    SelectUnit();
                }
            }
            else
            {
                if (App.player.ComparePlayerState(PlayerState.relocating))
                {
                    App.player.GetPickedUnit().GetComponent<UnitBehaviour>().DeselectUnit(true);
                }
                else if (App.player.ComparePlayerState(PlayerState.placing))
                {
                    App.unitCardManager.SwitchToCard(null);
                    App.player.DeleteTransparentUnit();
                    App.player.ChangeState(PlayerState.idle);
                }
            }
        }

        if ((App.player.ComparePlayerState(PlayerState.vaccinating) && !EventSystem.current.IsPointerOverGameObject()))
        {
            model.Vaccinating();
        }
    }

    public void SelectUnit()
    {
        App.player.SetUnitToRelocate(this.gameObject, model.GetTransparentSelf());
        //TODO: add unit highlight
    }

    public void DeselectUnit(bool changeState)
    {
        App.player.DeleteTransparentUnit();
        App.player.ChangeState(PlayerState.idle);
        //TODO: add dehighlight
    }

    public void Relocate(Vector3 targetPosition, TileBehaviour tile)
    {
        model.OnUnitPick();
        transform.position = targetPosition;
        model.OnUnitPlace();
        model.SwitchToTile(tile);
        DeselectUnit(true);
    }

    public void ShootingAnim(bool isShooting)
    {
        animator.SetBool("Shoot", isShooting);
    }


}