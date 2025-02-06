using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class LightningEffect : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private float _minDelay = 5f;
    [SerializeField] private float _maxDelay = 15f;
    [SerializeField] private float _baseExposure = 0f;
    [SerializeField] private float _maxFlash = 3f;
    [SerializeField] private float _flashDuration = 0.1f;
    [SerializeField] private float _fadeSpeed = 5f;
    [SerializeField] private float _flickerIntensity = 0.2f;
    [SerializeField] private float _flickerSpeed = 1f; 

    private ColorAdjustments _colorAdjustments;

    private void Start()
    {
        if (_volume.profile.TryGet(out _colorAdjustments))
        {
            StartCoroutine(LightningRoutine());
            StartCoroutine(FlickerRoutine());
        }
    }

    private IEnumerator LightningRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
            StartCoroutine(TriggerLightning());
        }
    }

    private IEnumerator TriggerLightning()
    {
        float intensity = Random.Range(_maxFlash * 0.5f, _maxFlash);
        int flashCount = Random.Range(1, 3);

        for (int i = 0; i < flashCount; i++)
        {
            _colorAdjustments.postExposure.value = intensity;
            yield return new WaitForSeconds(_flashDuration * Random.Range(0.8f, 1.2f));
            _colorAdjustments.postExposure.value = _baseExposure;

            if (flashCount > 1) yield return new WaitForSeconds(0.05f);
        }

        while (_colorAdjustments.postExposure.value > _baseExposure)
        {
            _colorAdjustments.postExposure.value = Mathf.Lerp(_colorAdjustments.postExposure.value, _baseExposure, _fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float flickerValue = Random.Range(_baseExposure - _flickerIntensity, _baseExposure + _flickerIntensity);
            _colorAdjustments.postExposure.value = Mathf.Lerp(_colorAdjustments.postExposure.value, flickerValue, _flickerSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
