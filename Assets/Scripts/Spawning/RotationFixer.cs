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
    public bool rotx = false;
    public bool roty = false;
    public bool rotz = false;
    private bool rotated = false;

    // Start is called before the first frame update
    private void Update() {
        float x = transform.rotation.eulerAngles.x;
        float y = transform.rotation.eulerAngles.y;
        float z = transform.rotation.eulerAngles.z;
        float w = transform.rotation.w;
        if (setx || sety || setz){
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
        }
        
        
    }
    private void Start() {
        rotated = false;
    }
    /*public void rotate(){
        else if ((rotx ||roty ||rotz)){

            if (rotx) x = xrotation;
            if (roty) y = yrotation;
            if (rotz) z = zrotation;
            
            
        
        }
    }*/

}
