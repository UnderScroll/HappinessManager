using UnityEngine;

[CreateAssetMenu(fileName = "ConnectionType", menuName = "Structure/ConnectionType")]
public class ConnectionType : ScriptableObject
{
    public string Name = "Default";
    public float BreakForce = float.PositiveInfinity;
    public float BreakTorque = float.PositiveInfinity;
    private bool EnableCollision = false;
    private bool EnablePreprocessing = true;
}
