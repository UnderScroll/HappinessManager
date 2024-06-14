using UnityEngine;

public class BasicCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude * 4;
        //Debug.Log(collisionForce);
        if (collisionForce > 4)
        {
            //Debug.Log(collisionForce);
            AkSoundEngine.SetRTPCValue("Collision_Velocity", collisionForce);
            if (collision.gameObject.CompareTag("block"))
            {
                AkSoundEngine.PostEvent("Play_Physics_basicToBlock", gameObject);
                //Debug.Log(collisionForce);
               
            }
            else
            {
                AkSoundEngine.PostEvent("Play_Physics_basicToFloor", gameObject);
            }
        }
    }
}
