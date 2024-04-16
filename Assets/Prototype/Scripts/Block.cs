using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

abstract public class Block : MonoBehaviour
{
    public (uint x, uint y, uint z) position;
    public List<Block> connectedBlocks;
    public long groupId;

    public void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        connectedBlocks = new List<Block>();
        groupId = -1;
    }

    public abstract bool place(Structure structure, (uint x, uint y, uint z) position);
    public abstract bool remove(Structure structure);

    public abstract List<Cell> getNeighbors(Structure structure);
}
