using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public Vector3Int CurrentCellPosition;
    public Vector3Int StructureSize;

    public void OnTestButtonClicked()
    {
        Debug.Log(CurrentCellPosition);
    }

    public void OnDrawGizmos()
    {
    }
}
