/*****************************************************************************
// File Name: MainMenuManager.cs
// Author: Billy
//
// Brief Description: Defines the use of buttons in the main menu
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
