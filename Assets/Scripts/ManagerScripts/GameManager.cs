﻿using System.Collections;
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
		SceneManager.LoadScene ("Level1");
	}

	public void RestartLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void StopLevel() {
		SceneManager.LoadScene ("MainMenu");
	}
}
