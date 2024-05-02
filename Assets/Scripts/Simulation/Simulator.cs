using Builder;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public class Simulator : MonoBehaviour
    {
        public Transform StructureOrigin { set { _structureOrigin = value; } }
        private Transform _structureOrigin;

        Structure _structure;
        GameObject[,,] _instances;
 
        public void InitializeSimulation(Structure structure)
        {
            _instances = new GameObject[10, 10, 10];

            _structure = structure;

            instantiateBlocks();
            createConnections();
        }

        private void instantiateBlocks()
        {
            foreach (CellData cellData in _structure.Cells)
            {
                if (cellData == null) continue;

                instanciateBlock(cellData);
            }
        }

        private GameObject instanciateBlock(CellData cellData)
        {
            GameObject instance = Instantiate(cellData.Type.Block, _structureOrigin);

            Rigidbody instanceRb = instance.AddComponent<Rigidbody>();
            instanceRb.mass = cellData.Type.Mass;

            instance.AddComponent<BoxCollider>();

            instance.transform.Translate(new Vector3(cellData.position.x, cellData.position.y, cellData.position.z));
            _instances[cellData.position.x, cellData.position.y, cellData.position.z] = instance;

            return instance;
        }

        private void createConnections()
        {
            foreach (CellData cellData in _structure.Cells)
            {
                if (cellData == null) continue;

                GameObject blockInstance = _instances[cellData.position.x, cellData.position.y, cellData.position.z];

                //Add north connection
                ConnectionType connectionType = cellData.GetConnectionType(CellData.Face.North);
                int3 northBlockPosition = new int3(cellData.position.x, cellData.position.y, cellData.position.z + 1);
                if (connectionType != null 
                    && isInBounds(northBlockPosition)
                    && cellData.Type.hasConnection(CellData.Face.North)
                    && _structure.Cells[northBlockPosition.x, northBlockPosition.y, northBlockPosition.z].Type.hasConnection(CellData.Face.South))
                {
                    GameObject northBlock = _instances[northBlockPosition.x, northBlockPosition.y, northBlockPosition.z];
                    if (northBlock != null)
                        addConnection(blockInstance, northBlock, connectionType);
                }

                //Add east connection
                connectionType = cellData.GetConnectionType(CellData.Face.East);
                int3 eastBlockPosition = new int3(cellData.position.x + 1, cellData.position.y, cellData.position.z);
                if (connectionType != null
                    && isInBounds(eastBlockPosition)
                    && cellData.Type.hasConnection(CellData.Face.East)
                    && _structure.Cells[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z].Type.hasConnection(CellData.Face.Bottom))
                {
                    GameObject eastBlock = _instances[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z];
                    if (eastBlock!= null)
                        addConnection(blockInstance, eastBlock, connectionType);
                }

                //Add top connection
                connectionType = cellData.GetConnectionType(CellData.Face.Top);
                int3 topBlockPosition = new int3(cellData.position.x, cellData.position.y + 1, cellData.position.z);
                if (connectionType != null 
                    && isInBounds(topBlockPosition)
                    && cellData.Type.hasConnection(CellData.Face.Top)
                    && _structure.Cells[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z].Type.hasConnection(CellData.Face.Bottom))
                {
                    GameObject topBlock = _instances[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z];
                    if (topBlock != null)
                        addConnection(blockInstance, topBlock, connectionType);
                }
            }
        }

        private bool isInBounds(int3 position)
        {
            return position.x >= 0
                && position.y >= 0
                && position.z >= 0
                && position.x < _structure.Cells.GetLength(0)
                && position.y < _structure.Cells.GetLength(1)
                && position.z < _structure.Cells.GetLength(2);
        }

        private void addConnection(GameObject obj, GameObject to, ConnectionType type)
        {
            FixedJoint joint = obj.AddComponent<FixedJoint>();
            joint.breakForce = type.BreakForce;
            joint.breakTorque = type.BreakTorque;
            joint.enableCollision = type.EnableCollision;
            joint.enablePreprocessing = type.EnablePreprocessing;

            joint.connectedBody = to.GetComponent<Rigidbody>();
        }

        public void Launch()
        {
        }

        public void Reset()
        {
            foreach (GameObject instance in _instances)
            {
                Destroy(instance);
            }
        }
    }
}