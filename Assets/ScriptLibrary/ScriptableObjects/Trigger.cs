using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerCallback();

[CreateAssetMenu(fileName = "Trigger", menuName = "ScriptableObjects/Triggers/Default", order = 1)]
public class Trigger : ScriptableObject
{
    private readonly List<TriggerCallback> _listeners =  new List<TriggerCallback>();

    public void AddListener(TriggerCallback listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(TriggerCallback listener)
    {
        _listeners.Remove(listener);
    }

    public void Emit()
    {
        _listeners.ForEach((listener) => listener());
    }
}
