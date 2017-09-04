using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Jump))]
public class BouncePad : MonoBehaviour
{
    public Jump jumpScript;
    public float strength;

    void OnTriggerEnter2D(Collider2D hit)
    {
        //Check if the player has hit the bounce pad
        if (hit.name == "Player")
            jumpScript.JumpUp(strength);
    }
}    
