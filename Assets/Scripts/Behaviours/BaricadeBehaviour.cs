using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class BaricadeBehaviour : MonoBehaviour, IDamagableBehaviour, IPlacebleBehaviour
{
    Baricade model;
    [SerializeField] private int hp;

    public BarController hpBar;

    private void Awake()
    {
        model = new Baricade(hp, transform.position, this);
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

    public Baricade GetModel()
    {
        return model;
    }

    public IPlacebla GetPlaceblaModel()
    {
        return (IPlacebla)model;
    }

}