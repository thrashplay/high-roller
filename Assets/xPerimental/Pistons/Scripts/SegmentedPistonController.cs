using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PistonSegment {
    public float Duration;
    public float Length;
}

public class SegmentedPistonController : MonoBehaviour
{
    // the Piston being controlled
    [SerializeField]
    private Piston piston;

    [SerializeField]
    private PistonSegment[] segments = {};

    private int _currentSegmentIndex = 0;

    private float _currentSegmentTime = 0;

    private float _lengthStep = 0;

    private void Start() {
        _currentSegmentIndex = 0;
        _currentSegmentTime = 0;
    }

    void FixedUpdate()
    {
        AdvanceSegmentTime();

        piston.State = Mathf.Abs(_lengthStep) < float.Epsilon
            ? PistonState.Idle
            : _lengthStep < 0 ? PistonState.Retracting : PistonState.Extending;

        piston.Speed = _lengthStep;
    }

    private void AdvanceSegmentTime() {
        var previousSegment = segments[_currentSegmentIndex];
        _currentSegmentTime += Time.fixedDeltaTime;

        if (_currentSegmentTime >= previousSegment.Duration) {
            // set the exact intended end length, so the next segment starts at the current position
            piston.SetLength(previousSegment.Length);

            _currentSegmentIndex = (_currentSegmentIndex + 1) % segments.Length;
            var nextSegment = segments[_currentSegmentIndex];

            _lengthStep = (nextSegment.Length - piston.transform.position.y) / (nextSegment.Duration / Time.fixedDeltaTime);
            _currentSegmentTime = 0;
        }
    }
}
