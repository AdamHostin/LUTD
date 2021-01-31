using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=rnqF6S7PfFA


public class CustomCameraController : MonoBehaviour
{

    // nie som si uplne isty ci to chcem robit takto
    // mozno by bolo vhodne aby sa tieto 4 atributy ziskavali z lvl managera
    // pripadne ak by velkost levelov bola konstantna moze to zostat aj tu
    // clamp rig position
    public Vector3 maxClampPos;
    public Vector3 minClampPos;
    // clamp camera local position
    public Vector3 minClampZoom;
    public Vector3 maxClampZoom;



    [SerializeField] Transform cameraTransform;

    [SerializeField] float movementTime;
    [SerializeField] float normalSpeed;
    [SerializeField] float fastSpeed;
    [SerializeField] float rotationAmount;
    [SerializeField] Vector3 zoomAmount;
    float movementSpeed;

    Vector3 newPos;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPos;
    Vector3 dragCurrentPos;
    Vector3 rotateStartPos;
    Vector3 rotateCurrentPos;


    void Start()
    {
        newPos = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleKeyMovement();
        HandleMouseMovement();
    }

    private void HandleMouseMovement()
    {
        return;
    }

    private void HandleKeyMovement()
    {

        if (Input.GetButton("Camera Speed")) movementSpeed = fastSpeed;
        else movementSpeed = normalSpeed;

        // Aby system reagoval na zmenu polohy abs. hodnota musí byt vacsia ako 0 (epsilon je 0 pri porovnávaní floating point cisel)
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            //(Verical position change * speed) + (horizontal position change * speed)
            newPos += (transform.forward * Input.GetAxisRaw("Vertical") * movementSpeed) + (transform.right * Input.GetAxisRaw("Horizontal") * movementSpeed);
            newPos = ClampVector3(newPos, minClampPos, maxClampPos);
            
        }

        if (Input.GetAxisRaw("Camera Rotation") != 0 )
        {
            newRotation *= Quaternion.Euler(Vector3.up * Input.GetAxisRaw("Camera Rotation") * rotationAmount);            
        }

        if (Input.GetAxisRaw("Camera Zoom") != 0)
        {
            if (Input.GetAxisRaw("Camera Zoom") > 0) newZoom += zoomAmount;
            else newZoom -= zoomAmount;
            ClampVector3(newZoom, minClampZoom, maxClampZoom);
        }

        
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);        
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

    }

    Vector3 ClampVector3(Vector3 actualVector, Vector3 min, Vector3 max)
    {
        actualVector.x = Mathf.Clamp(actualVector.x, min.x, max.x);
        actualVector.y = Mathf.Clamp(actualVector.y, min.y, max.y);
        actualVector.z = Mathf.Clamp(actualVector.z, min.z, max.z);
        return actualVector;
    }
}
