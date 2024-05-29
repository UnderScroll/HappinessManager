using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics.CodeAnalysis;

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
    public UI_HUD UI_HUD;

    private bool _playing = false;

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

        Simulator.StructureOrigin = StructureOrigin;
        Builder.StructureOrigin = StructureOrigin;
    }

    public void OnPlay(InputValue _)
    {
        if (!_playing)
        {
            Builder.DeactivatePreview();

            Simulator.InitializeSimulation(Builder.Level.Structure);
            Simulator.Launch();

            _playing = true;
        }
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres privés non utilisés", Justification = "OnReset is called by Unity Input System")]
    void OnReset(InputValue _)
    {
        Debug.Log("ResetingLevel");
        ResetSimulation();
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres privés non utilisés", Justification = "OnReloadLevel is called by Unity Input System")]
    void OnReloadLevel(InputValue _)
    {
        Debug.Log("ReloadingLevel");
        LevelLoader.ReloadLevel();
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres privés non utilisés", Justification = "OnLoadNextLevel is called by Unity Input System")]
    void OnLoadNextLevel(InputValue _)
    {
        Debug.Log("LoadingNextLevel");
        LevelLoader.LoadNextLevel();
    }

    [SuppressMessage("CodeQuality", "IDE0051:Supprimer les membres privés non utilisés", Justification = "OnLoadPreviousLevel is called by Unity Input System")]
    void OnLoadPreviousLevel(InputValue _)
    {
        Debug.Log("LoadingPreviousLevel");
        LevelLoader.LoadPreviousLevel();
    }

    public void PlaySimulation()
    {
        if (!_playing)
        {
            Builder.DeactivatePreview();

            Simulator.InitializeSimulation(Builder.Level.Structure);
            Simulator.Launch();

            _playing = true;
        }
    }

    public void ResetSimulation()
    {
        if (_playing)
        {
            Builder.ActivatePreview();
            Simulator.Reset();

            _playing = false;
        }
    }
}
