using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ResetProgress : MonoBehaviour { 
	public string journeyResetText = "twisted minds have been put to rest...";
    private bool touching = false;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = false;
    }

    public void PlayerInteract()
    {
        if (touching)
        {
            PlayerPrefs.DeleteAll();
			GetComponentInChildren<TextMesh> ().text = journeyResetText;
            _gameManager.UpdateLevelLoaders();
        }
    }
}