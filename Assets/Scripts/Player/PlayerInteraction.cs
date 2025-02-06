using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable _currentInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable)) return;

        interactable.Interact();
    }

    
}
