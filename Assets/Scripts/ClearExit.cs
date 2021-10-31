using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearExit : MonoBehaviour
{   
    public float distance = 5;
    private int sceneryLayer = 8;
    // Start is called before the first frame update
    public void Clear(){
        Collider[] hitentities = Physics.OverlapSphere(transform.position, distance, sceneryLayer);
        foreach (Collider hit in hitentities){
            hit.gameObject.SetActive(false);
        }
    }
}
