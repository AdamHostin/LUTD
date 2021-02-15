using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    private UnitModel model;
    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] float range;

    [SerializeField] List<int> xpToNxtLvl;


    float timeBetweenHits = 0.5f;

    //TODO: add unit to activeUnitListInLvlManager?
    private void Awake()
    {
        model = new UnitModel(hp, attack, range, transform.position ,xpToNxtLvl, this);
    }

    public UnitModel GetModel()
    {
        return model;
    }

    public void StartShooting()
    {
        if (model.state == UnitState.shooting) return;
        StartCoroutine(Shoottng());
    }

    //This will change once we'll have some animations 
    IEnumerator Shoottng()
    {
        model.state = UnitState.shooting;
        while (model.state == UnitState.shooting)
        {
            model.shoot();
            yield return new WaitForSeconds(timeBetweenHits);
        }
    }

}
