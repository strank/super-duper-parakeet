using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public class CreateRope : MonoBehaviour {

    #region Variables
    public GameObject ropePrefab;
    private Rope rope;

    public GameObject startPrefab;
    public GameObject endPrefab;

    private GameObject startObject;
    private GameObject endObject;
    private GameObject generatedRope;

    public Transform ropePosition;
    public Transform ropeStart;
    public Transform ropeEnd;

    public float offsetX = 1.0f;

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

    private void Start()
    {

        CreateObjects();

        SetRopeParameters();
    }

    private void CreateObjects()
    {
        generatedRope = (GameObject)Instantiate(ropePrefab, ropePosition.position, Quaternion.identity);
        rope = generatedRope.GetComponent<PulleySetup>().rope;


        Vector3 startObjectPosition = ropeStart.position - new Vector3(offsetX, 0.0f, 0.0f);
        Vector3 endObjectPosition = ropeEnd.position + new Vector3(offsetX, 0.0f, 0.0f);

        startObject = (GameObject)Instantiate(startPrefab, startObjectPosition, Quaternion.identity, generatedRope.transform);
        endObject = (GameObject)Instantiate(endPrefab, endObjectPosition, Quaternion.identity, generatedRope.transform);
    }

    private void SetRopeParameters()
    {
        rope.handles[0] = ropeStart;
        rope.handles[1] = ropeEnd;

        rope.startBody = startObject.GetComponent<Rigidbody>();
        rope.endBody = endObject.GetComponent<Rigidbody>();

        rope.gameObject.SetActive(true);
    }


    #endregion
}
