using Builder;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelEditor))]
public class LevelEditorCustomEditor : Editor
{
    public VisualTreeAsset XmlInspector;
    private LevelEditor _levelEditor;

    public IMGUIContainer EmployeeEditorContainer;
    public Foldout EmployeeEditorFoldout;

    public override VisualElement CreateInspectorGUI()
    {
        //Get LevelEditor
        _levelEditor = (LevelEditor)target;
        _levelEditor.Editor = this;

        //Clean leftover
        int childCount = _levelEditor.transform.childCount;
        for (uint i = 0; i < childCount; i++)
            DestroyImmediate(_levelEditor.transform.GetChild(0).gameObject);

        //Initialize
        _levelEditor.Initialize();

        //Create inspector from uxml
        VisualElement inspectorRoot = new();
        XmlInspector.CloneTree(inspectorRoot);

        //Get Level size IntegerFields
        VisualElement levelContainer = inspectorRoot.Query<IMGUIContainer>("Level");
        IntegerField structureSizeX = levelContainer.Query<IntegerField>("unity-x-input");
        IntegerField structureSizeY = levelContainer.Query<IntegerField>("unity-y-input");
        IntegerField structureSizeZ = levelContainer.Query<IntegerField>("unity-z-input");

        //LevelSelection Bindings
        ObjectField levelSelectionField = levelContainer.Query<ObjectField>("Level");
        Label levelSelectionStatusLable = levelContainer.Query<Label>("LevelSelectionStatus");
        levelSelectionField.RegisterValueChangedCallback((evt) =>
        {
            if (evt.newValue == null)
            {
                levelSelectionStatusLable.text = "No level Selected";
                _levelEditor.UnloadLevel();
                return;
            }

            levelSelectionStatusLable.text = "";
            _levelEditor.Level = (Level)evt.newValue;
            _levelEditor.OnLevelChanged();
        });
        if (_levelEditor.Level == null)
            levelSelectionStatusLable.text = "No level Selected";

        //EmployeeEditor
        EmployeeEditorContainer = inspectorRoot.Query<IMGUIContainer>("EmployeeEditor");
        EmployeeEditorFoldout = EmployeeEditorContainer.Query<Foldout>("Foldout");

        //Get CurrentCell position IntegerFields
        VisualElement currentCellContainer = inspectorRoot.Query<IMGUIContainer>("CurrentCell");
        IntegerField currentCellPosX = currentCellContainer.Query<IntegerField>("unity-x-input");
        IntegerField currentCellPosY = currentCellContainer.Query<IntegerField>("unity-y-input");
        IntegerField currentCellPosZ = currentCellContainer.Query<IntegerField>("unity-z-input");

        //Restrict Structure size to be [0..INT_MAX] &
        //Restrict CellPosition to be [0..StructureSize] on Structure ChangeEvent
        structureSizeX.RegisterValueChangedCallback((evt) =>
        {
            structureSizeX.value = evt.newValue > 1 ? evt.newValue : 1;
            if (_levelEditor != null)
                _levelEditor.OnLevelChangedSize(new int3(structureSizeX.value, structureSizeY.value, structureSizeZ.value));
            if (currentCellPosX.value > (structureSizeX.value - 1))
                currentCellPosX.value = structureSizeX.value - 1;
        });
        structureSizeY.RegisterValueChangedCallback((evt) =>
        {
            structureSizeY.value = evt.newValue > 1 ? evt.newValue : 1;
            if (_levelEditor != null)
                _levelEditor.OnLevelChangedSize(new int3(structureSizeX.value, structureSizeY.value, structureSizeZ.value));
            if (currentCellPosY.value > (structureSizeY.value - 1))
                currentCellPosY.value = structureSizeY.value - 1;
        });
        structureSizeZ.RegisterValueChangedCallback((evt) =>
        {
            structureSizeZ.value = evt.newValue > 1 ? evt.newValue : 1;
            if (_levelEditor != null)
                _levelEditor.OnLevelChangedSize(new int3(structureSizeX.value, structureSizeY.value, structureSizeZ.value));
            if (currentCellPosZ.value > (structureSizeZ.value - 1))
                currentCellPosZ.value = structureSizeZ.value - 1;
        });

        //Restrict CellPosition to be [0..StructureSize] on CurrentCell ChangeEvent//
        currentCellPosX.RegisterValueChangedCallback((evt) =>
        {
            //Restrict
            Transform previewTransform = _levelEditor.CurrentCellPreviewInstance.transform;

            int newValueRestricted = 0;
            if (evt.newValue < 0)
                newValueRestricted = 0;
            else if (evt.newValue > (structureSizeX.value - 1))
                newValueRestricted = structureSizeX.value - 1;
            else
                newValueRestricted = evt.newValue;

            currentCellPosX.value = newValueRestricted;
            previewTransform.position = new Vector3(newValueRestricted, previewTransform.position.y, previewTransform.position.z);
        });
        currentCellPosY.RegisterValueChangedCallback((evt) =>
        {
            //Restrict
            Transform previewTransform = _levelEditor.CurrentCellPreviewInstance.transform;

            int newValueRestricted = 0;
            if (evt.newValue < 0)
                newValueRestricted = 0;
            else if (evt.newValue > (structureSizeY.value - 1))
                newValueRestricted = structureSizeY.value - 1;
            else
                newValueRestricted = evt.newValue;

            currentCellPosY.value = newValueRestricted;
            previewTransform.position = new Vector3(previewTransform.position.x, newValueRestricted, previewTransform.position.z);
        });
        currentCellPosZ.RegisterValueChangedCallback((evt) =>
        {
            //Restrict
            Transform previewTransform = _levelEditor.CurrentCellPreviewInstance.transform;

            int newValueRestricted = 0;
            if (evt.newValue < 0)
                newValueRestricted = 0;
            else if (evt.newValue > (structureSizeZ.value - 1))
                newValueRestricted = structureSizeZ.value - 1;
            else
                newValueRestricted = evt.newValue;

            currentCellPosZ.value = newValueRestricted;
            previewTransform.position = new Vector3(previewTransform.position.x, previewTransform.position.y, newValueRestricted);
        });

        //Get custom Controller
        VisualElement customController = inspectorRoot.Query("CustomController");

        //Control Button Callback Bindings
        Button northButton = customController.Query<Button>("North");       //North
        northButton.clickable.clicked += _levelEditor.OnNorthButtonClicked;
        Button eastButton = customController.Query<Button>("East");         //East
        eastButton.clickable.clicked += _levelEditor.OnEastButtonClicked;
        Button southButton = customController.Query<Button>("South");       //South
        southButton.clickable.clicked += _levelEditor.OnSouthButtonClicked;
        Button westButton = customController.Query<Button>("West");         //West
        westButton.clickable.clicked += _levelEditor.OnWestButtonClicked;
        Button upButton = customController.Query<Button>("Up");             //Up
        upButton.clickable.clicked += _levelEditor.OnUpButtonClicked;
        Button downButton = customController.Query<Button>("Down");         //Down
        downButton.clickable.clicked += _levelEditor.OnDownButtonClicked;

        //Current Block Preview
        ObjectField blockSelectionField = customController.Query<ObjectField>("CurrentSelectedBlock");
        blockSelectionField.RegisterValueChangedCallback((evt) =>
        {
            _levelEditor.CurrentSelectedBlock = (CellType)evt.newValue;
        });

        //Block Placement Callback bindings
        Button placeButton = customController.Query<Button>("Place"); //Place
        placeButton.clickable.clicked += _levelEditor.OnPlaceBlock;

        Button removeButton = customController.Query<Button>("Remove"); //Remove
        removeButton.clickable.clicked += _levelEditor.OnRemoveBlock;

        //Save button bindings
        Button saveButton = inspectorRoot.Query<Button>("Save");
        saveButton.clickable.clicked += _levelEditor.OnSaveButtonClicked;

        //Utility
        VisualElement utility = inspectorRoot.Query<VisualElement>("Utility");
        Button fillButton = utility.Query<Button>("Fill");
        fillButton.clickable.clicked += _levelEditor.OnFillButtonClicked;

        //Additional
        VisualElement additional = inspectorRoot.Query<VisualElement>("Additional");

        //Wind
        VisualElement windContainer = levelContainer.Query<VisualElement>("Wind");
        Foldout windFoldout = windContainer.Query<Foldout>("Wind");
        Toggle windToggle = windContainer.Query<Toggle>("Toggle");
        windToggle.RegisterValueChangedCallback(evt =>
        {
            _levelEditor.Level.IsWindEnabled = evt.newValue;
            windFoldout.value = evt.newValue;
            windFoldout.SetEnabled(evt.newValue);
        });
        Vector3Field windDirectionField = windFoldout.Query<Vector3Field>();
        FloatField windDirectionX = currentCellContainer.Query<FloatField>("unity-x-input");
        windDirectionX.RegisterValueChangedCallback((evt) =>
        {
            _levelEditor.Level.WindDirection.x = evt.newValue;
        });
        FloatField windDirectionY = currentCellContainer.Query<FloatField>("unity-y-input");
        windDirectionY.RegisterValueChangedCallback((evt) =>
        {
            _levelEditor.Level.WindDirection.y = evt.newValue;
        });
        FloatField windDirectionZ = currentCellContainer.Query<FloatField>("unity-z-input");
        windDirectionZ.RegisterValueChangedCallback((evt) =>
        {
            _levelEditor.Level.WindDirection.z = evt.newValue;
        });
        FloatField windStrength = windFoldout.Query<FloatField>("Strength");
        windStrength.RegisterValueChangedCallback((evt) =>
        {
            if (_levelEditor.Level != null)
                _levelEditor.Level.WindStrength = evt.newValue;
        });

        windFoldout.value = _levelEditor.IsWindEnabled;
        windFoldout.SetEnabled(_levelEditor.IsWindEnabled);

        //FollowPath
        VisualElement followPath = EmployeeEditorContainer.Query<VisualElement>("FollowPath");
        Toggle followPathToggle = followPath.Query<Toggle>("Toggle");
        Foldout followPathFoldout = followPath.Query<Foldout>("FollowPath");
        followPathToggle.RegisterValueChangedCallback(_ =>
        {
            if (_levelEditor.EmployeeCellData == null)
            {
                Debug.LogError("Tried to edit EmployeeData but it was null");
                return;
            }
            _levelEditor.OnEmployeeDataChanged();

            followPathFoldout.value = _levelEditor.EmployeeCellData.Movement.HasFollowPath;
            followPathFoldout.SetEnabled(_levelEditor.EmployeeCellData.Movement.HasFollowPath);
        });

        followPathFoldout.value = _levelEditor.HasFollowPath;
        followPathFoldout.SetEnabled(_levelEditor.HasFollowPath);

        //Follow Path
        //ListView emplyeeWaypoints = followPath.Query<ListView>("Waypoints");
        EnumField modeField = followPath.Query<EnumField>("Mode");
        modeField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });
        FloatField followForceField = followPath.Query<FloatField>("FollowForce");
        followForceField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });
        FloatField maxVelocityField = followPath.Query<FloatField>("MaxVelocity");
        maxVelocityField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });
        FloatField radiusField = followPath.Query<FloatField>("Radius");
        radiusField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });
        VisualElement breakContainer = followPath.Query<VisualElement>("Break");
        Toggle breakableField = breakContainer.Query<Toggle>("Breakable");
        breakableField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });
        Toggle breakThresholdField = breakContainer.Query<Toggle>("BreakThreshold");
        breakThresholdField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });

        //Force Stand
        FloatField standForceField = EmployeeEditorFoldout.Query<FloatField>("StandForce");
        standForceField.RegisterValueChangedCallback((_) => { _levelEditor.OnEmployeeDataChanged(); });

        // Return the finished Inspector UI.
        return inspectorRoot;
    }
}
#endif