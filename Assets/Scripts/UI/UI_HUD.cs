using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_HUD : MonoBehaviour
{
    [Header("Player Money")]
    public int money;
    [SerializeField] TextMeshProUGUI text_money;

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

    private GameObject actualMenu;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Failed to find GameManager in UI_HUD");

        // TODO : init money value from level
        UpdateMoneyText();
        UpdateBlockDescription();
    }

    #region Money
    private void UpdateMoneyText()
    {
        text_money.text = money.ToString() + " $";
    }
    #endregion

    #region Construct Menu Initialisation

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
        }
        else
            CloseMenu();
    }
    public void InitDecoMenu()
    {
        if (actualMenu == null)
        {
            // do the thing
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
                    block.MoveDown();
            }
        }

    }
    #endregion

    #region Fonctionnal functions

    // TODO : afficher ou non le budget et le bon!
    // dans gamemanager builder level get rule budget limit, si la rule n'existe pas ne pas afficher le budget 

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
            }
        }
        UpdateBlockDescription();
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
}
