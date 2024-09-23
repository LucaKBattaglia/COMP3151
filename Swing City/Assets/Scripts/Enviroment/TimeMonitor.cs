using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeMonitor : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private TextMeshPro tmp;

    private TimeSpan recordTime;
    private string timeString;
    
    // Start is called before the first frame update
    void Start()
    {
        recordTime = GameManager.instance.GetRecordTime(this.transform.parent.name);
        if (string.Equals(recordTime.ToString(), TimeSpan.Zero.ToString()))
        {
            timeString = "--:--:--:---";
        }
        else
        {
            timeString = time2String(recordTime.Hours) + ":" + time2String(recordTime.Minutes) + ":" + time2String(recordTime.Seconds) + ":" + time2String(recordTime.Milliseconds, true);
        }
        tmp.text = levelName + "<br><br>Best Time: <br>" + timeString;
    }

    // Update is called once per frame
    void Update()
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
}
