using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private bool _holdRotate = false;
    private Camera _camera;
    private Vector2 _lastMousePos;
    public float RotationSpeed = 10.0f;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnRotateDelta(InputValue value)
    {
        if (_holdRotate)
        {
            _camera.transform.RotateAround(Vector3.zero, Vector3.up, value.Get<float>() * Time.deltaTime * RotationSpeed);
            Mouse.current.WarpCursorPosition(_lastMousePos);
        }
    }

    private void OnRotateHold(InputValue value)
    {
        Cursor.visible = !value.isPressed;
        _lastMousePos = Mouse.current.position.value;
        _holdRotate = value.isPressed;
    }
}
