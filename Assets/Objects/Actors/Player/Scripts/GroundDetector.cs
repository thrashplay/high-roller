using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundDetector : MonoBehaviour, IGroundDetector
{
    // flag indicating if the player is currently on the ground
    private bool _grounded = true;

    // emitted when the player leaves the ground
    [SerializeField]
    private Trigger _fallingTrigger;

    // emitted when the player lands on the ground; will be passed TerrainData for where they landed
    [SerializeField]
    private TriggerWithTerrainData _landedTrigger;

    [SerializeField]
    private PlayerConfigModule _playerConfig;

    // the tag to use to identify if an object is "terrain" or not
    [SerializeField]
    private string _terrainTag = "Terrain";

    public bool IsOnGround {
        get { return _grounded; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentlyGrounded = CheckAboveTerrain(out GameObject terrain);
        if (currentlyGrounded && !_grounded) {
            _landedTrigger.Emit(TerrainData.FromGameObject(terrain));
            _grounded = true;
        } else if (!currentlyGrounded && _grounded) {
            _fallingTrigger.Emit();
            _grounded = false;
        }
    }

    // returns true if the is object is "on" terrain, and sets the out parameter
    // to be the first piece of such terrain encountered if so
    private bool CheckAboveTerrain(out GameObject terrain) {
        var hits = Physics.SphereCastAll(
            transform.position,
            _playerConfig.GroundDetectionRadius,
            Vector3.down,
            _playerConfig.GroundDetectionDistance
        );

        if (_playerConfig.Debug) {
            var radius = _playerConfig.GroundDetectionRadius;
            
            // Debug.DrawLine(
            //     transform.TransformVector(new Vector3(radius, 0, 0)),
            //     transform.TransformVector(new Vector3(-radius, 0, 0)),
            //     Color.red
            // );

            // Debug.DrawLine(
            //     transform.TransformVector(new Vector3(0, 0, radius)),
            //     transform.TransformVector(new Vector3(0, 0, -radius)),
            //     Color.red
            // );

            // Debug.DrawRay(transform.position, Vector3.down * _playerConfig.GroundDetectionDistance, Color.red);
        }

        var terrainHits = hits.Where((hit) => hit.collider.CompareTag(_terrainTag));
        terrain = terrainHits.Count() < 1
            ? null
            : terrain = terrainHits.First().collider.gameObject;
        
        return terrainHits.Count() > 0;
    }
}
