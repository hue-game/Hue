using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(WorldManager))]
[RequireComponent(typeof(CollectibleManager))]
[RequireComponent(typeof(CheckpointManager))]
public class Player : IPlayer
{
    public float respawnTime = 3.0f;
    public bool isScaring = false;
    public GameObject SwitchShockwave;
    private CheckpointManager _checkpointManager;
	private WorldManager _worldManager;
    private GameManager _gameManager;
	private Rigidbody2D _rigidBody;

	Animator DieAnimation;

    private AudioSource _dieSound;

    private void Awake()
    {
		_checkpointManager = GetComponent<CheckpointManager> ();
		_worldManager = GetComponent<WorldManager> ();
        _gameManager = FindObjectOfType<GameManager>();
        _moveScript = GetComponent<Move>();
		_rigidBody = GetComponent<Rigidbody2D> ();
        _jumpScript = GetComponent<Jump>();
        _input = GetComponent<InputManager>();
        _dieSound = GetComponents<AudioSource>()[3];

        DieAnimation = GetComponent<Animator> ();
    }

    // FixedUpdate is called once per frame after physics have applied
    private void FixedUpdate()
    {
        //if (!isDead && !isScaring)
            _moveScript.MoveAnalog(_input.movementX, _input.movementY);
    }

    //Check for key inputs every frame
    private void Update()
    {
        if (_input.jump)
            _jumpScript.JumpUp();

        if (_input.Switch && !isDead && !isScaring)
        {
            _gameManager.PlayerInteract();
            if (!isScaring)
            {
                Instantiate(SwitchShockwave);
                _worldManager.SwitchWorld();
            }
            _input.Switch = false;
        }

        if (isDead)
        {
            DieAnimation.ResetTrigger("Land");
            DieAnimation.ResetTrigger("Jump");
            DieAnimation.ResetTrigger("Climbing");
        }
    }

	void Respawn(GameObject checkpoint) 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    //Check when the player collides with an object
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Danger")
            StartCoroutine(KillPlayer());
    }

    void OnTriggerEnter2D(Collider2D hit) {
	    if (hit.tag == "Checkpoint")
		    _checkpointManager.SetNewCheckpoint(hit.gameObject);
	    else if (hit.tag == "Danger")
            StartCoroutine(KillPlayer());
    } 

    public IEnumerator KillPlayer()
    {
        isDead = true;
        _rigidBody.simulated = false;
        _dieSound.Play();

        DieAnimation.ResetTrigger("Land");
        DieAnimation.ResetTrigger("Jump");
        DieAnimation.ResetTrigger("Climbing");
        DieAnimation.SetTrigger ("Die");
        yield return new WaitForSeconds(0.9f);
		DieAnimation.ResetTrigger ("Die");

        Respawn(_checkpointManager.GetLastCheckpoint());
    }
}




