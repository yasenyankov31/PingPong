using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float forceMagnitude = 5f;
    public float paddleForce = 1f;
    public AudioClip[] tableHitSounds;
    private Collider prevCollider;

    AudioSource audioSource;
    int audioClipElement = 0;
    int bounceCounter = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySoundOnCollision() {
        if (audioClipElement == tableHitSounds.Length)
        {
            audioClipElement = 0;
        }
        audioSource.clip = tableHitSounds[audioClipElement];
        audioSource.Play();
        audioClipElement++;

    }
    private void ResetGame() {
        if (prevCollider != null)
        {
            prevCollider.enabled = true;
        }
        PlayerActions playerController = FindObjectOfType<PlayerActions>();
        playerController.ResetServiceVariables();
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            ResetGame();
        }
        if (collision.collider.CompareTag("Paddle"))
        {
            bounceCounter = 0;
            if (prevCollider != null)
            {
                prevCollider.enabled = true;
            }
            prevCollider = collision.collider;
            collision.collider.enabled = false;
        }
        if (collision.collider.CompareTag("Table"))
        {
            if (bounceCounter > 1)
            {
                ResetGame();
            }
            else {
                PlaySoundOnCollision();
            }
            
            bounceCounter++;
        }


    }


}