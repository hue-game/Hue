using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : IEnemy {

    public bool followPlayer = true;

    // Use this for initialization
    new void Awake () {
        base.Awake();
	}
	
	// Update is called once per frame
	void Update () {
        float moveDirection = _rb.velocity.x;
        if (moveDirection > 0)
            _flipScript.FlipSprite(false);
        else if (moveDirection < 0)
            _flipScript.FlipSprite(true);

        switch (state)
        {
            case "idle":
                Idle();
                break;
            case "Attack":
                Attack();
                break;
            default:
                break;
        }
	}

    public override void Attack()
    {
        //move to player;


        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer > alertRadius)
        {
            SetState("Lost");
            Lost();
        }
    }

    public override void Idle()
    {
        //Move randomly


        float distanceToPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        if (followPlayer)
        {
            if (distanceToPlayer < alertRadius)
            {
                SetState("Found");
                Found();
            }
        }
    }


}
