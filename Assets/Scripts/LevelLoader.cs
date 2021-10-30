using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     LoadNextLevel();
        // }
    }

    // public void LoadNextLevel()
    // {
    //     StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    // }

    // IEnumerator LoadLevel(int levelIndex)
    // {
    //     // Play animation
    //     transition.SetTrigger("Start");
    //
    //     // Wait
    //     yield return new WaitForSeconds(transitionTime);
    //
    //     // Load Scene
    //     SceneManager.LoadScene(levelIndex);
    // }
    IEnumerator LoadScene(string sceneName)
    {
        // Play animation
        transition.SetTrigger("Start");
    
        // Wait
        yield return new WaitForSecondsRealtime(transitionTime);
        Debug.Log("Scene being loaded...");
    
        // Load Scene
        // Invoke("LoadMyScene",2)
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        
        Debug.Log("Pressed go to main menu");
        // Play animation
        // transition.SetTrigger("Start");
        // Invoke("LoadMainMenu", transitionTime);
        // StartCoroutine(LoadLevel(0));
        // StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        // SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        StartCoroutine(LoadScene("StartScene"));
    }

    // private void LoadMainMenu()
    // {
    //     Debug.Log("Scene being loaded...");
    //     SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    // }

    public void StartGame()
    {
        Debug.Log("Pressed play again");
        // Play animation
        // transition.SetTrigger("Start");

        // Invoke("LoadGame", transitionTime);
        // StartCoroutine(LoadLevel(1));
        // StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        // SceneManager.LoadScene("IntegratedScene", LoadSceneMode.Single);
        StartCoroutine(LoadScene("IntegratedScene"));
    }

    // private void LoadGame()
    // {
    //     Debug.Log("Scene being loaded...");
    //     SceneManager.LoadScene("IntegratedScene", LoadSceneMode.Single);
    // }
}
