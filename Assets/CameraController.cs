// Adapted from ?

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private GameObject player = null;        //Public variable to store a reference to the player game object


    private Vector3 offset;            //Private variable to store the offset distance between the player and camera
    private Vector3 INITIAL_POS = new Vector3(-11, 10, -12);

    // Use this for initialization
    void Start () 
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        
    }

    // LateUpdate is called after Update each frame
    void LateUpdate () 
    {

        //Check if the player has spawned
        //Also this is not very quick i think 
        if (player == null){
            player = GameObject.Find("Player(Clone)");
            if (player != null){
                transform.position = player.transform.position + INITIAL_POS;
                offset = transform.position - player.transform.position;
            }
        }
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;


    }
}