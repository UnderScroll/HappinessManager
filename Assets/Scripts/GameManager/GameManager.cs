using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Transform StructureOrigin;

    private Builder.Builder builder;
    private Simulation.Simulator simulator;
    private bool playing = false;

    private void Awake()
    {
        simulator = GetComponent<Simulation.Simulator>();
        builder = GetComponent<Builder.Builder>();

        simulator.StructureOrigin = StructureOrigin;
        builder.StructureOrigin = StructureOrigin;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "This function is called by Unity Input System")]
    void OnPlay(InputValue _)
    {
        if (!playing)
        {
            builder.DeactivatePreview();

            simulator.InitializeSimulation(builder.Structure);
            simulator.Launch();

            playing = true;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "This function is called by Unity Input System")]
    void OnReset(InputValue _)
    {
        if (playing)
        {
            ResetSimulation();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "This function is called by Unity Input System")]
    void OnLoadLevel(InputValue _)
    {

    }

    public void ResetSimulation()
    {
        builder.ActivatePreview();
        simulator.Reset();

        playing = false;
    }
}
