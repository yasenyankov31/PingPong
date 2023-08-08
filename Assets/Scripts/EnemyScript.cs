using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform servePoint;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        
        if (FindAnyObjectByType<BallScript>() != null) {
            Transform ballTransform = FindAnyObjectByType<BallScript>().transform;
            transform.position = new Vector3(ballTransform.position.x, transform.position.y, transform.position.z);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb.velocity.magnitude > 1)
        {
            rb.velocity = rb.velocity.normalized * 1;
        }
        audioSource.Play();
        rb.AddForce(servePoint.forward * 1, ForceMode.Impulse);
    }
}
