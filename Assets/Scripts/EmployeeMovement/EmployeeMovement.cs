using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEditor;
using UnityEngine;

public class EmployeeMovement : MonoBehaviour
{
    public ForceStand ForceStand;
    public FollowPath FollowPath;
    public AlignToCamera AlignToCamera;

    private void Awake()
    {
        if (ForceStand == null)
            Debug.LogError("ForceStand not found in EmployeeMovement");
        if (FollowPath == null)
            Debug.LogError("FollowPath not found in EmployeeMovement");
    }

    private void Update()
    {
        if (FollowPath.IsBroken)
        {
            ForceStand.enabled = false;
            AlignToCamera.enabled = false;
        }
    }
}
