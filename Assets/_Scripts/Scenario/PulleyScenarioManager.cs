using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public abstract class PulleyScenarioManager : ScenarioManager
{
    // Will contain any information that all Pulley Scenarios will use

    #region Variables
    //[Header("What to Generate")]
    //public bool isGeneratingRope = false;
    //public bool isGeneratingWheels = false;

    [Header("Objects to Use as Pulley Ends")]
    [SerializeField]private GameObject[] startPulleyMassOptions;
    [SerializeField]private GameObject[] endPulleyMassOptions;

    [Header("Parameter Ranges")]
    [Header("Rope")]
    [Header("Length")]
    public float minLength = 2.0f;
    public float maxLength = 12.0f;
    private float length;
    private Vector3 ropeDirection;
    private float ropeAngle;

    [Header("Start Point")]
    public Vector3 minStart = Vector3.zero;
    public Vector3 maxStart = new Vector3(20.0f, 20.0f, 20.0f);
    private Vector3 startPosition;

    // End will be determined by system to fit the proper length
    private Vector3 endPosition;

    public GameObject[] pulleyWheelOptions;
    [Header("Wheels")]
    [Header("Quantity")]
    public int minWheels = 1;
    public int maxWheels = 3;
    private int numberOfWheels;

    [Header("Other")]
    public float minSeparationBetweenWheels = 2.0f;
    public float distanceFromEnds = 2.0f;

    public bool useDefaultDistanceBelowRopeForWheels = true;
    public float distanceBelowRopeToSpawnWheels = 2.0f;
    private Vector3 wheelPosition;
    private Vector3[] wheelPositions;
    private float minDistanceBelowRope = 3.0f;
    private Vector3 wheelRangeStartPoint;
    private Vector3 wheelRangeEndPoint;
    #endregion

    #region Unity Methods

    /*
     * Conglomerates all the methods required to generate a rope so that it is easy to call a simple method 
     * from another object to generate the full rope.
     */
    protected void CreateVariedRope()
    {
        CreateRope rope = creationManager.GetComponent<CreateRope>();

        rope.BuildRope(minStart, maxStart, minLength, maxLength, startPulleyMassOptions, endPulleyMassOptions, scenarioParent);

        // Allows CreateRope to calculate these first, and then bring the values back here to use in CreatePulleyWheelSetup
        ropeAngle = rope.RopeAngle;
        ropeDirection = rope.RopeDirection;
        length = rope.Length;
        endPosition = rope.EndPosition;
        startPosition = rope.StartPosition;
    }

    /*
     * Conglomerates all the methods required to properly setup wheels of pulley system with variance
     */
    protected void CreatePulleyWheelSetup()
    {
        creationManager.GetComponent<CreatePulleyWheel>().BuildPulleyWheel(startPosition, endPosition, 
            useDefaultDistanceBelowRopeForWheels, distanceBelowRopeToSpawnWheels, minSeparationBetweenWheels, 
            ropeAngle, ropeDirection, distanceFromEnds, minWheels, maxWheels, pulleyWheelOptions, scenarioParent);
    }

    #endregion
}
