using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DefaultPlayerVals", order = 1)]
public class DefaultPlayerValues : ScriptableObject
{
    public int vaccines;
    public int coins;
    public int medkits;
    public int vaccineEffectivnes;
    public int medkitEffectivness;
}
