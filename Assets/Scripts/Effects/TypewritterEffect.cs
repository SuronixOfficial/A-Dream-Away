using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TypewriterEffect : Singleton<TypewriterEffect>
{
    [SerializeField] private float _delay = 0.1f;
    private string _currentText = "";

    private TMP_Text _textMeshProComponent;

    public Action OnEffectFinished;

    void Start()
    {
        _textMeshProComponent = GetComponent<TMP_Text>();
    }

    public void WriteText(string fullText)
    {
        StartCoroutine(ShowText(fullText));
    }
    public void ResetText()
    {
        _textMeshProComponent.text = string.Empty;
    }
    IEnumerator ShowText(string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            _currentText = fullText.Substring(0, i);
            _textMeshProComponent.text = _currentText;

            yield return new WaitForSeconds(_delay);
        }

        OnEffectFinished?.Invoke();
    }
}