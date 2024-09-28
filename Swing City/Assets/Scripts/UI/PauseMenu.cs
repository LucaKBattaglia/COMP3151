using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject DisplayPauseMenu;

    [SerializeField] private Slider xSlider;
    [SerializeField] private TextMeshProUGUI xSliderText;
    [SerializeField] private Slider ySlider;
    [SerializeField] private TextMeshProUGUI ySliderText;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        // Set X Sensitivity based on pref
        if (PlayerPrefs.HasKey("xSensitivity")) GameManager.instance.sensXRate = PlayerPrefs.GetFloat("xSensitivity");
        xSlider.value = GameManager.instance.sensXRate;

        // Set Y Sensitivity based on pref
        if (PlayerPrefs.HasKey("ySensitivity")) GameManager.instance.sensYRate = PlayerPrefs.GetFloat("ySensitivity");
        ySlider.value = GameManager.instance.sensYRate;

        float xSliderValue = GameManager.instance.sensXRate * 100f;
        float ySliderValue = GameManager.instance.sensYRate * 100f;
        xSliderText.text = "" + xSliderValue;
        ySliderText.text = "" + ySliderValue;

        // Set Quality based on pref
        if (PlayerPrefs.HasKey("qualityLevel")) qualityDropdown.value = PlayerPrefs.GetInt("qualityLevel");
        else qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        DisplayPauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }

    public void Play()
    {
        DisplayPauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenuButton()
    {   //Only works if their is 1 level in-front on main menu
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
        SceneManager.LoadScene("MainMenu");
    }

    // Handled by Option Sliders
    public void SetXSensitivity(float newValue)
    {
        newValue = Mathf.Round(newValue * 100f) / 100f;
        GameManager.instance.sensXRate = newValue;
        PlayerPrefs.SetFloat("xSensitivity", newValue);
        float xSliderValue = newValue * 100f;
        xSliderText.text = "" + xSliderValue;
    }

    // Handled by Option Sliders
    public void SetYSensitivity(float newValue)
    {
        newValue = Mathf.Round(newValue * 100f) / 100f;
        GameManager.instance.sensYRate = newValue;
        PlayerPrefs.SetFloat("ySensitivity", newValue);
        float ySliderValue = newValue * 100f;
        ySliderText.text = "" + ySliderValue;
    }

    // Handled by Option Dropdown
    public void SetQualitySetting(int newSetting)
    {
        QualitySettings.SetQualityLevel(newSetting);
        PlayerPrefs.SetInt("qualityLevel", newSetting);
    }
}


