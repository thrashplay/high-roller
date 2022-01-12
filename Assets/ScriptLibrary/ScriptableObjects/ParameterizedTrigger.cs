using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ParameterizedTriggerCallback<T>(T argument);

[CreateAssetMenu(fileName = "ParameterizedTrigger", menuName = "ScriptableObjects/ParameterizedTrigger", order = 1)]
public class ParameterizedTrigger<T> : ScriptableObject
{
    private readonly List<ParameterizedTriggerCallback<T>> _listeners =  new List<ParameterizedTriggerCallback<T>>();

    public void AddListener(ParameterizedTriggerCallback<T> listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(ParameterizedTriggerCallback<T> listener)
    {
        _listeners.Remove(listener);
    }

    public void Emit(T argument)
    {
        _listeners.ForEach((listener) => listener(argument));
    }
}
