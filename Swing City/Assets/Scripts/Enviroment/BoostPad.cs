using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    Vector3 dir;
    Vector3 pos;
    public int spd;
    public string pLayer;

    void Start()
    {
        dir = Vector3.forward + new Vector3(0, 0.1f, 0);
        print(dir);
        pos = new Vector3(0, 0.5f, 0);
    }

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.layer == LayerMask.NameToLayer(pLayer)) {
            col.gameObject.GetComponent<PlayerMovement>().boost(dir, spd, pos);
        }
    }
}
