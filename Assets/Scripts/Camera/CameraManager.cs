using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    CustomCameraController cameraController;
    [HideInInspector]
    public Vector3 maxClampPos, minClampPos, maxClampZoom, minClampZoom;
    [HideInInspector]
    public float minOrtograficSize, maxOrtograficSize;

    private void Awake()
    {
        App.CameraManager = this;
        cameraController = GetComponent<CustomCameraController>();
    }

    public void SetNewWorlBoundrees(Vector3 maxClampPos, Vector3 minClampPos,
                             Vector3 maxClampZoom, Vector3 minClampZoom,
                             float minOrtograficSize, float maxOrtograficSize)
    {
        this.maxClampPos = maxClampPos;
        this.minClampPos = minClampPos;
        this.maxClampZoom = maxClampZoom;
        this.minClampZoom = minClampZoom;
        this.minOrtograficSize = minOrtograficSize;
        this.maxOrtograficSize = maxOrtograficSize;
    }

    public void EnableCamera()
    {
        cameraController.enabled = true;
    }

    public void DisableCamera()
    {
        cameraController.enabled = false;
    }


}
