using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class EnemyRangeTriggerAdapter : MonoBehaviour
{
    Enemy model;
    SphereCollider coll;
    private string[] attackables;



    private void Awake() => coll = GetComponent<SphereCollider>();

    private void Start()
    {
        model = transform.parent.GetComponent<EnemyBehaviour>().GetModel();
        model.SetAttackRangeAdapter(this);
    }

    public void SetNewRange(float range) => coll.radius = range;
    public void SetAttackables(string[] attackables) => this.attackables = attackables;

    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in attackables)
        {
            if (other.tag == item)
            {
                model.OnDamagableInattackRange(other.gameObject.GetComponent<IDamagableBehaviour>().GetDamagableModel());
                break;
            }
        }
        
    }
}
