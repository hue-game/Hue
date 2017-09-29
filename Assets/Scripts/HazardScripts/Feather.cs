using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour {

    public float speed = 8f;
    public float _killAfterSeconds = 10f;

    public Sprite dreamFeather;
    public Sprite nightmareFeather;

    private Rigidbody2D _rb;
    private WorldManager _worldManager;

    private float _timeAlive = 0f;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _worldManager = FindObjectOfType<WorldManager>();
        _worldManager.AddGameObject(gameObject);
        _timeAlive = Time.time;
    }

    // Update is called once per frame
    void Update () {
	    if (Time.time > (_timeAlive + _killAfterSeconds))	
        {
            _worldManager.RemoveGameObject(gameObject);
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Feather>() == null)        
            _rb.simulated = false;
        if (other.gameObject.tag != "Player")
            gameObject.tag = "Untagged";
    }
}
