using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipSliderAnimation : MonoBehaviour
{
    public RectTransform canvasRectTransform;
    public float slideSpeed = 200.0f; // Adjust this value to control the speed of the slide.
    public float endXPosition = 200.0f; // Adjust this value to set the ending X position.

    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isSliding = false;

    private void Start()
    {
        startPosition = canvasRectTransform.anchoredPosition;
        endPosition = new Vector2(endXPosition, canvasRectTransform.anchoredPosition.y);
    }

    private void Update()
    {
        if (isSliding)
        {
            Vector2 newPosition = Vector2.MoveTowards(canvasRectTransform.anchoredPosition, endPosition, slideSpeed * Time.realtimeSinceStartup);

            canvasRectTransform.anchoredPosition = newPosition;
            if (Vector2.Distance(canvasRectTransform.anchoredPosition, endPosition) < 0.1f)
            {

                isSliding = false;
                canvasRectTransform.anchoredPosition = startPosition;
            }
        }
    }

    public void SlideCanvas()
    {
        isSliding = true;
    }

}
