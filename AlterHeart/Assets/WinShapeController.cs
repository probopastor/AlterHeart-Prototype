using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinShapeController : MonoBehaviour
{
    public GameObject player;
    public GameObject winPanel;
    public GameObject crosshair;
    public GameObject panelDimension1;
    public GameObject panelDimension2;

    // Start is called before the first frame update
    void Start()
    {
        winPanel.SetActive(false);
    }

    public void Win()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        crosshair.SetActive(false);
        panelDimension1.SetActive(false);
        panelDimension2.SetActive(false);
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
