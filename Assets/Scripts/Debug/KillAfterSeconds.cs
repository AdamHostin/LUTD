using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterSeconds : MonoBehaviour
{
    [SerializeField] private float seconds = 0.5f;

    void Start()
    {
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(seconds);
        App.levelManager.EnemyDied();
        Destroy(gameObject);
    }
}
