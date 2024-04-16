using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JointType
{
    [Tooltip("A unique identifier")]
    public string name;

    [SerializeField, Min(0.001f)]
    [Tooltip("Orthogonal force the joint can withstand before breaking")]
    public float breakForce;
    [SerializeField, Min(0.001f)]
    [Tooltip("Angular force the joint can withstand before breaking")]
    public float breakTorque;

    [SerializeField]
    [Tooltip("Enables collision between the objects connected by the joint")]
    public bool enableCollsion;
}
