using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 1f;
    public float yAngle, xAngle;
    public Vector2 forcePower = new(1.3f, 1.5f);
    public Vector2 xAngles = new(27f, 18f);

    public float rotationSpeed = 20f;
    public float rotation_smoothing = 0.1f;
    public float moveSpeed = 500f;
    public float yPosition;

    private Vector3 targetPosition;
    public float paddleXDisctance, paddleZDisctance;
    public float paddleZStartingPos;
    private float xRotation;
    private float minX, maxX;
    public float minZ,maxZ;
    private PlayerInput playerInput;
    private PlayerActions playerActions;
    private float lastFrameTime;


    void Start()
    {
        lastFrameTime = Time.realtimeSinceStartup;
        playerInput = GetComponent<PlayerInput>();
        playerActions = GetComponent<PlayerActions>();
        minX = transform.position.x - paddleXDisctance;
        maxX = transform.position.x + paddleXDisctance;

        minZ = transform.position.z - paddleZDisctance / 4;
        maxZ = transform.position.z;
        paddleZStartingPos = maxZ;
    }

    public void Movement()
    {
        // Compute the custom delta time
        float customDeltaTime = Time.realtimeSinceStartup - lastFrameTime;

        // Using customDeltaTime in place of Time.deltaTime
        Vector3 inputMovement = new Vector3(playerInput.MouseX, 0, playerInput.MouseY);
        targetPosition += inputMovement * moveSpeed * customDeltaTime;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = yPosition;
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        xRotation = Remap(targetPosition.x, minX, maxX, -45f, 45f);
        RotatePaddle(-xRotation);

        xAngle = Remap(targetPosition.z, minZ, maxZ, xAngles.x, xAngles.y);

        playerActions.servicePower = Remap(targetPosition.z, minZ, maxZ, forcePower.x, forcePower.y);
        playerActions.servePoint.rotation = Quaternion.Euler(xAngle, yAngle, 0);

        transform.position = targetPosition;

        // Update the lastFrameTime for the next frame
        lastFrameTime = Time.realtimeSinceStartup;
    }

    private void RotatePaddle(float zAngle)
    {
        //zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, zAngle, ref r, 0.1f);
        transform.rotation = Quaternion.Euler(0, yAngle, zAngle);
    }

    public float Remap(float value, float originalMin, float originalMax, float newMin, float newMax)
    {
        float clampedValue = Mathf.Clamp(value, originalMin, originalMax);
        float normalizedValue = Mathf.InverseLerp(originalMin, originalMax, clampedValue);
        float remappedValue = Mathf.Lerp(newMin, newMax, normalizedValue);

        return remappedValue;
    }


}
