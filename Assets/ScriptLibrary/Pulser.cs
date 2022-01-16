using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulser : MonoBehaviour
{   
    [SerializeField] 
    private float _pulseMagnitude = 0.15F;

    [SerializeField] 
    private float _pulseDuration = 2F;

    private bool _isPulsing;

    private void Update() {
        if (!_isPulsing) {
            StartCoroutine(LerpPulse(1 + _pulseMagnitude));
        }
    }

    IEnumerator LerpPulse(float targetScale)
    {
        _isPulsing = true;

        var baseScale = transform.localScale;

        float timeElapsed = 0;
        while (timeElapsed < _pulseDuration)
        {
            var value = Mathf.Lerp(1, targetScale, timeElapsed / _pulseDuration);
            transform.localScale = value * baseScale;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        while (timeElapsed < 2 * _pulseDuration)
        {
            var value = Mathf.Lerp(targetScale, 1, (timeElapsed - _pulseDuration) / _pulseDuration);
            transform.localScale = value * baseScale;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = baseScale;

        _isPulsing = false;
    }
}
