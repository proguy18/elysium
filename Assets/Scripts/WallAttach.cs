// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WallAttach : MonoBehaviour
// {
//     RaycastHit finalHit; 
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
//     void findBestDistance(){
//         if (finalHit != null){
//             List<Vector3> directions = new List<Vector3>();
//             directions.Add(Vector3.forward);
//             directions.Add(Vector3.back);
//             directions.Add(Vector3.left);
//             directions.Add(Vector3.right);
//             directions.Add(Vector3.forward + Vector3.left);
//             directions.Add(Vector3.forward + Vector3.right);
//             directions.Add(Vector3.back + Vector3.left);
//             directions.Add(Vector3.back + Vector3.right);
//             List<RaycastHit> hits = new List<RaycastHit>();
//             foreach (Vector3 dir in directions){
//                 RaycastHit hit = new RaycastHit();
//                 if (Physics.Raycast(trasform.position, dir, 15)) hits.Add(hit);
//             }

//             float distance = float.MaxValue;
//             RaycastHit bestHit = null;
//             foreach (RaycastHit hit in hits){
//                 if (distance > hit.distance){
//                     distance = hit.distance;
//                     bestHit = hit;
//                 }
//             }
//             finalHit = bestHit;
//         }
//     }
// }
