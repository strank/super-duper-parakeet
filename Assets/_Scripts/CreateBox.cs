using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBox : MonoBehaviour{

    #region Variables

    [Header("Object to Use as Box")]
    public GameObject box;

    [Header("Parameter Ranges")]
    [Header("Box Size")]
    public float minScale = 0.5f;
    public float maxScale = 4.0f;
    public float MAX_POSSIBLE_BOX_SIZE = 3.0f;

    [Header("Area Range")]
    public float minX = 0.0f;
    public float maxX = 10.0f;
    public float minY = 0.0f;
    public float maxY = 2.0f;
    public float minZ = 0.0f;
    public float maxZ = 10.0f;

    Vector3 boxPosition = new Vector3();

    private static CreateBox instance;
    public GenerationSystem generationSystem;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one CreateBox in scene");
            return;
        }
        instance = this;

        //generationSystem = GenerationSystem.GetInstance();
        //Debug.Log("CreateBox got generation system reference as this: " + generationSystem);

        PositionBox();
    }

    private void Start()
    {
        GameObject createdBox = (GameObject)Instantiate(box, boxPosition, Quaternion.identity);

        // Check and reset scale values if they are deemed impossible to use by the system
        minScale = CheckMin(minScale);
        maxScale = CheckMax(maxScale);
        ScaleBox(createdBox);
    }


    /*
     * Positions an object within specified bounds in 3D space
     */
    private void PositionBox()
    {
        boxPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ));
    }


    /*
     * Uniformly scales an object on all 3 axes
     */
    private void ScaleBox(GameObject newBox)
    {
        // Setting a single float randomly and then applying that to all the scaling dimensions
        // of the instantiated object ensures it is uniformly scaled
        float scalingFactor = Random.Range(minScale, maxScale);
        newBox.transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);

        Debug.Log("Box scale is: " + newBox.transform.localScale);
    }

    private float CheckMin(float min)
    {
        float check = generationSystem.GetLedgeInformation() - min;

        // Only update min value if it is out of the necessary bounds
        if(check > generationSystem.GetDistFromLedgeToBox())
        {
            min = generationSystem.GetLedgeInformation() - generationSystem.GetDistFromLedgeToBox();
            Debug.Log("Box scale failed CheckMin");
        }

        return min;
    }


    private float CheckMax(float max)
    {
        // Only update max value if it is out of the necessary bounds
        if(max > MAX_POSSIBLE_BOX_SIZE)
        {
            max = MAX_POSSIBLE_BOX_SIZE;
            Debug.Log("Box scale failed CheckMax");
        }

        return max;
    }

    public static CreateBox GetInstance()
    {
        return instance;
    }

    #endregion
}
