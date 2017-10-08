using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour {
	public float levelStartDelay = 2f;
    public Dictionary<string, int> levelRequirements = new Dictionary<string, int>();
    public Sprite[] checkboxes;
    public Image graphicsButton;
    public Image musicButton;

    private Collectible[] _collectibles;
    private LevelLoader[] _levelLoaders;
    private ResetProgress _resetProgress;
    private CheckpointManager _checkpointManager;
    private ViewTutorial _viewTutorial;
    private IPlayer _player;

	void Start() {
        _levelLoaders = FindObjectsOfType<LevelLoader>();
        _collectibles = FindObjectsOfType<Collectible>();
        _resetProgress = FindObjectOfType<ResetProgress>();
        _checkpointManager = FindObjectOfType<CheckpointManager>();
        _viewTutorial = FindObjectOfType<ViewTutorial>();
        _player = FindObjectOfType<IPlayer>();

        if (!PlayerPrefs.HasKey("totalCollectiblesGlobal"))
            PlayerPrefs.SetInt("totalCollectiblesGlobal", 0);
        if (!PlayerPrefs.HasKey("MusicOn"))
            PlayerPrefs.SetInt("MusicOn", 1);
        if (!PlayerPrefs.HasKey("GraphicsOn"))
            PlayerPrefs.SetInt("GraphicsOn", 1);
            
        if (PlayerPrefs.GetInt("GraphicsOn") == 0)
        {
            if (graphicsButton != null)
                graphicsButton.sprite = checkboxes[0];
            FindObjectOfType<PostProcessingBehaviour>().enabled = false;
        }

        if (PlayerPrefs.GetInt("MusicOn") == 0)
        {
            if (musicButton != null)
                musicButton.sprite = checkboxes[0];
            AudioListener.volume = 0.0f;
        }
  
        LoadLevelRequirements();
        if (SceneManager.GetActiveScene().name == "InteractiveMainMenu")
        {
            if (!PlayerPrefs.HasKey("FirstTime"))
            {
                PlayerPrefs.SetInt("FirstTime", 0);
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
        }
        else
            PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();

    }

    public void RestartLevel() {
        Time.timeScale = 1.0f;
        AudioSource[] audio = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audio.Length; i++)
            audio[i].pitch = 1.0f;
        PlayerPrefs.DeleteKey("CurrentLevel");
        PlayerPrefs.DeleteKey("CurrentCheckpoint");
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StopLevel() {
        Time.timeScale = 1.0f;
        AudioSource[] audio = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audio.Length; i++)
            audio[i].pitch = 1.0f;
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

    public void ToggleMusic()
    {
        PlayerPrefs.SetInt("MusicOn", 1 - PlayerPrefs.GetInt("MusicOn"));
        PlayerPrefs.Save();
        if (PlayerPrefs.GetInt("MusicOn") == 1)
        {
            AudioListener.volume = 1.0f;
            musicButton.sprite = checkboxes[1];
        }
        else
        {
            AudioListener.volume = 0.0f;
            musicButton.sprite = checkboxes[0];
        }
    }

    public void ToggleGraphics()
    {
        PlayerPrefs.SetInt("GraphicsOn", 1 - PlayerPrefs.GetInt("GraphicsOn"));
        PlayerPrefs.Save();
        if (PlayerPrefs.GetInt("GraphicsOn") == 1)
        {
            FindObjectOfType<PostProcessingBehaviour>().enabled = true;
            graphicsButton.sprite = checkboxes[1];
        }
        else
        {
            FindObjectOfType<PostProcessingBehaviour>().enabled = false;
            graphicsButton.sprite = checkboxes[0];
        }
    }

}

[System.Serializable]
public class LevelInfo
{
    public string levelName;
    public int collectiblesRequired;
}
