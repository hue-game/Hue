using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;
    public float jumpHeight;
    private bool grounded;
    private string currentColor;

    // Use this for initialization
    void Start()
    {
        SwitchColor("blue");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        //if (grounded)
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
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentColor == "blue")
                SwitchColor("red");
            else
                SwitchColor("blue");
        }
            

    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        Vector2 contactPoint = hit.contacts[0].point;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        float offset = GetComponent<Collider2D>().bounds.extents.y;

        grounded = contactPoint.y <= center.y - offset;

        if (hit.collider.tag == "Danger")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

    private void SwitchColor(string color)
    {
        switch (color)
        {
            case "blue":
                currentColor = "blue";
                GetComponent<SpriteRenderer>().color = Color.blue;
                runSpeed = 5;
                jumpHeight = 350;
                break;
            case "red":
                currentColor = "red";
                GetComponent<SpriteRenderer>().color = Color.red;
                runSpeed = 10;
                jumpHeight = 550;
                break;
            default: 
                break;
        }
    }

}
