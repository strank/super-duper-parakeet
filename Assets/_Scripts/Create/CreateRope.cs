﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public class CreateRope : MonoBehaviour {

    #region Variables
    private Vector3 startPosition;
    private Vector3 minStart;
    private Vector3 maxStart;

    private float minLength;
    private float maxLength;

    private GameObject[] startPulleyMassOptions;
    private GameObject[] endPulleyMassOptions;

    // Determined within this Create class
    private float length;
    private float ropeAngle;
    private Vector3 ropeDirection;
    private Vector3 endPosition;

    // Always set for this Create class
    public GameObject pulleyPrefab;

    // Important Rope variables
    /*
     * 2x Handle Positions
     * Start Body
     * End Body
     */

    //Less Important Rope variables
    /*
     * Number of Handles
     * Rigid Segments
     * Segment Mass
     * Length Multiplier
     */

    #endregion

    #region Unity Methods
    
    /*
     * Public method to allow outside classes to use this class to build a rope
     */
    public void BuildRope(Vector3 _minStart, Vector3 _maxStart, float _minLength, float _maxLength, 
        GameObject[] _startPulleyMassOptions, GameObject[] _endPulleyMassOptions)
    {
        minStart = _minStart;
        maxStart = _maxStart;
        minLength = _minLength;
        maxLength = _maxLength;
        startPulleyMassOptions = _startPulleyMassOptions;
        endPulleyMassOptions = _endPulleyMassOptions;

        RandomizeStartPosition();
        DetermineLength();
        PositionEndPoint();
        PlaceObjectsOnRopeEnds();
    }

    /*
     * Sets start position to a random vector3 within the min and max bounds.
     */
    private void RandomizeStartPosition()
    {
        startPosition = RandomGeneration.RandomVector3WithinBounds(minStart, maxStart);
        Debug.Log("Randomzied pulley start position is: " + startPosition);
    }

    /*
     * Determines a random length value based on the parameter bounds
     */
    private void DetermineLength()
    {
        length = Random.Range(minLength, maxLength);
        Debug.Log("Length of pulley rope is: " + length);
    }

    /*
     * Randomly positions the end point based on where the start point has been positioned 
     * and the length that should be between the start and end point.
     * Since rope will need to fall anyway, end point is just positioned within the same xz plane as 
     * the start point for simplicity.
     */
    private void PositionEndPoint()
    {
        // Chooses a random direction, then scales this by the determined length 
        // and adds it to the starting position
        ropeAngle = Random.Range(0.0f, 360.0f);
        // Converts from degrees to radians
        ropeAngle *= Mathf.Deg2Rad;

        ropeDirection = new Vector3(
            Mathf.Cos(ropeAngle),
            0.0f,
            Mathf.Sin(ropeAngle));

        endPosition = startPosition + ropeDirection * length;
        Debug.Log("Randomized pulley end position is: " + endPosition);
    }

    /*
     * Instantiates the base prefab used for the pulley system, as well as the objects determined to be the 
     * ones at the end(s) of the pulley system.
     * Also connects these instantiated objects to the rope of the base prefab to solidify the entire 
     * pulley system.
     */
    private void PlaceObjectsOnRopeEnds()
    {
        Vector3 TESTPOSITION = new Vector3(0f, 10f, 0f);
        Vector3 OFFSET = new Vector3(0.5f, 0f, 0f);
        GameObject firstObject = null;
        GameObject secondObject = null;

        GameObject quickPulley = (GameObject)Instantiate(pulleyPrefab, TESTPOSITION, Quaternion.identity);
        PulleySetup pulleySetup = quickPulley.GetComponent<PulleySetup>();
        Rope pulleyRope = pulleySetup.rope;

        // Moves the internal pulley prefab empty gameobject transforms to the proper position.
        // Needed to hold useful positional data as well as provide transform data for rope handles
        pulleySetup.ropeStart.transform.position = startPosition;
        pulleySetup.ropeEnd.transform.position = endPosition;
        Debug.Log("Rope start transform position moved to : " + startPosition);
        Debug.Log("Rope end transform position moved to : " + endPosition);

        // Instantiate the objects which make up the pulley end masses
        Vector3 firstObjectPosition = startPosition - OFFSET;
        Vector3 secondObjectPosition = endPosition + OFFSET;
        if (startPulleyMassOptions.Length > 0)
        {
            GameObject startPulleyMass = startPulleyMassOptions[Random.Range(0, startPulleyMassOptions.Length)];
            firstObject = (GameObject)Instantiate(startPulleyMass, firstObjectPosition, Quaternion.identity, quickPulley.transform);
        }
        if (endPulleyMassOptions.Length > 0)
        {
            GameObject endPulleyMass = endPulleyMassOptions[Random.Range(0, endPulleyMassOptions.Length)];
            secondObject = (GameObject)Instantiate(endPulleyMass, secondObjectPosition, Quaternion.identity, quickPulley.transform);
        }

        SetRopeParameters(pulleyRope, pulleySetup.ropeStart.transform, pulleySetup.ropeEnd.transform, firstObject, secondObject);
    }

    /*
     * Effectively connects the startObject and the endObject to the rope object that is 
     * passed in all into a single rope system.
     */
    private void SetRopeParameters(Rope rope, Transform ropeStart, Transform ropeEnd,
        GameObject startObject, GameObject endObject)
    {
        rope.handles[0] = ropeStart;
        rope.handles[1] = ropeEnd;

        if (startObject != null)
            rope.startBody = startObject.GetComponentInChildren<Rigidbody>();

        if (endObject != null)
            rope.endBody = endObject.GetComponentInChildren<Rigidbody>();

        rope.gameObject.SetActive(true);
    }

    //private void Start()
    //{

    //    CreateObjects();

    //    SetRopeParameters();
    //}

    //private void CreateObjects()
    //{
    //    generatedRope = (GameObject)Instantiate(ropePrefab, ropePosition.position, Quaternion.identity);
    //    rope = generatedRope.GetComponent<PulleySetup>().rope;


    //    Vector3 startObjectPosition = ropeStart.position - new Vector3(offsetX, 0.0f, 0.0f);
    //    Vector3 endObjectPosition = ropeEnd.position + new Vector3(offsetX, 0.0f, 0.0f);

    //    startObject = (GameObject)Instantiate(startPrefab, startObjectPosition, Quaternion.identity, generatedRope.transform);
    //    endObject = (GameObject)Instantiate(endPrefab, endObjectPosition, Quaternion.identity, generatedRope.transform);
    //}

    //private void SetRopeParameters()
    //{
    //    rope.handles[0] = ropeStart;
    //    rope.handles[1] = ropeEnd;

    //    rope.startBody = startObject.GetComponent<Rigidbody>();
    //    rope.endBody = endObject.GetComponent<Rigidbody>();

    //    rope.gameObject.SetActive(true);
    //}
    #endregion
}
