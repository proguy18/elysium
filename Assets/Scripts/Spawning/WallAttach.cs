using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAttach : MonoBehaviour
{
    RaycastHit finalHit = new RaycastHit();
    Vector3 normal = new Vector3(-40,-40,-40);
    Vector3 point = new Vector3(-40,-40,-40);
    public float adjustment = 0.3f;
    public float height = 1.0f; 

    // Update is called once per frame
    private void Awake() {
        MoveLights();
    }
    public void MoveLights(){
        findBestDistance();
        moveToPosition();
    }
    void moveToPosition(){
        transform.position = point + adjustment*normal;
        transform.rotation = Quaternion.LookRotation(normal);

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
                }
            }

            float distance = float.MaxValue;
            Vector3 _normal = new Vector3(-40, -40, -40);
            Vector3 _point = new Vector3(-40, -40, -40);
            foreach (RaycastHit hit in hits){
                if (distance > hit.distance){
                    distance = hit.distance;
                    _normal = hit.normal;
                    _point = hit.point;
                }
            }
            if (_normal != new Vector3(-40, -40, -40) && _point != new Vector3(-40, -40, -40)){
                normal = _normal;
                point = _point;
                return;
            }
            gameObject.SetActive(false);
            return;
        }
    }
    bool tryCast(){
        return Physics.Raycast(transform.position, Vector3.forward);
    }
}
