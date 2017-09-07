using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour {

    public float jumpHeight;

    private bool _grounded;
    private Rigidbody2D _rb;

	void Start () {
        _rb = GetComponent<Rigidbody2D>();

	}

    //Default Jump Method: Uses jumpHeight from player to determine the jump strength
    public void JumpUp()
    {
        //print(_grounded);
        if (_grounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0); //Reset velocity so you can keep bouncing on the bounce pads
            _rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            _grounded = false;
        }
    }

    //Jump Method for Bounce Pads: Uses strength from the respective bounce pad to determine the jump strength
    public void JumpUp(float strength)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0); //Reset velocity so you can keep bouncing on the bounce pads
        _rb.AddForce(new Vector2(0, strength), ForceMode2D.Impulse);
    }

    public bool GetGrounded()
    {
        return _grounded;
    }

    public void SetGrounded(bool grounded)
    {
        _grounded = grounded;
    }


}
