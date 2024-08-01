using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public Camera cam;

    private void Start() {
        cam =  GetComponentInChildren<Camera>();
    }
    private void Update()
    {
       cam.transform.position = cameraPosition.position;
    }
}

