using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "ScriptableObjects/Triggers/Terrain Data", order = 3)]
public class TriggerWithTerrainData : ParameterizedTrigger<ITerrainData> { }