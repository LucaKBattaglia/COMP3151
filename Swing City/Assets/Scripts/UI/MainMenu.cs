using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string scene;

    private void Update() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }
    public void PlayGame() {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Application is quitting, game can only be quit when built");
    }
}
