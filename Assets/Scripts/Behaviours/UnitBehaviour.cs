using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public abstract class UnitBehaviour : MonoBehaviour, IDamagableBehaviour 
{
    protected Unit model;
    [SerializeField] protected int hp;

    [SerializeField] protected int attack;
    [SerializeField] protected float range;
    [Tooltip("Value by which is maxhp, maxToxicityResistancce and range multiplyed on level up")]
    [Range(1f, 10f)] [SerializeField] protected float scaler = 1.5f;
    [SerializeField] protected float timeBetweenHits = 0.5f;
    [SerializeField] protected Transform gunPos;
    


    [SerializeField] protected List<int> xpToNxtLvl;

    public BarController hpBar;    
    public BarController xpBar;


    [SerializeField]
    protected bool isRelocatable;



    //TODO: add unit to activeUnitList in LevelManager?
    protected virtual void Awake()
    {
        model = new Unit(hp , attack, range, gunPos.position, scaler, xpToNxtLvl, this);
    }

    public IDamagable GetDamagableModel()
    {
        return model;
    }

    public Unit GetModel()
    {
        Debug.Log(model);
        return model;
    }

    public void StartShooting()
    {
        Debug.Log("Start Shooting");
        if (model.state == UnitState.shooting) return;
        StartCoroutine(Shooting());
    }

    protected abstract IEnumerator Shooting();
    


    public abstract void Die();
    
    protected virtual void OnMouseDown()
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

}