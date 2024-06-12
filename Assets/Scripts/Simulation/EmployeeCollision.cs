using UnityEngine;
using UnityEngine.Events;

public class EmployeeCollision : MonoBehaviour
{
    public static UnityEvent collisionEvent = new();
    private bool IsFirstGroundCollision = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            collisionEvent.Invoke();

            float collisionForce = collision.impulse.magnitude;
            if (IsFirstGroundCollision)
            {
                AkSoundEngine.SetRTPCValue("Collision_Velocity", collisionForce);
                IsFirstGroundCollision = false;
                if (TryGetComponent(out ForceStand forceStand))
                    forceStand.enabled = false;
                if (TryGetComponent(out FollowPath followPath))
                    followPath.IsBroken = true;
                if (TryGetComponent(out AlignToCamera alignToCamera))
                    alignToCamera.enabled = false;
            }
            if (collisionForce > 5)
            {
                AkSoundEngine.PostEvent("Play_Physics_employeeCollision", gameObject);
                Debug.Log(collisionForce);
            }
        }
    }
}
