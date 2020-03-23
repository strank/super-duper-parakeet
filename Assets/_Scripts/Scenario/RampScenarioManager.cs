using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public abstract class RampScenarioManager : ScenarioManager {

    #region Variables
    [Header("Parameter Ranges")]
    [Header("Ramp Dimensions")]
    public float minLength = 3.0f;
    public float maxLength = 5.0f;
    public float minWidth = 2.0f;
    public float maxWidth = 10.0f;
    public float minHeight = 2.0f;
    public float maxHeight = 8.0f;
    private float height;

    [Header("Other")]
    public Vector3 minStart = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 maxStart = new Vector3(5.0f, 5.0f, 5.0f);
    [SerializeField] private GameObject[] prefabListObstacles;

    #endregion

    #region Unity Methods

    protected void CreateRamp()
    {
        CreateRamp ramp = creationManager.GetComponent<CreateRamp>();

        ReplaceParametersWithDMWidth(ref minStart.z, ref maxStart.z);
        Debug.Log("minWidth is now: " + minWidth + " and maxWidth is now: " + maxWidth);
        Debug.Log("minStart is: " + minStart + " and maxStart is: " + maxStart);
        ConstrainPlacementBasedOnScenarioLength();

        ramp.BuildRamp(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, 
            scenarioParent, rng);

        // Needed to pass on to height range for obstacle
        height = ramp.Height;
        Debug.Log("Height from ramp height is: " + height);
    }

    protected void CreateObstacle()
    {
        CreateObstacleTerrain obstacle = creationManager.GetComponent<CreateObstacleTerrain>();

        ReplaceParametersWithDMWidth(ref minStart.z, ref maxStart.z);
        // Use height instead of maxHeight so obstacle is never greater than ramp size
        obstacle.BuildObstacle(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, height,
            prefabListObstacles, scenarioParent, rng);
    }

    protected void CreateScenarioWall()
    {
        //CreateObstacleTerrain obstacle = creationManager.GetComponent<CreateObstacleTerrain>();
        CreateObstacleTerrain obstacle = new CreateObstacleTerrain();

        obstacle.BuildObstacle(minStart, maxStart, minLength, maxLength, scenarioWidth, scenarioWidth, height, height + 1.0f,
            prefabListObstacles, scenarioParent, rng);

        Vector3 placement = new Vector3(effectiveScenarioLength - obstacle.Length, 0.0f, obstacle.Width / 2);
        obstacle.GeneratedObstacle.transform.localPosition = placement;
    }

    private void ConstrainPlacementBasedOnScenarioLength()
    {
        minStart.x = -effectiveScenarioLength;
        maxStart.x = effectiveScenarioLength;
    }

    #endregion

}
