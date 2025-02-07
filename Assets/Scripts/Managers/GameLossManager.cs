using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLossManager : Singleton<GameLossManager>
{
    [SerializeField] private Animator _blackScreenTransition;

    public void Lose()
    {
        _blackScreenTransition.SetTrigger("FadeIn");
    }
}
