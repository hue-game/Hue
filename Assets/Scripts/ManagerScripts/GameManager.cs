using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {
	public float levelStartDelay = 2f;
    public Dictionary<string, int> levelRequirements = new Dictionary<string, int>();

    private Collectible[] _collectibles;
    private LevelLoader[] _levelLoaders;
    private ResetProgress _resetProgress;
    private CheckpointManager _checkpointManager;
    private ViewTutorial _viewTutorial;
    private IPlayer _player;

	void Start() {
        if (!PlayerPrefs.HasKey("totalCollectiblesGlobal"))
        {
            PlayerPrefs.SetInt("totalCollectiblesGlobal", 0);
            PlayerPrefs.Save();
        }

        _levelLoaders = FindObjectsOfType<LevelLoader>();
        _collectibles = FindObjectsOfType<Collectible>();
        _resetProgress = FindObjectOfType<ResetProgress>();
        _checkpointManager = FindObjectOfType<CheckpointManager>();
        _viewTutorial = FindObjectOfType<ViewTutorial>();
        _player = FindObjectOfType<IPlayer>();
    
        LoadLevelRequirements();
        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            if (!PlayerPrefs.HasKey("FirstTime"))
            {
                PlayerPrefs.SetInt("FirstTime", 0);
                PlayerPrefs.Save();
                _viewTutorial.ShowTutorial();
            }

            if (PlayerPrefs.HasKey("LastLevel"))
            {
                foreach (LevelLoader levelLoader in _levelLoaders)
                {
                    if (levelLoader.levelToLoad == PlayerPrefs.GetString("LastLevel"))
                    {
                        _player.transform.position = new Vector3(levelLoader.transform.position.x, levelLoader.transform.position.y, _player.transform.position.z) - new Vector3(1f, -0.4f, 0f);
                        FindObjectOfType<FollowPlayer>().ResetCamera();
                    }
                }
            }

            UpdateLevelLoaders();

            PlayerPrefs.DeleteKey("CurrentLevel");
            PlayerPrefs.DeleteKey("CurrentCheckpoint");
            PlayerPrefs.Save();
        }
        else
            PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
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
        foreach (Collectible collectible in _collectibles)
            collectible.PlayerInteract();
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
                    levelLoader.GetComponentInChildren<TextMesh>().text = levelLoader.GetComponentInChildren<TextMesh>().text + "\n" + remaining.ToString() + " more twisted minds";
                else
                    levelLoader.GetComponentInChildren<TextMesh>().text = levelLoader.GetComponentInChildren<TextMesh>().text + "\n" + "Interact";

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
