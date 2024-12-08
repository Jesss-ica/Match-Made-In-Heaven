using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool Paused = false;
    private bool GameRunning = true;

    public Button AnswerButton1;
    public Button Resume;
    public GameObject PausePopup;
    public GameObject PauseButton;
    public GameObject BackdropBlur;

    public AudioSource TextTypeEffect;
    void Start()
    {
        Time.timeScale = 1f;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void UnPauseGame()
    {
        PauseButton.SetActive(true);
        PausePopup.SetActive(false);
        BackdropBlur.SetActive(false);
        AnswerButton1.Select();
        Paused = false;
        Time.timeScale = 1f;
        LoggerMulti.instance.audioManager.ResumeAudio(TextTypeEffect);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        PauseButton.SetActive(false);
        PausePopup.SetActive(true);
        BackdropBlur.SetActive(true);
        Resume.Select();
        Time.timeScale = 0f;
        Paused = true;
        LoggerMulti.instance.audioManager.PauseAudio(TextTypeEffect);
        //Cursor.lockState = CursorLockMode.None;
    }

    public void PauseMenuManager()
    {
        if (Paused == false)
        {
            PauseGame();
        }
        else if (Paused == true)
        {
            UnPauseGame();
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && GameRunning == true)
        {
            PauseMenuManager();
        }
    }
}