using System.Collections;
using System.Collections.Generic;
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
