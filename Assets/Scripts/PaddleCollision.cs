using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleCollision : MonoBehaviour
{
    public AudioClip[] soundClips;
    private AudioSource audioSource;
    int clipIndex = 0;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }
    public void PlayPaddleSound() {
        if (clipIndex>soundClips.Length-1) {
            clipIndex = 0;
        }
        audioSource.clip = soundClips[clipIndex];
        audioSource.Play();
        clipIndex++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayPaddleSound();
    }
}
