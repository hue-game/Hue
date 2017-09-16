using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class LevelLoader : MonoBehaviour {
	public GameObject levelTransitionCanvas;
	public string levelToLoad;

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
            if (levelToLoad != "InteractiveMainMenu")
            {
                if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
                {
                    if (PlayerPrefs.GetInt("totalCollectiblesGlobal") >= _gameManager.levelRequirements[levelToLoad])
                        InitLevel();
                }
                else
                {
                    if (PlayerPrefs.GetInt("totalCollectiblesGlobal") >= _gameManager.levelRequirements[levelToLoad])
                        levelToLoad = "InteractiveMainMenu";
                    InitLevel();
                }
            }
            else
            {
                InitLevel();
            }
        }
    }

    private void InitLevel()
    {
        GetComponent<Collider2D>().enabled = false;
        touching = false;
        FindObjectOfType<IPlayer>().GetComponent<Rigidbody2D>().simulated = false;
        levelTransitionCanvas.SetActive(true);
        StartCoroutine(this.Transition());
    }

    private IEnumerator Transition() {
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene (this.levelToLoad);
	}
}
