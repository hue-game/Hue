using System;
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

    void FixedUpdate()
    {
        if (_inAir && _groundedLeft && _groundedRight)
        {
            JumpAnimation.ResetTrigger("Climbing");
            JumpAnimation.ResetTrigger("Jump");
            JumpAnimation.SetTrigger("Land");
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
                StartCoroutine(SmoothJump(1));
            }
            else if (_inputManager.movementX < -0.5f && _groundedRight && !_groundedLeft)
            {
                StartCoroutine(SmoothJump(-1));
            }
        }
    }

    public bool GetGrounded(bool leftFoot)
    {
        if (leftFoot)
            return _groundedLeft;
        else
            return _groundedRight;
    }

    public void SetGrounded(bool grounded, bool leftFoot)
    {
        if (leftFoot)
            _groundedLeft = grounded;
        else
            _groundedRight = grounded;
    }

    IEnumerator SmoothJump(int direction)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _groundedLeft = false;
        _groundedRight = false;
        JumpAnimation.ResetTrigger("Land");
        JumpAnimation.SetTrigger("Jump");
        _inAir = true;

        float t = 0;

        _rb.velocity = new Vector2(0, 0);
        _rb.AddForce(new Vector2(jumpX * 0.2f * direction, jumpY * 0.6f), ForceMode2D.Impulse);
        
        while (t < 1)
        {
            t += Time.fixedDeltaTime * (Time.timeScale / 1f);
            if (t < 0.08f)
            {
                _rb.AddForce(new Vector2(jumpX * 0.04f * direction * (1 - t), jumpY * 0.09f * (1 - t)), ForceMode2D.Impulse);
                _rb.AddForce(new Vector2(jumpX * 1 * direction * (1 - t), jumpY * 1.4f * (1 - t)), ForceMode2D.Force);
            }
            else if (t < 0.2f)
                _rb.AddForce(new Vector2(jumpX * 4 * direction * (1 - t), jumpY * 9 * (1 - t)), ForceMode2D.Force);
  
            else if (t < 0.4f)
                _rb.AddForce(new Vector2(jumpX * 1 * -direction * (1 - t), 0), ForceMode2D.Force);
  
            if (_rb.velocity.x > 6.5f)
                _rb.velocity = new Vector2(6.5f, _rb.velocity.y);

            yield return new WaitForFixedUpdate();
        }    
    }
}
