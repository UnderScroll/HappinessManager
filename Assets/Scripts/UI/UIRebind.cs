using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIRebind : MonoBehaviour
{
    [HideInInspector]
    public string ActionName
    {
        get => _actionName;
        set {
            _actionName = value;
            UpdateActionLabel();
        }
    }

    [HideInInspector]
    public InputActionReference ActionRef
    {
        get => _action;
        set => _action = value;
    }
    
    [HideInInspector]
    public int ActionBindingIndex
    {
        get => _actionBindingIndex;
        set => _actionBindingIndex = value;
    }

    [Header("Rebind")]
    [SerializeField]
    InputActionReference _action;
    [SerializeField]
    int _actionBindingIndex;

    [Header("UI")]
    [SerializeField]
    private string _actionName;
    [SerializeField]
    private TextMeshProUGUI _actionLabel;
    [SerializeField]
    private Image _overlay;
    [SerializeField]
    private Image _iconDevice1;
    [SerializeField]
    private Image _iconDevice2;

    public void StartRebind()
    {
        //Set up and verifications
        if (_overlay != null)
            _overlay.enabled = true;
        else
            Debug.LogWarning("No overlay in UIRebind");

        if (ActionRef == null)
        {
            Debug.LogError($"No action to rebind");
            return;
        }

        if (!(_actionBindingIndex < ActionRef.action.bindings.Count))
        {
            Debug.LogError($"Failed to get action {_actionBindingIndex} of {ActionRef.name}");
            return;
        }

        //Rebinding
        InputActionRebindingExtensions.RebindingOperation rebindOperation =  new();
        rebindOperation = ActionRef.ToInputAction()
            .PerformInteractiveRebinding(_actionBindingIndex)
                .OnComplete(operation =>
                {
                    UpdateBindingDisplay();
                });

        //Clean up
        if (_overlay != null)
            _overlay.enabled = false;

        rebindOperation.Dispose();
    }

    public void UpdateActionLabel()
    {
        _actionLabel.text = ActionName;
    }

    public void UpdateBindingDisplay()
    {
        ActionRef.action.GetBindingDisplayString(0, out string deviceLayoutName, out string _);
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        UpdateActionLabel();
    }
#endif
}
