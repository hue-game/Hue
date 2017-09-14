using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour {

    public float jumpHeight;
    public float jumpX;
    public float jumpY;

    private bool _groundedLeft;
    private bool _groundedRight;
    //[HideInInspector]
    public bool _inAir;
    private Rigidbody2D _rb;
    private InputManager _inputManager;

	Animator JumpAnimation;

	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();
		JumpAnimation = GetComponent<Animator> ();

	}

    void LateUpdate()
    {
        if (_inAir && _groundedLeft && _groundedRight)
        {
            JumpAnimation.SetTrigger("Land");
            JumpAnimation.ResetTrigger("Jump");
            _inAir = false;
        }
        LedgeJump();
    }

    //Default Jump Method: Uses jumpHeight from player to determine the jump strength
    public void JumpUp()
    {
        if (_groundedLeft || _groundedRight)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0); //Reset velocity so you can keep bouncing on the bounce pads
            _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            _groundedLeft = false;
            _groundedRight = false;
        }
        JumpAnimation.SetTrigger ("Jump");
        _inAir = true;

    }

    //Jump Method for Bounce Pads: Uses strength from the respective bounce pad to determine the jump strength
    public void JumpUp(float strength)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0); //Reset velocity so you can keep bouncing on the bounce pads
        _rb.AddForce(new Vector2(0, strength), ForceMode2D.Impulse);

		JumpAnimation.SetTrigger ("Jump");
        _groundedLeft = false;
        _groundedRight = false;
        _inAir = true;

    }

    public void LedgeJump()
    {
        if(!_inAir)
        {
            if (_inputManager.movementX > 0.5f && !_groundedRight && _groundedLeft)
            {
                _rb.AddForce(new Vector2(jumpX, jumpY), ForceMode2D.Impulse);
                JumpAnimation.SetTrigger("Jump");
                _groundedLeft = false;
                _groundedRight = false;
                _inAir = true;
            }
            else if (_inputManager.movementX < -0.5f && _groundedRight && !_groundedLeft)
            {
                _rb.AddForce(new Vector2(-jumpX, jumpY), ForceMode2D.Impulse);
                JumpAnimation.SetTrigger("Jump");
                _groundedLeft = false;
                _groundedRight = false;
                _inAir = true;
            }
        }
    }

    public void SetGrounded(bool grounded, bool leftFoot)
    {
        if (leftFoot)
            _groundedLeft = grounded;
        else
            _groundedRight = grounded;
    }

}
