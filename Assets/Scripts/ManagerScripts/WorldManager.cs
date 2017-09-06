using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
	// False is nightmare world, true is dream world.
	private bool worldType = false;
	private List<GameObject> nightmareWorldObjects = new List<GameObject>();
	private List<GameObject> dreamWorldObjects = new List<GameObject>();
    private Player _player;

	// Use this for initialization
	void Awake () {
        //Get an array of all GameObjects in the scene
        _player = FindObjectOfType<Player>();
		GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

		foreach (GameObject gameObject in allGameObjects)
		{
			if(gameObject != null)
			{
				//Add objects with a blue or red layer (to be changed to gray and color) to their respective lists
				if (gameObject.layer == LayerMask.NameToLayer("NightmareWorld"))
					nightmareWorldObjects.Add(gameObject);
				else if (gameObject.layer == LayerMask.NameToLayer("DreamWorld"))
					dreamWorldObjects.Add(gameObject);
			}
		}

		UpdateWorld ();
	}

	public void Update()
	{
	}

	public void SwitchWorld()
	{
		worldType = !worldType;
		UpdateWorld ();
	}

	public void UpdateWorld() {
		if (worldType) {
			foreach (GameObject nightmareObject in nightmareWorldObjects)
			{	
            	if (nightmareObject.GetComponent<SpriteRenderer>() != null) {
                	nightmareObject.GetComponent<SpriteRenderer>().color = Color.white * 0.1f;
            	}	

                if (nightmareObject.GetComponent<Rope>() != null)
                {
                    nightmareObject.GetComponent<LineRenderer>().material.color = Color.white * 0.1f;
                    foreach (Transform joint in nightmareObject.GetComponentInChildren<Transform>())
                    {
                        if (joint.GetComponent<Joint2D>() != null)
                        {
                            if (_player.onRope == joint.gameObject)
                                joint.GetComponent<RopeSegment>().ExitRope();
                            joint.GetComponent<Rigidbody2D>().simulated = false;
                        }
                    }
                }
                if (nightmareObject.GetComponent<Collider2D>() != null) {
					nightmareObject.GetComponent<Collider2D>().enabled = false;
				}
			}

			foreach (GameObject dreamObject in dreamWorldObjects)
			{
                if (dreamObject.GetComponent<SpriteRenderer>() != null) {
                    dreamObject.GetComponent<SpriteRenderer>().color = Color.white * 1.0f;
                }
                if (dreamObject.GetComponent<Rope>() != null)
                {
                    dreamObject.GetComponent<LineRenderer>().material.color = Color.white * 1.0f;
                    foreach (Transform joint in dreamObject.GetComponentInChildren<Transform>())
                    {
                        if (joint.GetComponent<Joint2D>() != null)
                        {
                            joint.GetComponent<Rigidbody2D>().simulated = true;
                        }
                    }
                }
                if (dreamObject.GetComponent<Collider2D>() != null) {
					dreamObject.GetComponent<Collider2D>().enabled = true;
				}
			}
		} 
		else {
			//For each object in the both worlds enable/disable the collider and change the opacity of the sprite
			foreach (GameObject nightmareObject in nightmareWorldObjects) {
                if (nightmareObject.GetComponent<SpriteRenderer>() != null) {
                    nightmareObject.GetComponent<SpriteRenderer>().color = Color.white * 1.0f;
                }
                if (nightmareObject.GetComponent<Rope>() != null)
                {
                    nightmareObject.GetComponent<LineRenderer>().material.color = Color.white * 1.0f;
                    foreach (Transform joint in nightmareObject.GetComponentInChildren<Transform>())
                    {
                        if (joint.GetComponent<Joint2D>() != null)
                        {
                            joint.GetComponent<Rigidbody2D>().simulated = true;
                        }
                    }
                }
                if (nightmareObject.GetComponent<Collider2D>() != null) {
					nightmareObject.GetComponent<Collider2D>().enabled = true;
				}
			}

			foreach (GameObject dreamObject in dreamWorldObjects) {
		        if (dreamObject.GetComponent<SpriteRenderer>() != null) {
                    dreamObject.GetComponent<SpriteRenderer>().color = Color.white * 0.1f;
                }
                if (dreamObject.GetComponent<Rope>() != null)
                {
                    dreamObject.GetComponent<LineRenderer>().material.color = Color.white * 0.1f;
                    foreach (Transform joint in dreamObject.GetComponentInChildren<Transform>())
                    {
                        if (joint.GetComponent<Joint2D>() != null)
                        {
                            if (_player.onRope == joint.gameObject)
                                joint.GetComponent<RopeSegment>().ExitRope();
                            joint.GetComponent<Rigidbody2D>().simulated = false;
                        }
                    }
                }
                if (dreamObject.GetComponent<Collider2D>() != null) {
					dreamObject.GetComponent<Collider2D>().enabled = false;
				}
			}
		}
	}
}
