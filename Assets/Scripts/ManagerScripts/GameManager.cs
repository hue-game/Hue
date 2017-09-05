using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public float levelStartDelay = 2f;

	void Start() {
	}

    void Update()
    {
    }

	public void StartLevel() {
		SceneManager.LoadSceneAsync ("Level1");
	}

	void RestartLevel() {
	}

	void StopLevel() {
	}
}
