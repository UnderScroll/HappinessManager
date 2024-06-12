using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class UIRebind : MonoBehaviour
{
    [HideInInspector]
    public string ActionName
    {
        get => _actionName;
        set
        {
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

    RebindingOperation _rebindingOperation;

    [Header("UI")]
    [SerializeField]
    private string _actionName;
    [SerializeField]
    private TextMeshProUGUI _actionLabel;
    [SerializeField]
    private GameObject _overlay;
    [SerializeField]
    private GameObject _iconDevice1;
    [SerializeField]
    private GameObject _iconDevice2;

    [Header("Mouse Icons")]
    [SerializeField]
    private Sprite _LMBIcon;
    [SerializeField]
    private Sprite _RMBIcon;
    [SerializeField]
    private Sprite _MMBIcon;

    public void OnEnable()
    {
        UpdateActionLabel();
        UpdateBindingDisplay();
    }

    public void StartRebind()
    {
        void Clean()
        {
            ActionRef.action.Enable();
            _rebindingOperation.Dispose();
            if (_overlay != null)
                _overlay.SetActive(false);
        }

        //Set up and verifications
        if (_overlay != null)
            _overlay.SetActive(true);
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

        _rebindingOperation?.Cancel();
        ActionRef.action.Disable();
        EventSystem.current.SetSelectedGameObject(null);

        Debug.Log($"Rebinding...");

        _rebindingOperation = ActionRef.action.PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                UpdateBindingDisplay();
                Clean();
            })
            .OnCancel(_ => Clean());

        _rebindingOperation.Start();
    }

    public void ResetToDefault()
    {
        ActionRef.action.RemoveBindingOverride(0);
        UpdateBindingDisplay();
    }

    public void UpdateActionLabel()
    {
        _actionLabel.text = ActionName;
    }

    public void UpdateBindingDisplay()
    {
        string displayString = ActionRef.action.GetBindingDisplayString(0, out string deviceLayoutName, out string controlPath);
        //Debug.Log($"Device : {deviceLayoutName}");
        switch (deviceLayoutName)
        {
            case "Keyboard":
                _iconDevice1.gameObject.SetActive(false);
                _iconDevice2.gameObject.SetActive(true);
                TextMeshProUGUI keyLabel = _iconDevice2.GetComponentInChildren<TextMeshProUGUI>();
                keyLabel.text = displayString.ToUpper();
                break;
            case "Mouse":
                _iconDevice1.gameObject.SetActive(true);
                _iconDevice2.gameObject.SetActive(false);
                //Debug.Log(controlPath);
                switch (controlPath)
                {
                    case "leftButton":
                        _iconDevice1.GetComponent<Image>().sprite = _LMBIcon;
                        break;
                    case "rightButton":
                        _iconDevice1.GetComponent<Image>().sprite = _RMBIcon;
                        break;
                    case "middleButton":
                        _iconDevice1.GetComponent<Image>().sprite = _MMBIcon;
                        break;
                    default:
                        Debug.LogWarning("Control not known");
                        break;
                }
                break;
            default:
                Debug.LogWarning("Device used not compatible");
                break;
        }
    }

    [Serializable] public class InteractiveRebindEvent : UnityEvent<UIRebind, RebindingOperation> { }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        UpdateActionLabel();
    }
#endif
}
