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

    public void fadeIn() {
        StartCoroutine(fader(alpha, 1f, 1f));
    }

    public void fadeOut() {
        StartCoroutine(fader(alpha, 0f, 1f));
    }

    IEnumerator fader(float start, float end, float dur) {
        if (dur > 0f) { 
            float startTime = Time.time;   
            float endTime = startTime + dur;
            alpha = start;
            yield return null;
            while (Time.time < endTime) {
                float progress = (Time.time - startTime) / dur;
                alpha = Mathf.Lerp(start, end, progress);
                yield return null;
            }
        }
        alpha = end;
    }
}
