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
    public float servePower = 25f;
    public float yAngle, xAngle;
    public float rotationSpeed = 20f;
    public float rotation_smoothing = 0.1f;
    public float yPosition;

    private Vector3 targetPosition;
    private Vector3 movement_velocity = Vector2.zero;
    public float paddleXDisctance, paddleZDisctance;
    public float paddleZStartingPos;
    [SerializeField] private float movment_smoothing = 0.1f;
    [SerializeField] private float moveSpeed = 500f;
    private float xRotation,r;
    private float minX, minZ, maxX;
    public float maxZ;
    private PlayerInput input;
    private PlayerActions actions;
    

    void Start()
    {
        input = GetComponent<PlayerInput>();
        actions=GetComponent<PlayerActions>();
        minX = transform.position.x - paddleXDisctance;
        maxX = transform.position.x + paddleXDisctance;

        minZ = transform.position.z - paddleZDisctance / 4;
        maxZ = transform.position.z;
        paddleZStartingPos = maxZ;
    }

    public void Movement()
    {

        //yAngle = 15 * input.horizontalInput;

        Vector3 inputMovement = new Vector3(input.MouseX, 0, input.MouseY);
        targetPosition += inputMovement * moveSpeed * Time.deltaTime;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = yPosition;
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        xRotation = Remap(targetPosition.x, minX, maxX, -45f, 45f);
        RotatePaddle(-xRotation);

        actions.servePoint.rotation = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref movement_velocity, movment_smoothing);
    }

    private void RotatePaddle(float zAngle)
    {
        zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, zAngle, ref r, 0.1f);


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
