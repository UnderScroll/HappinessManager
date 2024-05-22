using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorCustomEditor : Editor
{
    public VisualTreeAsset XmlInspector;
    private LevelEditor _levelEditor;

    private void OnEnable()
    {
        _levelEditor = (LevelEditor)target;
    }

    public override VisualElement CreateInspectorGUI()
    {
        _levelEditor = (LevelEditor)target;

        //Create inspector from uxml
        VisualElement inspectorRoot = new();
        XmlInspector.CloneTree(inspectorRoot);

        //Get Structure size IntegerFields
        VisualElement structureContainer = inspectorRoot.Query<IMGUIContainer>("Structure");
        IntegerField structureSizeX = structureContainer.Query<IntegerField>("unity-x-input");
        IntegerField structureSizeY = structureContainer.Query<IntegerField>("unity-y-input");
        IntegerField structureSizeZ = structureContainer.Query<IntegerField>("unity-z-input");

        //Get CurrentCell position IntegerFields
        VisualElement currentCellContainer = inspectorRoot.Query<IMGUIContainer>("CurrentCell");
        IntegerField currentCellPosX = currentCellContainer.Query<IntegerField>("unity-x-input");
        IntegerField currentCellPosY = currentCellContainer.Query<IntegerField>("unity-y-input");
        IntegerField currentCellPosZ = currentCellContainer.Query<IntegerField>("unity-z-input");

        //Restrict Structure size to be [0..INT_MAX] &
        //Restrict CellPosition to be [0..StructureSize] on Structure ChangeEvent
        structureSizeX.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            //Debug.Log("structureSizeX.RegisterCallback<ChangeEvent<int>>");
            structureSizeX.value = evt.newValue > 1 ? evt.newValue : 1;
            if (currentCellPosX.value > (structureSizeX.value - 1))
                currentCellPosX.value = structureSizeX.value - 1;
        });
        structureSizeY.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            structureSizeY.value = evt.newValue > 1 ? evt.newValue : 1;
            if (currentCellPosY.value > (structureSizeY.value - 1))
                currentCellPosY.value = structureSizeY.value - 1;
        });
        structureSizeZ.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            structureSizeZ.value = evt.newValue > 1 ? evt.newValue : 1;
            if (currentCellPosZ.value > (structureSizeZ.value - 1))
                currentCellPosZ.value = structureSizeZ.value - 1;
        });

        //Restrict CellPosition to be [0..StructureSize] on CurrentCell ChangeEvent
        currentCellPosX.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            if (evt.newValue < 0)
                currentCellPosX.value = 0;
            else if (evt.newValue > (structureSizeX.value - 1))
                currentCellPosX.value = structureSizeX.value - 1;
            else
                currentCellPosX.value = evt.newValue;
        });
        currentCellPosY.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            if (evt.newValue < 0)
                currentCellPosY.value = 0;
            else if (evt.newValue > (structureSizeY.value - 1))
                currentCellPosY.value = structureSizeY.value - 1;
            else
                currentCellPosY.value = evt.newValue;
        });
        currentCellPosZ.RegisterCallback<ChangeEvent<int>>((evt) =>
        {
            if (evt.newValue < 0)
                currentCellPosZ.value = 0;
            else if (evt.newValue > (structureSizeZ.value - 1))
                currentCellPosZ.value = structureSizeZ.value - 1;
            else
                currentCellPosZ.value = evt.newValue;
        });

        //Button Callback Bindings
        VisualElement customController = inspectorRoot.Query("CustomController");
        //North
        Button northButton = customController.Query<Button>("North");
        northButton.clickable.clicked += _levelEditor.OnNorthButtonClicked;
        //East
        Button eastButton = customController.Query<Button>("East");
        eastButton.clickable.clicked += _levelEditor.OnEastButtonClicked;
        //South
        Button southButton = customController.Query<Button>("South");
        southButton.clickable.clicked += _levelEditor.OnSouthButtonClicked;
        //West
        Button westButton = customController.Query<Button>("West");
        westButton.clickable.clicked += _levelEditor.OnWestButtonClicked;
        //Up
        Button upButton = customController.Query<Button>("Up");
        upButton.clickable.clicked += _levelEditor.OnUpButtonClicked;
        //Down
        Button downButton = customController.Query<Button>("Down");
        downButton.clickable.clicked += _levelEditor.OnDownButtonClicked;

        //Debug.Log("Finished Building Custom Editor");

        // Return the finished Inspector UI.
        return inspectorRoot;
    }
}
