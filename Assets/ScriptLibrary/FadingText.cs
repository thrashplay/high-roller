using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour
{
    // time to fade in seconds
    public float fadeTime = 1.0F;

    public int initialFontSize = 14;

    public int finalFontSize = 14;

    public string message = "";

    public Text textElement;

    private float _elapsedTime = 0F;

    void Start()
    {
        textElement.text = message;
        textElement.fontSize = initialFontSize;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > fadeTime)
        {
            Destroy(gameObject);
        } 
        else
        {
            var fadePercent = Mathf.Min(1.0F, _elapsedTime / fadeTime);

            var fontSize = initialFontSize + ((finalFontSize - initialFontSize) * fadePercent);
            var alpha = 1.0F - fadePercent;

            var color = textElement.color;
            textElement.color = new Color(color.r, color.g, color.b, alpha);
            textElement.fontSize = (int) fontSize;
        }
    }
}
