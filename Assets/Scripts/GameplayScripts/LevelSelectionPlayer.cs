using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionPlayer : IPlayer
{
    private GameManager _gameManager;

    private void Awake()
	{
		_moveScript = GetComponent<Move>();
        _input = GetComponent<InputManager>();
        _gameManager = FindObjectOfType<GameManager>();
        onRope = null;
        _jumpScript = GetComponent<Jump>();
        _jumpScript._inAir = false;
        _jumpScript.SetGrounded(true, true);
        _jumpScript.SetGrounded(true, false);
	}

    // FixedUpdate is called once per frame after physics have applied
    private void FixedUpdate()
    {
        _moveScript.MoveAnalog(_input.movementX, _input.movementY);
    }

    //Check for key inputs every frame
    private void Update()
    {
        if (_input.Switch)
        {
            _gameManager.PlayerInteract();
            _input.Switch = false;
        }
    }
}




