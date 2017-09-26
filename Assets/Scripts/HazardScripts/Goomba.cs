using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : IEnemy {

    public bool followPlayer = true;
    [Header("left is false, right is true")]
    public bool startDirection = false;

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
        {
            SetState("idle");
        }

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

        yield return new WaitForSeconds(alertDuration);

        _animator.speed = 1.0f;
        SetState("idle");
    }

    public override IEnumerator Found()
    {
        _animator.speed = 0.0f;
        SetState("found");
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _animator.SetBool("WalkLeft", !alertDirection);

        yield return new WaitForSeconds(alertDuration);

        _animator.speed = 1.0f;
        SetState("attack");
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
}
