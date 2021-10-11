using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_TPSController : MonoBehaviour
{

    public float walkSpeed = 5f; 
    public float runSpeed = 8f;
    public float mouseSensitivity = 2.0f;
    public float lookXLimit = 60.0f;
    public bool movementAnimations = false; // will cause errors if changed during runtime

    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;

    public Transform playerCameraParent;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;
    private Animator m_Animator;
    private float y;

    void SetTrigger(string name){
        if(movementAnimations){
            m_Animator.SetTrigger(name);
        }
    }

    void ResetTrigger(string name){
        if(movementAnimations){
            m_Animator.ResetTrigger(name);
        }
    }
    int getAxis(KeyCode negativeDirection, KeyCode positiveDirection){
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

    void Start()
    {
        y = transform.position.y; // starting y-value
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        if(movementAnimations){
            m_Animator  = gameObject.GetComponent<Animator>();
        }
    }

    void animateMovements() {
        if(Input.GetKey(left) || Input.GetKey(right) || Input.GetKey(up) || Input.GetKey(down)){
            SetTrigger("Walk");
            if(Input.GetKey(run)){
                SetTrigger("Run");
            }
            else{
                ResetTrigger("Run");
            }
        } 
        else{
            ResetTrigger("Run");
            ResetTrigger("Walk");
        }
    }

    void Update()
    {

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 r = transform.TransformDirection(Vector3.right);

        if(movementAnimations){
           animateMovements(); 
        }
        
        float speed = Input.GetKey(run) ? runSpeed : walkSpeed;
        float curSpeedX = speed * getAxis(down, up);
        float curSpeedY = speed * getAxis(left, right);
        moveDirection = (forward * curSpeedX) + (r * curSpeedY);

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // prevent character from sinking into the grounded
        float offset = -transform.position.y + y; // set current position to 0, then add original y value
        characterController.Move(new Vector3(0, offset, 0));

        // Player and Camera rotation
        rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotation.x += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
        playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);

    }
}