using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Default Player Settings")]
    public float sensXRate = 0.4f;
    public float sensYRate = 0.4f;
    
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    // DEBUG MODE
    [Header("Debug Mode")]
    [SerializeField] private bool getKey1 = false;
    [SerializeField] private bool getKey2 = false;
    [SerializeField] private bool getKey3 = false;

    private GameData gameData;
    private FileDataHandler dataHandler;
    private LogFile log;

    // Dictionary to store key states (true if collected)
    private Dictionary<string, bool> collectedKeys = new Dictionary<string, bool>();

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
    }

    private void Start()
    {
        // find the logfile script
        log = gameObject.GetComponent<LogFile>();
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
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
        if (gameData.recordTimes.ContainsKey(levelName))
        {
            if (gameData.recordTimes[levelName] == 0f || gameData.recordTimes[levelName] > newTime.TotalMilliseconds)
            {
                gameData.recordTimes[levelName] = newTime.TotalMilliseconds;
                Debug.Log($"{levelName} recorded with {newTime.ToString()}!");
            }
            else
            {
                Debug.Log($"{levelName} finished with {newTime.ToString()} but did not beat {gameData.recordTimes[levelName].ToString()}. Better luck next time.");
            }
        }
        else
        {
            gameData.recordTimes.Add(levelName, newTime.TotalMilliseconds);
        }
    }

    // Method to receive the beaten time from a specified level by name
    public TimeSpan GetRecordTime(string levelName)
    {
        if (gameData != null && gameData.recordTimes.ContainsKey(levelName))
        {
            if (!string.Equals(TimeSpan.FromMilliseconds(gameData.recordTimes[levelName]).ToString(), TimeSpan.Zero.ToString())) return TimeSpan.FromMilliseconds(gameData.recordTimes[levelName]);
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
        // Load any saved data from a file using Data Handler
        this.gameData = dataHandler.Load();
        
        // If no data exists from a file, create new game data
        if (this.gameData == null)
        {
            Debug.Log("No data found. Setting data to defaults.");
            NewGame();
        }
    }

    public void SaveGame()
    {
        // Save data to a file using Data Handler
        dataHandler.Save(gameData);
        Debug.Log("Game Data saved. Goodbye!");
    }

    public void LogToFile(params object[] args)
    {
        log.WriteLine(args);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
