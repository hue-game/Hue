using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Flip))]
public class Move : MonoBehaviour {

    public float runSpeed = 20.0f;
    public float ropeSwingSpeed = 60.0f;
    public float ropeSwingDownwardForce = 20.0f;

    private bool _moveLeft;
    private bool _moveRight;
    private string _lastDirection;
    private Rigidbody2D _rb;
    private Flip _flipScript;

	Animator WalkAnimation;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _flipScript = GetComponent<Flip>();

		WalkAnimation = GetComponent<Animator> ();
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

		WalkAnimation.SetFloat ("XMovement", Mathf.Abs (move));

        //Apply the velocity of the player
        if (GetComponent<Player>().onRope == null)
            _rb.velocity = new Vector2(move * runSpeed, _rb.velocity.y);
        else
        {
            Rigidbody2D _ropeRb = GetComponent<Player>().onRope.GetComponent<Rigidbody2D>();
            if (move != 0)
                _ropeRb.AddForce(new Vector2(move * ropeSwingSpeed, -ropeSwingDownwardForce));
        }

        //Call flip script to flip the character's sprite
        if (move > 0)
            _flipScript.FlipSprite(false);
        else if (move < 0)
            _flipScript.FlipSprite(true);			
    }
}
