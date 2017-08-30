using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float runSpeed;
    public float jumpHeight;
    private bool grounded;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * runSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpHeight));
                grounded = false;
            }
        }

    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        Vector2 contactPoint = hit.contacts[0].point;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        float offset = GetComponent<Collider2D>().bounds.extents.y;

        grounded = contactPoint.y <= center.y - offset;
    }

    void OnCollisionStay2D(Collision2D hit)
    {
        Vector2 contactPoint = hit.contacts[0].point;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        float offset = GetComponent<Collider2D>().bounds.extents.y;

        grounded = contactPoint.y <= center.y - offset;
    }

    void OnCollisionExit2D(Collision2D hit)
    {
        grounded = false;
    }
}
