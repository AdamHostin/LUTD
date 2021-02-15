using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class BaseTriggerAdapter : MonoBehaviour
{
    Base model;

    private void Start()
    {
        model = transform.parent.GetComponent<BaseBehaviour>().GetModel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyBehaviour>().StartAttack();
        }
    }
}
