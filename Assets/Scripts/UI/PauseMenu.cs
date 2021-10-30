using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject blurOverlayUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        EnableCursor();
        EnableUI();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        DisableCursor();
        DisableUI();
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void EnableUI()
    {
        pauseMenuUI.SetActive(true);
        blurOverlayUI.SetActive(true);
    }
    
    private void DisableUI()
    {
        pauseMenuUI.SetActive(false);
        blurOverlayUI.SetActive(false);
    }
}
