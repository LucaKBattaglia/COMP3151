using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.instance.CollectKey("Key1");
            SceneManager.LoadScene("MainMenu");

        }
    }


}
