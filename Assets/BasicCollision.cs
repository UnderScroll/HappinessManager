using UnityEngine;

public class BasicCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude;
        if (collisionForce > 3)
        {
            Debug.Log(collisionForce);
            AkSoundEngine.SetRTPCValue("Collision_Velocity", collisionForce);
            if (collision.gameObject.CompareTag("block"))
            {
                AkSoundEngine.PostEvent("Play_Physics_basicToBlock", gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent("Play_Physics_basicToFloor", gameObject);
            }
        }
    }
}
