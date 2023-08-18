using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private BallTrajectory ballTrajectory;
    private PlayerActions playerActions;
    private PauseMenu pauseMenu;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        ballTrajectory = GetComponent<BallTrajectory>();
        playerActions= GetComponent<PlayerActions>();
        pauseMenu=FindAnyObjectByType<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu.isOpenedMenu)
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

}
