using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSphere : MonoBehaviour
{
    public float targetSize = 505f;
    public float duration = 1f; 
    public bool isScaledDown;


    public void StartScaleUp()
    {
        StartCoroutine(ScaleUp());
    }
    public void StartScaleDown()
    {
        StartCoroutine(ScaleDown());
    }

    IEnumerator ScaleUp()
    {
        Vector3 originalScale = Vector3.zero;
        Vector3 targetScale = new Vector3(targetSize, targetSize, targetSize);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    IEnumerator ScaleDown()
    {
        Vector3 originalScale = new Vector3(targetSize, targetSize, targetSize);
        Vector3 targetScale = Vector3.zero;
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            yield return null;
        }
        isScaledDown = true;
        transform.localScale = targetScale;
    }
}
