using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour {

    [Header("Properties")]
    public GameObject prefab;
    public bool alwaysOn = false;
    public bool triggerEnabled = true;
    public bool canTriggerAgain = false;

    [Space(5)]
    [Header("Location")]
    public Vector2 spawnPoint;
    public float radiusX = 0.0f;
    public float radiusY = 0.0f;

    [Space(5)]
    [Header("Spawnrate")]
    public float intervalBase = 0.0f;
    [Range(0, 100)]
    public int intervalRandomizePercentage = 0;
    public float duration = 0.0f;
    public float initialDelay = 0.0f;

    private bool _currentlyActive = false;
    private float _timeStart;
    private float _lastTrigger = 0.0f;
    private float _interval = 0.0f;

	// Use this for initialization
	void Start () {
        _interval = intervalBase;

        if (alwaysOn)
        {
            _currentlyActive = true;
            Invoke("SpawnObjectRepeat", initialDelay);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerEnabled && !_currentlyActive)
        {
            if (other.tag == "Player")
            {
                _currentlyActive = true;
                _timeStart = Time.time;
                if (!canTriggerAgain)
                    Invoke("SpawnObjectRepeat", initialDelay);
                else
                {
                    if (_lastTrigger == 0.0f || (Time.time >= (_lastTrigger + _interval)))
                        Invoke("SpawnObjectRepeat", initialDelay);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" && canTriggerAgain)
            _currentlyActive = false;
    }

    private void SpawnObjectRepeat()
    {
        if (_currentlyActive)
        {
            _interval = intervalBase * (1 - ((float) Random.Range(-intervalRandomizePercentage, intervalRandomizePercentage) / 100));

            Vector2 newSpawnPoint = spawnPoint + new Vector2(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY));
            GameObject instance = Instantiate(prefab, Vector2.zero, Quaternion.identity, transform);
            instance.transform.position = newSpawnPoint;
        }

        if (alwaysOn)
        {
            Invoke("SpawnObjectRepeat", _interval);
            return;
        }

        if ((Time.time < _timeStart + duration) && _currentlyActive && !canTriggerAgain)
            Invoke("SpawnObjectRepeat", _interval);
        else if ((canTriggerAgain && _currentlyActive))
        {
            _lastTrigger = Time.time;
            Invoke("SpawnObjectRepeat", _interval);
        }
        else 
        {
            _currentlyActive = false;
            triggerEnabled = false;
            _timeStart = 0.0f;
            if (canTriggerAgain)
                triggerEnabled = true;
        }

    }
}
