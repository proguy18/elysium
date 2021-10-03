// Adapted from https://www.studica.com/blog/isometric-camera-unity 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    public float walkSpeed = 2f; //Change in inspector to adjust move speed
    public float runSpeed = 6f;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;
 
    Vector3 forwardV, rightV; // Keeps track of our relative forward and right vectors
    Animator m_Animator;

    void Start()
    {
        forwardV = Camera.main.transform.forward; // Set forward to equal the camera's forward vector
        forwardV.y = 0; // make sure y is 0
        forwardV = Vector3.Normalize(forwardV); // make sure the length of vector is set to a max of 1.0
        rightV = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardV; // set the right-facing vector to be facing right relative to the camera's forward vector
        m_Animator  = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if(Input.GetKey(left) || Input.GetKey(right) || Input.GetKey(up) || Input.GetKey(down)){
            m_Animator.SetTrigger("Walk");
            if(Input.GetKey(run)){
                m_Animator.SetTrigger("Run");
                Move(runSpeed);
            }
            else{
                m_Animator.ResetTrigger("Run");
                Move(walkSpeed); 
            }
        } 
        else{
            m_Animator.ResetTrigger("Run");
            m_Animator.ResetTrigger("Walk");
        }
    }

    int getAxisValue(KeyCode negativeDirection, KeyCode positiveDirection){
        if(Input.GetKey(negativeDirection) && Input.GetKey(positiveDirection)){
            return 0;
        }
        if(Input.GetKey(negativeDirection)){
            return -1;
        }
        if(Input.GetKey(positiveDirection)){
            return 1;
        }
        return 0;
    }


    void Move(float speed)
    {
        Vector3 direction = new Vector3(getAxisValue(left, right), 0, getAxisValue(down, up)); // setup a direction Vector based on keyboard input. GetAxis returns a value between -1.0 and 1.0. If the A key is pressed, GetAxis(HorizontalKey) will return -1.0. If D is pressed, it will return 1.0
        Vector3 rightMovement = rightV * speed * Time.deltaTime * getAxisValue(left, right); // Our right movement is based on the right vector, movement speed, and our GetAxis command. We multiply by Time.deltaTime to make the movement smooth.
        Vector3 upMovement = forwardV * speed * Time.deltaTime * getAxisValue(down, up); // Up movement uses the forward vector, movement speed, and the vertical axis inputs.
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement); // This creates our new direction. By combining our right and forward movements and normalizing them, we create a new vector that points in the appropriate direction with a length no greater than 1.0
        transform.forward = heading; // Sets forward direction of our game object to whatever direction we're moving in
        transform.position += rightMovement; // move our transform's position right/left
        transform.position += upMovement; // Move our transform's position up/down
    }
}

