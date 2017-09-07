using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float strength = 15.0f;
    private Jump _jumpScript;

    void Start()
    {
        _jumpScript = FindObjectOfType<Player>().GetComponent<Jump>();
    }
    
    void OnTriggerEnter2D(Collider2D hit)
    {
        //Check if the player has hit the bounce pad
        if (hit.name == "Player")
            _jumpScript.JumpUp(strength);
    }
}    
