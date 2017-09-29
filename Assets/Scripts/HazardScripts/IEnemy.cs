using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemy : MonoBehaviour {

    public float alertRadius = 5;
    public float alertDuration = 0.2f;
    public float idleSpeed = 1.5f;
    public float attackSpeed = 3.0f;
    public Collider2D roamingArea;

    //[HideInInspector]
    public string state = "idle";

    protected Animator _animator;
    protected Animator _alertAnimator;
    protected Rigidbody2D _rb;
    protected Flip _flipScript;
    protected Transform _playerTransform;
    protected IPlayer _player;
    protected WorldManager _worldManager;
    protected Vector2 _moveDirection = Vector2.right;
    protected Vector2 _oldMoveDirection;

    private IEnemy[] _enemies;
    private bool playerIsDead;

    // Use this for initialization
    public void Awake () {
        _animator = GetComponent<Animator>();
        _alertAnimator = transform.GetChild(0).GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _flipScript = GetComponent<Flip>();
        _player = FindObjectOfType<IPlayer>();
        _playerTransform = _player.transform;
        _worldManager = FindObjectOfType<WorldManager>();
        _enemies = FindObjectsOfType<IEnemy>();

        foreach (IEnemy enemy in _enemies)
            Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}

    public abstract void Idle();

    public abstract void Attack();

    public abstract IEnumerator Lost();

    public abstract IEnumerator Found();

    public void SetState(string newState)
    {
        state = newState;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Danger")
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Danger")
            Destroy(gameObject);
    }

    public bool OutOfRangeCheck()
    {
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), roamingArea.GetComponent<BoxCollider2D>()))
            return false;
        else
            return true;
    }

    public abstract bool EdgeCheck();

    public abstract void FoundLookDirection();

    public abstract bool ObstacleCheck();

}
