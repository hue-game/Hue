using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    public bool leftFoot;
    private Jump _jumpScript;
    private Collider2D _playerCollider;
    private Collider2D _col;

	// Use this for initialization
	void Start () {
        _col = GetComponent<Collider2D>();
        _jumpScript = transform.parent.GetComponent<Jump>();
        _playerCollider = transform.parent.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(_playerCollider, GetComponent<Collider2D>());
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger && _col.isTrigger && other.GetComponent<RopeSegment>() == null)
        {
            bool groundFound = false;
            foreach(Collider2D colOverlap in Physics2D.OverlapBoxAll(_col.bounds.center, _col.bounds.size, 0f))
            {
                if (colOverlap != other && colOverlap != _col && colOverlap != _playerCollider && !colOverlap.isTrigger && !Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("PlayerFeet"), colOverlap.gameObject.layer))
                    groundFound = true;
            }
            if (!groundFound)
                _jumpScript.SetGrounded(false, leftFoot);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && _col.isTrigger && other.GetComponent<RopeSegment>() == null)
            _jumpScript.SetGrounded(true, leftFoot);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger && _col.isTrigger && other.GetComponent<RopeSegment>() == null)
            _jumpScript.SetGrounded(true, leftFoot);
    }

}
