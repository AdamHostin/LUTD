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
        model = new Base(hp,transform.position,this);
        App.levelManager.SetPlayerBase(model);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Enemy")
        {
            other.gameObject.GetComponent<EnemyBehaviour>().StartAttack();
        }
    }

    public void DestroyBase()
    {
        //TODO: play SFX
        Destroy(gameObject);
    }
}
