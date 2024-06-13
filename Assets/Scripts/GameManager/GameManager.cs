using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics.CodeAnalysis;
using LevelLoader;

public class GameManager : MonoBehaviour
{
    public Transform StructureOrigin;

    [HideInInspector]
    public Builder.Builder Builder;
    [HideInInspector]
    public Simulation.Simulator Simulator;
    [HideInInspector]
    public LevelLoader.LevelLoader LevelLoader;
    [HideInInspector]
    public RuleManager RuleManager;
    [HideInInspector]
    public SoundManager SoundManager;
    [HideInInspector]
    public UI_HUD UI_HUD;

    public enum Stage { Stage1, Stage2, Stage3, Stage4, Stage5 };

    [SerializeField] public Stage CurrentStage;

    private bool _playing = false;

    public string FloorName = "";

    private void Awake()
    {
        if (!TryGetComponent(out Simulator))
            Debug.LogError("Failed to get the Simulator Component");
        if (!TryGetComponent(out Builder))
            Debug.LogError("Failed to get the Builder Component");
        if (!TryGetComponent(out LevelLoader))
            Debug.LogError("Failed to get the LevelLoader Component");
        if (!TryGetComponent(out RuleManager))
            Debug.LogError("Failed to get the RuleManager Component");
        if (!TryGetComponent(out SoundManager))
            Debug.LogError("Failed to get the SoundManager Component");

        Simulator.StructureOrigin = StructureOrigin;
        Builder.StructureOrigin = StructureOrigin;

        UI_HUD = FindObjectOfType<UI_HUD>();
        if (UI_HUD == null)
            Debug.LogError("Failed to get the UI_HUD in GameManager");
        else
            LevelLoader.UI_HUD = UI_HUD;
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres priv�s non utilis�s", Justification = "OnReloadLevel is called by Unity Input System")]
    void OnReloadLevel(InputValue _)
    {
        Debug.Log("ReloadingLevel");
        LevelLoader.ReloadLevel();
        SoundManager.PlayOnBuilding();

        //TMP FOR PLAYTESTING
        UI_HUD.UpdateLevelName(Builder.Level.name);
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres priv�s non utilis�s", Justification = "OnLoadNextLevel is called by Unity Input System")]
    void OnLoadNextLevel(InputValue _)
    {
        Debug.Log("LoadingNextLevel");
        LevelLoader.LoadNextLevel();
        SoundManager.PlayOnBuilding();

        //TMP FOR PLAYTESTING
        UI_HUD.UpdateLevelName(Builder.Level.name);
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres priv�s non utilis�s", Justification = "OnLoadPreviousLevel is called by Unity Input System")]
    void OnLoadPreviousLevel(InputValue _)
    {
        Debug.Log("LoadingPreviousLevel");
        LevelLoader.LoadPreviousLevel();
        SoundManager.PlayOnBuilding();

        //TMP FOR PLAYTESTING
        UI_HUD.UpdateLevelName(Builder.Level.name);
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres priv�s non utilis�s", Justification = "OnToggleMode is called by Unity Input System")]
    public void OnToggleMode()
    {
        if (_playing)
            ResetSimulation();
        else
            PlaySimulation();
    }

    private void PlaySimulation()
    {
        if (_playing)
            return;

        Builder.DeactivatePreview();

        Simulator.InitializeSimulation(Builder.Level.Structure);
        Simulator.Launch();

        SoundManager.PlayOnLaunchingSimulation();

        _playing = true;
    }

    public void ResetSimulation()
    {
        if (!_playing)
            return;

        Builder.ActivatePreview();
        Simulator.Reset();

        SoundManager.PlayOnBuilding();

        _playing = false;
    }
}
