using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public VirtualJoystick joystick;
    [HideInInspector]
    public float movementX;
    [HideInInspector]
    public float movementY;
    [HideInInspector]
    public bool jump;
    [HideInInspector]
    public bool Switch;

    private float _movementXABS;
    private float _movementYABS;
    private Move _moveScript;
    private Jump _jumpScript;
    private WorldManager _worldManager;

	// Use this for initialization
	void Start () {
	}
	
    //Check for key inputs every frame
    private void Update()
    {
        _movementXABS = joystick.InputDirection.x;
        _movementYABS = joystick.InputDirection.y;

        #if UNITY_STANDALONE || UNITY_EDITOR
                if (_movementXABS == 0)
                    _movementXABS = Input.GetAxisRaw("Horizontal");
        #endif

        movementX = _movementXABS;

        jump = Input.GetButton("Jump");
        Switch = Input.GetKeyDown(KeyCode.LeftControl);
    }

}
