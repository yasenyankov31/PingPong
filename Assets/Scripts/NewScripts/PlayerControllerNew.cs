using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNew : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private BallTrajectory ballTrajectory;
    private PlayerActions playerActions;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        ballTrajectory = GetComponent<BallTrajectory>();
        playerActions= GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.SetInput();
        playerMovement.Movement();
        ballTrajectory.trajectoryLine.enabled = playerActions.canServeaAgain;
        if (playerActions.canServeaAgain)
        {
            ballTrajectory.SimulateTrajectory();
        }

        playerActions.Actions();
    }

}
