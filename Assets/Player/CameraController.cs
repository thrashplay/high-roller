using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _minDistance = 15;

    [SerializeField]
    private float _yPosition = 0F;
 
    void Update()
    {
        var playerProgress = CalculateProgress(transform.position);
        var cameraPosition = CalculateProgress(Camera.main.transform.position);

        if (cameraPosition < playerProgress + _minDistance) {
            SetCameraPosition(playerProgress + _minDistance);
        } else if (cameraPosition > playerProgress - _minDistance) {
            SetCameraPosition(playerProgress + _minDistance);
        }
    }

    private void SetCameraPosition(float position)
    {
        Camera.main.transform.position = new Vector3(-position / 2, _yPosition, -position / 2);
    }

    // progression is an aggregate of how far down the track a position is
    private float CalculateProgress(Vector3 position)
    {
        var x = Mathf.Abs(position.x);
        var z = Mathf.Abs(position.z);
        return x + z;
    }
}
