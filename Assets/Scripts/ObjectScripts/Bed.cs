using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour {

    public Image endFade;
    private GameObject _player;
    private GameManager _gameManager;

    // Use this for initialization
    void Start() {
        _player = FindObjectOfType<Player>().gameObject;
        _gameManager = FindObjectOfType<GameManager>();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _player)
        {
            _player.GetComponent<Rigidbody2D>().simulated = false;
            endFade.transform.parent.gameObject.SetActive(true);
            StartCoroutine(EndLevel());
        }
    }

    IEnumerator EndLevel()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / 4.0f);

            endFade.color = Color.black * t;
            yield return 0;
        }
        endFade.color = Color.black * 1.0f;

        yield return new WaitForSeconds(1.0f);

        _gameManager.StopLevel();

    }

}
