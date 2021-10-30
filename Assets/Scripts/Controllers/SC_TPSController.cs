using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(CharacterStats))]
public class SC_TPSController : MonoBehaviour
{

    public float walkSpeed = 5f; 
    public float runSpeed = 8f;
    public float mouseSensitivity = 2.0f;
    public bool movementAnimations = false; // will cause errors if changed during runtime

    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;

    private float lookXLimit = 0;
    private Transform playerCameraParent;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;
    private Animator m_Animator;
    private float y;
    private CharacterCombat characterCombat;
    private CharacterStats stats;
    private void OnEnable()
    {
        stats.OnHealthReachedZero += Die;
        stats.OnDamaged += PlayOnHitAnimation;
        characterCombat.OnAttacking += PlayAttackAnimation;
    }

    private void OnDisable()
    {
        stats.OnHealthReachedZero -= Die;
        stats.OnDamaged -= PlayOnHitAnimation;
        characterCombat.OnAttacking -= PlayAttackAnimation;
    }
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

    void Awake(){
        characterCombat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
        playerCameraParent = new GameObject("CameraParent").transform;
        playerCameraParent.SetParent(transform);
        playerCameraParent.localPosition = new Vector3(0,1,0);
        GameObject mainCamera = new GameObject("MainCamera");
        mainCamera.transform.SetParent(playerCameraParent);
        mainCamera.tag = "MainCamera";
        mainCamera.AddComponent<Camera>();
        mainCamera.transform.localPosition = new Vector3(0,2.5f,-6);
        mainCamera.transform.localRotation = Quaternion.Euler(15,0,0);
        SC_CameraCollision cameraScript  = mainCamera.AddComponent<SC_CameraCollision>();
        
        // Add postprocessing
        PostProcessLayer postProcessLayer = mainCamera.AddComponent<PostProcessLayer>();
        postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        postProcessLayer.volumeLayer = LayerMask.GetMask("Post-processing");
        
        // Add audio controller
        /*AudioListener audioListener = mainCamera.AddComponent<AudioListener>();*/

        cameraScript.referenceTransform = playerCameraParent;
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
            else
            {
                ResetTrigger("Run");
            }
        } 
        else{
            ResetTrigger("Run");
            ResetTrigger("Walk");
        }
    }
    private void PlayOnHitAnimation()
    {
        m_Animator.SetTrigger("Hit");
    }

    void Die() 
    {
        Debug.Log("Player has died");
        // Die animation
        m_Animator.SetTrigger("Die");
        m_Animator.SetBool("hasDied", true);
        gameObject.GetComponent<SC_TPSController>().enabled = false;

        // Disable the enemy
        // Destroy(gameObject, 2.1f);
    }

    void FixedUpdate()
    {
        if(movementAnimations){
            if(!m_Animator){
                m_Animator  = gameObject.GetComponent<Animator>();
            }
            animateMovements(); 
        }
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 r = transform.TransformDirection(Vector3.right);
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
    private void PlayAttackAnimation()
    {
        m_Animator.SetTrigger("Attack_2");
    }
}