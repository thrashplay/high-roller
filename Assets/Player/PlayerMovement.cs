using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DirectionKey
{
    Down,
    Left,
    Right,
    Up
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _body;

    [SerializeField]
    private float _force = 10;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var position = new Vector3(0, 1, 0);

        // _body.AddForceAtPosition(_force * GetMovementVector(), position, ForceMode.Force);

        if (IsPressing(new KeyCode[] { KeyCode.S }))
        {
            _body.AddForceAtPosition(new Vector3(-_force, 0, 0), position, ForceMode.Force);
        }
        else if (IsPressing(new KeyCode[] { KeyCode.F }))
        {
            _body.AddForceAtPosition(new Vector3(_force, 0, 0), position, ForceMode.Force);
        }
        
        if (IsPressing(new KeyCode[] { KeyCode.E }))
        {
            _body.AddForceAtPosition(new Vector3(0, 0, _force), position, ForceMode.Force);
        }        
        else if (IsPressing(new KeyCode[] { KeyCode.D }))
        {
            _body.AddForceAtPosition(new Vector3(0, 0, -_force), position, ForceMode.Force);
        }
    }

    private Vector3 GetMovementVector()
    {
        return Vector3.zero;
    }

    private bool IsPressed(DirectionKey direction)
    {
        return direction switch {
            DirectionKey.Down => IsPressing(new KeyCode[] { KeyCode.D }),
            DirectionKey.Left => IsPressing(new KeyCode[] { KeyCode.S }),
            DirectionKey.Right => IsPressing(new KeyCode[] { KeyCode.F }),
            DirectionKey.Up => IsPressing(new KeyCode[] { KeyCode.E }),
            _ => false
        };
    }

    private bool IsPressing(KeyCode[] keyCodes)
    {
        return keyCodes.Any(keyCode => Input.GetKey(keyCode));
    }
}
