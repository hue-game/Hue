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

    private bool touching = false;

	void InitLevel() {
		levelTransitionCanvas.SetActive (true);
		StartCoroutine (this.Transition());
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && interactable)
            touching = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && interactable)
            touching = true;
    }

    void OnTriggerLeave2D(Collider2D other)
    {
        touching = false;
    }

    public void PlayerInteract()
    {
        if (touching && interactable)
            InitLevel();
    }


	IEnumerator Transition() {
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene (this.levelToLoad);
	}
}
