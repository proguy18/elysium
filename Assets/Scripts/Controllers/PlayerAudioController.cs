using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class PlayerAudioController : MonoBehaviour
{
    
    // Add movement sound
    public AudioSource _mainAudioSource;
    public AudioSource _secondaryAudioSource;
    public AudioSource _secondaryAudioSource1;
    public AudioClip walking;
    public AudioClip running;
    public AudioClip swordSwing;
    public AudioClip itemPick;
    
    private float attackCooldown = 0f;
    public float attackSpeed = 1f;

    public bool isPaused = false;
    private bool uiIsNotActive;
    
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode attack = KeyCode.Mouse0;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //uiIsNotActive = (!PauseMenu.IsPaused && !PlayerInventory.InventoryIsActive);
        //Debug.Log((PauseMenu.IsPaused, PlayerInventory.InventoryIsActive));
        /*if (!isPaused)
        {
            movementSounds();
            attackCooldown -= Time.deltaTime;
        }*/
        movementSounds();
        attackCooldown -= Time.deltaTime;
    }

    private void movementSounds()
    {
        if((Input.GetKey(left) || Input.GetKey(right) || Input.GetKey(up) || Input.GetKey(down)) /*&& uiIsNotActive*/)
        {
            movementSound();
        }
        else
        {
            _mainAudioSource.Stop();
        }
        
        if(Input.GetKeyDown(attack) /*&& uiIsNotActive*/) 
        {
            attackSound();
        }
        else
        {
            _secondaryAudioSource.Stop();
        }
    }

    private void movementSound()
    {
        if(Input.GetKey(run)){
            Debug.Log("running");
                
            if (_mainAudioSource.clip != running)
            {
                Debug.Log("setting clip");
                _mainAudioSource.Stop();
                _mainAudioSource.clip = running;
                _mainAudioSource.Play();
            }
                
            if (!_mainAudioSource.isPlaying)
            {
                _mainAudioSource.Play();
            }
        }
        else
        {
            if (_mainAudioSource.clip != walking)
            {   
                _mainAudioSource.Stop();
                _mainAudioSource.clip = walking;
                _mainAudioSource.Play();
            }

            if (!_mainAudioSource.isPlaying)
            {
                _mainAudioSource.Play();
            }
        }
    }

    private void attackSound()
    {
        
        if (!(attackCooldown <= 0f)) return;
        
        
        if (_secondaryAudioSource.clip != swordSwing)
        {
            //Debug.Log(attackCooldown);
            _secondaryAudioSource.Stop();
            _secondaryAudioSource.clip = swordSwing;
            _secondaryAudioSource.Play();
        }

        if (!_secondaryAudioSource.isPlaying)
        {
            _secondaryAudioSource.Play();
            Debug.Log(attackCooldown);
        }

        attackCooldown = 1 / attackSpeed;
    }
    
    public void pickItem()
    {
        if (_secondaryAudioSource1.clip != itemPick)
        {
            _secondaryAudioSource1.Stop();
            _secondaryAudioSource1.clip = itemPick;
            _secondaryAudioSource1.Play();
        }

        if (!_secondaryAudioSource1.isPlaying)
        {
            _secondaryAudioSource1.Play();
        }

    }

    public void togglePause()
    {
        isPaused = !isPaused;
    }

    public void stopSounds()
    {
        _mainAudioSource.Stop();
        _secondaryAudioSource.Stop();
    }
}
