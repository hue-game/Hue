using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    private CollectibleManager _collectibleManager;
    private SpriteRenderer _sprite;

    void Start()
    {
        _collectibleManager = FindObjectOfType<CollectibleManager>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Player")
        {
            _collectibleManager.AddCollectible(gameObject);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(RemoveCollectible());
        }
    }

    IEnumerator RemoveCollectible()
    {
        float t = 0.0f;

        while(t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / 2.0f);

            _sprite.color = Color.white * (1.0f - (t * 0.5f));
            yield return 0;
        }

        gameObject.SetActive(false);
    }

}
