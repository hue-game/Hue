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
		Debug.Log (checkpoint.GetInstanceID());
		foreach (var c in checkpoint.GetComponents<Component> ()) {
//			Debug.Log (c);
		}

		if (!checkpoint.GetComponent<Checkpoint>()._CheckpointReached) {
//			print (" test");
			//checkpoint.GetComponent<Checkpoint>().CheckpointReached();
			//_checkpoints.Add(checkpoint);
		}
	}

	public GameObject GetLastCheckpoint() {
		return _checkpoints.Last();
	}
}

