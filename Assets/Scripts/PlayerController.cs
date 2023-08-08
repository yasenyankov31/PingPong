using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using static UnityEditor.Profiling.FrameDataView;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


    private PaddleCollision paddleCollision;

    [Header("Move logic")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float movment_smoothing = 0.1f;
    [SerializeField] float paddleXDisctance, paddleZDisctance;
    [SerializeField] float yPosition = 34f;
    private float minX, minZ, maxX, maxZ,startingMaxZ;
    public bool canServeaAgain=true;
    float MouseX, MouseY;

    [Header("Serve logic")]
    private float horizontalInput, verticalInput;
    public Transform servePoint;
    public float rotationOfService;
    public float servePower = 25f;
    public float yAngle, xAngle;

    [Header("Rotate logic")]
    [SerializeField] float rotaionValue;
    private Vector3 targetPosition;
    private Vector3 movement_velocity = Vector2.zero;
    public float rotationSpeed = 20f;
    public float rotation_smoothing = 0.1f;
    private float xRotation;

    [Header("Hit logic")]
    public bool isPressed = false;
    float r;


    [Header("Game prefabs")]
    public GameObject ball;
    public GameObject trajectory_Ball;

    [Header("Special mechaincs ")]
    private bool isTimeSlowed;
    public float slowMomentum;

    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _tableTransform;


    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private GameObject ghostBall = null;

    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        var ghostObj = Instantiate(_tableTransform.gameObject, _tableTransform.position, _tableTransform.rotation);
        ghostObj.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
    }

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        minX = transform.position.x - paddleXDisctance;
        maxX = transform.position.x + paddleXDisctance;

        minZ = transform.position.z - paddleZDisctance / 4;
        maxZ = transform.position.z;
        startingMaxZ = maxZ;
        paddleCollision=GetComponentInChildren<PaddleCollision>();

        CreatePhysicsScene();

    }


    private void Update()
    {
        SimulateTrajectory();
        ServiceBall();
        Movement();
        SlowMotion();

    }

    private void SlowMotion()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isTimeSlowed = !isTimeSlowed;
            Time.timeScale = 1f;
            if (isTimeSlowed)
            {
                Time.timeScale = slowMomentum;

            }
        }


    }

    private void Movement()
    {
        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        yAngle = 15 * horizontalInput;
        if (verticalInput != 0 && xAngle < 45 && xAngle > -45)
        {
            xAngle += verticalInput * 100 * Time.deltaTime;
        }


        // Calculate the target position based on the input
        Vector3 inputMovement = new Vector3(MouseX, 0, MouseY);
        targetPosition += inputMovement * moveSpeed * Time.deltaTime;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = yPosition;
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        xRotation = Remap(targetPosition.x, minX, maxX, -45f, 45f);
        RotatePaddle(-xRotation);


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


    private void ServiceBall()
    {

        float distance = Vector3.Distance(transform.position, servePoint.position);
        _line.enabled = canServeaAgain;

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
        maxZ = transform.position.z + paddleZDisctance;

        servePoint.GetComponentInChildren<MeshRenderer>().enabled = false;
        servePoint.rotation = Quaternion.Euler(xAngle, yAngle, 0);
        GameObject ballInst = Instantiate(ball, servePoint.position, servePoint.rotation);
        Rigidbody rigidbodyBall = ballInst.GetComponent<Rigidbody>();
        paddleCollision.PlayPaddleSound();
        rigidbodyBall.AddForce( servePower * servePoint.forward, ForceMode.Impulse);

    }

    public void SimulateTrajectory()
    {
        servePoint.rotation = Quaternion.Euler(xAngle, yAngle, 0);
        if (ghostBall == null)
        {
            ghostBall = Instantiate(trajectory_Ball, servePoint.position, servePoint.rotation);
            SceneManager.MoveGameObjectToScene(ghostBall, _simulationScene);
            ghostBall.GetComponent<MeshRenderer>().enabled = false;

        }
        else
        {
            ghostBall.transform.SetPositionAndRotation(servePoint.position, servePoint.rotation);
        }

        Rigidbody rigidbodyBall = ghostBall.GetComponent<Rigidbody>();
        rigidbodyBall.velocity = Vector3.zero;
        rigidbodyBall.AddForce(servePower * servePoint.forward, ForceMode.Impulse);

        _line.positionCount = _maxPhysicsFrameIterations;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostBall.transform.position);
        }

        ghostBall.transform.SetPositionAndRotation(servePoint.position, servePoint.rotation);
        rigidbodyBall.velocity = Vector3.zero;

    }

    public void ResetServiceVariables()
    {
        Vector3 resetPosition = transform.position; 
        resetPosition.z = minZ;
        transform.position = resetPosition;

        maxZ = startingMaxZ;
        canServeaAgain = true;
        servePoint.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

}
