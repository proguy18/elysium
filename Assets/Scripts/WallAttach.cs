using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAttach : MonoBehaviour
{
    RaycastHit finalHit = new RaycastHit();
    bool hasMoved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tryCast() && !hasMoved){
            findBestDistance();
            moveToPosition();
        }
        Debug.Log(tryCast());
        
    }
    void moveToPosition(){
        transform.position = finalHit.point;
        transform.Rotate(finalHit.normal);
        hasMoved = true;

    }
    void findBestDistance(){
        if (finalHit.Equals(new RaycastHit())){
            List<Vector3> directions = new List<Vector3>();
            directions.Add(Vector3.forward);
            directions.Add(Vector3.back);
            directions.Add(Vector3.left);
            directions.Add(Vector3.right);
            directions.Add(Vector3.forward + Vector3.left);
            directions.Add(Vector3.forward + Vector3.right);
            directions.Add(Vector3.back + Vector3.left);
            directions.Add(Vector3.back + Vector3.right);
            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (Vector3 dir in directions){
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(transform.position, dir, out hit)){
                    hits.Add(hit);
                    Debug.Log(hit.normal);
                }
            }

            float distance = float.MaxValue;
            foreach (RaycastHit hit in hits){
                if (distance > hit.distance){
                    distance = hit.distance;
                    finalHit = hit;
                }
            }
        }
    }
    bool tryCast(){
        return Physics.Raycast(transform.position, Vector3.forward);
    }
}
