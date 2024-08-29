using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

        // Predefine keys in the dictionary with default values as false (not collected)
        collectedKeys.Add("Key1", false);
        collectedKeys.Add("Key2", false);
        collectedKeys.Add("Key3", false);
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
            Debug.LogWarning($"Key \"{keyName}\" does not exist in the dictionary.");
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
}
