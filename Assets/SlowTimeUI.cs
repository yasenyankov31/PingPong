using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeUI : MonoBehaviour
{

    public Camera mainCamera;

    public RectTransform topBar;
    public RectTransform bottomBar;


    [Tooltip("Speed at which the panel moves")]
    public float moveSpeed = 2.0f;

    [Tooltip("Speed at which the panel moves")]
    public float fieldOfViewSpeed = 2.0f;

    [Tooltip("Ending Y position of the panel")]
    public float endYPositionTop;

    [Tooltip("Ending Y position of the bottom panel")]
    public float endYPositionBottom;

    private Vector2 startPositionBottom, endPositionBottom;
    private Vector2 startPositionTop, endPositionTop;


    private bool shouldMoveToEnd, shouldMoveToStart;
    private float startFieldOfView;
    public float endFieldOfView;

    private void Start()
    {
        startPositionTop = new Vector2(topBar.anchoredPosition.x, topBar.anchoredPosition.y);
        endPositionTop = new Vector2(topBar.anchoredPosition.x, endYPositionTop);

        startPositionBottom = new Vector2(bottomBar.anchoredPosition.x, bottomBar.anchoredPosition.y);
        endPositionBottom = new Vector2(bottomBar.anchoredPosition.x, endYPositionBottom);
        startFieldOfView = mainCamera.fieldOfView;

    }

    private void Update()
    {
        if (shouldMoveToEnd)
        {
            if (mainCamera.fieldOfView< endFieldOfView) {
                mainCamera.fieldOfView += fieldOfViewSpeed * Time.deltaTime;
            }
            MovePanel(endPositionTop, topBar, ref shouldMoveToEnd);
            MovePanel(endPositionBottom, bottomBar, ref shouldMoveToEnd);
        }
        else if (shouldMoveToStart)
        {
            if (mainCamera.fieldOfView > startFieldOfView)
            {
                mainCamera.fieldOfView -= fieldOfViewSpeed * Time.deltaTime;
            }
            MovePanel(startPositionTop, topBar, ref shouldMoveToStart);
            MovePanel(startPositionBottom, bottomBar, ref shouldMoveToStart);
        }
    }

    public void StartMovingPanelToEnd()
    {
        shouldMoveToEnd = true;
        shouldMoveToStart = false; 
    }

    public void StartMovingPanelToStart()
    {
        shouldMoveToStart = true;
        shouldMoveToEnd = false; 
    }

    private void MovePanel(Vector2 targetPosition,RectTransform rectTransform, ref bool movementFlag)
    {
        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
        {
            movementFlag = false;
            rectTransform.anchoredPosition = targetPosition;
            return;
        }

        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
    }
}
