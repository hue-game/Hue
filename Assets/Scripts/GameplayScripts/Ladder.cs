using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour {
	private IPlayer _player;
    private 
	// Use this for initialization

	Animator ClimbAnimation;


	void Start () {
		_player = FindObjectOfType<IPlayer>();	
		ClimbAnimation = _player.GetComponent<Animator>();
	}

	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player" && _player.onRope == null) {
            //_player.GetComponent<Animator>().SetTrigger("Land");
            //_player.GetComponent<Animator>().ResetTrigger("Jump");
            _player.onLadder = true;


		}
	}

	void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player")
        {
            bool differentLadderFound = false;
            foreach(Ladder ladder in FindObjectsOfType<Ladder>())
            {
                if(Physics2D.IsTouching(_player.GetComponent<Collider2D>(), ladder.GetComponent<Collider2D>()) && ladder != gameObject.GetComponent<Ladder>())
                {
                    differentLadderFound = true;
                }
            }
            if (!differentLadderFound)
            {
                _player.GetComponent<Rigidbody2D>().isKinematic = false;
                _player.onLadder = false;

				// ClimbAnimation.SetTrigger("Jump");
				ClimbAnimation.ResetTrigger("Climbing");
				ClimbAnimation.SetTrigger("Land");
				ClimbAnimation.speed = 1f;

            }
        }
    }
}
