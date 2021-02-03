using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class BaseBehaviour : MonoBehaviour
{
    Base model;
    public int hp;

    private void Awake()
    {
        model = new Base(hp,transform.position);
        App.levelManager.SetPlayerBase(model);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Enemy")
        {
            other.gameObject.GetComponent<EnemyBehaviour>().StartAttack();
        }
    }
}
