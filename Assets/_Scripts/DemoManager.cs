﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PuzzleType
{
    Seesaw,
    Pulley,
    SwingPlatform
}

public enum PulleyType
{
    Hook,
    Platform,
    HeavyDoor
}

public class DemoManager : MonoBehaviour {

    #region Variables
    private static DemoManager instance;
    private DemoManager()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }
    public static DemoManager Instance
    {
        get
        {
            if (instance == null)
            {
                new ScenarioManager();
            }

            return instance;
        }
    }

    [SerializeField] private bool generateSpecificScenario = false;
    public PuzzleType puzzleType;
    public PulleyType pulleyType;

    [SerializeField] private ScenarioManager[] pulleyScenarios;
    [SerializeField] private ScenarioManager[] seesawScenarios;
    [SerializeField] private ScenarioManager[] swingingPlatformScenarios;
    [SerializeField] private bool usePulleyScenarios;
    [SerializeField] private bool useSeesawScenarios;
    [SerializeField] private bool useSwingingPlatformScenarios;
    private List<ScenarioManager> possibleScenarios;

    [SerializeField] private int numberOfScenariosToGenerate = 1;
    [SerializeField] private float spacingBetweenScenarios = 10.0f;

    // Manager References
    public GameObject creationManager;

    #endregion


    #region Unity Methods

    private void Awake()
    {
        if (creationManager == null)
            Debug.LogWarning("Scenario Manager has no Creation Manager reference.");

        if (generateSpecificScenario)
            SpecificScenario();

        if (usePulleyScenarios)
            AddScenariosToPossible(pulleyScenarios);

        if (useSeesawScenarios)
            AddScenariosToPossible(seesawScenarios);

        if (useSwingingPlatformScenarios)
            AddScenariosToPossible(swingingPlatformScenarios);

        CreateScenarios();
    }

    /*
     * Directly determine a single scenario type to generate.
     */
    private void SpecificScenario()
    {
        switch (puzzleType)
        {
            case PuzzleType.Seesaw:
                Debug.Log("Needs seesaw types");
                break;
            case PuzzleType.Pulley:
                //generatePulleyScenario.Type = pulleyType;
                break;
            case PuzzleType.SwingPlatform:
                Debug.Log("Needs swing platform types");
                break;
            default:
                Debug.LogWarning("This puzzle type does not exist!");
                break;
        }
    }

    /*
     * Randomly select a type of scenario to generate from all of the options given in the Editor.
     */
    private void CreateScenarios()
    {
        for (int i = 0; i < numberOfScenariosToGenerate; i++)
        {
            int randomScenarioIndex = Random.Range(0, possibleScenarios.Count);

            // TODO: Use method consistent between all scenarios to generate
            // TODO: Pass position with spacing factor to scenario to give it 
            // a central location away from other scenarios.
            // Use randomly selected scenario index
        }
    }

    private void AddScenariosToPossible(ScenarioManager[] scenariosToAdd)
    {
        foreach (ScenarioManager scenario in scenariosToAdd)
        {
            possibleScenarios.Add(scenario);
        }
    }

    #endregion
}