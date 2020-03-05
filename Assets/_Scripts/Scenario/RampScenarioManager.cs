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

        ramp.BuildRamp(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, 
            scenarioParent, rng);

        // Needed to pass on to height range for obstacle
        height = ramp.Height;
    }

    protected void CreateObstacle()
    {
        CreateObstacleTerrain obstacle = creationManager.GetComponent<CreateObstacleTerrain>();

        // Use height instead of maxHeight so obstacle is never greater than ramp size
        obstacle.BuildObstacle(minStart, maxStart, minLength, maxLength, minWidth, maxWidth, minHeight, height,
            scenarioParent, rng);
    }

    #endregion

}
