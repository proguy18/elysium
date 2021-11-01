using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private KeyCode[] keyCodes;
    [SerializeField] private string[] messages;
    [SerializeField] private string congratsMessage;
    private int currentMessageIndex;

    private bool isNextMessageCoroutineRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        SetMessage();
    }

    private void SetMessage()
    {
        message.text = messages[currentMessageIndex];
    }
    
    private void SetCongratsMessage()
    {
        message.text = congratsMessage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCodes[currentMessageIndex]))
        {
            if (hasReachedEndTutorial())
                SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            else
                StartCoroutine(NextMessage());
        }
    }

    private IEnumerator NextMessage()
    {
        if(isNextMessageCoroutineRunning)
            yield break;
        isNextMessageCoroutineRunning = true;
        SetCongratsMessage();
        if (hasReachedEndTutorial())
            yield break;
        yield return new WaitForSecondsRealtime(2f);
        currentMessageIndex++;
        SetMessage();
        isNextMessageCoroutineRunning = false;
    }

    private bool hasReachedEndTutorial()
    {
        return currentMessageIndex == keyCodes.Length - 1;
    }
}
