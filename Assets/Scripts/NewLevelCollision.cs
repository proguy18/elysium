using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLevelCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject levelControl; 
    private LevelController levelController;
    // Update is called once per frame
    private void Awake() {
        levelControl = GameObject.Find("Level Controller");
        levelController = levelControl.GetComponent<LevelController>();
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "Player(Clone)"){
            levelController.playerFinishedLevel();
        }
        Debug.Log(other.gameObject.name);
    }
}
