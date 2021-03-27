using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.EventSystems;

public class UnitBehaviour : MonoBehaviour, IDamagableBehaviour
{
    private Unit model;
    [SerializeField] int hp;
    [SerializeField] int toxicityResistance;
    [SerializeField] int attack;
    [SerializeField] float range;
    [Tooltip("Value by which is maxhp, maxToxicityResistancce and range multiplyed on level up")]
    [Range(1f,10f)] [SerializeField] float scaler = 1.5f;
    [SerializeField] float timeBetweenHits = 0.5f;
    [SerializeField] GameObject zombiePrefab;

    [SerializeField] List<int> xpToNxtLvl;

    public BarController hpBar;
    public BarController toxicityBar;
    public BarController xpBar;

    [SerializeField]
    private bool isRelocatable;

    [SerializeField]
    string deathSound;


    //TODO: add unit to activeUnitList in LevelManager?
    private void Awake()
    {
        model = new Unit(hp, toxicityResistance, attack, range, transform.position, scaler ,xpToNxtLvl, this);
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
        gameObject.tag = "Untagged";
        App.audioManager.Play(deathSound);
        Destroy(gameObject);
    }

    public void GetInfected()
    {
        App.audioManager.Play("UnitInfected");
        gameObject.tag = "Untagged";
        App.levelManager.AddEnemies();
        GameObject zombie = Instantiate(zombiePrefab,transform.position, Quaternion.identity);
        zombie.transform.parent = App.levelManager.transform;
        Debug.Log("I am a zobie now");
        Destroy(gameObject);
    }

    private void OnMouseDown()
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
        App.audioManager.Play("UnitRemoved");
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