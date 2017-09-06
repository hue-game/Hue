using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour {

    private Dictionary<GameObject, bool> _collectiblesFound = new Dictionary<GameObject, bool>();
	// Use this for initialization
	void Start () {
        foreach(Collectible collectible in FindObjectsOfType<Collectible>())
        {
            _collectiblesFound.Add(collectible.gameObject, false);
        }
	}
	
    public void AddCollectible(GameObject collectible)
    {
        _collectiblesFound[collectible] = true;
        //TODO: Save collectibles for this level.
    }
}
