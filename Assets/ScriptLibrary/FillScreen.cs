using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillScreen : MonoBehaviour
{
    void Start()
    {
        transform.parent = Camera.main.transform;
    }

    void Update()
    {
        var aspect = (float) Screen.width / Screen.height;
        var height = Camera.main.orthographicSize * 2F;
        var width = height * aspect;
        transform.localScale = new Vector3(width, height, 1);
    }
}
