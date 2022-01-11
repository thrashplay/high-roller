using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PistonState {
    Extending,
    Idle,
    Retracting
}

public class Piston : MonoBehaviour
{
    // how fast the piston is moving
    public float Speed { get; set; }

    // what state the crusher is in
    public PistonState State { get; set; }

    private Rigidbody _faceRb;

    // the object at the face, or end, of the piston 
    [SerializeField]
    private GameObject _faceObject;

    // object to use as the shaft which "pushes" the face
    [SerializeField]
    private GameObject _shaftObject;

    public Piston() {
        State = PistonState.Idle;
    }

    public void SetLength(float length) {
        // _faceRb.MovePosition(new Vector3(transform.position.x, length, transform.position.z ));
        _faceRb.position = new Vector3(_faceRb.position.x, length, _faceRb.position.z);
    }

    private void Start() {
        _faceRb = _faceObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // _faceRb.MovePosition(new Vector3(transform.position.x, length, transform.position.z ));
        Debug.Log("sp:" + Speed);
        _faceRb.velocity = new Vector3(0, Speed, 0);

        var shaftPosition = _shaftObject.transform.position;
        var shaftScale = _shaftObject.transform.localScale;

        // _shaftObject.transform.localScale = new Vector3(shaftScale.x, Mathf.Max(0.01F, length), shaftScale.z);
        // _shaftObject.transform.position = new Vector3(shaftPosition.x, length / 2F - 0.01F, shaftPosition.z);

        // piston.transform.localScale = new Vector3(
        //     transform.localScale.x, 
        //     Mathf.Max(0F, Length) + 1, // default, non-extended size = 1 so we add that as a base
        //     transform.localScale.z
        // );
    }
}
