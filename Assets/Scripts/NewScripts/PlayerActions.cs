using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerActions : MonoBehaviour
{
    public Transform servePoint;
    public GameObject ball;
    public float servicePower;
    public float hitBackPower;
    public float movementTolerance = 0.01f;
    public float slowMomentum = 0.3f;
    public float distanceBtweenBall;
    private Vector3 startingHitPosition;

    public bool canServeaAgain;
    private bool isPressed;
    private PlayerMovement playerMovement;
    public AudioSource paddleAudioSource;
    public AudioSource specialEffectSource;
    public AudioClip slowTimeClip;
    public AudioClip[] stopTimeClips;
    public AudioClip[] paddleHitClips;
    public GameObject rewindScreen;
    public VideoPlayer videoPlayer;
    public TimeSphere timeSphere;


    public bool ballIsClose = false;
    public bool isRewinding, isTimeSlowed,isTimeStoped;
    private int counter;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void PlaySpecialEffectSound(AudioClip clip,float volume) {
        specialEffectSource.clip = clip;
        specialEffectSource.volume = volume;
        specialEffectSource.Play();
    }

    private void RotatePaddleByPlayerInput()
    {
        BallScript ballScript = FindAnyObjectByType<BallScript>();
        if (ballScript != null)
        {
            float distance = Vector3.Distance(transform.position, ballScript.transform.position);
            if (distance < distanceBtweenBall && !ballIsClose)
            {
                startingHitPosition = transform.position;
                ballIsClose = true;
            }
            if (distance > distanceBtweenBall)
            {
                ballIsClose = false;
            }

            if (distance < 40)
            {
                float horizontalMovement = transform.position.x - startingHitPosition.x;
                float distanceForce = Vector3.Distance(transform.position, startingHitPosition);
                //Debug.Log(distanceForce);
                if (Mathf.Abs(horizontalMovement) > movementTolerance)
                {
                    float answerAngle = playerMovement.Remap(distanceForce, 0, 50, 0, 45);
                    if (horizontalMovement > 0)
                    {
                        playerMovement.yAngle = answerAngle;
                    }
                    else
                    {
                        playerMovement.yAngle = -answerAngle;
                    }
                }
                else
                {
                    playerMovement.yAngle = 0;
                }
            }


        }
    }
    public void Actions()
    {
        RewindBallTime();
        MakeService();
        RotatePaddleByPlayerInput();
        SlowTime();
        StopTime();
    }

    private void StopTime() {
        if (Input.GetKeyDown(KeyCode.Q) && !isTimeStoped) {
            Time.timeScale = 0;
            isTimeStoped = true;
            PlaySpecialEffectSound(stopTimeClips[0], 1);
            timeSphere.StartScaleUp();
            
        }
        if (isTimeStoped) {
            if (!specialEffectSource.isPlaying)
            {
                timeSphere.StartScaleDown();
                PlaySpecialEffectSound(stopTimeClips[1], 1);
            }
            if (timeSphere.isScaledDown)
            {
                isTimeStoped = false;
                timeSphere.isScaledDown=false;
                Time.timeScale = 1;
            }
        }
    }
    private void RewindBallTime()
    {
        var ball = FindAnyObjectByType<BallScript>();
        if (Input.GetKeyDown(KeyCode.E) && ball != null)
        {
            isRewinding = true;
            ball.StartRewind();
        }
        rewindScreen.SetActive(isRewinding);
        if (isRewinding && !videoPlayer.isPlaying)
        {
            isRewinding = false;
            ball.StopRewind();
        }
        
    }
    private void SlowTime()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isTimeSlowed)
        {
            PlaySpecialEffectSound(slowTimeClip, 1);
            isTimeSlowed = true;            
        }
        if (isTimeSlowed)
        {
            Time.timeScale = slowMomentum;
            if (!specialEffectSource.isPlaying)
            {
                isTimeSlowed = false;
                Time.timeScale = 1f;
            }
           
        }
    }
    private void MakeService()
    {

        float distance = Vector3.Distance(transform.position, servePoint.position);
        //
        if (distance < 25 && isPressed && canServeaAgain)
        {
            InstantiateBall();
            playerMovement.yAngle = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
    }
    public void InstantiateBall()
    {
        canServeaAgain = false;
        servePoint.GetComponentInChildren<MeshRenderer>().enabled = false;
        playerMovement.maxZ = transform.position.z + playerMovement.paddleZDisctance;
        Collider paddleCollider = GetComponent<Collider>();
        paddleCollider.enabled = false;


        GameObject ballInst = Instantiate(ball, servePoint.position, servePoint.rotation);
        ballInst.GetComponent<BallScript>().prevCollider= paddleCollider;
        Rigidbody rigidbodyBall = ballInst.GetComponent<Rigidbody>();
        PlayPaddleHitSound();
        rigidbodyBall.AddForce(servicePower * servePoint.forward, ForceMode.Impulse);

    }

    public void ResetServiceVariables()
    {
        canServeaAgain = true;
        playerMovement.maxZ = playerMovement.paddleZStartingPos;
        servePoint.GetComponentInChildren<MeshRenderer>().enabled = true;
        playerMovement.yAngle = 0;
    }

    private void PlayPaddleHitSound()
    {
        if (counter>paddleHitClips.Length-1) {
            counter = 0;
        }
        paddleAudioSource.clip = paddleHitClips[counter];
        paddleAudioSource.Play();
        counter ++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb.velocity.magnitude > 1)
        {
            rb.velocity = rb.velocity.normalized;
        }
        PlayPaddleHitSound();
        rb.AddForce(servePoint.forward * hitBackPower, ForceMode.Impulse);
        playerMovement.yAngle = 0;
    }
}
