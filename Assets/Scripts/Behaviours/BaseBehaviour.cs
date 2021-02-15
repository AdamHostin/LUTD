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
        model = new Base(hp, transform.position, this);
        App.levelManager.SetPlayerBase(model);
    }

    public void DestroyBase()
    {
        //TODO: play SFX
        Destroy(gameObject);
    }

    public Base GetModel()
    {
        return model;
    }
        
}
