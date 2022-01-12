using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvasController : MonoBehaviour
{
    // object to anchor the canvas to
    [SerializeField]
    private GameObject attachedTo;

    // how far above the attached object to display the canvas
    [SerializeField]
    private float attachDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (attachedTo != null) {
            transform.position = attachedTo.transform.position + new Vector3(0, attachDistance, 0);
        }
        transform.LookAt(Camera.main.transform);
    }
}
