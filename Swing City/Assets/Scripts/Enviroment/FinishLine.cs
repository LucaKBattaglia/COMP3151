using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class FinishLine : MonoBehaviour
{
    public string keyNumber;  // For example, "1" so the key becomes "Key1"
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player collects the key
            GameManager.instance.CollectKey("Key" + keyNumber);
            if (!String.Equals(SceneManager.GetActiveScene().name, "PrototypeScene") || !String.Equals(SceneManager.GetActiveScene().name, "DemoScene"))
            {
                GameManager.instance.SetRecordTime(SceneManager.GetActiveScene().name, GameObject.Find("Timer").GetComponent<Timer>().getTime());
                GameManager.instance.LogToFile(SceneManager.GetActiveScene().name, GameObject.Find("Timer").GetComponent<Timer>().getTime());
            }
            SceneManager.LoadScene("Hub");
            Debug.Log("U have got key 1");
        }
    }

}
