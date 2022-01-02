using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerListener
{
    void OnTriggered();
}

[CreateAssetMenu(fileName = "Trigger", menuName = "ScriptableObjects/Trigger", order = 1)]
public class Trigger : ScriptableObject
{
    private readonly List<ITriggerListener> _listeners =  new List<ITriggerListener>();

    public void AddListener(ITriggerListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(ITriggerListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Emit()
    {
        _listeners.ForEach((listener) => listener.OnTriggered());
    }
}
