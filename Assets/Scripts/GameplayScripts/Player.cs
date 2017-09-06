using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Move))]
[RequireComponent(typeof(WorldManager))]
[RequireComponent(typeof(CheckpointManager))]
<<<<<<< HEAD
[RequireComponent(typeof(CollectibleManager))]
=======
>>>>>>> f80aaec3034aaea6b7a5c56dcf3b7cf66f8d1ede
public class Player : MonoBehaviour
{
    [HideInInspector]
    public GameObject onRope;

	[Range(0, 25)]
	public float respawnTime = 3.0f;
	private CheckpointManager _checkpointManager;
	private WorldManager _worldManager;
    private Move _moveScript;
    private Jump _jumpScript;

    private void Awake()
    {
		_checkpointManager = GetComponent<CheckpointManager> ();
		_worldManager = GetComponent<WorldManager> ();

        _moveScript = GetComponent<Move>();
        _jumpScript = GetComponent<Jump>();
		_worldManager = GetComponent<WorldManager>();
        onRope = null;
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

	void Respawn(GameObject checkpoint) {
		transform.position = checkpoint.transform.position;
	}

    //Check when the player collides with an object
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Danger")
        {
            Respawn(_checkpointManager.GetLastCheckpoint());
        }
    }
	void OnTriggerEnter2D(Collider2D hit) {
		if (hit.tag == "Checkpoint") {
			_checkpointManager.SetNewCheckpoint(hit.gameObject);
		}

		if (hit.tag == "Danger") {
			Respawn(_checkpointManager.GetLastCheckpoint());
		}
	}
}




