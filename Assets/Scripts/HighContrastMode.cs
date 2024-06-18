using UnityEngine;

public class HighContrastMode : MonoBehaviour
{
    private static Material _backgroundMaterial;

    public static Material BackgroundMaterial
    {
        get
        {
            if (_backgroundMaterial == null)
                _backgroundMaterial = Resources.Load<Material>("Materials/Skybox/Skybox_HighContrast");

            return _backgroundMaterial;
        }
        private set
        {
            _backgroundMaterial = value;
        }
    }

    public static bool Enabled { get; private set; }

    public static void SetEnable(bool value) => Enabled = value;
}
