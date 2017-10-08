using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public GameObject defaultCheckpoint;
    private List<GameObject> _checkpoints = new List<GameObject>();
    private FollowPlayer _followPlayer;

    // Use this for initialization
    void Start()
    {
        _followPlayer = FindObjectOfType<FollowPlayer>();

        if (PlayerPrefs.GetString("CurrentLevel") == SceneManager.GetActiveScene().name)
           defaultCheckpoint = GameObject.Find(PlayerPrefs.GetString("CurrentCheckpoint"));
        else
        {
            PlayerPrefs.DeleteKey("CurrentLevel");
            PlayerPrefs.DeleteKey("CurrentCheckpoint");
            PlayerPrefs.Save();
        }
        SetNewCheckpoint(defaultCheckpoint);
        transform.position = defaultCheckpoint.transform.position + new Vector3(0.5f, 0f, 0f);
        _followPlayer.ResetCamera();
    }

    public void SetNewCheckpoint(GameObject checkpoint)
    {
        if (!checkpoint.GetComponent<Checkpoint>()._CheckpointReached)
        {
            if (checkpoint != defaultCheckpoint)
                checkpoint.GetComponent<AudioSource>().Play();

            PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetString("CurrentCheckpoint", checkpoint.name);
            PlayerPrefs.Save();
            defaultCheckpoint = checkpoint;
            checkpoint.GetComponent<Checkpoint>().CheckpointReached();
            _checkpoints.Add(checkpoint);
        }
    }

    public GameObject GetLastCheckpoint()
    {
        return _checkpoints.Last();
    }
}