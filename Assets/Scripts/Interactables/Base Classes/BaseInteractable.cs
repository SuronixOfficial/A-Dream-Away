using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoClip _dreamVideo;

    public void Interact()
    {
        DreamEffect.Instance.EnterEffect(_dreamVideo);
        InputReader.Instance.SetDisablePlayerInput(true);

        Destroy(gameObject);
    }
}
