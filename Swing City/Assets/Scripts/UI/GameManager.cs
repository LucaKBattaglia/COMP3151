using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameData gameData;

    // Dictionary to store key states (true if collected)
    private Dictionary<string, bool> collectedKeys = new Dictionary<string, bool>();

    private Dictionary<string, TimeSpan> recordTimes = new Dictionary<string, TimeSpan>();

    // DEBUG MODE
    [Header("Debug Mode")]
    [SerializeField] private bool getKey1 = false;
    [SerializeField] private bool getKey2 = false;
    [SerializeField] private bool getKey3 = false;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Predefine keys in the dictionary with default values as false (not collected) unless specified by Debug Mode
        collectedKeys.Add("Key1", getKey1);
        collectedKeys.Add("Key2", getKey2);
        collectedKeys.Add("Key3", getKey3);

        recordTimes.Add("Level1", TimeSpan.Zero);
        recordTimes.Add("Level2", TimeSpan.Zero);
        recordTimes.Add("Level3", TimeSpan.Zero);
    }

    private void Start()
    {
        LoadGame();
    }

    // Method to collect any key by name
    public void CollectKey(string keyName)
    {
        // Mark the specified key as collected (true)
        if (collectedKeys.ContainsKey(keyName))
        {
            collectedKeys[keyName] = true;
            Debug.Log($"{keyName} collected!");
        }
        else
        {
            Debug.LogWarning($"Key \"{keyName}\" does not exist in the key dictionary.");
        }
    }

    // Method to check if a specific key has been collected
    public bool HasKey(string keyName)
    {
        // Return true if the key exists and is collected, otherwise false
        if (collectedKeys.ContainsKey(keyName))
        {
            return collectedKeys[keyName];
        }
        return false;
    }

    public void SetRecordTime(string levelName, TimeSpan newTime) {
        if (recordTimes.ContainsKey(levelName))
        {
            if (recordTimes[levelName].TotalMilliseconds > newTime.TotalMilliseconds)
            {
                recordTimes[levelName] = newTime;
                Debug.Log($"{levelName} recorded with {newTime.ToString()}!");
            }
            else
            {
                Debug.Log($"{levelName} finished with {newTime.ToString()} but did not beat {recordTimes[levelName].ToString()}. Better luck next time.");
            }
        }
        else
        {
            Debug.LogWarning($"Level \"{levelName}\" does not exist in the level dictionary.");
        }
    }

    // Method to receive the beaten time from a specified level by name
    public TimeSpan GetRecordTime(string levelName)
    {
        if (recordTimes.ContainsKey(levelName))
        {
            if (!string.Equals(recordTimes[levelName].ToString(), TimeSpan.Zero.ToString())) return recordTimes[levelName];
        }
        else
        {
            Debug.LogWarning($"Level \"{levelName}\" does not exist in the level dictionary.");
        }
        return TimeSpan.Zero;
    }

    // GAME DATA //
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (this.gameData == null)
        {
            Debug.Log("No data found. Setting data to defaults.");
            NewGame();
        }

        recordTimes["Level1"] = gameData.recordTime;
        Debug.Log(recordTimes["Level1"].ToString());
    }

    public void SaveGame()
    {
        Debug.Log("Game Data saved with " + gameData.recordTime.ToString() + ". Goodbye!");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
