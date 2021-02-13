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
        gameObject.GetComponent<SphereCollider>().radius = range;
        model = new UnitModel(hp, attack, transform.position ,xpToNxtLvl, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
           
            model.AddEnemy(other.gameObject.GetComponent<EnemyBehaviour>().GetModel());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            model.SubstractEnemy(other.gameObject.GetComponent<EnemyBehaviour>().GetModel());
        }
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
            Debug.Log("shooting");
            yield return new WaitForSeconds(timeBetweenHits);
        }
    }
}
