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

}