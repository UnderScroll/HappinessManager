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

        private GameManager _gameManager;
        private double timeCounter;
        private bool _isSimulationRunning;
        public double ValidationTime;

        private void Start()
        {
            _gameManager = GetComponent<GameManager>();
            if (_gameManager == null)
                Debug.LogWarning("GameManager wasn't found, this will cause issues");
        }

        public void InitializeSimulation(Structure structure)
        {
            _instances = new GameObject[10, 10, 10];

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

            instance.AddComponent<BoxCollider>();

            instance.transform.Translate(new Vector3(cellData.position.x, cellData.position.y, cellData.position.z));
            _instances[cellData.position.x, cellData.position.y, cellData.position.z] = instance;

            return instance;
        }

        private void CreateConnections()
        {
            foreach (CellData cellData in _structure.Cells)
            {
                if (cellData == null) continue;

                GameObject blockInstance = _instances[cellData.position.x, cellData.position.y, cellData.position.z];

                //Add north connection
                ConnectionType connectionType = cellData.GetConnectionType(CellData.Face.North);
                int3 northBlockPosition = new(cellData.position.x, cellData.position.y, cellData.position.z + 1);
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
                int3 eastBlockPosition = new(cellData.position.x + 1, cellData.position.y, cellData.position.z);
                if (connectionType != null
                    && IsInBounds(eastBlockPosition)
                    && cellData.Type.HasConnection(CellData.Face.North))
                {
                    if (_structure.Cells[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z] != null
                        && _structure.Cells[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z].Type.HasConnection(CellData.Face.South))
                    {
                        GameObject eastBlock = _instances[eastBlockPosition.x, eastBlockPosition.y, eastBlockPosition.z];
                        if (eastBlock != null)
                            AddConnection(blockInstance, eastBlock, connectionType);
                    }
                }

                //Add top connection
                connectionType = cellData.GetConnectionType(CellData.Face.Top);
                int3 topBlockPosition = new(cellData.position.x, cellData.position.y + 1, cellData.position.z);
                if (connectionType != null
                    && IsInBounds(topBlockPosition)
                    && cellData.Type.HasConnection(CellData.Face.North))
                {
                    if (_structure.Cells[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z] != null
                        && _structure.Cells[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z].Type.HasConnection(CellData.Face.South))
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
                && position.x < _structure.Cells.GetLength(0)
                && position.y < _structure.Cells.GetLength(1)
                && position.z < _structure.Cells.GetLength(2);
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
                if (timeCounter > ValidationTime)
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
            StopAllCoroutines();

            foreach (GameObject instance in _instances)
                Destroy(instance);
        }

        public void OnLevelValidated()
        {
            Debug.Log("LevelValidated");
        }

        public IEnumerator ResetAfterTime(float seconds)
        {
            Debug.Log($"Simulation will reset in {seconds} seconds");

            yield return new WaitForSeconds(seconds);
            
            _gameManager.ResetSimulation();
        }

        public void OnEmployeeGroundCollision()
        {
            Debug.Log("Employee touched grass");
            _isSimulationRunning = false;
            StartCoroutine(ResetAfterTime(2.0f));
        }
    }
}