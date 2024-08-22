using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    private void Update(){
        Cursor.lockState = CursorLockMode.None; 
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application is quitting, game can only be quit when built");
    }
}
