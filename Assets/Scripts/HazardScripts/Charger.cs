using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : IEnemy {

    public float changeDirectionMin = 1f;
    public float changeDirectionMax = 5f;
    [Header("left is false, right is true")]
    public bool startDirection = false;

    private float _lastDirectionSwitch = 0;
    private bool _isSliding = false;

    new void Awake () {
        base.Awake();
        if (startDirection)
            _moveDirection = new Vector2(1, 0f);
        else
            _moveDirection = new Vector2(-1, 0f);

        _oldMoveDirection = _moveDirection;
        _rb.velocity = _moveDirection;
    }

    void Update () {
        if (_player.isDead)
            SetState("idle");

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
        else
            _moveDirection = _oldMoveDirection;

        _rb.velocity = new Vector2(_moveDirection.x * idleSpeed, _rb.velocity.y);
        
        if (!_player.isDead)
        {
            float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer < alertRadius && !_isSliding)
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

        if (!_isSliding)
            _rb.velocity = new Vector2(_moveDirection.x * attackSpeed, _rb.velocity.y);

        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer > alertRadius && !_isSliding)
            StartCoroutine(Lost());
    }

    public override IEnumerator Lost()
    {
        SetState("lost");
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);

        _alertAnimator.SetBool("Lost", true);
        _alertAnimator.SetBool("Found", false);

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Lost", false);
        _animator.speed = 1.0f;
        SetState("idle");
    }

    public override IEnumerator Found()
    {
        SetState("found");
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);

        _alertAnimator.SetBool("Lost", false);
        _alertAnimator.SetBool("Found", true);
        FoundLookDirection();

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Found", false);
        SetState("attack");
    }

    public override void FoundLookDirection()
    {
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _flipScript.FlipSprite(!alertDirection);
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

    private Vector2 RandomDirection()
    {

        return Vector2.zero;
    }
    
    private IEnumerator Slide()
    {
        _isSliding = true;
        _animator.SetBool("Sliding", true);
        Vector2 startVelocity = _rb.velocity;
        float t = 0;
        while (t < 1)
        {
            Vector2 endVelocity = new Vector2(0, _rb.velocity.y);
            t += Time.fixedDeltaTime * (Time.timeScale / 2f);

            _rb.velocity = Vector2.Lerp(startVelocity, endVelocity, t);
            yield return 0;
        }

        _isSliding = false;
        _animator.SetBool("Sliding", false);
    }

}
