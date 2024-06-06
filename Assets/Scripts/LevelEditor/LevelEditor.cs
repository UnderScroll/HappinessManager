using Builder;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public partial class LevelEditor : MonoBehaviour
{
    [Flags]
    public enum AlignementLines
    {
        XAxis = 1 << 0,
        YAxis = 1 << 2,
        ZAxis = 1 << 3,
    }

    [Flags]
    public enum FillAxis
    {
        XAxis = 1 << 0,
        YAxis = 1 << 2,
        ZAxis = 1 << 3,
    }

    ///Custom Editor Bindings///
    //Level
    public Level Level;
    public Vector3Int StructureSize;
    public List<CellType> PlaceableBlocks;
    //CurrentCell
    public Vector3Int CurrentCellPosition;
    public CellType CurrentSelectedBlock;
    public AlignementLines alignementLines;
    //Utility
    public FillAxis AxisToFill;
    public bool DoReplace;
    //Additional
    public bool IsWindEnabled;
    public Vector3 WindDirection;
    public float WindStrength;
    ////////////////////////////

    public GameObject CurrentCellPreviewInstance;

    public void OnDrawGizmos()
    {
        //Draw CurrentCell
        if (CurrentCellPreviewInstance != null && CurrentCellPreviewInstance.GetComponent<MeshFilter>() != null)
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
        WindStrength = 0;
        WindDirection = new(0, 0, 0);
        IsWindEnabled = false;
        OnCurrenCellPositionChanged();
        PlaceableBlocks = new();
    }

    public void LoadLevel()
    {
        StructureSize = new(Level.Structure.Size.x, Level.Structure.Size.y, Level.Structure.Size.z);
        CurrentCellPosition = new(0, 0, 0);
        WindStrength = Level.WindStrength;
        WindDirection = Level.WindDirection;
        IsWindEnabled = Level.IsWindEnabled;
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
        PlaceCell(CurrentSelectedBlock, new(CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z));
    }

    public void PlaceCell(CellType cellType, int3 position)
    {
        if (cellType == null)
        {
            RemoveBlock(new int3(position.x, position.y, position.z));
            return;
        }

        Level.Structure.Cells[position.x, position.y, position.z] = (CellData)cellType;
        Level.Structure.Cells[position.x, position.y, position.z].Position = position;
        UpdateCell(position);

        Level.CellTypes[cellType.Name] = cellType;
    }

    public void OnRemoveBlock()
    {
        RemoveBlock(new int3(CurrentCellPosition.x, CurrentCellPosition.y, CurrentCellPosition.z));
    }

    public void RemoveBlock(int3 position)
    {
        if (Level.Structure.Cells[position.x, position.y, position.z] == null)
            return;

        CellType removedCellType = Level.Structure.Cells[position.x, position.y, position.z].Type;

        Level.Structure.Cells[position.x, position.y, position.z] = null;
        UpdateCell(new int3(position.x, position.y, position.z));

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

    public void OnFillButtonClicked()
    {
        Fill();
    }

    public void Fill()
    {
        int xStart = AxisToFill.HasFlag(FillAxis.XAxis) ? 0 : CurrentCellPosition.x;
        int xEnd = AxisToFill.HasFlag(FillAxis.XAxis) ? StructureSize.x : CurrentCellPosition.x + 1;
        int yStart = AxisToFill.HasFlag(FillAxis.YAxis) ? 0 : CurrentCellPosition.y;
        int yEnd = AxisToFill.HasFlag(FillAxis.YAxis) ? StructureSize.y : CurrentCellPosition.y + 1;
        int zStart = AxisToFill.HasFlag(FillAxis.ZAxis) ? 0 : CurrentCellPosition.z;
        int zEnd = AxisToFill.HasFlag(FillAxis.ZAxis) ? StructureSize.z : CurrentCellPosition.z + 1;

        for (int x = xStart; x < xEnd; x++)
            for (int y = yStart; y < yEnd; y++)
                for (int z = zStart; z < zEnd; z++)
                {
                    if (!DoReplace && Level.Structure.Cells[x, y, z] != null)
                        continue;

                    PlaceCell(CurrentSelectedBlock, new int3(x, y, z));
                }

        Initialize();
    }

    public void Save()
    {
        Level.WindDirection = IsWindEnabled ? WindDirection : Vector3.zero;
        Level.WindStrength = IsWindEnabled ? WindStrength : 0;

        UpdateCellTypeList();

        EditorUtility.SetDirty(Level);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif