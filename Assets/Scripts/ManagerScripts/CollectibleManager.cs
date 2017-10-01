using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour {

    public Text collectibleText;
    private Dictionary<GameObject, bool> _collectiblesFound = new Dictionary<GameObject, bool>();
    private int _collectiblesFoundFloat = 0;
    [HideInInspector]
    public int totalCollectiblesGlobal = 0;

    private WorldManager _worldManager;

	// Use this for initialization
	void Awake () {
        _worldManager = FindObjectOfType<WorldManager>();
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

        if (_collectiblesFound.Count == _collectiblesFoundFloat)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_completed", 1);
            PlayerPrefs.Save();
        }
    }

    public void AddCollectible(GameObject collectible)
    {
        _worldManager.RemoveGameObject(collectible.gameObject);

        _collectiblesFound[collectible] = true;
        _collectiblesFoundFloat++;
        totalCollectiblesGlobal++;
        collectibleText.text = _collectiblesFoundFloat + "/" + _collectiblesFound.Count;
        PlayerPrefs.SetInt(collectible.name, 1);
        PlayerPrefs.SetInt("totalCollectiblesGlobal", totalCollectiblesGlobal);
        if (_collectiblesFound.Count == _collectiblesFoundFloat)
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_completed", 1);
        PlayerPrefs.Save();
    }
}
