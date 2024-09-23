using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public TimeSpan time;
    public float curTime;
    public bool active;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        curTime = 0f;
        getTime();
        tmp.text = "Time: " + curTime.ToString();
        start();
    }

    void Update()
    {
        if (active)
        {
            curTime = curTime + Time.deltaTime;
        }
        getTime();
        tmp.text = "Time: " + time2String(time.Hours) + ":" + time2String(time.Minutes) + ":" + time2String(time.Seconds) + ":" + time2String(time.Milliseconds, true);
    }

    void FixedUpdate()
    {
        
    }

    public string time2String(int val, bool isMil = false) {
        if (isMil) {
            if (val < 10) // val is 001 -> 009
            {
                return "00" + val.ToString();
            }
            if (val < 100) { // val is 010 -> 099
                return "0" + val.ToString();
            }
        }
        else if (val < 10) { // val is 01 -> 09 
            return "0" + val.ToString();
        }
        return val.ToString(); // val is 10 -> 59 OR 100 -> 999
    }

    public void start() { active = true; }
    public void stop()  { active = false; }
    public TimeSpan getTime()
    {
        time = TimeSpan.FromSeconds(curTime);
        return time;
    }
}
