using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableController : MonoBehaviour {

    public bool canInteract;
    
    private void Awake()
    {
    }

    public void ToggleInteractable()
    {
        canInteract = !canInteract;
    }

    public abstract void Interact();
}
