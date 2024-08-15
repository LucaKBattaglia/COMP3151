using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        //GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        print(zTilt);
        Vector3 tempAng = transform.rotation.eulerAngles;
        print(tempAng);
        tempAng += new Vector3(0, 0, zTilt);
        Quaternion newAng = Quaternion.Euler(tempAng);
        print(newAng);
        StartCoroutine(rotateCam(transform.rotation, newAng, 0.2f));
    }

    IEnumerator rotateCam(Quaternion start, Quaternion end, float duration) {
        if (duration > 0f) { 
            float startTime = Time.time;   
            float endTime = startTime + duration;
            transform.rotation = start;
            yield return null;
            while (Time.time < endTime) {
                float progress = (Time.time - startTime) / duration;
                transform.rotation = Quaternion.Slerp(start, end, progress);
                yield return null;
            }
        }
        transform.rotation = end;
    }
}