using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public float runSpeedDefault;
    public float jumpHeightDefault;

    private bool grounded;
    private float runSpeed;
    private float jumpHeight;

    private bool moveLeft;
    private bool moveRight;
    private bool stop;

    // Use this for initialization
    void Awake()
    {
        runSpeed = runSpeedDefault * 1.6f;
        jumpHeight = jumpHeightDefault * 1.4f;
        SwitchColor();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        //Move(move);

    }

    void Update()
    {
        //if (Input.GetButtonDown("Jump"))
        //{
        //    Jump();
        //}
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SwitchColor();
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

    public void Jump()
    {
        if (grounded)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            grounded = false;
        }
    }

    public void Move(float move)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * runSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (move < 0)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    public void SwitchColor()
    {
        gameManager.SwitchColor();

        switch (gameManager.worldColor)
        {
            case "blue":
                GetComponent<SpriteRenderer>().color = Color.blue;
                //runSpeed = runSpeedDefault;
                //jumpHeight = jumpHeightDefault;
                break;
            case "red":
                GetComponent<SpriteRenderer>().color = Color.red;
                //runSpeed = runSpeedDefault * 1.6f;
                //jumpHeight = jumpHeightDefault * 1.4f;
                break;
            default:
                break;
        }
    }
}
