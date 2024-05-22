using LevelLoader;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public partial class LevelEditor : MonoBehaviour
{
    public Vector3Int CurrentCellPosition;
    public Vector3Int StructureSize;
    public List<CellType> PlaceableBlocks;
    public CellType CurrentSelectedBlock;
    public string levelName;
    public AlignementLines alignementLines;

    public Level Level;

    public GameObject CurrentCellPreviewInstance;

    [Flags]
    public enum AlignementLines
    {
        XAxis = 1 << 0,
        YAxis = 1 << 2,
        ZAxis = 1 << 3,
    }

    public void OnDrawGizmos()
    {
        //Draw CurrentCell
        Gizmos.color = Color.white;
        Gizmos.DrawWireMesh(CurrentCellPreviewInstance.GetComponent<MeshFilter>().sharedMesh, CurrentCellPreviewInstance.transform.position, CurrentCellPreviewInstance.transform.rotation, CurrentCellPreviewInstance.transform.localScale);

        //Draw alignment lines
        DrawAlignementLines();
    }

    private void DrawAlignementLines()
    {
        //X Axis
        if (alignementLines.HasFlag(AlignementLines.XAxis))
        {
            Gizmos.color = new Color(1, 0, 0);
            Vector3 beginXA = new(-0.5f, CurrentCellPosition.y - 0.5f, CurrentCellPosition.z - 0.5f);
            Vector3 endXA = new(StructureSize.x - 0.5f, CurrentCellPosition.y - 0.5f, CurrentCellPosition.z - 0.5f);
            Gizmos.DrawLine(beginXA, endXA);

            Vector3 beginXB = new(-0.5f, CurrentCellPosition.y - 0.5f, CurrentCellPosition.z + 0.5f);
            Vector3 endXB = new(StructureSize.x - 0.5f, CurrentCellPosition.y - 0.5f, CurrentCellPosition.z + 0.5f);
            Gizmos.DrawLine(beginXB, endXB);

            Vector3 beginXC = new(-0.5f, CurrentCellPosition.y + 0.5f, CurrentCellPosition.z - 0.5f);
            Vector3 endXC = new(StructureSize.x - 0.5f, CurrentCellPosition.y + 0.5f, CurrentCellPosition.z - 0.5f);
            Gizmos.DrawLine(beginXC, endXC);

            Vector3 beginXD = new(-0.5f, CurrentCellPosition.y + 0.5f, CurrentCellPosition.z + 0.5f);
            Vector3 endXD = new(StructureSize.x - 0.5f, CurrentCellPosition.y + 0.5f, CurrentCellPosition.z + 0.5f);
            Gizmos.DrawLine(beginXD, endXD);
        }

        //Y Axis
        if (alignementLines.HasFlag(AlignementLines.YAxis))
        {
            Gizmos.color = new Color(0, 1, 0);
            Vector3 beginYA = new(CurrentCellPosition.x - 0.5f, -0.5f, CurrentCellPosition.z - 0.5f);
            Vector3 endYA = new(CurrentCellPosition.x - 0.5f, StructureSize.y - 0.5f, CurrentCellPosition.z - 0.5f);
            Gizmos.DrawLine(beginYA, endYA);

            Vector3 beginYB = new(CurrentCellPosition.x - 0.5f, -0.5f, CurrentCellPosition.z + 0.5f);
            Vector3 endYB = new(CurrentCellPosition.x - 0.5f, StructureSize.y - 0.5f, CurrentCellPosition.z + 0.5f);
            Gizmos.DrawLine(beginYB, endYB);

            Vector3 beginYC = new(CurrentCellPosition.x + 0.5f, -0.5f, CurrentCellPosition.z - 0.5f);
            Vector3 endYC = new(CurrentCellPosition.x + 0.5f, StructureSize.y - 0.5f, CurrentCellPosition.z - 0.5f);
            Gizmos.DrawLine(beginYC, endYC);

            Vector3 beginYD = new(CurrentCellPosition.x + 0.5f, -0.5f, CurrentCellPosition.z + 0.5f);
            Vector3 endYD = new(CurrentCellPosition.x + 0.5f, StructureSize.y - 0.5f, CurrentCellPosition.z + 0.5f);
            Gizmos.DrawLine(beginYD, endYD);
        }

        //Y Axis
        if (alignementLines.HasFlag(AlignementLines.ZAxis))
        {
            Gizmos.color = new Color(0, 0, 1);
            Vector3 beginZA = new(CurrentCellPosition.x - 0.5f, CurrentCellPosition.y - 0.5f, -0.5f);
            Vector3 endZA = new(CurrentCellPosition.x - 0.5f, CurrentCellPosition.y - 0.5f, StructureSize.z - 0.5f);
            Gizmos.DrawLine(beginZA, endZA);

            Vector3 beginZB = new(CurrentCellPosition.x - 0.5f, CurrentCellPosition.y + 0.5f, -0.5f);
            Vector3 endZB = new(CurrentCellPosition.x - 0.5f, CurrentCellPosition.y + 0.5f, StructureSize.z - 0.5f);
            Gizmos.DrawLine(beginZB, endZB);

            Vector3 beginZC = new(CurrentCellPosition.x + 0.5f, CurrentCellPosition.y - 0.5f, -0.5f);
            Vector3 endZC = new(CurrentCellPosition.x + 0.5f, CurrentCellPosition.y - 0.5f, StructureSize.z - 0.5f);
            Gizmos.DrawLine(beginZC, endZC);

            Vector3 beginZD = new(CurrentCellPosition.x + 0.5f, CurrentCellPosition.y + 0.5f, -0.5f);
            Vector3 endZD = new(CurrentCellPosition.x + 0.5f, CurrentCellPosition.y + 0.5f, StructureSize.z - 0.5f);
            Gizmos.DrawLine(beginZD, endZD);
        }
    }

    public void OnCurrenCellPositionChanged()
    {
        CurrentCellPreviewInstance.transform.position = CurrentCellPosition;
    }

    public void OnNorthButtonClicked()
    {
        int nextCurrentCellPositionZ = CurrentCellPosition.z + 1;
        if (nextCurrentCellPositionZ < StructureSize.z)
            CurrentCellPosition.z = nextCurrentCellPositionZ;

        OnCurrenCellPositionChanged();
        SceneView.RepaintAll();
    }

    public void OnEastButtonClicked()
    {
        int nextCurrentCellPositionX = CurrentCellPosition.x + 1;
        if (nextCurrentCellPositionX < StructureSize.x)
            CurrentCellPosition.x = nextCurrentCellPositionX;

        OnCurrenCellPositionChanged();
        SceneView.RepaintAll();
    }

    public void OnSouthButtonClicked()
    {
        int nextCurrentCellPositionZ = CurrentCellPosition.z - 1;
        if (nextCurrentCellPositionZ >= 0)
            CurrentCellPosition.z = nextCurrentCellPositionZ;

        OnCurrenCellPositionChanged();
        SceneView.RepaintAll();
    }

    public void OnWestButtonClicked()
    {
        int nextCurrentCellPositionX = CurrentCellPosition.x - 1;
        if (nextCurrentCellPositionX >= 0)
            CurrentCellPosition.x = nextCurrentCellPositionX;

        OnCurrenCellPositionChanged();
        SceneView.RepaintAll();
    }

    public void OnUpButtonClicked()
    {
        int nextCurrentCellPositionY = CurrentCellPosition.y + 1;
        if (nextCurrentCellPositionY < StructureSize.y)
            CurrentCellPosition.y = nextCurrentCellPositionY;

        OnCurrenCellPositionChanged();
        SceneView.RepaintAll();
    }

    public void OnDownButtonClicked()
    {
        int nextCurrentCellPositionY = CurrentCellPosition.y - 1;
        if (nextCurrentCellPositionY >= 0)
            CurrentCellPosition.y = nextCurrentCellPositionY;

        OnCurrenCellPositionChanged();
        SceneView.RepaintAll();
    }

    public void OnSelectedBlockChanged()
    {
        if (CurrentCellPreviewInstance != null)
            DestroyImmediate(CurrentCellPreviewInstance);

        CurrentCellPreviewInstance = Instantiate(CurrentSelectedBlock.Block);

        CurrentCellPreviewInstance.transform.position = CurrentCellPosition;
    }

    public void OnLevelChanged(Level newLevel)
    {
        Load(newLevel);
    }

    public void OnPlaceBlock()
    {

    }

    public void OnRemoveBlock()
    {

    }
}
