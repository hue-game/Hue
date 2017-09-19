using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour {
	private Player _player;

	// Use this for initialization
	void Start () {
		_player = FindObjectOfType<Player> ();	
	}

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			_player.onLadder = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Player") {
			_player.onLadder = false;
		}
	}
}
