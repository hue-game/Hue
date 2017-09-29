using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {
	public float levelStartDelay = 2f;
    public Dictionary<string, int> levelRequirements = new Dictionary<string, int>();

    private LevelLoader[] _levelLoaders;
    private ResetProgress _resetProgress;
    private CheckpointManager _checkpointManager;
    private ViewTutorial _viewTutorial;

	void Start() {
        if (!PlayerPrefs.HasKey("totalCollectiblesGlobal"))
        {
            PlayerPrefs.SetInt("totalCollectiblesGlobal", 0);
            PlayerPrefs.Save();
        }

        _levelLoaders = FindObjectsOfType<LevelLoader>();
        _resetProgress = FindObjectOfType<ResetProgress>();
        _checkpointManager = FindObjectOfType<CheckpointManager>();
        _viewTutorial = FindObjectOfType<ViewTutorial>();
    
        LoadLevelRequirements();
        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            if (!PlayerPrefs.HasKey("FirstTime"))
            {
                PlayerPrefs.SetInt("FirstTime", 0);
                PlayerPrefs.Save();
                _viewTutorial.ShowTutorial();
            }
            UpdateLevelLoaders();
        }

        if (_checkpointManager == null)
        {
            PlayerPrefs.DeleteKey("CurrentLevel");
            PlayerPrefs.DeleteKey("CurrentCheckpoint");
            PlayerPrefs.Save();
        }
    }

	public void RestartLevel() {
        Time.timeScale = 1.0f;
        PlayerPrefs.DeleteKey("CurrentLevel");
        PlayerPrefs.DeleteKey("CurrentCheckpoint");
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StopLevel() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene ("InteractiveMainMenu");
	}

    public void PlayerInteract()
    {
        foreach(LevelLoader levelLoader in _levelLoaders)
            levelLoader.PlayerInteract();

        if (_resetProgress != null)
            _resetProgress.PlayerInteract();

        if (_viewTutorial != null)
            _viewTutorial.PlayerInteract();
    }

    private void LoadLevelRequirements()
    {
        string json = Resources.Load<TextAsset>("LevelRequirements").text;
        LevelInfo[] levelData = JsonHelper.FromJson<LevelInfo>(json);
            foreach (LevelInfo level in levelData)
                levelRequirements.Add(level.levelName, level.collectiblesRequired);
    }

    public void UpdateLevelLoaders()
    {
        foreach (LevelLoader levelLoader in _levelLoaders)
        {
            if (levelLoader.GetComponentInChildren<TextMesh>() != null)
            {
                int remaining = levelRequirements[levelLoader.levelToLoad] - PlayerPrefs.GetInt("totalCollectiblesGlobal");
                if (remaining > 0)
                    levelLoader.GetComponentInChildren<TextMesh>().text = remaining.ToString() + " more twisted minds";
                else
                    levelLoader.GetComponentInChildren<TextMesh>().text = "interact";

                if (PlayerPrefs.GetInt(levelLoader.levelToLoad + "_completed") == 1)
                    levelLoader.transform.GetChild(1).gameObject.SetActive(true);
                else
                    levelLoader.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class LevelInfo
{
    public string levelName;
    public int collectiblesRequired;
}
