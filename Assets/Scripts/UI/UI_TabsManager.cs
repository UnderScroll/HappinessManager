using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class UI_TabsManager : MonoBehaviour
{
    [SerializeField] List<Button> Buttons;
    [SerializeField] List<GameObject> Panels;
    [SerializeField] Vector3 hidePosition;
    [SerializeField] Vector2 showPosition;

    public UnityEvent OnPanelSwitch;

    int selectedPanel;

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
        Panels[_panel].transform.DOLocalMoveX(showPosition.x, 1.0f, true);
        Panels[_panel].transform.DOLocalMoveY(showPosition.y, 1.0f, true);
    }
    #endregion

    #region EVENTS
    private void PanelSwitch()
    {
        Debug.Log("son panels qui bougent");
        AkSoundEngine.PostEvent("Play_Menu_Settings_onSubMenuClick", gameObject);
    }
    #endregion
}

