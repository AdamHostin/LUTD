using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    GameObject unitPrefab;

    public GameObject GetUnitPrefab()
    {
        return unitPrefab;
    }
}