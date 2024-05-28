using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    [Header("Player Money")]
    public int money;
    [SerializeField] TextMeshProUGUI text_money;

    [Header("Construct Menu")]
    [SerializeField] 
    public List<CellType> blocs;
    [SerializeField] List<string> decos;
    [SerializeField] List<string> adhesifs;

    [Header("Initialisation Menu")]
    [SerializeField] GameObject PrefabItem;
    [SerializeField] GameObject BlocsDisplayMenuPrefab;
    [SerializeField] Transform parentTranform;

    private GameObject actualMenu;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Failed to find GameManager in UI_HUD");

        UpdateMoneyText();
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
            foreach (CellType cellType in blocs)
            {
               // cellType.Block;
               // cellType.Name;
               // cellType.Price;
               // cellType.BlockIcon
                Instantiate(PrefabItem, actualMenu.transform);
            }
        }
        else
            CloseMenu();
    }
    public void InitDecoMenu()
    {
        if (actualMenu == null)
        {
            actualMenu = Instantiate(BlocsDisplayMenuPrefab, parentTranform);
            foreach (string deco in decos)
                Instantiate(PrefabItem, actualMenu.transform);
        }
        else
            CloseMenu();
    }

    public void InitAdhesifsMenu()
    {
        if (actualMenu == null)
        {
            actualMenu = Instantiate(BlocsDisplayMenuPrefab, parentTranform);
            foreach (string adhesif in adhesifs)
                Instantiate(PrefabItem, actualMenu.transform);
        }
        else
            CloseMenu();
    }

    public void CloseMenu()
    {
        Destroy(actualMenu);
    }

    #endregion

    // region fonctionnement reel
    // TODO : dans le builder (dans le _gm), fct Select
    // GameManager.Builder.SelectBlock(indice du bloc de la liste)

    // TODO : afficher ou non le budget et le bon!
    // dans gamemanager builder level get rule budget limit, si la rule n'existe pas ne pas afficher le budget 
}
