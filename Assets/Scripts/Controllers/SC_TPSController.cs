// Adapted from https://sharpcoderblog.com/blog/third-person-camera-in-unity-3d

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(CharacterStats))]
public class SC_TPSController : MonoBehaviour
{
    public static bool hasDied;

    public float walkSpeed = 5f; 
    public float runSpeed = 8f;
    public float mouseSensitivity = 4.0f;
    public bool movementAnimations = false; // will cause errors if changed during runtime

    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;
    
    public PostProcessResources postProcessResources;

    public float lookXLimit = 20;
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
        Camera camera = mainCamera.AddComponent<Camera>();
        mainCamera.transform.localPosition = new Vector3(0,2f,-2);
        mainCamera.transform.localRotation = Quaternion.Euler(30,0,0);
        PlayerCamera.Camera = camera;
        
        // Add postprocessing
        PostProcessLayer postProcessLayer = mainCamera.AddComponent<PostProcessLayer>();
        postProcessLayer.Init(postProcessResources);
        postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        postProcessLayer.volumeLayer = LayerMask.GetMask("Post-processing");

        Cursor.visible = false;

    }

    void Start()
    {
        // Debug.Log("start player position: " + transform.position +
        //           " playerCameraParent: " + playerCameraParent.position);
        hasDied = false;
        y = transform.position.y; // starting y-value
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        if(movementAnimations){
            m_Animator  = gameObject.GetComponent<Animator>();
        }
    }

    private void LateUpdate()
    {
        // Debug.Log("late update player position: " + transform.position +
        //           " playerCameraParent: " + playerCameraParent.position);
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
        hasDied = true;
        // Die animation
        m_Animator.SetTrigger("Die");
        m_Animator.SetBool("hasDied", true);
        

        // Disable the enemy
        
        gameObject.GetComponent<SC_TPSController>().enabled = false;
        // Disable the enemy


        // prevent mobs from pushing, attacking or moving towards the player
        Collider[] nearby = Physics.OverlapSphere(transform.position, 10);
        for (int i = 0; i < nearby.Length; i++) {
            CharacterCombat enemyCombat = nearby[i].gameObject.GetComponent<CharacterCombat>();
            UnityEngine.AI.NavMeshAgent NMA = nearby[i].gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if(enemyCombat){
                enemyCombat.enabled = false;
            }
            if(NMA){
                NMA.enabled = false;
            }
        }

        // mute gameplay sounds
        gameObject.GetComponent<PlayerAudioController>().togglePause();
        gameObject.GetComponent<PlayerAudioController>().stopSounds(); 
    }

    void FixedUpdate()
    {
        if (!PauseMenu.IsPaused)
        {
            if (movementAnimations)
            {
                if (!m_Animator)
                    m_Animator = gameObject.GetComponent<Animator>();
                if (IsAlive())
                    animateMovements();
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 r = transform.TransformDirection(Vector3.right);
            float speed = Input.GetKey(run) ? runSpeed : walkSpeed;
            float curSpeedX = speed * getAxis(down, up);
            float curSpeedY = speed * getAxis(left, right);
            moveDirection = (forward * curSpeedX) + (r * curSpeedY);

            // Move the controller
            if (IsAlive())
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
    
    bool IsAlive()
    {
        return !m_Animator.GetBool("hasDied");
    }
    private void PlayAttackAnimation()
    {
        m_Animator.SetTrigger("Attack_2");
    }
}