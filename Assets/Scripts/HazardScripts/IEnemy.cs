using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemy : MonoBehaviour {

    public float alertRadius = 5;
    public float attackCooldown = 1f;

    [HideInInspector]
    public string state;
    protected Animator _animator;
    protected Rigidbody2D _rb;
    protected Flip _flipScript;
    protected Transform _playerTransform;
    protected WorldManager _worldManager;
    protected float timeAlerted = 0;

    // Use this for initialization
    public void Awake () {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _flipScript = GetComponent<Flip>();
        _playerTransform = FindObjectOfType<IPlayer>().transform;
        _worldManager = FindObjectOfType<WorldManager>();
        state = "idle"; 
	}
	
	// Update is called once per frame
	void Update () {
        //float moveDirection = _rb.velocity.x;
        //if (moveDirection > 0)
        //    _flipScript.FlipSprite(false);
        //else if (moveDirection < 0)
        //    _flipScript.FlipSprite(true);
    }

    public abstract void Idle();

    public abstract void Attack();

    public void Lost()
    {
        //animator.ResetTrigger("Attack");
        //animator.SetTrigger("Lost");
    }

    public void Found()
    {
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _flipScript.FlipSprite(alertDirection);
        timeAlerted = Time.time;

        //Play alert sound
        //animator.ResetTrigger("Idle");
        //animator.SetTrigger("Found");
    }

    //Set this from animator or script
    public void SetState(string newState)
    {
        state = newState;
    }

}
