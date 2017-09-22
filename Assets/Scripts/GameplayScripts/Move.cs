using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Flip))]
public class Move : MonoBehaviour {

    public float runSpeed = 20.0f;
    public float ropeSwingSpeed = 60.0f;
    public float ropeSwingDownwardForce = 20.0f;

    private Rigidbody2D _rb;
    private Flip _flipScript;
    private IPlayer _player;
    private Jump _jump;

	Animator WalkAnimation;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _flipScript = GetComponent<Flip>();
        _player = GetComponent<IPlayer>();
        _jump = GetComponent<Jump>();

		WalkAnimation = GetComponent<Animator> ();
	}
	
    public void MoveAnalog(float moveX, float moveY)
    {
        WalkAnimation.SetFloat("XMovement", Mathf.Abs(moveX));

        //Apply the velocity of the player
        if (_player.onRope == null)
        {
            if (_player.onLadder)
            {
                //ClimbAnimation.SetFloat("YMovement", Mathf.Abs(moveY));
                //Move player if on a ladder (allow horizontal movement)
                if (moveX == 0 && moveY == 0)
                {
                    _rb.velocity = new Vector2(0, 0);
                    _rb.isKinematic = true;
                }
                else
                {
                    _rb.velocity = new Vector2(moveX * runSpeed / 2f, moveY * runSpeed);
                    _rb.isKinematic = false;
                }
            }
            else
            {
                //Move player if not jumping in the air
                if (!_jump._inAir)
                    _rb.velocity = new Vector2(moveX * runSpeed, _rb.velocity.y);
            }
        }
   
        //Call flip script to flip the character's sprite
        if (moveX > 0)
            _flipScript.FlipSprite(false);
        else if (moveX < 0)
            _flipScript.FlipSprite(true);
    }
}
