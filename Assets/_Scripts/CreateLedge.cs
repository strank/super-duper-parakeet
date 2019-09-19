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

    Vector3 ledgePosition = new Vector3();

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
        //Debug.Log("CreateLedge got generation system reference as this: " + generationSystem);

        PositionLedge();
    }

    private void Start()
    {
        GameObject newLedge = (GameObject)Instantiate(ledge, ledgePosition, Quaternion.identity);
    }

    /*
     * Randomly determines the position the ledge, then passes the necessary information to the 
     * generation system.
     */
    private void PositionLedge()
    {
        ledgePosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minHeight, maxHeight),
            Random.Range(minZ, maxZ));

        // Passes height value to generation system
        generationSystem.SetLedgeInformation(ledgePosition.y);
    }

    public static CreateLedge GetInstance()
    {
        return instance;
    }

    #endregion
}
