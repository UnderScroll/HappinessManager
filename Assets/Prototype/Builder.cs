using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static BlockList;

public class Builder : MonoBehaviour
{
    Structure structure;
    [SerializeField]
    private PreviewBlock previewBlock;

    public BlockList blocks;

    void Awake()
    {
        structure = new Structure(10, 10, 10);
        Structure.previewBlock = previewBlock;

        Block block = blocks.getBlock("fixed_block");
        structure.placeBlock(block, 0, 0, 0);

    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            launchSim();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Preview")))
            {
                GameObject objectHit = hit.collider.gameObject;
                    PreviewBlock preview = objectHit.GetComponent<PreviewBlock>();
                    structure.placeBlock(blocks.getBlock("basic_block"), preview.position.x, preview.position.y, preview.position.z);
                    Destroy(objectHit);
            }
        }
    }

    private bool removeBlock(uint x, uint y, uint z)
    {
        if (!structure.isInBounds(x, y, z))
            return false;

        return false;
    }

    private void launchSim()
    {
        foreach (Cell c in structure.cells)
        {
            switch (c.type)
            {
                case Cell.Type.Full:
                case Cell.Type.Fixed:
                    c.block.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                default:
                    break;
            }
        }
    }
}