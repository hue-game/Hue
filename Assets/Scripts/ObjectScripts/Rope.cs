using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    public Transform target;
    public float resolution = 0.5F;                        
    public float ropeDrag = 0.1F;                            
    public float ropeMass = 0.1F;                          
    public float ropeColRadius = 0.5F;               
    public float ropeSwingSpeed;                 
    
    private Vector3[] segmentPos;         
    private GameObject[] joints;         
    private LineRenderer line;               
    private int segments = 0;                 
    private bool rope = false;              
    private Material ropeMaterial;
    private InputManager _input;
    private IPlayer _player;
    private Rigidbody2D _playerRB;
    private RelativeJoint2D _playerJoint;
    private SpriteRenderer _playerSprite;

    private float ropeSwingDownwardForce;

	Animator ClimbAnimation;
	private float _playerSpeedY;

    private WorldManager _worldManager;

    void Awake()
    {
        _input = FindObjectOfType<InputManager>();
        _player = FindObjectOfType<IPlayer>();
        _playerRB = _player.GetComponent<Rigidbody2D>();
        _playerSprite = _player.GetComponent<SpriteRenderer>();
        _worldManager = FindObjectOfType<WorldManager>();
        ropeSwingDownwardForce = ropeSwingSpeed / 3;
		ClimbAnimation = _player.GetComponent<Animator>();
        
        BuildRope();
    }

    void LateUpdate()
    {
        // Does rope exist? If so, update its position
        if (rope)
        {
            for (int i = 0; i < segments; i++)
            {
                if (i == 0)
                    line.SetPosition(i, transform.position);
                else
                {
                    if (i == segments - 1)
                        line.SetPosition(i, target.transform.position);
                    else
                        line.SetPosition(i, joints[i].transform.position);
                }
            }
            line.enabled = true;
        }
        else
            line.enabled = false;
    }

    void FixedUpdate()
    {
        GameObject _seg = _player.onRope;

        if (_seg != null)
        {
            if (Array.IndexOf(joints, _seg) > -1)
            {
                _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                _playerJoint = _player.GetComponent<RelativeJoint2D>();
                _playerJoint.autoConfigureOffset = false;
                
                float moveX = _input.movementX;
                float moveY = _input.movementY;
                int moveToJointIndex = -1;

				ClimbAnimation.SetFloat("ClimbingSpeed", Mathf.Abs(moveY));
				ClimbAnimation.speed = ClimbAnimation.GetFloat("ClimbingSpeed");

                if (moveX != 0)
                    _playerRB.AddForce(new Vector2(moveX * ropeSwingSpeed, -ropeSwingDownwardForce));
        
                if (_playerSprite.flipX)
                    _playerJoint.linearOffset = new Vector2(-0.2f, 0f);
                else
                    _playerJoint.linearOffset = new Vector2(0.2f, 0f);

                if (moveY > 0)
                {
                    if (_seg.GetComponent<RopeSegment>().index > 1)
                    {
                        moveToJointIndex = _seg.GetComponent<RopeSegment>().index - 1;
                        _playerJoint.autoConfigureOffset = true;
                    }
                }
                else if (moveY < 0)
                {
                    if (_seg.GetComponent<RopeSegment>().index < joints.Length - 1)
                    {
                        moveToJointIndex = _seg.GetComponent<RopeSegment>().index + 1;
                        _playerJoint.autoConfigureOffset = true;
                    }
                }

                if (moveToJointIndex != -1)
                {
                    Vector3 playerOffset = new Vector3(_playerJoint.linearOffset.x, _playerJoint.linearOffset.y, 0f);
                    _player.transform.position = Vector2.MoveTowards(_player.transform.position, joints[moveToJointIndex].transform.position - playerOffset, 2.0f * Mathf.Abs(moveY) * Time.deltaTime);
                    float distanceToCurrent = Vector2.Distance(_player.transform.position + playerOffset, _seg.transform.position);
                    float distanceToNext = Vector2.Distance(_player.transform.position + playerOffset, joints[moveToJointIndex].transform.position);

                    if (distanceToNext < distanceToCurrent)
                        ChangeSegment(joints[moveToJointIndex]);
                }
            }
        }
    }

    public void BuildRope()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.material = new Material(line.material);
        line.useWorldSpace = true;

        // Find the amount of segments based on the distance and resolution
        segments = (int)(Vector3.Distance(transform.position, target.position) * resolution);
        line.positionCount = segments;
        segmentPos = new Vector3[segments];
        joints = new GameObject[segments];
        segmentPos[0] = transform.position;
        segmentPos[segments - 1] = target.position;

        // Find the distance between each segment
        var segs = segments - 1;
        var seperation = ((target.position - transform.position) / segs);

        for (int s = 1; s < segments; s++)
        {
            // Find the each segments position using the slope from above
            Vector3 vector = (seperation * s) + transform.position;
            segmentPos[s] = vector;

            //Add Physics to the segments
            AddJointPhysics(s);
        }

        // Attach the joints to the target object and parent it to this object	
        HingeJoint2D end = target.gameObject.AddComponent<HingeJoint2D>();
        end.connectedBody = joints[joints.Length - 1].transform.GetComponent<Rigidbody2D>();
        target.parent = transform;

        // Rope = true, The rope now exists in the scene!
        rope = true;
    }

    void AddJointPhysics(int n)
    {
        joints[n] = new GameObject("Joint_" + n);
        joints[n].transform.parent = transform;
        Rigidbody2D rigid = joints[n].AddComponent<Rigidbody2D>();
        CircleCollider2D col = joints[n].AddComponent<CircleCollider2D>();
        HingeJoint2D ph = joints[n].AddComponent<HingeJoint2D>();
        joints[n].AddComponent<RopeSegment>();
        joints[n].GetComponent<RopeSegment>().index = n;
        joints[n].transform.position = segmentPos[n];
        joints[n].layer = joints[n].transform.parent.gameObject.layer;
        _worldManager.AddGameObject(joints[n]);

        rigid.drag = ropeDrag;
        rigid.mass = ropeMass;
        col.radius = ropeColRadius;
        ph.enableCollision = true;

        if (n == 1)
            ph.connectedBody = transform.GetComponent<Rigidbody2D>();
        else
            ph.connectedBody = joints[n - 1].GetComponent<Rigidbody2D>();
    }

    private void ChangeSegment(GameObject segment)
    {
        _player.onRope = segment;
        _player.transform.parent = segment.transform;
        _player.GetComponent<Joint2D>().connectedBody = segment.GetComponent<Rigidbody2D>();
    }
}