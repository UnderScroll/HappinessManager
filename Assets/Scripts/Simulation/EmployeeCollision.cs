using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EmployeeCollision : MonoBehaviour
{
    public UnityEvent collisionEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
            collisionEvent.Invoke();
    }
}
