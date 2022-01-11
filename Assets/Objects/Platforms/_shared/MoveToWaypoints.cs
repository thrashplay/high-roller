using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Waypoint {
    public Vector3 Target;
    public float MoveTime = 1;
    public float WaitTime = 0;
}

public class MoveToWaypoints : MonoBehaviour
{
    private readonly static float ARRIVAL_DELTA = 0.001F;

    [SerializeField]
    private Waypoint[] waypoints = {};

    private Vector3 _initialPosition;

    private Vector3 _movement = Vector3.zero;

    private int _waypointIndex;

    private Rigidbody _rigidbody;

    public Waypoint[] Waypoints {
        get { return waypoints; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
        SetNextWaypoint(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waypoints.Length < 1) {
            return;
        }

        if (AtWaypoint()) {
            SetNextWaypoint(_waypointIndex + 1);
        }

        _rigidbody.MovePosition(_rigidbody.position + _movement * Time.fixedDeltaTime);
    }

    private bool AtWaypoint() {
        var waypoint = waypoints[_waypointIndex];
        var targetGlobalPosition = waypoint.Target + _initialPosition;
        if (Mathf.Abs((targetGlobalPosition - transform.position).magnitude) < ARRIVAL_DELTA) {
            transform.position = targetGlobalPosition;
            return true;
        }

        return false;
    }

    private void SetNextWaypoint(int requestedIndex) {
        if (waypoints.Length < 1) {
            return;
        }

        _waypointIndex = requestedIndex % waypoints.Length;
        var waypoint = waypoints[_waypointIndex];
        var targetGlobalPosition = waypoint.Target + _initialPosition;
        _movement = (targetGlobalPosition - transform.position) / waypoint.MoveTime;
    }
}

// Displays lines of various thickness in the scene view
[CustomEditor(typeof(MoveToWaypoints))]
public class ExampleEditor : Editor
{
    public void OnSceneGUI()
    {
        var component = target as MoveToWaypoints;
        var waypoints = component.Waypoints;

        var validWaypoints = waypoints.Where(waypoint => waypoint.Target != null);

        if (validWaypoints.Count() > 0) {
            var originOffset = new Vector3(
                component.transform.localScale.x * 0.5F,
                component.transform.localScale.y * 0,
                component.transform.localScale.z * 0.5F
            );

            Handles.color = Color.yellow;
            Handles.DrawAAPolyLine(5, validWaypoints
                .Select(waypoint => waypoint.Target)
                .Prepend(Vector3.zero)
                .Select(target => target + component.transform.position + originOffset)
                .ToArray());
        }
    }
}