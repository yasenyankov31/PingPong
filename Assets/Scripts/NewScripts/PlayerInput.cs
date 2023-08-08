using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MouseX, MouseY, horizontalInput;


    public void SetInput() {
        Cursor.lockState = CursorLockMode.Locked;

        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");

        horizontalInput = Input.GetAxisRaw("Horizontal");

    }
}
