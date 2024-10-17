using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 2500f;
    public float sensY = 2500f;

    public Transform player;
    public Transform orientation;
    public Transform camPos;

    float xRotation, yRotation, zRotation;

    public bool freeze = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        freeze = false;
    }

    private void Update()
    {
        if(!freeze) {
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * GameManager.instance.sensXRate;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * GameManager.instance.sensYRate;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += mouseX;

            // rotate cam and orientation
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
            player.Rotate(Vector3.up * mouseX);
            transform.position = camPos.position;
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
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