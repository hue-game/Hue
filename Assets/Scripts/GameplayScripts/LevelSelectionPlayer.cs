using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionPlayer : IPlayer
{
    private LevelLoader _levelLoader;

    private void Awake()
	{
		_moveScript = GetComponent<Move>();
        _jumpScript = GetComponent<Jump>();
        _input = GetComponent<InputManager>();
        _levelLoader = FindObjectOfType<LevelLoader>();
        onRope = null;
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
            _levelLoader.PlayerInteract();
    }
}




