using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
