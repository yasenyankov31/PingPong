using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class HitLogic : MonoBehaviour
{
    public GameObject ball;
    public PlayerController playerController;
    private void Start()
    {
        playerController=GetComponentInParent<PlayerController>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        //collider.GetComponent<BallScript>().reverseDirection*=-1;
    }

}
