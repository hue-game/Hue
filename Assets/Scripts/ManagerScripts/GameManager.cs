using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject controlCanvas;
    public GameObject player;

	// False is nightmare world, true is dream world.
	private bool worldType = false;
    private List<GameObject> nightmareWorldObjects = new List<GameObject>();
    private List<GameObject> dreamWorldObjects = new List<GameObject>();

    // Use this for initialization
    void Awake () {
        #if UNITY_ANDROID && !UNITY_EDITOR
        controlCanvas.SetActive(true);
        #endif
        Application.targetFrameRate = 60;

        //Get an array of all GameObjects in the scene
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in allGameObjects)
        {
            if(gameObject != null)
            {
                //Add objects with a blue or red layer (to be changed to gray and color) to their respective lists
                if (gameObject.layer == LayerMask.NameToLayer("WaterWorld"))
                    nightmareWorldObjects.Add(gameObject);
                else if (gameObject.layer == LayerMask.NameToLayer("FireWorld"))
                    dreamWorldObjects.Add(gameObject);
            }
        }

        //Start in blue world which will then be switched to red at start (change this to gray and color eventually)
        //Call SwitchColor at start to change the background color and correctly set the gameObjects for that world (assume you always start in the grey world)
        SwitchWorld();
	}

    public void Update()
    {
    }

    public void SwitchWorld()
	{
		worldType = !worldType;

		if (worldType) {
			player.GetComponent<SpriteRenderer>().color = Color.white;

			foreach (GameObject nightmareObject in nightmareWorldObjects)
			{
				nightmareObject.GetComponent<SpriteRenderer>().color = Color.white * 0.1f;
				if (nightmareObject.GetComponent<Collider2D> () != null) {
					nightmareObject.GetComponent<Collider2D> ().enabled = false;
				}
			}

			foreach (GameObject dreamObject in dreamWorldObjects)
			{
				dreamObject.GetComponent<SpriteRenderer>().color = Color.white * 1.0f;
				if (dreamObject.GetComponent<Collider2D> () != null) {
					dreamObject.GetComponent<Collider2D> ().enabled = true;
				}
			}
		} else {
			player.GetComponent<SpriteRenderer> ().color = Color.red;

			//For each object in the both worlds enable/disable the collider and change the opacity of the sprite
			foreach (GameObject nightmareObject in nightmareWorldObjects) {
				nightmareObject.GetComponent<SpriteRenderer> ().color = Color.white * 1.0f;

				if (nightmareObject.GetComponent<Collider2D> () != null) {
					nightmareObject.GetComponent<Collider2D> ().enabled = false;
				}
			}

			foreach (GameObject dreamObject in dreamWorldObjects) {
				dreamObject.GetComponent<SpriteRenderer> ().color = Color.white * 0.1f;

				if (dreamObject.GetComponent<Collider2D> () != null) {
					dreamObject.GetComponent<Collider2D> ().enabled = false;
				}
			}
		}
    }
}
