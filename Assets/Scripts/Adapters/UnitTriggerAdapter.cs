﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitTriggerAdapter : MonoBehaviour
{
    UnitModel model;
    SphereCollider coll;

    

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();        
    }

    private void Start()
    {
        model = transform.parent.GetComponent<UnitBehaviour>().GetModel();
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