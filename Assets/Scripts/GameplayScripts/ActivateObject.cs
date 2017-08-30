using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : InteractableController {

    public GameObject[] activateObjects;
    public bool[] disableObjects;

	// Use this for initialization
	void Start () {
		
	}

    public override void Interact()
    {
        for (int i = 0; i < activateObjects.Length; i++)
        {
            if (!disableObjects[i])
            {
                gameObject.SetActive(false);
            }
            else 
            {
                gameObject.SetActive(true);
            }
        }
    }
}
