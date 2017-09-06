using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
	public GameObject defaultCheckpoint;
	private List<GameObject> _checkpoints = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{
		SetNewCheckpoint (defaultCheckpoint);
	}

	public void SetNewCheckpoint(GameObject checkpoint) {
		if (!checkpoint.GetComponent<Checkpoint>()._CheckpointReached) {
			checkpoint.GetComponent<Checkpoint>().CheckpointReached();
			_checkpoints.Add(checkpoint);
		}
	}

	public GameObject GetLastCheckpoint() {
		return _checkpoints.Last();
	}
}

