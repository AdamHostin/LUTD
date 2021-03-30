using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Models;

public class UnitTriggerAdapter : MonoBehaviour
{
    Unit model;
    SphereCollider coll;

    

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();        
    }

    private void Start()
    {
        model = transform.parent.GetComponents<UnitBehaviour>()[0].GetModel();
        Debug.Log(model);
        model.SetAdapter(this);
    }

    public void SetNewRange(float range)
    {
        coll.radius = range;
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
}
