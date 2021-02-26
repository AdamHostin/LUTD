using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class BaseBehaviour : MonoBehaviour, IDamagableBehaviour
{
    Base model;
    [SerializeField] private int hp;

    public BarController hpBar;

    private void Awake()
    {
        model = new Base(hp, transform.position, this);
    }

    public void DestroyBase()
    {
        //TODO: play SFX
        Destroy(gameObject);
    }

    public IDamagable GetDamagableModel()
    {
        return model;
    }

    public Base GetModel()
    {
        return model;
    }

}
