using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceStand : MonoBehaviour
{
    public float StandForce;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
            Debug.LogError("The Rigidbody component was not found in ForceStand");
    }

    void FixedUpdate()
    {
        Quaternion rotation = _rigidbody.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));

        float angle = Quaternion.Angle(rotation, targetRotation);
        if (angle > 0)
        {
            Quaternion goToRotation = Quaternion.Lerp(rotation, targetRotation, StandForce * 1.0f / Mathf.Pow(angle, 2));

            _rigidbody.MoveRotation(goToRotation);
        }
    }
}
