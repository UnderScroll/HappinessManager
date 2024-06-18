using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCamera : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 direction = new Vector3(transform.position.x - _camera.transform.position.x, 0, transform.position.z - _camera.transform.position.z);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
