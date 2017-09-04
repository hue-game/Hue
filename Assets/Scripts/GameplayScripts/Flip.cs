using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flip : MonoBehaviour {

    public void FlipSprite(bool flipTrue)
    {
        GetComponent<SpriteRenderer>().flipX = flipTrue;
    }
}
