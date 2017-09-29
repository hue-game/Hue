using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour {
    [HideInInspector]
    public LayerMask layerMask;
    [HideInInspector]
    public float angle;

    public Sprite dreamFeather;
    public Sprite nightmareFeather;

    private Rigidbody2D _rb;
    private WorldManager _worldManager;

    private float _timeAlive = 0f;
    private float _killAfterSeconds = 3f;

	// Use this for initialization
	void Awake () {
        //gameObject.layer = layerMask;
        _rb = GetComponent<Rigidbody2D>();
        _worldManager = FindObjectOfType<WorldManager>();
        _worldManager.AddGameObject(gameObject);
        _timeAlive = Time.time;
        _killAfterSeconds = UnityEngine.Random.Range(0f, 3f);

        _rb.MoveRotation(-angle);
    }

    // Update is called once per frame
    void Update () {
	    if (Time.time > (_timeAlive + _killAfterSeconds))	
        {
            _worldManager.RemoveGameObject(gameObject);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        _rb.simulated = false;
    }
}
