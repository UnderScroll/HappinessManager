using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private bool _holdRotate = false;
    private Camera _camera;
    private Vector2 _lastMousePos;
    [Header("Sensitivity")]
    [Range(0.1f, 1), Tooltip("The value that is set by the player's preference")]
    public float RotationSpeed = 0.0f;
    [Range(0, 5)]
    [Tooltip("Value set by the GD to tweak the range of the player's sensitivity preference")]
    public float RotationSpeedMultiplier;
    public float maxPitchAngle = 80;
    public float minPitchAngle = -20;

    private void Start()
    {
        _camera = Camera.main;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "This function is called by Unity Input System")]
    private void OnRotateDelta(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        if (_holdRotate && mouseDelta != null)
        {
            if (Mathf.Abs(mouseDelta.x) < Mathf.Abs(mouseDelta.y))
            {
                float cameraEulerXAngle = _camera.transform.rotation.eulerAngles.x;
                cameraEulerXAngle = cameraEulerXAngle > 180 ? cameraEulerXAngle - 360 : cameraEulerXAngle;

                if (mouseDelta.y > 0 && cameraEulerXAngle < maxPitchAngle ||
                    mouseDelta.y < 0 && cameraEulerXAngle > minPitchAngle)
                    _camera.transform.RotateAround(Vector3.zero, _camera.transform.right, mouseDelta.y * RotationSpeed * RotationSpeedMultiplier);
            }
            else
            {
                _camera.transform.RotateAround(Vector3.zero, Vector3.up, mouseDelta.x * RotationSpeed * RotationSpeedMultiplier);
            }
            Mouse.current.WarpCursorPosition(_lastMousePos);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "This function is called by Unity Input System")]
    private void OnRotateHold(InputValue value)
    {
        Cursor.visible = !value.isPressed;
        _lastMousePos = Mouse.current.position.value;
        _holdRotate = value.isPressed;
    }
}
