using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject realityController;
    public GameObject crosshair;

    public GameObject pausePanel;
    public GameObject howToPlayPanel;

    public string restartGameSceneToLoad;

    private bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        crosshair.SetActive(true);
        pausePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(!isPaused)
        {
            isPaused = true;
            crosshair.SetActive(false);
            pausePanel.SetActive(true);
            howToPlayPanel.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }
        else if(isPaused)
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshair.SetActive(true);
            pausePanel.SetActive(false);
            howToPlayPanel.SetActive(false);
            Time.timeScale = 1;
        }

        realityController.GetComponent<RealityController>().RealityPanelActivation();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(restartGameSceneToLoad);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
