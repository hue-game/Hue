using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private Jump _jumpScript;

	Animator JumpAnimation;

	// Use this for initialization
	void Start () {
        _jumpScript = transform.parent.GetComponent<Jump>();	

		JumpAnimation = GetComponent<Animator>(); 
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        _jumpScript.SetGrounded(true);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        _jumpScript.SetGrounded(true);
    }
    private void OnTriggerLeave2D(Collider2D other)
    {
        _jumpScript.SetGrounded(false);
    }

}
