using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject optionsUI;
    public GameObject deathMenuUI;
    public GameObject pauseMenuUI;
    public GameObject blurOverlayUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && 
            !SC_TPSController.hasDied && 
            !PlayerInventory.InventoryIsActive &&
            !optionsUI.activeSelf)
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }

        if (SC_TPSController.hasDied)
        {
            StartCoroutine(PlayDeathScreen());
        }
    }

    private void Pause()
    {
        EnableCursor();
        EnableUI();
        Time.timeScale = 0f;
        IsPaused = true;
        // mute gameplay sounds
        /*gameObject.GetComponent<PlayerAudioController>().togglePause();
        gameObject.GetComponent<PlayerAudioController>().stopSounds();*/
    }

    public void Resume()
    {
        DisableCursor();
        DisableUI();
        Time.timeScale = 1f;
        IsPaused = false;
        /*gameObject.GetComponent<PlayerAudioController>().togglePause();*/
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void DisableCursor()
    {
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
    
    private void EnableDeathUI()
    {
        deathMenuUI.SetActive(true);
        blurOverlayUI.SetActive(true);
    }
    
    private IEnumerator PlayDeathScreen()
    {
        yield return new WaitForSeconds(1f);
        
        EnableCursor();
        EnableDeathUI();
        Time.timeScale = 0f;
    }
}
