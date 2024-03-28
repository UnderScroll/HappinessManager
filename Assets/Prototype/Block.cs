using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

abstract public class Block : MonoBehaviour
{
    public BlockList blocks;
    public (uint x, uint y, uint z) position;

    public void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public abstract bool place(ref Structure structure, uint x, uint y, uint z);
    public abstract bool remove(ref Structure structure, uint x, uint y, uint z);
}
