using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

public class DreamEffect : Singleton<DreamEffect>
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private AnimationClip _dreamEffectFadeInAnimationClip;
    [SerializeField] private Animator _dreamEffectAnim;
    [SerializeField] private float _exitTimeBeforeVideoEnds = 0.1f;

    public void EnterEffect(VideoClip dreamClip,string playerDialogue)
    {
        TypewriterEffect.Instance.WriteText(playerDialogue);
        InputReader.Instance.SetDisablePlayerInput(true);

        TypewriterEffect.Instance.OnEffectFinished += StartTransition;

        _videoPlayer.clip = dreamClip;
    }

    private void StartTransition()
    {
        _dreamEffectAnim.SetTrigger("FadeIn");

        StartCoroutine(ExitEffect());

        TypewriterEffect.Instance.OnEffectFinished -= StartTransition;
    }
    private IEnumerator ExitEffect()
    {
        yield return new WaitForSeconds(_dreamEffectFadeInAnimationClip.length + (float)_videoPlayer.clip.length - _exitTimeBeforeVideoEnds);
        TypewriterEffect.Instance.ResetText();

        _dreamEffectAnim.SetTrigger("FadeOut");
        InputReader.Instance.SetDisablePlayerInput(false);
    }

    private void OnDestroy()
    {
        TypewriterEffect.Instance.OnEffectFinished -= StartTransition;
    }
}
