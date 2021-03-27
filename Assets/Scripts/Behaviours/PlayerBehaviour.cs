using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    bool canRotate = false;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    public void Update()
    {
        if (canRotate && Input.GetMouseButtonDown(1))
        {
            App.player.Rotate();
        }
    }

    public void SetCanRotate(bool value)
    {
        canRotate = value;
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (App.player.behaviour == null)
            App.player.behaviour = this;
    }
}