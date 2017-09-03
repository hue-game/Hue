using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    public PlayerController player;
    public float strength;

    void OnTriggerEnter2D(Collider2D hit)
    {
        //Check if the player has hit the bounce pad
        if (hit.name == "Player")
            player.Jump(strength);
    }
}    
