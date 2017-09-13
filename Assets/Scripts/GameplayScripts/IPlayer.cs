using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class IPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject onRope;

    protected Move _moveScript;
    protected InputManager _input;
}

