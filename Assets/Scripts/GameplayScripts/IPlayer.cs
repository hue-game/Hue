using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Jump))]
public abstract class IPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject onRope = null;

    [HideInInspector]
    public bool onLadder = false;

    protected Move _moveScript;
    protected InputManager _input;
    protected Jump _jumpScript;
}

