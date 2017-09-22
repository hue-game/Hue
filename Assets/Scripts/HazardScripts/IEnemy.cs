using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void Idle();

    public abstract void Patrol();

    public abstract void Attack();

    public abstract void Lost();

}
