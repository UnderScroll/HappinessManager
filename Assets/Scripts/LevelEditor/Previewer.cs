using Builder;
using LevelLoader;
using UnityEngine;

public partial class LevelEditor : MonoBehaviour
{
    public GameObject[,,] previewBlocks;

    public void Load(Level level)
    {
        Unload();

        if (level.Structure == null)
        {
            Debug.LogError("Trying to load level with null Structure");
            return;
        }

        previewBlocks = new GameObject[level.Structure.Size.x, level.Structure.Size.y, level.Structure.Size.z];

        foreach (CellData cellData in level.Structure.Cells)
        {
            if (cellData == null)
                continue;

            GameObject blockInstance = Instantiate(cellData.Type.Block);
            previewBlocks[cellData.Position.x, cellData.Position.y, cellData.Position.z] = blockInstance;
        }
    }

    public void Unload()
    {
        if (previewBlocks != null)
            foreach (GameObject block in previewBlocks)
                DestroyImmediate(block);
    }
}