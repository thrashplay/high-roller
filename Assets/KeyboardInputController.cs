using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyboardInputController : MonoBehaviour
{
    private Rigidbody _body;

    private readonly float _speed = 1F;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (IsPressing(KeyCode.LeftArrow))
        {
           transform.Translate(Time.deltaTime * -_speed, 0, 0);
        }

        if (IsPressing(KeyCode.RightArrow))
        {
           transform.Translate(Time.deltaTime * _speed, 0, 0);
        }

        if (IsPressing(KeyCode.UpArrow))
        {
           transform.Translate(0, 0, Time.deltaTime * -_speed);
        }

        if (IsPressing(KeyCode.RightArrow))
        {
           transform.Translate(0, 0, Time.deltaTime * _speed);
        }
    }

    private bool IsPressing(KeyCode keyCode)
    {
        return Input.GetKey(keyCode);
    }
}
