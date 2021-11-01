// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class TutorialEnd : MonoBehaviour
// {
//     private bool doorIsAnimated = false;
//     // Update is called once per frame
//     void Update()
//     {
//         if (hasReachedEndTutorial())
//             PlayDoorAnimations();
//     }
//
//     private void PlayDoorAnimations()
//     {
//         gameObject.transform.GetChild(2).gameObject.SetActive(true);
//         gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
//         doorIsAnimated = true;
//     }
//
//     private void OnCollisionEnter(Collision other) {
//         if (other.gameObject.name == "Player(Clone)" && doorIsAnimated){
//             SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
//         }
//         Debug.Log(other.gameObject.name);
//     }
// }
