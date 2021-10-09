using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public GameObject uiObject;

    public float radius = 3f;

    public Transform interactionTransform;

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

    void OnDrawGizmosSelected ()
    {
        if (interactionTransform == null) 
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    public virtual void Interact ()
    {
        Debug.Log("Interacting with " + transform.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
