using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ConnectionType", menuName = "Structure/ConnectionType")]
public class ConnectionType : ScriptableObject
{
    public string Name = "Default";
    public float BreakForce = float.PositiveInfinity;
    public float BreakTorque = float.PositiveInfinity;
    public bool EnableCollision = false;
    public bool EnablePreprocessing = true;

    public bool isStronger(ConnectionType other)
    {
        return other.BreakForce < BreakForce;
    }
}
