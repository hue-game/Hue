using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    [HideInInspector]
    public float movementX;
    [HideInInspector]
    public float movementY;
    [HideInInspector]
    public bool jump;
    [HideInInspector]
    public bool Switch;

    private VirtualJoystick joystick;

	// Use this for initialization
	void Start () {
        joystick = FindObjectOfType<VirtualJoystick>();
	}
	
    //Check for key inputs every frame
    private void Update()
    {
        movementX = joystick.InputDirection.x;
        movementY = joystick.InputDirection.y;

        #if UNITY_STANDALONE || UNITY_EDITOR
        if (movementX == 0)
            movementX = Input.GetAxisRaw("Horizontal");
        #endif

        jump = Input.GetButton("Jump");
        Switch = Input.GetKeyDown(KeyCode.LeftControl);
    }

}
