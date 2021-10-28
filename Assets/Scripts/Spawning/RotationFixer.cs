using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFixer : MonoBehaviour
{
    public float xrotation = 270;
    public float yrotation = 0;
    public float zrotation = 0;
    public bool setx = false;
    public bool sety = false;
    public bool setz = false;

    // Start is called before the first frame update
    private void Update() {
        float x = transform.rotation.x;
        float y = transform.rotation.y;
        float z = transform.rotation.z;
        float w = transform.rotation.w;
        if (setx) x = xrotation;
        if (sety) y = yrotation;
        if (setz) z = zrotation;
        //modifying the Vector3, based on input multiplied by speed and time
        Vector3 currentEulerAngles = new Vector3(x, y, z);

        //moving the value of the Vector3 into Quanternion.eulerAngle format
        Quaternion currentRotation = new Quaternion();
        currentRotation.eulerAngles = currentEulerAngles;

        //apply the Quaternion.eulerAngles change to the gameObject
        transform.rotation = currentRotation;
        Debug.Log(transform.rotation);
        
    }

}
