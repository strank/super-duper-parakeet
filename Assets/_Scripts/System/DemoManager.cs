using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public enum PuzzleType
{
    Lever,
    Pulley,
    Ramp
}

public enum PulleyType
{
    Hook,
    Platform,
    HeavyDoor
}

public enum RampType
{
    Basic, 
    Build
}

public enum LeverType
{
    One, 
    Two, 
    Three
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

    [Header("For Specific Scenario Type Generation")]
    public PuzzleType puzzleType;
    public PulleyType pulleyType;
    public RampType rampType;
    public LeverType leverType;

    [Header("For Varied Scenario Type Generation")]
    [SerializeField] private ScenarioManager[] pulleyScenarios;
    [SerializeField] private ScenarioManager[] seesawScenarios;
    [SerializeField] private ScenarioManager[] swingingPlatformScenarios;
    [SerializeField] private bool usePulleyScenarios;
    [SerializeField] private bool useSeesawScenarios;
    [SerializeField] private bool useSwingingPlatformScenarios;
    private List<ScenarioManager> possibleScenarios;

    [Header("General Parameters")]
    [SerializeField] private int numberOfScenariosToGenerate = 1;
    [SerializeField] private float spacingBetweenScenarios = 10.0f;
    [SerializeField] private float scenarioWidth = 20.0f; // Space on the z-axis to use for scenarios

    [Header("In Editor")]
    [SerializeField] private bool drawCubeScenarioVolume = false;

    public string Seed
    {
        get { return seed; }
    }
    [SerializeField] private string seed;

    public System.Random PsuedoRandom
    {
        get { return psuedoRandom; }
    }
    private System.Random psuedoRandom;


    // Manager References
    public GameObject creationManager;
    public GameObject scenarioManager;

    #endregion


    #region Unity Methods
    private void Awake()
    {
        psuedoRandom = new System.Random(seed.GetHashCode());
    }

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
            case PuzzleType.Lever:
                switch (leverType)
                {
                    case LeverType.One:
                        BuildSpecificScenario<LeverOneScenario>();
                        break;
                    case LeverType.Two:
                        Debug.Log("LeverType Two not implemented yet.");
                        break;
                    case LeverType.Three:
                        Debug.Log("LeverType Three not implemented yet.");
                        break;
                    default:
                        Debug.Log("Incorrect lever type was selected in DemoManager.");
                        break;
                }
                break;
            case PuzzleType.Pulley:
                switch (pulleyType)
                {
                    case PulleyType.Hook:
                        BuildSpecificScenario<PulleyHookScenario>();
                        break;
                    case PulleyType.HeavyDoor:
                        BuildSpecificScenario<PulleyHeavyDoorScenario>();
                        break;
                    case PulleyType.Platform:
                        BuildSpecificScenario<PulleyPlatformScenario>();
                        break;
                    default:
                        Debug.Log("Incorrect pulley type was selected in DemoManager.");
                        break;
                }
                break;
            case PuzzleType.Ramp:
                switch (rampType)
                {
                    case RampType.Basic:
                        BuildSpecificScenario<RampBasicScenario>();
                        break;
                    case RampType.Build:
                        Debug.Log("RampBuild type not implemented yet.");
                        break;
                    default:
                        Debug.Log("Incorrect ramp type was selected in DemoManager.");
                        break;
                }
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
            GameObject scenarioParent = new GameObject();
            scenarioParent.name = "ScenarioSpecific" + i;
            scenarioParent.transform.position = new Vector3(spacingBetweenScenarios * i, 0, 0);

            ScenarioManager currentScenario = scenarioManager.GetComponent<T>();
            currentScenario.SetScenarioParent(scenarioParent);
            currentScenario.SetScenarioWidth(scenarioWidth);
            currentScenario.SetScenarioLength(spacingBetweenScenarios);
            currentScenario.SetRNG(psuedoRandom);
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

            // TODO:
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

    /*
     * For use in Unity editor
     * Draws a cube around the given scenario space per scenario to help give sense of volume encompassed 
     * and allowed for each scenario.
     */
    private void OnDrawGizmos()
    {
        if (drawCubeScenarioVolume)
        {
            for (int i = 0; i < numberOfScenariosToGenerate; i++)
            {
                Gizmos.color = Color.yellow;
                Vector3 cubeDimensions = new Vector3(spacingBetweenScenarios, 30.0f, scenarioWidth);
                Vector3 cubeCenter = new Vector3(i * spacingBetweenScenarios, 15.0f, 0.0f);
                Gizmos.DrawWireCube(cubeCenter, cubeDimensions);
            }
        }
    }

    #endregion
}
