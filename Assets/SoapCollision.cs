using UnityEngine;

public class SoapCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude;
        if (collisionForce > 1)
        {
            //Debug.Log(collisionForce);
            AkSoundEngine.SetRTPCValue("Collision_Velocity", collisionForce);
            if (collision.gameObject.CompareTag("block"))
            {
                AkSoundEngine.PostEvent("Play_Physics_soapToBlock", gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent("Play_Physics_soapToFloor", gameObject);
            }
        }
    }
}
