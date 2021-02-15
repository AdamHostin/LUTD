using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIgnoreLayers : MonoBehaviour
{
    public string[] layers;
    void Awake()
    {
        Camera.main.eventMask = LayerMask.GetMask(layers);
    }
}