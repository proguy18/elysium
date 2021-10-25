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
    }

    public void stopMainSound()
    {
        _mainAudioSource.Stop();
    }

    public void stopSecondarySound()
    {
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
        // to add attack sounds
    }
}
