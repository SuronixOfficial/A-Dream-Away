using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.Events;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoClip _dreamVideo;
    [SerializeField] private string _playerDialogue;
    [SerializeField] private UnityEvent _interactionEvent;

    public void Interact()
    {
        _interactionEvent?.Invoke();

        DreamEffect.Instance.EnterEffect(_dreamVideo, _playerDialogue);

        Destroy(gameObject);
    }
}
