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
                new DemoManager();
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

    public GameObject ScenarioParent
    {
        get { return scenarioParent; }
    }
    private GameObject scenarioParent;


    // Manager References
    public GameObject creationManager;
    public GameObject scenarioManager;

    #endregion


    #region Unity Methods

    private void Start()
    {
        if (creationManager == null)
        {
            Debug.LogWarning("Demo Manager has no Creation Manager reference.");
        }

        if (generateSpecificScenario)
            DetermineSpecificScenario();
        else
        {
            if (usePulleyScenarios)
                AddScenariosToPossible(pulleyScenarios);

            if (useSeesawScenarios)
                AddScenariosToPossible(seesawScenarios);

            if (useSwingingPlatformScenarios)
                AddScenariosToPossible(swingingPlatformScenarios);

            CreateScenarios();
        }

    }

    /*
     * Directly determine a single scenario type to generate.
     */
    private void DetermineSpecificScenario()
    {
        switch (puzzleType)
        {
            case PuzzleType.Seesaw:
                Debug.Log("Needs seesaw types");
                break;
            case PuzzleType.Pulley:
                switch (pulleyType)
                {
                    case PulleyType.Hook:
                        Debug.Log("Not in yet");
                        break;
                    case PulleyType.HeavyDoor:
                        //scenarioManager.GetComponent<HeavyDoorPulleyScenario>().BuildScenario();
                        BuildSpecificScenario<HeavyDoorPulleyScenario>();
                        break;
                    case PulleyType.Platform:
                        Debug.Log("Not in yet");
                        break;
                }
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
     * Helps perform steps for building any specific scenario
     */
    private void BuildSpecificScenario<T>() where T : ScenarioManager
    {
        for (int i = 0; i < numberOfScenariosToGenerate; i++)
        {
            scenarioParent = new GameObject();
            scenarioParent.name = "ScenarioSpecific" + i;
            scenarioParent.transform.position = new Vector3(spacingBetweenScenarios * i, 0, 0);

            ScenarioManager currentScenario = scenarioManager.GetComponent<T>();
            currentScenario.GetScenarioParent(scenarioParent);
            currentScenario.BuildScenario();
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
            ScenarioManager selectedScenario = possibleScenarios[randomScenarioIndex];
            selectedScenario.centralLocation = new Vector3(i * spacingBetweenScenarios, 0.0f, 0.0f);
            selectedScenario.BuildScenario();

            // TODO: Use method consistent between all scenarios to generate
            // TODO: Pass position with spacing factor to scenario to give it 
            // a central location away from other scenarios.
            // Use randomly selected scenario index
        }
    }

    private void AddScenariosToPossible(ScenarioManager[] scenariosToAdd)
    {
        //DebugPossibleScenariosArray(scenariosToAdd);
        //foreach (ScenarioManager scenario in scenariosToAdd)
        if (scenariosToAdd.Length != 0)
        {
            foreach (ScenarioManager scenario in scenariosToAdd)
            {
                possibleScenarios.Add(scenario);
            }
        }
        else
            Debug.Log("scenariosToAdd is empty.");
        
    }

    #endregion
}
