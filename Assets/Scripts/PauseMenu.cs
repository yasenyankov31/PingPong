using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public bool isOpenedMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpenedMenu = !isOpenedMenu;
               
        }
        Cursor.lockState =!isOpenedMenu ? CursorLockMode.Locked :CursorLockMode.None;
        menuPanel.SetActive(isOpenedMenu);

    }
    public void Resume() {
        isOpenedMenu = false; 
    }

    public void ExitApp() {
        Application.Quit();
    }
}
