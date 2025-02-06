using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        InputReader.Instance.SetDisablePlayerInput(true);
    }
}
