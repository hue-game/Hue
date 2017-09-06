using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour {

    public Text collectibleText;
    private Dictionary<GameObject, bool> _collectiblesFound = new Dictionary<GameObject, bool>();
    private int _collectiblesFoundFloat = 0;
	// Use this for initialization
	void Start () {
        foreach(Collectible collectible in FindObjectsOfType<Collectible>())
        {
            _collectiblesFound.Add(collectible.gameObject, false);
        }
        collectibleText.text = _collectiblesFoundFloat + "/" + _collectiblesFound.Count;
    }

    public void AddCollectible(GameObject collectible)
    {
        _collectiblesFound[collectible] = true;
        _collectiblesFoundFloat++;
        collectibleText.text = _collectiblesFoundFloat + "/" + _collectiblesFound.Count;
        //TODO: Save collectibles for this level.
    }
}
