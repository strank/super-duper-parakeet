using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanAPI;

public class CreateConveyors : MonoBehaviour {

    #region Variables
    [Header("Object to Instantiate")]
    public GameObject conveyorPrefab;

    [Header("Instantiation Factors")]
    public int numberObjectsToGenerate = 3;
    public float distanceApart = 2.0f;

    [Header("Meshes")]
    public Mesh coreMesh;
    public Mesh segmentMesh;
    public int numberOfSegments = 1;

    [Header("Dimensions")]
    public float lengthParameter = 5.0f;
    public float radiusParameter = 5.0f;

    [Header("Other")]
    public float speedParameter = 5.0f;

    [Header("Check to Run")]
    public bool isRunning = false;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        if (isRunning == true)
        {
            //LoopInstantiation();
            InstantiateConveyor(transform.position);
            //AlterObject(objectToAlter);
        }

    }

    private void InstantiateConveyor(Vector3 relativePosition)
    {
        //GameObject newConveyorObj = (GameObject)Instantiate(conveyorPrefab, relativePosition, Quaternion.identity);

        //Conveyor conveyor = newConveyorObj.GetComponentInChildren<Conveyor>();
        Conveyor conveyor = conveyorPrefab.GetComponentInChildren<Conveyor>();
        Debug.Log("What is conveyor: " + conveyor.name);
        conveyor.segmentCount = numberOfSegments;
        conveyor.length = lengthParameter;
        conveyor.radius = radiusParameter;
        conveyor.speed = speedParameter;

        Instantiate(conveyorPrefab, relativePosition, transform.rotation);
        //newPlatform.GetComponent<SignalMathMul>().in2.initialValue = Random.Range(minInput2Param, maxInput2Param);

        //axisChild = newPlatform.transform.GetChild(0).gameObject;
        //axisChild.transform.localEulerAngles = new Vector3(
        //    Random.Range(minRotationAngle.x, maxRotationAngle.x),
        //    Random.Range(minRotationAngle.y, maxRotationAngle.y),
        //    Random.Range(minRotationAngle.z, maxRotationAngle.z));

        //newPlatform.GetComponentInChildren<LinearJoint>().maxValue = Random.Range(minMaxValueParam, maxMaxValueParam);
        //newPlatform.GetComponentInChildren<LinearJoint>().maxSpeed = Random.Range(minMaxSpeedParam, maxMaxSpeedParam);
        //newPlatform.GetComponentInChildren<LinearJoint>().maxAcceleration = Random.Range(minMaxAccelerationParam, maxMaxAccelerationParam);

        Debug.Log("CONVEYOR CREATED!");
    }

    //private void AlterObject(GameObject alterObject)
    //{
    //    GameObject textChild = alterObject.transform.GetChild(0).gameObject;
    //    textChild.transform.localEulerAngles = new Vector3(12.2f, 35.5f, 12.5f);
    //}

    private void LoopInstantiation()
    {
        Vector3 separationPosition = new Vector3();

        for (int i = 0; i < numberObjectsToGenerate; i++)
        {
            separationPosition = new Vector3(i * distanceApart, 0, 0) + transform.position;

            InstantiateConveyor(separationPosition);
        }
    }

    #endregion
}
