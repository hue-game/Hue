using System;
using System.Collections;
using UnityEngine;

public class Charger : IEnemy {

    public float changeDirectionMin = 1f;
    public float changeDirectionMax = 5f;
    public float slideDuration = 1.75f;
    [Header("left is false, right is true")]
    public bool startDirection = false;

    private bool _isSliding = false;
    private bool _playerInWorld = false;

    private bool _lostToIdle = false;
    private float _nextDirectionSwitch = 0;

    new void Awake () {
        base.Awake();
        if (startDirection)
            _moveDirection = new Vector2(1, 0f);
        else
            _moveDirection = new Vector2(-1, 0f);

        _oldMoveDirection = _moveDirection;
        _rb.velocity = _moveDirection;
        _nextDirectionSwitch = Time.time + UnityEngine.Random.Range(changeDirectionMin, changeDirectionMax);
    }

    void Update () {
        if (_player.isDead)
            SetState("idle");

        _playerInWorld = !_worldManager.worldType;

        switch (state)
        {
            case "idle":
                Idle();
                break;
            case "attack":
                Attack();
                break;
            default:
                _rb.velocity = new Vector2(_moveDirection.x, _rb.velocity.y);
                break;
        }

        if (state != "lost" && state != "found" && !_isSliding)
        {
            if (_moveDirection.x > 0)
                _flipScript.FlipSprite(false);
            else if (_moveDirection.x < 0)
                _flipScript.FlipSprite(true);
        }
    }

    public override void Idle()
    {
        if (OutOfRangeCheck())
        {
            if (roamingArea.bounds.center.x > transform.position.x)
                _moveDirection = Vector2.right;
            else
                _moveDirection = Vector2.left;

            _oldMoveDirection = _moveDirection;
        }
        else if (EdgeCheck())
        {
            _moveDirection *= -1;
            _oldMoveDirection = _moveDirection;
        }
        else if (ObstacleCheck()) 
        {
            _moveDirection *= -1;
            _oldMoveDirection = _moveDirection;
        }
        else if (_lostToIdle)
        {
            _lostToIdle = false;
            if (UnityEngine.Random.Range(0, 2) == 1)
                _moveDirection = new Vector2(1f, 0f);
            else
                _moveDirection = new Vector2(-1f, 0f);
            _oldMoveDirection = _moveDirection;
        }
        else if (Time.time > _nextDirectionSwitch)
        {
            _nextDirectionSwitch = Time.time + UnityEngine.Random.Range(changeDirectionMin, changeDirectionMax);
            float movementChance = 0.66f;
            if (roamingArea.bounds.center.x > transform.position.x)
                movementChance = 0.33f;

            if (UnityEngine.Random.Range(0f, 1f) > movementChance)
                _moveDirection = new Vector2(1f, 0f);
            else
                _moveDirection = new Vector2(-1f, 0f);

            if (UnityEngine.Random.Range(0f, 1f) > 0.8f)
            {
                _moveDirection = new Vector2(0f, 0f);
                _animator.speed = 0.0f;
                _nextDirectionSwitch = Time.time + UnityEngine.Random.Range(0.8f, 2.5f);
            }

            _oldMoveDirection = _moveDirection;
        }
        else
            _moveDirection = _oldMoveDirection;

        if (_moveDirection.x != 0)
            _animator.speed = 1.0f;

        _rb.velocity = new Vector2(_moveDirection.x * idleSpeed, _rb.velocity.y);
        
        if (!_player.isDead && _playerInWorld)
        {
            float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer < alertRadius && Mathf.Abs(_playerTransform.position.y - transform.position.y) < 2f && !_isSliding)
                StartCoroutine(Found());
        }
    }

    public override void Attack()
    {
        if (_playerTransform.position.x > transform.position.x)
            _moveDirection = Vector2.right;
        else
            _moveDirection = Vector2.left;

        _oldMoveDirection = _moveDirection;

        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (((distanceToPlayer > alertRadius) && !_isSliding) || (!_isSliding && !_playerInWorld) || (!_isSliding && Mathf.Abs(_playerTransform.position.y - transform.position.y) > 2f))
            StartCoroutine(Lost());

        if (!_isSliding)
        {
            if (!_playerInWorld)
                StartCoroutine(Slide());
            else
                _rb.velocity = new Vector2(_moveDirection.x * attackSpeed, _rb.velocity.y);
        }
    }

    public override IEnumerator Lost()
    {
        SetState("lost");
        _alertAnimator.SetBool("Lost", true);
        _alertAnimator.SetBool("Found", false);

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Lost", false);
        _animator.speed = 1.0f;
        SetState("idle");
        _lostToIdle = true;
    }

    public override IEnumerator Found()
    {
        SetState("found");
        _animator.speed = 0.0f;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Attack", true);

        _alertAnimator.SetBool("Lost", false);
        _alertAnimator.SetBool("Found", true);
        FoundLookDirection();

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Found", false);
        _animator.speed = 1.0f;
        SetState("attack");
    }

    public override void FoundLookDirection()
    {
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _flipScript.FlipSprite(!alertDirection);
    }

    public override bool OutOfRangeCheck()
    {
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), roamingArea.GetComponent<Collider2D>()))
            return false;
        else
            return true;
    }

    public override bool ObstacleCheck()
    {
        RaycastHit2D[] obstacleHitChecks;
        if (_moveDirection.x > 0)
            obstacleHitChecks = Physics2D.RaycastAll(transform.position, Vector2.right, 0.5f);
        else
            obstacleHitChecks = Physics2D.RaycastAll(transform.position, Vector2.left, 0.5f);

        foreach (RaycastHit2D obstacleHit in obstacleHitChecks)
        {
            if (obstacleHit.transform.GetComponent<IEnemy>() == null && obstacleHit.transform.tag != "Player")
                return true;
        }

        return false;
    }

    private IEnumerator Slide()
    {
        _isSliding = true;
        _animator.SetBool("Attack", false);
        _animator.SetBool("Slide", true);
        Vector2 startVelocity = _rb.velocity;
        float t = 0;
        while (t < 1)
        {
            Vector2 endVelocity = new Vector2(0, _rb.velocity.y);
            t += Time.fixedDeltaTime * (Time.timeScale / slideDuration);

            _rb.velocity = Vector2.Lerp(new Vector2(startVelocity.x, _rb.velocity.y), endVelocity, t);
            yield return null;
        }

        _isSliding = false;
        _animator.SetBool("Slide", false);
        _animator.SetBool("Idle", true);
    }

    public override bool EdgeCheck()
    {
        RaycastHit2D[] edgeHitChecks;

        if (_moveDirection.x > 0)
            edgeHitChecks = Physics2D.RaycastAll(transform.position, Vector2.right + Vector2.down, 1.0f);
        else
            edgeHitChecks = Physics2D.RaycastAll(transform.position, Vector2.left + Vector2.down, 1.0f);

        foreach (RaycastHit2D edgeHit in edgeHitChecks)
        {
            if (edgeHit.transform.GetComponent<IEnemy>() == null && edgeHit.transform.tag != "Player")
                return false;
        }

        return true;
    }

    private void OnDestroy()
    {
        _worldManager.RemoveGameObject(gameObject);
    }
}
