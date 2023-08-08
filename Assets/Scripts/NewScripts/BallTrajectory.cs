using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallTrajectory : MonoBehaviour
{
    

    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _tableTransform;
    [SerializeField] private GameObject trajectory_Ball;


    public LineRenderer trajectoryLine;
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private GameObject ghostBall;
    private PlayerActions playerActions;
    private Transform servePoint;

    void Start()
    {
        trajectoryLine=GetComponent<LineRenderer>();
        playerActions=GetComponent<PlayerActions>();
        servePoint = playerActions.servePoint;
        CreatePhysicsScene();
    }

    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        var ghostObj = Instantiate(_tableTransform.gameObject, _tableTransform.position, _tableTransform.rotation);
        ghostObj.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
    }

    public void SimulateTrajectory()
    {
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
        rigidbodyBall.AddForce(playerActions.servicePower * servePoint.forward, ForceMode.Impulse);

        trajectoryLine.positionCount = _maxPhysicsFrameIterations;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            trajectoryLine.SetPosition(i, ghostBall.transform.position);
        }
        ghostBall.transform.SetPositionAndRotation(servePoint.position, servePoint.rotation);
        rigidbodyBall.velocity = Vector3.zero;
    }

}
