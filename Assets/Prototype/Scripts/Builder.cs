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
    public JointTypes joints;

    private int previewLayer;
    private int blockLayer;

    bool buildMode = true;
    float simulationTime = 0;

    void Awake()
    {
        structure = new Structure(10, 10, 10);
        Structure.previewBlock = previewBlock;
        Structure.origin = transform;

        blockLayer = 1 << LayerMask.NameToLayer("Block");
        previewLayer = 1 << LayerMask.NameToLayer("Preview");

        Block employeeBlock = blocks.getBlock("employee_block");
        structure.placeBlockAndUpdatePreview(employeeBlock, (3, 5, 8));
        structure.placeBlockAndUpdatePreview(employeeBlock, (8, 9, 1));

        Block fixedBlock = blocks.getBlock("fixed_block");
        structure.placeBlockAndUpdatePreview(fixedBlock, (3, 4, 8));
        structure.placeBlockAndUpdatePreview(fixedBlock, (8, 8, 1));
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

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Debug.LogAssertion("Forced breakpoint");
        }

        if (!buildMode)
        {
            bool dead = false;
            foreach (EmployeeBlock employee in EmployeeBlock.employees)
            {
                if (employee.isDead)
                    dead = true;
            }
            if (dead)
                Invoke("resetSim", 1);
            else
            {
                if (simulationTime > 5)
                    foreach (EmployeeBlock employee in EmployeeBlock.employees)
                        employee.GetComponent<MeshRenderer>().material.color = Color.green;
                else
                    simulationTime += Time.deltaTime;

            }
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
            structure.placeBlockAndUpdatePreview(
                blocks.getBlock("basic_block"),
                (pointedPreview.position.x, pointedPreview.position.y, pointedPreview.position.z));

            structure.connectBlockToNeighbors(structure.cells[pointedPreview.position.x, pointedPreview.position.y, pointedPreview.position.z].block);
            pointedPreview = null;
            Destroy(objectHit);
        }
    }

    void onBlockPointed(GameObject objectHit)
    {
        Block block = objectHit.GetComponent<Block>();

        if (Input.GetMouseButtonDown(1))
        {
            structure.removeBlockAndUpdatePreview(block.position);
        }
    }

    private void launchSim()
    {
        buildMode = false;

        foreach (Cell c in structure.cells)
        {
            switch (c.type)
            {
                case Cell.Type.Full:
                    c.block.GetComponent<Rigidbody>().isKinematic = false;
                    foreach (Block b in c.block.connectedBlocks)
                    {
                        FixedJoint joint = c.block.AddComponent<FixedJoint>();

                        joint.breakForce = joints.types[0].breakForce;
                        joint.breakTorque = joints.types[0].breakForce;
                        joint.enableCollision = joints.types[0].enableCollsion;

                        joint.enablePreprocessing = false;

                        joint.connectedBody = b.GetComponent<Rigidbody>();
                    }
                    break;
                case Cell.Type.Preview:
                    c.block.GetComponent<Collider>().enabled = false;
                    c.block.GetComponent<MeshRenderer>().enabled = false;
                    break;
                case Cell.Type.Employee:
                    c.block.GetComponent<Rigidbody>().isKinematic = false;
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
                    foreach (Joint joint in c.block.GetComponents<Joint>())
                        Destroy(joint);
                    break;
                case Cell.Type.Preview:
                    c.block.GetComponent<Collider>().enabled = true;
                    break;
                case Cell.Type.Employee:
                    c.block.GetComponent<Rigidbody>().isKinematic = true;
                    c.block.transform.localPosition = new Vector3(c.block.position.x, c.block.position.y, c.block.position.z);
                    c.block.transform.rotation = Quaternion.identity;
                    c.block.GetComponent<MeshRenderer>().material.color = Color.blue;
                    c.block.GetComponent<EmployeeBlock>().isDead = false;
                    break;
                default:
                    break;
            }
        }

        buildMode = true;
        simulationTime = 0;
    }
}