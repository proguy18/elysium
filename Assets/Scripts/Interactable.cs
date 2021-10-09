using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public GameObject uiObject;

    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
    }

    void OnTriggerEnter (Collider player) 
    {
        if (player.gameObject.tag == "Player") 
        {
            uiObject.SetActive(true);
        }
    }

    void OnTriggerExit (Collider player) 
    {
        if (player.gameObject.tag == "Player") 
        {
            uiObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
