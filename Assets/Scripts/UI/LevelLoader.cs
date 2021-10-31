using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    IEnumerator LoadScene(string sceneName)
    {
        // Play animation
        transition.SetTrigger("Start");
    
        // Wait
        yield return new WaitForSecondsRealtime(transitionTime);

        // Load Scene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        
        // Resumes Game
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        StartCoroutine(LoadScene("StartScene"));
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene("SampleScene"));
    }
    
    public void StartTutorial()
    {
        StartCoroutine(LoadScene("TutorialScene"));
    }
}
