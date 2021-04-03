using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=rnqF6S7PfFA


public class CustomCameraController : MonoBehaviour
{


    [SerializeField] Transform cameraTransform;

    [Header("rotation boundrees")]
    [SerializeField] float maxXrotation;
    [SerializeField] float minXrotation;


    [SerializeField] float movementTime;
    [SerializeField] float normalSpeed;
    [SerializeField] float fastSpeed;
    [SerializeField] float rotationAmount;
    [SerializeField] Vector3 zoomAmount;
    [SerializeField] float ortograficZoom = 1f;
    [SerializeField] float mouseRotationConst = 5f;
    float movementSpeed;

    Vector3 newPos;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPos;
    Vector3 dragCurrentPos;
    Vector3 rotateStartPos;
    Vector3 rotateCurrentPos;


    Vector3 maxClampPos;
    Vector3 minClampPos;
    Vector3 minClampZoom;
    Vector3 maxClampZoom;

    float minOrtograficSize;
    float maxOrtograficSize;


    void OnEnable()
    {

        maxClampPos = App.CameraManager.maxClampPos;
        minClampPos = App.CameraManager.minClampPos;
        minClampZoom = App.CameraManager.minClampZoom;
        maxClampZoom = App.CameraManager.maxClampZoom;
        minOrtograficSize = App.CameraManager.minOrtograficSize;
        maxOrtograficSize = App.CameraManager.maxOrtograficSize;

        newPos = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleKeyMovement();
        HandleMouseMovement();

        newZoom =  ClampVector3(newZoom, minClampZoom, maxClampZoom);
        newPos = ClampVector3(newPos, minClampPos, maxClampPos);

        newRotation = Quaternion.Euler(//Mathf.Clamp( newRotation.eulerAngles.x, minXrotation, maxXrotation),
                                       newRotation.eulerAngles.x,
                                       newRotation.eulerAngles.y,
                                       0);
        Debug.Log(newRotation.x);
        newRotation.x = Mathf.Clamp(newRotation.x, minXrotation, maxXrotation);

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        if (!Camera.main.orthographic)
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

        
    }

    private void HandleMouseMovement()
    {
        //pohyb
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPos = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                newPos = transform.position + dragStartPos - ray.GetPoint(entry);
            }
        }

        //zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Camera.main.orthographic) Camera.main.orthographicSize = Mathf.Clamp(
                    Camera.main.orthographicSize - ortograficZoom * Input.mouseScrollDelta.y,
                    minOrtograficSize,
                    maxOrtograficSize);
            else newZoom += zoomAmount * Input.mouseScrollDelta.y;
        }
        //rotacia
        if (Input.GetMouseButtonDown(2))
        {
            
            rotateStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPos = Input.mousePosition;
            Vector3 diff = rotateStartPos - rotateCurrentPos;
            rotateStartPos = rotateCurrentPos;

            newRotation *= Quaternion.Euler((Vector3.up * (-diff.x/mouseRotationConst) + Vector3.right * (diff.y / mouseRotationConst)));
            
        }

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
            
            
        }

        if (Input.GetAxisRaw("Camera Rotation") != 0 )
        {
            newRotation *= Quaternion.Euler(-Vector3.up * Input.GetAxisRaw("Camera Rotation") * rotationAmount);            
        }

        if (Input.GetAxisRaw("Camera Zoom") != 0)
        {
            if (Camera.main.orthographic)
            {
                if (Input.GetAxisRaw("Camera Zoom") > 0) Camera.main.orthographicSize = Mathf.Clamp(
                        Camera.main.orthographicSize + ortograficZoom,
                        minOrtograficSize,
                        maxOrtograficSize);
                else Camera.main.orthographicSize -= ortograficZoom;
            }
            else
            {
                if (Input.GetAxisRaw("Camera Zoom") > 0) newZoom += zoomAmount;
                else newZoom -= zoomAmount;
            }
        }

    }

    Vector3 ClampVector3(Vector3 actualVector, Vector3 min, Vector3 max)
    {
        actualVector.x = Mathf.Clamp(actualVector.x, min.x, max.x);
        actualVector.y = Mathf.Clamp(actualVector.y, min.y, max.y);
        actualVector.z = Mathf.Clamp(actualVector.z, min.z, max.z);
        return actualVector;
    }
}
