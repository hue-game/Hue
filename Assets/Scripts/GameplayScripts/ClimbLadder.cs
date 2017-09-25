using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour {
	public float climbSpeed = 2.0f;
	private float climbVelocity;
	private Player _player;

	// Use this for initialization
	void Start () {
		_player = FindObjectOfType<Player>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		InputManager inputManager = _player.GetComponent<InputManager> ();
		Rigidbody2D rigidBody = _player.GetComponent<Rigidbody2D> ();

		float moveY = inputManager.movementY;
		float velocityY = moveY * climbSpeed;
			
		if (_player.onLadder && rigidBody != null) {
			rigidBody.gravityScale = 0f;
		}
		else {
			rigidBody.gravityScale = _player.InitialGravity;
		}
	}
}
