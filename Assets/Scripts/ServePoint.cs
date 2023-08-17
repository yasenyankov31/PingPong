using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServePoint : MonoBehaviour
{
    private Vector3 startingPosition;

    public Transform playerTransform;


    void Start()
    {
        startingPosition=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, startingPosition.y, playerTransform.position.z+10f);

    }
}
