using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Move))]
[RequireComponent(typeof(WorldManager))]
public class Player : MonoBehaviour
{
	private WorldManager _worldManager;
    private Move _moveScript;
    private Jump _jumpScript;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveScript = GetComponent<Move>();
        _jumpScript = GetComponent<Jump>();
		_worldManager = GetComponent<WorldManager> ();
    }

    // FixedUpdate is called once per frame after physics have applied
    private void FixedUpdate()
    {
        _moveScript.MoveCharacter();
    }

    //Check for key inputs every frame
    private void Update()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            _moveScript.MoveLeft();
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            _moveScript.MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _moveScript.MoveRight();
        if (Input.GetKeyUp(KeyCode.RightArrow))
            _moveScript.MoveRight();

        if (Input.GetButtonDown("Jump"))
            _jumpScript.JumpUp();
        if (Input.GetKeyDown(KeyCode.LeftControl))
            _worldManager.SwitchWorld();
        #endif
    }

    //Check when the player collides with an object
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.collider.tag == "Danger")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (hit.collider.tag == "Rope")
        {
            HingeJoint2D myJoint = (HingeJoint2D)gameObject.AddComponent<HingeJoint2D>();
            myJoint.connectedBody = hit.rigidbody;
        }

        //Code below only used for manual jump
        Vector2 contactPoint = hit.contacts[0].point;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        float offset = GetComponent<Collider2D>().bounds.extents.y;

        _jumpScript.SetGrounded(contactPoint.y <= center.y - offset);

    }

    //Only used for manual jump
    void OnCollisionStay2D(Collision2D hit)
    {
        Vector2 contactPoint = hit.contacts[0].point;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        float offset = GetComponent<Collider2D>().bounds.extents.y;

        _jumpScript.SetGrounded(contactPoint.y <= center.y - offset);
    }

    //Only used for manual jump
    void OnCollisionExit2D(Collision2D hit)
    {
        _jumpScript.SetGrounded(false);
    }
}




