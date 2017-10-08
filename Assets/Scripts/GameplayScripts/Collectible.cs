using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    private bool touching = false;
    private CollectibleManager _collectibleManager;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private Player _player;
    private Rigidbody2D _playerRB;
    private Animator _playerAnimator;

    void Start()
    {
        _collectibleManager = FindObjectOfType<CollectibleManager>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _playerRB = _player.GetComponent<Rigidbody2D>();
        _playerAnimator = _player.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            touching = false;
    }


    public void PlayerInteract()
    {
        if (touching)
        {
            _collectibleManager.AddCollectible(gameObject);
            StartCoroutine(RemoveCollectible());
        }
    }

    IEnumerator RemoveCollectible()
    {
        float t = 0.0f;
        GetComponent<Collider2D>().enabled = false;
        _animator.SetTrigger("Scared");
        _playerAnimator.SetBool("Scare", true);
        _playerRB.simulated = false;

        _player.isScaring = true;
        GetComponent<AudioSource>().Play();
        _player.GetComponents<AudioSource>()[2].Play();

        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / 1.6f);

            _sprite.color = Color.white * (1.0f - t);
            yield return 0;
        }

        _playerAnimator.SetBool("Scare", false);
        _player.isScaring = false;
        _playerRB.simulated = true;
        gameObject.SetActive(false);
    }

}
