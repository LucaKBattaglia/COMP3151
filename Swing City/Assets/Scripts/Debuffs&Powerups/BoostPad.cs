using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public Vector3 dir;
    public Vector3 pos;
    public int spd;
    public string pLayer;

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.layer == LayerMask.NameToLayer(pLayer)) {
            col.gameObject.GetComponent<PlayerMovement>().boost(dir, spd, pos);
        }
    }
}
