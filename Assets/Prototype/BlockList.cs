using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockList", menuName = "Structure/BlockList")]
public class BlockList : ScriptableObject
{
    [SerializeField]
    private List<BlockWrapper> blocks;

    [SerializeField]
    public PreviewBlock previewBlock;

    [Serializable]
    public struct BlockWrapper
    {
        public string id;
        public Block block;
    }

    public Block getBlock(string id)
    {
        BlockWrapper? bw = blocks.Find(x => x.id == id);
        return bw.HasValue ? bw.Value.block : null;
    }
}
