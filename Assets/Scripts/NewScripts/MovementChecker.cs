using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementChecker : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Vector3 lastSecondPosition;
    private float timePassed = 0f;

    public float movementTolerance = 0.01f;

    private void Start()
    {
        lastSecondPosition = transform.position;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        timePassed += Time.deltaTime;


        if (timePassed >= 1f)
        {
            float distanceMoved = Vector3.Distance(lastSecondPosition, transform.position);
            float horizontalMovement = transform.position.x - lastSecondPosition.x;

            if (Mathf.Abs(horizontalMovement) > movementTolerance)
            {
                float direction = playerMovement.Remap(distanceMoved, 0, 60, 0, 25);
                if (horizontalMovement > 0)
                {
                    playerMovement.yAngle=direction;
                    Debug.Log("Moved Right in the last second.Distance:" + distanceMoved);
                }
                else
                {
                    playerMovement.yAngle = -direction;
                    Debug.Log("Moved Left in the last second.Distance:" + distanceMoved);
                }
            }
            else
            {
                playerMovement.yAngle = 0;
                Debug.Log("Moved Forward in the last second.Distance:" + distanceMoved);
            }



            timePassed = 0f;
            lastSecondPosition = transform.position;
        }
    }
}
