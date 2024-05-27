using Builder;
using LevelLoader;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public partial class LevelEditor : MonoBehaviour
{
    [Flags]
    public enum AlignementLines
    {
        XAxis = 1 << 0,
        YAxis = 1 << 2,
        ZAxis = 1 << 3,
    }

    ///Custom Editor Bindings///
    public Vector3Int CurrentCellPosition;
    public Vector3Int StructureSize;
    public List<CellType> PlaceableBlocks;
    public CellType CurrentSelectedBlock;
    public AlignementLines alignementLines;
    public Level Level;
    ////////////////////////////

    public GameObject CurrentCellPreviewInstance;

    public void OnDrawGizmos()
    {
        //Draw CurrentCell
        if (CurrentCellPreviewInstance != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh(CurrentCellPreviewInstance.GetComponent<MeshFilter>().sharedMesh, CurrentCellPreviewInstance.transform.position, CurrentCellPreviewInstance.transform.rotation, CurrentCellPreviewInstance.transform.localScale);
        }

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

        //Z Axis
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
    }

    public void OnCurrenCellPositionChanged()
    {
        if (CurrentCellPreviewInstance == null)
            return;

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

        if (CurrentSelectedBlock == null)
            return;

        CurrentCellPreviewInstance = Instantiate(CurrentSelectedBlock.Block, transform);

        CurrentCellPreviewInstance.transform.position = CurrentCellPosition;
    }

    public void OnLevelChanged()
    {
        if (Level == null)
        {
            Debug.LogWarning("Trying to load a null Level, creating a new one instead");
        }
        else
        {
            Debug.Log($"Loading {Level.name} for edition");
            UnloadLevel();
            LoadLevel();
        }
    }

    public void UnloadLevel()
    {
        Reset();
        StructureSize = new(1, 1, 1);
        CurrentCellPosition = new(0, 0, 0);
        OnCurrenCellPositionChanged();
        PlaceableBlocks = new();
    }

    public void LoadLevel()
    {
        StructureSize = new(Level.Structure.Size.x, Level.Structure.Size.y, Level.Structure.Size.z);
        CurrentCellPosition = new(0, 0, 0);
        OnCurrenCellPositionChanged();
        PlaceableBlocks = Level.PlaceableCellTypes.Get();
        Initialize();
    }

    public void OnLevelChangedSize(int3 newSize)
    {
        Reset();

        Structure newStructure = new(newSize);
        foreach (CellData cellData in Level.Structure.Cells)
        {
            if (cellData == null)
                continue;

            if (!(cellData.Position.x < newSize.x &&
                cellData.Position.y < newSize.y &&
                cellData.Position.z < newSize.z))
                continue;

            newStructure.Cells[cellData.Position.x, cellData.Position.y, cellData.Position.z] = cellData;
        }
        UnloadLevel();

        Level.Structure = newStructure;

        LoadLevel();

        Initialize();
    }

    public void OnPlaceBlock()
    {
        if (CurrentSelectedBlock == null)
            return;

        int3 cellPosition = new(CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z);
        Level.Structure.Cells[cellPosition.x, cellPosition.y, cellPosition.z] = new CellData(CurrentSelectedBlock) { Position = cellPosition };
        UpdateCell(cellPosition);

        Level.CellTypes[CurrentSelectedBlock.Name] = CurrentSelectedBlock;
    }

    public void OnRemoveBlock()
    {
        if (Level.Structure.Cells[CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z] == null)
            return;

        CellType removedCellType = Level.Structure.Cells[CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z].Type;

        Level.Structure.Cells[CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z] = null;
        UpdateCell(new int3(CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z));

        if (Level.CellTypes.TryGetValue(removedCellType.Name, out _))
            Level.CellTypes.Remove(removedCellType.Name);
    }

    public void UpdateCellTypeList()
    {
        if (Level == null)
            return;

        Level.CellTypes = new CellTypes();
        foreach (CellType cellType in PlaceableBlocks)
        {
            if (cellType == null) continue;

            Level.CellTypes[cellType.Name] = cellType;
        }

        if (Level.Structure == null)
            return;
        foreach (CellData cellData in Level.Structure.Cells)
        {
            if (cellData == null) continue;

            Level.CellTypes[cellData.Type.Name] = cellData.Type;
        }
    }

    public void OnSaveButtonClicked()
    {
        Debug.Log($"Saving level {Level.name}");
        Save();
    }

    public void Save()
    {
        UpdateCellTypeList();

        EditorUtility.SetDirty(Level);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
