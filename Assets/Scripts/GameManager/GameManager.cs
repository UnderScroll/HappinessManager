using System.Collections;
using System.Collections.Generic;
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

    void OnPlay(InputValue value)
    {
        Debug.Log("OnPlay");

        if (!playing)
        {
            builder.deactivatePreview();

            simulator.InitializeSimulation(builder.Structure);
            simulator.Launch();

            playing = true;
        }
    }

    void OnReset(InputValue value)
    {
        Debug.Log("OnReset");

        if (playing)
        {
            builder.activatePreview();
            simulator.Reset();

            playing = false;
        }
    }
}
