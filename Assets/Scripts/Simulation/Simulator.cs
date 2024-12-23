using Builder;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

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
        private bool _isSimulationFailed;

        UnityAction OnValidationFailed;

        private void Start()
        {
            if (!TryGetComponent(out _gameManager))
                Debug.LogError("GameManager wasn't found, this will cause issues");

            OnValidationFailed += LevelFailed;
        }

        public void InitializeSimulation(Structure structure)
        {
            _instances = new GameObject[structure.Size.x, structure.Size.y, structure.Size.z];

            _structure = structure;

            EmployeeCollision.collisionEvent.AddListener(OnEmployeeGroundCollision);

            if (_gameManager.Builder.Level.IsWindEnabled)
            {
                AkSoundEngine.PostEvent("Play_Wind_Gust", gameObject);
            }

            InstantiateBlocks();
            CreateConnections();

            if (!EasyMode.Enabled)
            {
                List<IRule> unvalidatedRules = _gameManager.RuleManager.ValidateAllRules();
                if (unvalidatedRules.Count > 0)
                    OnValidationFailed?.Invoke();
            }

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

            if (cellData.Type.IsEmployee)
            {
                EmployeeCellData employeeCell = (EmployeeCellData)cellData;

                //ForceStand
                ForceStand forceStand = instance.AddComponent<ForceStand>();
                forceStand.StandForce = employeeCell.Movement.StandForce;

                if (employeeCell.Movement.HasFollowPath)
                {
                    //FollowPath
                    FollowPath followPath = instance.AddComponent<FollowPath>();
                    followPath.Breakable = employeeCell.Movement.Breakable;
                    followPath.BreakThreshold = employeeCell.Movement.BreakThreshold;
                    followPath.Waypoints = employeeCell.Movement.Waypoints;
                    followPath.OffsetWaypoints = new List<Vector3>();
                    foreach (Vector3 waypoint in followPath.Waypoints)
                        followPath.OffsetWaypoints.Add(waypoint + _structureOrigin.position);
                    followPath.FollowForce = employeeCell.Movement.FollowForce;
                    followPath.FollowMode = employeeCell.Movement.Mode;
                    followPath.Radius = employeeCell.Movement.Radius;
                    followPath.maxVelocity = employeeCell.Movement.MaxVelocity;

                    //EmployeeMovement
                    EmployeeMovement employeeMovement = instance.AddComponent<EmployeeMovement>();
                    instance.TryGetComponent(out employeeMovement.AlignToCamera);
                    employeeMovement.TryGetComponent(out employeeMovement.ForceStand);
                    employeeMovement.TryGetComponent(out employeeMovement.FollowPath);

                    instance.GetComponentInChildren<Animator>().SetBool("IsWalking", true);
                }
            }

            return instance;
        }

        private void CreateConnections()
        {
            foreach (CellData cellData in _structure.Cells)
            {
                if (cellData == null) continue;

                GameObject blockInstance = _instances[cellData.Position.x, cellData.Position.y, cellData.Position.z];
                if (blockInstance == null) continue;

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
                        {
                            if (_structure.Cells[topBlockPosition.x, topBlockPosition.y, topBlockPosition.z].Type.IsEmployee)
                                AddSwivelConnection(blockInstance, topBlock, connectionType);
                            else
                                AddConnection(blockInstance, topBlock, connectionType);
                        }
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

        private void AddSwivelConnection(GameObject obj, GameObject to, ConnectionType type)
        {
            ConfigurableJoint configurableJoint = obj.AddComponent<ConfigurableJoint>();

            configurableJoint.breakForce = type.BreakForce;
            configurableJoint.breakTorque = type.BreakTorque;
            configurableJoint.enableCollision = type.EnableCollision;
            configurableJoint.enablePreprocessing = type.EnablePreprocessing;

            configurableJoint.xMotion = ConfigurableJointMotion.Locked;
            configurableJoint.yMotion = ConfigurableJointMotion.Locked;
            configurableJoint.zMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;

            configurableJoint.connectedBody = to.GetComponent<Rigidbody>();
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
            _gameManager.SoundManager.PlayOnLevelValidated();
            StartCoroutine(_gameManager.UI_HUD.DisplayEndLevelPanel(true));
        }

        public void OnEmployeeGroundCollision()
        {
            if (_isSimulationRunning)
            {
                OnValidationFailed?.Invoke();
                _isSimulationFailed = true;
                _isSimulationRunning = false;
            }
        }

        private void LevelFailed()
        {
            // Only once    
            if (!_isSimulationFailed)
            {
                _gameManager.SoundManager.PlayOnLevelFailed();
                StartCoroutine(_gameManager.UI_HUD.DisplayEndLevelPanel(false));
            }

        }
    }
}