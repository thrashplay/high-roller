using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "ScriptableObjects/Triggers/Game Object", order = 2)]
public class TriggerWithGameObject : ParameterizedTrigger<GameObject> { }