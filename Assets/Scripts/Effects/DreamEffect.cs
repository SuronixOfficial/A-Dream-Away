using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

public class DreamEffect : Singleton<DreamEffect>
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private Animator _dreamEffectAnim;
    [SerializeField] private float _exitTimeBeforeVideoEnds = 0.1f;

    public void EnterEffect(VideoClip dreamClip)
    {
        InputReader.Instance.SetDisablePlayerInput(true);

        _videoPlayer.clip = dreamClip;
        _dreamEffectAnim.SetTrigger("FadeIn");

        StartCoroutine(ExitEffect(dreamClip.length));
    }

    private IEnumerator ExitEffect(double videoLength)
    {
        yield return new WaitForSeconds((float)videoLength - _exitTimeBeforeVideoEnds);

        _dreamEffectAnim.SetTrigger("FadeOut");
        InputReader.Instance.SetDisablePlayerInput(false);
    }
}
