using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class EnemyAwarenessTriggerAdapter : MonoBehaviour
{
    Enemy model;
    SphereCollider coll;
    private string[] attackables;



    private void Awake() => coll = GetComponent<SphereCollider>();

    private void Start()
    {
        model = transform.parent.GetComponent<EnemyBehaviour>().GetModel();
        model.SetAwarenessRangeAdapter(this);
    }

    public void SetNewRange(float range) => coll.radius = range;
    public void SetAttackables(string[] attackables) => this.attackables = attackables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Base") return;

        foreach (var item in attackables)
        {
            if (other.tag == item)
            {
                
                IDamagableBehaviour damagable = other.gameObject.GetComponent<IDamagableBehaviour>();
                if (damagable == null) continue;
                model.AddDamagable(damagable.GetDamagableModel());      
            }
        }

    }
    /*
    private void OnTriggerExit(Collider other)
    {
        foreach (var item in attackables)
        {
            if (other.tag == item)
            {
                model.SubstractDamagable(other.gameObject.GetComponent<IDamagableBehaviour>().GetDamagableModel());
                break;
            }
        }
    }*/
}
