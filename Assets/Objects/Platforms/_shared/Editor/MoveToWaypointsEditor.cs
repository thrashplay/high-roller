using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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