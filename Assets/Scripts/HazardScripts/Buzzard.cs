using System;
using System.Collections;
using UnityEngine;

public class Buzzard : IEnemy {

    public RuntimeAnimatorController dreamAnimator;
    public AnimatorOverrideController nightmareAnimator;

    public float changeDirectionMin = 1f;
    public float changeDirectionMax = 5f;
    //public float attackCooldown = 1f;

    private bool _lostToIdle = false;
    private float _nextDirectionSwitch = 0;
    private bool _turning = false;
    private Vector2 _newMoveDirection;
    private bool _hoverAbove = true;

    new void Awake () {
        base.Awake();

        _moveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

        _oldMoveDirection = _moveDirection;
        _rb.velocity = _moveDirection;
        _nextDirectionSwitch = Time.time + UnityEngine.Random.Range(changeDirectionMin, changeDirectionMax);
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
                _rb.velocity = _moveDirection;
                break;
        }

        if (state != "lost" && state != "found")
        {
            if (_moveDirection.x > 0)
                _flipScript.FlipSprite(true);
            else if (_moveDirection.x < 0)
                _flipScript.FlipSprite(false);
        }
    }

    public override void Idle()
    {
        if (!_turning)
        {
            if (OutOfRangeCheck())
            {
                _oldMoveDirection = _moveDirection;
                _newMoveDirection = (roamingArea.bounds.center - transform.position).normalized;
                StartCoroutine(Turn());
            }
            else if (ObstacleCheck())
            {
                _oldMoveDirection = _moveDirection;
                _newMoveDirection = _moveDirection *= -1;
                StartCoroutine(Turn());
            }
            else if (_lostToIdle)
            {
                _lostToIdle = false;
                _oldMoveDirection = _moveDirection;
                _newMoveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                StartCoroutine(Turn());
            }
            else if (Time.time > _nextDirectionSwitch)
            {
                _nextDirectionSwitch = Time.time + UnityEngine.Random.Range(changeDirectionMin, changeDirectionMax);
                _oldMoveDirection = _moveDirection;

                Vector2 center = (roamingArea.bounds.center - transform.position).normalized;
                bool moveTowardsCenter = false;
                if (UnityEngine.Random.Range(0f, 1f) > 0.66f)
                    moveTowardsCenter = true;

                if (moveTowardsCenter)
                {
                    if (center.x > 0 && center.y > 0)
                        _newMoveDirection = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                    else if (center.x > 0 && center.y < 0)
                        _newMoveDirection = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(-1f, 0f));
                    else if (center.x < 0 && center.y < 0)
                        _newMoveDirection = new Vector2(UnityEngine.Random.Range(-1f, 0f), UnityEngine.Random.Range(-1f, 0f));
                    else if (center.x < 0 && center.y > 0)
                        _newMoveDirection = new Vector2(UnityEngine.Random.Range(-1f, 0f), UnityEngine.Random.Range(0f, 1f));
                    else
                        _newMoveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                }
                else
                    _newMoveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

                StartCoroutine(Turn());
            }
        }

        _rb.velocity = _moveDirection * idleSpeed;
        
        if (!_player.isDead)
        {
            float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer < alertRadius)
                StartCoroutine(Found());
        }
    }

    public override void Attack()
    {
        _moveDirection = (_playerTransform.position- transform.position).normalized;

        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer > alertRadius)
            StartCoroutine(Lost());

        _rb.velocity = _moveDirection * attackSpeed;
    }

    public override IEnumerator Lost()
    {
        SetState("lost");
        _animator.speed = 0.0f;
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);

        _alertAnimator.SetBool("Lost", true);
        _alertAnimator.SetBool("Found", false);

        _moveDirection = Vector2.zero;

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
        
        _alertAnimator.SetBool("Lost", false);
        _alertAnimator.SetBool("Found", true);
        FoundLookDirection();

        _moveDirection = Vector2.zero;

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Found", false);
        _animator.speed = 1.0f;
        SetState("attack");
    }

    public override void FoundLookDirection()
    {
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _flipScript.FlipSprite(alertDirection);
    }

    public override bool ObstacleCheck()
    {
        RaycastHit2D[] obstacleHitChecks;

        if (_moveDirection.x > 0)
            obstacleHitChecks = Physics2D.RaycastAll(transform.position, Vector2.right, 1f);
        else
            obstacleHitChecks = Physics2D.RaycastAll(transform.position, Vector2.left, 1f);

        foreach (RaycastHit2D obstacleHit in obstacleHitChecks)
        {
            if (obstacleHit.transform.GetComponent<IEnemy>() == null && obstacleHit.transform.tag != "Player")
                return true;
        }

        if (_moveDirection.y > 0)
            obstacleHitChecks = Physics2D.RaycastAll(transform.position, Vector2.down, 1f);
        else
            obstacleHitChecks = Physics2D.RaycastAll(transform.position, Vector2.up, 1f);

        foreach (RaycastHit2D obstacleHit in obstacleHitChecks)
        {
            if (obstacleHit.transform.GetComponent<IEnemy>() == null && obstacleHit.transform.tag != "Player")
                return true;
        }

        return false;
    }

    private IEnumerator Turn()
    {
        _turning = true;

        float t = 0;
        while (t < 1)
        {
            t += Time.fixedDeltaTime * (Time.timeScale / 0.6f);

            _moveDirection = Vector2.Lerp(_oldMoveDirection, _newMoveDirection, t);
            yield return null;
        }

        _turning = false;
    }



    //private IEnumerator Slide()
    //{
    //    _isSliding = true;
    //    _animator.SetBool("Attack", false);
    //    _animator.SetBool("Sliding", true);
    //    Vector2 startVelocity = _rb.velocity;
    //    float t = 0;
    //    while (t < 1)
    //    {
    //        Vector2 endVelocity = new Vector2(0, _rb.velocity.y);
    //        t += Time.fixedDeltaTime * (Time.timeScale / slideDuration);

    //        _rb.velocity = Vector2.Lerp(new Vector2(startVelocity.x, _rb.velocity.y), endVelocity, t);
    //        yield return null;
    //    }

    //    _isSliding = false;
    //    _animator.SetBool("Sliding", false);
    //    _animator.SetBool("idle", true);
    //}


    public override bool EdgeCheck()
    {
        return false;
    }

}
