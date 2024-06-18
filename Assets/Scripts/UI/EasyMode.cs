using UnityEngine;

public class EasyMode : MonoBehaviour
{
    public static bool Enabled { get; private set; }

    public static void OnToggleValueChanged(bool value) => Enabled = value;
}
