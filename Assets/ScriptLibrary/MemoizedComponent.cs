using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Memoized cache of a game object component.
public class MemoizedComponent<T> where T : Component
{
    // instance ID of the cached component
    private int _instanceId;

    // the component
    private T _component;

    // Returns the component of type 'T' from the game object, or 'null' if there is no such
    // component. Repeated calls using the same GameObject will return a cached value with
    // no component lookup. If a new game object is provided, the cache will be invalidated. (i.e,
    // only a single value is cached.) Negative lookups (i.e. when the object does not have a
    // matching component), are not cached and will result in a lookup on each call.
    //
    // This class should be used for repeatedly accessing components game objects that only
    // infrequently change, and are expected to always have the corresponding component.
    public T Get(GameObject gameObject) {
        if (gameObject == null) {
            _component = null;
        } else if (_component == null || _instanceId != gameObject.GetInstanceID()) {
            _component = gameObject.GetComponent<T>();
            _instanceId = gameObject.GetInstanceID();
        }

        return _component;
    }
}
