using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public class CreatePlatforms : MonoBehaviour {

    #region Variables
    public GameObject movingPlatformPrefab;
    //public GameObject objectToAlter;
    private SignalMathMul signalMathMul;
    private LinearJoint linearJoint;

    [Header("Instantiation Factors")]
    public int numberObjectsToGenerate = 3;
    public float distanceApart = 2.0f;

    [Header("Signal Math Mul Parameters")]
    public float minInput2Param = 0.1f;
    public float maxInput2Param = 1.0f;

    [Header("Axis Parameters")]
    public Vector3 minRotationAngle = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 maxRotationAngle = new Vector3(180.0f, 180.0f, 180.0f);
    private GameObject axisChild;

    [Header("Linear Joint Parameters")]
    [Header("maxValue Range")]
    public float minMaxValueParam = 0.0f;
    public float maxMaxValueParam = 15.0f;

    [Header("maxSpeed Range")]
    public float minMaxSpeedParam = 0.0f;
    public float maxMaxSpeedParam = 35.0f;

    [Header("maxAcceleration Range")]
    public float minMaxAccelerationParam = 0.0f;
    public float maxMaxAccelerationParam = 50.0f;

    [Header("Check to Run")]
    public bool isRunning = false;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        if(isRunning == true)
        {
            LoopInstantiation();
            //GOInstantiatePlatform();
            //AlterObject(objectToAlter);
        }

    }

    private void InstantiateMovingPlatformVertical(Vector3 relativePosition)
    {
        GameObject newPlatform = (GameObject)Instantiate(movingPlatformPrefab, relativePosition, Quaternion.identity);

        newPlatform.GetComponent<SignalMathMul>().in2.initialValue = Random.Range(minInput2Param, maxInput2Param);

        axisChild = newPlatform.transform.GetChild(0).gameObject;
        axisChild.transform.localEulerAngles = new Vector3(
            Random.Range(minRotationAngle.x, maxRotationAngle.x),
            Random.Range(minRotationAngle.y, maxRotationAngle.y),
            Random.Range(minRotationAngle.z, maxRotationAngle.z));

        newPlatform.GetComponentInChildren<LinearJoint>().maxValue = Random.Range(minMaxValueParam, maxMaxValueParam);
        newPlatform.GetComponentInChildren<LinearJoint>().maxSpeed = Random.Range(minMaxSpeedParam, maxMaxSpeedParam);
        newPlatform.GetComponentInChildren<LinearJoint>().maxAcceleration = Random.Range(minMaxAccelerationParam, maxMaxAccelerationParam);

        Debug.Log("PLATFORM CREATED!");
    }

    private void InstantiateMovingPlatformVertical_EditPrefab(Vector3 relativePosition)
    {
        //GameObject movingPlatformPrefab = (GameObject)Instantiate(this.movingPlatformPrefab, relativePosition, Quaternion.identity);

        movingPlatformPrefab.GetComponent<SignalMathMul>().in2.initialValue = Random.Range(minInput2Param, maxInput2Param);

        axisChild = movingPlatformPrefab.transform.GetChild(0).gameObject;
        axisChild.transform.localEulerAngles = new Vector3(
            Random.Range(minRotationAngle.x, maxRotationAngle.x),
            Random.Range(minRotationAngle.y, maxRotationAngle.y),
            Random.Range(minRotationAngle.z, maxRotationAngle.z));

        movingPlatformPrefab.GetComponentInChildren<LinearJoint>().maxValue = Random.Range(minMaxValueParam, maxMaxValueParam);
        movingPlatformPrefab.GetComponentInChildren<LinearJoint>().maxSpeed = Random.Range(minMaxSpeedParam, maxMaxSpeedParam);
        movingPlatformPrefab.GetComponentInChildren<LinearJoint>().maxAcceleration = Random.Range(minMaxAccelerationParam, maxMaxAccelerationParam);

        Instantiate(movingPlatformPrefab, relativePosition, Quaternion.identity);
        Debug.Log("PLATFORM CREATED!");
    }

    private void AlterObject(GameObject alterObject)
    {
        GameObject textChild = alterObject.transform.GetChild(0).gameObject;
        textChild.transform.localEulerAngles = new Vector3(12.2f, 35.5f, 12.5f);
    }

    private void LoopInstantiation()
    {
        Vector3 separationPosition = new Vector3();

        for(int i = 0; i < numberObjectsToGenerate; i++)
        {
            separationPosition = new Vector3(i * distanceApart, 0, 0) + transform.position;

            //InstantiateMovingPlatformVertical(separationPosition);
            InstantiateMovingPlatformVertical_EditPrefab(separationPosition);
        }
    }

    #endregion
}
