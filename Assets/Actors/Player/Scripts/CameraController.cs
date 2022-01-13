using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float maxPanSpeed = 3F;

    [SerializeField]
    private float minPanDelta = 0;

    [SerializeField]
    private float _minDistance = 15;

    [SerializeField]
    private float _yPosition = 0F;

    private float _targetX = 0;

    private float _targetZ = 0;

    private float _panX = 0;

    private float _panZ = 0;

    void Update()
    {
        var doubleMinDistance = _minDistance * 2;
        var playerProgress = CalculateProgress(transform.position);

        CalculateCameraPan();
        SetCameraPosition(playerProgress + doubleMinDistance);

        // var cameraPosition = CalculateProgress(Camera.main.transform.position);
        // if (cameraPosition < playerProgress + doubleMinDistance) {
        //     SetCameraPosition(playerProgress + doubleMinDistance);
        // } else if (cameraPosition > playerProgress - doubleMinDistance) {
        //     SetCameraPosition(playerProgress + doubleMinDistance);
        // }
    }

    private void SetCameraPosition(float position)
    {
        Camera.main.transform.position = new Vector3(-position / 2 - _panX, _yPosition, -position / 2 - _panZ);
    }

    // progression is an aggregate of how far down the track a position is
    private float CalculateProgress(Vector3 position)
    {
        var x = Mathf.Abs(position.x);
        var z = Mathf.Abs(position.z);
        return x + z;
    }

    private void CalculateCameraPan()
    {
        var x = Mathf.Abs(transform.position.x);
        var z = Mathf.Abs(transform.position.z);

        _targetX = Mathf.Max(0, x - z);
        _targetZ = Mathf.Max(0, z - x);

        var deltaX = _targetX - _panX;
        var deltaZ = _targetZ - _panZ;

        if (deltaX >= minPanDelta || deltaZ >= minPanDelta) {
            _panX += Mathf.Min(deltaX, maxPanSpeed) * Time.deltaTime;
            _panZ += Mathf.Min(deltaZ, maxPanSpeed) * Time.deltaTime;
        }

    }
}
