using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
	private bool _isPaused = false;
	public GameObject pauseUICanvas;
	public GameObject pauseOverlayCanvas;
	public GameObject controlCanvas;

	// Use this for initialization
	void Start () {
		pauseOverlayCanvas.SetActive (false);
		pauseUICanvas.SetActive (true);
        controlCanvas.SetActive(true);
    }

	public void Pause () {
		_isPaused = true;
		Time.timeScale = 0.0f;
		controlCanvas.SetActive(false);
		pauseUICanvas.SetActive (false);
		pauseOverlayCanvas.SetActive(true);

        AudioSource[] audio = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audio.Length; i++)
            audio[i].pitch = 0.0f;
    }

	public void Resume () {
		_isPaused = false;
		Time.timeScale = 1.0f;
		pauseOverlayCanvas.SetActive(false);
		pauseUICanvas.SetActive (true);
		controlCanvas.SetActive(true);

        AudioSource[] audio = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audio.Length; i++)
            audio[i].pitch = 1.0f;
    }

    void Update()
    { 
        if (Input.GetButtonDown("Cancel"))
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }  
    }

    void OnApplicationPause(bool pauseStatus)
    {
        #if !UNITY_EDITOR
        if (pauseStatus)
            Pause();
        #endif
    }

}
