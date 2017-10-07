using System;
using System.Collections;
using UnityEngine;
using System.Linq;

public class Buzzard : IEnemy {

    public RuntimeAnimatorController dreamAnimator;
    public AnimatorOverrideController nightmareAnimator;
    public GameObject featherPrefab;

    public float changeDirectionMin = 1f;
    public float changeDirectionMax = 5f;
    public float hoverHeight = 3f;
    public float maximumHoverOffset = 3.0f;
    public float attackCooldown = 1f;

    private bool _lostToIdle = false;
    private float _nextDirectionSwitch = 0;
    private bool _turning = false;
    private Vector2 _newMoveDirection;
    private int _hoverLocation = 0;
    private float _hoverOffset = 0f;

    private AudioSource[] _audioSources;


    new void Awake () {
        base.Awake();

        _moveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

        _oldMoveDirection = _moveDirection;
        _rb.velocity = _moveDirection;
        _nextDirectionSwitch = Time.time + UnityEngine.Random.Range(changeDirectionMin, changeDirectionMax);

        _audioSources = GetComponents<AudioSource>();
        _audioSources[0].Play();

        StartCoroutine(Screach());
    }

    void FixedUpdate () {

        if (_player.isDead)
            SetState("idle");

        if (state != "lost" && state != "found")
        {
            if (_moveDirection.x > 0)
                _flipScript.FlipSprite(true);
            else if (_moveDirection.x < 0)
                _flipScript.FlipSprite(false);
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
                _rb.velocity = _moveDirection;
                break;
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

		//Flap.Play();
    }

    public override void Attack()
    {
        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer > alertRadius)
            StartCoroutine(Lost());

        if (_playerTransform.position.x > transform.position.x)
            _flipScript.FlipSprite(true);
        else
            _flipScript.FlipSprite(false);

        _hoverLocation = FlightMode();

        if (_hoverLocation == 0)
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_playerTransform.position + new Vector2(_hoverOffset, hoverHeight), attackSpeed * Time.deltaTime);
        else if (_hoverLocation == 1)
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_playerTransform.position + new Vector2(hoverHeight, 0.4f + Math.Abs(_hoverOffset / 2)), attackSpeed * Time.deltaTime);
        else if (_hoverLocation == 2)
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_playerTransform.position + new Vector2(-hoverHeight, 0.4f + Math.Abs(_hoverOffset / 2)), attackSpeed * Time.deltaTime);
    }

    public override IEnumerator Lost()
    {
        _audioSources[0].Stop();
        _audioSources[1].Stop();

        CancelInvoke("ThrowFeather");
        CancelInvoke("AttackAnimation");
        SetState("lost");
        _animator.speed = 0.0f;
        _alertAnimator.SetBool("Lost", true);
        _alertAnimator.SetBool("Found", false);

        _moveDirection = Vector2.zero;

        yield return new WaitForSeconds(alertDuration);

        _alertAnimator.SetBool("Lost", false);
        _animator.speed = 1.0f;
        SetState("idle");
        _lostToIdle = true;

        _audioSources[0].Play();
    }

    public override IEnumerator Found()
    {
        _audioSources[0].Stop();
        _audioSources[1].Stop();

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

        _hoverOffset = UnityEngine.Random.Range(-maximumHoverOffset, maximumHoverOffset);

        InvokeRepeating("AttackAnimation", attackCooldown - 0.35f, attackCooldown);
        InvokeRepeating("ThrowFeather", attackCooldown, attackCooldown);

        _audioSources[1].Play();
    }

    public override void FoundLookDirection()
    {
        bool alertDirection = _playerTransform.position.x > transform.position.x;
        _flipScript.FlipSprite(alertDirection);
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

    private int FlightMode()
    {
        if (_playerTransform.position.x > roamingArea.bounds.center.x + roamingArea.bounds.extents.x)
            return 2;
        else if (_playerTransform.position.x < roamingArea.bounds.center.x - roamingArea.bounds.extents.x)
            return 1;
        else
            return 0;
    }

    private void AttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    private void ThrowFeather()
    {
        GameObject feather = Instantiate(featherPrefab, transform.position, Quaternion.identity, null);

        if (_worldManager.worldType)
        {
            feather.layer = LayerMask.NameToLayer("DreamWorld");
            feather.GetComponent<SpriteRenderer>().sprite = feather.GetComponent<Feather>().dreamFeather;
        }
        else
        {
            feather.layer = LayerMask.NameToLayer("NightmareWorld");
            feather.GetComponent<SpriteRenderer>().sprite = feather.GetComponent<Feather>().nightmareFeather;
        }

        Vector2 featherAngle =  _playerTransform.position - transform.position;
        float featherAngleFloat = (float) ((Mathf.Atan2(featherAngle.x, featherAngle.y) / Math.PI) * 180f);
        if (featherAngleFloat < 0)
            featherAngleFloat += 360f;

        feather.transform.rotation = Quaternion.Euler(0f, 0f, -featherAngleFloat);
        feather.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, -featherAngleFloat) * Vector2.up * feather.GetComponent<Feather>().speed;

        foreach (Buzzard buzzard in _worldManager.buzzards)
            Physics2D.IgnoreCollision(buzzard.GetComponent<Collider2D>(), feather.GetComponent<Collider2D>());
    }

    public override bool OutOfRangeCheck()
    {
        if (Physics2D.IsTouching(GetComponent<Collider2D>(), roamingArea.GetComponent<Collider2D>()))
            return true;
        else
            return false;
    }

    public override bool EdgeCheck()
    {
        return false;
    }

    public override bool ObstacleCheck()
    {
        return false;
    }

    public void ChangeBuzzardSprite()
    { 
        if (_worldManager.worldType)
            GetComponent<Animator>().runtimeAnimatorController = dreamAnimator;
        else
            GetComponent<Animator>().runtimeAnimatorController = nightmareAnimator;
    }

    private IEnumerator Screach()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5, 10));
            _audioSources[2].Play();
        }
    }

    private void OnDestroy()
    {
        _worldManager.buzzards.Where(val => val != gameObject).ToArray();
    }
}
