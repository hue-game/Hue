using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Flip))]
public class Move : MonoBehaviour {

    public float runSpeed;

    private bool _moveLeft;
    private bool _moveRight;
    private string _lastDirection;
    private Rigidbody2D _rb;
    private Flip _flipScript;

	Animator anim;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _flipScript = GetComponent<Flip>();

		anim = GetComponent<Animator> ();
	}
	
    public void MoveLeft()
    {
        if (!_moveLeft)
            _lastDirection = "left";
        _moveLeft = !_moveLeft;
    }

    public void MoveRight()
    {
        if (!_moveRight)
            _lastDirection = "right";
        _moveRight = !_moveRight;
    }

    public void MoveCharacter()
    {
        float move = 0.0f;
        if (_moveLeft && !_moveRight)
            move = -1.0f;
        else if (!_moveRight && !_moveLeft)
            move = 0.0f;
        else if (!_moveLeft && _moveRight)
            move = 1.0f;
        else if (_moveLeft && _moveRight && (_lastDirection == "left"))
            move = -1.0f;
        else if (_moveLeft && _moveRight && (_lastDirection == "right"))
            move = 1.0f;

        //Apply the velocity of the player
        _rb.velocity = new Vector2(move * runSpeed, _rb.velocity.y);

        //Call flip script to flip the character's sprite
        if (move > 0)
            _flipScript.FlipSprite(false);
        else if (move < 0)
            _flipScript.FlipSprite(true);
			
		anim.SetFloat ("Movement", Mathf.Abs (Input.GetAxis ("Horizontal")));
    }
}
