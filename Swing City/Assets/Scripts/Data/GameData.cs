using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, double> recordTimes;

    // The values defined in this constructor will be the default values
    // The game starts with when there's no data to load
    public GameData() 
    {
        recordTimes = new SerializableDictionary<string, double>();
    }
}
