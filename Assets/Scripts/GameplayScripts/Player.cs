using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(WorldManager))]
[RequireComponent(typeof(CollectibleManager))]
[RequireComponent(typeof(CheckpointManager))]
public class Player : IPlayer
{
	[Range(0, 25)]

	public float respawnTime = 3.0f;
	private CheckpointManager _checkpointManager;
	private WorldManager _worldManager;
    private GameManager _gameManager;
	private Rigidbody2D _rigidBody;

	Animator DieAnimation;

    private void Awake()
    {
		_checkpointManager = GetComponent<CheckpointManager> ();
		_worldManager = GetComponent<WorldManager> ();
        _gameManager = FindObjectOfType<GameManager>();
        _moveScript = GetComponent<Move>();
		_rigidBody = GetComponent<Rigidbody2D> ();
        _jumpScript = GetComponent<Jump>();
        _input = GetComponent<InputManager>();

		DieAnimation = GetComponent<Animator> ();
    }

    // FixedUpdate is called once per frame after physics have applied
    private void FixedUpdate()
    {
        _moveScript.MoveAnalog(_input.movementX, _input.movementY);
    }

    //Check for key inputs every frame
    private void Update()
    {
        if (_input.jump)
            _jumpScript.JumpUp();

        if (_input.Switch)
        {
            _worldManager.SwitchWorld();
            _gameManager.PlayerInteract();
        }
    }

	void Respawn(GameObject checkpoint) {
		//transform.position = checkpoint.transform.position;

  //      _worldManager.ResetRopes();
  //      _worldManager.ResetTriggers();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    //Check when the player collides with an object
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Danger")
        {
            StartCoroutine(KillPlayer());

        }
    }

    void OnTriggerEnter2D(Collider2D hit) {
	    if (hit.tag == "Checkpoint") {
		    _checkpointManager.SetNewCheckpoint(hit.gameObject);
	    }

	    if (hit.tag == "Danger") {
            StartCoroutine(KillPlayer());
        }
    } 

    public IEnumerator KillPlayer()
    {
        _rigidBody.simulated = false;
        
		DieAnimation.SetTrigger ("Die");
        yield return new WaitForSeconds(0.9f);
		DieAnimation.ResetTrigger ("Die");

        Respawn(_checkpointManager.GetLastCheckpoint());
    }
}




