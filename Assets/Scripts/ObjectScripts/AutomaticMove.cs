using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AutomaticMove : MonoBehaviour {
	[Range(-10, 10)]
	public float movementSpeedX = 2f;

	[Range(-10, 10)]
	public float movementSpeedY = 0f;

	[Range(0, 25)]
	public float offsetFromCenterX = 2f;

	[Range(0, 25)]
	public float offsetFromCenterY = 2f;

    private Rigidbody2D _playerBody;
    private Rigidbody2D _body;
    private Vector3 _originalPosition;
	private float _activeVelocityX;
	private float _activeVelocityY;
    private bool _playerOnPlatform;

	// Use this for initialization
	void Start () {
        _playerBody = FindObjectOfType<IPlayer>().GetComponent<Rigidbody2D>();
		_body = GetComponent<Rigidbody2D>();
		_originalPosition = transform.position;
		_activeVelocityX = movementSpeedX;
		_activeVelocityY = movementSpeedY;
	}
	
	private void FixedUpdate() {
		if (movementSpeedX > 0) {
			if (_originalPosition.x - this.transform.position.x > offsetFromCenterX) {
				_activeVelocityX = movementSpeedX;	
			} else if (_originalPosition.x - this.transform.position.x < -offsetFromCenterX) {
				_activeVelocityX = -movementSpeedX;
			}
		} else if (movementSpeedX < 0) {
			if (_originalPosition.x - this.transform.position.x < -offsetFromCenterX) {
				_activeVelocityX = movementSpeedX;
			} else if (_originalPosition.x - this.transform.position.x > offsetFromCenterX) {
				_activeVelocityX = -movementSpeedX;
			}
		}

		if (movementSpeedY > 0) {
			if (_originalPosition.y - this.transform.position.y > offsetFromCenterY) {
				_activeVelocityY = movementSpeedY;	
			} else if (_originalPosition.y - this.transform.position.y < -offsetFromCenterY) {
				_activeVelocityY = -movementSpeedY;
			}
		} else if (movementSpeedY < 0) {
			if (_originalPosition.y - this.transform.position.y < -offsetFromCenterY) {
				_activeVelocityY = movementSpeedY;	
			} else if (_originalPosition.y - this.transform.position.y > offsetFromCenterY) {
				_activeVelocityY = -movementSpeedY;
			}
		}
			
		_body.velocity = new Vector2 (_activeVelocityX, _activeVelocityY);

        if (_playerOnPlatform)
        {
            _playerBody.transform.position = _playerBody.position + _body.velocity * Time.deltaTime;
            _playerOnPlatform = false;
        }
    }
 
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Feet")
            _playerOnPlatform = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Feet")
            _playerOnPlatform = true;
    }
}
