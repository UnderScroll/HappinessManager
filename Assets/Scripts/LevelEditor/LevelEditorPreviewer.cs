using Builder;
using LevelLoader;
using Unity.Mathematics;
using UnityEngine;

public partial class LevelEditor : MonoBehaviour
{
    public GameObject[,,] previewBlocks;

    public void Initialize()
    {
        Reset();

        if (Level.Structure == null)
        {
            Debug.LogError("Trying preview a level with null structure");
            return;
        }

        previewBlocks = new GameObject[Level.Structure.Size.x, Level.Structure.Size.y, Level.Structure.Size.z];

        foreach (CellData cellData in Level.Structure.Cells)
        {
            if (cellData == null)
                continue;

            GameObject blockInstance = Instantiate(cellData.Type.Block, transform);
            blockInstance.transform.position = new (cellData.Position.x, cellData.Position.y, cellData.Position.z);
            previewBlocks[cellData.Position.x, cellData.Position.y, cellData.Position.z] = blockInstance;
        }
    }

    public void Reset()
    {
        if (previewBlocks != null)
            foreach (GameObject block in previewBlocks)
                DestroyImmediate(block);
    }

    public void UpdateCell(int3 position)
    {
        GameObject instance = previewBlocks[position.x, position.y, position.z];
        if (instance != null)
            DestroyImmediate(instance);

        CellData newData = Level.Structure.Cells[position.x, position.y, position.z];
        if (newData == null)
            return;
        

        GameObject newInstance = Instantiate(newData.Type.Block, transform);
        newInstance.transform.position = CurrentCellPosition;
        previewBlocks[position.x, position.y, position.z] = newInstance;
    }
}