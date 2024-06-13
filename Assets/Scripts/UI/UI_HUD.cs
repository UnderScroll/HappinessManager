using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Timeline;
using System.Threading;
using Unity.VisualScripting;

public class UI_HUD : MonoBehaviour
{
    [Header("Player Money")]
    public int money;
    [SerializeField] TextMeshProUGUI text_money;

    [Header("Temp For PlayTests")]
    [SerializeField] TextMeshProUGUI levelNameGUI;

    [Header("Construct Menu")]
    [SerializeField]
    [HideInInspector]
    public List<CellType> blocks = new List<CellType>();
    [HideInInspector]
    public List<CellType> decos = new List<CellType>();
    private CellType selected;

    [Header("Initialisation Menu")]
    [SerializeField] GameObject PrefabItem;
    [SerializeField] GameObject BlocsDisplayMenuPrefab;
    [SerializeField] Transform parentTranform;
    [SerializeField] TextMeshProUGUI blockDescription;

    [Header("EndLevel Menu")]
    [SerializeField] GameObject EndLevelMenuPrefab;
    private UI_EndLevelPanel endLevelPanel = null;

    [Header("Notebook Menu")]
    [SerializeField] GameObject notebook;
    [SerializeField] UI_Notebook ui_notebook;

    [SerializeField] GameObject BetweenStagesUI;

    private GameObject actualMenu;
    private GameManager _gameManager;

    public UnityAction DisplayNextFloorUI;
    public UnityAction GoToNextFloor;

    private bool flag = false;
    private float timer = 4f;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Failed to find GameManager in UI_HUD");

        CloseNotebook();
        DisplayNextFloorUI += DisplayUI;

        InitBudget();
        UpdateMoneyText();
        UpdateBlockDescription();
    }
    private void Update()
    {
        if (flag)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                GoToNextFloor?.Invoke();
        }
    }

    #region FOR_PLAYTESTS
    public void UpdateLevelName(string name)
    {
        levelNameGUI.text = name.Remove(name.Length - 7);
    }
    #endregion

    #region Money
    public void UpdateMoneyText()
    {
        float limit = GetBudgetLimit();
        if (limit != -1f)
        {
            money = (int) (limit - GetSpentMoney());
            text_money.text = money.ToString() + " $";
            AkSoundEngine.PostEvent("Play_UI_money_spent", gameObject);
        }
    }

    private float GetSpentMoney()
    {
        return _gameManager.Builder.SpentMoney;
    }

    private float GetBudgetLimit()
    {
        BudgetLimit budgetLimitRule = (BudgetLimit)_gameManager.RuleManager.Rules.Find(rule => { return rule.Type == "BudgetLimit"; });

        if (budgetLimitRule == null)
            return -1f;

        return budgetLimitRule.Budget;
    }
    public void ReloadBudgetUI()
    {
        InitBudget();
        UpdateMoneyText();
    }
    #endregion

    #region Construct Menu 

    public void InitBlocsMenu()
    {
        if (actualMenu == null)
        {
            actualMenu = Instantiate(BlocsDisplayMenuPrefab, parentTranform);
            foreach (CellType cellType in blocks)
            {
                GameObject _go = Instantiate(PrefabItem, actualMenu.transform);
                _go.name = cellType.Name;
                _go.GetComponent<UI_SelectableBlock>().ui_hud = this;
                _go.GetComponent<UI_SelectableBlock>().blockInfo = cellType;
            }

            if (selected != null)
            {
                List<UI_SelectableBlock> list = new();
                list.AddRange(actualMenu.GetComponentsInChildren<UI_SelectableBlock>());

                if (list.Count > 0)
                {
                    foreach (UI_SelectableBlock block in list)
                    {
                        if (IsThisBlockSelected(block.blockInfo))
                        {
                            SelectBlock(block.blockInfo);
                        }
                    }
                }
            }

        }
        else
            CloseMenu();
    }

    private void CloseMenu()
    {
        Destroy(actualMenu);
    }
    private void UpdateBlockDescription()
    {
        if (selected != null)
        {
            blockDescription.text = selected.name + " : " + selected.Description;
        }
    }
    public void TemporaryDescription(CellType _block, bool _active)
    {
        if (_active)
            blockDescription.text = _block.name + " : " + _block.Description;
        else
            UpdateBlockDescription();
    }
    private void Unselect(CellType _block) // ONLY DISPLAY, NO FUNCTIONNAL THING
    {
        // List of all the selectable blocks to find the one to unselect
        List<UI_SelectableBlock> list = new();
        list.AddRange(actualMenu.GetComponentsInChildren<UI_SelectableBlock>());

        if (list.Count > 0)
        {
            foreach (UI_SelectableBlock block in list)
            {
                if (block.blockInfo.name == _block.name)
                    block.Unover();
            }
        }

    }

    public void ResetConstructMenu()
    { 
        CloseMenu();
    }
    #endregion

    #region FloorsUI
    public void DisplayUI()
    {
        BetweenStagesUI.GetComponent<UI_BetweenStages>().Init();
        flag = true;
    }
    #endregion

    #region Fonctionnal functions

    private void InitBudget()
    {
        float limit = GetBudgetLimit();
        if (limit != -1f)
        {
            money = (int)limit;
        }
    }

    public void SelectBlock(CellType _block)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].name == _block.name)
            {
                CellType _oldSelection = selected;

                // select new block
                _gameManager.Builder.SelectBlock((uint)i);
                selected = blocks[i];

                // Update UI
                if (_oldSelection != null)
                    Unselect(_oldSelection);

                UpdateBlockDescription();
            }
        }
    }

    public bool IsThisBlockSelected(CellType _block)
    {
        if (selected != null)
        {
            if (_block.name == selected.name)
                return true;
            else
                return false;
        }
        return false;
    }
    #endregion

    #region EndLevel
    public IEnumerator DisplayEndLevelPanel(bool _victory)
    {
        yield return new WaitForSeconds(1);

        if (_gameManager.Playing)
        {
            GameObject _go = Instantiate(EndLevelMenuPrefab, this.gameObject.transform);
            endLevelPanel = _go.GetComponent<UI_EndLevelPanel>();
            endLevelPanel.ui_hud = this;

            if (endLevelPanel != null)
            {
                endLevelPanel.win = _victory;
                endLevelPanel.Init();
            }
        }
    }
    public void CloseEndLevelPanel()
    {
        if (endLevelPanel != null)
            Destroy(endLevelPanel.gameObject);
    }

    public void NextLevel()
    {
        _gameManager.LevelLoader.LoadNextLevel();
        _gameManager.SoundManager.PlayOnBuilding();

        //TMP FOR PLAYTESTING
        _gameManager.UI_HUD.UpdateLevelName(_gameManager.Builder.Level.name);
        CloseEndLevelPanel();
    }
    public void RestartLevel()
    {
        CloseEndLevelPanel();
        _gameManager.LevelLoader.ReloadLevel();
        _gameManager.SoundManager.PlayOnBuilding();
    }
    #endregion

    #region Notebook
    public void OpenNotebook()
    {
        // TODO : actuellement on peut poser des blocs à travers l'UI pour une raison obscure
        notebook.SetActive(true);
        AkSoundEngine.PostEvent("Play_Menu_Settings_onMenuOpen", gameObject);
    }
    public void CloseNotebook()
    {
        notebook.SetActive(false);
        AkSoundEngine.PostEvent("Play_Menu_Settings_onMenuOut", gameObject);
    }
    #endregion
}
