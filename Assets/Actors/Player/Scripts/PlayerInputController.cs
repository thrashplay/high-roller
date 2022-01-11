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

public enum Direction {
    East,
    Northeast,
    North,
    Northwest,
    West,
    Southwest,
    South,
    Southeast,
    None
}

public class PlayerInputController : MonoBehaviour
{
    // the most recent direction the player moved in
    private Direction _direction;

    private void LateUpdate() {
        _direction = GetRequestedDirection();
    }

    // retrieves the normalized vector for the direction corresponding to the user's input
    // if there is no user input, a zero vector is returned instead
    public Vector3 GetInputVector()
    {
        var direction = GetRequestedDirection();
        if (direction == Direction.None) {
            return Vector3.zero;
        }

        var vector = direction switch {
            Direction.East => new Vector3(1, 0, 0),
            Direction.Northeast => new Vector3(1, 0, 1),
            Direction.North => new Vector3(0, 0, 1),
            Direction.Northwest => new Vector3(-1, 0, 1),
            Direction.West => new Vector3(-1, 0, 0),
            Direction.Southwest => new Vector3(-1, 0, -1),
            Direction.South => new Vector3(0, 0, -1),
            Direction.Southeast => new Vector3(1, 0, -1),
            _ => Vector3.zero,
        };

        var mapRotation = Camera.main.transform.eulerAngles.y;
        var result = mapRotation == 0 ? vector : Quaternion.AngleAxis(mapRotation, Vector3.up) * vector;
        return result.normalized;
    }

    public Direction GetRequestedDirection()
    {
        if (IsPressed(DirectionKey.Right))
        {
            if (IsPressed(DirectionKey.Up))
            {
                return Direction.Northeast;
            }
            else if (IsPressed(DirectionKey.Down))
            {
                return Direction.Southeast;
            }
            else
            {
                return Direction.East;
            }
        }
        else if (IsPressed(DirectionKey.Left))
        {
            if (IsPressed(DirectionKey.Up))
            {
                return Direction.Northwest;
            }
            else if (IsPressed(DirectionKey.Down))
            {
                return Direction.Southwest;
            }
            else
            {
                return Direction.West;
            }
        }
        else if (IsPressed(DirectionKey.Up))
        {
            return Direction.North;
        }
        else if (IsPressed(DirectionKey.Down))
        {
            return Direction.South;
        }

        return Direction.None;
    }

    // returns true if the user has changed direction this frame
    public bool IsTurned() {
        var currentDirection = GetRequestedDirection();
        return _direction != currentDirection && _direction != Direction.None && currentDirection != Direction.None;
    }

    // returns true if the direction has been reversed on either the left/right or up/down axis
    public bool IsReversed() {
        var currentDirection = GetRequestedDirection();
        return IsReversedVertical(_direction, currentDirection) || IsReversedHorizontal(_direction, currentDirection);
    }

    private bool IsReversedVertical(Direction oldDirection, Direction currentDirection) {
        switch (oldDirection)
        {
            case Direction.Northeast:
            case Direction.North:
            case Direction.Northwest:
                return currentDirection == Direction.Southeast || currentDirection == Direction.South || currentDirection == Direction.Southwest;

            case Direction.Southeast:
            case Direction.South:
            case Direction.Southwest:
                return currentDirection == Direction.Northeast || currentDirection == Direction.North || currentDirection == Direction.Northwest;

            default:
                return false;
        }
    }

    private bool IsReversedHorizontal(Direction oldDirection, Direction currentDirection) {
        switch (oldDirection)
        {
            case Direction.Northeast:
            case Direction.East:
            case Direction.Southeast:
                return currentDirection == Direction.Northwest || currentDirection == Direction.West || currentDirection == Direction.Southwest;

            case Direction.Northwest:
            case Direction.West:
            case Direction.Southwest:
                return currentDirection == Direction.Northeast || currentDirection == Direction.East || currentDirection == Direction.Southeast;

            default:
                return false;
        }
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
