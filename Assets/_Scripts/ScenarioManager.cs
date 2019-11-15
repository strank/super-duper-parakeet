using System.Collections;
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

[RequireComponent(typeof(GeneratePulleyScenario))]
public class ScenarioManager : MonoBehaviour {

    #region Variables
    

    public PuzzleType puzzleType;
    public PulleyType pulleyType;

    public GameObject creationManager;
    public GeneratePulleyScenario generatePulleyScenario;

    private static ScenarioManager instance;

    private ScenarioManager()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static ScenarioManager Instance
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

    #endregion

    #region Methods

    #region Initialization

    private void Awake()
    {
        if(creationManager == null)
        {
            Debug.LogWarning("Scenario Manager has no Creation Manager reference.");
        }
        DetermineScenarioGenerator();
    }

    #endregion

    private void DetermineScenarioGenerator()
    {
        switch (puzzleType)
        {
            case PuzzleType.Seesaw:
                Debug.Log("Needs seesaw types");
                break;
            case PuzzleType.Pulley:
                generatePulleyScenario.Type = pulleyType;
                break;
            case PuzzleType.SwingPlatform:
                Debug.Log("Needs swing platform types");
                break;
            default:
                Debug.LogWarning("This puzzle type does not exist!");
                break;
        }
    }


    #endregion
}
