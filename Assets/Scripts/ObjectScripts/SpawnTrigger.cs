using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour {

    public bool triggerEnabled = true;
    public float interval = 0.0f;
    public GameObject prefab;
    public Vector2 spawnPoint;
    public float radius = 0.0f;
    public float duration = 0.0f;
    public float delay = 0.0f;
    public bool canTriggerAgain = false;

    private bool _currentlyActive = false;
    private float _timeActive = 0.0f;
    private float _lastTrigger;


	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (_currentlyActive)
        {
            if (_timeActive > duration)
            {
                _currentlyActive = false;
                triggerEnabled = false;
                _timeActive = 0.0f;
                CancelInvoke("SpawnObject");
                if (canTriggerAgain)
                    triggerEnabled = true;
            }
            else 
            {
                _timeActive += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerEnabled && !_currentlyActive)
        {
            if (other.GetComponent<IPlayer>() != null)
            {
                if (interval != 0 && !canTriggerAgain)
                {
                    _currentlyActive = true;
                    InvokeRepeating("SpawnObject", delay, interval);
                }
                else
                {
                    if (delay > 0)
                        Invoke("SpawnObject", delay);
                    else
                        SpawnObject();
                    if (!canTriggerAgain)
                        _currentlyActive = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        print(_lastTrigger + "," + Time.time);
        if (triggerEnabled && !_currentlyActive && canTriggerAgain && ((_lastTrigger + interval) < Time.time))
        {
            if (other.GetComponent<IPlayer>() != null)
            {
                SpawnObject();
            }
        }
    }

    private void SpawnObject()
    {
        if (canTriggerAgain)
            _lastTrigger = Time.time;
        if (radius > 0.0)
            spawnPoint += Random.insideUnitCircle * radius;

        GameObject instance = Instantiate(prefab, Vector2.zero, Quaternion.identity, transform);
        instance.transform.position = spawnPoint;
    }

    public void ResetTrigger()
    {
        triggerEnabled = true;
        _currentlyActive = false;
        _timeActive = 0;
        CancelInvoke("SpawnObject");

        foreach (Transform child in GetComponentInChildren<Transform>())
            Destroy(child.gameObject);
    }
}
