using UnityEngine;
using System.Collections;
using System;

// Require a Rigidbody and LineRenderer object for easier assembly
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]

public class Rope : MonoBehaviour
{
    /*========================================
	==  Physics Based Rope				==
	==  File: Rope.js					  ==
	==  Original by: Jacob Fletcher		==
	==  Use and alter Freely			 ==
	==  CSharp Conversion by: Chelsea Hash  ==
	==========================================
	How To Use:
	 ( BASIC )
	 1. Simply add this script to the object you want a rope teathered to
	 2. In the "LineRenderer" that is added, assign a material and adjust the width settings to your likeing
	 3. Assign the other end of the rope as the "Target" object in this script
	 4. Play and enjoy!
 
	 ( Advanced )
	 1. Do as instructed above
	 2. If you want more control over the rigidbody on the ropes end go ahead and manually
		 add the rigidbody component to the target end of the rope and adjust acordingly.
	 3. adjust settings as necessary in both the rigidbody and rope script
 
	 (About Character Joints)
	 Sometimes your rope needs to be very limp and by that I mean NO SPRINGY EFFECT.
	 In order to do this, you must loosen it up using the swingAxis and twist limits.
	 For example, On my joints in my drawing app, I set the swingAxis to (0,0,1) sense
	 the only axis I want to swing is the Z axis (facing the camera) and the other settings to around -100 or 100.
 
 
	*/
    public Transform target;
    public float resolution = 0.5F;                           //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
    public float ropeDrag = 0.1F;                                //  Sets each joints Drag
    public float ropeMass = 0.1F;                           //  Sets each joints Mass
    public float ropeColRadius = 0.5F;                  //  Sets the radius of the collider in the SphereCollider component
    public float ropeLeaveMultiplier = 2.0f;
    
    private Vector3[] segmentPos;           //  DONT MESS!	This is for the Line Renderer's Reference and to set up the positions of the gameObjects
    private GameObject[] joints;            //  DONT MESS!	This is the actual joint objects that will be automatically created
    private LineRenderer line;                          //  DONT MESS!	 The line renderer variable is set up when its assigned as a new component
    private int segments = 0;                   //  DONT MESS!	The number of segments is calculated based off of your distance * resolution
    private bool rope = false;                       //  DONT MESS!	This is to keep errors out of your debug window! Keeps the rope from rendering when it doesnt exist...
    private Material ropeMaterial;
    private GameObject ropeEnd;
    private Vector3 ropeEndPosition;
    private InputManager _input;
    private IPlayer _player;

    private float ropeSwingSpeed;
    private float ropeSwingDownwardForce;
    //Joint Settings
    //public Vector3 swingAxis = new Vector3(1, 1, 0);                 //  Sets which axis the character joint will swing on (1 axis is best for 2D, 2-3 axis is best for 3D (Default= 3 axis))
    //public float lowTwistLimit = -100.0F;                   //  The lower limit around the primary axis of the character joint. 
    //public float highTwistLimit = 100.0F;                   //  The upper limit around the primary axis of the character joint.
    //public float swing1Limit = 20.0F;                   //	The limit around the primary axis of the character joint starting at the initialization point.

    void Awake()
    {
        ropeEnd = target.gameObject;
        ropeEndPosition = ropeEnd.transform.localPosition;
        BuildRope();
        _input = FindObjectOfType<InputManager>();
        _player = FindObjectOfType<IPlayer>();
        ropeSwingSpeed = _player.GetComponent<Move>().ropeSwingSpeed;
        ropeSwingDownwardForce = _player.GetComponent<Move>().ropeSwingDownwardForce;
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
                float moveX = _input.movementX;
                float moveY = _input.movementY;
                int moveToJointIndex = -1;

                Rigidbody2D _ropeRb = _seg.GetComponent<Rigidbody2D>();
                if (moveX != 0)
                    _ropeRb.AddForce(new Vector2(moveX * ropeSwingSpeed, -ropeSwingDownwardForce));

                if (moveY > 0)
                {
                    if (_seg.GetComponent<RopeSegment>().index > 5)
                        moveToJointIndex = _seg.GetComponent<RopeSegment>().index - 1;
                }
                else if (moveY < 0)
                {
                    if (_seg.GetComponent<RopeSegment>().index < joints.Length - 1)
                        moveToJointIndex = _seg.GetComponent<RopeSegment>().index + 1;
                }

                if (moveToJointIndex != -1)
                {
                    _player.transform.position = Vector2.MoveTowards(_player.transform.position, joints[moveToJointIndex].transform.position, 1.5f * Mathf.Abs(moveY) * Time.deltaTime);
                    float distanceToCurrent = Vector2.Distance(_player.transform.position, _seg.transform.position);
                    float distanceToNext = Vector2.Distance(_player.transform.position, joints[moveToJointIndex].transform.position);

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
        // Example: [resolution of 1.0 = 1 joint per unit of distance]
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
        joints[n].GetComponent<RopeSegment>().RopeLeaveMultiplier = ropeLeaveMultiplier;
        joints[n].GetComponent<RopeSegment>().index = n;
        joints[n].transform.position = segmentPos[n];

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
    }

    public void ChangeMass(GameObject joint, float mass)
    {
        bool foundJoint = false;   
        for (int i = 0; i < joints.Length - 1; i++)
        {
            if (joints[i] == joint)
                joints[i].GetComponent<Rigidbody2D>().mass = mass;
                
            //if (joints[i] == joint)
            //    foundJoint = true;
            //if (foundJoint)
            //    joints[i].GetComponent<Rigidbody2D>().mass = mass;
        }
        foundJoint = false;
    }

    public void ToggleTriggerCollider()
    {
        foreach(GameObject joint in joints)
        {
            joint.GetComponent<CircleCollider2D>().isTrigger = !joint.GetComponent<CircleCollider2D>().isTrigger;
        }
    }

    public void DestroyRope()
    {
        // Stop Rendering Rope then Destroy all of its components
        rope = false;
        for (int dj = 0; dj < joints.Length; dj++)
        {
            Destroy(joints[dj]);
        }

        segmentPos = new Vector3[0];
        joints = new GameObject[0];
        segments = 0;

        ropeEnd.transform.localPosition = ropeEndPosition;
        Destroy(ropeEnd.GetComponent<HingeJoint2D>());
        ropeEnd.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }
}