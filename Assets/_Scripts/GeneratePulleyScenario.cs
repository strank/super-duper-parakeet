using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePulleyScenario : MonoBehaviour {

    #region Variables

    private ScenarioManager scenarioManager;
    public GameObject creationManager;
    private CreatePulley createPulley;

    private PulleyType type;
    public PulleyType Type {
        set { type = value; }
    }

    [Header("Pulley End Physics Objects")]
    public GameObject[] hooks; // Hook type objects that could latch onto other objects
    public GameObject[] platforms; // Objects that could be used as a platform a player could stand on 
    public GameObject[] massive; // Very large and heavy objects that require a lot of force to move
    public GameObject[] genericMasses; // Assortment of objects that are mostly defined by their mass value

    [Header("Wheels")]
    public GameObject[] wheels; // Objects to use as wheels of pulley setups
    #endregion

    #region Methods

    #region Initialization

    private void Awake()
    {
        //scenarioManager = ScenarioManager.Instance;
        scenarioManager = FindObjectOfType<ScenarioManager>();
        //creationManager = scenarioManager.creationManager;
        createPulley = creationManager.GetComponent<CreatePulley>();

        // Nothing will work as intended if any of these references are not properly obtained.
        if (scenarioManager == null || creationManager == null || createPulley == null)
        {
            Debug.LogWarning(name + " did not obtain a necessary reference at awake.");
            Debug.Log("ScenarioManager is: " + scenarioManager +
                "CreationManager is:" + creationManager +
                "CreatePulley is:" + createPulley);
        }

        InformCreateClasses();

    }

    #endregion

    private void InformCreateClasses()
    {
        switch (type)
        {
            case PulleyType.Hook:
                CreateHookScenario();
                break;
            case PulleyType.Platform:
                CreatePlatformScenario();
                break;
            case PulleyType.HeavyDoor:
                CreateMassiveScenario();
                break;
            default:
                break;
        }
    }

    private void CreateHookScenario()
    {
        createPulley.SetEndObjectOptions(hooks, genericMasses);
        createPulley.SetPulleyWheelOptions(wheels);

        createPulley.CreateVariedRope();
    }

    private void CreatePlatformScenario()
    {
        createPulley.SetEndObjectOptions(platforms, genericMasses);
        createPulley.SetPulleyWheelOptions(wheels);

        createPulley.CreateVariedRope();
    }

    private void CreateMassiveScenario()
    {
        createPulley.SetEndObjectOptions(massive, genericMasses);
        createPulley.SetPulleyWheelOptions(wheels);

        createPulley.CreateVariedRope();
    }

    #endregion
}
