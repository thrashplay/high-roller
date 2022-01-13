using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody _body;

    private PlayerInputController _inputController;

    [SerializeField]
    private PlayerConfigModule playerConfig;

    // player state, used to determine if input is allowed or not
    [SerializeField]
    private PlayerState playerState;

    // the vector of the slope the player is on; (0, 0, 0) for flat terrain
    private Vector3 _slope;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _inputController = GetComponent<PlayerInputController>();
    }

    void FixedUpdate()
    {
        CheckTerrainContact();

        // apply movement forces requested by user input
        if (IsInputAllowed()) {
            ApplyUserInputBasedForces();
        }

        // apply special downward sloping and falling forces
        ApplySpecialGravity();

        var terrain = playerState.CurrentTerrain;
        if (terrain.VelocityMultiplier != 1) {
            _body.angularVelocity *= terrain.VelocityMultiplier;
            _body.velocity *= terrain.VelocityMultiplier;
        }

        if (playerState.Winning && !playerState.Falling) {
            _body.angularVelocity *= 0.95F;
            _body.velocity *= 0.95F;
        }

        if (_inputController.GetRequestedDirection() == Direction.None) {
            // if (playerState.MovementState.OnSlope) {
            //     _body.angularVelocity = Vector3.zero;
            // } else {
            _body.angularVelocity = _body.velocity.magnitude * _body.angularVelocity.normalized;
            // }
        } else {
            // do not let angular velocity get too low if the user is providing movement input
            _body.angularVelocity = Mathf.Max(0.8F * _body.maxAngularVelocity, _body.velocity.magnitude) * _body.angularVelocity.normalized;
        }
    }

    private bool IsInputAllowed() {
        return playerState.MovementState.OnGround && playerState.State switch {
            PlayerStateType.Winning => false,
            PlayerStateType.WinningWhileFalling => false,
            _ => true
        };
    }

    private void ApplyUserInputBasedForces() {
        var currentDirection = _inputController.GetRequestedDirection();

        if (currentDirection != Direction.None) {
            var position = new Vector3(0, 1, 0);
            var vector = _inputController.GetInputVector();

            // we apply only one of stationary boost, reverse boost or turn boost, favoring them in that order
            if (playerConfig.StationaryBoost > 0 && !IsMoving()) {
                _body.AddForceAtPosition(playerConfig.StationaryBoost * vector, position, ForceMode.Impulse);
            } else if (playerConfig.ReverseBoost > 0 && _inputController.IsReversed()) {
                _body.AddForceAtPosition(playerConfig.ReverseBoost * vector, position, ForceMode.Impulse);
            } else if (playerConfig.TurnBoost > 0 && _inputController.IsTurned()) {
                _body.AddForceAtPosition(playerConfig.TurnBoost * vector, position, ForceMode.Impulse);
            }

            _body.AddForceAtPosition(playerConfig.ContinuousForce * vector, position, ForceMode.Force);
        }
    }

    private void ApplySpecialGravity() {
        if (playerState.MovementState.OnSlope)
        {
            // apply downward "slide" boost
            _body.AddForce(_slope * playerConfig.SlopeSlideBoost, ForceMode.Acceleration);

            var currentDirection = _inputController.GetRequestedDirection();
            if (currentDirection != Direction.None) {
                var angle = Vector3.Angle(_slope, _inputController.GetInputVector());
                if (angle > 90) {
                    var magnitude = playerConfig.SlopeClimbAssist * ((angle - 90F) / 180F);
                    var direction = _inputController.GetInputVector();

                    if (playerConfig.Debug) 
                    {
                        Debug.DrawRay(transform.position, 10 * magnitude * direction, Color.yellow);
                        Debug.DrawRay(transform.position, _inputController.GetInputVector(), Color.blue);
                    }
                    _body.AddForce(magnitude * direction, ForceMode.Impulse); 
                }
            }
        } else if (playerState.MovementState == MovementState.Falling)
        {
            if (playerConfig.FreefallGravityMultiplier > 1)
            {
                _body.AddForce(Physics.gravity * (playerConfig.FreefallGravityMultiplier - 1), ForceMode.Acceleration); 
            }
        }
    }

    // use the player config's "stationary threshold" to determine if the player is moving
    private bool IsMoving()
    {
        return _body.velocity.magnitude >= playerConfig.StationaryThreshold;
    }

    private void CheckTerrainContact()
    {
        var hits = Physics.SphereCastAll(
            transform.position,
            playerConfig.GroundDetectionRadius,
            Vector3.down,
            playerConfig.GroundDetectionDistance
        );

        var terrainHits = hits.Where((hit) => hit.collider.CompareTag("Terrain"));

        if (terrainHits.Count() > 0)
        {
            var hit = terrainHits.First();
            var angle = Vector3.Angle(Vector3.up, hit.normal);
            var sloped = angle > playerConfig.SlopeThreshold;

            // https://forum.unity.com/threads/checking-for-terrains-slope-with-raycast-need-help.251063/#post-1659684
            var forward = Vector3.Cross(Vector3.up, hit.normal);
            var slope = Vector3.Cross(forward, hit.normal);

            if (playerConfig.Debug) 
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red);
                Debug.DrawRay(transform.position, _body.velocity * 0.5F, Color.green);

                Debug.DrawRay(transform.position, slope, Color.white);
            }

            if (sloped) {
                _slope = slope;
                playerState.MovementState = _body.velocity.y > 0 
                    ? MovementState.AscendingSlope 
                    : MovementState.DescendingSlope;
            } else {
                playerState.MovementState = MovementState.OnFlatTerrain;
                _slope = Vector3.zero;
            }
        }
        else
        {
            playerState.MovementState = MovementState.Falling;
            _slope = Vector3.zero;
        }
    }
}

public sealed class MovementState
{
    private MovementState() { }

    // moving up a slope
    public static MovementState AscendingSlope = new MovementState() {
        Ascending = true,
        Name = "AscendingSlope",
        OnGround = true,
        OnSlope = true,
    };

    // moving down a slope
    public static MovementState DescendingSlope = new MovementState() {
        Name = "DescendingSlope",
        OnGround = true,
        OnSlope = true,
    };

    public static MovementState Falling = new MovementState() {
        Name = "Falling",
        OnGround = false,
        OnSlope = false,
    };

    public static MovementState OnFlatTerrain = new MovementState() {
        Name = "OnFlatTerrain",
        OnGround = true,
        OnSlope = false,
    };

    public bool Ascending { get; private set; }

    // name of this state
    public string Name { get; private set; }

    // whether the state represents being on the ground
    public bool OnGround { get; private set; }

    // whether the state represents being on ramp-type terrain
    public bool OnSlope { get; private set; }
}