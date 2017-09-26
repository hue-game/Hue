using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
	// False is nightmare world, true is dream world.
    [HideInInspector]
	public bool worldType = false;
	private List<GameObject> nightmareWorldObjects = new List<GameObject>();
	private List<GameObject> dreamWorldObjects = new List<GameObject>();
    private IPlayer _player;
    private Rope[] _ropes;
    private SpawnSystem[] _spawnSystems;

    // Use this for initialization
    void Start () {
        //Get an array of all GameObjects in the scene
        _player = FindObjectOfType<IPlayer>();
        _ropes = FindObjectsOfType<Rope>();
        _spawnSystems = FindObjectsOfType<SpawnSystem>();
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

    public void AddGameObject(GameObject addObject)
    { 
        if (addObject.layer == LayerMask.NameToLayer("NightmareWorld"))
            nightmareWorldObjects.Add(addObject);
        else if (addObject.layer == LayerMask.NameToLayer("DreamWorld"))
            dreamWorldObjects.Add(addObject);
    }

    public void RemoveGameObject(GameObject removeObject)
    {
        if (removeObject.layer == LayerMask.NameToLayer("NightmareWorld"))
            nightmareWorldObjects.Remove(removeObject);
        else if (removeObject.layer == LayerMask.NameToLayer("DreamWorld"))
            dreamWorldObjects.Remove(removeObject);
    }

	public void SwitchWorld()
	{
		worldType = !worldType;
		UpdateWorld ();
	}

	public void UpdateWorld() {
		if (worldType) {
			foreach (GameObject nightmareObject in nightmareWorldObjects)
                UpdateWorldObject(nightmareObject, false);

			foreach (GameObject dreamObject in dreamWorldObjects)
                UpdateWorldObject(dreamObject, true);
		} 
		else {
			foreach (GameObject nightmareObject in nightmareWorldObjects)
                UpdateWorldObject(nightmareObject, true);

            foreach (GameObject dreamObject in dreamWorldObjects)
                UpdateWorldObject(dreamObject, false);
        }
	}

    public void UpdateWorldObject(GameObject worldObject, bool show)
    {
        float opacity = show ? 1.0f : 0.1f;
		if (worldObject.GetComponent<SpriteRenderer> () != null) 
        {
			if (worldObject.tag != "Background") 
            {
                //Change sprite of rolling rock
                if (worldObject.tag == "RollingRock")
                {
                    if (worldType)
                        worldObject.GetComponent<SpriteRenderer>().sprite = worldObject.GetComponent<RollingRock>().dreamSprite;
                    else
                        worldObject.GetComponent<SpriteRenderer>().sprite = worldObject.GetComponent<RollingRock>().nightmareSprite;
                }
                else
                {
                    //Show object from other world at lower opacity
                    Color woc = worldObject.GetComponent<SpriteRenderer>().color;
                    woc.a = opacity;
                    worldObject.GetComponent<SpriteRenderer>().color = woc;
                }
            } 
            else 
            {
                //Don't enable collectibles if found
                if (worldObject.GetComponent<Collectible>() != null)
                {
                    if (PlayerPrefs.GetInt(worldObject.name) == 1)
                        worldObject.SetActive(false);
                    else
                        worldObject.SetActive(show);
                }
                else
                    worldObject.SetActive(show);
            }
		} 
        else if (worldObject.tag == "Danger") 
			worldObject.SetActive (show);
        
   //     else if (worldObject.GetComponent<Collider2D>() != null)
			//worldObject.GetComponent<Collider2D>().enabled = show;
			
        if (worldObject.GetComponent<Rope>() != null)
        {
            worldObject.GetComponent<LineRenderer>().material.color = Color.white * opacity;
            foreach (Transform joint in worldObject.GetComponentInChildren<Transform>())
            {
                if (joint.GetComponent<Joint2D>() != null)
                {
                    if (_player.onRope == joint.gameObject)
                        joint.GetComponent<RopeSegment>().ExitRope();
					if (joint.GetComponent<Collider2D>() != null && joint.GetComponent<RopeSegment>() != null)
                        joint.GetComponent<RopeSegment>().TogglePlayerCollision(show);
                }
            }
        }

        if (worldObject.GetComponent<Collider2D>() != null && worldObject.tag != "RollingRock")
            worldObject.GetComponent<Collider2D>().enabled = show;
    }

    //public void ResetRopes()
    //{
    //    foreach (Rope rope in _ropes)
    //    {
    //        rope.DestroyRope();
    //        rope.BuildRope();
    //    }
    //}

    //public void ResetTriggers()
    //{
    //    foreach (SpawnTrigger trigger in _spawnTriggers)
    //    {
    //        trigger.ResetTrigger();
    //    }
    //}

}
