using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIgnoreLayers : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.eventMask = LayerMask.GetMask("Default", "UI", "CInteractable");
    }
}