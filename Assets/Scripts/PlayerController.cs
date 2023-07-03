using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform paddleModel;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float movment_smoothing = 0.1f;
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] float zPosition = 18f;
    private Vector3 targetPosition;
    private Vector3 movement_velocity = Vector2.zero;
    public float rotationSpeed = 20f;
    public float smoothing = 0.1f;


    float xMiddle;
    void Start()
    {
        xMiddle = (minX + maxX) / 2;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void FixedUpdate()
    {
        Movement();

    }
    private void Movement()
    {
        // Get the mouse movement input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the target position based on the input
        Vector3 inputMovement = new Vector3(mouseX, mouseY, 0f);
        targetPosition += inputMovement * moveSpeed;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        targetPosition.z = zPosition;
        zPosition += Input.GetAxisRaw("Vertical");
        Debug.Log(targetPosition.x+" "+xMiddle);
        if (targetPosition.x > xMiddle)
        {
            RotatePaddle(100);
        }
        else
        {
            RotatePaddle(-90);
        }

        // Smoothly move the object towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref movement_velocity, movment_smoothing);
    }

    private void RotatePaddle(float yAngle)
    {
        float smoothRotation = Mathf.LerpAngle(paddleModel.eulerAngles.y, yAngle, smoothing * Time.deltaTime);
        paddleModel.rotation = Quaternion.Euler(-54.8f, smoothRotation, 90f);
    }
}
