using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class RollingRock : MonoBehaviour {

    public float killVelocity = 3;
    public Sprite dreamSprite;
    public Sprite nightmareSprite;

    private Rigidbody2D _rb;
    private Player _player;
    private WorldManager _worldManager;

    // Use this for initialization
    void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
        _worldManager = FindObjectOfType<WorldManager>();

        _worldManager.AddGameObject(gameObject);
        if (LayerMask.LayerToName(gameObject.layer) == "DreamWorld")
            _worldManager.UpdateWorldObject(gameObject, _worldManager.worldType);
        else if (LayerMask.LayerToName(gameObject.layer) == "NightmareWorld")
            _worldManager.UpdateWorldObject(gameObject, !_worldManager.worldType);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_rb.velocity.sqrMagnitude > 3)
                StartCoroutine(_player.KillPlayer());
        }
    }
}
