using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : IEnemy {

    public bool followPlayer = true;
    [Header("left is false, right is true")]
    public bool startDirection = false;

    private bool _edgeFound = false;
    private bool _flippedLastFrame = false;

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

        if (state != "lost" && state != "found")
        {
            if (_moveDirection.x > 0)
            {
                _animator.SetBool("WalkLeft", false);
            }
            else if (_moveDirection.x < 0)
            {
                _animator.SetBool("WalkLeft", true);
            }
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
            _flippedLastFrame = false;
        }
        else if (EdgeCheck())
        {
            if (!_flippedLastFrame)
            {
                _moveDirection *= -1;
                _oldMoveDirection = _moveDirection;
                _flippedLastFrame = true;
            }
        }
        else if (ObstacleCheck())
        {
            _moveDirection *= -1;
            _oldMoveDirection = _moveDirection;
            _flippedLastFrame = false;
        }
        else
        {
            _moveDirection = _oldMoveDirection;
            _flippedLastFrame = false;
        }

        _rb.velocity = new Vector2(_moveDirection.x * idleSpeed, _rb.velocity.y);
        
        if (followPlayer && !_player.isDead)
        {
            float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer < alertRadius)
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
        _rb.velocity = new Vector2(_moveDirection.x * attackSpeed, _rb.velocity.y);

        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer > alertRadius)
            StartCoroutine(Lost());
    }

    public override IEnumerator Lost()
    {
        _animator.speed = 0.0f;
        SetState("lost");
        _alertAnimator.SetBool("Lost", true);
        _alertAnimator.SetBool("Found", false);

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Lost", false);
        _animator.speed = 1.0f;
        SetState("idle");
    }

    public override IEnumerator Found()
    {
        _animator.speed = 0.0f;
        SetState("found");
        FoundLookDirection();
        _alertAnimator.SetBool("Lost", false);
        _alertAnimator.SetBool("Found", true);

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Found", false);
        _animator.speed = 1.0f;
        SetState("attack");
    }


    public override void FoundLookDirection()
    {
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _animator.SetBool("WalkLeft", !alertDirection);
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

    public override bool EdgeCheck()
    {
        RaycastHit2D[] edgeHitChecks;

        LayerMask layerMask;
        if (_worldManager.worldType)
            layerMask = ~(1 << 9);
        else
            layerMask = ~(1 << 8);

        if (_moveDirection.x > 0)
            edgeHitChecks = Physics2D.RaycastAll(transform.position, Vector2.right + Vector2.down, 1.0f, layerMask);
        else
            edgeHitChecks = Physics2D.RaycastAll(transform.position, Vector2.left + Vector2.down, 1.0f, layerMask);

        foreach (RaycastHit2D edgeHit in edgeHitChecks)
        {
            if (edgeHit.transform.GetComponent<IEnemy>() == null && edgeHit.transform.tag != "Player")
                return false;
        }

        return true;
    }
}
