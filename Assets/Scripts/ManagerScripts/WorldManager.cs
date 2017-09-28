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

    // Use this for initialization
    void Start () {
        _player = FindObjectOfType<IPlayer>();
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

		foreach (GameObject gameObject in allGameObjects)
		{
			if(gameObject != null)
			{
				//Add objects with a nightmare or dream layer to their respective lists
				if (gameObject.layer == LayerMask.NameToLayer("NightmareWorld"))
					nightmareWorldObjects.Add(gameObject);
				else if (gameObject.layer == LayerMask.NameToLayer("DreamWorld"))
					dreamWorldObjects.Add(gameObject);
			}
		}

		UpdateWorld();
	}

    public bool GetGameObject(GameObject checkObject)
    {
        if (checkObject.layer == LayerMask.NameToLayer("NightmareWorld"))
            return nightmareWorldObjects.Contains(checkObject);
        else if (checkObject.layer == LayerMask.NameToLayer("DreamWorld"))
            return nightmareWorldObjects.Contains(checkObject);
        else
            return false;
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
		UpdateWorld();
	}

	public void UpdateWorld() {
		if (worldType) 
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DreamWorld"), LayerMask.NameToLayer("Player"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NightmareWorld"), LayerMask.NameToLayer("Player"));
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DreamWorld"), LayerMask.NameToLayer("Goomba"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NightmareWorld"), LayerMask.NameToLayer("Goomba"));
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DreamWorld"), LayerMask.NameToLayer("PlayerFeet"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NightmareWorld"), LayerMask.NameToLayer("PlayerFeet"));

            foreach (GameObject nightmareObject in nightmareWorldObjects)
                UpdateWorldObject(nightmareObject, false);

			foreach (GameObject dreamObject in dreamWorldObjects)
                UpdateWorldObject(dreamObject, true);
        }
        else 
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NightmareWorld"), LayerMask.NameToLayer("Player"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DreamWorld"), LayerMask.NameToLayer("Player"));
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NightmareWorld"), LayerMask.NameToLayer("Goomba"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DreamWorld"), LayerMask.NameToLayer("Goomba"));
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("NightmareWorld"), LayerMask.NameToLayer("PlayerFeet"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DreamWorld"), LayerMask.NameToLayer("PlayerFeet"));

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
                    {
                        worldObject.GetComponent<SpriteRenderer>().sprite = worldObject.GetComponent<RollingRock>().dreamSprite;
                        worldObject.layer = LayerMask.NameToLayer("DreamWorld");
                    }
                    else
                    {
                        worldObject.GetComponent<SpriteRenderer>().sprite = worldObject.GetComponent<RollingRock>().nightmareSprite;
                        worldObject.layer = LayerMask.NameToLayer("NightmareWorld");
                    }
                }
                else if(worldObject.GetComponent<Buzzard>() != null)
                {
                    if (worldType)
                    {
                        worldObject.GetComponent<Animator>().runtimeAnimatorController = worldObject.GetComponent<Buzzard>().dreamAnimator;
                        worldObject.layer = LayerMask.NameToLayer("DreamWorld");
                    }
                    else
                    {
                        worldObject.GetComponent<Animator>().runtimeAnimatorController = worldObject.GetComponent<Buzzard>().nightmareAnimator;
                        worldObject.layer = LayerMask.NameToLayer("NightmareWorld");
                    }
                }
                else if (worldObject.GetComponent<Feather>() != null)
                {
                    if (worldType)
                    {
                        worldObject.GetComponent<SpriteRenderer>().sprite = worldObject.GetComponent<Feather>().dreamFeather;
                        worldObject.layer = LayerMask.NameToLayer("DreamWorld");
                    }
                    else
                    {
                        worldObject.GetComponent<SpriteRenderer>().sprite = worldObject.GetComponent<Feather>().nightmareFeather;
                        worldObject.layer = LayerMask.NameToLayer("NightmareWorld");
                    }
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
			worldObject.SetActive(show);

        //Change the opacity of the line renderer for the rope
        if (worldObject.GetComponent<Rope>() != null)
            worldObject.GetComponent<LineRenderer>().material.color = Color.white * opacity;

        //Check if you are on a rope when switching, if so exit the rope
        if (_player.onRope == worldObject)
            worldObject.GetComponent<RopeSegment>().ExitRope();

        //if (worldObject.GetComponent<Goomba>() != null)
        //{
            
        //}
    }
}
