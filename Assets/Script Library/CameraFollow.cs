using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private string _tagToFollow;

    [SerializeField]
    private float _xOffset = 0F;

    [SerializeField]
    private float _yOffset = 0F;
 
    [SerializeField]
    private float _zOffset = 0F;

    void Update()
    {
        var objectToFollow = GameObject.FindGameObjectWithTag(_tagToFollow);
        var targetPosition = objectToFollow.transform.position;

        transform.position = new Vector3(
            targetPosition.x + _xOffset,
            targetPosition.y + _yOffset,
            targetPosition.z + _zOffset
        );  
    }
}
