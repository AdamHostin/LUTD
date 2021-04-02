using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacebla
{
    void SetTransparentSelf(GameObject transparentSelf);
    GameObject GetTransparentSelf();
    void SetCurrentTile(TileBehaviour tile);
    TileBehaviour GetCurrentTile();
    void SwitchToTile(TileBehaviour newTile);
}
