using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemy : MonoBehaviour {

    public float alertRadius = 5;
    public float attackCooldown = 1f;
    public float idleSpeed = 1.5f;
    public float attackSpeed = 3.0f;
    public Collider2D roamingArea;

    //[HideInInspector]
    public string state = "idle";

    protected Animator _animator;
    protected Rigidbody2D _rb;
    protected Flip _flipScript;
    protected Transform _playerTransform;
    protected WorldManager _worldManager;
    protected float _timeAlerted = 0;
    protected Vector2 _moveDirection = Vector2.right;
    //Enable this if you want to randomly switch direction next frame (in idle)
    protected bool _changeDirectionNextUpdate = false;
    protected float _lastDirectionSwitch = 0;

    private IEnemy[] _enemies;
    private bool playerIsDead;

    // Use this for initialization
    public void Awake () {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _flipScript = GetComponent<Flip>();
        _playerTransform = FindObjectOfType<IPlayer>().transform;
        _worldManager = FindObjectOfType<WorldManager>();
        _enemies = FindObjectsOfType<IEnemy>();

        foreach (IEnemy enemy in _enemies)
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}


    public void Lost()
    {
        SetState("idle");
        //animator.ResetTrigger("Attack");
        //animator.SetTrigger("Lost");
    }

    public void Found()
    {
        SetState("attack");
        _moveDirection = Vector2.zero;
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _flipScript.FlipSprite(!alertDirection);
        _timeAlerted = Time.time;

        //Play alert sound
        //animator.ResetTrigger("Idle");
        //animator.SetTrigger("Found");
    }

    //Set this from animator or script
    public void SetState(string newState)
    {
        state = newState;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Danger")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player")
            playerIsDead = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.gameObject.tag == "Danger")
        {
            Destroy(gameObject);
        }
    }

    public abstract void Idle();

    public abstract void Attack();

    public abstract bool ObstacleCheck();

    public abstract bool OutOfRangeCheck();

    public abstract bool EdgeCheck();

}
