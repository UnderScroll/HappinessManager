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

    public ConnectionType() { }

    public ConnectionType(ConnectionType other)
    {
        Name = other.Name;
        BreakForce = other.BreakForce;
        BreakTorque = other.BreakTorque;
        EnableCollision = other.EnableCollision;
        EnablePreprocessing = other.EnablePreprocessing;
    }

    public bool isStronger(ConnectionType other)
    {
        return other.BreakForce < BreakForce;
    }
}
