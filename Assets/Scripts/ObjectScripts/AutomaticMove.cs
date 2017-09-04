using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class AutomaticMove : MonoBehaviour {
	[Range(0, 10)]
	public float movementSpeedX = 2f;

	[Range(0, 10)]
	public float movementSpeedY = 0f;

	[Range(0, 25)]
	public float offsetFromCenterX = 2f;

	[Range(0, 25)]
	public float offsetFromCenterY = 2f;

	private Rigidbody2D _body;
	private Vector3 _originalPosition;
	private float _activeVelocityX;
	private float _activeVelocityY;

	// Use this for initialization
	void Start () {
		_body = GetComponent<Rigidbody2D>();
		_originalPosition = transform.position;
		_activeVelocityX = movementSpeedX;
		_activeVelocityY = movementSpeedY;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (_originalPosition.x - this.transform.position.x > offsetFromCenterX) {
			_activeVelocityX = movementSpeedX;	
		} else if (_originalPosition.x - this.transform.position.x < -offsetFromCenterX) {
			_activeVelocityX = -movementSpeedX;
		}

		if (_originalPosition.y - this.transform.position.y > offsetFromCenterY) {
			_activeVelocityY = movementSpeedY;	
		} else if (_originalPosition.y - this.transform.position.y < -offsetFromCenterY) {
			_activeVelocityY = -movementSpeedY;
		}

		_body.velocity = new Vector2 (_activeVelocityX, _activeVelocityY);
	}
}
