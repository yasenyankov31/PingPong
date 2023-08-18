using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BallScript : MonoBehaviour
{



    public float forceMagnitude = 5f;
    public float paddleForce = 1f;
    public AudioClip[] tableHitSounds;
    public Collider prevCollider;
    private Rigidbody rb;

    AudioSource audioSource;
    int audioClipElement = 0;
    int bounceCounter = 0;

    [Header("Rewind")]
    private bool isRewinding = false;
    private List<(Vector3 position, Vector3 velocity)> ballRewindTrailInfo;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballRewindTrailInfo = new List<(Vector3 position, Vector3 velocity)>();
        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    private void PlaySoundOnCollision()
    {
        if (audioClipElement == tableHitSounds.Length)
        {
            audioClipElement = 0;
        }
        audioSource.clip = tableHitSounds[audioClipElement];
        audioSource.Play();
        audioClipElement++;

    }
    private void ResetGame()
    {
        if (prevCollider != null)
        {
            prevCollider.enabled = true;
        }
        PlayerActions playerActions = FindObjectOfType<PlayerActions>();
        playerActions.ResetServiceVariables();
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            var other_balls = FindObjectsOfType<BallScript>();
            if (other_balls.Length > 1)
            {
                Destroy(this.gameObject);
            }
            else {
                ResetGame();
            }
            
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
            if (bounceCounter > 2)
            {
                ResetGame();
            }
            else
            {
                PlaySoundOnCollision();
            }

            bounceCounter++;
        }


    }

    public void ResetRewindVariables()
    {
        ballRewindTrailInfo.Clear();
    }

    public void StartRewind()
    {
        prevCollider.enabled = true;
        isRewinding = true;
        rb.isKinematic = true;
        bounceCounter = 0;
    }

    public void StopRewind()
    {
        rb.isKinematic = false;
        isRewinding = false;
        if (ballRewindTrailInfo.Count>0) {
            rb.velocity = ballRewindTrailInfo[0].velocity;
        }
        

        ResetRewindVariables();

    }

    public void Rewind()
    {
        if (ballRewindTrailInfo.Count>1) {
            transform.position = ballRewindTrailInfo[0].position;
            ballRewindTrailInfo.RemoveAt(0);
        }


    }

    public void Record()
    {
        ballRewindTrailInfo.Insert(0, (transform.position, rb.velocity));
    }
}