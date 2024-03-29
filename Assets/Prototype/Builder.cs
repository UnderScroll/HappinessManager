using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static BlockList;

public class Builder : MonoBehaviour
{
    Structure structure;
    [SerializeField]
    private PreviewBlock previewBlock;

    private PreviewBlock pointedPreview;
    private MeshRenderer pointedPreviewRenderer;

    public BlockList blocks;

    private int previewLayer;
    private int blockLayer;

    void Awake()
    {
        structure = new Structure(10, 10, 10);
        Structure.previewBlock = previewBlock;
        Structure.origin = transform;

        Block block = blocks.getBlock("fixed_block");
        structure.placeBlock(block, (0, 0, 0));
        Cell[] updatedCells = { structure.cells[0, 0, 0] };
        structure.updatePreviewBlocks(updatedCells);

        blockLayer = 1 << LayerMask.NameToLayer("Block");

        previewLayer = 1 << LayerMask.NameToLayer("Preview");
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            launchSim();
        }
        if (Input.GetKeyDown("escape"))
        {
            resetSim();
        }

        if (Input.GetKeyDown("left"))
        {
            Debug.Log("Test");
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, -10 * Time.deltaTime);
        }
        if (Input.GetKeyDown("right"))
        {
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, previewLayer))
        {
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.tag == "Preview")
                onPreviewBlockPointed(objectHit);
        }
        else
        {
            if (pointedPreviewRenderer != null)
                pointedPreviewRenderer.enabled = false;
            if (pointedPreview != null)
                pointedPreview = null;
        }

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, blockLayer))
        {
            GameObject objectHit = hit.collider.gameObject;
            onBlockPointed(objectHit);
        }
    }

    void onPreviewBlockPointed(GameObject objectHit)
    {
        if (pointedPreviewRenderer != null)
            pointedPreviewRenderer.enabled = false;

        pointedPreview = objectHit.GetComponent<PreviewBlock>();
        pointedPreviewRenderer = objectHit.GetComponent<MeshRenderer>();
        pointedPreviewRenderer.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            structure.placeBlock(blocks.getBlock("basic_block"), (pointedPreview.position.x, pointedPreview.position.y, pointedPreview.position.z));
            Cell[] updatedCells = { structure.cells[pointedPreview.position.x, pointedPreview.position.y, pointedPreview.position.z] };
            structure.updatePreviewBlocks(updatedCells);

            pointedPreview = null;
            Destroy(objectHit);
        }
    }

    void onBlockPointed(GameObject objectHit)
    {
        Block block = objectHit.GetComponent<Block>();

        if (Input.GetMouseButtonDown(1))
        {
            Cell[] updatedCells = { structure.cells[block.position.x, block.position.y, block.position.z] };
            structure.removeBlock(block.position);
            structure.updatePreviewBlocks(updatedCells);
        }
    }

    private void launchSim()
    {
        foreach (Cell c in structure.cells)
        {
            switch (c.type)
            {
                case Cell.Type.Full:
                    c.block.GetComponent<Rigidbody>().isKinematic = false;
                    break;
                case Cell.Type.Preview:
                    c.block.GetComponent<Collider>().enabled = false;
                    break;
                default:
                    break;
            }
        }
    }

    private void resetSim()
    {
        foreach (Cell c in structure.cells)
        {
            switch (c.type)
            {
                case Cell.Type.Full:
                    c.block.GetComponent<Rigidbody>().isKinematic = true;
                    c.block.transform.localPosition = new Vector3(c.block.position.x, c.block.position.y, c.block.position.z);
                    c.block.transform.rotation = Quaternion.identity;
                    break;
                case Cell.Type.Preview:
                    c.block.GetComponent<Collider>().enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}