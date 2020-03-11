using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public abstract class RampScenarioManager : ScenarioManager {

    #region Variables
    [Header("Parameter Ranges")]
    [Header("Ramp Dimensions")]
    public float minLength = 2.0f;
    public float maxLength = 10.0f;
    public float minWidth = 2.0f;
    public float maxWidth = 10.0f;
    public float minHeight = 2.0f;
    public float maxHeight = 8.0f;
    private float height;

    [Header("Other")]
    public Vector3 minStart = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 maxStart = new Vector3(5.0f, 5.0f, 5.0f);

    #endregion

    #region Unity Methods

    protected void CreateRamp()
    {
        CreateRamp ramp = creationManager.GetComponent<CreateRamp>();

        ReplaceParametersWithDMWidth(ref minStart.z, ref maxStart.z);
        Debug.Log("minWidth is now: " + minWidth + " and maxWidth is now: " + maxWidth);
        Debug.Log("minStart is: " + minStart + " and maxStart is: " + maxStart);
        ramp.BuildRamp(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, 
            scenarioParent, rng);

        // Needed to pass on to height range for obstacle
        height = ramp.Height;
    }

    protected void CreateObstacle()
    {
        CreateObstacleTerrain obstacle = creationManager.GetComponent<CreateObstacleTerrain>();

        ReplaceParametersWithDMWidth(ref minStart.z, ref maxStart.z);
        // Use height instead of maxHeight so obstacle is never greater than ramp size
        obstacle.BuildObstacle(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, height,
            scenarioParent, rng);
    }

    protected void CreateScenarioWall()
    {
        CreateObstacleTerrain obstacle = creationManager.GetComponent<CreateObstacleTerrain>();

        ReplaceParametersWithDMWidth(ref minWidth, ref maxWidth);

        obstacle.BuildObstacle(minStart, maxStart, minLength, maxLength, scenarioWidth, scenarioWidth, minHeight, height,
            scenarioParent, rng);
    }

    #endregion

}
