using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunCalc : MonoBehaviour {

    public static WallRunCalc runCalc;

    public RectTransform barTransform;

    [SerializeField]
    GameObject barPrefab;

    public float maxStam;
    public float maxBarAmt;
    public float curStam;
    public float barWid;
    public float barHei;
    WallRunBar bar;

    void Start() {
        runCalc = this;
    }

    public void generateStam(float barAmt) {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
        maxBarAmt = barAmt;
        maxStam = maxBarAmt * barWid;
        curStam = maxStam;
        GameObject newBar = Instantiate(barPrefab, transform);
        barTransform = (RectTransform)newBar.transform;
        barTransform.sizeDelta = new Vector2(maxBarAmt * barWid, barHei);
        bar = newBar.GetComponent<WallRunBar>();
        bar.setMaxStam(maxStam);
    }

    public void modStam(float stamMod) {
        stamMod*=barWid;
        curStam -= stamMod;
        if (curStam < 0) {
            curStam = 0;
        }
        bar.setCurStam(curStam);
    }

    public void resetStam() {
        curStam = maxStam;
        bar.setMaxStam(maxStam);
    }
}