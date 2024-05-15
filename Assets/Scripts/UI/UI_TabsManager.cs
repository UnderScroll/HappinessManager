using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using TMPro.EditorUtilities;
using UnityEngine.Events;

public class UI_TabsManager : MonoBehaviour
{
    [SerializeField] List<Button> Buttons;
    [SerializeField] List<GameObject> Panels;

    public UnityEvent OnPanelSwitch;

    int selectedPanel;

    private Vector3 hidePosition = new Vector3(1080, -1080, 0);

    void Start()
    {
        selectedPanel = 0;
        OnPanelSwitch.AddListener(PanelSwitch);
    }
    public void OnClick(int _panel)
    {
        if (_panel != selectedPanel)
        {
            int oldPanel = selectedPanel;
            selectedPanel = _panel;
            UpdatePanels(oldPanel, _panel);
        }
    }

    #region Private Methods
    private void UpdatePanels(int _old, int _new)
    {
        HidePanel(_old);
        ShowPanel(_new);
    }

    private void HidePanel(int _panel)
    {
        Panels[_panel].transform.DOLocalMoveX(hidePosition.x, 1.0f, true);
        Panels[_panel].transform.DOLocalMoveY(hidePosition.y, 1.0f, true);
    }
    private void ShowPanel(int _panel)
    {
        OnPanelSwitch?.Invoke();
        Panels[_panel].transform.DOLocalMoveX(0, 1.0f, true);
        Panels[_panel].transform.DOLocalMoveY(-90, 1.0f, true);
    }
    #endregion

    #region EVENTS
    private void PanelSwitch()
    {
        Debug.Log("son panels qui bougent");
    }
    #endregion
}

