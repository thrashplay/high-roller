using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServiceLocator {
  T GetService<T>();

  void Register<T>(T serviceInstance);

  void Unregister<T>(T serviceInstance);
}

public sealed class ServiceLocator : IServiceLocator
{
    private static ServiceLocator _instance;
    public static IServiceLocator Instance 
    { 
      get {
        if (_instance == null) {
          _instance = new ServiceLocator();
        }

        return _instance;
      }
    } 

    private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    private ServiceLocator() {
      // true singleton
    }

    public void Register<T>(T serviceInstance)
    {
      _services[typeof(T)] = serviceInstance;
    }

    public void Unregister<T>(T serviceInstance)
    {
      _services.Remove(serviceInstance.GetType());
    }

    public T GetService<T>()
    {
      return (T) _services[typeof(T)];
    }

    public void Reset()
    {
      _services.Clear();
    }
}

