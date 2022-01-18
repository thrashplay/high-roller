using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFallListener {
    // Emitted when the player begins to fall
    void OnFalling();

    // Emitted when the player falls off a level
    void OnFellOffLevel();

    // Emitted when the player lands
    void OnLanded(ITerrainData terrain);

    // Emitted when the player lands after falling far enough to shatter
    void OnShattered();
}

public interface IFallEventEmitter {
    void AddFallListener(IFallListener listener);
    
    void RemoveFallListener(IFallListener listener);
}
