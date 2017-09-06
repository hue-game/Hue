using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour {

    private Player _player;
    private Rigidbody2D _rb;
    private GameObject _parentRope;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody2D>();
        _parentRope = transform.parent.gameObject;
    }

    void FixedUpdate()
    {
        if (_player.onRope == gameObject)
        {
            _player.transform.position = transform.position;
            _player.transform.localRotation = Quaternion.Inverse(transform.rotation);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_player.onRope == null)
        {
            if (other.gameObject == _player.gameObject)
            {
                _player.onRope = gameObject;
                _player.transform.parent = gameObject.transform;
                _rb.AddForce(new Vector2(_player.GetComponent<Rigidbody2D>().velocity.x / 2, -5.0f), ForceMode2D.Impulse);
                _player.GetComponent<Rigidbody2D>().simulated = false;
                _parentRope.GetComponent<Rope>().ChangeMass(gameObject, 2.0f);
            }
        }
    }

    public void ExitRope()
    {
        if (_player.onRope == gameObject)
        {
            _player.onRope = null;
            _player.transform.parent = null;
            _player.GetComponent<Rigidbody2D>().simulated = true;
            _player.GetComponent<Rigidbody2D>().velocity = _rb.velocity * 2;
            _parentRope.GetComponent<Rope>().ChangeMass(gameObject, 1.0f);
            _parentRope.SetActive(false);
        }
    }
}
