using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLedge : MonoBehaviour {

    #region Variables
    [Header("Object to Use as Ledge")]
    public GameObject ledge;

    [Header("Parameter Ranges")]
    [Header("Height Range")]
    public float minHeight = 0.0f;
    public float maxHeight = 2.0f;

    [Header("Area Range")]
    public float minX = 0.0f;
    public float maxX = 10.0f;
    public float minZ = 0.0f;
    public float maxZ = 10.0f;

    Vector3[] ledgePosition;

    private int numberOfTests;
    private float separationDistance;
    private static CreateLedge instance;
    public GenerationSystem generationSystem;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one CreateLedge in scene");
            return;
        }
        instance = this;

        // ERROR: Doing this in Awake does not give time in GenerationSystem to actually set
        // instance in time for other classes to GetInstance.

        //generationSystem = GenerationSystem.GetInstance();
        GetDataForMultipleTests();

        ledgePosition = new Vector3[numberOfTests];

        PositionLedge();
    }


    private void Start()
    {
        GenerateLedges();
    }


    /*
     * Gets necessary information from GenerationSystem in cases where multiple test scenarios are being created
     */
    private void GetDataForMultipleTests()
    {
        numberOfTests = generationSystem.GetNumberOfTests();
        separationDistance = generationSystem.GetTestSeparationDistance();
    }


    /*
     * Creates a number of ledges designated by the number of tests, and positions them each randomly.
     * For cases with multiple tests, the general spawning areas are separated some distance, depending 
     * on the number of tests.
     */
    private void GenerateLedges()
    {
        for (int i = 0; i < numberOfTests; i++)
        {
            // First add distance to ledge position depending on the separation distance and how many tests have been created
            Vector3 adjustedLedgePostion = ledgePosition[i] + separationDistance * (float)i * new Vector3(0.0f, 0.0f, 1.0f);
            GameObject newLedge = (GameObject)Instantiate(ledge, adjustedLedgePostion, Quaternion.identity);
        }
    }

    /*
     * Randomly determines the position the ledge, then passes the necessary information to the 
     * generation system.
     */
    private void PositionLedge()
    {
        for(int i = 0; i < numberOfTests; i++)
        {
            ledgePosition[i] = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minHeight, maxHeight),
            Random.Range(minZ, maxZ));

            // Allows for positioning of entire instantiation system by moving parent game object
            ledgePosition[i] += transform.position;

            // Passes height value to generation system
            generationSystem.SetLedgeInformation(ledgePosition[i].y, i);

            Debug.Log("SYSTEM FACTOR: Ledge positioned at height: " + ledgePosition[i].y);
        }
    }

    public static CreateLedge GetInstance()
    {
        return instance;
    }

    #endregion
}
