using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale.y / _renderer.material.GetTextureScale("_MainTex").y;
        var y = transform.position.y % scale;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, y / scale));
    }
}
