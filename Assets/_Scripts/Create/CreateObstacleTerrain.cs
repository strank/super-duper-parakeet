using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObstacleTerrain : Create {

    #region Variables
    private Vector3 minStart;
    private Vector3 maxStart;

    private float minLength;
    private float maxLength;
    private float minWidth;
    private float maxWidth;
    private float minHeight;
    private float maxHeight;

    // Determined within this Create class
    private Vector3 startPosition;
    public Vector3 StartPosition
    {
        get { return startPosition; }
    }

    private float length;
    public float Length
    {
        get { return length; }
    }

    private float width;
    public float Width
    {
        get { return width; }
    }

    private float height;
    public float Height
    {
        get { return height; }
    }

    // Always set for this Create class
    public GameObject obstaclePrefab;
    private System.Random seedValue;

    #endregion

    #region Unity Methods

    /*
     * Core method for allowing other classes to use this one to construct the proper seesaw for the situation
     */
    public void BuildObstacle(Vector3 _minStart, Vector3 _maxStart, float _minLength, float _maxLength, float _minWidth, float _maxWidth,
        float _minHeight, float _maxHeight, GameObject parent, System.Random _seedValue)
    {
        // Assign passed in set of parameters within this class to use for ranged generation
        minStart = _minStart;
        maxStart = _maxStart;
        minLength = _minLength;
        maxLength = _maxLength;
        minWidth = _minWidth;
        maxWidth = _maxWidth;
        minHeight = _minHeight;
        maxHeight = _maxHeight;

        // Core values to set for controlling the child/parent hierarchy of the scenario and the randomization
        // with the seeding
        scenarioParent = parent.transform;
        seedValue = _seedValue;

        // Methods to instantiate a varied type of object using passed in parameters
        GenerateObstacle();
    }

    /*
     * Responsible for the heavy lifting of creating the altered seesaw object using the base prefab along with the given 
     * parameter ranges
     */
    public void GenerateObstacle()
    {
        // Instantiates a new gameObject instance of the seesawPrefab
        GameObject generatedObstacle = (GameObject)Instantiate(obstaclePrefab);

        // Creates a scaling vector for all the dimensions of the seesaw and applies it to its localScale
        Vector3 obstacleScalingFactor = new Vector3(
            RandomGeneration.CalculateRandomFloatRange(minLength, maxLength, seedValue),
            RandomGeneration.CalculateRandomFloatRange(minHeight, maxHeight, seedValue),
            RandomGeneration.CalculateRandomFloatRange(minWidth, maxWidth, seedValue));
        generatedObstacle.transform.localScale = obstacleScalingFactor;

        // Positions seesaw within its scenario and sets it as child of overall scenario gameObject
        generatedObstacle.transform.position = RandomGeneration.RandomVector3WithinBounds(minStart, maxStart, seedValue);
        generatedObstacle.transform.SetParent(scenarioParent, false);
    }

    #endregion
}
