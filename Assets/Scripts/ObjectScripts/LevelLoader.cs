using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {
    public GameManager gameManager;
	public string levelToLoad;
	public bool shouldInteract;

	void LoadLevel(string levelToLoad) {
		SceneManager.LoadSceneAsync (levelToLoad);
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Player")
		{
			if (shouldInteract) {
				
			} else {
				this.LoadLevel (levelToLoad);
				// Load level
			}
        }
    }
}
