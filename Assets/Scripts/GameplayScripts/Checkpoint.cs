using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Checkpoint : MonoBehaviour {
	public Sprite checkpointNotReachedSprite;
	public Sprite checkpointReachedSprite;
	private bool _checkpointReached;
	private SpriteRenderer _spriteRenderer;

	public bool _CheckpointReached {
		get {
			return _checkpointReached;
		}
	}

	// Use this for initialization
	void Awake () {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = checkpointNotReachedSprite;
	}

	public void CheckpointReached() {
		if (!_checkpointReached) {
			_checkpointReached = true;
            _spriteRenderer.sprite = checkpointReachedSprite;
        }
	}
}
