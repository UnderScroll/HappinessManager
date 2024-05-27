using System.Collections;
using Builder;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation
{
    public partial class Simulator : MonoBehaviour
    {
        public Transform StructureOrigin { set { _structureOrigin = value; } }
        private Transform _structureOrigin;

        private Structure _structure;
        private GameObject[,,] _instances;

        //private GameManager _gameManager;
        private double timeCounter;
        private bool _isSimulationRunning;
        public double ValidationTime;
        private bool _isSimulationFailed;

        private void Start()
        {
            /*
            if (!TryGetComponent(out _gameManager))
                Debug.LogError("GameManager wasn't found, this will cause issues");
            */
        }

        public void InitializeSimulation(Structure structure)
        {
            _instances = new GameObject[structure.Size.x, structure.Size.y, structure.Size.z];

            _structure = structure;

            EmployeeCollision.collisionEvent.AddListener(OnEmployeeGroundCollision);

            InstantiateBlocks();
            CreateConnections();

            Launch();
        }

        private void InstantiateBlocks()
        {
            foreach (CellData cellData in _structure.Cells)
            {
                if (cellData == null) continue;
                if (!cellData.Type.ShouldBeSimulated) continue;

                InstanciateBlock(cellData);
            }
        }

        private GameObject InstanciateBlock(CellData cellData)
        {
            GameObject instance = Instantiate(cellData.Type.Block, _structureOrigin);

            Rigidbody instanceRb = instance.AddComponent<Rigidbody>();
            instanceRb.mass = cellData.Type.Mass;

            instance.transform.Translate(new Vector3(cellData.Position.x, cellData.Position.y, cellData.Position.z));
            _instances[cellData.Position.x, cellData.Position.y, cellData.Position.z] = instance;

            return instance;
        }

        private void CreateConnections()
        {
            foreach (CellData cellData in _structure.Cells)
            {
                if (cellData == null) continue;

                GameObject blockInstance = _instances[cellData.Position.x, cellData.Position.y, cellData.Position.z];

                //Add north connection
                ConnectionType connectionType = cellData.GetConnectionType(CellData.Face.North);
                int3 northBlockPosition = new(cellData.Position.x, cellData.Position.y, cellData.Position.z + 1);
                if (connectionType != null
                    && IsInBounds(northBlockPosition)
                    && cellData.Type.HasConnection(CellData.Face.North))
                {
                    if (_structure.Cells[northBlockPosition.x, northBlockPosition.y, northBlockPosition.z] != null
                        && _structure.Cells[northBlockPosition.x, northBlockPosition.y, northBlockPosition.z].Type.HasConnection(CellData.Face.South))
                    {
                        GameObject northBlock = _instances[northBlockPosition.x, northBlockPosition.y, northBlockPosition.z];
                        if (northBlock != null)
                            AddConnection(blockInstance, northBlock, connectionType);
                    }
                }

                //Add east connection
                connectionType = cellData.GetConnectionType(CellData.Face.East);
                int3 eastBlockPosition = new(cellData.Position.x + 1, cellData.Position.y, cellData.Position.z);
                if (connectionType != null
                    && IsInBounds(eastBlockPosition)
                    && cellData.Type.HasConnection(CellData.Face.East))
                {
                    if (_structure.Cells[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z] != null
                        && _structure.Cells[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z].Type.HasConnection(CellData.Face.West))
                    {
                        GameObject eastBlock = _instances[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z];
                        if (eastBlock != null)
                            AddConnection(blockInstance, eastBlock, connectionType);
                    }
                }

                //Add top connection
                connectionType = cellData.GetConnectionType(CellData.Face.Top);
                int3 topBlockPosition = new(cellData.Position.x, cellData.Position.y + 1, cellData.Position.z);
                if (connectionType != null
                    && IsInBounds(topBlockPosition)
                    && cellData.Type.HasConnection(CellData.Face.Top))
                {
                    if (_structure.Cells[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z] != null
                        && _structure.Cells[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z].Type.HasConnection(CellData.Face.Bottom))
                    {
                        GameObject topBlock = _instances[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z];
                        if (topBlock != null)
                            AddConnection(blockInstance, topBlock, connectionType);
                    }
                }
            }
        }

        private bool IsInBounds(int3 position)
        {
            return position.x >= 0
                && position.y >= 0
                && position.z >= 0
                && position.x < _structure.Size.x
                && position.y < _structure.Size.y
                && position.z < _structure.Size.z;
        }

        private void AddConnection(GameObject obj, GameObject to, ConnectionType type)
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
            _isSimulationRunning = true;
        }

        private void Update()
        {
            if (_isSimulationRunning)
            {
                if (timeCounter > ValidationTime && !_isSimulationFailed)
                {
                    _isSimulationRunning = false;
                    OnLevelValidated();
                }
                else
                    timeCounter += Time.deltaTime;
            }
        }

        public void Reset()
        {
            timeCounter = 0;
            _isSimulationRunning = false;
            _isSimulationFailed = false;

            foreach (GameObject instance in _instances)
                Destroy(instance);
        }

        public void OnLevelValidated()
        {
            Debug.Log("LevelValidated");
        }

        public void OnEmployeeGroundCollision()
        {
            _isSimulationFailed = true;
            _isSimulationRunning = false;
        }
    }
}