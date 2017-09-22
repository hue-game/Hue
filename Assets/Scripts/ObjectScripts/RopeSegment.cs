using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour {

    private IPlayer _player;
    private Rigidbody2D _rb;
    private GameObject _parentRope;
    private float _ropeLeaveMultiplier;
    private float _mass;
    [HideInInspector]
    public int index;

	Animator ClimbAnimation;

    public float RopeLeaveMultiplier {
		set {
			_ropeLeaveMultiplier = value;
		}
	}

    void Awake()
    {
        _player = FindObjectOfType<IPlayer>();
        _rb = GetComponent<Rigidbody2D>();
        _parentRope = transform.parent.gameObject;
        _mass = _parentRope.GetComponent<Rope>().ropeMass;

		ClimbAnimation = _player.GetComponent<Animator>();
    }

    //void Update()
    //{
    //    if (_player.onRope == gameObject)
    //        _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    //}

    //void FixedUpdate()
    //{
    //    if (_player.onRope == gameObject)
    //    {
    //        RelativeJoint2D playerJoint = _player.GetComponent<RelativeJoint2D>();
    //        playerJoint.linearOffset

    //    }
    //}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_player.onRope == null && _player.onLadder == false)
        {
            if (other.gameObject == _player.gameObject)
            {
                _player.onRope = gameObject;
                _player.transform.parent = transform;
                //_player.transform.position = transform.position;
                //_rb.AddForce(new Vector2(_player.GetComponent<Rigidbody2D>().velocity.x / 2, -5.0f), ForceMode2D.Impulse);
                //_player.GetComponent<Rigidbody2D>().simulated = false;
                //_parentRope.GetComponent<Rope>().ChangeMass(gameObject, _mass * 2f);

                _player.GetComponent<Collider2D>().isTrigger = true;
                RelativeJoint2D _playerHJ = _player.gameObject.AddComponent<RelativeJoint2D>();
                _playerHJ.enableCollision = false;
                _playerHJ.maxForce = 200;
                _playerHJ.maxTorque = 200;
                _playerHJ.autoConfigureOffset = false;
                _playerHJ.linearOffset = new Vector2(0.2f, 0f);
                _playerHJ.angularOffset = 0;
           

                //_playerHJ.linearOffset = Vector2.zero;
                //_playerHJ.connectedAnchor = Vector2.zero;
                //_playerHJ.enableCollision = false;
                _playerHJ.connectedBody = _rb;


				ClimbAnimation.ResetTrigger("Jump");
				ClimbAnimation.SetTrigger("Climbing");
            }
        }
    }

    public void ExitRope()
    {
        if (_player.onRope == gameObject)
        {
            _player.onRope = null;
            _player.transform.parent = null;
            _player.GetComponent<Collider2D>().isTrigger = false;
            _player.GetComponent<Rigidbody2D>().simulated = true;
            _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _player.GetComponent<Rigidbody2D>().AddForce(_rb.velocity * 2, ForceMode2D.Impulse);
            Destroy(_player.GetComponent<Joint2D>());

            //print(_rb.velocity);
            //Vector2 dismount = Vector2.ClampMagnitude(_rb.velocity, Vector2.Dot(new Vector2(_player.GetComponent<Jump>().jumpX, _player.GetComponent<Jump>().jumpY), new Vector2(2, 2)));
            //print(dismount);
            //print(Vector2.SqrMagnitude(_rb.velocity));
            //Vector2 dismount1 = Vector2.ClampMagnitude(_rb.velocity, 3);
            //print(dismount1);
            //_player.GetComponent<Rigidbody2D>().AddForce(dismount1 * 2, ForceMode2D.Impulse);
            //_player.GetComponent<Rigidbody2D>().velocity = _rb.velocity * _ropeLeaveMultiplier;
            print(_rb.velocity);
            //_player.GetComponent<Rigidbody2D>().velocity = _rb.velocity * 2;
            //_parentRope.GetComponent<Rope>().ChangeMass(gameObject, _mass);

			ClimbAnimation.SetTrigger("Jump");
			ClimbAnimation.ResetTrigger("Climbing");
			ClimbAnimation.speed = 1f;

        }
    }

    public void TogglePlayerCollision(bool enable)
    {
		if (_player.GetComponent<Collider2D>())
		{
				if (!enable)
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _player.GetComponent<Collider2D>());
				else
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _player.GetComponent<Collider2D>(), false);
		}
    }
		

}
	
