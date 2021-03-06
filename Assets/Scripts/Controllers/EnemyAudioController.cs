using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class EnemyAudioController : MonoBehaviour
{
    
    // Add movement sound
    public AudioSource _mainAudioSource;
    public AudioSource _secondaryAudioSource;
    public AudioSource _secondaryAudioSource1;
    public AudioClip Movement;
    public AudioClip getHit;
    public AudioClip Attack;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            stopMainSound();
            stopSecondarySound();
        }
    }

    public void stopMainSound()
    {
        if(_mainAudioSource != null)
            _mainAudioSource.Stop();
    }

    public void stopSecondarySound()
    {
        if(_secondaryAudioSource != null)
            _secondaryAudioSource.Stop();
    }
    
    public void getHitSound () 
    {
        // Add movement sound
        if (_secondaryAudioSource.clip != getHit)
        {
            _secondaryAudioSource.Stop();
            _secondaryAudioSource.clip = getHit;
            _secondaryAudioSource.Play();
        }

        if (!_secondaryAudioSource.isPlaying)
        {
            _secondaryAudioSource.Play();
        }
    }

    public void movementSounds()
    {
        if (_mainAudioSource == null)
            return;
        // Add movement sound
        if (_mainAudioSource.clip != Movement)
        {
            _mainAudioSource.Stop();
            _mainAudioSource.clip = Movement;
            _mainAudioSource.Play();
        }

        if (!_mainAudioSource.isPlaying)
        {
            _mainAudioSource.Play();
        }
    }

    public void attackSound()
    {
        if (_secondaryAudioSource1 == null)
            return;
        // to add attack sounds
        // Add movement sound
        if (_secondaryAudioSource1.clip != Attack)
        {
            _secondaryAudioSource1.Stop();
            _secondaryAudioSource1.clip = Attack;
            _secondaryAudioSource1.Play();
        }

        if (!_secondaryAudioSource1.isPlaying)
        {
            _secondaryAudioSource1.Play();
        }
    }
    
}
