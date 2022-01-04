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

        _body.AddForceAtPosition(_force * GetMovementVector(), position, ForceMode.Force);
    }

    private Vector3 GetMovementVector()
    {
        var x = 0;
        var z = 0;
        if (IsPressed(DirectionKey.Right))
        {
            if (IsPressed(DirectionKey.Up))
            {
                x = 1;
                z = 1;
            }
            else if (IsPressed(DirectionKey.Down))
            {
                x = 1;
                z = -1;
            }
            else
            {
                x = 1;
                z = 0;
            }
        }
        else if (IsPressed(DirectionKey.Left))
        {
            if (IsPressed(DirectionKey.Up))
            {
                x = -1;
                z = 1;
            }
            else if (IsPressed(DirectionKey.Down))
            {
                x = -1;
                z = -1;            
            }
            else
            {
                x = -1;
                z = 0;            
            }
        }
        else if (IsPressed(DirectionKey.Up))
        {
            x = 0;
            z = 1;
        }
        else if (IsPressed(DirectionKey.Down))
        {
            x = 0;
            z = -1;        
        }

        var mapRotation = Camera.main.transform.eulerAngles.y;
        var vector = new Vector3(x, 0, z);
        return mapRotation == 0 ? vector : Quaternion.AngleAxis(mapRotation, Vector3.up) * vector;
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
