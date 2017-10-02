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
    [HideInInspector]
    public Buzzard[] buzzards;
    public Rope[] ropes;

    private float opacity = 1f;

    // Use this for initialization
    void Start () {
        _player = FindObjectOfType<IPlayer>();
        ropes = FindObjectsOfType<Rope>();
        buzzards = FindObjectsOfType<Buzzard>();
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
        nightmareWorldObjects.RemoveAll(obj => obj == removeObject);
        dreamWorldObjects.RemoveAll(obj => obj == removeObject);
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

        foreach (Buzzard buzzard in buzzards)
            buzzard.ChangeBuzzardSprite();

        foreach (Rope rope in ropes)
        {
            if (worldType)
            {
                if (rope.gameObject.layer == LayerMask.NameToLayer("DreamWorld"))
                    rope.GetComponent<LineRenderer>().material.color = Color.white * 1.0f;
                else
                    rope.GetComponent<LineRenderer>().material.color = Color.white * 0.15f;
            }
            else
            {
                if (rope.gameObject.layer == LayerMask.NameToLayer("DreamWorld"))
                    rope.GetComponent<LineRenderer>().material.color = Color.white * 0.15f;
                else
                    rope.GetComponent<LineRenderer>().material.color = Color.white * 1.0f;
            }
        }

        if (_player.onRope != null)
            _player.onRope.GetComponent<RopeSegment>().ExitRope();

    }

    public void UpdateWorldObject(GameObject worldObject, bool show)
    {
        float opacity = show ? 1.0f : 0.15f;
        if (worldObject.GetComponent<SpriteRenderer>() != null) 
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
                //Change sprite of feather
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
                    SpriteRenderer spriteRenderer = worldObject.GetComponent<SpriteRenderer>();
                    Color woc = spriteRenderer.color;
                    woc.a = opacity;
                    spriteRenderer.color = woc;
                }
            } 
            else 
            {
                worldObject.SetActive(show);
            }
		}
        else if (worldObject.tag == "Danger")
            worldObject.SetActive(show);

    }
}
