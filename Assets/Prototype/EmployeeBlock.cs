using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeBlock : Block
{
    static long currentId = 0;
    public static List<EmployeeBlock> employees = new List<EmployeeBlock>();
    public bool isDead = false;

    public override List<Cell> getNeighbors(Structure structure)
    {
        List<Cell> neighbours = new List<Cell>();

        if (!structure.isInBounds((position.x, position.y - 1, position.z)))
             return neighbours;

        neighbours.Add(structure.cells[position.x, position.y - 1, position.z]);

        return neighbours;
    }

    public override bool place(Structure structure, (uint x, uint y, uint z) position)
    {
        groupId = currentId;
        currentId++;

        transform.localPosition = new Vector3(position.x, position.y, position.z);
        this.position = position;

        Block block = this;

        Cell cell = structure.cells[position.x, position.y, position.z];

        cell.block = block;
        cell.type = Cell.Type.Employee;
        cell.removable = false;

        employees.Add(this);

        return true;
    }

    public override bool remove(Structure structure)
    {
        return false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            isDead = true;
            Debug.Log("Employee touched ground");
        }
    }
}
