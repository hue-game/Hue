using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Move))]

public class LevelSelectionPlayer : MonoBehaviour
{
	[HideInInspector]
	public GameObject onRope;
	private Move _moveScript;

	private void Awake()
	{
		_moveScript = GetComponent<Move>();
		onRope = null;
	}

	// FixedUpdate is called once per frame after physics have applied
	private void FixedUpdate()
	{
		_moveScript.MoveCharacter();
	}

	//Check for key inputs every frame
	private void Update()
	{
		#if UNITY_STANDALONE || UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			_moveScript.MoveLeft();
		if (Input.GetKeyUp(KeyCode.LeftArrow))
			_moveScript.MoveLeft();
		if (Input.GetKeyDown(KeyCode.RightArrow))
			_moveScript.MoveRight();
		if (Input.GetKeyUp(KeyCode.RightArrow))
			_moveScript.MoveRight();
		#endif
	} 
}




