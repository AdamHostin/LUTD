using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class UnitBehaviour : MonoBehaviour, IDamagableBehaviour
{
    private Unit model;
    [SerializeField] int hp;
    [SerializeField] int toxicityResistance;
    [SerializeField] int attack;
    [SerializeField] float range;
    [SerializeField] float timeBetweenHits = 0.5f;

    [SerializeField] List<int> xpToNxtLvl;

    public BarController hpBar;
    public BarController toxicityBar;
    public BarController xpBar;

    [SerializeField]
    private bool isRelocatable;



    //TODO: add unit to activeUnitList in LevelManager?
    private void Awake()
    {
        model = new Unit(hp, toxicityResistance, attack, range, transform.position ,xpToNxtLvl, this);
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
        if (model.state == UnitState.shooting) return;
        StartCoroutine(Shooting());
    }

    //This will change once we'll have some animations 
    IEnumerator Shooting()
    {
        model.state = UnitState.shooting;
        while (model.state == UnitState.shooting)
        {
            model.Shoot();
            yield return new WaitForSeconds(timeBetweenHits);
        }
    }

    public void Die()
    {
        //TODO: play SFX
        Destroy(gameObject);
    }

    public void GetInfected()
    {
        //TODO: turn unit into a zombie
        App.levelManager.AddEnemies();
        Debug.Log("I am a zobie now");
    }

    private void OnMouseDown()
    {
        if (isRelocatable && App.levelManager.CompareLevelState(LevelState.betweenWave))
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
    }

    public void SelectUnit()
    {
        App.player.SetUnitToRelocate(this.gameObject, model.GetTransparentSelf());
        //TODO: add unit highlight
    }

    public void DeselectUnit(bool changeState)
    {
        App.player.DeleteTransparentUnit(changeState);
        //TODO: add dehighlight
    }

    public void Relocate(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        DeselectUnit(true);
    }
}