using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JointTypes", menuName = "Structure/JointTypes")]

public class JointTypes : ScriptableObject
{
    public List<JointType> types;
}
