using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform player;
    public Transform orientation;
    public Transform camPos;

    float xRotation, yRotation, zRotation;

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

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        // rotate cam and orientation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        player.Rotate(Vector3.up * mouseX);
        transform.position = camPos.position;
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        //GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        StartCoroutine(rotateCam(zRotation, zTilt, 0.4f));
    }

    IEnumerator rotateCam(float start, float end, float duration) {
        if (duration > 0f) { 
            float startTime = Time.time;   
            float endTime = startTime + duration;
            zRotation = start;
            yield return null;
            while (Time.time < endTime) {
                float progress = (Time.time - startTime) / duration;
                zRotation = Mathf.Lerp(start, end, progress);
                yield return null;
            }
        }
        zRotation = end;
    }
}