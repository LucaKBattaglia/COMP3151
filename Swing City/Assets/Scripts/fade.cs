using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{
    float alpha;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.color = new Color(0,0,0,0);
        alpha = img.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        img.color = new Color(0,0,0,alpha);
    }
}
