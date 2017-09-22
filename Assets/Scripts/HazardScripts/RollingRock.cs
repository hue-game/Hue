using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class RollingRock : MonoBehaviour {

    public float killVelocity = 3;

    private Rigidbody2D _rb;
    private Player _player;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
	}
	
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_rb.velocity.sqrMagnitude > 3)
                StartCoroutine(_player.KillPlayer());
        }
    }
}
