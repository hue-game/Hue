using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class LevelLoader : MonoBehaviour {
	public GameObject levelTransitionCanvas;
	public string levelToLoad;
	public bool interactable = false;

	void InitLevel() {
		levelTransitionCanvas.SetActive (true);
		StartCoroutine (this.Transition());
	}


    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Player" && !this.interactable)
		{
			this.InitLevel ();
        }
    }
		
	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player" && this.interactable && Input.GetButtonDown("Jump")) {
			this.InitLevel();
		}
	}

	IEnumerator Transition() {
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene (this.levelToLoad);
	}
}
