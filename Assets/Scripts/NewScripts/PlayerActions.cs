using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Transform servePoint;
    public GameObject ball;
    public float servicePower;
    public float slowMomentum = 0.3f;
    public float distanceBtweenBall;
    private Vector3 startingHitPosition;

    public bool canServeaAgain;
    private bool isPressed;
    private PlayerMovement playerMovement;
    public AudioSource paddleAudioSource;
    public AudioSource specialEffectSource;
    public AudioClip[] slowMotionClips;

    private bool isTimeSlowed;
    public bool ballIsClose = false;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
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

            if (distance<40) {
                float horizontalMovement = transform.position.x - startingHitPosition.x;

                if (horizontalMovement > 0)
                {
                    playerMovement.yAngle = 15;
                }
                else
                {
                    playerMovement.yAngle = -15;
                }
            }


        }
    }
    public void Actions()
    {
        MakeService();
        SlowTime();
    }
    private void SlowTime()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isTimeSlowed = !isTimeSlowed;
            Time.timeScale = 1f;
            if (isTimeSlowed)
            {
                specialEffectSource.clip = slowMotionClips[0];
                specialEffectSource.Play();
                Time.timeScale = slowMomentum;

            }
            else
            {
                specialEffectSource.clip = slowMotionClips[1];
                specialEffectSource.Play();

            }
        }
    }
    private void MakeService()
    {

        float distance = Vector3.Distance(transform.position, servePoint.position);

        if (distance < 25 && canServeaAgain && isPressed)
        {
            InstantiateBall();
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

        GameObject ballInst = Instantiate(ball, servePoint.position, servePoint.rotation);
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
        paddleAudioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb.velocity.magnitude > 1)
        {
            rb.velocity = rb.velocity.normalized * 1;
        }
        PlayPaddleHitSound();
        rb.AddForce(servePoint.forward * 1, ForceMode.Impulse);
 
    }
}
