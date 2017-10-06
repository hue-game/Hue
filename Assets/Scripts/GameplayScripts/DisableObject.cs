using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour {

    public List<GameObject> objectsToDisable = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject disableObject in objectsToDisable)
                disableObject.SetActive(false);
        }
    }
}
