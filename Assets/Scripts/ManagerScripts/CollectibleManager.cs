using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour {

    public Text collectibleText;
    private Dictionary<GameObject, bool> _collectiblesFound = new Dictionary<GameObject, bool>();
    private int _collectiblesFoundFloat = 0;
    [HideInInspector]
    public int totalCollectiblesGlobal = 0;
	// Use this for initialization
	void Awake () {
        totalCollectiblesGlobal = PlayerPrefs.GetInt("totalCollectiblesGlobal");
        foreach(Collectible collectible in FindObjectsOfType<Collectible>())
        {
            if (PlayerPrefs.GetInt(collectible.name) == 1)
            {
                _collectiblesFound.Add(collectible.gameObject, true);
                _collectiblesFoundFloat++;
                collectible.gameObject.SetActive(false);
            }
            else
            {
                _collectiblesFound.Add(collectible.gameObject, false);
            }
        }
        collectibleText.text = _collectiblesFoundFloat + "/" + _collectiblesFound.Count;
    }

    public void AddCollectible(GameObject collectible)
    {
        _collectiblesFound[collectible] = true;
        _collectiblesFoundFloat++;
        totalCollectiblesGlobal++;
        collectibleText.text = _collectiblesFoundFloat + "/" + _collectiblesFound.Count;
        PlayerPrefs.SetInt(collectible.name, 1);
        PlayerPrefs.SetInt("totalCollectiblesGlobal", totalCollectiblesGlobal);
        PlayerPrefs.Save();
    }
}
