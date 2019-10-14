using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSystem : MonoBehaviour {

    #region Variables
    public int numberOfTests = 1;
    public float testSeparationDistance = 10.0f; // Distance between separate instances of puzzle scenarios

    private float[] ledgeHeight = new float[10]; // Setting array size here until a better solution is found to actually feed in numberOfTests value
    [SerializeField] private float MAX_DIST_LEDGE_TO_BOX = 1.0f; // Would like as constant, but cannot set constants in Unity inspector

    static GenerationSystem instance;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one GenerationSystem in scene");
            return;
        }
        instance = this;

        //ledgeHeight = new float[numberOfTests];
    }


    /*
     * Inform other classes how many objects to instantiate to satisfy the desired number of tests
     */
    public int GetNumberOfTests()
    {
        return numberOfTests;
    }


    /*
     * Inform other classes how far apart seperate scenarios should be created
     */
    public float GetTestSeparationDistance()
    {
        return testSeparationDistance;
    }


    /*
     * Receives randomly selected height value of the ledge to reevaluate and update the possible scale range 
     * for a created box. This is to ensure that the box will be of sufficient size to reach the ledge.
     */
    public void SetLedgeInformation(float height, int index)
    {
        //ledgeHeight = height;
        ledgeHeight[index] = height;

        //Debug.Log("SYSTEM FACTOR: GenerationSystem received ledge height of: " + ledgeHeight);
    }

    public float GetLedgeInformation(int index)
    {
        return ledgeHeight[index];
    }

    public float GetDistFromLedgeToBox()
    {
        return MAX_DIST_LEDGE_TO_BOX;
    }

    public static GenerationSystem GetInstance()
    {
        return instance;
    }

    #endregion
}
