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

        _rb.velocity = _moveDirection;
    }

    void Update () {
        if (_moveDirection.x > 0) 
        {
            //Change animation
            _flipScript.FlipSprite(false);
        }
        else if (_moveDirection.x < 0)
        {
            //Change animation
            _flipScript.FlipSprite(true);
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
	}

    public override void Attack()
    {
        if (_playerTransform.position.x > transform.position.x)
            _moveDirection = Vector2.right;
        else
            _moveDirection = Vector2.left;

        _rb.velocity = new Vector2(_moveDirection.x * attackSpeed, _rb.velocity.y);

        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer > alertRadius)
            Lost();
    }

    public override void Idle()
    {
        if (OutOfRangeCheck())
        {
            if (roamingArea.bounds.center.x > transform.position.x)
                _moveDirection = Vector2.right;
            else
                _moveDirection = Vector2.left;
        }
        else if (EdgeCheck())
            _moveDirection *= -1;
        else if (ObstacleCheck())
            _moveDirection *= -1;

        _rb.velocity = new Vector2(_moveDirection.x * idleSpeed, _rb.velocity.y);
        
        if (followPlayer)
        {
            float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer < alertRadius)
                Found();
        }
    }

    public override bool OutOfRangeCheck()
    {
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), roamingArea.GetComponent<Collider2D>()))
            return false;
        else
            return true;
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
