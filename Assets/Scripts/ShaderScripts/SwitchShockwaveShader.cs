using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShockwaveShader : MonoBehaviour
{
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        transform.localScale = Vector3.zero;
    }


    void FixedUpdate()
    {
        transform.position = _player.transform.position;
        transform.localScale += new Vector3(0.06f, 0.06f, 0.06f);

        if (transform.localScale.x > 3f)
            Destroy(gameObject);

    }
}
