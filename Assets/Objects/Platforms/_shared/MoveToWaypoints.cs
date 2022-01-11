using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint {
    public Vector3 Target;
    public float MoveTime = 1;
    public float WaitTime = 0;
}

public class MoveToWaypoints : MonoBehaviour
{
    [SerializeField]
    private Waypoint[] waypoints = {};

    private Vector3 _initialPosition;

    private Rigidbody _rigidbody;

    public Waypoint[] Waypoints {
        get { return waypoints; }
    }

    void Start()
    {
        _initialPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();

        if (waypoints.Length > 0) {
            StartCoroutine(LerpMove(0));
        }
    }


    IEnumerator LerpMove(int index)
    {
        Vector3 startPosition = _rigidbody.position;
        float timeElapsed = 0;
        Waypoint waypoint = waypoints[index];

        while (timeElapsed < waypoint.MoveTime)
        {
            var newPosition = Vector3.Lerp(startPosition, waypoint.Target + _initialPosition, timeElapsed / waypoint.MoveTime);
            _rigidbody.MovePosition(newPosition);
            timeElapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        transform.position = waypoint.Target + _initialPosition;

        if (waypoint.WaitTime > 0) {
            yield return new WaitForSeconds(waypoint.WaitTime);
        }
    
        yield return StartCoroutine(LerpMove((index + 1) % waypoints.Length));
    }
}
