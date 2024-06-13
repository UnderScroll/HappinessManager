using UnityEngine;

public class ConcreteCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude * 2;
        if (collisionForce > 2)
        {
            //Debug.Log(collisionForce);
            AkSoundEngine.SetRTPCValue("Collision_Velocity", collisionForce);
            if (collision.gameObject.CompareTag("block"))
            {
                AkSoundEngine.PostEvent("Play_Physics_concreteToBlock", gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent("Play_Physics_concreteToFloor", gameObject);
            }
        }
    }
}
