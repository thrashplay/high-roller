using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    private readonly float _speed = 10F;

    void FixedUpdate()
    {
        if (IsPressing(KeyCode.S))
        {
           transform.Translate(Time.deltaTime * -_speed, 0, 0);
        }

        if (IsPressing(KeyCode.F))
        {
           transform.Translate(Time.deltaTime * _speed, 0, 0);
        }

        if (IsPressing(KeyCode.E))
        {
           transform.Translate(0, Time.deltaTime * _speed, Time.deltaTime * _speed);
        }

        if (IsPressing(KeyCode.D))
        {
           transform.Translate(0, Time.deltaTime * -_speed, Time.deltaTime * -_speed);
        }
    }

    private bool IsPressing(KeyCode keyCode)
    {
        return Input.GetKey(keyCode);
    }
}
